using System;
using System.Collections.Generic;
using Core;
using System.Text;
using Microsoft.Xna.Framework;
//<author:cody>
//grammer system to generate trees
namespace UU_GameProject
{
    public struct RuleResult
    {
        public string result;
        public uint weight;

        public RuleResult(string result, uint weight)
        {
            this.result = result;
            this.weight = weight;
        }
    }

    public class LSystem
    {
        private readonly string start;
        private Dictionary<char, List<RuleResult>> rules;

        public LSystem(string start)
        {
            this.start = start;
            rules = new Dictionary<char, List<RuleResult>>();
        }

        public void AddRule(char input, string output, uint weight = 1)
        {
            if (rules.ContainsKey(input))
                rules[input].Add(new RuleResult(output, weight));
            else
            {
                List<RuleResult> l = new List<RuleResult>();
                l.Add(new RuleResult(output, weight));
                rules.Add(input, l);
            }
        }

        private string Choose(char token)
        {
            if (!rules.ContainsKey(token)) return "";
            uint max = 0;
            List<RuleResult> l = rules[token];
            for (int i = 0; i < l.Count; i++)
                max += l[i].weight;
            float r = (float)(MathH.random.NextDouble() * max);
            uint bar = 0;
            int c = 0;
            for (int i = 0; i < l.Count; i++)
            {
                bar += l[i].weight;
                c = i;
                if (r <= bar) break;
            }
            return l[c].result;
        }

        public string Generate(uint iterations)
        {
            StringBuilder builder = new StringBuilder(start);
            for (int i = 0; i < iterations; i++)
            {
                StringBuilder temp = new StringBuilder();
                for(int j = 0; j < builder.Length; j++)
                {
                    if (!rules.ContainsKey(builder[j]))
                        temp.Append(builder[j]);
                    else temp.Append(Choose(builder[j]));
                }
                builder = temp;
            }
            return builder.ToString();
        }
    }
}
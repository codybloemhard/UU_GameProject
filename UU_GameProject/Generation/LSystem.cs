using System;
using System.Collections.Generic;
using Core;
using System.Text;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class LSystem
    {
        private readonly string start;
        private Dictionary<char, string> rules;

        public LSystem(string start)
        {
            this.start = start;
            rules = new Dictionary<char, string>();
        }

        public void AddRule(char input, string output)
        {
            if (rules.ContainsKey(input)) return;
            rules.Add(input, output);
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
                    else temp.Append(rules[builder[j]]);
                }
                builder = temp;
            }
            return builder.ToString();
        }
    }
}
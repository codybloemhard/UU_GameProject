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
        private Dictionary<char, string> textures;

        public LSystem(string start)
        {
            this.start = start;
            rules = new Dictionary<char, string>();
            textures = new Dictionary<char, string>();
        }

        public void AddRule(char input, string output)
        {
            if (rules.ContainsKey(input)) return;
            rules.Add(input, output);
        }

        public void AddTexture(char token, string texture)
        {
            if (textures.ContainsKey(token)) return;
            textures.Add(token, texture);
        }

        public string Generate(uint iterations)
        {
            StringBuilder builder = new StringBuilder(start);
            for (int i = 0; i < iterations; i++)
            {
                StringBuilder temp = new StringBuilder();
                for(int j = 0; j < builder.Length; j++)
                {
                    if (!rules.ContainsKey(builder[j])) continue;
                    temp.Append(rules[builder[j]]);
                }
                builder = temp;
            }
            return builder.ToString();
        }

        /*public GameObject CreateObject(Vector2 feetPos, string lstring)
        {

        }*/

        public void FromToTranslation(GameObject go, Vector2 p, Vector2 q)
        {
            if (go.Renderer == null) return;
            Vector2 diff = p - q;
            float len = diff.Length();
            float rot = (float)Math.Atan2(diff.Y, diff.X) * MathH.RAD_TO_DEG;
            go.Renderer.SetRotation(rot);
            go.Size = new Vector2(len, 0.2f);
            Vector2 orig = go.Size / 2f;
            go.Pos = p + ((q - p) / 2) - orig;
        }
    }
}
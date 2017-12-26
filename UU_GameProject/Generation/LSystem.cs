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
                    if (builder[j] == '+') temp.Append("+");
                    if (builder[j] == '-') temp.Append("-");
                    if (!rules.ContainsKey(builder[j])) continue;
                    temp.Append(rules[builder[j]]);
                }
                builder = temp;
            }
            return builder.ToString();
        }

        public GameObject CreateObject(GameState context, Vector2 feetPos, string lstring, float startAngle, float changeAngle, Vector2 lineSize)
        {
            Vector2 pos = feetPos;
            float angle = startAngle;
            Vector2 dir = Vector2.Zero;
            SetDir(ref dir, angle);
            float len = lineSize.X;
            float width = lineSize.Y;
            GameObject root = null;
            for (int i = 0; i < lstring.Length; i++)
            {
                if(lstring[i] == '+')
                {
                    angle += changeAngle;
                    SetDir(ref dir, angle);
                    continue;
                }
                if (lstring[i] == '-')
                {
                    angle -= changeAngle;
                    SetDir(ref dir, angle);
                    continue;
                }
                if (!textures.ContainsKey(lstring[i])) continue;
                GameObject go = _obj("_child", context, 0, textures[lstring[i]]);
                Vector2 next = pos + (dir * len);
                FromToTranslation(go, pos, next, width);
                pos = next;
                if (i == 0) root = go;
            }
            return root;
        }

        private void SetDir(ref Vector2 dir, float angle)
        {
            dir = new Vector2((float)Math.Sin(angle * MathH.DEG_TO_RAD), (float)Math.Cos(angle * MathH.DEG_TO_RAD));
        }

        private GameObject _obj(string t, GameState c, uint l, string tex)
        {
            GameObject go = new GameObject(t, c, l);
            go.AddComponent(new CRender(tex));
            return go;
        }

        public void FromToTranslation(GameObject go, Vector2 p, Vector2 q, float width)
        {
            if (go.Renderer == null) return;
            Vector2 diff = p - q;
            float len = diff.Length();
            float rot = (float)Math.Atan2(diff.Y, diff.X) * MathH.RAD_TO_DEG;
            go.Renderer.SetRotation(rot);
            go.Size = new Vector2(len, width);
            Vector2 orig = go.Size / 2f;
            go.Pos = p + ((q - p) / 2f) - orig;
        }
    }
}
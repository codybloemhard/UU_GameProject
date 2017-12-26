using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Core;

namespace UU_GameProject
{
    public class TurtleGraphics
    {
        private Vector2 pos, dir;
        private float angle;
        private GameState context;
        private Vector2 lineSize;
        private Dictionary<char, string> draw;
        private Dictionary<char, float> rotations;
        private Dictionary<char, bool> pushpop;
        private Stack<Vector3> states;

        public TurtleGraphics(GameState context)
        {
            pos = Vector2.Zero;
            dir = Vector2.Zero;
            lineSize = Vector2.Zero;
            angle = 0f;
            this.context = context;
            draw = new Dictionary<char, string>();
            rotations = new Dictionary<char, float>();
            pushpop = new Dictionary<char, bool>();
            states = new Stack<Vector3>();
        }

        public void AddDrawToken(char token, string texture)
        {
            if (draw.ContainsKey(token)) return;
            draw.Add(token, texture);
        }

        public void AddRotationToken(char token, float rotation)
        {
            if (rotations.ContainsKey(token)) return;
            rotations.Add(token, rotation);
        }

        public void AddPushPopToken(char token, bool pushOrPop)
        {
            if (pushpop.ContainsKey(token)) return;
            pushpop.Add(token, pushOrPop);
        }

        public void Init(Vector2 startpos, float startangle, Vector2 lineSize)
        {
            pos = startpos;
            angle = startangle;
            SetDir();
            this.lineSize = lineSize;
        }

        public GameObject CreateObject(string lstring)
        {
            GameObject root = null;
            for (int i = 0; i < lstring.Length; i++)
            {
                char token = lstring[i];
                if (pushpop.ContainsKey(token))
                {
                    if (pushpop[token])
                        states.Push(new Vector3(pos.X, pos.Y, angle));
                    else
                    {
                        if (states.Count == 0) continue;
                        Vector3 res = states.Pop();
                        pos.X = res.X;
                        pos.Y = res.Y;
                        angle = res.Z;
                        SetDir();
                    }
                    continue;
                }
                if (rotations.ContainsKey(token))
                {
                    angle += rotations[token];
                    SetDir();
                    continue;
                }
                if (draw.ContainsKey(token))
                {
                    GameObject go = _obj("_child", context, 0, draw[token]);
                    Vector2 next = pos + (dir * lineSize.X);
                    FromToTranslation(go, pos, next, lineSize.Y);
                    pos = next;
                    if (i == 0) root = go;
                }              
            }
            return root;
        }

        private void SetDir()
        {
            dir = new Vector2((float)Math.Sin(angle * MathH.DEG_TO_RAD), (float)Math.Cos(angle * MathH.DEG_TO_RAD));
        }

        private GameObject _obj(string t, GameState c, uint l, string tex)
        {
            GameObject go = new GameObject(t, c, l);
            go.AddComponent(new CRender(tex));
            return go;
        }

        public static void FromToTranslation(GameObject go, Vector2 p, Vector2 q, float width)
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
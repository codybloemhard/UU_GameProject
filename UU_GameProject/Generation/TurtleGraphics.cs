using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Core;

namespace UU_GameProject
{
    public struct RotationEvent
    {
        public float min, max;

        public RotationEvent(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float GetAngle()
        {
            return Image.Lerp(min, max, (float)MathH.random.NextDouble());
        }
    }

    public struct TurtleState
    {
        public Vector2 pos, size;
        public float angle;
    }

    public class TurtleGraphics
    {
        private Vector2 dir;
        private TurtleState state;
        private GameState context;
        private Dictionary<char, string> draw;
        private Dictionary<char, RotationEvent> rotations;
        private Dictionary<char, bool> pushpop;
        private Dictionary<char, Vector2> resize;
        private Stack<TurtleState> states;

        public TurtleGraphics(GameState context)
        {
            state = new TurtleState();
            this.context = context;
            draw = new Dictionary<char, string>();
            rotations = new Dictionary<char, RotationEvent>();
            pushpop = new Dictionary<char, bool>();
            resize = new Dictionary<char, Vector2>();
            states = new Stack<TurtleState>();
        }

        public void AddDrawToken(char token, string texture)
        {
            if (draw.ContainsKey(token)) return;
            draw.Add(token, texture);
        }

        public void AddRotationToken(char token, float min, float max)
        {
            if (rotations.ContainsKey(token)) return;
            rotations.Add(token, new RotationEvent(min, max));
        }

        public void AddPushPopToken(char token, bool pushOrPop)
        {
            if (pushpop.ContainsKey(token)) return;
            pushpop.Add(token, pushOrPop);
        }

        public void AddResizeToken(char token, Vector2 change)
        {
            if (resize.ContainsKey(token)) return;
            resize.Add(token, change);
        }

        public void Init(Vector2 startpos, float startangle, Vector2 lineSize)
        {
            state.pos = startpos;
            state.angle = startangle;
            SetDir();
            state.size = lineSize;
        }
        
        public GameObject CreateObject(string lstring)
        {
            GameObject root = null;
            for (int i = 0; i < lstring.Length; i++)
            {
                char token = lstring[i];
                if (pushpop.ContainsKey(token))
                {
                    if (pushpop[token]) states.Push(state);
                    else
                    {
                        if (states.Count == 0) continue;
                        state = states.Pop();
                        SetDir();
                    }
                    continue;
                }
                if (rotations.ContainsKey(token))
                {
                    state.angle += rotations[token].GetAngle(); 
                    SetDir();
                    continue;
                }
                if (resize.ContainsKey(token))
                {
                    state.size *= resize[token];
                    continue;
                }
                if (draw.ContainsKey(token))
                {
                    GameObject go = _obj("_child", context, 0, draw[token]);
                    Vector2 next = state.pos + (dir * state.size.X * 0.8f);
                    FromToTranslation(go, state.pos, next, state.size.Y);
                    state.pos = next;
                    if (i == 0) root = go;
                }              
            }
            return root;
        }

        private void SetDir()
        {
            dir = new Vector2((float)Math.Sin(state.angle * MathH.DEG_TO_RAD), (float)Math.Cos(state.angle * MathH.DEG_TO_RAD));
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
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Core;

namespace UU_GameProject
{
    public struct RotationAct
    {
        public float min, max;
        public bool relative;

        public RotationAct(float min, float max, bool relative)
        {
            this.min = min;
            this.max = max;
            this.relative = relative;
        }

        public float GetAngle(float angle)
        {
            float change = Image.Lerp(min, max, (float)MathH.random.NextDouble());
            if (relative) return angle + change;
            else return change;
        }
    }

    public struct ResizeAct
    {
        public Vector2 resize;
        public string sensitives;

        public ResizeAct(Vector2 resize, string sensitives)
        {
            this.resize = resize;
            this.sensitives = sensitives;
        }
    }

    public struct DrawAct
    {
        public List<string> textures;
        public uint layerAdd;
        public float offset;

        public DrawAct(string texture, uint layerAdd, float offset)
        {
            textures = new List<string>();
            textures.Add(texture);
            this.layerAdd = layerAdd;
            this.offset = offset;
        }

        public DrawAct(List<string> textures, uint layerAdd, float offset)
        {
            this.textures = textures;
            this.layerAdd = layerAdd;
            this.offset = offset;
        }

        public string GetTex()
        {
            return textures[(int)(MathH.random.NextDouble() * textures.Count)];
        }
    }

    public struct TurtleState
    {
        public Vector2 pos;
        public float angle;
        public Dictionary<char, Vector2> sizes;
    }

    public class TurtleGraphics
    {
        private Vector2 dir;
        private TurtleState state;
        private GameState context;
        private Vector2 size;
        private Dictionary<char, DrawAct> draw;
        private Dictionary<char, RotationAct> rotations;
        private Dictionary<char, bool> pushpop;
        private Dictionary<char, ResizeAct> resize;
        private Dictionary<char, Vector2> sizes;
        private Stack<TurtleState> states;
        
        public TurtleGraphics(GameState context)
        {
            state = new TurtleState();
            this.context = context;
            draw = new Dictionary<char, DrawAct>();
            rotations = new Dictionary<char, RotationAct>();
            pushpop = new Dictionary<char, bool>();
            resize = new Dictionary<char, ResizeAct>();
            sizes = new Dictionary<char, Vector2>();
            states = new Stack<TurtleState>();
        }

        public void AddDrawToken(char token, string texture, uint layerAdd, Vector2 size, float offset = 0f)
        {
            if (draw.ContainsKey(token)) return;
            draw.Add(token, new DrawAct(texture, layerAdd, offset));
            sizes.Add(token, size);
        }

        public void AddDrawToken(char token, List<string> textures, uint layerAdd, Vector2 size, float offset = 0f)
        {
            if (draw.ContainsKey(token)) return;
            draw.Add(token, new DrawAct(textures, layerAdd, offset));
            sizes.Add(token, size);
        }

        public void AddRotationToken(char token, float min, float max, bool relative = true)
        {
            if (rotations.ContainsKey(token)) return;
            rotations.Add(token, new RotationAct(min, max, relative));
        }

        public void AddPushPopToken(char token, bool pushOrPop)
        {
            if (pushpop.ContainsKey(token)) return;
            pushpop.Add(token, pushOrPop);
        }

        public void AddResizeToken(char token, Vector2 change, string sensitives)
        {
            if (resize.ContainsKey(token)) return;
            resize.Add(token, new ResizeAct(change, sensitives));
        }

        public void Init(Vector2 startpos, float startangle, Vector2 size)
        {
            state.pos = startpos;
            state.angle = startangle;
            SetDir();
            state.sizes = Misc.Copy(sizes);
            this.size = size;
        }
        
        public GameObject CreateObject(string lstring, uint layer, string tag)
        {
            GameObject root = null;
            bool rootDone = false;
            for (int i = 0; i < lstring.Length; i++)
            {
                char token = lstring[i];
                if (pushpop.ContainsKey(token))
                {
                    if (pushpop[token])
                    {
                        TurtleState s = state;
                        s.sizes = Misc.Copy(state.sizes);
                        states.Push(s);
                    }
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
                    state.angle = rotations[token].GetAngle(state.angle); 
                    SetDir();
                    continue;
                }
                if (resize.ContainsKey(token))
                {
                    for(int j = 0; j < resize[token].sensitives.Length; j++)
                    {
                        char elem = resize[token].sensitives[j];
                        if (!draw.ContainsKey(elem)) continue;
                        state.sizes[elem] *= resize[token].resize;
                    }
                }
                if (draw.ContainsKey(token))
                {
                    DrawAct da = draw[token];
                    GameObject go = _obj("_child", context, layer + da.layerAdd, da.GetTex());
                    Vector2 next = state.pos + (dir * state.sizes[token].X * size.X);
                    FromToTranslation(go, state.pos, next, state.sizes[token].Y * size.Y);
                    go.Pos += (dir * state.sizes[token].X * size.X) * da.offset;
                    state.pos = next;
                    if (!rootDone)
                    {
                        root = go;
                        rootDone = true;
                    }
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
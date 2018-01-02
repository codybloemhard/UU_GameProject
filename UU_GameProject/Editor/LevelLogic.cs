using System;
using System.Collections.Generic;
using System.IO;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public struct LvObj
    {
        public Vector2 pos;
        public Vector2 size;
        public string tag;
    }

    public static class LevelLogic
    {
        public const string testurl = "../../../../Content/level.txt";

        public static void WriteLevel(string url)
        {
            using (BinaryWriter w = new BinaryWriter(File.Open(url, FileMode.Open)))
            {
                int count = CLevelEditorObject.objectList.Count;
                w.Write(count);
                for (int i = 0; i < count; i++)
                {
                    GameObject obj = CLevelEditorObject.objectList[i];
                    w.Write(obj.Pos.X);
                    w.Write(obj.Pos.Y);
                    w.Write(obj.Size.X);
                    w.Write(obj.Size.Y);
                    w.Write(obj.tag);
                }
            }
        }

        public static LvObj[] ReadLevel(string url)
        {
            if (!File.Exists(url)) return null;
            List<LvObj> objs = new List<LvObj>();
            BinaryReader r = new BinaryReader(File.Open(url, FileMode.Open));
            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                LvObj o = new LvObj();
                float x, y, w, h;
                x = r.ReadSingle();
                y = r.ReadSingle();
                w = r.ReadSingle();
                h = r.ReadSingle();
                o.tag = r.ReadString();
                o.pos = new Vector2(x, y);
                o.size = new Vector2(w, h);
                objs.Add(o);
            }
            return objs.ToArray();
        }
    }

    public struct Decorator
    {
        public Action<GameObject> action;
        public uint layer;
        public bool isStatic;

        public Decorator(Action<GameObject> action, uint layer, bool isStatic)
        {
            this.action = action;
            this.layer = layer;
            this.isStatic = isStatic;
        }
    }

    public class LevelSolidifier
    {
        private Dictionary<string, Decorator> decorators;
        private GameState context;

        public LevelSolidifier(GameState context)
        {
            this.context = context;
            decorators = new Dictionary<string, Decorator>();
        }

        public void AddSource(string kind, uint layer, bool isStatic, Action<GameObject> action)
        {
            if (decorators.ContainsKey(kind)) return;
            decorators.Add(kind, new Decorator(action, layer, isStatic));
        }

        public void BuildWorld(LvObj[] source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                GameObject go = BuildObj(source[i]);
                if (go == null) continue;
                decorators[source[i].tag].action(go);
            }
        }

        private GameObject BuildObj(LvObj o)
        {
            if (!decorators.ContainsKey(o.tag)) return null;
            Decorator dec = decorators[o.tag];
            GameObject go = new GameObject(context, dec.layer, dec.isStatic);
            go.Pos = o.pos;
            go.Size = o.size;
            return go;
        }
    }
}
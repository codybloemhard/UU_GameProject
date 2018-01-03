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

    public class Chunk
    {
        public LvObj[] source;
        public int x, y;
    }

    public static class LevelLogic
    {
        public const string baseurl = "../../../../Content/Levels/";

        public static void WriteChunk(string file, int x, int y)
        {
            using (BinaryWriter w = new BinaryWriter(File.Open(baseurl + file, FileMode.OpenOrCreate)))
            {
                w.Write(x);
                w.Write(y);
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

        public static Chunk ReadChunk(string file)
        {
            string url = baseurl + file;
            if (!File.Exists(url)) return null;
            Chunk chunk = new Chunk();
            List<LvObj> objs = new List<LvObj>();
            BinaryReader r = new BinaryReader(File.Open(url, FileMode.Open));
            chunk.x = r.ReadInt32();
            chunk.y = r.ReadInt32();
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
            chunk.source = objs.ToArray();
            return chunk;
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

    public class LoadedChunk
    {
        private GameObject[] objects;
        private Vector2 pos;

        public LoadedChunk(Vector2 pos)
        {
            this.pos = pos;
        }

        public void Set(GameObject[] gos)
        {
            objects = gos;
        }

        public void Unload()
        {
            for(int i = 0; i < objects.Length; i++)
                if(objects[i] != null)
                    objects[i].Destroy();
        }
    }

    public class ChunkFactory
    {
        private Dictionary<string, Decorator> decorators;
        private GameState context;
        private Vector2 chunkSize;

        public ChunkFactory(GameState context, Vector2 chunkSize)
        {
            this.context = context;
            this.chunkSize = chunkSize;
            decorators = new Dictionary<string, Decorator>();
        }
        
        public void AddSource(string kind, uint layer, bool isStatic, Action<GameObject> action)
        {
            if (decorators.ContainsKey(kind)) return;
            decorators.Add(kind, new Decorator(action, layer, isStatic));
        }

        public LoadedChunk BuildChunk(Chunk chunk)
        {
            if (chunk == null) return null;
            if (chunk.source == null) return null;
            Vector2 displace = chunkSize * new Vector2(chunk.x, chunk.y);
            LoadedChunk loaded = new LoadedChunk(displace);
            GameObject[] objects = new GameObject[chunk.source.Length];
            for (int i = 0; i < chunk.source.Length; i++)
            {
                GameObject go = BuildObj(chunk.source[i], displace);
                if (go == null) continue;
                decorators[chunk.source[i].tag].action(go);
                objects[i] = go;
            }
            loaded.Set(objects);
            return loaded;
        }

        private GameObject BuildObj(LvObj o, Vector2 displace)
        {
            if (!decorators.ContainsKey(o.tag)) return null;
            Decorator dec = decorators[o.tag];
            GameObject go = new GameObject(context, dec.layer, dec.isStatic);
            go.tag = "";
            go.Pos = o.pos + displace;
            go.Size = o.size;
            return go;
        }
    }

    public class ChunkManager
    {
        private List<Chunk> chunks;
        private List<LoadedChunk> loaded;
        private ChunkFactory factory;

        public ChunkManager()
        {
            factory = null;
            chunks = new List<Chunk>();
            loaded = new List<LoadedChunk>();
        }

        public void Discover(string path)
        {
            string dir = Files.FormatToDir(path);
            Console.WriteLine(dir);
            string[] lvlFiles = Files.AllFilesOfExtension(dir, "lvl");
            Console.WriteLine("Found these chunk files: ");
            Console.WriteLine(lvlFiles.Length);
            Files.PrintFiles(path, lvlFiles);
        }

        public void SetFactory(ChunkFactory factory)
        {
            this.factory = factory;
        }

        public void Update()
        {
            if(factory == null)
            {
                Console.WriteLine("ChunkManager: ChunkFactory not set!");
                return;
            }
            
        }
    }
}
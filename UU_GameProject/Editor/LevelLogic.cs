using System;
using System.Collections.Generic;
using System.IO;
using Core;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
//<author:cody>
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
        public static void WriteChunk(List<GameObject> objects, string file, int x, int y)
        {
            using (BinaryWriter w = new BinaryWriter(File.Open(file, FileMode.OpenOrCreate)))
            {
                w.Write(x);
                w.Write(y);
                int count = objects.Count;
                w.Write(count);
                for (int i = 0; i < count; i++)
                {
                    GameObject obj = objects[i];
                    bool spawner = obj.tag[0] == '!';
                    if (spawner) w.Write(true);
                    else w.Write(false);           
                    if (!spawner)
                    {
                        w.Write(obj.Pos.X);
                        w.Write(obj.Pos.Y);
                        w.Write(obj.Size.X);
                        w.Write(obj.Size.Y);
                    }
                    else
                    {
                        w.Write(obj.Pos.X + 0.5f);
                        w.Write(obj.Pos.Y + 0.5f);
                    }
                    w.Write(obj.tag);
                }
            }
        }

        public static Chunk ReadChunk(string file)
        {
            if (!File.Exists(file)) return null;
            Chunk chunk = new Chunk();
            List<LvObj> objs = new List<LvObj>();
            BinaryReader r = new BinaryReader(File.Open(file, FileMode.Open));
            chunk.x = r.ReadInt32();
            chunk.y = r.ReadInt32();
            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                LvObj o = new LvObj();
                bool spawner;
                float x, y, w = 0f, h = 0f;
                spawner = r.ReadBoolean();
                x = r.ReadSingle();
                y = r.ReadSingle();
                if (!spawner)
                {
                    w = r.ReadSingle();
                    h = r.ReadSingle();
                }
                o.tag = r.ReadString();
                o.pos = new Vector2(x, y);
                o.size = new Vector2(w, h);
                objs.Add(o);
            }
            chunk.source = objs.ToArray();
            return chunk;
        }

        public static void EditChunk(string xy, string baseurl, GameState context)
        {
            Chunk chunk = ReadChunk(baseurl + "chunk" + xy + ".lvl");
            if (chunk == null)
            {
                Console.WriteLine("Chunk does not exist.");
                return;
            }
            context.Unload();
            for (int i = 0; i < chunk.source.Length; i++)
            {
                bool spawner = chunk.source[i].tag[0] == '!';
                GameObject newObj = new GameObject(chunk.source[i].tag, context, 0, true);
                if(spawner) newObj.AddComponent(new CRender("cross"));
                else newObj.AddComponent(new CRender("block"));
                newObj.AddComponent(new CAABB());
                newObj.AddComponent(new CLevelEditorObject(newObj, spawner));
                if (!spawner) newObj.Pos = chunk.source[i].pos;
                else newObj.Pos = chunk.source[i].pos - new Vector2(0.5f);
                if (spawner) newObj.Size = new Vector2(1f, 1f);
                else newObj.Size = chunk.source[i].size;
            }
        }
    }

    public struct Decorator
    {
        public Action<GameObject> decorator;
        public Func<ReplacerInput, GameObject[]> replacer;
        public uint layer;
        public bool isStatic;

        public Decorator(Action<GameObject> decorator, uint layer, bool isStatic)
        {
            this.decorator = decorator;
            this.replacer = null;
            this.layer = layer;
            this.isStatic = isStatic;
        }

        public Decorator(Func<ReplacerInput, GameObject[]> replacer, uint layer, bool isStatic)
        {
            this.decorator = null;
            this.replacer = replacer;
            this.layer = layer;
            this.isStatic = isStatic;
        }
    }

    public struct ReplacerInput
    {
        public uint layer;
        public bool isStatic;
        public LvObj obj;
        public GameState context;

        public ReplacerInput(uint l, bool s, LvObj o, GameState c)
        {
            layer = l;
            isStatic = s;
            obj = o;
            context = c;
        }
    }

    public enum ChunkAction
    {
        STAY,
        LOAD,
        DESTROY,
        NULL
    }

    public class LoadedChunk
    {
        public GameObject[] objects;
        public Vector2 pos;
        public ChunkAction action;

        public LoadedChunk(Vector2 pos)
        {
            this.pos = pos;
            action = ChunkAction.NULL;
        }

        public void Unload()
        {
            if (objects == null) return;
            Task.Run(() =>
            {
                for (int i = 0; i < objects.Length; i++)
                    if (objects[i] != null)
                        objects[i].Destroy();
            });
        }

        public bool IsChunk(int x, int y)
        {
            if (pos.X == x && pos.Y == y) return true;
            return false;
        }
    }

    public class ChunkFactory
    {
        private Dictionary<string, Decorator> decorators;
        private GameState context;
        private Vector2 chunkSize;
        private TaskEngine engine;
        private uint chunkcounter = 0;
        private Dictionary<string, List<GameObject>> lists;
        private Dictionary<string, uint> counters;
        private Dictionary<string, LoadedChunk> chunks;

        public ChunkFactory(GameState context, Vector2 chunkSize)
        {
            this.context = context;
            this.chunkSize = chunkSize;
            decorators = new Dictionary<string, Decorator>();
            engine = new TaskEngine();
            lists = new Dictionary<string, List<GameObject>>();
            counters = new Dictionary<string, uint>();
            chunks = new Dictionary<string, LoadedChunk>();
        }
        
        public void AddSource(string kind, uint layer, bool isStatic, Action<GameObject> action)
        {
            if (decorators.ContainsKey(kind)) return;
            decorators.Add(kind, new Decorator(action, layer, isStatic));
        }

        public void AddSource(string kind, uint layer, bool isStatic, Func<ReplacerInput, GameObject[]> action)
        {
            if (decorators.ContainsKey(kind)) return;
            decorators.Add(kind, new Decorator(action, layer, isStatic));
        }

        public void BuildChunk(Chunk chunk, LoadedChunk lc)
        {
            if (chunk == null) return;
            if (chunk.source == null) return;
            if (lc == null) return;
            Vector2 displace = chunkSize * new Vector2(chunk.x, chunk.y);
            string cstring = chunkcounter.ToString();
            lists.Add(cstring, new List<GameObject>());
            chunks.Add(cstring, lc);
            for (int i = 0; i < chunk.source.Length; i++)
            {
                string key = chunk.source[i].tag;
                if (!decorators.ContainsKey(key)) continue;
                Decorator dec = decorators[key];
                if (dec.decorator != null)
                {
                    GameObject go = BuildObj(chunk.source[i]);
                    go.Pos += displace;
                    dec.decorator(go);
                    lists[cstring].Add(go);
                }
                else
                {
                    ReplacerInput input = new ReplacerInput(dec.layer, dec.isStatic, chunk.source[i], context);
                    engine.Add(delegate() { return Function(input, dec, displace, cstring); }, Callback);
                    if (counters.ContainsKey(cstring))
                        counters[cstring]++;
                    else counters.Add(cstring, 1);
                }
            }
            chunkcounter++;
        }

        private void Callback(Returner<GameObject[]> returner)
        {
            lists[returner.msg].Add(returner.result);
            counters[returner.msg]--;
            if (counters[returner.msg] == 0)
            {
                chunks[returner.msg].objects = lists[returner.msg].ToArray();
                lists[returner.msg].Clear();
                chunks.Remove(returner.msg);
                lists.Remove(returner.msg);
                counters.Remove(returner.msg);
            }
        }

        private Returner<GameObject[]> Function(ReplacerInput input, Decorator dec, Vector2 displace, string msg)
        {
            GameObject[] objs = dec.replacer(input);
            foreach (GameObject o in objs) o.Pos += displace;
            return new Returner<GameObject[]>(objs, msg);
        }

        private GameObject BuildObj(LvObj o)
        {
            Decorator dec = decorators[o.tag];
            GameObject go = new GameObject(context, dec.layer, dec.isStatic);
            go.tag = "";
            go.Pos = o.pos;
            go.Size = o.size;
            return go;
        }

        public Vector2 ChunkSize { get { return chunkSize; } }
    }

    public class ChunkManager
    {
        private Dictionary<Vector2, Chunk> chunks;
        private List<LoadedChunk> loaded, newloaded;
        private ChunkFactory factory;
        private GameObject player;
        private Vector2 middle;

        public ChunkManager()
        {
            factory = null;
            player = null;
            chunks = new Dictionary<Vector2, Chunk>();
            loaded = new List<LoadedChunk>();
            newloaded = new List<LoadedChunk>();
            middle = new Vector2(float.MaxValue, float.MinValue);
        }

        public void Discover(string path, ChunkFactory factory, GameObject player)
        {
            string[] files = Files.AllFilesOfExtension(path, "lvl");
            this.factory = factory;
            this.player = player;
            foreach (string f in files)
            {
                Chunk c = LevelLogic.ReadChunk(f);
                chunks.Add(new Vector2(c.x, c.y), c);
            }
            Console.WriteLine("files found: ");
            Files.PrintFiles(path, files);
        }

        public void Update()
        {
            if(factory == null)
            {
                Console.WriteLine("ChunkManager: ChunkFactory not set!");
                return;
            }
            if(player == null)
            {
                Console.WriteLine("ChunkManager: Player is not set!");
                return;
            }//check if we changed chunk
            Vector2 newmid = player.Pos;
            newmid /= factory.ChunkSize;
            newmid.X = (int)Math.Floor(newmid.X);
            newmid.Y = (int)Math.Floor(newmid.Y);
            if (newmid == middle) return;
            newloaded.Clear();//set new chunks that need to be
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                {
                    int xx = (int)newmid.X + (x - 1);
                    int yy = (int)newmid.Y + (y - 1);
                    LoadedChunk c = new LoadedChunk(new Vector2(xx, yy));
                    newloaded.Add(c);
                }
            SetActions();//see what chunks need what action
            LoadChunks();//create and destroy physical chunks
            middle = newmid;//switch
            loaded = Misc.Copy(newloaded);//switch
        }

        private void SetActions()
        {
            for (int i = 0; i < newloaded.Count; i++)
            {
                int xx = (int)newloaded[i].pos.X;
                int yy = (int)newloaded[i].pos.Y;
                LoadedChunk alreadyLoaded = GetLoadedChunk(xx, yy);
                if (alreadyLoaded != null)
                    newloaded[i].action = ChunkAction.STAY;
                else
                    newloaded[i].action = ChunkAction.LOAD;
            }
            for (int i = 0; i < loaded.Count; i++)
            {
                int x0 = (int)loaded[i].pos.X;
                int y0 = (int)loaded[i].pos.Y;
                bool found = false;
                for (int j = 0; j < newloaded.Count; j++)
                {
                    int x1 = (int)newloaded[j].pos.X;
                    int y1 = (int)newloaded[j].pos.Y;
                    if (x0 == x1 && y0 == y1)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) loaded[i].action = ChunkAction.DESTROY;
            }
        }
        
        private void LoadChunks()
        {
            for(int i = 0; i < loaded.Count; i++)
            {
                if (loaded[i].action != ChunkAction.DESTROY) continue;
                loaded[i].Unload();
            }
            for(int i = 0; i < newloaded.Count; i++)
            {
                if(newloaded[i].action == ChunkAction.LOAD)
                {
                    Vector2 pos = newloaded[i].pos;
                    if (!chunks.ContainsKey(pos)) continue;
                    Chunk chunk = chunks[pos];
                    factory.BuildChunk(chunk, newloaded[i]);
                }
                else if(newloaded[i].action == ChunkAction.STAY)
                {
                    int xx = (int)newloaded[i].pos.X;
                    int yy = (int)newloaded[i].pos.Y;
                    LoadedChunk alreadyLoaded = GetLoadedChunk(xx, yy);
                    newloaded[i] = alreadyLoaded;
                }
            }
        }

        private LoadedChunk GetLoadedChunk(int x, int y)
        {
            for (int i = 0; i < loaded.Count; i++)
                if (loaded[i].IsChunk(x, y)) return loaded[i];
            return null;
        }
    }
}
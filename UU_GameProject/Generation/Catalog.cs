using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;
//<author:cody>
//in this file are all decorators and replacers for the generation
namespace UU_GameProject
{
    public enum BASETILES
    {
        DIRT = 0,
        STONE = 1,
        SAND = 2,
        SANDSTONE = 3
    }

    public enum LAYERTILES
    {
        NONE = 0,
        CRACKS = 1,
        ICE = 2,
        ICETOP = 3
    }

    public enum TOPTILES
    {
        NONE = 0,
        GRASS = 1,
        SNOW = 2
    }
    
    public static class Catalog
    {
        private static Dictionary<string, uint> textures;

        static Catalog()
        {
            textures = new Dictionary<string, uint>();
        }

        public static void Registertexture(string name, uint amount)
        {
            if (textures.ContainsKey(name)) textures[name] = amount;
            else textures.Add(name, amount);
        }

        public static uint TextureAmount(string name)
        {
            if (!textures.ContainsKey(name)) return 0;
            return textures[name];
        }

        public static string RandomTexture(string name, int seed)
        {
            return name + (uint)((Seed.Random(seed)* TextureAmount(name)) % TextureAmount(name));
        }

        public static List<string> GetAllTextures(string name)
        {
            List<string> alltex = new List<string>();
            for (int i = 0; i < TextureAmount(name); i++)
                alltex.Add(name + i);
            return alltex;
        }

        public static GameObject CreateObject(GameState context, uint layer, string tag, string tex, int seed, bool isStatic = false)
        {
            GameObject go = new GameObject(tag, context, layer, isStatic);
            go.AddComponent(new CRender(RandomTexture(tex, seed)));
            return go;
        }

        public static void CreateFromReplacer(GameState context, Vector2 pos, Func<ReplacerInput, GameObject[]>f)
        {
            LvObj obj = new LvObj();
            obj.pos = pos;
            ReplacerInput input = new ReplacerInput(0, true, obj, context);
            f(input);
        }

        private static bool presentID(int[] ids, int[][] must)
        {
            bool[] res = new bool[4];
            for (int i = 0; i < res.Length; i++)
                res[i] = false;
            for (int i = 0; i < ids.Length; i++) {
                int first = i;
                if (i == 2) first = 1;
                else if (first == 3) first = 2;
                for (int j = 0; j < must[first].Length; j++)
                {
                    if (must[first][j] == ids[i])
                    {
                        res[i] = true;
                        break;
                    }
                }
            }
            return res[0] && (res[1] || res[2]) && res[3];
        }
        //boulder, stone, snowystone, frostystone, stoneshard, bush
        //,palm,flower,grassplant,grassdot,grasshigh,snowman
        private static bool[] PossibleObject(int[] ids)
        {
            bool[] possible = new bool[12];
            for (int i = 0; i < possible.Length; i++)
                possible[i] = false;
            possible[00] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 0, 1, 2 }, new int[] { 0, 1 } });
            possible[01] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 0, 1, 2 }, new int[] { 0, 1 } });
            possible[02] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 2, 3 }, new int[] { 0, 2 } });
            possible[03] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 2, 3 }, new int[] { 0, 2 } });
            possible[04] = presentID(ids, new int[][] { new int[] { 2, 3 }, new int[] { 0, 1 }, new int[] { 0 } });
            possible[05] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 0, 1 }, new int[] { 0, 1 } });
            possible[06] = presentID(ids, new int[][] { new int[] { 2, 3 }, new int[] { 0, 1 }, new int[] { 0 } });
            possible[07] = presentID(ids, new int[][] { new int[] { 0 }, new int[] { 0, 1 }, new int[] { 0, 1 } });
            possible[08] = presentID(ids, new int[][] { new int[] { 0 }, new int[] { 0, 1 }, new int[] { 0, 1 } });
            possible[09] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 0, 1 }, new int[] { 0, 1 } });
            possible[10] = presentID(ids, new int[][] { new int[] { 0 }, new int[] { 0, 1 }, new int[] { 1 } });
            possible[11] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 0, 1, 2, 3 }, new int[] { 2 } });
            return possible;
        }

        private static bool[] PossibleTree(int[] ids)
        {
            bool[] possible = new bool[9];
            for (int i = 0; i < possible.Length; i++)
                possible[i] = false;
            possible[00] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 0, 1, 2 }, new int[] { 1 } });
            possible[01] = presentID(ids, new int[][] { new int[] { 0 }, new int[] { 0, 1, 2 }, new int[] { 0, 1 } });
            possible[02] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 0, 1, 2, 3 }, new int[] { 0, 2 } });
            possible[03] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 0, 1, 2 }, new int[] { 1 } });
            possible[04] = presentID(ids, new int[][] { new int[] { 0 }, new int[] { 0, 1, 2 }, new int[] { 0 } });
            possible[05] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 0, 1, 2 }, new int[] { 1 } });
            possible[06] = presentID(ids, new int[][] { new int[] { 0 }, new int[] { 0, 1, 2 }, new int[] { 0, 1 } });
            possible[07] = presentID(ids, new int[][] { new int[] { 0 }, new int[] { 0, 1, 2 }, new int[] { 0, 1 } });
            possible[08] = presentID(ids, new int[][] { new int[] { 0, 1 }, new int[] { 0, 1 }, new int[] { 1 } });
            return possible;
        }

        public static GameObject[] ChooseTree(int n, ReplacerInput newi)
        {
            switch (n)
            {
                case 00: return Catalog.ReplacerTree0(newi);
                case 01: return Catalog.ReplacerTree1(newi);
                case 02: return Catalog.ReplacerTree2(newi);
                case 03: return Catalog.ReplacerTree3(newi);
                case 04: return Catalog.ReplacerTree4(newi);
                case 05: return Catalog.ReplacerTree5(newi);
                case 06: return Catalog.ReplacerTree6(newi);
                case 07: return Catalog.ReplacerTree7(newi);
                case 08: return Catalog.ReplacerTree9(newi);
                default: return null;
            }
        }

        public static GameObject[] ChooseObject(int n, ReplacerInput newi)
        {
            switch (n)
            {
                case 00: return Catalog.ReplacerBoulder(newi);
                case 01: return Catalog.ReplacerStone(newi);
                case 02: return Catalog.ReplacerSnowyStone(newi);
                case 03: return Catalog.ReplacerFrostyStone(newi);
                case 04: return Catalog.ReplacerStoneShard(newi);
                case 05: return Catalog.ReplacerBush(newi);
                case 06: return Catalog.ReplacerTree8(newi);
                case 07: return Catalog.ReplacerFlower(newi);
                case 08: return Catalog.ReplacerGrassPlant(newi);
                case 09: return Catalog.ReplacerGrassDot(newi);
                case 10: return Catalog.ReplacerGrassHigh(newi);
                case 11: return Catalog.ReplacerSnowman(newi);
                
                default: return null;
            }
        }
        
        private static bool IsPlant(int n)
        {
            return n >= 7 && n <= 10;
        }
        
        public static void AddObjectsOnBlock(List<GameObject> list, ReplacerInput i, int[] ids)
        {
            Seed.Set(i.obj.pos);
            ReplacerInput newi = i;
            newi.obj.pos += newi.obj.size * new Vector2(0.5f, 0f);
            newi.obj.size = Vector2.Zero;
            bool[] possible = PossibleObject(ids);
            List<int> pieces = new List<int>();
            List<int> plants = new List<int>();
            for (int j = 0; j < possible.Length; j++)
                if (possible[j])
                {
                    if (IsPlant(j)) plants.Add(j);
                    else pieces.Add(j);
                }
            int nPlant = (int)(Seed.Random(i.obj.pos) * plants.Count);
            int nPieces = (int)(Seed.Random(i.obj.pos) * pieces.Count);
            int chosenPlant = 12;
            int chosenPiece = 12;
            if (plants.Count > 0) chosenPlant = plants[nPlant];
            if (pieces.Count > 0 && Seed.Random(i.obj.pos) < 0.3f)
                chosenPiece = pieces[nPieces];
            else if(Seed.Random(i.obj.pos) < 0.1f)
            {
                newi.layer = 50;
                bool[] possibleTrees = PossibleTree(ids);
                List<int> trees = new List<int>();
                for (int j = 0; j < possibleTrees.Length; j++)
                    if (possibleTrees[j])
                        trees.Add(j);

                int nTree = 0;
                if(trees.Count > 0)
                    nTree = (int)(i.obj.pos.X * 1.345f) % trees.Count;
                int chosenTree = 90;
                if(trees.Count > 0) chosenTree = trees[nTree];
                GameObject[] resTree = ChooseTree(chosenTree, newi);
                if (resTree != null)
                    list.Add(resTree);
            }
            newi.layer = 5;
            GameObject[] resPlant = ChooseObject(chosenPlant, newi);
            if (resPlant != null)
                list.Add(resPlant);
            newi.layer = 20;
            GameObject[] resPieces = ChooseObject(chosenPiece, newi);
            if (resPieces != null)
                list.Add(resPieces);
        }

        public static GameObject[] ReplacerBlock(ReplacerInput i,
            BASETILES baset, LAYERTILES layert0 = LAYERTILES.NONE, LAYERTILES layert1 = LAYERTILES.NONE, TOPTILES topt = TOPTILES.NONE)
        {
            int seed = Seed.GetSeed(i.obj.pos);
            Seed.Set(seed);
            float w = i.obj.size.X;
            float h = i.obj.size.Y;
            int ratio = w > h ? (int)(w / h) : (int)(h / w);
            float leftover = w > h ? (w / h) - ratio : (h / w) - ratio;
            float xinv = w > h ? 1f : 0f;
            float yinv = w > h ? 0f : 1f;
            float size = w > h ? h : w;
            List<GameObject> list = new List<GameObject>();
            for(int j = 0; j < ratio; j++)
            {
                ReplacerInput input = new ReplacerInput();
                input = i;
                input.obj.pos = i.obj.pos + new Vector2(j * size * xinv, j * size * yinv);
                input.obj.size = new Vector2(size);
                GetBlocks(list, input, w > h, seed, baset, layert0, layert1, topt);
            }
            if(leftover > 0f)
            {
                ReplacerInput input = new ReplacerInput();
                input = i;
                input.obj.pos = i.obj.pos + new Vector2(ratio * size * xinv, ratio * size * yinv);
                if (w > h) input.obj.size = new Vector2(leftover, size);
                else input.obj.size = new Vector2(size, leftover);
                GetBlocks(list, input, w > h, seed, baset, layert0, layert1, topt);
            }
            GameObject collider = new GameObject(i.context, 0, i.isStatic);
            collider.Pos = i.obj.pos;
            collider.Size = i.obj.size;
            collider.AddComponent(new CAABB());
            collider.tag = "solid";
            list.Add(collider);
            return list.ToArray();
        }
        
        private static void GetBlocks(List<GameObject> list, ReplacerInput i, bool horizontal, int seed,
            BASETILES baset, LAYERTILES layert0 = LAYERTILES.NONE, LAYERTILES layert1 = LAYERTILES.NONE, TOPTILES topt = TOPTILES.NONE)
        {
            string basetex, layer0tex, layer1tex, toptex;
            switch (baset)
            {
                case BASETILES.DIRT: basetex = "_dirt"; break;
                case BASETILES.STONE: basetex = "_coursestone"; break;
                case BASETILES.SAND: basetex = "_sand"; break;
                case BASETILES.SANDSTONE: basetex = "_sandstone"; break;
                default: basetex = "_dirt"; break;
            }
            switch (layert0)
            {
                case LAYERTILES.CRACKS: layer0tex = "_crackedlayer"; break;
                case LAYERTILES.ICE: layer0tex = "_frostylayer"; break;
                case LAYERTILES.ICETOP: layer0tex = "_frostytop"; break;
                default: layer0tex = ""; break;
            }
            switch (layert1)
            {
                case LAYERTILES.CRACKS: layer1tex = "_crackedlayer"; break;
                case LAYERTILES.ICE: layer1tex = "_frostylayer"; break;
                case LAYERTILES.ICETOP: layer1tex = "_frostytop"; break;
                default: layer1tex = ""; break;
            }
            switch (topt)
            {
                case TOPTILES.GRASS: toptex = "_grasstop"; break;
                case TOPTILES.SNOW: toptex = "_snowytop"; break;
                default: toptex = ""; break;
            }
            int[] ids = new int[4];
            ids[0] = (int)baset;
            ids[1] = (int)layert0;
            ids[2] = (int)layert1;
            ids[3] = (int)topt;
            
            GameObject basego = CreateObject(i.context, i.layer + 3, "solid", basetex, seed, i.isStatic);
            basego.Pos = i.obj.pos;
            basego.Size = i.obj.size;
            list.Add(basego);

            if (layer0tex != "")
            {
                GameObject layergo = CreateObject(i.context, i.layer + 2, "", layer0tex, seed, i.isStatic);
                layergo.Pos = i.obj.pos;
                layergo.Size = i.obj.size;
                list.Add(layergo);
            }
            if (layer1tex != "")
            {
                GameObject layergo = CreateObject(i.context, i.layer + 1, "", layer1tex, seed, i.isStatic);
                layergo.Pos = i.obj.pos;
                layergo.Size = i.obj.size;
                list.Add(layergo);
            }
            if (toptex != "")
            {
                GameObject layergo = CreateObject(i.context, i.layer, "", toptex, seed, i.isStatic);
                layergo.Pos = i.obj.pos;
                layergo.Size = i.obj.size * new Vector2(1f, 0.5f);
                list.Add(layergo);
            }
            if (horizontal)AddObjectsOnBlock(list, i, ids);
        }
        
        public static GameObject[] ReplacerBoulder(ReplacerInput i)
        {
            const string tex = "_boulder";
            Seed.Set(i.obj.pos);
            GameObject go = CreateObject(i.context, i.layer, "boulder", tex, Seed.GetSeed(i.obj.pos), i.isStatic);
            const float sizeMin = 2f, sizeMax = 4f;
            float size = Seed.RandomRange(sizeMin, sizeMax, i.obj.pos);
            go.Size = new Vector2(size);
            go.Pos = i.obj.pos - go.Size * new Vector2(0.5f, 0.8f);
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerStone(ReplacerInput i)
        {
            const string tex = "_stone";
            Seed.Set(i.obj.pos);
            GameObject go = CreateObject(i.context, i.layer, "stone", tex, Seed.GetSeed(i.obj.pos), i.isStatic);
            go.Size = new Vector2(1f, 1f);
            go.Pos = i.obj.pos - go.Size * new Vector2(0.5f, 0.8f);
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerSnowyStone(ReplacerInput i)
        {
            const string tex = "_snowystone";
            Seed.Set(i.obj.pos);
            GameObject go = CreateObject(i.context, i.layer, "stone", tex, Seed.GetSeed(i.obj.pos), i.isStatic);
            go.Size = new Vector2(1f, 1f);
            go.Pos = i.obj.pos - go.Size * new Vector2(0.5f, 0.8f);
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerFrostyStone(ReplacerInput i)
        {
            const string tex = "_frostystone";
            Seed.Set(i.obj.pos);
            GameObject go = CreateObject(i.context, i.layer, "stone", tex, Seed.GetSeed(i.obj.pos), i.isStatic);
            go.Size = new Vector2(1f, 1f);
            go.Pos = i.obj.pos - go.Size * new Vector2(0.5f, 0.8f);
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerStoneShard(ReplacerInput i)
        {
            const string tex = "_stoneshard";
            Seed.Set(i.obj.pos);
            GameObject go = CreateObject(i.context, i.layer, "stone", tex, Seed.GetSeed(i.obj.pos), i.isStatic);
            go.Size = new Vector2(1f, 1f);
            go.Pos = i.obj.pos - go.Size * new Vector2(0.5f, 0.7f);
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerCloud(ReplacerInput i)
        {
            const string tex = "_cloud";
            Seed.Set(i.obj.pos);
            GameObject go = CreateObject(i.context, i.layer, "stone", tex, Seed.GetSeed(i.obj.pos), i.isStatic);
            go.Size = new Vector2(2f, 2f);
            go.Pos = i.obj.pos - go.Size/2f;
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerBush(ReplacerInput i)
        {
            const string tex = "_bush";
            Seed.Set(i.obj.pos);
            GameObject go = CreateObject(i.context, i.layer, "stone", tex, Seed.GetSeed(i.obj.pos), i.isStatic);
            go.Size = new Vector2(1f, 1f);
            go.Pos = i.obj.pos - go.Size * new Vector2(0.5f, 0.8f);
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerTree0(ReplacerInput i)
        {
            LSystem lsys = new LSystem("X");
            lsys.AddRule('X', "F[#-X][#X]F[#-X]+FX", 5);
            lsys.AddRule('F', "F*F");
            Vector2 branchSize = new Vector2(1f, 2f);
            Vector2 leafSize = new Vector2(1f);
            TurtleGraphics turtle = new TurtleGraphics(i.context);
            turtle.AddDrawToken('F', "_woodlight0", 1, branchSize);
            turtle.AddDrawToken('X', GetAllTextures("_blossom"), 0, leafSize);
            turtle.AddRotationToken('-', -25f, -35f);
            turtle.AddRotationToken('+', +25f, 35f);
            turtle.AddPushPopToken('[', true);
            turtle.AddPushPopToken(']', false);
            turtle.AddResizeToken('*', new Vector2(1f, 0.95f), "F");
            turtle.AddResizeToken('#', new Vector2(0.95f, 0.6f), "F");
            turtle.Init(i.obj.pos, 180, new Vector2(0.15f));
            string lstring = lsys.Generate(4);
            return turtle.CreateObject(lstring, i.layer, "_tree");
        }

        public static GameObject[] ReplacerTree1(ReplacerInput i)
        {
            LSystem lsys = new LSystem("X");
            lsys.AddRule('X', "F[+*FX]F[-*FX]X");
            Vector2 branchSize = new Vector2(1f, 1f);
            Vector2 leafSize = new Vector2(5f);
            TurtleGraphics turtle = new TurtleGraphics(i.context);
            turtle.AddDrawToken('F', "_wooddark0", 1, branchSize);
            turtle.AddDrawToken('X', GetAllTextures("_greenleafshalf"), 0, leafSize);
            turtle.AddRotationToken('-', -25f, -35f);
            turtle.AddRotationToken('+', +25f, 35f);
            turtle.AddPushPopToken('[', true);
            turtle.AddPushPopToken(']', false);
            turtle.AddResizeToken('*', new Vector2(0.9f, 0.5f), "F");
            turtle.Init(i.obj.pos, 180, new Vector2(0.6f));
            string lstring = lsys.Generate(4);
            return turtle.CreateObject(lstring, i.layer, "_tree");
        }
        
        public static GameObject[] ReplacerTree2(ReplacerInput i)
        {
            LSystem lsys = new LSystem("X#X#X#X[+*X[+*XY][-*XY]][-*X[+*XY][-*XY]]");
            lsys.AddRule('Y', "#[+*XY][-*XY]", 3);
            lsys.AddRule('Y', "#[+*XY]", 3);
            lsys.AddRule('Y', "#[-*XY]", 3);
            lsys.AddRule('Y', "#Z", 2);
            Vector2 branchSize = new Vector2(1f, 0.5f);
            Vector2 leafSize = new Vector2(1f);
            TurtleGraphics turtle = new TurtleGraphics(i.context);
            turtle.AddDrawToken('X', "_woodburned0", 1, branchSize);
            turtle.AddDrawToken('Y', GetAllTextures("_leafburned"), 0, leafSize);
            turtle.AddDrawToken('Z', GetAllTextures("_leafburned"), 0, leafSize);
            turtle.AddRotationToken('-', -25f, -35f);
            turtle.AddRotationToken('+', +25f, 35f);
            turtle.AddRotationToken('#', +9f, -9f);
            turtle.AddPushPopToken('[', true);
            turtle.AddPushPopToken(']', false);
            turtle.AddResizeToken('*', new Vector2(0.99f, 0.7f), "X");
            turtle.Init(i.obj.pos, 180, new Vector2(0.4f));
            string lstring = lsys.Generate(5);
            return turtle.CreateObject(lstring, i.layer, "_tree");
        }

        public static GameObject[] ReplacerTree3(ReplacerInput i)
        {
            LSystem lsys = new LSystem("X#X#X#XY");
            lsys.AddRule('Y', "#[+*XY][-*XY]", 5);
            lsys.AddRule('Y', "#[+*XY]", 1);
            lsys.AddRule('Y', "#[-*XY]", 1);
            Vector2 branchSize = new Vector2(1f, 0.5f);
            Vector2 leafSize = new Vector2(0.8f);
            TurtleGraphics turtle = new TurtleGraphics(i.context);
            turtle.AddDrawToken('X', "_woodmedium0", 1, branchSize);
            turtle.AddDrawToken('Y', GetAllTextures("_leaf"), 0, leafSize);
            turtle.AddDrawToken('Z', GetAllTextures("_leaf"), 0, leafSize);
            turtle.AddRotationToken('-', -25f, -35f);
            turtle.AddRotationToken('+', +25f, 35f);
            turtle.AddRotationToken('#', +9f, -9f);
            turtle.AddPushPopToken('[', true);
            turtle.AddPushPopToken(']', false);
            turtle.AddResizeToken('*', new Vector2(0.99f, 0.7f), "X");
            turtle.Init(i.obj.pos, 180, new Vector2(0.6f));
            string lstring = lsys.Generate(6);
            return turtle.CreateObject(lstring, i.layer, "_tree");
        }

        public static GameObject[] ReplacerTree4(ReplacerInput i)
        {
            LSystem lsys = new LSystem("X#X#X#XY");
            lsys.AddRule('Y', "#[+*XY][-*XY]", 5);
            lsys.AddRule('Y', "#[-*XY]XX[@Z]Y", 5);
            lsys.AddRule('Y', "#[+*XY]XX[@Z]Y", 5);
            Vector2 branchSize = new Vector2(1f, 0.5f);
            Vector2 leafSize = new Vector2(6f, 0.7f);
            TurtleGraphics turtle = new TurtleGraphics(i.context);
            turtle.AddDrawToken('X', "_woodmedium0", 1, branchSize);
            turtle.AddDrawToken('Z', GetAllTextures("_hangingleaf"), 0, leafSize);
            turtle.AddRotationToken('-', -25f, -35f);
            turtle.AddRotationToken('+', +25f, 35f);
            turtle.AddRotationToken('#', +15f, -15f);
            turtle.AddRotationToken('@', 0f, 0f, false);
            turtle.AddPushPopToken('[', true);
            turtle.AddPushPopToken(']', false);
            turtle.AddResizeToken('*', new Vector2(0.99f, 0.7f), "X");
            turtle.Init(i.obj.pos, 180, new Vector2(0.4f));
            string lstring = lsys.Generate(5);
            return turtle.CreateObject(lstring, i.layer, "_tree");
        }

        public static GameObject[] ReplacerTree5(ReplacerInput i)
        {
            LSystem lsys = new LSystem("X#X#X#X#X#X#X#XY");
            lsys.AddRule('Y', "#[+*XY][-*XY]", 5);
            lsys.AddRule('Y', "#[+*Y][-*Y]", 5);
            Vector2 branchSize = new Vector2(1f, 0.5f);
            Vector2 leafSize = new Vector2(3f);
            TurtleGraphics turtle = new TurtleGraphics(i.context);
            turtle.AddDrawToken('X', "_woodlight0", 1, branchSize);
            turtle.AddDrawToken('Y', GetAllTextures("_leafhigh"), 0, leafSize);
            turtle.AddDrawToken('Z', GetAllTextures("_leafhigh"), 0, leafSize);
            turtle.AddRotationToken('-', -10f, -15f);
            turtle.AddRotationToken('+', +10f, 15f);
            turtle.AddRotationToken('!', -20f, -30f);
            turtle.AddRotationToken('@', +20f, 30f);
            turtle.AddRotationToken('#', +3f, -3f);
            turtle.AddPushPopToken('[', true);
            turtle.AddPushPopToken(']', false);
            turtle.AddResizeToken('*', new Vector2(1.3f, 0.8f), "X");
            turtle.Init(i.obj.pos, 180, new Vector2(0.3f));
            string lstring = lsys.Generate(5);
            return turtle.CreateObject(lstring, i.layer, "_tree");
        }

        public static GameObject[] ReplacerTree6(ReplacerInput i)
        {
            LSystem lsys = new LSystem("X+Y");
            lsys.AddRule('Y', "+XY", 1);
            lsys.AddRule('Y', "[-*XZ]+XY", 10);
            lsys.AddRule('Z', "-XZ", 10);
            lsys.AddRule('Z', "[-*XZ]+XZ", 3);
            Vector2 branchSize = new Vector2(2f, 0.5f);
            Vector2 leafSize = new Vector2(5f);
            TurtleGraphics turtle = new TurtleGraphics(i.context);
            turtle.AddDrawToken('X', "_woodmedium0", 1, branchSize);
            turtle.AddDrawToken('Y', GetAllTextures("_greenleafs"), 0, leafSize, -0.5f);
            turtle.AddDrawToken('Z', GetAllTextures("_greenleafs"), 0, leafSize, -0.5f);
            turtle.AddRotationToken('-', -10f, -15f);
            turtle.AddRotationToken('+', +10f, 15f);
            turtle.AddPushPopToken('[', true);
            turtle.AddPushPopToken(']', false);
            turtle.AddResizeToken('*', new Vector2(0.8f, 0.7f), "X");
            turtle.Init(i.obj.pos, 180, new Vector2(0.5f));
            string lstring = lsys.Generate(8);
            return turtle.CreateObject(lstring, i.layer, "_tree");
        }

        public static GameObject[] ReplacerTree7(ReplacerInput i)
        {
            LSystem lsys = new LSystem("X#Y");
            lsys.AddRule('Y', "X#Y", 1);
            lsys.AddRule('Y', "[*+XZ]X#Y", 2);
            lsys.AddRule('Y', "[*-XZ]X#Y", 2);
            lsys.AddRule('Z', "X#Z", 2);
            lsys.AddRule('Z', "#[*-XZ][*+XZ]", 2);
            Vector2 branchSize = new Vector2(2f, 1f);
            Vector2 leafSize = new Vector2(4f);
            float backSet = -0.5f;
            TurtleGraphics turtle = new TurtleGraphics(i.context);
            turtle.AddDrawToken('X', "_wooddark0", 1, branchSize);
            turtle.AddDrawToken('Y', GetAllTextures("_greenleafsbig"), 0, leafSize, backSet);
            turtle.AddDrawToken('Z', GetAllTextures("_greenleafsbig"), 0, leafSize, backSet);
            turtle.AddRotationToken('-', -20f, -25f);
            turtle.AddRotationToken('+', +20f, 25f);
            turtle.AddPushPopToken('[', true);
            turtle.AddPushPopToken(']', false);
            turtle.AddResizeToken('*', new Vector2(0.8f, 0.75f), "X");
            turtle.AddResizeToken('#', new Vector2(1f, 0.85f), "X");
            turtle.Init(i.obj.pos, 180, new Vector2(0.8f));
            string lstring = lsys.Generate(9);
            return turtle.CreateObject(lstring, i.layer, "_tree");
        }

        public static GameObject[] ReplacerTree8(ReplacerInput i)
        {
            LSystem lsys = new LSystem("X#*X#*X#*X#*X#*X#Y");
            lsys.AddRule('Y', "*X#X#Y", 2);
            lsys.AddRule('Y', "X#W", 1);
            lsys.AddRule('W', "[$A]W", 1);
            lsys.AddRule('A', "C+C+D");
            Vector2 branchSize = new Vector2(1f, 1f);
            Vector2 leafBranch = new Vector2(3f, 3f);
            Vector2 leafSize = new Vector2(1f, 1f);
            TurtleGraphics turtle = new TurtleGraphics(i.context);
            turtle.AddDrawToken('X', "_woodpalm0", 1, branchSize);
            turtle.AddDrawToken('C', GetAllTextures("_palmleafbody"), 0, leafBranch);
            turtle.AddRotationToken('$', +0f, +85f);
            turtle.AddRotationToken('%', -0f, -85f);
            turtle.AddRotationToken('+', +20f, +25f);
            turtle.AddRotationToken('-', -20f, -25f);
            turtle.AddRotationToken('#', -15f, 15f);
            turtle.AddPushPopToken('[', true);
            turtle.AddPushPopToken(']', false);
            turtle.AddResizeToken('*', new Vector2(1f, 0.9f), "X");
            turtle.Init(i.obj.pos, 180, new Vector2(0.2f));
            string lstring = lsys.Generate(15);
            return turtle.CreateObject(lstring, i.layer, "_tree");
        }

        public static GameObject[] ReplacerTree9(ReplacerInput i)
        {
            LSystem lsys = new LSystem("XX-X-X-#Y");
            lsys.AddRule('Y', "*W#Y");
            lsys.AddRule('W', "X", 2);
            lsys.AddRule('W', "[--[-AZ][++AZ]]X", 1);
            Vector2 branchSize = new Vector2(3f, 1f);
            Vector2 subBranchSize = new Vector2(8f, 0.5f);
            Vector2 leafSize = new Vector2(1f, 1f);
            TurtleGraphics turtle = new TurtleGraphics(i.context);
            turtle.AddDrawToken('X', "_woodmedium0", 1, branchSize);
            turtle.AddDrawToken('A', "_woodmedium0", 1, subBranchSize);
            turtle.AddDrawToken('Z', GetAllTextures("_blossom"), 1, leafSize);
            turtle.AddRotationToken('+', +20f, +25f);
            turtle.AddRotationToken('-', -20f, -25f);
            turtle.AddRotationToken('#', 25f, 25f);
            turtle.AddPushPopToken('[', true);
            turtle.AddPushPopToken(']', false);
            turtle.AddResizeToken('*', new Vector2(0.97f, 0.97f), "XA");
            turtle.Init(i.obj.pos, 180-0, new Vector2(0.2f));
            string lstring = lsys.Generate(50);
            return turtle.CreateObject(lstring, i.layer, "_tree");
        }
        
        public static GameObject[] ReplacerFlower(ReplacerInput i)
        {
            const string tex = "_flower";
            Seed.Set(i.obj.pos);
            GameObject go = CreateObject(i.context, i.layer, "", tex, Seed.GetSeed(i.obj.pos), i.isStatic);
            go.Size = new Vector2(0.5f);
            go.Pos = i.obj.pos - (go.Size * new Vector2(0.5f, 1f));
            //go.AddComponent(new CRender(RandomTexture(tex)));
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerGrassPlant(ReplacerInput i)
        {
            const string tex = "_grassplant";
            Seed.Set(i.obj.pos);
            GameObject go = CreateObject(i.context, i.layer, "", tex, Seed.GetSeed(i.obj.pos), i.isStatic);
            go.Size = new Vector2(1f);
            go.Pos = i.obj.pos - (go.Size * new Vector2(0.5f, 1f));
            //go.AddComponent(new CRender(RandomTexture(tex)));
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerGrassDot(ReplacerInput i)
        {
            const string tex = "_grassdot";
            Seed.Set(i.obj.pos);
            GameObject go = CreateObject(i.context, i.layer, "", tex, Seed.GetSeed(i.obj.pos), i.isStatic);
            go.Size = new Vector2(1f);
            go.Pos = i.obj.pos - (go.Size * new Vector2(0.5f, 1f));
            //go.AddComponent(new CRender(RandomTexture(tex)));
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerGrassHigh(ReplacerInput i)
        {
            const string tex = "_grasshigh";
            Seed.Set(i.obj.pos);
            GameObject go = CreateObject(i.context, i.layer, "", tex, Seed.GetSeed(i.obj.pos), i.isStatic);
            go.Size = new Vector2(1f, 2f);
            go.Pos = i.obj.pos - (go.Size * new Vector2(0.5f, 1f));
            //go.AddComponent(new CRender(RandomTexture(tex)));
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerSnowman(ReplacerInput i)
        {
            Seed.Set(i.obj.pos);
            float scale = 1.0f;
            const string texBody = "_snowmanbody", texEye = "_snowmaneye",
                texHat = "_snowmanhat", texMouth = "_snowmanmouth",
                texNose = "_snowmannose", texArmL = "_snowmanarmleft", texArmR = "_snowmanarmright";
            GameObject go = CreateObject(i.context, i.layer + 1, "", texBody, Seed.GetSeed(i.obj.pos));
            go.Size = new Vector2(1.0f, 2.0f) * scale;
            go.Pos = i.obj.pos - (go.Size * new Vector2(0.5f, 1f));
            GameObject eye0 = CreateObject(i.context, i.layer, "__child", texEye, Seed.GetSeed(i.obj.pos));
            GameObject eye1 = CreateObject(i.context, i.layer, "__child", texEye, Seed.GetSeed(i.obj.pos));
            eye0.SetParent(go);
            eye1.SetParent(go);
            eye0.LocalSize = eye1.LocalSize = new Vector2(0.1f, 0.05f);
            eye0.LocalPos = new Vector2(0.35f, 0.25f) * go.Size;
            eye1.LocalPos = new Vector2(0.55f, 0.25f) * go.Size;
            GameObject hat = CreateObject(i.context, i.layer, "__child", texHat, Seed.GetSeed(i.obj.pos));
            hat.SetParent(go);
            hat.LocalSize = new Vector2(Seed.RandomDeviation(0.45f, 0.05f, i.obj.pos), Seed.RandomDeviation(0.4f, 0.05f, i.obj.pos) * 0.5f);
            hat.LocalPos = new Vector2(0.5f, 0.13f) * go.Size - hat.Size / 2f;
            GameObject mouth = CreateObject(i.context, i.layer, "__child", texMouth, Seed.GetSeed(i.obj.pos));
            mouth.SetParent(go);
            mouth.LocalSize = new Vector2(Seed.RandomRange(0.3f, 0.45f, i.obj.pos), Seed.RandomDeviation(0.125f, 0.04f, i.obj.pos));
            mouth.LocalPos = new Vector2(0.5f, 0.33f) * go.Size - mouth.Size/2f;
            GameObject nose = CreateObject(i.context, i.layer, "__child", texNose, Seed.GetSeed(i.obj.pos));
            nose.SetParent(go);
            nose.LocalSize = new Vector2(0.1f, 0.05f);
            nose.LocalPos = new Vector2(0.5f, 0.31f) * go.Size - nose.Size/2f;
            GameObject arm0 = CreateObject(i.context, i.layer, "__child", texArmL, Seed.GetSeed(i.obj.pos));
            arm0.SetParent(go);
            arm0.LocalSize = new Vector2(1f, 0.5f);
            arm0.LocalPos = new Vector2(-0.2f, 0.3f) * go.Size - arm0.Size/2f;
            GameObject arm1 = CreateObject(i.context, i.layer, "__child", texArmR, Seed.GetSeed(i.obj.pos));
            arm1.SetParent(go);
            arm1.LocalSize = new Vector2(1f, 0.5f);
            arm1.LocalPos = new Vector2(1.2f, 0.3f) * go.Size - arm0.Size / 2f;
            return new GameObject[] { go, eye0, eye1, hat, mouth, nose, arm0, arm1};
        }
    }
}
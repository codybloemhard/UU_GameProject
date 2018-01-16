using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;
//<author:cody>
namespace UU_GameProject
{
    public enum BASETILES
    {
        DIRT,
        STONE,
        SAND,
        SANDSTONE
    }

    public enum LAYERTILES
    {
        NONE,
        CRACKS,
        ICE,
        ICETOP
    }

    public enum TOPTILES
    {
        NONE,
        GRASS,
        SNOW
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

        public static string RandomTexture(string name)
        {
            return name + (uint)(MathH.random.Next() % TextureAmount(name));
        }

        public static List<string> GetAllTextures(string name)
        {
            List<string> alltex = new List<string>();
            for (int i = 0; i < TextureAmount(name); i++)
                alltex.Add(name + i);
            return alltex;
        }

        public static GameObject CreateObject(GameState context, uint layer, string tag, string tex, bool isStatic = false)
        {
            GameObject go = new GameObject(tag, context, layer, isStatic);
            go.AddComponent(new CRender(RandomTexture(tex)));
            return go;
        }

        public static void CreateFromReplacer(GameState context, Vector2 pos, Func<ReplacerInput, GameObject[]>f)
        {
            LvObj obj = new LvObj();
            obj.pos = pos;
            ReplacerInput input = new ReplacerInput(0, true, obj, context);
            f(input);
        }
        
        public static GameObject[] ReplacerBlock(ReplacerInput i,
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
            int size = 1;
            if (layer0tex != "") size++;
            if (layer1tex != "") size++;
            if (toptex != "") size++;
            GameObject[] objs = new GameObject[size];
            int count = 1;

            GameObject basego = CreateObject(i.context, i.layer + 3, "", basetex, i.isStatic);
            basego.Pos = i.obj.pos;
            basego.Size = i.obj.size;
            basego.AddComponent(new CAABB());
            objs[0] = basego;

            if (layer0tex != "")
            {
                GameObject layergo = CreateObject(i.context, i.layer + 2, "", layer0tex, i.isStatic);
                layergo.Pos = i.obj.pos;
                layergo.Size = i.obj.size;
                objs[count++] = layergo;
            }
            if (layer1tex != "")
            {
                GameObject layergo = CreateObject(i.context, i.layer + 1, "", layer1tex, i.isStatic);
                layergo.Pos = i.obj.pos;
                layergo.Size = i.obj.size;
                objs[count++] = layergo;
            }
            if (toptex != "")
            {
                GameObject layergo = CreateObject(i.context, i.layer, "", toptex, i.isStatic);
                layergo.Pos = i.obj.pos;
                layergo.Size = i.obj.size * new Vector2(1f, 0.5f);
                objs[count++] = layergo;
            }
            return objs;
        }
        
        public static GameObject CreateBoulder(GameState context, float x, float y, uint layer, string tag)
        {
            const string tex = "_boulder";
            GameObject go = CreateObject(context, layer, tag, tex);
            go.Pos = new Vector2(x, y);
            const float sizeMin = 2f, sizeMax = 4f;
            float size = Image.RandomRange(sizeMin, sizeMax);
            go.Size = new Vector2(2f, 2f);
            return go;
        }

        public static GameObject CreateStone(GameState context, float x, float y, uint layer, string tag)
        {
            const string tex = "_stone";
            GameObject go = CreateObject(context, layer, tag, tex);
            go.Pos = new Vector2(x, y);
            go.Size = new Vector2(1f, 1f);
            return go;
        }

        public static GameObject CreateSnowyStone(GameState context, float x, float y, uint layer, string tag)
        {
            const string tex = "_snowystone";
            GameObject go = CreateObject(context, layer, tag, tex);
            go.Pos = new Vector2(x, y);
            go.Size = new Vector2(1f, 1f);
            return go;
        }

        public static GameObject CreateFrostyStone(GameState context, float x, float y, uint layer, string tag)
        {
            const string tex = "_frostystone";
            GameObject go = CreateObject(context, layer, tag, tex);
            go.Pos = new Vector2(x, y);
            go.Size = new Vector2(1f, 1f);
            return go;
        }

        public static GameObject CreateStoneShard(GameState context, float x, float y, uint layer, string tag)
        {
            const string tex = "_stoneshard";
            GameObject go = CreateObject(context, layer, tag, tex);
            go.Pos = new Vector2(x, y);
            go.Size = new Vector2(1f, 1f);
            return go;
        }

        public static GameObject CreateCloud(GameState context, float x, float y, uint layer, string tag)
        {
            const string tex = "_cloud";
            GameObject go = CreateObject(context, layer, tag, tex);
            go.Pos = new Vector2(x, y);
            go.Size = new Vector2(2f, 2f);
            return go;
        }

        public static GameObject CreateBush(GameState context, float x, float y, uint layer, string tag)
        {
            const string tex = "_bush";
            GameObject go = CreateObject(context, layer, tag, tex);
            go.Pos = new Vector2(x, y);
            go.Size = new Vector2(1f, 1f); 
            return go;
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
            return turtle.CreateObject(lstring, 0, "_tree");
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
            return turtle.CreateObject(lstring, 0, "_tree");
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
            return turtle.CreateObject(lstring, 0, "_tree");
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
            return turtle.CreateObject(lstring, 0, "_tree");
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
            return turtle.CreateObject(lstring, 0, "_tree");
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
            return turtle.CreateObject(lstring, 0, "_tree");
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
            return turtle.CreateObject(lstring, 0, "_tree");
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
            return turtle.CreateObject(lstring, 0, "_tree");
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
            return turtle.CreateObject(lstring, 0, "_tree");
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
            return turtle.CreateObject(lstring, 0, "_tree");
        }
        
        public static GameObject[] ReplacerFlower(ReplacerInput i)
        {
            const string tex = "_flower";
            GameObject go = CreateObject(i.context, i.layer, "", tex, i.isStatic);
            go.Size = new Vector2(0.5f);
            go.Pos = i.obj.pos - (go.Size * new Vector2(0.5f, 1f));
            go.AddComponent(new CRender(RandomTexture(tex)));
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerGrassPlant(ReplacerInput i)
        {
            const string tex = "_grassplant";
            GameObject go = CreateObject(i.context, i.layer, "", tex, i.isStatic);
            go.Size = new Vector2(1f);
            go.Pos = i.obj.pos - (go.Size * new Vector2(0.5f, 1f));
            go.AddComponent(new CRender(RandomTexture(tex)));
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerGrassDot(ReplacerInput i)
        {
            const string tex = "_grassdot";
            GameObject go = CreateObject(i.context, i.layer, "", tex, i.isStatic);
            go.Size = new Vector2(1f);
            go.Pos = i.obj.pos - (go.Size * new Vector2(0.5f, 1f));
            go.AddComponent(new CRender(RandomTexture(tex)));
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerGrassHigh(ReplacerInput i)
        {
            const string tex = "_grasshigh";
            GameObject go = CreateObject(i.context, i.layer, "", tex, i.isStatic);
            go.Size = new Vector2(1f, 2f);
            go.Pos = i.obj.pos - (go.Size * new Vector2(0.5f, 1f));
            go.AddComponent(new CRender(RandomTexture(tex)));
            return new GameObject[] { go };
        }

        public static GameObject[] ReplacerSnowman(ReplacerInput i)
        {
            float scale = 1.0f;
            const string texBody = "_snowmanbody", texEye = "_snowmaneye",
                texHat = "_snowmanhat", texMouth = "_snowmanmouth",
                texNose = "_snowmannose", texArmL = "_snowmanarmleft", texArmR = "_snowmanarmright";
            GameObject go = CreateObject(i.context, i.layer + 1, "", texBody);
            go.Size = new Vector2(1.0f, 2.0f) * scale;
            go.Pos = i.obj.pos - (go.Size * new Vector2(0.5f, 1f));
            GameObject eye0 = CreateObject(i.context, i.layer, "__child", texEye);
            GameObject eye1 = CreateObject(i.context, i.layer, "__child", texEye);
            eye0.SetParent(go);
            eye1.SetParent(go);
            eye0.LocalSize = eye1.LocalSize = new Vector2(0.1f, 0.05f);
            eye0.LocalPos = new Vector2(0.35f, 0.25f) * go.Size;
            eye1.LocalPos = new Vector2(0.55f, 0.25f) * go.Size;
            GameObject hat = CreateObject(i.context, i.layer, "__child", texHat);
            hat.SetParent(go);
            hat.LocalSize = new Vector2(Image.RandomDeviation(0.45f, 0.05f), Image.RandomDeviation(0.4f, 0.05f) * 0.5f);
            hat.LocalPos = new Vector2(0.5f, 0.13f) * go.Size - hat.Size / 2f;
            GameObject mouth = CreateObject(i.context, i.layer, "__child", texMouth);
            mouth.SetParent(go);
            mouth.LocalSize = new Vector2(Image.RandomRange(0.3f, 0.45f), Image.RandomDeviation(0.125f, 0.04f));
            mouth.LocalPos = new Vector2(0.5f, 0.33f) * go.Size - mouth.Size/2f;
            GameObject nose = CreateObject(i.context, i.layer, "__child", texNose);
            nose.SetParent(go);
            nose.LocalSize = new Vector2(0.1f, 0.05f);
            nose.LocalPos = new Vector2(0.5f, 0.31f) * go.Size - nose.Size/2f;
            GameObject arm0 = CreateObject(i.context, i.layer, "__child", texArmL);
            arm0.SetParent(go);
            arm0.LocalSize = new Vector2(1f, 0.5f);
            arm0.LocalPos = new Vector2(-0.2f, 0.3f) * go.Size - arm0.Size/2f;
            GameObject arm1 = CreateObject(i.context, i.layer, "__child", texArmR);
            arm1.SetParent(go);
            arm1.LocalSize = new Vector2(1f, 0.5f);
            arm1.LocalPos = new Vector2(1.2f, 0.3f) * go.Size - arm0.Size / 2f;
            return new GameObject[] { go, eye0, eye1, hat, mouth, nose, arm0, arm1};
        }
    }
}
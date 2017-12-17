﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;
/*
biomes: artic, desert, hills, forest
*/
namespace UU_GameProject
{
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

        public static GameObject CreateObject(GameState context, uint layer, string tag, string tex)
        {
            GameObject go = new GameObject(tag, context, layer);
            go.AddComponent(new CRender(RandomTexture(tex)));
            return go;
        }
        
        public static GameObject CreateBoulder(GameState context, float x, float y, uint layer, string tag)
        {
            const string tex = "_boulder";
            GameObject go = CreateObject(context, layer, tag, tex);
            go.Pos = new Vector2(x, y);
            const float sizeMin = 2f, sizeMax = 4f;
            float size = Image.RandomRange(sizeMin, sizeMax);
            go.Size = new Vector2(size, size);
            return go;
        }

        public static GameObject CreateBush(GameState context, float x, float y, uint layer, string tag)
        {
            const string texLeaf = "_bush";
            GameObject go = CreateObject(context, layer, tag, texLeaf);
            go.Pos = new Vector2(x, y);
            go.Size = new Vector2(1.0f, 1.0f);
            return go;
        }

        public static GameObject CreateSnowman(GameState context, float x, float y, uint layer, string tag, float scale = 1.0f)
        {
            const string texBody = "_snowmanbody", texEye = "_snowmaneye",
                texHat = "_snowmanhat", texMouth = "_snowmanmouth",
                texNose = "_snowmannose", texArmL = "_snowmanarmleft", texArmR = "_snowmanarmright";
            GameObject go = CreateObject(context, layer + 1, tag, texBody);
            go.Pos = new Vector2(x, y);
            go.Size = new Vector2(1.0f, 2.0f) * scale;
            GameObject eye0 = CreateObject(context, layer, "__child", texEye);
            GameObject eye1 = CreateObject(context, layer, "__child", texEye);
            eye0.SetParent(go);
            eye1.SetParent(go);
            eye0.LocalSize = eye1.LocalSize = new Vector2(0.1f, 0.05f);
            eye0.LocalPos = new Vector2(0.35f, 0.25f) * go.Size;
            eye1.LocalPos = new Vector2(0.55f, 0.25f) * go.Size;
            GameObject hat = CreateObject(context, layer, "__child", texHat);
            hat.SetParent(go);
            hat.LocalSize = new Vector2(Image.RandomDeviation(0.45f, 0.05f), Image.RandomDeviation(0.4f, 0.05f) * 0.5f);
            hat.LocalPos = new Vector2(0.5f, 0.13f) * go.Size - hat.Size / 2f;
            GameObject mouth = CreateObject(context, layer, "__child", texMouth);
            mouth.SetParent(go);
            mouth.LocalSize = new Vector2(Image.RandomRange(0.3f, 0.45f), Image.RandomDeviation(0.125f, 0.04f));
            mouth.LocalPos = new Vector2(0.5f, 0.33f) * go.Size - mouth.Size/2f;
            GameObject nose = CreateObject(context, layer, "__child", texNose);
            nose.SetParent(go);
            nose.LocalSize = new Vector2(0.1f, 0.05f);
            nose.LocalPos = new Vector2(0.5f, 0.31f) * go.Size - nose.Size/2f;
            GameObject arm0 = CreateObject(context, layer, "__child", texArmL);
            arm0.SetParent(go);
            arm0.LocalSize = new Vector2(1f, 0.5f);
            arm0.LocalPos = new Vector2(-0.2f, 0.3f) * go.Size - arm0.Size/2f;
            GameObject arm1 = CreateObject(context, layer, "__child", texArmR);
            arm1.SetParent(go);
            arm1.LocalSize = new Vector2(1f, 0.5f);
            arm1.LocalPos = new Vector2(1.2f, 0.3f) * go.Size - arm0.Size / 2f;
            return go;
        }
    }
}
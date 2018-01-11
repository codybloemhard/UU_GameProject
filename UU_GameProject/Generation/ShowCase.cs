using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Core;
//<author:cody>
namespace UU_GameProject
{
    public static class ShowCase
    {
        private static float height = 0f;

        public static void CreateRow(GameState context, string obj, uint amount, uint layer, float hRatio = 1f)
        {
            uint texam = Catalog.TextureAmount(obj);
            if (texam == 0) return;
            float size = 16.0f / amount;
            for (int i = 0; i < amount; i++)
            {
                GameObject go = new GameObject("", context, layer);
                go.AddComponent(new CRender(obj + (i % texam)));
                go.Pos = new Vector2(i * size, height);
                go.Size = new Vector2(size, size * hRatio);
            }
            height += size * hRatio;
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;

namespace UU_GameProject
{
    public class PrerunGenerationCycle
    {
        public PrerunGenerationCycle() { }

        public void GenTest()
        {
            const int size = 128;
            ColourField f = new ColourField(size, size);
            Image.Perlin(f, 4, 10, 0.5, 2.0);
            Image.ToRange(f, 0f, 1f);
            f.Save();
            TextureManager.LoadTexture("test", f.Texture);
        }
    }
}
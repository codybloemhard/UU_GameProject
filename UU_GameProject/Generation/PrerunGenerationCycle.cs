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
            const int size = 32;
            for(int i = 0; i < 4; i++)
            {
                FloatField fPerlin = Image.Perlin(size, size, 4, 5, 0.5, 2.0);
                Image.ToRange(fPerlin, 0.01f, 1.0f);
                FloatField fMask = Image.VoronoiCell(size, size, 10, 0.8f, 0.1f);
                FloatField fCircle = Image.Circle(size, size, 0.9f, 1, 0.01f, 0.6f);
                FloatField fResult = Image.Intersection(fPerlin, fMask);
                Image.Multiply(fResult, fCircle);

                ColourField cf = new ColourField(size, size);
                Colour cDark = new Colour(0.2f, 0.1f, 0.1f) / 4;
                Colour clight = new Colour(0.5f, 0.2f, 0.1f);
                cf.FloatsToColours(fResult, cDark, clight);
                cf.Save();
                TextureManager.LoadTexture("test" + i, cf.Texture);
            }
        }
    }
}
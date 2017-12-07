using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;

namespace UU_GameProject
{
    public static class Image
    {
        public static float[,] Perlin(uint w, uint h, uint octaves, double scale, double persistence, double lacunarity)
        {
            float[,] res = new float[w, h];
            double z = MathH.random.NextDouble();
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    res[x, y] = (float)PerlinNoise.Perlin((double)x / w, (double)y / h, z, octaves, scale, persistence, lacunarity);
            return res;
        }

        public static void Perlin(ColourField f, uint octaves, double scale, double persistence, double lacunarity)
        {
            double z = MathH.random.NextDouble();
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                {
                    float p = (float)PerlinNoise.Perlin((double)x / f.Width, (double)y / f.Height, z, octaves, scale, persistence, lacunarity);
                    Colour c = new Colour(p);
                    f.Set(c, x, y);
                }
        }

        public static void Power(ColourField f, uint i)
        {
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++) {
                    Colour c = f.Get(x, y);
                    for (uint j = 0; j < i - 1; j++) c *= c;
                    f.Set(c, x, y);
                }
        }

        public static float RangeMap(float x, float a, float b, float c, float d)
        {
            float scale = (d - c) / (b - a);
            return c + (x - a) * scale;
        }

        public static Colour RangeMap(Colour x, Colour a, Colour b, Colour c, Colour d)
        {
            Colour scale = (d - c) / (b - a);
            return c + (x - a) * scale;
        }

        public static void Normalize(ColourField f)
        {
            ToRange(f, 0, 1);
        }

        public static void ToRange(ColourField f, float fmin, float fmax)
        {
            float min = f.Min();
            float max = f.Max();
            Colour cmin = new Colour(min);
            Colour cmax = new Colour(max);
            Colour czero = new Colour(fmin);
            Colour cone = new Colour(fmax);
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                {
                    Colour c = f.Get(x, y);
                    f.Set(RangeMap(c, cmin, cmax, czero, cone), x, y);
                }
        }
    }
}
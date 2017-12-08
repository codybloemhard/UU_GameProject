using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;

namespace UU_GameProject
{
    public static class Image
    {
        public static FloatField Perlin(uint w, uint h, uint octaves, double scale, double persistence, double lacunarity)
        {
            FloatField f = new FloatField(w, h);
            double z = MathH.random.NextDouble();
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                {
                    float p = (float)PerlinNoise.Perlin((double)x / f.Width, (double)y / f.Height, z, octaves, scale, persistence, lacunarity);
                    f.array[f.To1D(x, y)] = p;
                }
            return f;
        }

        public static FloatField Cut(uint w, uint h, bool left, Vector2 p, Vector2 q)
        {
            float init = left ? 0 : 1;
            float assign = left ? 1 : 0;
            FloatField f = new FloatField(w, h);
            //scale to field
            p.X *= f.Width;
            p.Y *= f.Height;
            q.X *= f.Width;
            q.Y *= f.Height;
            //to y = mx + b
            float m = (p.Y - q.Y) / (p.X - q.X);
            float b = p.Y - (m * p.X);
            for(int x = 0; x < f.Width; x++)
                for(int y = 0; y < f.Height; y++)
                {
                    float res = init; //left to line, than res = 1
                    if ((y - b) / m > x) res = assign;
                    f.array[f.To1D(x, y)] = res;
                }
            return f;
        }

        public static FloatField VoronoiCell(uint w, uint h, uint p, float size, float diff)
        {
            Vector2[] points = new Vector2[p];
            Vector2 mid = new Vector2(0.5f, 0.5f);
            for (int i = 0; i < p; i++)
            {
                double rad = (double)i / p * 2 * MathH.PI;
                points[i] = new Vector2((float)Math.Sin(rad), (float)Math.Cos(rad)) * size;
                points[i] += mid;
                double diffrad = MathH.random.NextDouble() * 2 * MathH.PI;
                points[i] += new Vector2((float)Math.Sin(diffrad), (float)Math.Cos(diffrad)) * diff;
            }
            FloatField f = new FloatField(w, h);
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                {
                    float min = 10000f;
                    Vector2 pp = new Vector2((float)x / w, (float)y / h);
                    for (int i = 0; i < p; i++)
                    {
                        float dist = (points[i] - pp).LengthSquared();
                        if (dist < min) min = dist;
                    }
                    float middist = (mid - pp).LengthSquared();
                    if (middist < min) f.array[f.To1D(x, y)] = 1;
                    else f.array[f.To1D(x, y)] = 0;
                }
            return f;
        }

        public static FloatField Circle(uint w, uint h, float r, float alphaA, float alphaB, float inner = 0.0f)
        {
            if (r < 0) r = 0;
            if (r > 1) r = 1;
            r *= Math.Min(w / 2, h / 2);
            if (inner < 0) inner = 0;
            if (inner > 1) inner = 1;
            inner *= Math.Min(w / 2, h / 2);
            Vector2 mid = new Vector2(w / 2, h / 2);
            FloatField f = new FloatField(w, h);
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                {
                    float dist = (new Vector2(x, y) - mid).Length();
                    float res = 0;
                    if (dist < r)
                    {
                        if (dist < inner) res = alphaA;
                        else res = Lerp(alphaA, alphaB, (dist - inner) / (r - inner));
                    }
                    f.array[f.To1D(x, y)] = res;
                }
            return f;
        }

        public static FloatField Intersection(FloatField a, FloatField b)
        {
            FloatField res = new FloatField((uint)a.Width, (uint)a.Height);
            for (int x = 0; x < res.Width; x++)
                for (int y = 0; y < res.Height; y++)
                {
                    float aa = a.array[a.To1D(x, y)];
                    float bb = b.array[b.To1D(x % b.Width, y % b.Height)];
                    float r = 0.0f;
                    if (aa > 0.0f && bb > 0.0f)
                        r = aa;
                    res.array[res.To1D(x, y)] = r;
                }
            return res;
        }

        public static FloatField Union(FloatField a, FloatField b)
        {
            FloatField res = new FloatField((uint)a.Width, (uint)a.Height);
            for (int x = 0; x < res.Width; x++)
                for (int y = 0; y < res.Height; y++)
                {
                    float aa = a.array[a.To1D(x, y)];
                    float bb = b.array[b.To1D(x % b.Width, y % b.Height)];
                    float r = aa;
                    if (aa == 0) r = bb;
                    res.array[res.To1D(x, y)] = r;
                }
            return res;
        }

        public static void Multiply(FloatField fa, FloatField fb)
        {
            for(int x = 0; x < fa.Width; x++)
                for (int y = 0; y < fa.Height; y++)
            {
                float aa = fa.array[fa.To1D(x, y)];
                float bb = fb.array[fb.To1D(x % fb.Width, y % fb.Height)];
                float r = aa * bb;
                fa.array[fa.To1D(x, y)] = r;
            }
        }

        public static void ThresholdCut(FloatField f, float min, float max)
        {
            for(int x = 0; x < f.Width; x++)
                for(int y = 0; y < f.Height; y++)
                {
                    float r = f.array[f.To1D(x, y)] = 0;
                    if (r < min) r = 0;
                    if (r > max) r = 0;
                    f.array[f.To1D(x, y)] = r;
                }
        }

        public static void ThresholdFlatten(FloatField f, float min, float max)
        {
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                {
                    float r = f.array[f.To1D(x, y)] = 0;
                    if (r < min) r = min;
                    if (r > max) r = max;
                    f.array[f.To1D(x, y)] = r;
                }
        }

        public static void Power(FloatField f, uint i)
        {
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++) {
                    float c = f.array[f.To1D(x, y)];
                    for (uint j = 0; j < i - 1; j++) c *= c;
                    f.array[f.To1D(x, y)] = c;
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

        public static void ToRange(FloatField f, float fmin, float fmax)
        {
            float min = f.Min();
            float max = f.Max();
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                {
                    float c = f.array[f.To1D(x, y)];
                    f.array[f.To1D(x, y)] = RangeMap(c, min, max, fmin, fmax);
                }
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

        public static void Normalize(FloatField f)
        {
            ToRange(f, 0, 1);
        }

        public static void Normalize(ColourField f)
        {
            ToRange(f, 0, 1);
        }

        public static Colour Lerp(Colour a, Colour b, float t)
        {
            return a + ((b - a) * t);
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + ((b - a) * t);
        }
    }
}
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
                    float p = (float)PerlinNoise.Perlin((double)x / f.Width, (double)y / f.Width, z, octaves, scale, persistence, lacunarity);
                    f.array[f.To1D(x, y)] = p;
                }
            return f;
        }

        public static FloatField Noise(uint w, uint h, float a = 0f, float b = 1f)
        {
            FloatField f = new FloatField(w, h);
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                    f.array[f.To1D(x, y)] = RandomRange(a, b);
            return f;
        }

        public static FloatField FiniteNoise(uint w, uint h, uint steps, float a = 0f, float b = 1f)
        {
            FloatField f = new FloatField(w, h);
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                    f.array[f.To1D(x, y)] = (float)((int)(RandomRange(a, b) * steps)) / steps;
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

        public static FloatField TriangleFan(uint w, uint h, Vector2 middle, Vector2[] points)
        {
            FloatField f = new FloatField(w, h);
            for(int x = 0; x < w; x++)
                for(int y = 0; y < h; y++)
                    for(int i = 0; i < points.Length; i++)
                    {
                        Vector2 q;
                        if (i + 1 >= points.Length) q = points[0];
                        else q = points[i + 1];
                        if (!PointInTriangle(new Vector2((float)x/w, (float)y/h), middle, points[i], q)) continue;
                        f.array[f.To1D(x, y)] = 1f;
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

        public static FloatField VoronoiBlock(uint w, uint h, uint p, float diff, bool flat)
        {
            uint stepw = w / 3, steph = h / 3;
            Vector2[] points = new Vector2[p + 12];
            for(int i = 0; i < 4; i++)
                points[i] = new Vector2(i * stepw, 0);
            for (int i = 0; i < 4; i++)
                points[i + 4] = new Vector2(i * stepw, h);
            points[8] = new Vector2(0, steph);
            points[9] = new Vector2(0, steph * 2);
            points[10] = new Vector2(w, steph);
            points[11] = new Vector2(w, steph * 2);
            for(int i = 0; i < p; i++)
                points[12 + i] = new Vector2(RandomDeviation(0.5f, diff) *w, RandomDeviation(0.5f, diff)*h);
            FloatField f = new FloatField(w, h);
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                {
                    Vector2 point = new Vector2(x, y);
                    float min = 1000000f;
                    int ppp = 0;
                    for(int i = 0; i < points.Length; i++)
                    {
                        Vector2 diffr = point - points[i];
                        float dist = diffr.LengthSquared();
                        if (dist < min) { min = dist; ppp = i; }
                    }
                    if (flat) f.array[f.To1D(x, y)] = (float)ppp / points.Length;
                    else f.array[f.To1D(x, y)] = (float)Math.Sqrt(min) / w;
                }
            return f;
        }

        public static FloatField Circle(uint w, uint h, float r, float alphaA, float alphaB, float inner = 0.0f, float xx = 0.5f, float yy = 0.5f)
        {
            if (r < 0) r = 0;
            r *= Math.Min(w / 2, h / 2);
            if (inner < 0) inner = 0;
            if (inner > 1) inner = 1;
            inner *= Math.Min(w / 2, h / 2);
            Vector2 mid = new Vector2(w * xx, h * yy);
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

        public static FloatField Rectangle(uint w, uint h, float xx, float yy, float ww, float hh)
        {
            xx *= w;
            ww *= w;
            yy *= h;
            hh *= h;
            FloatField f = new FloatField(w, h);
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                    if(x >= xx && x <= xx + ww && y >= yy && y <= yy + hh)
                        f.array[f.To1D(x, y)] = 1.0f;
            return f;
        }

        public static FloatField Edge(FloatField ff)
        {
            FloatField f = new FloatField((uint)ff.Width, (uint)ff.Height);
            for(int x = 0; x < f.Width; x++)
                for(int y = 0; y < f.Height; y++)
                {
                    float original = ff.array[ff.To1D(x, y)];
                    if (original == 0f) continue;
                    bool up = ff.array[ff.To1D(x, Math.Max(y - 1, 0))] == 0f;
                    bool down = ff.array[ff.To1D(x, Math.Min(y + 1, ff.Height - 1))] == 0f;
                    bool left = ff.array[ff.To1D(Math.Max(x - 1, 0), y)] == 0f;
                    bool right = ff.array[ff.To1D(Math.Min(x + 1, ff.Width - 1), y)] == 0f;
                    if (y - 1 <= 0) up = true;
                    if (y + 1 >= ff.Height) down = true;
                    if (x - 1 <= 0) left = true;
                    if (x + 1 >= ff.Width) right = true;
                    if (up || down || left || right)
                        f.array[f.To1D(x, y)] = 1f;
                }
            return f;
        }

        public static FloatField EdgeFromFlats(FloatField ff)
        {
            FloatField f = new FloatField((uint)ff.Width, (uint)ff.Height);

            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                {
                    float current = ff.array[ff.To1D(x, y)];
                    bool left = ff.array[ff.To1D(Math.Min(x + 1, f.Width - 1), y)] != current;
                    bool down = ff.array[ff.To1D(x, Math.Min(y + 1, f.Height - 1))] != current;
                    if (left || down) f.array[f.To1D(x, y)] = 1f;
                }
            return f;
        }

        public static FloatField EdgeToBlendBody(FloatField edge, FloatField body, float power, float from = 0f, float to = 1f)
        {
            //return body;
            FloatField f = new FloatField((uint)edge.Width, (uint)edge.Height);
            f.Copy(body);
            List<Vector2> edges = new List<Vector2>();
            for (int x = 0; x < edge.Width; x++)
                for (int y = 0; y < edge.Height; y++)
                {
                    float init = edge.array[body.To1D(x, y)];
                    if (init == 0f) continue;
                    edges.Add(new Vector2(x, y));
                }
            for (int x = 0; x < edge.Width; x++)
                for (int y = 0; y < edge.Height; y++)
                {
                    float original = body.array[body.To1D(x, y)];
                    if (original == 0f) continue;
                    float min = 1000f;
                    for (int i = 0; i < edges.Count; i++) {
                        float dist = (new Vector2(x, y) - edges[i]).LengthSquared();
                        if (dist < min) min = dist;
                    }
                    min = (float)Math.Sqrt(min);
                    f.array[f.To1D(x, y)] = Lerp(from, to, (float)MathH.Clamp(min / f.Width / power, 0, 1));
                }

            return f;
        }

        public static FloatField RandomWalk(uint w, uint h, float xx, float yy, uint steps, Vector2 dir = new Vector2())
        {
            if (xx < 0 || xx >= w) xx = 0;
            if (yy < 0 || yy >= h) yy = 0;
            Vector2 pos = new Vector2(xx * w, yy * h);
            FloatField f = new FloatField(w, h);
            for(int i = 0; i < steps; i++)
            {
                Vector2 step = new Vector2((float)MathH.random.NextDouble()*2f - 1f, (float)MathH.random.NextDouble()*2f - 1f);
                step += dir;
                step.Normalize();
                pos += step;
                pos.X = (float)MathH.Clamp(pos.X, 0, w - 1);
                pos.Y = (float)MathH.Clamp(pos.Y, 0, h - 1);
                f.array[f.To1D((int)pos.X, (int)pos.Y)] = 1f;
            }
            return f;
        }

        public static FloatField Fade(uint w, uint h, bool xas, float from, float to)
        {
            uint size = xas ? w : h;
            uint start = (uint)(from * size);
            uint end = (uint)(to * size);
            float stretch = Math.Abs(from - to);
            FloatField f = new FloatField(w, h);
            for (int x = 0; x < f.Width; x++)
                for (int y = 0; y < f.Height; y++)
                {
                    float res = 0f;
                    float v = xas ? x : y;
                    if (v < start) res = 1f;
                    else if (v > end) res = 0f;
                    else res = Lerp(1f, 0f, (v - start) / stretch / size);
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

        public static FloatField Difference(FloatField a, FloatField b)
        {
            FloatField res = new FloatField((uint)a.Width, (uint)a.Height);
            for (int x = 0; x < res.Width; x++)
                for (int y = 0; y < res.Height; y++)
                {
                    float aa = a.array[a.To1D(x, y)];
                    float bb = b.array[b.To1D(x % b.Width, y % b.Height)];
                    float r = aa;
                    if (bb > 0f) r = 0f;
                    res.array[res.To1D(x, y)] = r;
                }
            return res;
        }
        
        public static FloatField Sum(FloatField a, FloatField b)
        {
            FloatField res = new FloatField((uint)a.Width, (uint)a.Height);
            for(int i = 0; i < res.array.Length; i++)
            {
                float x = a.array[i % a.array.Length] + b.array[i % b.array.Length];
                x = Math.Min(x, 1f);
                res.array[i] = x;
            }
            return res;
        }

        public static void CutAlpha(ColourField cf, FloatField ff)
        {
            for(int x = 0; x < cf.Width; x++)
                for(int y = 0; y < cf.Height; y++)
                {
                    float cut = ff.array[ff.To1D(x % ff.Width, y % ff.Height)];
                    if (cut == 0f) cf.Set(new Colour(0,0,0,0), x, y);
                }
        }

        public static void ScaleClamp(FloatField ff, float x)
        {
            for (int i = 0; i < ff.array.Length; i++)
                ff.array[i] = (float)MathH.Clamp(ff.array[i] * x, 0f, 1f);
        }

        public static void AddClamp(FloatField ff, float x)
        {
            for (int i = 0; i < ff.array.Length; i++)
                ff.array[i] = (float)MathH.Clamp(ff.array[i] + x, 0f, 1f);
        }

        public static void CopyMin(FloatField fa, FloatField fb)
        {
            for (int i = 0; i < fa.array.Length; i++)
                fa.array[i] = (float)Math.Min(fa.array[i], fb.array[i % fb.array.Length]);
        }

        public static void NormalizeSize(FloatField ff)
        {
            uint minx = (uint)ff.Width, maxx = 0, miny = (uint)ff.Height, maxy = 0;
            for (uint x = 0; x < ff.Width; x++)
                for (uint y = 0; y < ff.Height; y++)
                {
                    if (ff.array[ff.To1D((int)x, (int)y)] == 0.0f) continue;
                    if (minx > x) minx = x;
                    if (maxx < x) maxx = x;
                    if (miny > y) miny = y;
                    if (maxy < y) maxy = y;
                }
            if (minx >= maxx || miny >= maxy) return;
            FloatField cut = ff.CopyCut(minx, miny, maxx, maxy);
            ff.CopyStretch(cut);
        }

        public static void Invert(FloatField ff)
        {
            for (int i = 0; i < ff.array.Length; i++)
                ff.array[i] = 1.0f - ff.array[i];
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

        public static void Multiply(ColourField ca, FloatField fb)
        {
            for (int x = 0; x < ca.Width; x++)
                for (int y = 0; y < ca.Height; y++)
                {
                    Colour aa = ca.Get(x, y);
                    if (aa.a == 0f) continue;
                    float bb = fb.array[fb.To1D(x % fb.Width, y % fb.Height)];
                    Colour r = aa * bb;
                    ca.Set(r, x, y);
                }
        }

        public static void ThresholdCut(FloatField f, float min, float max, float minval, float maxval)
        {
            for(int x = 0; x < f.Width; x++)
                for(int y = 0; y < f.Height; y++)
                {
                    float r = f.array[f.To1D(x, y)];
                    if (r < min) r = minval;
                    if (r > max) r = maxval;
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

        public static void Normalize(FloatField f, bool nozero = false)
        {
            float min = nozero ? 0.01f : 0f;
            ToRange(f, min, 1);
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

        public static float RandomRange(float a, float b)
        {
            return Lerp(a, b, (float)MathH.random.NextDouble());
        }

        public static float RandomDeviation(float x, float t)
        {
            return Lerp(x - t, x + t, (float)MathH.random.NextDouble());
        }

        public static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }
        //source:
        //https://stackoverflow.com/questions/2049582/how-to-determine-if-a-point-is-in-a-2d-triangle
        public static bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            bool b1, b2, b3;

            b1 = Sign(pt, v1, v2) <= 0.0f;
            b2 = Sign(pt, v2, v3) <= 0.0f;
            b3 = Sign(pt, v3, v1) <= 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;
//<author:cody>
//helper classes to edit images
namespace UU_GameProject
{
    public struct Colour
    {
        public float r, g, b, a;

        public Colour(float x)
        {
            r = g = b = x;
            a = 1;
        }

        public Colour(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 1;
        }

        public Colour(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public float Strength()
        {
            return (r + g + b) / 3;
        }

        public bool ZeroAlpha()
        {
            return a == 0f;
        }

        public static Colour Black { get { return new Colour(0, 0, 0); } }
        public static Colour White { get { return new Colour(1, 1, 1); } }

        public static Colour Blend(Colour a, Colour b)
        {
            if (a.a == 0f) return b;
            if (b.b == 0f) return a;
            return Image.Lerp(a, b, b.a * 0.5f);
        }

        public static Colour BlendOver(Colour a, Colour over)
        {
            if (a.a == 0f) return over;
            if (over.b == 0f) return a;
            return Image.Lerp(a, over, over.a);
        }

        public static Colour operator* (Colour c, float m)
        {
            return new Colour(c.r * m, c.g * m, c.b * m);
        }

        public static Colour operator* (Colour x, Colour y)
        {
            return new Colour(x.r * y.r, x.g * y.g, x.b * y.b);
        }

        public static Colour operator/ (Colour c, float m)
        {
            return new Colour(c.r / m, c.g / m, c.b / m);
        }

        public static Colour operator/ (Colour x, Colour y)
        {
            return new Colour(x.r / y.r, x.g / y.g, x.b / y.b);
        }

        public static Colour operator+ (Colour c, float m)
        {
            return new Colour(c.r + m, c.g + m, c.b + m);
        }

        public static Colour operator+ (Colour x, Colour y)
        {
            return new Colour(x.r + y.r, x.g + y.g, x.b + y.b);
        }

        public static Colour operator- (Colour c, float m)
        {
            return new Colour(c.r - m, c.g - m, c.b - m);
        }

        public static Colour operator- (Colour x, Colour y)
        {
            return new Colour(x.r - y.r, x.g - y.g, x.b - y.b);
        }
    }

    public class ColourField
    {
        public Colour[] array;
        private int w, h;
        private Texture2D texture;
        public int Width { get { return w; } }
        public int Height { get { return h; } }
        public Texture2D Texture { get { return texture; } }

        public ColourField(uint w, uint h)
        {
            this.w = (int)w;
            this.h = (int)h;
            array = new Colour[w * h];
            texture = AssetManager.GetNewTexture(w, h);
        }

        public void FloatsToColours(FloatField ff, Colour a, Colour b, bool deleteZero = true)
        {
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    float t = ff.array[ff.To1D(x % ff.Width, y % ff.Height)];
                    if (t > 0) array[To1D(x, y, h)] = Image.Lerp(a, b, t);
                    else array[To1D(x, y, h)] = new Colour(0, 0, 0, 0);
                }
        }

        public void Blend(ColourField over)
        {
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    Colour t = over.Get(x % over.Width, y % over.Height);
                    Colour s = array[To1D(x, y, h)];
                    array[To1D(x, y, h)] = Colour.Blend(s, t);
                }
        }

        public void DrawOver(ColourField over, uint xx, uint yy)
        {
            int lx = 0, ly = 0;
            for (int x = (int)xx; x < Math.Min(w, xx + over.w); x++, lx++, ly = 0)
                for (int y = (int)yy; y < Math.Min(h, yy + over.h); y++, ly++)
                {
                    Colour t = over.Get(lx, ly);
                    Colour s = array[To1D(x, y, h)];
                    Colour res = t.ZeroAlpha() ? s : t;
                    array[To1D(x, y, h)] = res;
                }
        }

        public void BlendOver(ColourField over, uint xx, uint yy)
        {
            int lx = 0, ly = 0;
            for (int x = (int)xx; x < Math.Min(w, xx + over.w); x++, lx++, ly = 0)
                for (int y = (int)yy; y < Math.Min(h, yy + over.h); y++, ly++)
                {
                    Colour t = over.Get(lx, ly);
                    Colour s = array[To1D(x, y, h)];
                    Colour res = Colour.BlendOver(s, t);
                    array[To1D(x, y, h)] = res;
                }
        }

        public void SetAlpha(float alpha, bool zeroStayZero = true)
        {
            int tot = w * h;
            for(int i = 0; i < tot; i++)
            {
                float sa = array[i].a;
                if (sa == 0 && zeroStayZero) continue;
                array[i].a = alpha;
            }
        }

        public void SetAlpha(FloatField f)
        {
            if (f.Width != w || f.Height != h) return;
            int tot = w * h;
            for(int i = 0; i < tot; i++)
            {
                float a = f.array[i];
                Colour s = array[i];
                s.a = a;
                array[i] = s;
            }
        }

        public void Save()
        {
            Color[] c = new Color[w * h];
            for (int i = 0; i < w * h; i++)
                c[i] = new Color(array[i].r, array[i].g, array[i].b, array[i].a);
            texture.SetData<Color>(c);
        }

        public void Fill(Colour c)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = c;
        }

        public void Set(Colour c, int x, int y)
        {
            if (x < 0 || x >= w) return;
            if (y < 0 || y >= h) return;
            array[To1D(x, y, h)] = c;
        }

        public Colour Get(int x, int y)
        {
            return array[To1D(x, y, h)];
        }

        public int To1D(int x, int y, int h)
        {
            return y * w + x;
        }

        public float Average()
        {
            float res = 0.0f;
            for (int i = 0; i < w * h; i++)
                res += array[i].Strength();
            res /= w * h;
            return res;
        }

        public float Min()
        {
            float res = 10000000.0f;
            for (int i = 0; i < w * h; i++)
            {
                float s = array[i].Strength();
                if (s < res) res = s;
            }
            return res;
        }

        public float Max()
        {
            float res = 0.0f;
            for (int i = 0; i < w * h; i++)
            {
                float s = array[i].Strength();
                if (s > res) res = s;
            }
            return res;
        }
    }

    public class FloatField
    {
        public float[] array;
        private int w, h;
        public int Width { get { return w; } }
        public int Height { get { return h; } }

        public FloatField(uint w, uint h)
        {
            this.w = (int)w;
            this.h = (int)h;
            array = new float[w * h];
        }

        public void ColoursToFloats(ColourField cf)
        {
            if (w > cf.Width || h > cf.Height) return;
            int tot = w * h;
            for (int i = 0; i < tot; i++)
                array[i] = cf.array[i].Strength();
        }

        public void CopyStretch(FloatField ff)
        {
            for (int x = 0; x < w; x++)
            {
                int sx = (int)Math.Round(((float)x / w) * ff.w);
                for (int y = 0; y < h; y++)
                {
                    int sy = (int)Math.Round(((float)y / h) * ff.h);
                    array[To1D(x, y)] = ff.array[ff.To1D(sx, sy)];
                }
            }
        }

        public FloatField CopyCut(uint x0, uint y0, uint x1, uint y1)
        {
            if (x0 < 0 || y0 < 0 || x0 >= w || y0 >= h) return null;
            if (x1 < 1 || y1 < 1 || x1 >= w || y1 >= h) return null;
            uint ww = x1 - x0 + 1, hh = y1 - y0 + 1;
            FloatField ff = new FloatField(ww, hh);
            for (int x = 0; x < ww; x++)
                for(int y = 0; y < hh; y++)
                {
                    ff.array[ff.To1D(x, y)] = array[To1D((int)x0 + x, (int)y0 + y)];
                }
            return ff;
        }

        public int To1D(int x, int y)
        {
            return y * w + x;
        }

        public int To1D(int x, int y, int h)
        {
            return y * h + x;
        }

        public float Average()
        {
            float res = 0.0f;
            for (int i = 0; i < w * h; i++)
                res += array[i];
            res /= w * h;
            return res;
        }

        public float Min()
        {
            float res = 10000000.0f;
            for (int i = 0; i < w * h; i++)
            {
                float s = array[i];
                if (s < res) res = s;
            }
            return res;
        }

        public float Max()
        {
            float res = 0.0f;
            for (int i = 0; i < w * h; i++)
            {
                float s = array[i];
                if (s > res) res = s;
            }
            return res;
        }

        public void Copy(FloatField f)
        {
            if (w != f.w || h != f.h) return;
            int tot = w * h;
            for (int i = 0; i < tot; i++)
                array[i] = f.array[i];
        }
    }
}
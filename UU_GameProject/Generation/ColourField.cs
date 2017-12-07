using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;

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
        private Colour[] array;
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

        public void Save()
        {
            Color[] c = new Color[w * h];
            for (int i = 0; i < w * h; i++)
                c[i] = new Color(array[i].r, array[i].g, array[i].b, array[i].a);
            texture.SetData<Color>(c);
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
            return y * h + x;
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
}
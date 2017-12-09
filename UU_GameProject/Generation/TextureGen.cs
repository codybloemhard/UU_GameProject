using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;
/*
biomes: artic, desert, hills, forest
*/
namespace UU_GameProject
{
    public static class TextureGen
    {
        public const uint SXSsmall = 8;
        public const uint SSmall = 16;
        public const uint SMedium = 32;
        public const uint SLarge = 64;
        public const uint SXLarge = 128;
        public const uint SXXLarge = 256;

        public static Texture2D GenBoulder()
        {
            const uint size = SLarge;
            FloatField fPerlin = Image.Perlin(size, size, 4, 5, 0.3, 3f);
            Image.Normalize(fPerlin, true);
            FloatField fMask = Image.VoronoiCell(size, size, 20, 0.8f, 0.15f);
            FloatField fEdge = Image.Edge(fMask);
            FloatField fGlow = Image.EdgeToBlendBody(fEdge, fMask, 0.1f, 0.0f, 1f);
            FloatField fResult = Image.Intersection(fPerlin, fMask);
            Image.Multiply(fResult, fGlow);
            ColourField cf = new ColourField(size, size);
            Colour cDark = new Colour(0.2f, 0.1f, 0.1f) / 2;
            Colour clight = new Colour(0.5f, 0.2f, 0.1f) * 2f;
            cf.FloatsToColours(fResult, cDark, clight);
            cf.Save();
            return cf.Texture;
        }

        public static Texture2D GenStone()
        {
            const uint size = SMedium;
            FloatField fPerlin = Image.Perlin(size, size, 3, 1, 0.5, 3f);
            Image.ToRange(fPerlin, 0.3f, 1.0f);
            FloatField fMask = Image.VoronoiCell(size, size, 8, 0.9f, 0.4f);
            FloatField fEdge = Image.Edge(fMask);
            FloatField fGlow = Image.EdgeToBlendBody(fEdge, fMask, 0.1f, 0.0f, 1.0f);
            FloatField fResult = Image.Intersection(fPerlin, fMask);
            Image.Multiply(fResult, fGlow);
            ColourField cf = new ColourField(size, size);
            Colour cDark = new Colour(0.2f, 0.15f, 0.15f) * 0.5f;
            Colour clight = new Colour(0.5f, 0.4f, 0.4f);
            cf.FloatsToColours(fResult, cDark, clight);
            cf.Save();
            return cf.Texture;
        }

        public static Texture2D GenStoneShard()
        {
            const uint size = SMedium;
            FloatField fPerlin = Image.Perlin(size, size, 3, 10, 0.5, 2f);
            Image.ToRange(fPerlin, 0.3f, 1.0f);
            FloatField fMask = Image.VoronoiCell(size, size, 7, 1f, 0.8f);
            FloatField fEdge = Image.Edge(fMask);
            FloatField fGlow = Image.EdgeToBlendBody(fEdge, fMask, 0.2f, 0.5f, 1.0f);
            FloatField fResult = Image.Intersection(fPerlin, fMask);
            Image.Multiply(fResult, fGlow);
            ColourField cBody = new ColourField(size, size);
            Colour cDark = new Colour(0.4f, 0.2f, 0.05f) * 0.5f;
            Colour cLight = new Colour(0.8f, 0.5f, 0.2f);
            cBody.FloatsToColours(fResult, cDark, cLight);
            ColourField cEdge = new ColourField(size, size);
            cEdge.FloatsToColours(fEdge, cLight*2, cLight*2);
            cEdge.SetAlpha(0.25f);
            cBody.Blend(cEdge);
            cBody.Save();
            return cBody.Texture;
        }

        public static Texture2D GenCloud()
        {
            const uint size = SLarge;
            FloatField fPerlin = Image.Perlin(size, size, 1, 3, 1, 1);
            Image.ToRange(fPerlin, 0.5f, 1.0f);
            FloatField fMask = null;
            int max = (int)(MathH.random.NextDouble() * 3) + 4;
            for (int i = 0; i < max; i++)
            {
                float r = 0.25f;
                float xx = r + ((float)MathH.random.NextDouble() * (1.0f - r*2));
                float yy = r + ((float)MathH.random.NextDouble() * (1.0f - r*2));
                FloatField circle = Image.Circle(size, size, r*2, 1.0f, 1.0f, 0.0f, xx, yy);
                if (fMask == null) fMask = circle;
                else fMask = Image.Union(fMask, circle);
            }
            FloatField fEdge = Image.Edge(fMask);
            FloatField fShine = Image.EdgeToBlendBody(fEdge, fMask, 0.1f, 0.5f, 0.9f);
            FloatField fResult = Image.Intersection(fPerlin, fMask);
            ColourField cBody = new ColourField(size, size);
            Colour cDark = new Colour(0.3f, 0.2f, 0.9f) * 0.5f;
            Colour cLight = new Colour(0.2f, 0.5f, 0.9f);
            cBody.FloatsToColours(fResult, cDark, cLight);
            cBody.SetAlpha(fShine);
            cBody.Save();
            return cBody.Texture;
        }

        public static Texture2D GenBushLeaf()
        {
            const uint size = SMedium;
            FloatField fPerlin = Image.Perlin(size, size, 2, 3, 0.5, 3);
            Image.Normalize(fPerlin, true);
            FloatField fMask = null;
            for (int i = 0; i < 3; i++)
            {
                float r = 0.3f;
                float xx = r + ((float)MathH.random.NextDouble() * (1.0f - r * 2));
                float yy = r + ((float)MathH.random.NextDouble() * (1.0f - r * 2));
                FloatField circle = Image.Circle(size, size, r * 2, 1.0f, 1.0f, 0.0f, xx, yy);
                if (fMask == null) fMask = circle;
                else fMask = Image.Union(fMask, circle);
            }
            FloatField fResult = Image.Intersection(fPerlin, fMask);
            ColourField cBody = new ColourField(size, size);
            Colour cDark = new Colour(0.05f, 0.3f, 0.1f);
            Colour cLight = new Colour(0.1f, 0.6f, 0.2f);
            cBody.FloatsToColours(fResult, cDark, cLight);
            cBody.Save();
            return cBody.Texture;
        }

        public static Texture2D GenBerry()
        {
            const uint size = SXSsmall;
            FloatField fPerlin = Image.Perlin(size, size, 2, 3, 0.5, 3);
            Image.Normalize(fPerlin, true);
            FloatField fMask = null;
            for (int i = 0; i < 3; i++)
            {
                float r = 0.3f;
                float xx = r + ((float)MathH.random.NextDouble() * (1.0f - r * 2));
                float yy = r + ((float)MathH.random.NextDouble() * (1.0f - r * 2));
                FloatField circle = Image.Circle(size, size, r * 2, 1.0f, 1.0f, 0.0f, xx, yy);
                if (fMask == null) fMask = circle;
                else fMask = Image.Union(fMask, circle);
            }
            FloatField fResult = Image.Intersection(fPerlin, fMask);
            ColourField cBody = new ColourField(size, size);
            Colour cDark = new Colour(0.3f, 0.1f, 0.1f);
            Colour cLight = new Colour(0.6f, 0.1f, 0.2f);
            cBody.FloatsToColours(fResult, cDark, cLight);
            cBody.Save();
            return cBody.Texture;
        }
    }
}
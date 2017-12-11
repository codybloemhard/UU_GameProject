using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;

namespace UU_GameProject
{
    public static class TextureGen
    {
        public const uint SXXSsmall = 4;
        public const uint SXSsmall = 8;
        public const uint SSmall = 16;
        public const uint SMedium = 32;
        public const uint SLarge = 64;
        public const uint SXLarge = 128;
        public const uint SXXLarge = 256;

        public static ColourField Gen(string name)
        {
            switch (name)
            {
                //blocks
                case "_dirtgrassblock": return GenDirtGrassBlock();
                //objects
                case "_boulder": return GenBoulder();
                case "_stone": return GenStone();
                case "_stoneshard": return GenStoneShard();
                case "_bushleaf": return GenBushLeaf();
                case "_berry": return GenBerry();
                case "_bush": return GenBush();
                case "_cloud": return GenCloud();
                //snowman
                case "_snowmanbody": return GenSnowManBody();
                case "_snowmaneye": return GenSnowManEye();
                case "_snowmanhat": return GenSnowManHat();
                case "_snowmanmouth": return GenSnowManMouth();
                case "_snowmannose": return GenSnowManNose();
                case "_snowmanarmleft": return GenSnowManArm(true);
                case "_snowmanarmright": return GenSnowManArm(false);
                case "_stonesnowy": return GenStoneSnowy();
                //basics
                case "_greystone": return GenGrayStone();
                case "_snow": return GenSnow();
                case "_grass": return GenGrass();
                case "_dirt": return GenDirt();
                default: return null;
            }
        }

        public static ColourField GenBoulder()
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
            return cf;
        }

        public static FloatField GenStoneMask()
        {
            const uint size = SMedium;
            FloatField fMask = Image.VoronoiCell(size, size, 8, 0.9f, 0.4f);
            return fMask;
        }
        
        public static ColourField GenStone()
        {
            FloatField fMask = GenStoneMask();
            FloatField fEdge = Image.Edge(fMask);
            FloatField fGlow = Image.EdgeToBlendBody(fEdge, fMask, 0.1f, 0.4f, 1.0f);
            ColourField cf = GenGrayStone();
            cf.SetAlpha(fMask);
            Image.Multiply(cf, fGlow);
            cf.Save();
            return cf;
        }

        public static ColourField GenStoneSnowy()
        {
            const uint size = SMedium;
            FloatField fMask = GenStoneMask();
            FloatField fEdge = Image.Edge(fMask);
            FloatField fGlow = Image.EdgeToBlendBody(fEdge, fMask, 0.1f, 0.4f, 1.0f);
            FloatField fSnow = Image.Circle(size, size, Image.RandomRange(2.5f, 3f), 1f, 0f, 0.8f, 0.5f, -0.5f);
            fSnow = Image.Intersection(fSnow, fMask);
            ColourField cSnow = GenSnow(size);
            cSnow.SetAlpha(fSnow);
            ColourField cStone = GenGrayStone(size);
            cStone.SetAlpha(fMask);
            Image.Multiply(cStone, fGlow);
            cStone.BlendOver(cSnow, 0, 0);
            cStone.Save();
            return cStone;
        }

        public static ColourField GenStoneShard()
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
            return cBody;
        }

        public static ColourField GenDirtGrassBlock()
        {
            const uint size = SMedium;
            ColourField cGrass = GenGrass(size, size);
            ColourField cDirt = GenDirt(size, size);
            FloatField fMask = Image.Perlin(size, size, 2, 4, 0.5f, 2f);
            FloatField fFade = Image.Fade(size, size, false, false);
            Image.Power(fFade, 2);
            Image.Normalize(fFade);
            Image.Multiply(fMask, fFade);
            cGrass.SetAlpha(fMask);
            cDirt.BlendOver(cGrass, 0, 0);
            cDirt.Save();
            return cDirt;
        }

        public static ColourField GenCloud()
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
            return cBody;
        }

        public static ColourField GenBushLeaf()
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
            Image.NormalizeSize(fMask);
            FloatField fResult = Image.Intersection(fPerlin, fMask);
            FloatField fEdge = Image.Edge(fMask);
            FloatField fAlpha = Image.EdgeToBlendBody(fEdge, fMask, 0.1f, 0.6f, 1.0f);
            ColourField cBody = new ColourField(size, size);
            Colour cDark = new Colour(0.05f, 0.3f, 0.1f);
            Colour cLight = new Colour(0.1f, 0.6f, 0.2f);
            cBody.FloatsToColours(fResult, cDark, cLight);
            cBody.SetAlpha(fAlpha);
            cBody.Save();
            return cBody;
        }

        public static ColourField GenBerry(uint size = SXSsmall, Colour cDark = new Colour(), Colour cLight = new Colour())
        {
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
            if(cDark.a == 0f) cDark = new Colour(0.3f, 0.1f, 0.1f);
            if(cLight.a == 0f) cLight = new Colour(0.6f, 0.1f, 0.2f);
            cBody.FloatsToColours(fResult, cDark, cLight);
            cBody.Save();
            return cBody;
        }

        public static ColourField GenBush()
        {
            ColourField bush = GenBushLeaf();

            uint max = (uint)Image.RandomRange(5, 8);
            for(int i = 0; i < max; i++)
            {
                uint x = 0, y = 0;
                byte c = 0;
                while (c < 10)
                {
                    x = (uint)Image.RandomRange(0, bush.Width);
                    y = (uint)Image.RandomRange(0, bush.Height);
                    bool ok = bush.Get((int)x, (int)y).a > 0.8f;
                    if (ok) break;
                    c++;//masterrace
                }
                ColourField smallBerry = GenBerry(4);
                bush.DrawOver(smallBerry, x, y);
            }
            bush.Save();
            return bush;
        }

        //start Snowman
        public static ColourField GenSnowManEye()
        {
            return GenBerry(4, new Colour(0.1f), new Colour(0.2f));
        }

        public static ColourField GenSnowManNose()
        {
            Colour orange = new Colour(0.5f, 0.3f, 0.1f);
            return GenBerry(3, orange * 0.5f, orange * 1.5f);
        }

        public static ColourField GenSnowManArm(bool left = false)
        {
            const uint size = 32;
            float xstart = left ? 1f : 0f;
            float xdir = left ? -2f : 2f;
            FloatField fMask = Image.RandomWalk(size, size, xstart, 1.0f, 32, new Vector2(xdir, -5f) * 0.25f);
            ColourField cFinal = new ColourField(size, size);
            Colour cDark = new Colour(0.2f, 0.1f, 0.1f);
            cFinal.FloatsToColours(fMask, cDark, cDark);
            cFinal.Save();
            return cFinal;
        }

        public static ColourField GenSnowManMouth()
        {
            const uint size = SSmall;
            FloatField fMask = Image.Circle(size, size/2, 0.9f, 1f, 1f, 0f, 0.5f, 0.5f);
            FloatField fNeg = Image.Circle(size, size / 2, 0.9f, 1f, 1f, 0f, 0.5f, 0.35f);
            fMask = Image.Difference(fMask, fNeg);
            Colour cDark = new Colour(0.1f);
            Colour cLight = new Colour(0.1f);
            ColourField cFinal = new ColourField(size, size/2);
            cFinal.FloatsToColours(fMask, cDark, cLight);
            cFinal.Save();
            return cFinal;
        }

        public static ColourField GenSnowManHat()
        {
            const uint size = SSmall;
            FloatField fMask = Image.Rectangle(size, size, 0.15f, 0.0f, 0.7f, 0.7f);
            FloatField fExtra = Image.Rectangle(size, size, 0.0f, 0.7f, 1f, 0.3f);
            fMask = Image.Union(fMask, fExtra);
            FloatField fEdge = Image.Edge(fMask);
            FloatField fGlow = Image.EdgeToBlendBody(fEdge, fMask, 0.1f, 0.5f, 1f);
            FloatField fGrain = Image.Noise(size, size, 0.1f, 1f);
            ColourField cFinal = new ColourField(size, size);
            Colour cDark = new Colour(0.2f);
            Colour cLight = new Colour(0.3f);
            cFinal.FloatsToColours(fGrain, cDark, cLight);
            Image.Multiply(cFinal, fGlow);
            cFinal.SetAlpha(fMask);
            cFinal.Save();
            return cFinal;
        }

        public static ColourField GenSnowManBody()
        {
            const uint w = 32, h = 64;
            FloatField fMask = new FloatField(32, 64);
            FloatField fBody0 = Image.Circle(32, 64, Image.RandomDeviation(0.9f, 0.05f), 1, 1, 0, 0.5f, 0.85f);
            FloatField fBody1 = Image.Circle(32, 64, Image.RandomDeviation(0.7f, 0.05f), 1, 1, 0, 0.5f, 0.55f);
            FloatField fBody2 = Image.Circle(32, 64, Image.RandomDeviation(0.5f, 0.05f), 1, 1, 0, 0.5f, 0.3f);
            fMask = Image.Union(fMask, fBody0);
            fMask = Image.Union(fMask, fBody1);
            fMask = Image.Union(fMask, fBody2);
            FloatField fEdge = Image.Edge(fMask);
            FloatField fGlow = Image.EdgeToBlendBody(fEdge, fMask, 0.2f, 0.5f, 1f);
            ColourField cSnow = GenSnow(32, 64);
            for(uint i = 0; i < 8; i++)
            {
                ColourField detail = GenBerry(3, new Colour(0.1f), new Colour(0.2f));
                cSnow.DrawOver(detail, 14, 28 + i*6);
            }
            cSnow.SetAlpha(fMask);
            Image.Multiply(cSnow, fGlow);
            cSnow.Save();
            return cSnow;
        }
        //end Snowman
        //raw materials
        public static ColourField GenSnow(uint w = SMedium, uint h = SMedium)
        {
            FloatField fGrain = Image.Noise(w, h);
            ColourField cFinal = new ColourField(w, h);
            Colour cDark = new Colour(0.6f, 0.6f, 0.7f);
            Colour cLight = new Colour(0.9f, 0.9f, 0.9f);
            cFinal.FloatsToColours(fGrain, cDark, cLight);
            fGrain = Image.Perlin(w, h, 2, 2, 0.5f, 2f);
            ColourField cPerlin = new ColourField(w, h);
            cPerlin.FloatsToColours(fGrain, cDark, cLight);
            cFinal.Blend(cPerlin);
            cFinal.Save();
            return cFinal;
        }

        public static ColourField GenGrayStone(uint w = SMedium, uint h = SMedium)
        {
            FloatField fPerlin = Image.Perlin(w, h, 3, 1, 0.5, 3f);
            Image.ToRange(fPerlin, 0.3f, 1.0f);
            ColourField cf = new ColourField(w, h);
            Colour cDark = new Colour(0.2f, 0.15f, 0.15f) * 0.5f;
            Colour clight = new Colour(0.5f, 0.4f, 0.4f);
            cf.FloatsToColours(fPerlin, cDark, clight);
            cf.Save();
            return cf;
        }

        public static ColourField GenGrass(uint w = SMedium, uint h = SMedium)
        {
            FloatField fGrain = Image.Noise(w, h);
            Image.ToRange(fGrain, 0.3f, 0.7f);
            ColourField cFinal = new ColourField(w, h);
            Colour cDark = new Colour(0.3f, 0.9f, 0.3f) * 0.3f;
            Colour cLight = new Colour(0.2f, 0.9f, 0.2f);
            cFinal.FloatsToColours(fGrain, cDark, cLight);
            fGrain = Image.Perlin(w, h, 2, 3, 0.5f, 2f);
            ColourField cPerlin = new ColourField(w, h);
            cPerlin.FloatsToColours(fGrain, cDark, cLight);
            cFinal.Blend(cPerlin);
            cFinal.Save();
            return cFinal;
        }

        public static ColourField GenDirt(uint w = SMedium, uint h = SMedium)
        {
            FloatField fGrain = Image.Noise(w, h);
            Image.Normalize(fGrain, true);
            ColourField cFinal = new ColourField(w, h);
            Colour cDark = new Colour(0.4f, 0.3f, 0.3f) * 0.3f;
            Colour cLight = new Colour(0.5f, 0.2f, 0.2f);
            cFinal.FloatsToColours(fGrain, cDark, cLight);
            fGrain = Image.Perlin(w, h, 2, 2, 0.5f, 2f);
            Image.ToRange(fGrain, 0.1f, 0.5f);
            ColourField cPerlin = new ColourField(w, h);
            cPerlin.FloatsToColours(fGrain, cDark, cLight);
            cFinal.Blend(cPerlin);
            cFinal.Save();
            return cFinal;
        }
    }
}
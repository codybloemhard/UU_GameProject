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
                case "_grasstop": return GenGrassTop();
                case "_frostytop": return GenFrostyTop();
                case "_snowytop": return GenSnowyTop();
                case "_frostylayer": return GenFrostyLayer();
                case "_crackedlayer": return GenCrackedLayer();
                //objects
                case "_boulder": return GenBoulder();
                case "_stone": return GenStone();
                case "_snowystone": return GenSnowyStone();
                case "_frostystone": return GenFrostyStone();
                case "_stoneshard": return GenStoneShard();
                case "_cloud": return GenCloud();
                //plants
                case "_bushleaf": return GenBushLeaf();
                case "_berry": return GenBerry();
                case "_bush": return GenBush();
                case "_woodmedium": return GenWoodMedium();
                case "_wooddark": return GenWoodDark();
                case "_woodlight": return GenWoodLight();
                case "_woodburned": return GenWoodBurned();
                case "_woodpalm": return GenWoodPalm();
                case "_greenleafs": return GenGreenLeafs(true);
                case "_greenleafshalf": return GenGreenLeafsHalf();
                case "_greenleafsbig": return GenGreenLeafs(false);
                case "_blossom": return GenBlossom();
                case "_leaf": return GenLeaf(false, false);
                case "_leafhigh": return GenLeaf(false, true);
                case "_leafburned": return GenLeaf(true);
                case "_hangingleaf": return GenHangingLeaf();
                case "_palmleafbody": return GenPalmLeafBody();
                //snowman
                case "_snowmanbody": return GenSnowManBody();
                case "_snowmaneye": return GenSnowManEye();
                case "_snowmanhat": return GenSnowManHat();
                case "_snowmanmouth": return GenSnowManMouth();
                case "_snowmannose": return GenSnowManNose();
                case "_snowmanarmleft": return GenSnowManArm(true);
                case "_snowmanarmright": return GenSnowManArm(false);
                //basics
                case "_greystone": return GenGrayStone();
                case "_snow": return GenSnow();
                case "_grass": return GenGrass();
                case "_dirt": return GenDirt();
                case "_ice": return GenIce();
                case "_coursestone": return GenGenCourseStone();
                case "_sand": return GenSand();
                case "_sandstone": return GenSandStone();
                default: return null;
            }
        }
        //start Blocks
        public static ColourField GenGrassTop()
        {
            const uint size = SMedium;
            ColourField cGrass = GenGrass(size, size/2);
            FloatField fMask = Image.Perlin(size, size/2, 4, 4, 0.5f, 3f);
            FloatField fFade = Image.Fade(size, size/2, false, 0.2f, 1f);
            Image.Multiply(fMask, fFade);
            Image.ToRange(fMask, 0.3f, 1f);
            Image.ThresholdCut(fMask, 0.5f, 0.7f, 0f, 1f);
            cGrass.SetAlpha(fMask);
            return cGrass;
        }

        public static ColourField GenFrostyTop()
        {
            const uint size = SMedium;
            ColourField cIce = GenIce();
            FloatField fMask = Image.Perlin(size, size, 2, 5, 0.5f, 2f);
            Image.ThresholdCut(fMask, 0.0f, 0.4f, 0f, 0f);
            FloatField fSpots = Image.Noise(size, size);
            Image.ThresholdCut(fSpots, 0.65f, 0.7f, 0f, 0f);
            FloatField fTop = Image.Fade(size, size, false, 0.1f, 0.7f);
            Image.ToRange(fTop, 0f, 0.9f);
            fMask = Image.Union(fMask, fSpots);
            fMask = Image.Sum(fMask, fTop);
            cIce.SetAlpha(fMask);
            return cIce;
        }

        public static ColourField GenFrostyLayer()
        {
            const uint size = SMedium;
            ColourField cIce = GenIce();
            FloatField fMask = Image.Perlin(size, size, 2, 5, 0.5f, 2f);
            Image.ThresholdCut(fMask, 0.0f, 0.4f, 0f, 0f);
            FloatField fSpots = Image.Noise(size, size);
            Image.ThresholdCut(fSpots, 0.65f, 0.7f, 0f, 0f);
            fMask = Image.Union(fMask, fSpots);
            cIce.SetAlpha(fMask);
            return cIce;
        }

        public static ColourField GenSnowyTop()
        {
            const uint size = SMedium;
            ColourField cSnow = GenSnow(size, size/2);
            FloatField fMask = Image.Perlin(size, size/2, 4, 4, 0.5f, 3f);
            FloatField fFade = Image.Fade(size, size/2, false, 0.2f, 1f);
            Image.Multiply(fMask, fFade);
            Image.ToRange(fMask, 0.3f, 1f);
            Image.ThresholdCut(fMask, 0.5f, 0.7f, 0f, 1f);
            cSnow.SetAlpha(fMask);
            return cSnow;
        }

        public static ColourField GenCrackedLayer(uint w = SMedium, uint h = SMedium)
        {
            FloatField fMask = Image.VoronoiBlock(w, h, 6, 0.4f, true);
            FloatField fEdge = Image.EdgeFromFlats(fMask);
            ColourField cFinal = new ColourField(w, h);
            Image.ScaleClamp(fEdge, 0.5f);
            cFinal.Fill(Colour.Black);
            cFinal.SetAlpha(fEdge);
            return cFinal;
        }

        //end Blocks
        //start Objects
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
            return cf;
        }

        public static ColourField GenSnowyStone()
        {
            const uint size = SMedium;
            FloatField fMask = GenStoneMask();
            FloatField fEdge = Image.Edge(fMask);
            FloatField fGlow = Image.EdgeToBlendBody(fEdge, fMask, 0.1f, 0.4f, 1.0f);
            FloatField fSnow = Image.Circle(size, size, Image.RandomRange(2.5f, 3f), 1f, 0f, 0.8f, 0.5f, -0.5f);
            fSnow = Image.Intersection(fSnow, fMask);
            ColourField cSnow = GenSnow(size);
            Image.ToRange(fGlow, 0.7f, 1f);
            Image.Multiply(cSnow, fGlow);
            cSnow.SetAlpha(fSnow);
            ColourField cStone = GenGrayStone(size);
            cStone.SetAlpha(fMask);
            Image.Normalize(fGlow, true);
            Image.Multiply(cStone, fGlow);
            cStone.BlendOver(cSnow, 0, 0);
            return cStone;
        }

        public static ColourField GenFrostyStone()
        {
            const uint size = SMedium;
            FloatField fMask = GenStoneMask();
            FloatField fEdge = Image.Edge(fMask);
            FloatField fGlow = Image.EdgeToBlendBody(fEdge, fMask, 0.1f, 0.5f, 1.0f);
            FloatField fPerlin = Image.Perlin(size, size, 1, 4, 0f, 0f);
            ColourField cIce = GenIce();
            ColourField cStone = GenGrayStone();
            cStone.SetAlpha(fMask);
            cIce.SetAlpha(fPerlin);
            Image.CutAlpha(cIce, fMask);
            cStone.BlendOver(cIce, 0, 0);
            Image.Multiply(cStone, fGlow);
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
            return cBody;
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
            return cBody;
        }
        //end Objects
        //start Plants
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
            return bush;
        }

        public static ColourField GenWoodMedium()
        {
            const uint size = 8;
            FloatField fMask = Image.Rectangle(size, size, 0.0f, 0f, 1f, 1f);
            FloatField fGrain = Image.FiniteNoise(size, size, 5, 0.4f, 1f);
            FloatField fExtra = Image.Noise(size, size, 0f, 1f);
            Image.ThresholdCut(fExtra, 0f, 0.05f, 0f, 0f);
            fMask = Image.Intersection(fGrain, fMask);
            fMask = Image.Union(fExtra, fMask);
            ColourField cFinal = new ColourField(size, size);
            Colour cDark = new Colour(0.2f, 0.1f, 0.1f);
            Colour cLight = new Colour(0.2f, 0.1f, 0.1f)*2f;
            cFinal.FloatsToColours(fMask, cDark, cLight);
            return cFinal;
        }

        public static ColourField GenWoodLight()
        {
            const uint size = 8;
            FloatField fMask = Image.Rectangle(size, size, 0.0f, 0f, 1f, 1f);
            FloatField fGrain = Image.FiniteNoise(size, size, 5, 0.4f, 1f);
            FloatField fExtra = Image.Noise(size, size, 0f, 1f);
            Image.ThresholdCut(fExtra, 0f, 0.05f, 0f, 0f);
            fMask = Image.Intersection(fGrain, fMask);
            fMask = Image.Union(fExtra, fMask);
            ColourField cFinal = new ColourField(size, size);
            Colour cDark = new Colour(0.5f, 0.4f, 0.3f);
            Colour cLight = new Colour(0.7f, 0.6f, 0.6f);
            cFinal.FloatsToColours(fMask, cDark, cLight);
            return cFinal;
        }

        public static ColourField GenWoodDark()
        {
            const uint size = 8;
            FloatField fMask = Image.Rectangle(size, size, 0.0f, 0f, 1f, 1f);
            FloatField fGrain = Image.FiniteNoise(size, size, 5, 0.2f, 1f);
            FloatField fExtra = Image.Noise(size, size, 0f, 1f);
            Image.ThresholdCut(fExtra, 0f, 0.05f, 0f, 0f);
            fMask = Image.Intersection(fGrain, fMask);
            fMask = Image.Union(fExtra, fMask);
            ColourField cFinal = new ColourField(size, size);
            Colour cDark = new Colour(0.1f, 0.1f, 0.1f);
            Colour cLight = new Colour(0.2f, 0.1f, 0.1f) * 1.5f;
            cFinal.FloatsToColours(fMask, cDark, cLight);
            return cFinal;
        }

        public static ColourField GenWoodBurned()
        {
            const uint size = 8;
            FloatField fMask = Image.Rectangle(size, size, 0.0f, 0f, 1f, 1f);
            FloatField fGrain = Image.FiniteNoise(size, size, 5, 0.4f, 1f);
            FloatField fExtra = Image.Noise(size, size, 0f, 1f);
            Image.ThresholdCut(fExtra, 0f, 0.05f, 0f, 0f);
            fMask = Image.Intersection(fGrain, fMask);
            fMask = Image.Union(fExtra, fMask);
            ColourField cFinal = new ColourField(size, size);
            Colour cDark = new Colour(0.1f, 0.1f, 0.1f);
            Colour cLight = new Colour(0.2f, 0.2f, 0.2f) * 1.5f;
            cFinal.FloatsToColours(fMask, cDark, cLight);
            return cFinal;
        }

        public static ColourField GenWoodPalm()
        {
            const uint size = 8;
            FloatField fMask = Image.Rectangle(size, size, 0.0f, 0f, 1f, 1f);
            FloatField fGrain = Image.FiniteNoise(size, size, 5, 0.4f, 1f);
            fMask = Image.Intersection(fGrain, fMask);
            FloatField fFade = Image.Fade(size, size, true, 0f, 1f);
            Image.Multiply(fMask, fFade);
            ColourField cFinal = new ColourField(size, size);
            Colour cDark = new Colour(0.2f, 0.1f, 0.1f);
            Colour cLight = new Colour(0.6f, 0.5f, 0.4f) * 1.5f;
            cFinal.FloatsToColours(fMask, cDark, cLight);
            return cFinal;
        }
        
        public static ColourField GenGreenLeafsHalf()
        {
            const uint size = 64;
            FloatField fGrain = Image.Perlin(size, size, 1, 5f*2, 1f, 1f);
            Image.ThresholdCut(fGrain, 0.45f, 0.6f, 0f, 0f);
            FloatField fMask = Image.Circle(size, size, 1f, 1f, 1, 1f, 1f, 0.5f);
            fGrain = Image.Intersection(fGrain, fMask);
            Colour cDark = new Colour(0.45f, 1f, 0.3f) * 0.1f;
            Colour cLight = new Colour(0.6f, 1f, 0.6f) * 1f;
            ColourField cFinal = new ColourField(size, size);
            cFinal.FloatsToColours(fGrain, cDark, cLight);
            cFinal.SetAlpha(0.9f);
            return cFinal;
        }

        public static ColourField GenGreenLeafs(bool small)
        {
            uint size = (uint)(small ? 64 : 128);
            float mul = small ? 2 : 4;
            FloatField fGrain = Image.Perlin(size, size, 1, 5f * mul, 1f, 1f);
            if(small) Image.ThresholdCut(fGrain, 0.45f, 0.6f, 0f, 0f);
            else Image.ThresholdCut(fGrain, 0.5f, 0.6f, 0f, 0f);
            FloatField fMask = Image.Circle(size, size, 1f, 1f, 1, 1f, 0.5f, 0.5f);
            fGrain = Image.Intersection(fGrain, fMask);
            Colour cDark = new Colour(0.45f, 1f, 0.3f) * 0.1f;
            Colour cLight = new Colour(0.6f, 1f, 0.6f) * 1f;
            ColourField cFinal = new ColourField(size, size);
            cFinal.FloatsToColours(fGrain, cDark, cLight);
            cFinal.SetAlpha(0.8f);
            return cFinal;
        }

        public static ColourField GenBlossom()
        {
            const uint size = 8;
            FloatField fMask = Image.VoronoiCell(size, size, 6, 0.8f, 0.3f);
            FloatField fGrain = Image.Noise(size, size, 0.3f, 1f);
            fMask = Image.Intersection(fGrain, fMask);
            ColourField cFinal = new ColourField(size, size);
            Colour cDark = new Colour(0.2f, 0.1f, 0.3f);
            Colour cLight = new Colour(8f, 0.4f, 0.7f);
            cFinal.FloatsToColours(fMask, cDark, cLight);
            return cFinal;
        }

        public static ColourField GenLeaf(bool burned = false, bool highres = false)
        {
            uint size = (uint)(highres ? 32 : 16);
            Vector2 mid = new Vector2(0.5f);
            List<Vector2> constr = new List<Vector2>();
            constr.Add(new Vector2(0f, 0.5f));
            constr.Add(new Vector2(0.1f, 0.4f));
            constr.Add(new Vector2(0.2f, 0.3f));
            constr.Add(new Vector2(0.4f, 0.2f));
            constr.Add(new Vector2(0.6f, 0.1f));
            constr.Add(new Vector2(0.7f, 0.15f));
            constr.Add(new Vector2(0.8f, 0.2f));
            constr.Add(new Vector2(0.9f, 0.3f));
            constr.Add(new Vector2(1f, 0.5f));
            for (int i = 1; i < constr.Count - 1; i++)
            {
                float diff = 0.1f;
                float xx = ((float)MathH.random.NextDouble() - 0.5f) * diff;
                float yy = ((float)MathH.random.NextDouble() - 0.5f) * diff;
                constr[i] += new Vector2(xx, yy);
            }
            for (int i = constr.Count - 1; i > 0; i--)
            {
                Vector2 cur = constr[i];
                constr.Add(new Vector2(cur.X, 1f - cur.Y));
            }
            FloatField fMask = Image.TriangleFan(size, size, mid, constr.ToArray());
            FloatField fNoise = Image.FiniteNoise(size, size, 5, 0.3f, 1f);
            fMask = Image.Intersection(fNoise, fMask);
            ColourField cRod = new ColourField(size, 1);
            ColourField cFinal = new ColourField(size, size);
            Colour cDark, cLight;
            if (!burned)
            {
                cDark = new Colour(0.2f, 0.5f, 0.1f);
                cLight = new Colour(0.3f, 0.8f, 0.2f);
            }
            else
            {
                cDark = new Colour(0.13f, 0.15f, 0.14f);
                cLight = new Colour(0.23f, 0.25f, 0.3f);
            }
            cFinal.FloatsToColours(fMask, cDark, cLight);
            cRod.Fill(cDark*0.9f);
            cFinal.DrawOver(cRod, 0, size/2);
            return cFinal;
        }

        public static ColourField GenHangingLeaf()
        {
            const uint ww = 32, hh = 8;
            FloatField fMask0 = Image.RandomWalk(ww, hh, 0f, 0.5f, 64, new Vector2(2f, 0f));
            FloatField fMask1 = Image.RandomWalk(ww, hh, 0f, 0.5f, 64, new Vector2(2f, 0f));
            ColourField cFinal = new ColourField(ww, hh);
            ColourField cDetail = new ColourField(ww, hh);
            Colour cDark = new Colour(0.5f, 0.5f, 0.2f);
            Colour cLight = new Colour(0.7f, 0.8f, 0.2f);
            cFinal.FloatsToColours(fMask0, cLight, cLight);
            cDetail.FloatsToColours(fMask1, cDark, cDark);
            cFinal.DrawOver(cDetail, 0, 0);
            return cFinal;
        }

        public static ColourField GenPalmLeafBody()
        {
            const uint size = 32;
            FloatField fMiddle = Image.RandomWalk(size, size, 0f, 0.5f, 64, new Vector2(10, 0));
            Colour cDark = new Colour(0.2f, 0.3f, 0.2f);
            Colour cLight = new Colour(0.3f, 0.5f, 0.2f);
            ColourField cFinal = new ColourField(size, size);
            cFinal.FloatsToColours(fMiddle, cDark, cDark);
            ColourField cTwig = new ColourField(size, size);
            FloatField fTwig = new FloatField(size, size);
            for (int i = 0; i < 8; i++)
            {
                fTwig = Image.RandomWalk(size, size, (float)i / 7, 0.5f, 12, new Vector2(-2, +1));
                cTwig.FloatsToColours(fTwig, cLight, cLight);
                cFinal.DrawOver(cTwig, 0, 0);
                fTwig = Image.RandomWalk(size, size, (float)i / 7, 0.5f, 12, new Vector2(-2, -1));
                cTwig.FloatsToColours(fTwig, cLight, cLight);
                cFinal.DrawOver(cTwig, 0, 0);
            }
            return cFinal;
        }

        //end Plants
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
            return cSnow;
        }
        //end Snowman
        //raw materials
        public static ColourField GenSnow(uint w = SMedium, uint h = SMedium)
        {
            FloatField fGrain = Image.Noise(w, h);
            ColourField cFinal = new ColourField(w, h);
            Colour cDark = new Colour(0.65f, 0.65f, 0.7f);
            Colour cLight = new Colour(0.95f, 0.95f, 0.95f);
            cFinal.FloatsToColours(fGrain, cDark, cLight);
            fGrain = Image.Perlin(w, h, 2, 2, 0.5f, 2f);
            ColourField cPerlin = new ColourField(w, h);
            cPerlin.FloatsToColours(fGrain, cDark, cLight);
            cFinal.Blend(cPerlin);
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
            return cf;
        }

        public static ColourField GenGenCourseStone(uint w = SMedium, uint h = SMedium)
        {
            FloatField fPerlin = Image.Perlin(w, h, 2, 10, 0.5, 3f);
            Image.ToRange(fPerlin, 0.3f, 1.0f);
            ColourField cf = new ColourField(w, h);
            Colour cDark = new Colour(0.2f, 0.15f, 0.15f) * 0.5f;
            Colour clight = new Colour(0.5f, 0.4f, 0.4f);
            cf.FloatsToColours(fPerlin, cDark, clight);
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
            return cFinal;
        }

        public static ColourField GenIce(uint w = SMedium, uint h = SMedium)
        {
            FloatField fGrain = Image.Noise(w, h);
            Image.ThresholdCut(fGrain, 0.9f, 1f, 1f, 0f);
            Colour cDark = new Colour(0.8f, 0.9f, 0.9f);
            Colour cLight = new Colour(0.3f, 0.4f, 0.5f);
            ColourField cFinal = new ColourField(w, h);
            cFinal.FloatsToColours(fGrain, cDark, cLight);
            return cFinal;
        }

        public static ColourField GenSand(uint w = SMedium, uint h = SMedium)
        {
            FloatField fGrain = Image.FiniteNoise(w, h, 5, 0.3f, 1f);
            FloatField fDetail = Image.Noise(w, h, 0f, 1f);
            Image.ThresholdCut(fDetail, 0f, 0.02f, 1f, 0f);
            FloatField fResult = Image.Union(fDetail, fGrain);
            Colour cDark = new Colour(0.4f, 0.3f, 0.1f);
            Colour cLight = new Colour(0.7f, 0.6f, 0.1f);
            ColourField cFinal = new ColourField(w, h);
            cFinal.FloatsToColours(fResult, cDark, cLight);
            return cFinal;
        }

        public static ColourField GenSandStone(uint w = SMedium, uint h = SMedium)
        {
            FloatField fGrain = Image.FiniteNoise(w, h, 10, 0.6f, 1f);
            Colour cDark = new Colour(0.4f, 0.3f, 0.1f);
            Colour cLight = new Colour(0.7f, 0.6f, 0.1f);
            ColourField cFinal = new ColourField(w, h);
            cFinal.FloatsToColours(fGrain, cDark, cLight);
            return cFinal;
        }
    }
}
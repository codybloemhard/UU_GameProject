using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    //this class is to make sure the same blocks etc are loaded on the same position everytime
    public static class Seed
    {
        private static Dictionary<string, Random> randoms = new Dictionary<string, Random>();
        private static int baseseed;

        static Seed()
        {
            randoms = new Dictionary<string, Random>();
            System.Random r = new System.Random();
            baseseed = r.Next() / 1000;
        }

        public static int GetSeed(Vector2 pos)
        {
            int x = (int)(pos.X * 3837364f) ^ 12;
            int y = (int)(pos.Y * 3837364f) ^ 12;
            return (x * y + x + y);
        }

        public static void Set(Vector2 pos)
        {
            lock (randoms)
            {
                int seed = GetSeed(pos);
                string key = "" + seed;
                if (randoms.ContainsKey(key))
                    randoms[key] = new Random(seed + baseseed);
                else
                    randoms.Add(key, new Random(seed + baseseed));
            }
        }
        
        public static void Set(int seed)
        {
            lock (randoms)
            {
                string key = "" + seed;
                if (randoms.ContainsKey(key))
                    randoms[key] = new Random(seed + baseseed);
                else
                    randoms.Add(key, new Random(seed + baseseed));
            }
        }

        public static float Random(Vector2 pos)
        {
            return Random(GetSeed(pos));
        }

        public static float Random(int seed)
        {
            lock (randoms)
            {
                string key = "" + seed;
                if (!randoms.ContainsKey(key))
                {
                    Set(seed);
                    return (float)randoms[key].NextDouble();
                }
                return (float)randoms[key].NextDouble();
            }
        }

        public static float RandomRange(float a, float b, Vector2 pos)
        {
            return Image.Lerp(a, b, Random(pos));
        }

        public static float RandomDeviation(float x, float t, Vector2 pos)
        {
            return Image.Lerp(x - t, x + t, Random(pos));
        }

        public static float RandomRange(float a, float b, int seed)
        {
            return Image.Lerp(a, b, Random(seed));
        }

        public static float RandomDeviation(float x, float t, int seed)
        {
            return Image.Lerp(x - t, x + t, Random(seed));
        }
    }
}
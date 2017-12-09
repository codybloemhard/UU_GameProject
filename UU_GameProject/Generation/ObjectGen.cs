using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;

namespace UU_GameProject
{
    public static class ObjectGen
    {
        public static GameObject GenBush(GameState context)
        {
            GameObject obj = new GameObject("genobj", context, 0);
            for(int i = 0; i < 3; i++)
            {
                GameObject leaf = new GameObject("genobj", context, 5);
                int k = MathH.random.Next();
                leaf.AddComponent(new CRender("_bushleaf" + (k % 8)));
                leaf.Size = new Vector2(1f, 1f);
                float x = 0.2f + (float)MathH.random.NextDouble() * 0.4f;
                float y = 0.2f + (float)MathH.random.NextDouble() * 0.3f;
                leaf.Pos = new Vector2(x, y);
                for (int j = 0; j < 3; j++)
                {
                    GameObject berry = new GameObject("genobj", context, 4);
                    k = MathH.random.Next();
                    berry.AddComponent(new CRender("_berry" + (k % 8)));
                    float xx = x + leaf.Size.X / 2;
                    float yy = y + leaf.Size.Y / 2;
                    xx += ((float)MathH.random.NextDouble() - 0.5f) * leaf.Size.X / 2f;
                    yy += ((float)MathH.random.NextDouble() - 0.5f) * leaf.Size.Y / 2f;
                    berry.Pos = new Vector2(xx, yy);
                    berry.Size = new Vector2(0.1f, 0.1f);
                }
            }
            obj.Size = new Vector2(1f, 1f);
            return obj;
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;

namespace UU_GameProject
{
    public class PrerunGenerationCycle
    {
        public PrerunGenerationCycle() { }

        public void GenTest()
        {
            for(int i = 0; i < 8; i++)
                TextureManager.LoadTexture("_berry" + i, TextureGen.GenBerry());
            for (int i = 0; i < 8; i++)
                TextureManager.LoadTexture("_bushleaf" + i, TextureGen.GenBushLeaf());
            for (int i = 0; i < 4; i++)
                TextureManager.LoadTexture("_boulder" + i, TextureGen.GenBoulder());
            for (int i = 0; i < 16; i++)
                TextureManager.LoadTexture("_stone" + i, TextureGen.GenStone());
            for (int i = 0; i < 16; i++)
                TextureManager.LoadTexture("_stoneshard" + i, TextureGen.GenStoneShard());
            for (int i = 0; i < 4; i++)
                TextureManager.LoadTexture("_cloud" + i, TextureGen.GenCloud());
            GC.Collect();
        }
    }
}
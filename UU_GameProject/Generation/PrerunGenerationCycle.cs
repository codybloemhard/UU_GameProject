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
            PTimer timer = new PTimer();
            timer.GetElapsedTime();
            GenerateTexture(8, "_grass");
            GenerateTexture(8, "_dirt");
            GenerateTexture(8, "_snow");
            GenerateTexture(8, "_ice");

            GenerateTexture(8, "_frostydirt");
            GenerateTexture(8, "_dirtgrassblock");
            GenerateTexture(8, "_bush");
            GenerateTexture(4, "_boulder");
            GenerateTexture(16, "_stone");
            GenerateTexture(32, "_stoneshard");
            GenerateTexture(16, "_snowystone");
            GenerateTexture(4, "_cloud");
            GenerateTexture(8, "_snowmanbody");
            GenerateTexture(8, "_snowmaneye");
            GenerateTexture(8, "_snowmanhat");
            GenerateTexture(8, "_snowmanmouth");
            GenerateTexture(8, "_snowmannose");
            GenerateTexture(4, "_snowmanarmleft");
            GenerateTexture(4, "_snowmanarmright");
            double elap = timer.GetElapsedTime();
            Console.WriteLine("Generation: " + elap + " Seconds.");
            GC.Collect();
        }

        public void GenerateTexture(uint n, string name)
        {
            for (int i = 0; i < n; i++)
                TextureManager.LoadTexture(name + i, TextureGen.Gen(name).Texture);
            Catalog.Registertexture(name, n);
        }
    }
}
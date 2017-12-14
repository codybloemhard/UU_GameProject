using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core;
using System.Threading;
namespace UU_GameProject
{
    public class PrerunGenerationCycle
    {
        public PrerunGenerationCycle() { }

        public void GenTest()
        {
            PTimer timer = new PTimer();
            timer.GetElapsedTime();

            /*Thread a = new Thread(new ThreadStart(A));
            Thread b = new Thread(new ThreadStart(B));
            Thread c = new Thread(new ThreadStart(C));
            a.Start(); b.Start(); c.Start();
            a.Join();
            b.Join();
            c.Join();*/
            A();
            B();
            C();
            double elap = timer.GetElapsedTime();
            Console.WriteLine("Generation: " + elap + " Seconds.");
            GC.Collect();
        }

        private void A()
        {
            GenerateTexture(8, "_grass");
            GenerateTexture(8, "_dirt");
            GenerateTexture(8, "_snow");
            GenerateTexture(8, "_ice");
            GenerateTexture(8, "_snowydirt");
            GenerateTexture(8, "_frostydirt");
            GenerateTexture(8, "_dirtgrassblock");
        }

        private void B()
        {
            GenerateTexture(16, "_bush");
            GenerateTexture(8, "_boulder");
            GenerateTexture(16, "_stone");
            GenerateTexture(16, "_stoneshard");
            GenerateTexture(8, "_frostystone");
            GenerateTexture(16, "_snowystone");
            GenerateTexture(8, "_cloud");
        }

        private void C()
        {
            GenerateTexture(16, "_snowmanbody");
            GenerateTexture(16, "_snowmaneye");
            GenerateTexture(16, "_snowmanhat");
            GenerateTexture(16, "_snowmanmouth");
            GenerateTexture(16, "_snowmannose");
            GenerateTexture(16, "_snowmanarmleft");
            GenerateTexture(16, "_snowmanarmright");
        }

        public void GenerateTexture(uint n, string name)
        {
            for (int i = 0; i < n; i++)
            {
                ColourField cf = TextureGen.Gen(name);
                cf.Save();
                TextureManager.LoadTexture(name + i, cf.Texture);
            }
            Catalog.Registertexture(name, n);
        }
    }
}
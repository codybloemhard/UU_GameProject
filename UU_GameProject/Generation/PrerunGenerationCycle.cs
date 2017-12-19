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
            Thread d = new Thread(new ThreadStart(D));
            a.Start(); b.Start(); c.Start(); d.Start();
            a.Join();
            b.Join();
            c.Join();
            d.Join();*/
            A();
            B();
            C();
            D();
            double elap = timer.GetElapsedTime();
            Console.WriteLine("Generation: " + elap + " Seconds.");
            GC.Collect();
        }

        private void A()
        {
            PTimer timer = new PTimer();
            timer.GetElapsedTime();
            GenerateTexture(4, "_dirt");
            GenerateTexture(4, "_ice");
            GenerateTexture(4, "_coursestone");
            GenerateTexture(8, "_snowytop");
            GenerateTexture(8, "_frostylayer");
            GenerateTexture(8, "_frostytop");
            GenerateTexture(8, "_grasstop");
            GenerateTexture(8, "_crackedlayer");
            Console.WriteLine("A: " + timer.GetElapsedTime() + " Seconds.");
        }

        private void B()
        {
            PTimer timer = new PTimer();
            timer.GetElapsedTime();
            GenerateTexture(8, "_boulder");
            GenerateTexture(8, "_cloud");
            Console.WriteLine("B: " + timer.GetElapsedTime() + " Seconds.");
        }

        private void C()
        {
            PTimer timer = new PTimer();
            timer.GetElapsedTime();
            GenerateTexture(16, "_bush");
            GenerateTexture(16, "_stone");
            GenerateTexture(16, "_stoneshard");
            GenerateTexture(8, "_frostystone");
            GenerateTexture(16, "_snowystone");
            Console.WriteLine("C: " + timer.GetElapsedTime() + " Seconds.");
        }

        private void D()
        {
            PTimer timer = new PTimer();
            timer.GetElapsedTime();
            GenerateTexture(16, "_snowmanbody");
            GenerateTexture(16, "_snowmaneye");
            GenerateTexture(16, "_snowmanhat");
            GenerateTexture(16, "_snowmanmouth");
            GenerateTexture(16, "_snowmannose");
            GenerateTexture(16, "_snowmanarmleft");
            GenerateTexture(16, "_snowmanarmright");
            Console.WriteLine("D: " + timer.GetElapsedTime() + " Seconds.");
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
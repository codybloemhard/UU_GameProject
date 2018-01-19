using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Core;

namespace UU_GameProject
{
    public class CGrowingDoorSprite : Component
    {
        private int growing;
        private float speed = 10f;
        private float height = 1f;
        private float width = 1f;
        private Vector2 startpos;

        public CGrowingDoorSprite(float height) : base()
        {
            this.height = height;
        }

        public override void Init()
        {
            base.Init();
            growing = 0;
            startpos = GO.Pos;
            width = GO.Size.X;
            GO.Size = new Vector2(width, 0f);
        }

        public override void Update(float time)
        {
            base.Update(time);
            if(growing == 1)
            {
                GO.Size += Vector2.UnitY * time * speed;
                if(GO.Size.Y > height)
                {
                    GO.Size = new Vector2(width, height);
                    growing = 2;
                    GO.Renderer.active = true;
                }
                GO.Pos = startpos - new Vector2(0, GO.Size.Y);
            }
            else if (growing == -1)
            {
                GO.Size -= Vector2.UnitY * time * speed;
                if (GO.Size.Y < 0)
                {
                    GO.Size = new Vector2(width, 0);
                    growing = -2;
                    GO.Renderer.active = false;
                }
                GO.Pos = startpos - new Vector2(0, GO.Size.Y);
            }
        }

        public void Grow()
        {
            if (growing > 0) return;
            growing = 1;
        }

        public void Shrink()
        {
            if (growing < 0) return;
            growing = -1;
        }
    }
}
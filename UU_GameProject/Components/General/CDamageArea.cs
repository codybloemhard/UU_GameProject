﻿using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CDamageArea : Component
    {
        private float duration;
        private string caller;
        private Vector2 dimensions;
        private Vector2 dir;

        public CDamageArea(Vector2 dir, Vector2 dimensions, float duration, string caller) : base()
        {
            this.duration = duration;
            this.caller = caller;
            this.dimensions = dimensions;
            this.dir = dir;
        }

        public override void Init()
        {
            CRender render = GO.Renderer as CRender;
            if (render != null) render.colour = Color.Red;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (dir.X > 0)
                GO.Pos = GO.FindWithTag(caller).Pos + new Vector2(GO.FindWithTag(caller).Size.X / 2f, 0);
            else
                GO.Pos = GO.FindWithTag(caller).Pos + new Vector2(GO.FindWithTag(caller).Size.X / 2f - GO.Size.X, 0);
            Timers.Add("DamageAreaLifespan", duration, Destroy);
        }

        public override void OnCollision(GameObject other)
        {
            
        }

        public void Destroy()
        {
            GO.Destroy();
        }
    }
}
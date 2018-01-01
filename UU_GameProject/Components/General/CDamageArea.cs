using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CDamageArea : Component
    {
        private float duration;
        private string caller;
        private Vector2 dimensions;//never used?
        private Vector2 dir;

        public CDamageArea(Vector2 dir, float duration, string caller) : base()
        {
            this.duration = duration;
            this.caller = caller;
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

        public void Destroy()
        {
            GO.Destroy();
        }
    }
}
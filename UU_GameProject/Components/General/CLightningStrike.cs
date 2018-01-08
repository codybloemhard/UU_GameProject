using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CLightningStrike : Component
    {
        private float duration;
        private string caller;
        private Vector2 dimensions;
        private Vector2 dir;

        public CLightningStrike(float duration, string caller) : base()
        {
            this.duration = duration;
            this.caller = caller;
        }

        public override void Init()
        {
            CRender render = GO.Renderer as CRender;
            if (render != null) render.colour = Color.Red;
        }

        public override void Update(float time)
        {
            base.Update(time);
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
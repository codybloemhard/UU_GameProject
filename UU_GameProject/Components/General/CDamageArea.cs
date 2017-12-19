using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CDamageArea : Component
    {
        private float duration;

        public CDamageArea(float duration) : base()
        {
            this.duration = duration;
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
            //add damage receiving here
        }

        public void Destroy()
        {
            GO.Destroy();
        }
    }
}
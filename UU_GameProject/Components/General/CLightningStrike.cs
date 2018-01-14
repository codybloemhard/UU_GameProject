using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CLightningStrike : CDamageDealer
    {
        private float duration;
        private string caller;

        public CLightningStrike(float duration, string caller, float damage, bool potionous) : base(damage, potionous)
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

        public override void OnCollision(GameObject other) { }

        public void Destroy()
        {
            GO.Destroy();
        }
    }
}
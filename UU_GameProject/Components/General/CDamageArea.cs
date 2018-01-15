using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CDamageArea : CDamageDealer
    {
        private float duration;
        private string caller;
        private Vector2 dir;
        private bool iniated = false;

        public CDamageArea(Vector2 dir, float duration, string caller, float damage, bool potionous) : base(damage, potionous)
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

        public void Set()
        {
            iniated = true;
            GameObject obj = GO.FindWithTag(caller);
            if (obj == null) return;
            if (dir.X > 0)
                GO.Pos = obj.Pos + new Vector2(obj.Size.X / 2f, 0);
            else
                GO.Pos = obj.Pos + new Vector2(obj.Size.X / 2f - GO.Size.X, 0);
            Timers.Add("DamageAreaLifespan", duration, () => GO.Destroy());
            AudioManager.PlayEffect("melee");
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!iniated) Set();
        }
    }
}
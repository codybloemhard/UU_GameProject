using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CBulletMovement : CDamageDealer
    {
        private float speed;
        private Vector2 dir;
        private float time = 0f;

        public CBulletMovement(float speed, Vector2 dir, float damage, bool potionous) : base(damage, potionous)
        {
            this.speed = speed;
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
            GO.Pos += dir * speed * time;
            this.time = time;
            if (this.time > 10f) GO.Destroy();
        }

        //checks for despawning the bullets as soon as they hit solid surface
        public override void OnCollision(GameObject other)
        {
            if (other.tag.Contains("solid"))
                GO.Destroy();
        }
    }
}
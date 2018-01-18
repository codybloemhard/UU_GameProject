using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CFireballMovement : CDamageDealer
    {
        private Vector2 playerSpeed;
        private Vector2 dir;
        private Vector2 fireballVelocity;
        private float fireballTotalSpeed = 6f;
        private float time;

        public CFireballMovement(Vector2 playerSpeed, Vector2 path, Vector2 dir, float damage, bool potionous) : base(damage, potionous)
        {
            this.dir = dir;
            this.playerSpeed = playerSpeed;
            fireballVelocity.X = (fireballTotalSpeed * (path.X / (Math.Abs(path.X) + Math.Abs(path.Y)))) + playerSpeed.X;
            fireballVelocity.Y = (fireballTotalSpeed * (path.Y / (Math.Abs(path.X) + Math.Abs(path.Y)))) + playerSpeed.Y;
        }

        public override void Init()
        {
            CRender render = GO.Renderer as CRender;
            if (render != null) render.colour = Color.Red;
        }

        public override void Update(float time)
        {
            base.Update(time);
            GO.Pos += fireballVelocity * time;
            this.time += time;
            if (this.time > 10f) GO.Destroy();
        }

        public override void OnCollision(GameObject other)
        {
            if (other.tag == "solid")
                GO.Destroy();
        }
    }
}
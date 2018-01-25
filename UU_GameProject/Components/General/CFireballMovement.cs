using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CFireballMovement : CDamageDealer
    {
        private Vector2 playerSpeed;
        private Vector2 fireballVelocity;
        private float fireballTotalSpeed;
        private float time;

        public CFireballMovement(Vector2 playerSpeed, Vector2 path, Vector2 dir, float damage, bool potionous, float speed = 6) : base(damage, potionous)
        {
            this.playerSpeed = playerSpeed;
            path.Normalize();
            fireballTotalSpeed = speed;
            fireballVelocity = path * fireballTotalSpeed;
        }

        public override void Init()
        {
            CRender render = GO.Renderer as CRender;
            if (render != null) render.colour = Color.Red;
            double angle = Math.Atan2(fireballVelocity.Y, fireballVelocity.X);
            GO.Renderer.SetRotation((float)angle * MathH.RAD_TO_DEG);
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
            if (other.tag.Contains("solid") && other.tag != "bossdoorsolid")
                GO.Destroy();
        }
    }
}
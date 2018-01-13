using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CFireballMovement : Component
    {
        private Vector2 playerSpeed;
        private Vector2 dir;
        private Vector2 fireballVelocity;
        private float fireballTotalSpeed = 6;
        public float Damage { get; private set; }

        public CFireballMovement(Vector2 playerSpeed, Vector2 path, Vector2 dir, float damage) : base()
        {
            this.dir = dir;
            this.playerSpeed = playerSpeed;
            fireballVelocity.X = (fireballTotalSpeed * (path.X / (Math.Abs(path.X) + Math.Abs(path.Y)))) + playerSpeed.X;
            fireballVelocity.Y = (fireballTotalSpeed * (path.Y / (Math.Abs(path.X) + Math.Abs(path.Y)))) + playerSpeed.Y;
            Damage = damage;
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
            if (GO.Pos.X < 0 || GO.Pos.X > 16
                || GO.Pos.Y < 0 || GO.Pos.Y > 9)
                GO.Destroy();
        }

        public override void OnCollision(GameObject other)
        {
            if (other.tag == "stone")
                GO.Destroy();
        }

        public Vector2 direction()
        {
            return dir;
        }
    }
}
using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CFireballMovement : Component
    {
        private Vector2 playerSpeed;
        private float angle;
        private Vector2 dir;
        private Vector2 fireballVelocity;
        private float fireballTotalSpeed = 6;
        private int xDir;
        private int yDir;

        public CFireballMovement(Vector2 playerSpeed, Vector2 path, Vector2 dir) : base()
        {
            this.dir = dir;
            this.playerSpeed = playerSpeed;
            angle = path.Y / path.X;
            if (path.X < 0)
                xDir = -1;
            else xDir = 1;
            if (path.Y <= 0)
                yDir = -1;
            else yDir = 1;
            fireballVelocity.X = (float)Math.Sqrt((fireballTotalSpeed * fireballTotalSpeed) / (1 + angle * angle));
            fireballVelocity.Y = fireballTotalSpeed - fireballVelocity.X;
            fireballVelocity.X *= xDir;
            fireballVelocity.Y *= yDir;
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
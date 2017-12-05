using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    class CEnemyMovement : Component
    {
        private float speed;
        private bool grounded;
        private float gravity = 1, vertVelo = 0f;

        public CEnemyMovement(float speed)
        {
            this.speed = speed;
        }
        public override void Init()
        {
            CRender render = GO.Renderer;
            if (render != null) render.colour = Color.Red;
        }
        
        public override void Update(float time)
        {
            base.Update(time);

            Vector2 feetLeft = GO.Pos + new Vector2(0, GO.Size.Y + 0.01f);
            Vector2 feetRight = GO.Pos + new Vector2(GO.Size.X, GO.Size.Y + 0.01f);
            RaycastResult hitLeft = GO.Raycast(feetLeft, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hitRight = GO.Raycast(feetRight, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hit;
            if (hitLeft.distance > hitRight.distance) hit = hitRight;
            else hit = hitLeft;

            if (hit.hit && hit.distance < 0.05f)
            { 
                grounded = true;
            }
            else grounded = false;

            if (grounded && (hitLeft.distance > 0.05f || hitRight.distance > 0.05f))
            {
                speed *= -1;
            }

            if (!grounded) vertVelo += gravity * time;
            GO.Pos += new Vector2(speed * time, Math.Min(hit.distance, vertVelo * time));
        }

        public override void OnCollision(GameObject other)
        {
            if (other.tag == "player")
            {
                other.Pos = new Vector2(1, 1);
            }

            else if (other.tag == "bullet")
            {
                GO.active = false;
            }
        }
    }
}

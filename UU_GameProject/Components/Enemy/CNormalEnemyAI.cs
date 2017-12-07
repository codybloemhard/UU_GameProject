using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    class CNormalEnemyAI : Component
    {
        private float speed;
        private bool grounded;
        private float gravity = 0.8f, vertVelo = 0f;

        public CNormalEnemyAI(float speed)
        {
            this.speed = speed;
        }

        public override void Init()
        {
            CRender render = GO.Renderer as CRender;
            if (render != null) render.colour = Color.Red;
        }
        
        public override void Update(float time)
        {
            base.Update(time);

            //Movement behaviour
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

        //Damage handling
        public override void OnCollision(GameObject other)
        {
            if (other.tag == "bullet")
            {
                CHealthBar health = GO.FindWithTag("enemy").GetComponent<CHealthBar>();
                health.hit(1);
                if (health.hp <= 0)
                    GO.active = false;
            }
        }
    }
}

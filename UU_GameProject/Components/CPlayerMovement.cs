using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    public class CPlayerMovement : Component
    {
        private float speed;
        private float maxPlayerSpeed = 2.0f;
        private Vector2 dir;
        private float jumpPower = 15f;
        private float acceleration = 0.8f, vertVelo = 0f;
        private bool grounded = false;
        private bool isCrouching = false;
        Vector2 velocity = Vector2.Zero;

        public CPlayerMovement(float speed) : base()
        {
            this.speed = speed;
            dir = new Vector2(1, 0);
        }

        public override void Init()
        {
            CRender render = GO.Renderer;
            if (render != null) render.colour = Color.White;
        }

        public override void Update(float time)
        {
            //slowly accelerates the player
            if (Input.GetKey(PressAction.DOWN, Keys.D) && velocity.X <= maxPlayerSpeed)
                velocity += new Vector2(0.1f, 0);
            if (Input.GetKey(PressAction.DOWN, Keys.A) && velocity.X >= -maxPlayerSpeed)
                velocity += new Vector2(-0.1f, 0);
            //stops the player if no buttons are pressed
            if (!Input.GetKey(PressAction.DOWN, Keys.D) && velocity.X > 0 && grounded)
                velocity -= new Vector2(Math.Min(0.2f, velocity.X), 0);
            if (!Input.GetKey(PressAction.DOWN, Keys.A) && velocity.X < 0 && grounded)
                velocity -= new Vector2(Math.Max(-0.2f, velocity.X), 0);
            if (GO.Pos.Y > 9) GO.Pos = new Vector2(1, -1);
            if (velocity != Vector2.Zero)
            {
                dir = velocity;
                dir.Normalize();
            }

            //crouching
            if (Input.GetKey(PressAction.DOWN, Keys.LeftShift) && grounded)
            {
                maxPlayerSpeed = 0.5f;
                isCrouching = true;
            }
            if (Input.GetKey(PressAction.RELEASED, Keys.LeftShift))
            {
                maxPlayerSpeed = 2.0f;
                isCrouching = false;
            }

            //sliding
            if (isCrouching && velocity.X > 0 && velocity.X > maxPlayerSpeed)
            {
                velocity -= new Vector2(Math.Min(0.03f, velocity.X), 0);
            }
            if (isCrouching && velocity.X < 0 && velocity.X < -maxPlayerSpeed)
            {
                velocity -= new Vector2(Math.Max(-0.03f, velocity.X), 0);
            }

            //gravity and jump
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
            if (grounded && Input.GetKey(PressAction.PRESSED, Keys.W) || grounded && Input.GetKey(PressAction.PRESSED, Keys.Space))
                vertVelo = -jumpPower;
            if (!grounded) vertVelo += acceleration;
            //speed is in Units/Second
            GO.Pos += velocity * speed * time;
            GO.Pos += new Vector2(0, Math.Min(hit.distance, vertVelo * time));
            //shoot
            if (Input.GetKey(PressAction.PRESSED, Keys.F))
                GO.GetComponent<CShoot>().Shoot(dir, new Vector2(0.2f, 0.2f), velocity);
        }

        public override void OnCollision(GameObject other)
        {
            if (other.tag == "killer")
                Reset();
        }

        public void Reset()
        {
            GO.Pos = new Vector2(1, 1);
            velocity = new Vector2(0, 0);
            vertVelo = 0;
        }
    }
}
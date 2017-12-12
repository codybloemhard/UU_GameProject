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
        private float intendedDir;
        private float jumpPower = 15f;
        private float acceleration = 0.8f, vertVelo = 0f;
        private float playerAccel = .1f;
        private float jumpDelayTime = 0;
        private float dashToggleDelayTime = 0;
        private float dashSlowdownDelayTime = 0;
        private float lastVertVelo;
        private float maxDashSpeed = 4;
        private bool isDashing = false;
        private bool fallPanic = false;
        private bool grounded = false;
        private bool isCrawling = false;
        private bool isCrouching = false;
        private bool isSliding = false;
        private bool isDown = false;
        Vector2 velocity = Vector2.Zero;

        public CPlayerMovement(float speed) : base()
        {
            this.speed = speed;
            dir = new Vector2(1, 0);
        }

        public override void Init()
        {
            CRender render = GO.Renderer as CRender;
            if (render != null) render.colour = Color.White;
        }

        public override void Update(float time)
        {

            //basic movement: slowly accelerates the player
            if (Input.GetKey(PressAction.DOWN, Keys.D) && velocity.X + playerAccel <= maxPlayerSpeed)
                velocity += new Vector2(playerAccel, 0);
            if (Input.GetKey(PressAction.DOWN, Keys.A) && velocity.X - playerAccel >= -maxPlayerSpeed)
                velocity += new Vector2(-playerAccel, 0);
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
            if (Input.GetKey(PressAction.DOWN, Keys.D))
                intendedDir = 1;
            if (Input.GetKey(PressAction.DOWN, Keys.A))
                intendedDir = -1;

            //down
                if (Input.GetKey(PressAction.DOWN, Keys.S) && grounded)
            {
                maxPlayerSpeed = 0.5f;
                isDown = true;
            }
            if (Input.GetKey(PressAction.RELEASED, Keys.S))
            {
                maxPlayerSpeed = 2.0f;
                isDown = false;
            }

            //crouching
            if (isDown && velocity.X == 0)
                isCrouching = true;
            else isCrouching = false;

            //crawling
            if (isDown && ((velocity.X > 0 && velocity.X <= maxPlayerSpeed) || (velocity.X < 0 && velocity.X >= -maxPlayerSpeed)))
                isCrawling = true;
            else isCrawling = false;

            //sliding forward
            if (isDown && velocity.X > maxPlayerSpeed)
            {
                velocity.X = Math.Max(maxPlayerSpeed, velocity.X - .03f);
                isSliding = true;
            } //sliding backward
            else if (isDown && velocity.X < -maxPlayerSpeed)
            {
                velocity.X = Math.Min(maxPlayerSpeed, velocity.X + .03f);
                isSliding = true;
            } //not sliding
            else isSliding = false;

            //fall panic
            if (vertVelo > 25 || lastVertVelo > 25)
            {
                fallPanic = true;
                //fall damage
                if (grounded)
                {
                    GO.GetComponent<CHealthPool>().ChangeHealth((int)lastVertVelo - 25);
                }
                lastVertVelo = vertVelo;
            }
            else fallPanic = false;

            //Dashing
            //turning dashing state on
            if (Input.GetKey(PressAction.PRESSED, Keys.LeftShift) && Math.Abs(velocity.X) <= maxDashSpeed && isDashing == false)
            {
                isDashing = true;
                dashToggleDelayTime = 0;
            }
            dashToggleDelayTime += time;

            //turning dashing state off
            if ((Math.Abs(velocity.X) > maxDashSpeed) || (Input.GetKey(PressAction.PRESSED, Keys.LeftShift) && dashToggleDelayTime > time) || isDown || dir.X != intendedDir)
                isDashing = false;
            if (!isDashing)
                dashSlowdownDelayTime = 0;
            //Slowdown after dashing
            if (isDashing && Math.Abs(velocity.X) >= maxPlayerSpeed)
            {
                dashSlowdownDelayTime += time;
                if (dashSlowdownDelayTime >= 15 * time)
                    velocity.X -= .1f * dir.X;
            }
            
            //the dashing itself
            if (isDashing && ((Input.GetKey(PressAction.DOWN, Keys.A)) || (Input.GetKey(PressAction.DOWN, Keys.D))) && GO.GetComponent<CManaPool>().ReturnMana() >= 25 && Math.Abs(velocity.X) <= maxDashSpeed * .75)
            {
                GO.GetComponent<CManaPool>().ConsumeMana(25);
                velocity.X = Math.Min(Math.Abs(velocity.X) + 2.0f, maxDashSpeed) * dir.X;
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
            if (grounded && vertVelo > 0) vertVelo = 0;
            if (grounded && Input.GetKey(PressAction.PRESSED, Keys.W) || grounded && Input.GetKey(PressAction.PRESSED, Keys.Space))
            {
                vertVelo = -jumpPower;
                jumpDelayTime = 0;
            }
            if (!grounded && Input.GetKey(PressAction.PRESSED, Keys.W) || !grounded && Input.GetKey(PressAction.PRESSED, Keys.Space))
            {
                if (GO.GetComponent<CManaPool>().ReturnMana() >= 75 && fallPanic == false && jumpDelayTime >= 10 * time)
                {
                    GO.GetComponent<CManaPool>().ConsumeMana(75);
                    vertVelo = -jumpPower;
                    jumpDelayTime = 0;
                }
            }
            if (!grounded)
            {
                vertVelo += acceleration;
                jumpDelayTime += time;
            }
            //speed is in Units/Second
            GO.Pos += velocity * speed * time;
            GO.Pos += new Vector2(0, Math.Min(hit.distance, vertVelo * time));
            
            //shoot
            if (Input.GetKey(PressAction.PRESSED, Keys.F))
            {
                //double if, for adding sounds or animations showing the player that no mana remains later
                if (GO.GetComponent<CManaPool>().ReturnMana() > 20)
                {
                    GO.GetComponent<CManaPool>().ConsumeMana(20);
                    GO.GetComponent<CShoot>().Shoot(dir, new Vector2(0.2f, 0.2f), velocity);
                }
            }
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
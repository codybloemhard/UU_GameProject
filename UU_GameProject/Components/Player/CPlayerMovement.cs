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
        private float jumpPower = 13f;
        private float acceleration = 50f, vertVelo = 0f;
        private float playerAccel = 0.02f;
        private float jumpDelayTime = 0;
        private float dashToggleDelayTime = 0;
        private float dashSlowdownDelayTime = 0;
        private float lastVertVelo;
        private float maxDashSpeed = 4;
        private bool isDashing = false;
        private bool fallPanic = false;
        private bool grounded = false;
        private bool leftSideAgainstWall = false;
        private bool rightSideAgainstWall = false;
        private bool leftIsSlidingOnWall = false;
        private bool rightIsSlidingOnWall = false;
        private bool isCrawling = false;
        private bool isCrouching = false;
        private bool isSliding = false;
        private bool isDown = false;
        private Vector2 velocity = Vector2.Zero;
        private bool initiated = false;
        private CAnimatedSprite animation;
        private CHealthPool healthPool;
        private CManaPool manaPool;

        public CPlayerMovement(float speed) : base()
        {
            this.speed = speed;
            dir = new Vector2(1, 0);
        }

        public void InitPlayer()
        {
            initiated = true;
            Renderer render = GO.Renderer;
            if (render != null) render.colour = Color.White;
            animation = GO.Renderer as CAnimatedSprite;
            healthPool = GO.GetComponent<CHealthPool>();
            manaPool = GO.GetComponent<CManaPool>();
        }

        public override void Update(float time)
        {
            if (!initiated) InitPlayer();
            //animations
            if (isCrawling)
                animation.PlayAnimation("crawling", 2);
            else if (isSliding)
                animation.PlayAnimation("sliding", 2);
            else if (isCrouching)
                animation.PlayAnimation("crouching", 2);
            else if (!grounded)
                animation.PlayAnimation("airborn", 2);
            else if (fallPanic)
                animation.PlayAnimation("fallPanic", 2);
            else
                animation.PlayAnimation("walking", 2);

            //basic movement: slowly accelerates the player
            if (Input.GetKey(PressAction.DOWN, Keys.D) && velocity.X + playerAccel <= maxPlayerSpeed)
                velocity += new Vector2(playerAccel, 0);
            if (Input.GetKey(PressAction.DOWN, Keys.A) && velocity.X - playerAccel >= -maxPlayerSpeed)
                velocity += new Vector2(-playerAccel, 0);

            //stops the player when they hit a wall
            if (leftSideAgainstWall && Input.GetKey(PressAction.DOWN, Keys.A))
                velocity.X = 0;
            if (rightSideAgainstWall && Input.GetKey(PressAction.DOWN, Keys.D))
                velocity.X = 0;

            //stops the player if no buttons are pressed
            if (!Input.GetKey(PressAction.DOWN, Keys.D) && velocity.X > 0 && grounded)
                velocity -= new Vector2(Math.Min(playerAccel, velocity.X), 0);
            if (!Input.GetKey(PressAction.DOWN, Keys.A) && velocity.X < 0 && grounded)
                velocity -= new Vector2(Math.Max(-playerAccel, velocity.X), 0);
            if (GO.Pos.Y > 9) healthPool.ChangeHealth(1000);
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
            if (isDown && velocity.X == 0) isCrouching = true;
            else isCrouching = false;

            //crawling
            if (isDown && ((velocity.X > 0 && velocity.X <= maxPlayerSpeed) || (velocity.X < 0 && velocity.X >= -maxPlayerSpeed)))
                isCrawling = true;
            else isCrawling = false;

            //sliding forward
            if (isDown && velocity.X > maxPlayerSpeed)
            {
                velocity.X = Math.Max(maxPlayerSpeed, velocity.X - playerAccel * 0.2f);
                isSliding = true;
            } //sliding backward
            else if (isDown && velocity.X < -maxPlayerSpeed)
            {
                velocity.X = Math.Min(maxPlayerSpeed, velocity.X + playerAccel * 0.2f);
                isSliding = true;
            } //not sliding
            else isSliding = false;

            //fall panic
            if (vertVelo > 25 || lastVertVelo > 25)
            {
                fallPanic = true;
                //fall damage
                if (grounded)
                    healthPool.ChangeHealth((int)lastVertVelo - 25);
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
            if (isDashing && ((Input.GetKey(PressAction.DOWN, Keys.A)) || (Input.GetKey(PressAction.DOWN, Keys.D))) && manaPool.ConsumeMana(25) && Math.Abs(velocity.X) <= maxDashSpeed * .75)
                velocity.X = Math.Min(Math.Abs(velocity.X) + 2.0f, maxDashSpeed) * dir.X;

            //gravity, jump and player head and bottom collision
            Vector2 BottomLeft = GO.Pos + new Vector2(0, GO.Size.Y + 0.01f);
            Vector2 BottomRight = GO.Pos + new Vector2(GO.Size.X, GO.Size.Y + 0.01f);
            Vector2 TopLeft = GO.Pos + new Vector2(0, -0.01f);
            Vector2 TopRight = GO.Pos + new Vector2(GO.Size.X, -0.01f);
            RaycastResult hitBottomLeft = GO.Raycast(BottomLeft, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hitBottomRight = GO.Raycast(BottomRight, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hitTopLeft = GO.Raycast(TopLeft, new Vector2(0, -1), RAYCASTTYPE.STATIC);
            RaycastResult hitTopRight = GO.Raycast(TopRight, new Vector2(0, -1), RAYCASTTYPE.STATIC);
            RaycastResult hitBottom;
            RaycastResult hitTop;
            if (hitBottomLeft.distance > hitBottomRight.distance)
                hitBottom = hitBottomRight;
            else hitBottom = hitBottomLeft;

            if (hitTopLeft.distance > hitTopRight.distance)
                hitTop = hitTopRight;
            else hitTop = hitTopLeft;

            if (hitBottom.hit && hitBottom.distance < 0.001f)
                grounded = true;

            else grounded = false;

            if (hitTop.hit && hitTop.distance < 0.03f)
                vertVelo = 0;

            if (grounded && vertVelo > 0)
                vertVelo = 0;
            if (grounded && Input.GetKey(PressAction.PRESSED, Keys.W) || grounded && Input.GetKey(PressAction.PRESSED, Keys.Space))
            {
                vertVelo = -jumpPower;
                jumpDelayTime = 0;
            }
            if (!grounded && Input.GetKey(PressAction.PRESSED, Keys.W) || !grounded && Input.GetKey(PressAction.PRESSED, Keys.Space))
            {
                if (manaPool.ConsumeMana(75) && fallPanic == false && jumpDelayTime >= 0.166666f)
                {
                    vertVelo = -jumpPower;
                    jumpDelayTime = 0;
                }
            }
            if (!grounded && !leftIsSlidingOnWall && !rightIsSlidingOnWall)
            {
                vertVelo += acceleration * time;
                jumpDelayTime += time;
            }

            //speed is in Units/Second
            GO.Pos += velocity * speed * time;
            GO.Pos += new Vector2(0, Math.Min(hitBottom.distance, vertVelo * time));

            //Wall sliding
            if (leftSideAgainstWall && Input.GetKey(PressAction.DOWN, Keys.A) && vertVelo > 0)
                leftIsSlidingOnWall = true;
            else leftIsSlidingOnWall = false;

            if (rightSideAgainstWall && Input.GetKey(PressAction.DOWN, Keys.D) && vertVelo > 0)
                rightIsSlidingOnWall = true;
            else rightIsSlidingOnWall = false;

            if (leftIsSlidingOnWall || rightIsSlidingOnWall)
                vertVelo = 1;

            //player side collision
            Vector2 leftTop = GO.Pos + new Vector2(-0.01f, 0);
            Vector2 leftMiddle = GO.Pos + new Vector2(-0.01f, 0.5f * GO.Size.Y);
            Vector2 leftBottom = GO.Pos + new Vector2(-0.01f, GO.Size.Y);
            Vector2 rightTop = GO.Pos + new Vector2(GO.Size.X + 0.01f, 0);
            Vector2 rightMiddle = GO.Pos + new Vector2(GO.Size.X + 0.01f, 0.5f * GO.Size.Y);
            Vector2 rightBottom = GO.Pos + new Vector2(GO.Size.X + 0.01f, GO.Size.Y);
            RaycastResult hitLeftTop = GO.Raycast(leftTop, new Vector2(-1, 0), RAYCASTTYPE.STATIC);
            RaycastResult hitLeftMiddle = GO.Raycast(leftMiddle, new Vector2(-1, 0), RAYCASTTYPE.STATIC);
            RaycastResult hitLeftBottom = GO.Raycast(leftBottom, new Vector2(-1, 0), RAYCASTTYPE.STATIC);
            RaycastResult hitRightTop = GO.Raycast(rightTop, new Vector2(1, 0), RAYCASTTYPE.STATIC);
            RaycastResult hitRightMiddle = GO.Raycast(rightMiddle, new Vector2(1, 0), RAYCASTTYPE.STATIC);
            RaycastResult hitRightBottom = GO.Raycast(rightBottom, new Vector2(1, 0), RAYCASTTYPE.STATIC);
            RaycastResult hitLeft;
            RaycastResult hitRight;
            if (Math.Min(hitLeftTop.distance, Math.Min(hitLeftMiddle.distance, hitLeftBottom.distance)) == hitLeftTop.distance)
                hitLeft = hitLeftTop;
            else if (Math.Min(hitLeftTop.distance, Math.Min(hitLeftMiddle.distance, hitLeftBottom.distance)) == hitLeftMiddle.distance)
                hitLeft = hitLeftMiddle;
            else hitLeft = hitLeftBottom;

            if (Math.Min(hitRightTop.distance, Math.Min(hitRightMiddle.distance, hitRightBottom.distance)) == hitRightTop.distance)
                hitRight = hitRightTop;
            else if (Math.Min(hitRightTop.distance, Math.Min(hitRightMiddle.distance, hitRightBottom.distance)) == hitRightMiddle.distance)
                hitRight = hitRightMiddle;
            else hitRight = hitRightBottom;

            if (hitLeft.hit && hitLeft.distance < 0.02f)
                leftSideAgainstWall = true;
            else leftSideAgainstWall = false;

            if (hitRight.hit && hitRight.distance < 0.02f)
                rightSideAgainstWall = true;
            else rightSideAgainstWall = false;
            //fireball
            //fires toward the cursor
            if (Input.GetMouseButton(PressAction.PRESSED, MouseButton.LEFT))
            {
                GO.GetComponent<Components.General.CMagicness>().Fireball(new Vector2(.2f, .2f), velocity, GO.GetComponent<Components.General.CFaction>().GetFaction());
            }

            //RAITUNINGU STORAIKO!!!
            if (Input.GetMouseButton(PressAction.PRESSED, MouseButton.RIGHT))
                GO.GetComponent<Components.General.CMagicness>().Lightning(new Vector2(1.5f, 1.5f), 0.2f, GO.tag, GO.GetComponent<Components.General.CFaction>().GetFaction());

            //shoot
            //if (Input.GetKey(PressAction.PRESSED, Keys.Space))
            //{ GO.GetComponent<CMeleeAttack>().Melee(dir, new Vector2(2, 2), 1.0f); }
            if (Input.GetKey(PressAction.PRESSED, Keys.E))
            GO.GetComponent<CMeleeAttack>().Melee(dir, new Vector2(0.75f, 1), 0.2f, GO.tag, GO.GetComponent<Components.General.CFaction>().GetFaction());
            if (Input.GetKey(PressAction.PRESSED, Keys.F) && GO.GetComponent<CManaPool>().ConsumeMana(20))
                GO.GetComponent<CShoot>().Shoot(dir, new Vector2(0.2f, 0.2f), velocity, GO.GetComponent<Components.General.CFaction>().GetFaction());
        }

        public override void OnCollision(GameObject other)
        {
            if (other.tag == "killer")
                healthPool.ChangeHealth(20);
        }

        public Vector2 Velocity()
        {
            return velocity;
        }

        public void Reset()
        {
            GO.Pos = new Vector2(1, 7);
            velocity = new Vector2(0, 0);
            vertVelo = 0;
        }
    }
}
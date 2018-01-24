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
        public float intendedDir;
        private float jumpPower = 13f;
        private float acceleration = 50f, vertVelo = 0f;
        private float playerAccel = 8f;
        private float jumpDelayTime = 0;
        private float dashToggleDelayTime = 0;
        private float dashSlowdownDelayTime = 0;
        private float lastVertVelo;
        private float maxDashSpeed = 4;
        private bool isDashing = false;
        private bool fallPanic = false;
        private bool leftIsSlidingOnWall = false;
        private bool rightIsSlidingOnWall = false;
        private bool isCrawling = false;
        private bool isCrouching = false;
        private bool isSliding = false;
        private bool isDown = false;
        private bool initiated = false;
        private bool canMelee = true;
        public bool spawned = false;
        public Vector2 playerPosition;
        private Vector2 dir;
        public Vector2 velocity = Vector2.Zero;
        private Vector2 checkPos = new Vector2(-1000, -1000);
        private CAnimatedSprite animation;
        private CHealthPool healthPool;
        private CManaPool manaPool;
        private CMagicness magicness;
        private CFaction faction;
        private CMeleeAttack melee;
        private CShoot shoot;
        private CMopWeapon mopWeapon;
        private CRaycasts cRaycasts;

        public CPlayerMovement(float speed) : base()
        {
            this.speed = speed;
        }

        public void InitPlayer()
        {
            initiated = true;
            dir = new Vector2(1, 0);
            checkPos = GO.Pos;
            Renderer render = GO.Renderer;
            if (render != null) render.colour = Color.White;
            animation = GO.Renderer as CAnimatedSprite;
            healthPool = GO.GetComponent<CHealthPool>();
            manaPool = GO.GetComponent<CManaPool>();
            magicness = GO.GetComponent<CMagicness>();
            faction = GO.GetComponent<CFaction>();
            melee = GO.GetComponent<CMeleeAttack>();
            shoot = GO.GetComponent<CShoot>();
            mopWeapon = GO.GetComponent<CMopWeapon>();
            cRaycasts = GO.GetComponent<CRaycasts>();
        }

        public override void Update(float time)
        {
            if (!initiated) InitPlayer();
            float timeAccel = playerAccel * time;
            playerPosition = GO.Pos;

            PickAnimation();
            BasicMovement(time, timeAccel);
            Gravity(time);
            AdvancedMovement(timeAccel);
            Jump();
            DoubleJump();
            Attacks();
            FallPanic();
            Dashing(time);
            WallSliding();

            GO.Pos += cRaycasts.Move((velocity * speed + new Vector2(0, vertVelo)) * time);
        }

        private void BasicMovement(float time, float timeAccel)
        {
            //basic movement: slowly accelerates the player
            if (Input.GetKey(PressAction.DOWN, Keys.D) && velocity.X + timeAccel <= maxPlayerSpeed)
                velocity.X += timeAccel;
            if (Input.GetKey(PressAction.DOWN, Keys.A) && velocity.X - timeAccel >= -maxPlayerSpeed)
                velocity.X -= timeAccel;
            //stops the player when they hit a wall
            if (cRaycasts.WallLeftHit && Input.GetKey(PressAction.DOWN, Keys.A))
                velocity.X = 0;
            if (cRaycasts.WallRightHit && Input.GetKey(PressAction.DOWN, Keys.D))
                velocity.X = 0;
            //stops the player if no buttons are pressed
            if (!Input.GetKey(PressAction.DOWN, Keys.D) && velocity.X > 0 && cRaycasts.Grounded)
                velocity -= new Vector2(Math.Min(timeAccel, velocity.X), 0);
            if (!Input.GetKey(PressAction.DOWN, Keys.A) && velocity.X < 0 && cRaycasts.Grounded)
                velocity -= new Vector2(Math.Max(-timeAccel, velocity.X), 0);
            if (GO.Pos.Y > 200) Reset();
            if (velocity != Vector2.Zero)
            {
                dir = velocity;
                dir.Normalize();
            }
            if (Input.GetKey(PressAction.DOWN, Keys.D))
                intendedDir = 1;
            if (Input.GetKey(PressAction.DOWN, Keys.A))
                intendedDir = -1;
        }

        private void Gravity(float time)
        {
            //gravity
            if (!cRaycasts.Grounded && !leftIsSlidingOnWall && !rightIsSlidingOnWall)
            {
                vertVelo += acceleration * time;
                jumpDelayTime += time;
            }
        }

        private void AdvancedMovement(float timeAccel)
        {
            //down
            if (Input.GetKey(PressAction.DOWN, Keys.S) && cRaycasts.Grounded)
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
            if (isDown && Math.Abs(velocity.X) < 0.1f)
                isCrouching = true;
            else isCrouching = false;
            //crawling
            if (isDown && ((velocity.X > 0 && velocity.X <= maxPlayerSpeed) || (velocity.X < 0 && velocity.X >= -maxPlayerSpeed)))
                isCrawling = true;
            else isCrawling = false;
            //sliding forward
            if (isDown && velocity.X > maxPlayerSpeed)
            {
                velocity.X = Math.Max(maxPlayerSpeed, velocity.X - timeAccel * 0.2f);
                isSliding = true;
            }
            //sliding backward
            else if (isDown && velocity.X < -maxPlayerSpeed)
            {
                velocity.X = Math.Min(maxPlayerSpeed, velocity.X + timeAccel * 0.2f);
                isSliding = true;
            }
            else isSliding = false;
        }

        private void Jump()
        {
            //jump
            if (cRaycasts.Grounded && vertVelo > 0) vertVelo = 0;
            if (cRaycasts.Grounded && Input.GetKey(PressAction.PRESSED, Keys.W) || cRaycasts.Grounded && Input.GetKey(PressAction.PRESSED, Keys.Space))
            {
                vertVelo = -jumpPower;
                jumpDelayTime = 0;
                AudioManager.PlayEffect("jump");
            }
        }

        private void DoubleJump()
        {
            //double jump
            if (!cRaycasts.Grounded && Input.GetKey(PressAction.PRESSED, Keys.W) || !cRaycasts.Grounded && Input.GetKey(PressAction.PRESSED, Keys.Space))
            {
                if (fallPanic == false && jumpDelayTime >= 0.166666f)
                {
                    if (magicness.DoubleJump())
                    {
                        vertVelo = -jumpPower;
                        jumpDelayTime = 0;
                        AudioManager.PlayEffect("jump");
                    }
                }
            }
        }

        private void FallPanic()
        {
            //fall panic and damage
            if (vertVelo > 25 || lastVertVelo > 25)
            {
                fallPanic = true;
                if (cRaycasts.Grounded)
                    healthPool.ChangeHealth((int)lastVertVelo - 25, false);
                lastVertVelo = vertVelo;
            }
            else fallPanic = false;
        }

        private void Dashing(float time)
        {
            //Dashing, broken!
            if (false && Input.GetKey(PressAction.PRESSED, Keys.LeftShift) && Math.Abs(velocity.X) <= maxDashSpeed && isDashing == false)
            {
                if (magicness.Dash())
                {
                    isDashing = true;
                    dashToggleDelayTime = 0;
                    AudioManager.PlayEffect("jump");
                }
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
            if (isDashing && ((Input.GetKey(PressAction.DOWN, Keys.A)) || (Input.GetKey(PressAction.DOWN, Keys.D))) && Math.Abs(velocity.X) <= maxDashSpeed * .75)
                velocity.X = Math.Min(Math.Abs(velocity.X) + 2.0f, maxDashSpeed) * dir.X;
        }

        private void WallSliding()
        {
            //Wall sliding
            if (cRaycasts.WallLeftHit && Input.GetKey(PressAction.DOWN, Keys.A) && vertVelo > 0)
                leftIsSlidingOnWall = true;
            else leftIsSlidingOnWall = false;

            if (cRaycasts.WallRightHit && Input.GetKey(PressAction.DOWN, Keys.D) && vertVelo > 0)
                rightIsSlidingOnWall = true;
            else rightIsSlidingOnWall = false;

            if (leftIsSlidingOnWall || rightIsSlidingOnWall)
                vertVelo = 1;
        }


        private void PickAnimation()
        {
            //player animations
            if (isCrawling && intendedDir > 0)
                animation.PlayAnimationIfDifferent("crawlingRight", 6);
            else if (isCrawling && intendedDir < 0)
                animation.PlayAnimationIfDifferent("crawlingLeft", 6);
            else if (isSliding)
                animation.PlayAnimationIfDifferent("sliding", 2);
            else if (leftIsSlidingOnWall)
                animation.PlayAnimationIfDifferent("wallSlidingRight", 2);
            else if (rightIsSlidingOnWall)
                animation.PlayAnimationIfDifferent("wallSlidingLeft", 2);
            else if (!cRaycasts.Grounded && intendedDir > 0)
                animation.PlayAnimationIfDifferent("airborneRight", 2);
            else if (!cRaycasts.Grounded && intendedDir < 0)
                animation.PlayAnimationIfDifferent("airborneLeft", 2);
            else if (fallPanic)
                animation.PlayAnimationIfDifferent("fallPanic", 2);
            else if (velocity.X < 0)
                animation.PlayAnimationIfDifferent("runningLeft", 8);
            else if (velocity.X > 0)
                animation.PlayAnimationIfDifferent("runningRight", 8);
            else if (intendedDir < 0)
                animation.PlayAnimationIfDifferent("standingLeft", 8);
            else
                animation.PlayAnimationIfDifferent("standingRight", 8);
        }

        private void Attacks()
        {
            //attacks
            if (Input.GetMouseButton(PressAction.PRESSED, MouseButton.LEFT))
            {
                mopWeapon.ChangeAnimationFire();
                magicness.Fireball(new Vector2(.4f, .4f), velocity, faction.GetFaction());
            }
            if (Input.GetMouseButton(PressAction.PRESSED, MouseButton.RIGHT))
            {
                mopWeapon.ChangeAnimationLightning();
                magicness.Lightning(new Vector2(1.5f, 1.5f), 0.2f, GO.tag, faction.GetFaction());
            }
            if (Input.GetKey(PressAction.PRESSED, Keys.F))
                magicness.Heal();
            if (Input.GetKey(PressAction.PRESSED, Keys.E))
                DoMelee();

        }
        
        //tries to melee
        private void DoMelee()
        {
            if (!canMelee) return;
            mopWeapon.Melee();
            melee.Melee(dir, new Vector2(0.75f, 1), 0.2f, 15, false, GO.tag, faction.GetFaction());
            canMelee = false;
            Timers.Add("playermelee", 0.5f, () => canMelee = true);
        }

        //hardfix for bad collision detection, if you end up inside a solid block you get moved out of it
        public override void OnCollision(GameObject other)
        {
            if (other.tag == "checkpoint")
            {
                checkPos = GO.Pos;
            }

            else if (other.tag.Contains("solid") && other.tag != "bossdoorsolid")
            {
                float up, down, left, right;
                up = GO.Pos.Y + GO.Size.Y - other.Pos.Y;
                down = other.Pos.Y + other.Size.Y - GO.Pos.Y;
                left = GO.Pos.X + GO.Size.X - other.Pos.X;
                right = other.Pos.X + other.Size.X - GO.Pos.X;
                if (Math.Abs(up) < 0.5f && up < down && up < left && up < right)
                    GO.Pos -= new Vector2(0, up);
                else if (Math.Abs(down) < 0.5f && down < left && down < right)
                    GO.Pos += new Vector2(0, down);
                else if (Math.Abs(left) < 0.5f && left < right)
                    GO.Pos -= new Vector2(left, 0);
                else if (Math.Abs(right) < 0.5f)
                    GO.Pos += new Vector2(right, 0);
            }
        }
        
        public Vector2 Velocity()
        {
            return velocity;
        }

        //reset
        public void Reset()
        {
            AudioManager.PlayEffect("dead");
            if (checkPos != new Vector2(-1000, -1000))
                GO.Pos = checkPos;
            velocity = new Vector2(0, 0);
            vertVelo = 0;
            healthPool.Reset();
            manaPool.Reset();
        }
    }
}
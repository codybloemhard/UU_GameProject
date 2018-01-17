using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    public class CRobotBoss : Component
    {
        private GameObject player;
        private CAnimatedSprite animationBoss;
        private CCamera camera;
        private CMeleeAttack melee;
        private CRaycasts cRaycasts;
        private CFaction faction;
        private bool initiated;
        private FSM fsm = new FSM();
        private Vector2 velocity, difference, dir = Vector2.Zero;
        private bool movingRight, crushing, chasing;
        private float speed, gravity = 10f, jetpackPower = 10, acceleration = 5f, maxYSpeed = 3,crushSpeed = 20f, turningSpeed = 3;
        private float ctime, shootDelay = 1f, shootTime, crushDelay = 5, crushTime, changeDelay = 5, changeTime, chaseTime, chaseMax = 5, chaseSpeedIncrease;
        private bool grounded, falling;

        public CRobotBoss(float speed)
        {
            this.speed = speed;
            velocity = new Vector2(-speed, 0);
        }

        public void InitRobot()
        {
            initiated = true;
            Renderer render = GO.Renderer;
            animationBoss = GO.Renderer as CAnimatedSprite;
            cRaycasts = GO.GetComponent<CRaycasts>();
            player = GO.FindWithTag("player");
            Vector2 difference = player.Pos + player.Size / 2 - (GO.Pos + GO.Size / 2);
            camera = player.GetComponent<CCamera>();
            melee = GO.GetComponent<CMeleeAttack>();
            faction = GO.GetComponent<CFaction>();
            if (render != null) render.colour = Color.White;

            fsm.Add("walk", IdleBehaviour);
            fsm.Add("fly", Fly);
            fsm.Add("chase", Chase);
        }

        public override void Update(float time)
        {
            chasing = false;
            if (!initiated) InitRobot();

            else if (fsm.CurrentState == "fly" && falling)
                animationBoss.PlayAnimationIfDifferent("flying", 4);
            else if (fsm.CurrentState == "fly" && !falling)
                animationBoss.PlayAnimationIfDifferent("falling", 4);
            else
                animationBoss.PlayAnimationIfDifferent("walking", 2);

            difference = player.Pos + player.Size / 2 - (GO.Pos + GO.Size / 2);
            fsm.Update();
            ctime = time;
            grounded = cRaycasts.Grounded;

            if (changeTime > 0)
                changeTime -= ctime;
            if (changeTime <= 0)
            {
                changeTime = changeDelay;
                ChangeFSM(false);
                fsm.SetCurrentState("chase");
            }
        }

        private void ChangeFSM(bool chase)
        {
            int random = MathH.random.Next(3);
            if (random == 0)
                fsm.SetCurrentState("walk");
            else if (random == 1)
                fsm.SetCurrentState("fly");
            else if (random == 2 && !chase)
            {
                fsm.SetCurrentState("chase");
                chaseTime = chaseMax;
                chaseSpeedIncrease = 1;
            }
            else ChangeFSM(chase);
        }

        private void IdleBehaviour()
        {
            if (grounded && (!cRaycasts.LeftGrounded || cRaycasts.WallLeftHit))
                velocity.X = speed;
            if (grounded && (!cRaycasts.RightGrounded || cRaycasts.WallRightHit))
                velocity.X = -speed;

            if (Math.Abs(velocity.X) < speed)
                velocity.X += acceleration * ctime;

            if (!grounded)
                velocity.Y += gravity * ctime;
            else velocity.Y = 0;

            if (shootTime > 0)
                shootTime -= ctime;
            else if ((velocity.X < 0 && difference.X < 0) || (velocity.X > 0 && difference.X > 0))
            {
                ShootAtPlayer(difference);
                shootTime = shootDelay;
            }

            GO.Pos += cRaycasts.Move(velocity * ctime);
        }

        private void Fly()
        { 
            if(crushTime > 0) crushTime -= ctime;

            if (crushTime <= 0 && GO.Pos.X + GO.Size.X / 2 > player.Pos.X && GO.Pos.X + GO.Size.X / 2 < player.Pos.X + player.Size.X)
            {
                crushing = true;
                crushTime = crushDelay;
                velocity = Vector2.Zero;
            }

            if (crushing)
            {
                velocity.Y += crushSpeed * ctime;
                if (cRaycasts.DistanceToFloor == 0)
                {
                    crushing = false;
                    camera.ShakeCamera(1f, Math.Min(velocity.Y * 2f, 0.3f));
                    melee.Melee(new Vector2(0, 1), new Vector2(0.75f, 1.5f), 0.2f, 50, false, GO.tag, faction.GetFaction());
                }
            }
            else
            {
                if (difference.X < -2)
                    movingRight = false;
                else if (difference.X > 2)
                    movingRight = true;

                if (movingRight && velocity.X < speed)
                    velocity.X += acceleration * ctime;
                else if (!movingRight && velocity.X > -speed)
                    velocity.X -= acceleration * ctime;

                if (grounded)
                    velocity.Y = 0;
                else if (cRaycasts.CeilingHit)
                    velocity.Y = 0;

                if (cRaycasts.DistanceToFloor > 2f && velocity.Y < maxYSpeed)
                {
                    falling = false;
                    velocity.Y += gravity * ctime;
                }
                else if (velocity.Y > -maxYSpeed)
                {
                    falling = true;
                    velocity.Y -= jetpackPower * ctime;
                }

                if (shootTime > 0)
                    shootTime -= ctime;
                if (shootTime <= 0)
                {
                    ShootAtPlayer(difference);
                    shootTime = shootDelay;
                }
            }

            GO.Pos += cRaycasts.Move(velocity * ctime);
        }

        private void Chase()
        {
            if (chaseTime <= 0)
                Explode();
            else
            {
                chaseTime -= ctime;
                chaseSpeedIncrease += ctime / 5;
                chasing = true;
                Vector2 difference = player.Pos + player.Size / 2 - (GO.Pos + GO.Size / 2);

                float dirRads = (float)Math.Atan2(dir.Y, dir.X);
                float difRads = (float)Math.Atan2(difference.Y, difference.X);

                if (Math.Abs(dirRads - difRads) < Math.PI)
                {
                    if (dirRads > difRads)
                        dirRads -= turningSpeed * ctime;
                    else if (dirRads < difRads)
                        dirRads += turningSpeed * ctime;
                }
                else
                {
                    if (dirRads > difRads)
                        dirRads += turningSpeed * ctime;
                    else if (dirRads < difRads)
                        dirRads -= turningSpeed * ctime;
                }

                dir = new Vector2((float)Math.Cos(dirRads), (float)Math.Sin(dirRads));
                GO.Pos += cRaycasts.Move(dir * speed * chaseSpeedIncrease * ctime);
            }
        }
        
        private void ShootAtPlayer(Vector2 dir)
        {
            GameObject bullet = new GameObject(GO.tag + "explobullet", GO.Context, 0);
            bullet.AddComponent(new CRender("block"));
            bullet.AddComponent(new CHeatSeakingBullet(player, 4, dir, 1.5f, GO.tag));
            bullet.AddComponent(new CAABB());
            bullet.AddComponent(new CDamageDealer(10, false));
            bullet.AddComponent(new CFaction("enemy"));
            if (dir.X > 0)
                bullet.Pos = GO.Pos + GO.Size / 2f - new Vector2(.2f) / 2f + new Vector2(GO.Size.X / 2f + .2f, 0);
            else
                bullet.Pos = GO.Pos + GO.Size / 2f - new Vector2(.2f) / 2f - new Vector2(GO.Size.X / 2f + .2f, 0);
            bullet.Size = new Vector2(.2f);
        }

        public void Explode()
        {
            GameObject explosion = new GameObject(GO.tag + "chase", GO.Context);
            explosion.AddComponent(new CRender("block"));
            explosion.Renderer.colour = Color.Red;
            explosion.AddComponent(new CAABB());
            explosion.AddComponent(new CExplosionArea());
            explosion.AddComponent(new CDamageDealer(50, false));
            explosion.Size = new Vector2(1.8f);
            explosion.Pos = GO.Pos + GO.Size / 2 - explosion.Size / 2;
            ChangeFSM(true);
        }

        public bool Crushing { get { return crushing; } set { crushing = value; } }
        public bool Chasing { get { return chasing; } set { chasing = value; } }
    }
}

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
    public class CMageBoss : Component
    {
        private float ctime;
        private float maxYSpeed = 3, acceleration = 5, teleportSpeed = 4f, actionTime, teleportDelay = 4, teleportTime;
        private bool initiated = false, disappearing, appearing, firingFireball = false, firingLightning = false;
        private CRaycasts cRaycasts;
        private FSM fsm = new FSM();
        private Vector2 targetPosition, newTarget, targetSize, velocity, origin;
        private CHealthPool healthpool;
        private GameObject player;
        private CAnimatedSprite animationBoss;

        private void InitMage()
        {
            origin = GO.Pos;
            initiated = true;
            healthpool = GO.GetComponent<CHealthPool>();
            cRaycasts = GO.GetComponent<CRaycasts>();
            player = GO.Context.objects.FindWithTag("player");
            targetPosition = new Vector2(MathH.random.Next(2, 12), MathH.random.Next(2, 7));
            fsm.Add("stay", StayInPlace);
            fsm.Add("teleport", Teleport);
            targetSize = GO.Size;
            fsm.SetCurrentState("stay");
            animationBoss = GO.Renderer as CAnimatedSprite;
        }

        public override void Update(float time)
        {
            animationBoss = GO.Renderer as CAnimatedSprite;
            base.Update(time);
            ctime = time;
            if (!initiated) InitMage();
            animation();

            if (teleportTime <= 0)
            {
                teleportTime = teleportDelay;
                newTarget = origin + new Vector2(MathH.random.Next(-3, 3), MathH.random.Next(-3, 3));
                targetPosition = newTarget;
                disappearing = true;
                fsm.SetCurrentState("teleport");
            }
            teleportTime -= time;

            fsm.Update();
        }

        private void LightningBolt()
        {
            Vector2 target = player.Pos;
            GameObject lightning = new GameObject("lightningbolt" + GO.tag, GO.Context);
            CAnimatedSprite animLight = new CAnimatedSprite();
            animLight.AddAnimation("lightningSpawner", "lightningSpawner");
            animLight.PlayAnimation("lightningSpawner", 4);
            lightning.AddComponent(animLight);
            lightning.AddComponent(new CLightningBolt(target));
            lightning.AddComponent(new CAABB());
            lightning.Size = new Vector2(.3f, .6f);
            lightning.Pos = new Vector2(target.X - lightning.Size.X/2, GO.Pos.Y - 5);
            firingLightning = false;
        }

        //shoots a fireball at the player
        private void FireBall()
        {
            Vector2 dir = player.Pos + player.Size/2 - (GO.Pos + GO.Size/2);
            Vector2 size = new Vector2(1f, 1f);
            GameObject fireball = new GameObject("fireball", GO.Context, 0);
            CAnimatedSprite animBall = new CAnimatedSprite();
            animBall.AddAnimation("fireball", "fireball");
            animBall.PlayAnimation("fireball", 8);
            fireball.Pos = GO.Pos + new Vector2(GO.Size.X/2, .4f) - size / 2f;
            fireball.Size = size;
            fireball.AddComponent(animBall);
            fireball.AddComponent(new CFireballMovement(Vector2.Zero, dir, dir, 20f, false));
            fireball.AddComponent(new CAABB());
            fireball.AddComponent(new CFaction("enemy"));
            AudioManager.PlayEffect("shoot");
            firingFireball = false;
        }

        //hovering, picking whether to attack or teleport
        private void StayInPlace()
        {
            float sign = Math.Sign(targetPosition.Y - (GO.Pos.Y + GO.Size.Y/2));
            velocity.Y += sign * acceleration * ctime;

            float velocitySign = Math.Sign(velocity.Y);
            velocity.Y = sign * Math.Min(sign * velocity.Y, maxYSpeed);
            GO.Pos += velocity * ctime /(healthpool.HealhPercent + .5f);

            if(actionTime <= 0)
            {
                actionTime = .4f + (float)MathH.random.NextDouble() * healthpool.HealhPercent * 2;
                int action = MathH.random.Next(2);
                if (action == 0)
                {
                    firingFireball = true;
                    Timers.Add("fireball", 1f, () => FireBall());
                }
                else if (action == 1)
                {
                    firingLightning = true;
                    Timers.Add("lightning", 1f, () => LightningBolt());
                }
            }
            actionTime -= ctime;
        }

        private void Teleport()
        {
            Console.WriteLine("teleporting");
            if (disappearing)
            {
                
                if (GO.Size.X - targetSize.X * teleportSpeed * ctime < 0 || GO.Size.Y - targetSize.Y * teleportSpeed * ctime < 0)
                {
                    disappearing = false;
                    appearing = true;
                    GO.Pos = newTarget;
                }
                else
                {
                    GO.Size -= targetSize * teleportSpeed * ctime;
                    GO.Pos += 0.5F * targetSize * teleportSpeed * ctime;
                }
            }

            if (appearing)
            {
                GO.Size = new Vector2(Math.Min(GO.Size.X + targetSize.X * teleportSpeed * ctime, targetSize.X), Math.Min(GO.Size.Y + targetSize.Y * teleportSpeed * ctime, targetSize.Y));
                GO.Pos -= 0.5f * targetSize * teleportSpeed * ctime;
                if (GO.Size == targetSize)
                {
                    appearing = false;
                    targetPosition.Y -= 1f;
                    fsm.SetCurrentState("stay");
                }
            }
        }

        //picks different animations, based on current behaviour
        private void animation()
        {
            if (firingFireball)
                animationBoss.PlayAnimationIfDifferent("fireball", 12 - healthpool.HealhPercent * 6);
            else if (firingLightning)
                animationBoss.PlayAnimationIfDifferent("lightning", 12 - healthpool.HealhPercent * 6);
            else
                animationBoss.PlayAnimationIfDifferent("hovering", 12 - healthpool.HealhPercent * 6);
        }
    }
}

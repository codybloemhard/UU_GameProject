using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    public class CRangedEnemyAI : CBasicEnemyAI
    {
        private CAnimatedSprite animationRangedEnemy;
        private bool firingFireball = false;
        private string rangedType;

        public CRangedEnemyAI(ENEMY type, string rangedEnemyType) : base(type)
        {
            rangedType = rangedEnemyType;
            damage = 10f;
            maxSpeed = 2.5f;
            maxHP = 25;
            magicChange = 4;
        }

        public override void Init()
        {
            base.Init();
            animationRangedEnemy = GO.Renderer as CAnimatedSprite;
            
            fsm.Add("idle", IdleBehaviour);
            fsm.Add("active", ActiveBehaviour);
            fsm.SetCurrentState("idle");
        }

        public override void Update(float time)
        {
            base.Update(time);
            animationRangedEnemy = GO.Renderer as CAnimatedSprite;
            if (length <= 5.25f && fsm.CurrentState == "idle")
                fsm.SetCurrentState("active");
            else if (length > 5.25f && fsm.CurrentState != "idle")
                fsm.SetCurrentState("idle");
            animation();
        }

        private void ActiveBehaviour()
        {
            float range = 4.5f;
            wait = Math.Max(0, wait - ctime);
            Vector2 feetLeft = GO.Pos + new Vector2(0, GO.Size.Y + 0.01f);
            Vector2 feetRight = GO.Pos + new Vector2(GO.Size.X, GO.Size.Y + 0.01f);
            RaycastResult hitLeft = GO.Raycast(feetLeft, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hitRight = GO.Raycast(feetRight, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hit;
            if (hitLeft.distance > hitRight.distance)
                hit = hitRight;
            else hit = hitLeft;
            if (hit.hit && hit.distance < 0.05f) grounded = true;
            else grounded = false;
            //keep  distance to player
            float diff = player.Pos.X - GO.Pos.X;
            if (diff > 0 && dir.X < 0) dir *= -1;
            if (diff < 0 && dir.X > 0) dir *= -1;
            bool run = false;
            if (length < range)
            {
                run = true;
                if (diff > 0 && speed > 0)
                    speed *= -1;
                if (diff < 0 && speed < 0)
                    speed *= -1;
            }
            if (hitLeft.distance > 0.1f || hitRight.distance > 0.1f)
                run = false;
            if (leftBlocked || rightBlocked)
                run = false;
            if (length < range && wait == 0)
            {
                firingFireball = true;
                wait = 1.75f;
                Timers.Add("fireball", 0.5f, () => fireball());
            }
            if (!grounded)
            {
                vertVelo += gravity * ctime;
                GO.Pos += new Vector2(speed * ctime, Math.Min(hit.distance, vertVelo * ctime));
            }
            else if(run)
                GO.Pos += new Vector2(speed * ctime, 0f);
        }

        //Choosing one out of 8 directions to shoot.
        private Vector2 shootdir(float x)
        {
            if(x < 0)
                x *= -1;
            double angle =  Math.Acos(x / length) / Math.PI * 180;
            if (angle < 22.5)
                return new Vector2(dir.X, 0);
            else if (GO.Pos.Y < player.Pos.Y && angle < 67.5)
                return new Vector2(dir.X, 1);
            else if (angle < 67.5)
                return new Vector2(dir.X, -1);
            else if (GO.Pos.Y < player.Pos.Y)
                return new Vector2(0, 1);
            else
                return new Vector2(0, -1);
        }

        private void fireball()
        {
            Vector2 direction = shootdir(GO.Pos.X - player.Pos.X);
            GO.GetComponent<CShoot>().Shoot(direction, new Vector2(0.2f, 0.2f), Vector2.Zero, GO.GetComponent<CFaction>().GetFaction(), damage, DoPotion());
            AudioManager.PlayEffect("shoot");
            firingFireball = false;
        }

        //picks different animations, based on direction and enemy type
        private void animation()
        {
            switch (rangedType)
            {
                case "redMage":
                    if (dir.X > 0 && firingFireball)
                        animationRangedEnemy.PlayAnimationIfDifferent("redMageCastingRight", 8);
                    else if (dir.X < 0 && firingFireball)
                        animationRangedEnemy.PlayAnimationIfDifferent("redMageCastingLeft", 8);
                    else if (dir.X > 0)
                        animationRangedEnemy.PlayAnimationIfDifferent("redMageStandingRight", 8);
                    else
                        animationRangedEnemy.PlayAnimationIfDifferent("redMageStandingLeft", 8);
                    break;
                case "greenMage":
                    if (dir.X > 0 && firingFireball)
                        animationRangedEnemy.PlayAnimationIfDifferent("greenMageCastingRight", 8);
                    else if (dir.X < 0 && firingFireball)
                        animationRangedEnemy.PlayAnimationIfDifferent("greenMageCastingLeft", 8);
                    else if (dir.X > 0)
                        animationRangedEnemy.PlayAnimationIfDifferent("greenMageStandingRight", 8);
                    else
                        animationRangedEnemy.PlayAnimationIfDifferent("greenMageStandingLeft", 8);
                    break;
                case "purpleMage":
                    if (dir.X > 0 && firingFireball)
                        animationRangedEnemy.PlayAnimationIfDifferent("purpleMageCastingRight", 8);
                    else if (dir.X < 0 && firingFireball)
                        animationRangedEnemy.PlayAnimationIfDifferent("purpleMageCastingLeft", 8);
                    else if (dir.X > 0)
                        animationRangedEnemy.PlayAnimationIfDifferent("purpleMageStandingRight", 8);
                    else
                        animationRangedEnemy.PlayAnimationIfDifferent("purpleMageStandingLeft", 8);
                    break;
                case "cyborg":
                    if (dir.X > 0)
                        animationRangedEnemy.PlayAnimationIfDifferent("rangedCyborgRight", 8);
                    else
                        animationRangedEnemy.PlayAnimationIfDifferent("rangedCyborgLeft", 8);
                    break;
            }
        }
    }
}
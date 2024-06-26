using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    public class CNormalEnemyAI : CBasicEnemyAI
    {
        private CAnimatedSprite animationNormalEnemy;
        private string normalType;

        public CNormalEnemyAI(ENEMY type, string normalEnemyType) : base(type)
        {
            normalType = normalEnemyType;
            damage = 20f;
            maxSpeed = 2f;
            maxHP = 50;
            magicChange = 6;
        }

        public override void Init()
        {
            base.Init();
            animationNormalEnemy = GO.Renderer as CAnimatedSprite;
            fsm.Add("idle", IdleBehaviour);
            fsm.Add("active", ActiveBehaviour);
            fsm.SetCurrentState("idle");
        }
        
        public override void Update(float time)
        {
            animationNormalEnemy = GO.Renderer as CAnimatedSprite;

            base.Update(time);
            animation(); 
            if (length <= 4.5f && fsm.CurrentState == "idle")
                fsm.SetCurrentState("active");
            else if (length > 4.5f && fsm.CurrentState != "idle")
                fsm.SetCurrentState("idle");

        }

        //deals with movement
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
            float diff = player.Pos.X - GO.Pos.X;
            if (diff < 0 && dir.X > 0) dir *= -1;
            if (diff > 0 && dir.X < 0) dir *= -1;
            bool run = false;
            if (length > range * 0.1f)
            {
                run = true;
                if (diff < 0 && speed > 0) speed *= -1;
                if (diff > 0 && speed < 0) speed *= -1;
            }
            if ((hitLeft.distance > 0.1f && dir.X < 0) || (hitRight.distance > 0.1f && dir.X > 0))
                run = false;
            if ((leftBlocked && dir.X < 0 ) || (rightBlocked && dir.X > 0))
                run = false;
            if (length < range && wait == 0)
            {
                if (GO.GetComponent<CMeleeAttack>() != null)
                {
                    GO.GetComponent<CMeleeAttack>().Melee(dir, new Vector2(0.75f, 1), 0.2f, damage, DoPoison(), GO.tag, GO.GetComponent<CFaction>().GetFaction());
                    wait = 1f;
                }
            }

            if (!grounded)
            {
                vertVelo += gravity * ctime;
                GO.Pos += new Vector2(speed * ctime, Math.Min(hit.distance, vertVelo * ctime));
            }
            else if (run)
                GO.Pos += new Vector2(speed * ctime, 0f);
        }

        //picks different animations, based on direction and enemy type
        private void animation()
        {
            switch (normalType)
            {
                case "magic":
                    if (base.dir.X > 0)
                        animationNormalEnemy.PlayAnimationIfDifferent("redSlimeMovingRight", 6);
                    else
                        animationNormalEnemy.PlayAnimationIfDifferent("redSlimeMovingLeft", 6);
                    break;
                case "robot":
                    if (base.dir.X > 0)
                        animationNormalEnemy.PlayAnimationIfDifferent("robotSlimeMovingRight", 6);
                    else
                        animationNormalEnemy.PlayAnimationIfDifferent("robotSlimeMovingLeft", 6);
                    break;
                case "cyborg":
                    if (base.dir.X > 0)
                        animationNormalEnemy.PlayAnimationIfDifferent("normalCyborgRight", 6);
                    else
                        animationNormalEnemy.PlayAnimationIfDifferent("normalCyborgLeft", 6);
                    break;
            }
        }
    }
}
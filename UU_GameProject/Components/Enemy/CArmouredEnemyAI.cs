using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    public class CArmouredEnemyAI : CBasicEnemyAI
    {
        private CAnimatedSprite animationArmouredEnemy;
        private string armouredType;

        public CArmouredEnemyAI(ENEMY type, string armouredEnemyType) : base(type)
        {
            armouredType = armouredEnemyType;
            damage = 10f;
            maxSpeed = 1.75f;
            maxHP = 100;
            magicChange = 6;
        }
        
        public override void Init()
        {
            base.Init();
            animationArmouredEnemy = GO.Renderer as CAnimatedSprite;
            fsm.Add("idle", IdleBehaviour);
            fsm.Add("active", ActiveBehaviour);
            fsm.SetCurrentState("idle");
            turnTime = 0f;
        }
        
        public override void Update(float time)
        {
            animation();
            animationArmouredEnemy = GO.Renderer as CAnimatedSprite;
            base.Update(time);
            if (length <= 4.5f && fsm.CurrentState == "idle")
                fsm.SetCurrentState("active");
            else if (length > 4.5f && fsm.CurrentState != "idle")
                fsm.SetCurrentState("idle");
        }

        private void ActiveBehaviour()
        {
            float range = 4.5f;
            turnTime += ctime;
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
            if (turnTime < 2f) wait = 1;
            bool run = false;
            float diff = player.Pos.X - GO.Pos.X;
            CHealthPool hPool = GO.GetComponent<CHealthPool>();
            if (diff < 0 && dir.X < 0)
                hPool.isProtected = true;
            else if (diff > 0 && dir.X > 0)
                hPool.isProtected = true;
            else hPool.isProtected = false;
            if (turnTime > 2f)
            {
                if (diff < 0 && dir.X > 0)
                    dir *= -1;
                if (diff > 0 && dir.X < 0)
                    dir *= -1;          
                if (length > range * 0.1f)
                {
                    run = true;
                    if (diff < 0 && speed > 0)
                        speed *= -1;
                    if (diff > 0 && speed < 0)
                        speed *= -1;
                }
            }
            if (hitLeft.distance > 0.1f || hitRight.distance > 0.1f)
                run = false;
            if (leftBlocked || rightBlocked)
                run = false;
            if (turnTime < 2f)
                run = false;
            if (length < range && wait == 0)
            {
                GO.GetComponent<CMeleeAttack>().Melee(dir, new Vector2(0.75f, 1), 0.2f, damage, DoPoison(), GO.tag, GO.GetComponent<CFaction>().GetFaction());
                wait = 1f;
                turnTime = 0f;
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
            switch (armouredType)
            {
                case "robot":
                    if (base.dir.X > 0)
                        animationArmouredEnemy.PlayAnimationIfDifferent("armoredRobotRight", 8);
                    else
                        animationArmouredEnemy.PlayAnimationIfDifferent("armoredRobotLeft", 8);
                    break;
                case "cyborg":
                    if (base.dir.X > 0)
                        animationArmouredEnemy.PlayAnimationIfDifferent("armoredCyborgRight", 8);
                    else
                        animationArmouredEnemy.PlayAnimationIfDifferent("armoredCyborgLeft", 8);
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Core;

namespace UU_GameProject
{
    public class CBasicEnemyAI : Component
    {
        protected float speed, ctime, wait, length;
        protected Vector2 dir = new Vector2(1, 0);
        protected bool grounded;
        protected float gravity = 1.8f, vertVelo = 0f;
        protected FSM fsm = new FSM();
        protected GameObject player;
        protected bool leftBlocked { get; private set; }
        protected bool rightBlocked { get; private set; }

        public CBasicEnemyAI(float speed)
        {
            this.speed = speed;
        }

        public override void Init()
        {
            player = GO.FindWithTag("player");
        }

        public override void Update(float time)
        {
            base.Update(time);
            ctime = time;
            Vector2 difference = player.Pos - GO.Pos;
            length = difference.Length();
            CheckSides();
            fsm.Update();
        }

        protected void IdleBehaviour()
        {
            //Passive movement behaviour, patrolling a platform.
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
            if (grounded && (hitLeft.distance > 0.1f || hitRight.distance > 0.1f))
            {
                speed *= -1;
                GO.Pos += Vector2.UnitX * speed * 0.05f;
            }
            if (leftBlocked || rightBlocked)
                speed *= -1;
            float hordisplace = 0f, verdisplace = 0f;
            if (grounded)
            {
                hordisplace = speed * ctime;
                vertVelo = 0f;
            }
            else vertVelo += gravity * ctime;
            if (ctime > 0.25f) //fix voor window draggen bug
            {
                hordisplace = 0f;
                verdisplace = 0f;
            }
            verdisplace = Math.Min(hit.distance, vertVelo * ctime);
            GO.Pos += new Vector2(hordisplace, verdisplace);
        }

        private void CheckSides()
        {
            leftBlocked = false;
            rightBlocked = false;
            Vector2 feetLeft = GO.Pos + new Vector2(-0.01f, GO.Size.Y);
            Vector2 feetRight = GO.Pos + new Vector2(GO.Size.X + 0.01f, GO.Size.Y);
            RaycastResult hitToLeft = GO.Raycast(feetLeft, new Vector2(-1, 0), RAYCASTTYPE.STATIC);
            RaycastResult hitToRight = GO.Raycast(feetRight, new Vector2(+1, 0), RAYCASTTYPE.STATIC);
            if (hitToLeft.hit && hitToLeft.distance < speed * ctime)
                leftBlocked = true;
            if (hitToRight.hit && hitToRight.distance < speed * ctime)
                rightBlocked = true;
        }
    }
}
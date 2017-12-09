﻿using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    class CRangedEnemyAI : Component
    {
        private float speed, ctime, wait, length;
        private Vector2 dir = new Vector2(1, 0);
        private bool grounded;
        private float gravity = 0.8f, vertVelo = 0f;
        private FSM fsm = new FSM();

        public CRangedEnemyAI(float speed)
        {
            this.speed = speed;
        }

        public override void Init()
        {
            CRender render = GO.Renderer as CRender;
            if (render != null) render.colour = Color.Green;
            fsm.Add("idle", IdleBehaviour);
            fsm.Add("active", ActiveBehaviour);
            fsm.SetCurrentState("idle");
        }

        public override void Update(float time)
        {
            base.Update(time);
            ctime = time;
            Vector2 difference = GO.FindWithTag("player").Pos - GO.Pos;
            length = difference.Length();

            if (length <= 5.0f && fsm.CurrentState == "idle")
            {
                fsm.SetCurrentState("active");
                Console.WriteLine("OI!");
            }
            else if (length > 5.0f && fsm.CurrentState != "idle")
            {
                fsm.SetCurrentState("idle");
                Console.WriteLine("It msut've been the wind...");
            }

            fsm.Update();
        }

        //Damage handling
        public override void OnCollision(GameObject other)
        {
            if (other.tag == "bullet")
            {
                CHealthBar health = GO.GetComponent<CHealthBar>();
                health.hit(1);
                other.active = false;
            }
        }

        private void IdleBehaviour()
        {
            //Passive movement behaviour, patrolling a platform.
            Vector2 feetLeft = GO.Pos + new Vector2(0, GO.Size.Y + 0.01f);
            Vector2 feetRight = GO.Pos + new Vector2(GO.Size.X, GO.Size.Y + 0.01f);
            RaycastResult hitLeft = GO.Raycast(feetLeft, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hitRight = GO.Raycast(feetRight, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hit;
            if (hitLeft.distance > hitRight.distance)
                hit = hitRight;
            else
                hit = hitLeft;

            if (hit.hit && hit.distance < 0.05f)
            {
                grounded = true;
            }
            else grounded = false;

            if (grounded && (hitLeft.distance > 0.05f || hitRight.distance > 0.05f))
            {
                dir *= -1;
                speed *= -1;
            }

            if (grounded)
                GO.Pos += new Vector2(speed * ctime, Math.Min(hit.distance, vertVelo * ctime));
            else
                vertVelo += gravity * ctime;
        }

        private void ActiveBehaviour()
        {
            //Aiming at the player and shooting the projectile in one of 8 directions,
            //when the player is within a certain range of course.
            //After having shot try to keep optimal distance, for safety, from the player.

            float range = 4.5f;
            wait = Math.Max(0, wait - ctime);

            Vector2 feetLeft = GO.Pos + new Vector2(0, GO.Size.Y + 0.01f);
            Vector2 feetRight = GO.Pos + new Vector2(GO.Size.X, GO.Size.Y + 0.01f);
            RaycastResult hitLeft = GO.Raycast(feetLeft, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hitRight = GO.Raycast(feetRight, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hit;
            if (hitLeft.distance > hitRight.distance)
                hit = hitRight;
            else
                hit = hitLeft;

            if (hit.hit && hit.distance < 0.05f)
                grounded = true;
            else
                grounded = false;

            //Moving left or right, depending on where the player is in relation to the enemy.
            if (GO.Pos.X > GO.FindWithTag("player").Pos.X)
            {
                if (dir.X > 0)
                { dir *= -1; speed *= -1; }
            }
            else
            {
                if (dir.X < 0)
                { dir *= -1; speed *= -1; }
            }
            if (grounded)
                if (length > 2 * range / 3 && hitLeft.distance < 0.05f && hitRight.distance < 0.05f)
                    GO.Pos += new Vector2(speed * ctime, Math.Min(hit.distance, vertVelo * ctime));
                else if (!grounded)
                    vertVelo += gravity * ctime;
        }
    }
}
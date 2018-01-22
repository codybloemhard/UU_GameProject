using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Core;

namespace UU_GameProject
{
    public enum ENEMY
    {
        MAGIC, ROBOT, CYBORG
    }

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
        protected ENEMY type;
        protected float turnTime;
        protected float damage = 0;
        protected int maxHP = 0;
        protected float maxSpeed = 0;
        protected bool iniated = false;
        protected int magicChange = -1;

        public CBasicEnemyAI(ENEMY type)
        {
            this.type = type;
        }

        public override void Init()
        {
            player = GO.FindWithTag("player");
        }

        private void Set()
        {
            iniated = true;
            if (type == ENEMY.ROBOT)
            {
                speed = maxSpeed * 1.2f;
                damage *= 1.2f;
                maxHP = (int)(maxHP * 1.2f);
                magicChange = -1;
            }
            else if (type == ENEMY.MAGIC)
                speed = maxSpeed;
            else
            {
                speed = maxSpeed * 1.1f;
                damage *= 1.1f;
                maxHP = (int)(maxHP * 1.1f);
            }
            GO.GetComponent<CHealthPool>().InitHP(maxHP);
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!iniated) Set();
            ctime = time;
            Vector2 difference = player.Pos - GO.Pos;
            length = difference.Length();
            CheckSides();
            fsm.Update();
        }
        //patrolling a platform.
        protected void IdleBehaviour()
        {
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
            if (grounded && hitLeft.distance > 0.1)
                speed = maxSpeed;
            else if (grounded && hitRight.distance > 0.1)
                speed = -maxSpeed;

            if (leftBlocked || rightBlocked)
            speed *= -1;

            Vector2 velocity = Vector2.Zero;
            
            if (grounded)
            {
                velocity.X = speed * ctime;
                vertVelo = 0f;
            }
            else vertVelo += gravity * ctime;
            if (ctime > 0.25f) //fix for window drag bug
            {
                velocity = Vector2.Zero;
            }
            velocity.Y = Math.Min(hit.distance, vertVelo * ctime);
            GO.Pos += velocity;
        }

        protected bool DoPoison()
        {
            if (magicChange > 0)
            {
                int r = (int)(MathH.random.NextDouble() * magicChange);
                if (r == 0) return true;
            }
            return false;
        }

        private void CheckSides()
        {
            leftBlocked = false;
            rightBlocked = false;
            Vector2 feetLeft = GO.Pos + new Vector2(-0.01f, GO.Size.Y);
            Vector2 feetRight = GO.Pos + new Vector2(GO.Size.X + 0.01f, GO.Size.Y);
            RaycastResult hitToLeft = GO.Raycast(feetLeft, new Vector2(-1, 0), RAYCASTTYPE.STATIC);
            RaycastResult hitToRight = GO.Raycast(feetRight, new Vector2(+1, 0), RAYCASTTYPE.STATIC);
            float marge = Math.Max(Math.Abs(speed * ctime), 0.1f);
            if (hitToLeft.hit && hitToLeft.distance < marge)
                leftBlocked = true;
            if (hitToRight.hit && hitToRight.distance < marge)
                rightBlocked = true;
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if (other.tag.Contains("solid") && other.tag != "bossdoorsolid")
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
    }
}
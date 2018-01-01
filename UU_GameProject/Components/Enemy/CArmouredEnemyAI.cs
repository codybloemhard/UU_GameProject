using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    class CArmouredEnemyAI : Component
    {
        private float speed, ctime, wait, turntime = 1.0f, length;
        private Vector2 dir = new Vector2(1, 0);
        private bool grounded;
        private float gravity = 1.8f, vertVelo = 0f;
        private FSM fsm = new FSM();

        public CArmouredEnemyAI(float speed)
        {
            this.speed = speed;
        }
    
        public override void Init()
        {
            CRender render = GO.Renderer as CRender;
            FSM fsm = new FSM();
            if (render != null) render.colour = Color.Yellow;
            fsm.Add("idle", IdleBehaviour);
            fsm.Add("active", ActiveBehaviour);
            fsm.SetCurrentState("idle");
        }

        //Selecting behaviour
        public override void Update(float time)
        {
            base.Update(time);
            ctime = time;
            Vector2 difference = GO.FindWithTag("player").Pos - GO.Pos;
            length = difference.Length();

            /*if (length <= 4.5f && fsm.CurrentState == "idle")
            {
                fsm.SetCurrentState("active");
                Console.WriteLine(2);
            }
            else if (length > 4.5f && fsm.CurrentState != "idle")
            {
                fsm.SetCurrentState("idle");
                Console.WriteLine(1);
            }*/

            fsm.SetCurrentState("idle");
            fsm.Update();

            /*Console.WriteLine(fsm.CurrentState);
            Console.WriteLine(1);*/
        }

        private void IdleBehaviour()
        {
            //Passive movement behaviour, patrolling a platform.
            Vector2 feetLeft = GO.Pos + new Vector2(0, GO.Size.Y + 0.01f);
            Vector2 feetRight = GO.Pos + new Vector2(GO.Size.X, GO.Size.Y + 0.01f);
            RaycastResult hitLeft = GO.Raycast(feetLeft, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hitRight = GO.Raycast(feetRight, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hit;
            if (hitLeft.distance > hitRight.distance) hit = hitRight;
            else hit = hitLeft;

            if (hit.hit && hit.distance < 0.05f) grounded = true;
            else grounded = false;

            if (grounded && (hitLeft.distance > 0.05f || hitRight.distance > 0.05f))
            {
                dir *= -1;
                speed *= -1;
            }

            if (grounded)
                GO.Pos += new Vector2(speed * ctime, Math.Min(hit.distance, vertVelo * ctime));
            else
            {
                vertVelo += gravity * ctime;
                GO.Pos += new Vector2(speed * ctime, Math.Min(hit.distance, vertVelo * ctime));
            }       
        }

        private void ActiveBehaviour()
        {
            //When the player comes within a certain range, 
            //start running at the player to get within melee range and then making a melee attack.
            //Melee attack needs a timer to prevent instadeath.
            //We need to figure out how to make this dude immune to dmg from the front, 
            //but die in 2 to 3 hits from the back.
            //Thus he needs to wait for a timer before turning around to make it fair.
            float reach = 2.25f;
            wait = Math.Max(0, wait - ctime);
            turntime = Math.Max(0, wait - ctime);

            Vector2 feetLeft = GO.Pos + new Vector2(0, GO.Size.Y + 0.01f);
            Vector2 feetRight = GO.Pos + new Vector2(GO.Size.X, GO.Size.Y + 0.01f);
            RaycastResult hitLeft = GO.Raycast(feetLeft, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hitRight = GO.Raycast(feetRight, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            RaycastResult hit;
            if (hitLeft.distance > hitRight.distance) hit = hitRight;
            else hit = hitLeft;

            if (hit.hit && hit.distance < 0.05f) grounded = true;
            else grounded = false;

            //Moving left or right, depending on where the player is in relation to the enemy.
            if (GO.Pos.X > GO.FindWithTag("player").Pos.X)
            {
                if (dir.X > 0 && turntime == 0)
                {
                    dir *= -1;
                    speed *= -1;
                }
                if (length < reach - (0.1f * reach) && wait <= 0 && dir.X < 0)
                {
                    GO.GetComponent<CMeleeAttack>().Melee(dir, new Vector2(4, 2), 1.5f, GO.tag);
                    wait = 1.3f;
                    Console.WriteLine("OUCH!");
                }
                if (grounded && length > 2 * reach / 3 && !(hitLeft.distance > 0.05f || hitRight.distance > 0.05f) && dir.X < 0)
                    GO.Pos += new Vector2(speed * ctime, Math.Min(hit.distance, vertVelo * ctime));
                else if (!grounded)
                {
                    vertVelo += gravity * ctime;
                    GO.Pos += new Vector2(speed * ctime, Math.Min(hit.distance, vertVelo * ctime));
                }
            }
            else if (turntime == 0)
            {
                if (dir.X < 0 && turntime == 0)
                {
                    dir *= -1;
                    speed *= -1;
                }
                if (length - GO.Pos.X < reach - (0.1f * reach) && wait <= 0 && dir.X > 0)
                {
                    GO.GetComponent<CMeleeAttack>().Melee(dir, new Vector2(4, 2), 1.0f, GO.tag);
                    wait = 1.3f;
                    Console.WriteLine("OUCH!");
                }
                if (grounded && length > 2 * reach / 3 && !(hitLeft.distance > 0.05f || hitRight.distance > 0.05f) && dir.X > 0)
                    GO.Pos += new Vector2(speed * ctime, Math.Min(hit.distance, vertVelo * ctime));
                else if (!grounded)
                {
                    vertVelo += gravity * ctime;
                    GO.Pos += new Vector2(speed * ctime, Math.Min(hit.distance, vertVelo * ctime));
                }
            }
        }

        public Vector2 direction()
        {
            return dir;
        }
    }
}
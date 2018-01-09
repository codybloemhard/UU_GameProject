using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    public class CNormalEnemyAI : CBasicEnemyAI
    {
        public CNormalEnemyAI(float speed) : base(speed) { }

        public override void Init()
        {
            base.Init();
            CRender render = GO.Renderer as CRender;
            if (render != null) render.colour = Color.OrangeRed;
            fsm.Add("idle", IdleBehaviour);
            fsm.Add("active", ActiveBehaviour);
            fsm.SetCurrentState("idle");
        }
        
        //Selecting behaviour
        public override void Update(float time)
        {
            base.Update(time);          
            if (length <= 4.5f && fsm.CurrentState == "idle")
                fsm.SetCurrentState("active");
            else if (length > 4.5f && fsm.CurrentState != "idle")
                fsm.SetCurrentState("idle");
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
            else hit = hitLeft;
            if (hit.hit && hit.distance < 0.05f) grounded = true;
            else grounded = false;
            //Moving left or right, depending on where the player is in relation to the enemy and keeping distance.
            float diff = player.Pos.X - GO.Pos.X;
            if (diff < 0 && dir.X > 0)
                dir *= -1;
            if (diff > 0 && dir.X < 0)
                dir *= -1;
            bool run = false;
            if (length > range * 0.1f)
            {
                run = true;
                if (diff < 0 && speed > 0)
                    speed *= -1;
                if (diff > 0 && speed < 0)
                    speed *= -1;
            }
            if (hitLeft.distance > 0.1f || hitRight.distance > 0.1f)
                run = false;
            if (leftBlocked || rightBlocked)
                run = false;
            if (length < range && wait == 0)
            {
                GO.GetComponent<CMeleeAttack>().Melee(dir, new Vector2(0.75f, 1), 0.2f, GO.tag);
                wait = 1f;
            }
            if (!grounded)
            {
                vertVelo += gravity * ctime;
                GO.Pos += new Vector2(speed * ctime, Math.Min(hit.distance, vertVelo * ctime));
            }
            else if (run)
                GO.Pos += new Vector2(speed * ctime, 0f);
        }
    }
}
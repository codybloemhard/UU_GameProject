using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    public class CRangedEnemyAI : CBasicEnemyAI
    {
        public CRangedEnemyAI(float speed) : base(speed) { }

        public override void Init()
        {
            base.Init();
            CRender render = GO.Renderer as CRender;
            if (render != null) render.colour = Color.DarkSeaGreen;
            fsm.Add("idle", IdleBehaviour);
            fsm.Add("active", ActiveBehaviour);
            fsm.SetCurrentState("idle");
        }

        //Selecting behaviour
        public override void Update(float time)
        {
            base.Update(time);
            if (length <= 5.25f && fsm.CurrentState == "idle")
                fsm.SetCurrentState("active");
            else if (length > 5.25f && fsm.CurrentState != "idle")
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
            if (diff > 0 && dir.X < 0)
                dir *= -1;
            if (diff < 0 && dir.X > 0)
                dir *= -1;
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
                Vector2 thing = shootdir(GO.Pos.X - player.Pos.X);
                GO.GetComponent<CShoot>().Shoot(thing, new Vector2(0.2f, 0.2f), Vector2.Zero);
                wait = 1.75f;
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
    }
}
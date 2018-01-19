using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Core;

namespace UU_GameProject
{
    public class CBossTrigger : Component
    {
        private bool closed = false;
        private bool beaten = false;
        private CGrowingDoor door;
        private GameObject boss;

        public CBossTrigger() : base() { }

        public override void Update(float time)
        {
            base.Update(time);
            if (!closed) return;
            if (boss == null && !beaten)
            {
                door.Open();
                beaten = true;
            }
        }

        private void Close()
        {
            GameObject[] d = GO.FindAllWithTag("bossdoor");
            GameObject closest = null;
            float min = float.MaxValue;
            Vector2 mid = GO.Pos + GO.Size / 2f;
            foreach(GameObject go in d)
            {
                float dist = (mid - go.Pos).Length();
                if(dist < min)
                {
                    min = dist;
                    closest = go;
                }
            }
            if (closest == null) return;
            door = closest.GetComponent<CGrowingDoor>();
            if (door == null) return;
            door.Close();
            closed = true;
            boss = GO.FindWithTag("boss");
            if(boss == null)
            {
                door.Open();
                beaten = true;
                return;
            }
            CRobotBoss comp = boss.GetComponent<CRobotBoss>();
            comp.started = true;
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if (beaten) return;
            if (other.tag == "player") Close();
        }
    }
}
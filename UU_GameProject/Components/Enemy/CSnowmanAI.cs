﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    class CSnowmanAI : Component
    {
        private bool initiated;
        private RaycastResult BLdown, BRdown;
        private Vector2 velocity;
        private float speed = 4;

        private void InitSnowman()
        {
            initiated = true;
            velocity = new Vector2(-speed, 0);

        }

        public override void Update(float time)
        {
            if (!initiated) InitSnowman();
            Raycast();
            if (BLdown.distance > 0.1f && BRdown.distance < 0.1f)
                velocity = new Vector2(speed, 0);

            GO.Pos += velocity * time;
        }

        private void Raycast()
        {
            BLdown = GO.Raycast(GO.Pos + new Vector2(0, GO.Size.Y), new Vector2(0, 1), RAYCASTTYPE.STATIC);
            BRdown = GO.Raycast(GO.Pos + GO.Size, new Vector2(0, 1), RAYCASTTYPE.STATIC);
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if (other.tag == "boss" && other.GetComponent<CSnowmanBoss>() != null)
                GO.Destroy();
        }
    }
}

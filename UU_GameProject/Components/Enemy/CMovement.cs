using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    class CMovement : Component
    {
        bool grounded;
        private float speed, ctime, wait, length;
        private Vector2 dir = new Vector2(1, 0);
        private float gravity = 1.8f, vertVelo = 0f;

        public CMovement(float speed)
        {
            this.speed = speed;
        }

        public override void Update(float time)
        {
            base.Update(time);
            ctime = time;
        }
        public bool Move(Vector2 direction, float distance)
        {
            direction.Normalize();
            Vector2 newPos = GO.Pos + direction * distance * ctime;

            Vector2 newTopLeft = newPos;
            Vector2 newBotRight = newPos + GO.Size;

            RaycastResult TLRight, TLDown, BRLeft, BRUp;
            TLRight = GO.Raycast(newTopLeft, new Vector2(1, 0), RAYCASTTYPE.STATIC);
            TLDown = GO.Raycast(newTopLeft, new Vector2(0, 1), RAYCASTTYPE.STATIC);
            BRLeft = GO.Raycast(newBotRight, new Vector2(-1, 0), RAYCASTTYPE.STATIC);
            BRUp = GO.Raycast(newBotRight, new Vector2(0, -1), RAYCASTTYPE.STATIC);

            Vector2 topLeft = GO.Pos;
            Vector2 topRight = GO.Pos + new Vector2(GO.Size.X, 0);
            Vector2 botLeft = GO.Pos + new Vector2(0, GO.Size.Y);
            Vector2 botRight = GO.Pos + GO.Size;

            RaycastResult TLMain, TRMain, BLMain, BRMain;
            TLMain = GO.Raycast(topLeft, direction, RAYCASTTYPE.STATIC);
            TRMain = GO.Raycast(topRight, direction, RAYCASTTYPE.STATIC);
            BLMain = GO.Raycast(botLeft, direction, RAYCASTTYPE.STATIC);
            BRMain = GO.Raycast(botRight, direction, RAYCASTTYPE.STATIC);

            if (TLRight.distance < GO.Size.X || TLDown.distance < GO.Size.Y || BRLeft.distance < GO.Size.X|| BRUp.distance < GO.Size.Y)
            {
                GO.Pos += Math.Min(TLRight.distance -GO.Size.X, Math.Min(TLDown.distance - GO.Size.Y, Math.Min(BRLeft.distance - GO.Size.X, BRUp.distance - GO.Size.Y))) * direction * ctime;
                return false;
            }

            else if(TLMain.distance < distance * ctime || TRMain.distance < distance * ctime || BLMain.distance < distance * ctime || BRMain.distance < distance * ctime)
            {
                GO.Pos += Math.Min(TLMain.distance, Math.Min(TRMain.distance, Math.Min(BLMain.distance, BRMain.distance))) * direction * ctime;
                return false;
            }

            GO.Pos += direction * distance * ctime;
            return true;
        }

        public void IdleMovement()
        {
            if (BotLeftGrounded || BotRightGrounded)
                grounded = true;
            else grounded = false;

            if (grounded && !GO.GetComponent<CMovement>().BotLeftGrounded)
            {
                dir = new Vector2(1, 0);
                speed = 1;
            }
            else if (grounded && !GO.GetComponent<CMovement>().BotRightGrounded)
            {
                dir = new Vector2(-1, 0);
                speed = -1;
            }

            if (grounded)
                vertVelo = 0;
            else
                vertVelo += gravity * ctime;
            GO.GetComponent<CMovement>().Move(new Vector2(speed, vertVelo), new Vector2(speed, vertVelo).Length());
        }

        public bool BotLeftGrounded
        {
            get
            {
                Vector2 footLeft = GO.Pos + new Vector2(0, GO.Size.Y);
                RaycastResult hit = GO.Raycast(footLeft, new Vector2(0, 1), RAYCASTTYPE.STATIC);

                if (hit.hit && hit.distance < 0.01f)
                    return true;
                return false;
            }
        }

        public bool BotRightGrounded
        {
            get
            {
                Vector2 botRight = GO.Pos + new Vector2(GO.Size.X, GO.Size.Y);
                RaycastResult hit = GO.Raycast(botRight, new Vector2(0, 1), RAYCASTTYPE.STATIC);

                if (hit.hit && hit.distance < 0.01f)
                    return true;
                return false;
            }
        }
    }
}

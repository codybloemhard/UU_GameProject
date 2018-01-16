using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    class CRaycasts : Component
    {
        private bool initiated;
        private Vector2[] leftOrigins, topOrigins, rightOrigins, botOrigins;
        private Vector2 topRight, botLeft, botRight;
        private RaycastResult[] leftCast, topCast, rightCast, botCast;
        private Vector2 left = new Vector2(-1, 0), right = new Vector2(1, 0), up = new Vector2(0, -1), down = new Vector2(0, 1);

        public override void Init()
        {
            base.Init();
            CalculateOrigins();
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!initiated) { CalculateOrigins(); initiated = true; }

            for (int i = 0; i < leftCast.Length; i++)
                leftCast[i] = GO.Raycast(GO.Pos + leftOrigins[i], left, RAYCASTTYPE.STATIC);
            for (int i = 0; i < rightCast.Length; i++)
                rightCast[i] = GO.Raycast(GO.Pos + rightOrigins[i], right, RAYCASTTYPE.STATIC);
            for (int i = 0; i < topCast.Length; i++)
                topCast[i] = GO.Raycast(GO.Pos + topOrigins[i], up, RAYCASTTYPE.STATIC);
            for (int i = 0; i < botCast.Length; i++)
                botCast[i] = GO.Raycast(GO.Pos + botOrigins[i], down, RAYCASTTYPE.STATIC);
        }

        public Vector2 Move(Vector2 movement)
        {
            if (!initiated) return Vector2.Zero;
            Vector2 realMovement = movement;
            if (movement.X < 0)
            {
                for (int i = 0; i < leftCast.Length; i++)
                    realMovement.X = Math.Max(realMovement.X, -leftCast[i].distance);
            }
            else if (movement.X > 0)
            {
                for (int i = 0; i < rightCast.Length; i++)
                    realMovement.X = Math.Min(realMovement.X, rightCast[i].distance);
            }
            if (movement.Y < 0)
            {
                for (int i = 0; i < topCast.Length; i++)
                    realMovement.Y = Math.Max(realMovement.Y, -topCast[i].distance);
            }
            else if (movement.Y > 0)
            {
                for (int i = 0; i < botCast.Length; i++)
                    realMovement.Y = Math.Min(realMovement.Y, botCast[i].distance);
            }
            return realMovement;
        }


        private void CalculateOrigins()
        {
            leftOrigins = new Vector2[(int)(GO.Size.Y / 0.25f) + 2];
            rightOrigins = new Vector2[(int)(GO.Size.Y / 0.25f) + 2];
            botOrigins = new Vector2[(int)(GO.Size.X / 0.25f) + 2];
            topOrigins = new Vector2[(int)(GO.Size.X / 0.25f) + 2];
            leftCast = new RaycastResult[leftOrigins.Length];
            rightCast = new RaycastResult[leftOrigins.Length];
            topCast = new RaycastResult[botOrigins.Length];
            botCast = new RaycastResult[botOrigins.Length];

            for (int i = 0; i < leftOrigins.Length; i++)
            {
                leftOrigins[i] = new Vector2(0, i * (GO.Size.Y / (leftOrigins.Length - 1)));
                rightOrigins[i] = new Vector2(GO.Size.X, i * (GO.Size.Y / (rightOrigins.Length - 1)));
            }

            for (int i = 0; i < topOrigins.Length; i++)
            {
                topOrigins[i] = new Vector2(i * (GO.Size.X / (topOrigins.Length - 1)), 0);
                botOrigins[i] = new Vector2(i * (GO.Size.X / (botOrigins.Length - 1)), GO.Size.Y);
            }

            topRight = new Vector2(GO.Size.X, 0);
            botLeft = new Vector2(0, GO.Size.Y);
            botRight = new Vector2(GO.Size.X, GO.Size.Y);
        }

        public bool LeftGrounded
        {
            get
            {
                if (!initiated) return false;
                if (botCast[0].distance == 0)
                    return true;
                return false;
            }
        }

        public bool RightGrounded
        {
            get
            {
                if (!initiated) return false;
                if (botCast[botCast.Length - 1].distance == 0)
                    return true;
                return false;
            }
        }

        public bool Grounded
        {
            get
            {
                if (!initiated) return false;
                foreach (RaycastResult rcr in botCast)
                    if (rcr.distance == 0)
                        return true;
                return false;
            }
        }

        public bool WallRightHit
        {
            get
            {
                if (!initiated) return false;
                foreach (RaycastResult rcr in rightCast)
                    if (rcr.distance == 0)
                        return true;
                return false;
            }
        }

        public bool WallLeftHit
        {
            get
            {
                if (!initiated) return false;
                foreach (RaycastResult rcr in leftCast)
                    if (rcr.distance == 0)
                        return true;
                return false;
            }
        }

        public bool CeilingHit
        {
            get
            {
                if (!initiated) return false;
                foreach (RaycastResult rcr in topCast)
                    if (rcr.distance == 0)
                        return true;
                return false;
            }
        }

        public float DistanceToFloor
        {
            get
            {
                if (!initiated) return 0;
                float res = 100;
                foreach (RaycastResult rcr in botCast)
                    if (rcr.distance < res)
                        res = rcr.distance;
                return res;
            }
        }
    }
}

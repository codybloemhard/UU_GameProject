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
    public class CMageBoss : Component
    {
        private float ctime;
        private float maxYSpeed = 3, acceleration = 5;
        private bool initiated, disappearing, appearing;
        private CRaycasts cRaycasts;
        private FSM fsm = new FSM();
        private Vector2 targetPosition, newTarget, targetSize, velocity;

        public override void Update(float time)
        {
            if (!initiated) InitMage();
            base.Update(time);
            ctime = time;

            if (Input.GetKey(PressAction.PRESSED, Keys.T))
                LightningBolt(new Vector2(8, 8));

            if (Input.GetKey(PressAction.PRESSED, Keys.Y) && fsm.CurrentState != "teleport")
            {
                newTarget = new Vector2(MathH.random.Next(2, 12), MathH.random.Next(2, 7));
                targetPosition = newTarget;
                disappearing = true;
                fsm.SetCurrentState("teleport");
            }

            //fsm.SetCurrentState("stay");
            fsm.Update();
        }

        private void InitMage()
        {
            initiated = true;
            cRaycasts = GO.GetComponent<CRaycasts>();
            targetPosition = new Vector2(MathH.random.Next(2, 12), MathH.random.Next(2, 7));
            fsm.Add("stay", StayInPlace);
            fsm.Add("teleport", Teleport);
            targetSize = GO.Size;
        }

        private void LightningBolt(Vector2 target)
        {
            GameObject lightning = new GameObject("lightningbolt" + GO.tag, GO.Context);
            lightning.AddComponent(new CRender("block"));
            lightning.AddComponent(new CLightningBolt(target));
            lightning.Size = new Vector2(.3f);
            lightning.Pos = new Vector2(target.X - lightning.Size.X/2, GO.Pos.Y);
        }

        private void StayInPlace()
        {
            float sign = Math.Sign(targetPosition.Y - (GO.Pos.Y + GO.Size.Y/2));
            velocity.Y += sign * acceleration * ctime;

            float velocitySign = Math.Sign(velocity.Y);
            velocity.Y = sign * Math.Min(sign * velocity.Y, maxYSpeed);
            GO.Pos += velocity * ctime;
        }

        private void Teleport()
        {
            if(disappearing)
            {
                if (GO.Size.X - targetSize.X * 0.5f * ctime < 0 || GO.Size.Y - targetSize.Y * 0.5f * ctime < 0)
                {
                    disappearing = false;
                    appearing = true;
                    GO.Pos = newTarget;
                }
                else GO.Size -= targetSize * 0.5f * ctime;
            }

            if (appearing)
            {
                GO.Size = new Vector2(Math.Min(GO.Size.X + targetSize.X * 0.5f * ctime, targetSize.X), Math.Min(GO.Size.Y + targetSize.Y * 0.5f * ctime, targetSize.Y));
                if (GO.Size == targetSize)
                {
                    appearing = false;
                    fsm.SetCurrentState("stay");
                }
            }
        }
    }
}

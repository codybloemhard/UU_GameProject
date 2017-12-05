using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    class CMovable : Component
    {
        private static bool staticGrabbed;
        private bool grabbed, axisAligned;
        private Vector2 grabPoint;
        public override void Update(float time)
        {
            base.Update(time);
            if (Input.GetMouseButton(PressAction.PRESSED, MouseButton.LEFT) && !staticGrabbed && GO.GetAABB().Inside(Input.GetMousePosition()))
            {
                staticGrabbed = true;
                grabbed = true;
                grabPoint = Input.GetMousePosition() - GO.Pos;
            }
            else if (Input.GetMouseButton(PressAction.RELEASED, MouseButton.LEFT))
            {
                grabbed = staticGrabbed = false;
                grabPoint = Vector2.Zero;
            }

            if (Input.GetKey(PressAction.DOWN, Keys.LeftShift))
                axisAligned = true;
            else
                axisAligned = false;

            if (grabbed)
            {
                Vector2 mousePos = Input.GetMousePosition();

                if (!axisAligned)
                    GO.Pos = mousePos - grabPoint;
                else
                    GO.Pos = new Vector2((int)mousePos.X - grabPoint.X, (int)mousePos.Y - grabPoint.Y);
            }
        }
    }
}

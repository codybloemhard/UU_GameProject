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
    class CLevelEditorObject : Component
    {
        private List<string> components = new List<string>();
        private static bool staticGrabbed, staticSelected;
        private bool grabbed, axisAligned;
        private static GameObject selected;
        private Vector2 grabPoint;
        public override void Update(float time)
        {
            base.Update(time);
            if (Input.GetMouseButton(PressAction.PRESSED, MouseButton.LEFT))
            {
                if (GO.GetAABB().Inside(Input.GetMousePosition()))
                {
                    selected = GO;
                    if (!staticGrabbed)
                    {
                        staticGrabbed = true;
                        grabbed = true;
                        grabPoint = Input.GetMousePosition() - GO.Pos;
                    }
                }

                else
                {
                    if (selected == GO)
                        selected = null;
                }
            }
            else if (Input.GetMouseButton(PressAction.RELEASED, MouseButton.LEFT))
            {
                grabbed = staticGrabbed = false;
                grabPoint = Vector2.Zero;
            }

            if (selected == GO)
            {
                GO.Renderer.colour = new Color(180, 180, 180);
                if (Input.GetKey(PressAction.PRESSED, Keys.P))
                {
                    Console.WriteLine("New Size x y");
                    GO.Size = new Vector2(Int32.Parse(Console.ReadLine()), Int32.Parse(Console.ReadLine()));
                }
            }
            else
                GO.Renderer.colour = Color.White;
            
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
                    GO.Pos = new Vector2((int)mousePos.X - (int)grabPoint.X, (int)mousePos.Y - (int)grabPoint.Y);
            }
        }
    }
}

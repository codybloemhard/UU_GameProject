using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Core;
//<author:cody>
namespace UU_GameProject
{
    public class CFreeCamera : Component
    {
        private float speed = 5f;
        public CFreeCamera() : base() { }

        public override void Update(float time)
        {
            base.Update(time);
            Vector2 displacement = Vector2.Zero;
            if (Input.GetKey(PressAction.DOWN, Keys.W))
                displacement.Y = -speed;
            if (Input.GetKey(PressAction.DOWN, Keys.S))
                displacement.Y = +speed;
            if (Input.GetKey(PressAction.DOWN, Keys.A))
                displacement.X = -speed;
            if (Input.GetKey(PressAction.DOWN, Keys.D))
                displacement.X = +speed;
            displacement *= time;
            GO.Pos += displacement;
            Camera.SetCameraTopLeft(GO.Pos + GO.Size/2f - new Vector2(16,9)/2f);
        }
    }
}
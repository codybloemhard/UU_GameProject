using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CCamera : Component
    {
        private Vector2 campos;
        private Vector2 middle = new Vector2(8f, 4.5f);

        public CCamera() : base()
        {
            campos = middle;
        }

        public override void Update(float time)
        {
            base.Update(time);
            Vector2 target = GO.Pos + GO.Size / 2f;
            Vector2 diff = campos - target;
            float distx = ExpTrans(Math.Abs(diff.X / middle.X));
            float disty = ExpTrans(Math.Abs(diff.Y / middle.Y));
            Vector2 to = target;
            Vector2 from = campos;
            Vector2 move = Lerp(from, to, distx, disty);
            Camera.SetCameraTopLeft(move - middle);
            campos = move;
        }
        //math functions not centralized, to be refactored
        public float Lerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }

        public Vector2 Lerp(Vector2 oldp, Vector2 newp, float xt, float yt)
        {
            Vector2 ans = new Vector2(0, 0);
            ans.X = Lerp(oldp.X, newp.X, xt);
            ans.Y = Lerp(oldp.Y, newp.Y, yt);
            return ans;
        }

        public float ExpTrans(float t)
        {
            return MathHelper.Clamp((float)Math.Pow(t, 9), 0, 1);
        }
    }
}
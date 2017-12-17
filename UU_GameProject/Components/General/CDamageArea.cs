using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject.Components.General
{
    class CDamageArea : Component
    {
        public CDamageArea() : base() { }

        public override void Update(float time)
        {
            base.Update(time);
        }

        public void CreateDamageArea(Vector2 origin, Vector2 dimensions, int damage = 1, int interval = 5)
        {

        }
    }
}

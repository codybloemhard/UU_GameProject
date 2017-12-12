using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject.Components
{
    class CMeleeAttack : Component
    {
        public CMeleeAttack() : base() { }

        public void melee(int dmagae, float reach)
        {
            GO.Raycast(GO.Pos + new Vector2(0, GO.Size.Y), Vector2.Zero, RAYCASTTYPE.STATIC);
        }
    }
}

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

        public string melee(Vector2 dir, int dmagae, float reach)
        {
            RaycastResult ray;
            ray = GO.Raycast(GO.Pos + new Vector2(0, GO.Size.Y / 2), dir, RAYCASTTYPE.STATIC);
            if (ray.distance <= reach)
                return ray.obj.tag;
            else
                return "null";
        }
    }
}

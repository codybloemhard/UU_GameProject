using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace UU_GameProject
{
    class CMeleeAttack : Component
    {
        public CMeleeAttack() : base() { }

        public void melee(Vector2 dir, int damage, float reach)
        {
            RaycastResult ray1;
            RaycastResult ray2;
            RaycastResult ray3;
            CHealthBar hp = null;

            ray1 = GO.Raycast(GO.Pos + new Vector2(0, GO.Size.Y / 2), dir, RAYCASTTYPE.STATIC);
            ray2 = GO.Raycast(GO.Pos + new Vector2(0, GO.Size.Y / 2), dir + new Vector2(0, 0), RAYCASTTYPE.STATIC);
            ray3 = GO.Raycast(GO.Pos + new Vector2(0, GO.Size.Y / 2), dir + new Vector2(0, 0), RAYCASTTYPE.STATIC);

            if (ray1.distance <= reach)
                hp = ray1.obj.GetComponent<CHealthBar>();
            else if (ray2.distance <= reach)
                hp = ray2.obj.GetComponent<CHealthBar>();
            else if (ray3.distance <= reach)
                hp = ray3.obj.GetComponent<CHealthBar>();

            if (hp != null)
                hp.hit(damage);

            Console.WriteLine(ray1.obj.tag + ", " + ray2.obj.tag + ", " + ray2.obj.tag);
        }
    }
}

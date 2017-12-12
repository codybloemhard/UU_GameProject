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
            RaycastResult ray = null;
            RaycastResult ray1;
            RaycastResult ray2;
            RaycastResult ray3;
            CHealthBar hp = null;
            float x = 0;

            if (dir.X > 0)
                x = GO.Size.X/2;
            else if (dir.X < 0)
                x = -GO.Size.X/2;

            ray1 = GO.Raycast(GO.Pos + new Vector2(x, GO.Size.Y / 2), dir + new Vector2(0, 1), RAYCASTTYPE.DYNAMIC);
            ray2 = GO.Raycast(GO.Pos + new Vector2(x, GO.Size.Y / 2), dir, RAYCASTTYPE.DYNAMIC);
            ray3 = GO.Raycast(GO.Pos + new Vector2(x, GO.Size.Y / 2), dir + new Vector2(0, -1), RAYCASTTYPE.DYNAMIC);

            if (ray1.distance <= reach)
                ray = ray1;
            else if (ray2.distance <= reach)
                ray = ray2;
            else if (ray3.distance <= reach)
                ray = ray3;

            if (ray != null)
            {
                if (hp != null && ray.obj.tag != GO.tag && ray.obj.tag != "Aenemy")
                {
                    ray.obj.GetComponent<CHealthBar>().hit(damage);
                }
                else if (ray.obj.tag == "Aenemy" && ray.obj.tag != GO.tag && -ray.obj.GetComponent<CArmouredEnemyAI>().direction() != dir)
                {
                    ray.obj.GetComponent<CHealthBar>().hit(damage);
                }
            }
        }
    }
}

using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    public class CMeleeAttack : Component
    {
        public CMeleeAttack() : base() { }

        public void Melee(Vector2 dir, Vector2 dimensions, float duration, string caller)
        {
            GameObject meleeDamageArea = new GameObject("meleeDamageArea" + GO.tag, GO.Context, 0);
            meleeDamageArea.AddComponent(new CRender("block"));
            meleeDamageArea.AddComponent(new CDamageArea(dir, duration, caller));
            meleeDamageArea.AddComponent(new CAABB());
            if (dir.X > 0)
                meleeDamageArea.Pos = GO.Pos + new Vector2(dimensions.X / 2f, 0);
            else
                meleeDamageArea.Pos = GO.Pos + new Vector2(-dimensions.X / 2f, 0);
            meleeDamageArea.Size = dimensions;
        }
    }
}

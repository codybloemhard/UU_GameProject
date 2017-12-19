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

        public override void Update(float time)
        {
            base.Update(time);
        }

        public void Melee(Vector2 dir, Vector2 dimensions, float duration)
        {
            GameObject meleeDamageArea = new GameObject("meleeDamageArea", GO.Context, 0);
            meleeDamageArea.AddComponent(new CRender("block"));
            meleeDamageArea.AddComponent(new CDamageArea(duration));
            meleeDamageArea.AddComponent(new CAABB());
            if (dir.X > 0)
                meleeDamageArea.Pos = GO.Pos + GO.Size / 2f - dimensions / 2f + new Vector2(GO.Size.X / 2f + dimensions.X, 0);
            else
                meleeDamageArea.Pos = GO.Pos + GO.Size / 2f - dimensions / 2f - new Vector2(GO.Size.X / 2f + dimensions.X, 0);
            meleeDamageArea.Size = dimensions;
        }
    }
}

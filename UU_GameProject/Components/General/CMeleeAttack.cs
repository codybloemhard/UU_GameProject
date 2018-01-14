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

        public override void Update(float time)
        {
            base.Update(time);
        }

        public void Melee(Vector2 dir, Vector2 dimensions, float duration, float damage, bool potionous, string caller, string Faction)
        {
            GameObject meleeDamageArea = new GameObject("meleeDamageArea" + GO.tag, GO.Context, 0);
            meleeDamageArea.AddComponent(new CRender("block"));
            meleeDamageArea.AddComponent(new CDamageArea(dir, duration, caller, damage, potionous));
            meleeDamageArea.AddComponent(new CAABB());
            meleeDamageArea.AddComponent(new CFaction(Faction));
            if (dir.X > 0)
                meleeDamageArea.Pos = GO.Pos + new Vector2(dimensions.X / 2f, 0);
            else
                meleeDamageArea.Pos = GO.Pos + new Vector2(-dimensions.X / 2f, 0);
            meleeDamageArea.Size = dimensions;
        }
    }
}

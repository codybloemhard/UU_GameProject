using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    class CHealthBar : Component
    {
        private int HP;
        Text healthBar;
        public CHealthBar(int HP, GameObject GO)
        {
            this.HP = HP;
            healthBar = new Text(GO.Context, "Health: " + HP, new Vector2(0, 0), new Vector2(3, 1), AssetManager.GetResource<SpriteFont>("mainFont"));
            healthBar.tag = "HP";
            healthBar.AddGameObject(GO);
        }

        public override void Update(float time)
        {
            base.Update(time);
        }


        public void hit(int i)
        {
            HP = Math.Max(0, HP - i);
            if (HP == 0)
                GO.active = false;
        }

        public int hp
        { get { return this.HP; } }
    }
}

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
            healthBar.AddGameObject(GO, Vector2.Zero);
        }

        public override void Update(float time)
        {
            base.Update(time);


        }

        public void update()
        {
            Text text = GO.Context.ui.FindWithTag("positionText") as Text;
        }

        public void hit(int i)
        {
            HP = HP - i;
        }

        public int hp
        { get { return this.HP; } }
    }
}

using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    class CHealthPool : Component
    {
        private int HP;
        Text healthPool;
        public CHealthPool(int HP, GameObject GO)
        {
            this.HP = HP;
            healthPool = new Text(GO.Context, "Health: " + HP, new Vector2(0, 0), new Vector2(4, 0), AssetManager.GetResource<SpriteFont>("mainFont"));
            healthPool.AddGameObject(GO);
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if (other.tag != "killer")
                return;
            HP -= 1;
            healthPool.text = "Health: " + HP;
            if (HP <= 0)
                GameStateManager.RequestChange("gameover", CHANGETYPE.LOAD);
        }

        //method to be called for instances that change HP
        public void ChangeHealth(int amount)
        {
            HP -= amount;
            healthPool.text = "Health: " + HP;
            if (HP <= 0)
                GameStateManager.RequestChange("gameover", CHANGETYPE.LOAD);
        }
    }
}

﻿using System;
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
            healthBar.AddGameObject(GO);
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if (other.tag != "killer")
                return;
            HP -= 1;
            healthBar.text = "Health: " + HP;
            if (HP <= 0)
                GameStateManager.RequestChange("gameover", CHANGETYPE.LOAD);
        }
        
    }
}

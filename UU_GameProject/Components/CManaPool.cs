﻿using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    class CManaPool : Component
    {
        private int MP;
        private int maxMana = 100;
        Text manaPool;
        public CManaPool(int MP, GameObject GO)
        {
            this.MP = MP;
            manaPool = new Text(GO.Context, "Mana: " + MP, new Vector2(0, 0), new Vector2(4, 1), AssetManager.GetResource<SpriteFont>("mainFont"));
            manaPool.AddGameObject(GO);
        }

        public override void Update(float time)
        {
            base.Update(time);
            RegenerateMana();
        }

        //method to be called for abilities using mana
        public void ConsumeMana(int amount)
        {
            MP -= amount;
            manaPool.text = "Mana: " + MP;
        }

        public void RegenerateMana()
        {
            if (MP < maxMana)
                MP += 1;
            manaPool.text = "Mana: " + MP;
        }

        public int ReturnMana()
        {
            return MP;
        }
    }
}
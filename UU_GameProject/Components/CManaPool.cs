using System;
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
        private bool shouldManaRegen = true;
        Text manaPool;
        public CManaPool(int MP, GameObject GO)
        {
            this.MP = MP;
            manaPool = new Text(GO.Context, "Mana: " + MP, new Vector2(0, 0), new Vector2(4, 1), AssetManager.GetResource<SpriteFont>("mainFont"));
            manaPool.AddGameObject(GO, Vector2.Zero);
        }

        public override void Update(float time)
        {
            base.Update(time);
            RegenerateMana(time);
            manaPool.text = "Mana: " + MP;
        }

        public int sufficientMana()
        {
            return MP;
        }

        //method to be called for abilities using mana
        public bool ConsumeMana(int amount)
        {
            if (MP >= amount)
            {
                MP -= amount;
                return true;
            }
            else
            {
                Console.WriteLine("Not enough mana!");
                return false;
            }

        }

        public void manaRegenerateTimer()
        {
            shouldManaRegen = true;
        }

        public void RegenerateMana(float time)
        {
            if (MP < maxMana && shouldManaRegen)
            {
                Timers.Add("manaRegen", 0.03f, manaRegenerateTimer);
                MP += 1;
                shouldManaRegen = false;
                Timers.FindWithTag("manaRegen").Reset();
            }
        }
    }
}

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
        private float manaRegenMultiplier;
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

        /// <summary>
        /// Method that will attempt to consume the amount of mana specified,
        /// and will return true if the requested amount is succesfully subtracted.
        /// Otherwise will return false, and not subtract any.
        /// </summary>
        /// <param name="amount">Mana cost of an ability</param>
        /// <returns></returns>
        public bool ConsumeMana(int amount)
        {
            if (MP >= amount)
            {
                MP -= amount;
                return true;
            }
            else
            {
                Console.WriteLine("Not enough mana!"); //<- placeholder for any not-enough-mana-message
                return false;
            }

        }

        //method to be called by the regen timer
        public void manaRegenerateTimer()
        {
            shouldManaRegen = true;
        }

        public void RegenerateMana(float time)
        {
            if (MP < maxMana && shouldManaRegen)
            {
                manaRegenMultiplier = 3.0f - Math.Min(Math.Abs(GO.GetComponent<CPlayerMovement>().Velocity().X), 2.9f);
                Timers.Add("manaRegen", 0.03f * manaRegenMultiplier, manaRegenerateTimer);
                MP += 1;
                shouldManaRegen = false;
                Timers.FindWithTag("manaRegen").Reset();
            }
        }
    }
}

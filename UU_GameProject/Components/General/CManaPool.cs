using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    public class CManaPool : Component
    {
        private int mana;
        private int maxMana = 100;
        private float manaRegenMultiplier;
        private bool shouldManaRegen = true;

        public CManaPool(int MP, GameObject GO)
        {
            this.mana = MP;
        }

        public override void Update(float time)
        {
            base.Update(time);
            RegenerateMana(time);
        }

        public void Reset()
        {
            mana = maxMana;
            shouldManaRegen = true;
        }

        //true when has enough, false when not and will not subtract
        public bool ConsumeMana(int amount)
        {
            if (mana >= amount)
            {
                mana -= amount;
                return true;
            }
            else
            {
                Console.WriteLine("Not enough mana!"); //<- placeholder for any not-enough-mana-message
                return false;
            }
        }

        public bool PeekMana(int amount)
        {
            if (mana >= amount)
                return true;
            return false;
        }

        //method to be called by the regen timer
        public void manaRegenerateTimer()
        {
            shouldManaRegen = true;
        }

        public void RegenerateMana(float time)
        {
            if (mana < maxMana && shouldManaRegen)
            {
                manaRegenMultiplier = 3.0f - Math.Min(Math.Abs(GO.GetComponent<CPlayerMovement>().Velocity().X), 2.9f);
                Timers.Add("manaRegen", 0.03f * manaRegenMultiplier, manaRegenerateTimer);
                mana += 1;
                shouldManaRegen = false;
                Timers.FindWithTag("manaRegen").Reset();
            }
        }

        public float ManaPercentage { get { return (float)mana / maxMana; } }
    }
}
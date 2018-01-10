using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    public class CHealthPool : Component
    {
        private int HP;
        private int maxHP;
        private int Duration;
        private float amount;
        private float regenExcess;
        private float timer = 0;
        private bool untilEnd;
        private bool Heal;
        private int bulletHitDamage = 10;
        private int meleeHitDamage = 25;
        private int lightningDamage = 20;
        private int fireballDamage = 12;
        private bool isInvincible = false;
        private Text healthPool;

        public CHealthPool(int HP, GameObject GO)
        {
            this.HP = HP;
            maxHP = HP;
            healthPool = new Text(GO.Context, "Health: " + HP, new Vector2(0, 0), new Vector2(4, 0), AssetManager.GetResource<SpriteFont>("mainFont"));
            healthPool.AddGameObject(GO, Vector2.Zero);
        }

        public void Update()
        {
            if (amount != 0 && MP < maxMana && shouldManaRegen)
            {
                manaRegenMultiplier = 3.0f - Math.Min(Math.Abs(GO.GetComponent<CPlayerMovement>().Velocity().X), 2.9f);
                Timers.Add("manaRegen", 0.03f * manaRegenMultiplier, manaRegenerateTimer);
                MP += 1;
                shouldManaRegen = false;
                Timers.FindWithTag("manaRegen").Reset();
            }
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if (other.tag.Contains(GO.tag))
                return;
            if (!other.IsStatic) 
            {
                if (GO.GetComponent<Components.General.CFaction>().ClashingFactions(GO, other) == true)
                {
                    if (other.tag.Contains("bullet"))
                    {
                        ChangeHealth(bulletHitDamage);
                        other.Destroy();
                    }
                    if (other.tag.Contains("meleeDamageArea"))
                        ChangeHealth(meleeHitDamage);
                    if (other.tag.Contains("lightningStrike"))
                        ChangeHealth(lightningDamage);
                    if (other.tag.Contains("fireball"))
                    {
                        ChangeHealth(fireballDamage);
                        other.Destroy();
                    }


                }
            }
        }

        //method to be called for gradually regenerating or depleting health (i.e. poison)
        public void GradualChange(int Duration, int totalAmount, bool Heal)
        {
            this.Heal = Heal;
            this.Duration = Duration;
            amount = totalAmount / this.Duration;
        }
        //GradualChange, but then to heal until HP is full or damage until HP is empty
        public void GradualChangeUntilEnd(float amountPerSecond, bool Heal)
        {
            this.Heal = Heal;
            untilEnd = true;
            amount = amountPerSecond;
        }



        //method to be called for instances that change HP
        /// <summary>
        /// Reduces the health of a character by the amount specified, use -NUMBER for healsies
        /// </summary>
        /// <param name="amount">Positive: Take damage, Negative: Recieve heals</param>
        public void ChangeHealth(int amount)
        {
            if (!isInvincible)
            {
                HP = Math.Max(0, HP - amount);
                isInvincible = true;
                Timers.Add("manaRegen", 0.5f, ResetInvincibility);
                healthPool.text = "Health: " + HP;
                if (HP <= 0)
                {
                    if (GO.tag.Contains("player"))
                        GameStateManager.RequestChange("gameover", CHANGETYPE.LOAD);
                    else GO.active = false;
                }
            }
        }

        private void ResetInvincibility()
        {
            isInvincible = false;
        }
    }
}

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
        private int bulletHitDamage = 10;
        private int meleeHitDamage = 25;
        private bool isInvincible = false;
        Text healthPool;
        public CHealthPool(int HP, GameObject GO)
        {
            this.HP = HP;
            healthPool = new Text(GO.Context, "Health: " + HP, new Vector2(0, 0), new Vector2(4, 0), AssetManager.GetResource<SpriteFont>("mainFont"));
            healthPool.AddGameObject(GO, Vector2.Zero);
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if (other.tag.Contains(GO.tag))
                return;
            if (other.tag.Contains("bullet"))
                ChangeHealth(bulletHitDamage);
            if (other.tag.Contains("meleeDamageArea"))
                ChangeHealth(meleeHitDamage);
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

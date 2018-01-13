using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    public class CHealthPool : Component
    {
        private float hp;
        private int maxHP;
        private int bulletHitDamage = 10;
        private int meleeHitDamage = 25;
        private int lightningDamage = 20;
        private int fireballDamage = 12;
        private bool isInvincible = false;
        private float healTime = 0f;
        private float healRate = 0f;
        public bool isProtected = false;

        public CHealthPool(int HP)
        {
            this.hp = HP;
            maxHP = HP;
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Update(float time)
        {
            if(healTime > 0f)
            {
                healTime -= time;
                if (time < 0) time = 0;
                ModifyHP(healRate * time);
            }
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if (other.tag.Contains(GO.tag)) return;
            if (other.IsStatic) return;
            if (!GO.GetComponent<CFaction>().ClashingFactions(GO, other)) return;
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

        public void Reset()
        {
            hp = maxHP;
            isInvincible = false;
            isProtected = false;
            healTime = 0f;
            healRate = 0f;
        }
        
        public void HealOverTime(float rate, float time)
        {
            if ((healRate < 0f && rate > 0f)
                && (healRate > 0f && rate < 0f))//cancel potion or healing
            {
                healRate = 0f;
                healTime = 0f;
                return;
            }
            healTime = time;
            healRate = rate;
        }
        
        //amount > 0 :: take dmg, amount < 0 :: heal
        public void ChangeHealth(float amount)
        {
            if (!isInvincible && !isProtected)
            {
                isInvincible = true;
                Timers.Add("hpRegen", 0.5f, ResetInvincibility);
                ModifyHP(amount);
            }
        }

        private void ModifyHP(float amount)
        {
            hp = Math.Max(0f, hp - amount);
            if (hp <= 0) Die();
            if (hp > maxHP) hp = maxHP;
            if(amount > 0)
                AudioManager.PlayEffect("hit");
        }

        private void Die()
        {
            if (GO.tag.Contains("player"))
                GO.GetComponent<CPlayerMovement>().Reset();
            else
            {
                GO.Destroy();
                AudioManager.PlayEffect("kill");
            }
        }

        private void ResetInvincibility()
        {
            isInvincible = false;
        }

        public float Health { get { return hp; } }
        public float HealhPercent { get { return hp / maxHP; } }
    }
}
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
        private bool isInvincible = false;
        private float healTime = 0f;
        private float healRate = 0f;
        public bool isProtected = false;
        
        public CHealthPool(int HP)
        {
            this.hp = HP;
            maxHP = HP;
        }

        public void InitHP(int HP)
        {
            hp = HP;
            maxHP = HP;
        }

        public override void Update(float time)
        {
            if(healTime > 0f)
            {
                healTime -= time;
                if (time < 0) time = 0;
                ModifyHP(healRate * time, true);
            }
        }
        
        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if (other.tag.Contains(GO.tag)) return;
            if (other.IsStatic) return;
            CFaction faction = GO.GetComponent<CFaction>();
            if (faction == null) return;
            if (!faction.ClashingFactions(GO, other)) return;
            CDamageDealer comp = other.GetComponent<CDamageDealer>();
            if (comp == null) return;
            bool applPotion = comp.Potionous;
            
            if (other.tag == "bullet")
            {
                ChangeHealth(comp.Damage, false);
                other.Destroy();
            }
            else if(other.tag.Contains("meleeDamageArea"))
                ChangeHealth(comp.Damage, true);
            else if(other.tag.Contains("lightningStrike"))
                ChangeHealth(comp.Damage, true);
            else if(other.tag.Contains("fireball"))
            {
                ChangeHealth(comp.Damage, false);
                other.Destroy();
            }
            else if (other.tag == "cyborgboss")
                ChangeHealth(comp.Damage, true);
            else if (other.tag == "robotboss" && other.GetComponent<CRobotBoss>().Chasing)
                other.GetComponent<CRobotBoss>().Explode();
            else if (other.tag.Contains("explobullet"))
            {
                ChangeHealth(comp.Damage, false);
                other.Destroy();
            }
            else if (other.tag.Contains("explosion"))
                ChangeHealth(comp.Damage, false);
            else if (other.tag.Contains("chase"))
                ChangeHealth(comp.Damage, true);

            if (applPotion) HealOverTime(4f, 10f);
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
                || (healRate > 0f && rate < 0f))//cancel poison or simply heals
            {
                healRate = 0f;
                healTime = 0f;
                return;
            }
            healTime = time;
            healRate = rate;
        }
        
        //amount > 0, take dmg | amount < 0, heal
        public void ChangeHealth(float amount, bool useInvincible)
        {
            if (!isInvincible && !isProtected && useInvincible)
            {
                isInvincible = true;
                Timers.Add("hpRegen", 0.5f, () => isInvincible = false);
                ModifyHP(amount);
            }
            else if (!useInvincible)
                ModifyHP(amount);
        }

        private void ModifyHP(float amount, bool fromPotion = false)
        {
            hp = Math.Max(0f, hp - amount);
            if (hp <= 0) Die();
            if (hp > maxHP) hp = maxHP;
            if(amount > 0 && !fromPotion)
                AudioManager.PlayEffect("hit");
        }

        private void Die()
        {
            if (GO.tag.Contains("player"))
                GO.GetComponent<CPlayerMovement>().Reset();
            else if (GO.tag.Contains("cyborgboss"))
                GO.GetComponent<CCyborgBoss>().Split();
            else
            {
                GO.Destroy();
                AudioManager.PlayEffect("kill");
            }
        }

        public float Health { get { return hp; } }
        public float HealhPercent { get { return hp / maxHP; } }
    }
}
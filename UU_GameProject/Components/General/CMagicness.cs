using System;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CMagicness : Component
    {
        private Vector2 dir;
        private bool unlockedLightning, unlockedFitness, unlockedHealing;
        private bool iniated = false;
        private CManaPool manaPool;
        private CHealthPool healthPool;
        private const int lightningCost = 75, healingCost = 50,
            dashCost = 25, jumpCost = 50;

        public CMagicness() : base() { }

        private void Init()
        {
            iniated = true;
            manaPool = GO.GetComponent<CManaPool>();
            healthPool = GO.GetComponent<CHealthPool>();
            unlockedLightning = true;
            unlockedFitness = true;
            unlockedHealing = true;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!iniated) Init();
        }

        public void Fireball(Vector2 size, Vector2 playerSpeed, string Faction)
        {
            if (!manaPool.ConsumeMana(10)) return;
            if (Input.GetMousePosition().X >= GO.Pos.X)
                dir = new Vector2(1, 0);
            else dir = new Vector2(-1,0);
            GameObject fireball = new GameObject("fireball", GO.Context, 0);
            if (dir.X > 0)
                fireball.Pos = GO.Pos + GO.Size / 2f - size / 2f + new Vector2(GO.Size.X / 2f + size.X, 0);
            else
                fireball.Pos = GO.Pos + GO.Size / 2f - size / 2f - new Vector2(GO.Size.X / 2f + size.X, 0);
            fireball.Size = size;
            fireball.AddComponent(new CRender("block"));
            fireball.AddComponent(new CFireballMovement(playerSpeed, (Input.GetMousePosition() - (fireball.Pos + .5f * (fireball.Size))), dir));
            fireball.AddComponent(new CAABB());
            fireball.AddComponent(new CFaction(Faction));
            AudioManager.PlayEffect("shoot");
        }

        public void Lightning(Vector2 dimensions, float duration, string caller, string Faction)
        {
            if (!unlockedLightning) return;
            if (!manaPool.ConsumeMana(lightningCost)) return;
            GameObject lightningStrike = new GameObject("lightningStrike" + GO.tag, GO.Context, 0);
            lightningStrike.AddComponent(new CRender("block"));
            lightningStrike.AddComponent(new CLightningStrike(duration, caller));
            lightningStrike.AddComponent(new CAABB());
            lightningStrike.AddComponent(new CFaction(Faction));
            lightningStrike.Pos = Input.GetMousePosition() - new Vector2(dimensions.X / 2f, dimensions.Y / 2f);
            lightningStrike.Size = dimensions;
            AudioManager.PlayEffect("lightning");
        }

        public void Heal()
        {
            if (!unlockedHealing) return;
            if (!manaPool.ConsumeMana(healingCost)) return;
            GO.GetComponent<CHealthPool>().HealOverTime(-10f, 5f);
            AudioManager.PlayEffect("healing");
        }

        public bool Dash()
        {
            if (!unlockedFitness) return false;
            if (manaPool.ConsumeMana(dashCost)) return true;
            return false;
        }

        public bool DoubleJump()
        {
            if (!unlockedFitness) return false;
            if (manaPool.ConsumeMana(jumpCost)) return true;
            return false;
        }

        public bool CanLightning { get
        {
            if (manaPool == null) return false;
            return manaPool.PeekMana(lightningCost);
        } }
        public bool CanHeal { get
        {
            if (manaPool == null) return false;
            return manaPool.PeekMana(healingCost);
        } }
        public bool CanDoublejump { get
        {
            if (manaPool == null) return false;
            return manaPool.PeekMana(jumpCost);
        } }
        public bool CanDash { get
        {
            if (manaPool == null) return false;
            return manaPool.PeekMana(dashCost);
        } }

        public bool UnlockedLightning { get { return unlockedLightning; } }
        public bool UnlockedHealing { get { return unlockedHealing; } }
        public bool UnlockedFitness { get { return unlockedFitness; } }
    }
}
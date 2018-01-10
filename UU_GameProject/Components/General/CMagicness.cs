using System;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CMagicness : Component
    {
        private Vector2 dir;
        private float regenTime;

        public CMagicness() : base() { }

        public override void Update(float time)
        {
            base.Update(time);
        }

        public void Fireball(Vector2 size, Vector2 playerSpeed, string Faction)
        {
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

        }

        public void Lightning(Vector2 dimensions, float duration, string caller, string Faction)
        {
            GameObject lightningStrike = new GameObject("lightningStrike" + GO.tag, GO.Context, 0);
            lightningStrike.AddComponent(new CRender("block"));
            lightningStrike.AddComponent(new CLightningStrike(duration, caller));
            lightningStrike.AddComponent(new CAABB());
            lightningStrike.AddComponent(new CFaction(Faction));
            lightningStrike.Pos = Input.GetMousePosition() - new Vector2(dimensions.X / 2f, dimensions.Y / 2f);
            lightningStrike.Size = dimensions;
        }
        public void HealthRegen()
        {

        }
        public void Heal()
        {

        }
    }
}
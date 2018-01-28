using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CLightningBolt : Component
    {
        private Vector2 target;

        public CLightningBolt(Vector2 target)
        {
            this.target = target;
        }

        public override void Update(float time)
        {
            base.Update(time);
            GO.Pos += new Vector2(0, 5) * time;
        }

        //lightningstrike
        private void LightningStrike(Vector2 target)
        {
            GameObject lightningStrike = new GameObject("lightningStrike" + GO.tag, GO.Context, 0);
            CAnimatedSprite animLight = new CAnimatedSprite();
            animLight.AddAnimation("lightningStrike", "lightningStrike");
            animLight.PlayAnimation("lightningStrike", 40);
            lightningStrike.AddComponent(animLight);
            lightningStrike.AddComponent(new CLightningStrike(0.1f, GO.tag, 50, false));
            lightningStrike.AddComponent(new CAABB());
            lightningStrike.AddComponent(new CFaction("enemy"));
            lightningStrike.Size = new Vector2(1);
            lightningStrike.Pos = target - lightningStrike.Size/2;
            AudioManager.PlayEffect("lightning");
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if(other.tag != "boss")
            {
                LightningStrike(GO.Pos + GO.Size/2);
                GO.Destroy();
            }
        }
    }
}

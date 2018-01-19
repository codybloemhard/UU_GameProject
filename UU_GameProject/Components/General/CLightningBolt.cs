﻿using System;
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
            GO.Pos += new Vector2(0, 3) * time;
            if (GO.Pos.Y + GO.Size.Y >= target.Y)
            {
                LightningStrike(target);
                GO.Destroy();
            }
        }

        private void LightningStrike(Vector2 target)
        {
            GameObject lightningStrike = new GameObject("lightningStrike" + GO.tag, GO.Context, 0);
            lightningStrike.AddComponent(new CRender("block"));
            lightningStrike.AddComponent(new CLightningStrike(0.1f, GO.tag, 50, false));
            lightningStrike.AddComponent(new CAABB());
            lightningStrike.AddComponent(new CFaction("enemy"));
            lightningStrike.Size = new Vector2(1);
            lightningStrike.Pos = target - lightningStrike.Size/2;
            AudioManager.PlayEffect("lightning");
        }
    }
}

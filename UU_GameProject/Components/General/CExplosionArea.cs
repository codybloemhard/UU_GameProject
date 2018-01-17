using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CExplosionArea : Component
    {
        private int destroy = 0; 

        public override void Update(float time)
        {
            base.Update(time);
            if (destroy == 100)
                GO.Destroy();
            else destroy += 1;
        }
    }
}

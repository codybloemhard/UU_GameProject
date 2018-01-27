using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    class CSnowFall : Component
    {
        private Vector2 velocity = new Vector2(0, 10), spawn;

        public CSnowFall(Vector2 spawn)
        {
            this.spawn = spawn;
        }

        public override void Update(float time)
        {
            base.Update(time);
            GO.Pos += velocity * time;
            Console.WriteLine(GO.Pos);
            if (Math.Abs((GO.Pos - spawn).Length()) > 50)
                GO.Destroy();
        }
    }
}

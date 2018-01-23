using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace UU_GameProject
{
    class CSpike : Component
    {
        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if (other.tag == "player")
                other.GetComponent<CPlayerMovement>().Reset();
        }
    }
}

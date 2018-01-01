using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace UU_GameProject.Components.General
{
    public class CFaction : Component
    {
        private string Faction;

        public CFaction(string Faction)
        {
            this.Faction = Faction;
        }

        public bool ClashingFactions(GameObject Collisionee, GameObject Collider)
        {
            if (Collisionee.GetComponent<CFaction>().Faction != Collider.GetComponent<CFaction>().Faction)
                return true;
            else return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace UU_GameProject.Components.General
{
    class CFaction : Component
    {
        private string Faction;
        public CFaction(string Faction)
        {
            this.Faction = Faction;
        }
        public string GetFaction()
        {
            return Faction;
        }
        public bool ClashingFactions(GameObject Collisionee, GameObject Collider)
        {
            if (Collisionee.GetComponent<CFaction>().GetFaction() != Collider.GetComponent<CFaction>().GetFaction())
                return true;
            else return false;
        }
    }
}

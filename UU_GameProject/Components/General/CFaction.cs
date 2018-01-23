using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace UU_GameProject
{
    public class CFaction : Component
    {
        private string faction;

        public CFaction(string faction)
        {
            this.faction = faction;
        }
        public string GetFaction()
        {
            return faction;
        }

        //deals with factions
        public bool ClashingFactions(GameObject collisionee, GameObject collider)
        {
            CFaction fac0 = collisionee.GetComponent<CFaction>();
            CFaction fac1 = collider.GetComponent<CFaction>();
            if (fac0 == null || fac1 == null)
                return false;
            if (fac0.GetFaction() != fac1.GetFaction())
                return true;
            else return false;
        }
    }
}
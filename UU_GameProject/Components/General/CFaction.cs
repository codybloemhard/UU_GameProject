﻿using System;
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

        public CFaction(string Faction)
        {
            this.faction = Faction;
        }
        public string GetFaction()
        {
            return faction;
        }

        public bool ClashingFactions(GameObject Collisionee, GameObject Collider)
        {
            CFaction fac0 = Collisionee.GetComponent<CFaction>();
            CFaction fac1 = Collider.GetComponent<CFaction>();
            if (fac0 == null || fac1 == null)
                return false;
            if (fac0.GetFaction() != fac1.GetFaction())
                return true;
            else return false;
        }
    }
}
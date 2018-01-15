using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Core;

namespace UU_GameProject
{
    public class CDamageDealer : Component
    {
        public float Damage { get; private set; }
        public bool Potionous { get; private set; }

        public CDamageDealer(float damage, bool potionous) : base()
        {
            Damage = damage;
            Potionous = potionous;
        }
    }
}
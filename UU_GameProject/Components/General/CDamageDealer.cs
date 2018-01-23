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

        //deals with damage dealers
        public CDamageDealer(float damage, bool poisonous) : base()
        {
            Damage = damage;
            Potionous = poisonous;
        }
    }
}
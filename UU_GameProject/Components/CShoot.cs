﻿using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CShoot : Component
    {
        public CShoot() : base() { }

        public override void Update(float time)
        {
            base.Update(time);
        }

        public void Shoot(Vector2 dir, Vector2 size)
        {
            GameObject bullet = new GameObject("bullet", GO.Context, 0);
            bullet.AddComponent("render", new CRender("block"));
            bullet.AddComponent("move", new CBulletMovement(6, dir));
            bullet.AddComponent("collision", new CAABB());
            bullet.Pos = GO.Pos + GO.Size / 2f - size / 2f;
            bullet.Size = size;
        }
    }
}
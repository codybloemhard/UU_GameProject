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
            bullet.AddComponent(new CRender("block"));
            bullet.AddComponent(new CBulletMovement(6, dir));
            bullet.AddComponent(new CAABB());
            if (dir.X > 0)
                bullet.Pos = GO.Pos + GO.Size / 2f - size / 2f + new Vector2(GO.Size.X / 2f + size.X, 0);
            else
                bullet.Pos = GO.Pos + GO.Size / 2f - size / 2f - new Vector2(GO.Size.X / 2f + size.X, 0);
            bullet.Size = size;
        }
    }
}
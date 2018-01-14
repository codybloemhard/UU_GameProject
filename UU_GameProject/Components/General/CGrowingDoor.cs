using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Core;

namespace UU_GameProject
{
    public class CGrowingDoor : Component
    {
        private CGrowingDoorSprite sprite;
        private CAABB collider;
        private bool done = false;

        public CGrowingDoor() : base() { }

        private void Set()
        {
            GameObject obj = new GameObject("doei", GO.Context, 0, false);
            obj.AddComponent(new CRender("block"));
            obj.Pos = GO.Pos + (Vector2.UnitY * GO.Size.Y);
            obj.Size = GO.Size;
            sprite = new CGrowingDoorSprite(GO.Size.Y);
            obj.AddComponent(sprite);
            collider = new CAABB();
            collider.active = false;
            GO.AddComponent(collider);
            done = true;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!done) Set();
        }

        public void Open()
        {
            collider.active = false;
            sprite.Shrink();
        }

        public void Close()
        {
            collider.active = true;
            sprite.Grow();
        }
    }
}
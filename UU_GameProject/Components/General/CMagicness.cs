using System;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject.Components.General
{
    public class CMagicness : Component
    {
        private Vector2 dir;

        public CMagicness() : base() { }

        public void Fireball(Vector2 size, Vector2 playerSpeed)
        {
            if (Input.GetMousePosition().X >= GO.Pos.X)
                dir = new Vector2(1, 0);
            else dir = new Vector2(-1,0);
            GameObject fireball = new GameObject("fireball", GO.Context, 0);
            if (dir.X > 0)
                fireball.Pos = GO.Pos + GO.Size / 2f - size / 2f + new Vector2(GO.Size.X / 2f + size.X, 0);
            else
                fireball.Pos = GO.Pos + GO.Size / 2f - size / 2f - new Vector2(GO.Size.X / 2f + size.X, 0);
            fireball.Size = size;
            fireball.AddComponent(new CRender("block"));
            fireball.AddComponent(new CFireballMovement(playerSpeed, (Input.GetMousePosition() - (fireball.Pos + .5f * (fireball.Size))), dir));
            fireball.AddComponent(new CAABB());
        }
    }
}
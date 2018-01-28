using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    class CMap : Component
    {
        private Vector2 topLeft;
        private int size;
        private GameObject player, mapPlayer;
        private bool initiated;

        public CMap(Vector2 topLeft, int size)
        {
            this.topLeft = topLeft;
            this.size = size;     
        }

        private void InitMap()
        {
            initiated = true;
            GameObject mapPlayer = new GameObject("mapPlayer", GO.Context, 0);
            CAnimatedSprite anim = new CAnimatedSprite();
            anim.AddAnimation("radarDot", "radarDot");
            anim.PlayAnimation("radarDot", 10);
            mapPlayer.AddComponent(anim);
            mapPlayer.Size = new Vector2(.3f);
            this.mapPlayer = mapPlayer;
            player = GO.Context.objects.FindWithTag("player");
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!initiated) InitMap();
            mapPlayer.Pos = player.Pos/ (size * 16) * GO.Size.X + GO.Pos - mapPlayer.Size/2 + GO.Size/2;
            Console.WriteLine("Pos" + mapPlayer.Pos);
        }
    }
}

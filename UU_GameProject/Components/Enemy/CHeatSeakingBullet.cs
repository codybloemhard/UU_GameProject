using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    public class CHeatSeakingBullet : Component
    {
        private string spawnerTag;
        private bool initiated;
        private Vector2 dir;
        private float speed, turningSpeed, life = 5;
        private GameObject following;
        private GameObject player;

        public CHeatSeakingBullet(GameObject following, float speed, Vector2 dir, float turningSpeed, string spawnerTag)
        {
            this.spawnerTag = spawnerTag;
            this.following = following;
            this.turningSpeed = turningSpeed;
            this.speed = speed;
            dir.Normalize();
            this.dir = dir;
        }

        private void InitBullet()
        {
            initiated = true;
            player = GO.FindWithTag("player");
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!initiated) InitBullet();

            Vector2 difference = player.Pos + player.Size / 2 - (GO.Pos + GO.Size / 2);

            float dirRads = (float)Math.Atan2(dir.Y, dir.X);
            float difRads = (float)Math.Atan2(difference.Y, difference.X);

            if (Math.Abs(dirRads - difRads) < Math.PI)
            {
                if (dirRads > difRads)
                    dirRads -= turningSpeed * time;
                else if (dirRads < difRads)
                    dirRads += turningSpeed * time;
            }
            else
            {
                if (dirRads > difRads)
                    dirRads += turningSpeed * time;
                else if (dirRads < difRads)
                    dirRads -= turningSpeed * time;
            }

            dir = new Vector2((float)Math.Cos(dirRads), (float)Math.Sin(dirRads));
            GO.Pos += dir * speed * time;
            life -= time;
            double angle = Math.Atan2(dir.Y, dir.X);
            GO.Renderer.SetRotation((float)angle * MathH.RAD_TO_DEG);

            if (GO.Pos.X < 0 || GO.Pos.X > 16 || GO.Pos.Y < 0 || GO.Pos.Y > 9 || life < 0)
                GO.Destroy();
        }

        //checks wheter a bullet collides with the player, or just a solid surface
        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            if (other.tag.Contains("solid"))
            {
                Explode();
                AudioManager.PlayEffect("hit");
            }
        }

        //if a bullet hits a solid object, it explodes
        public void Explode()
        {
            GameObject explosion = new GameObject(spawnerTag + "explosion", GO.Context);
            explosion.AddComponent(new CAABB());
            explosion.AddComponent(new CExplosionArea());
            explosion.AddComponent(new CDamageDealer(20, false));
            explosion.Size = new Vector2(.8f);
            explosion.Pos = GO.Pos + GO.Size / 2 - explosion.Size/2;
            GO.Destroy();
        }
    }
}

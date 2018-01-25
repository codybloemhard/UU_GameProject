using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Core;

namespace UU_GameProject
{
    class CSnowmanBoss : Component
    {
        private FSM fsm = new FSM();
        private bool initiated;
        private float ctime, throwDelay = 8, throwTime, switchTime, switchDelay = 5;
        private GameObject player;
        private int snowmanCount;

        private void InitSnowman()
        {
            initiated = true;
            player = GO.Context.objects.FindWithTag("player");
            fsm.Add("idle", Idle);
            fsm.Add("storm", SnowStorm);
        }
        public override void Update(float time)
        {
            if (!initiated) InitSnowman(); 
            ctime = time;


            switchTime -= time;
            if (switchTime <= 0)
            {
                ChangeFsm();
                switchTime = switchDelay;
            }

            fsm.Update();
        }

        private void ChangeFsm()
        {
            int random = MathH.random.Next(1);
            if (random == 0)
                fsm.SetCurrentState("storm");
        }

        private void Idle()
        {
            throwTime -= ctime;
            if (throwTime <= 0)
            {
                ThrowSnowball(player.Pos - (GO.Pos + GO.Size/2), GO.Pos + GO.Size/2);
                throwTime = throwDelay;
            }
        }

        private void SnowStorm()
        {
            int numberOfBalls = 6;
            int random = MathH.random.Next(numberOfBalls - 2);
            Console.WriteLine(random);
            for (int i = 0; i < numberOfBalls; i++)
            {
                ThrowSnowball(new Vector2(-1, 0), GO.Pos + i * new Vector2(0, GO.Size.Y / (numberOfBalls)));
                if (random == i)
                    i += 2;
            }
            fsm.SetCurrentState("idle");
        }

        private void ThrowSnowball(Vector2 dir, Vector2 pos)
        {
            GameObject snowball = new GameObject("snowball", GO.Context);
            snowball.AddComponent(new CRender("block"));
            snowball.AddComponent(new CFireballMovement(Vector2.Zero, dir, dir, 20, false, 8));
            snowball.AddComponent(new CAABB());
            snowball.AddComponent(new CFaction("enemy"));
            snowball.Size = new Vector2(.4f);
            snowball.Pos = pos;
        }

        private void Snowpocalypse()
        {
            if(snowmanCount < 10)
            {
                GameObject snowman = new GameObject("snowman", GO.Context);
                snowman.AddComponent(new CRender("block"));
                snowman.AddComponent(new CFaction("enemy"));
                snowman.AddComponent(new CSnowmanAI());
                snowman.AddComponent(new CDamageDealer(20, false));
                snowman.Size = new Vector2(1, 2);
                snowman.Pos = GO.Pos + new Vector2(-snowman.Size.X, GO.Size.Y - snowman.Size.Y);
            }
        }


    }
}

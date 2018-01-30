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
        private float ctime, throwDelay = 2, throwTime, switchTime, switchDelay = 5, snowmanDelay = 1, snowmanTime, avalancheDelay = .5f, avalancheTime;
        private GameObject player;
        private int snowmanCount, avalancheCount = 10, avalancheWaves = 5;

        private void InitSnowman()
        {
            initiated = true;
            player = GO.Context.objects.FindWithTag("player");
            fsm.Add("aim", AimedThrowing);
            fsm.Add("storm", SnowStorm);
            fsm.Add("snowballs", Snowballs);
            fsm.Add("avalanche", Avalanche);
        }
        public override void Update(float time)
        {
            if (!initiated) InitSnowman(); 
            ctime = time;

            



            if (switchTime <= 0)
            {
                ChangeFsm();
                switchTime = switchDelay;
            }
            fsm.Update();
        }

        private void ChangeFsm()
        {
            int random = MathH.random.Next(3);
            if (random == 0)
                fsm.SetCurrentState("storm");
            if (random == 1)
                fsm.SetCurrentState("snowballs");
            if (random == 2)
                fsm.SetCurrentState("avalanche");

        }

        private void AimedThrowing()
        {
            throwTime -= ctime;
            if (throwTime <= 0)
            {
                ThrowSnowball(player.Pos - (GO.Pos + GO.Size/2), GO.Pos + GO.Size/2);
                throwTime = throwDelay;
            }
            switchTime -= ctime;
        }

        private void SnowStorm()
        {
            int numberOfBalls = 6;
            int random = MathH.random.Next(numberOfBalls - 2);
            for (int i = 0; i < numberOfBalls; i++)
            {
                ThrowSnowball(new Vector2(-1, 0), GO.Pos + i * new Vector2(0, GO.Size.Y / (numberOfBalls)));
                if (random == i)
                    i += 2;
            }
            fsm.SetCurrentState("aim");
        }

        private void Snowballs()
        {
            if (snowmanCount < 5 && snowmanTime <= 0)
            {
                snowmanTime = snowmanDelay;
                GameObject snowball = new GameObject(GO.tag + "snowball", GO.Context);
                snowball.AddComponent(new CFaction("enemy"));
                snowball.AddComponent(new CSnowmanAI());
                snowball.AddComponent(new CAABB());
                snowball.AddComponent(new CDamageDealer(20, false));
                snowball.Size = new Vector2(1, 2);
                snowball.Pos = GO.Pos + new Vector2(-snowball.Size.X, GO.Size.Y - snowball.Size.Y);
                LvObj lvobj = new LvObj();
                lvobj.pos = new Vector2((float)MathH.random.NextDouble()*1000, (float)MathH.random.NextDouble() * 1000);//dit hoort
                ReplacerInput i = new ReplacerInput(10, false, lvobj, GO.Context);
                GameObject[] objs = Catalog.ReplacerSnowman(i);
                objs[0].Pos = snowball.Pos + new Vector2(-3, 0);
                objs[0].SetParent(snowball);
                objs[0].LocalPos = Vector2.Zero;
                objs[0].LocalSize = new Vector2(1f);
                snowmanCount += 1;
            }
            else if (snowmanCount >= 5)
            {
                snowmanCount = 0;
                fsm.SetCurrentState("aim");
            }
            else
                snowmanTime -= ctime;
        }

        private void Avalanche()
        {
            if (avalancheWaves < 1)
            {
                if (avalancheTime <= 0)
                {
                    for (int i = 0; i < avalancheCount; i++)
                    {
                        GameObject snowfall = new GameObject(GO.tag + "snowfall", GO.Context);
                        snowfall.AddComponent(new CAABB());
                        snowfall.AddComponent(new CRender("block"));
                        snowfall.AddComponent(new CSnowFall(GO.Pos));
                        snowfall.AddComponent(new CFaction("enemy"));
                        snowfall.AddComponent(new CDamageDealer(20, false));
                        snowfall.Size = new Vector2(.3f);
                        snowfall.Pos = GO.Pos + new Vector2(-10, -1) + new Vector2(1.5f * i, 0);
                    }
                    avalancheTime = avalancheDelay;
                    avalancheWaves += 1;
                }
                else avalancheTime -= ctime;
                
            }
            else
            {
                avalancheWaves = 0;
                fsm.SetCurrentState("aim");
            }
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
    }
}

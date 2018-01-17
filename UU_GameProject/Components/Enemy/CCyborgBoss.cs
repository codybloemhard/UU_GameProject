using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    class CCyborgBoss : Component
    {
        private bool initiated;
        private FSM fsm = new FSM();
        private CRaycasts cRaycasts;
        private Vector2 velocity;
        private float ctime;
        private float speed = 4, gravity = 10f, bouncePower = 8;
        private int stage;

        public CCyborgBoss(int stage, int direction)
        {
            this.stage = stage;
            speed *= direction;
        }

        private void InitRobot()
        {
            initiated = true;
            cRaycasts = GO.GetComponent<CRaycasts>();

            fsm.Add("bouncybounce", Bounce);
            fsm.SetCurrentState("bouncybounce");
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!initiated) InitRobot();
            ctime = time;

            fsm.Update();


        }

        private void Bounce()
        {
            if (cRaycasts.WallLeftHit)
                speed = Math.Abs(speed);
            else if (cRaycasts.WallRightHit)
                speed = -Math.Abs(speed);

            if (cRaycasts.CeilingHit)
                velocity.Y *= -1;

            if (cRaycasts.Grounded)
                velocity.Y = -(0.8f * bouncePower + 0.4f * (float)MathH.random.NextDouble() * bouncePower);
            else velocity.Y += gravity * ctime;

            velocity.X = speed;

            GO.Pos += cRaycasts.Move(velocity * ctime);
        }

        public void Split()
        {
            stage -= 1;
            if (stage == 0)
            {
                GO.Destroy();
                return;
            }

            for (int i = -1; i < 2; i += 2)
            {
                Console.WriteLine(stage + ", " + i);
                GameObject cyborgBoss = new GameObject("CyborgBoss", GO.Context, 2);
                cyborgBoss.AddComponent(new CRender("block"));
                cyborgBoss.AddComponent(new CCyborgBoss(stage, i));
                cyborgBoss.AddComponent(new CRaycasts());
                cyborgBoss.AddComponent(new CHealthPool(50));
                cyborgBoss.AddComponent(new CAABB());
                cyborgBoss.AddComponent(new CShoot());
                cyborgBoss.Pos = GO.Pos + 0.5f * i * GO.Pos;
                cyborgBoss.Size = new Vector2(stage, stage);
            }

            GO.active = false;
        }
    }
}


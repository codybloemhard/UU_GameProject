using System;
using System.IO;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    public class TestGame : GameState
    {
        public TestGame() : base() { }

        public override void Load(SpriteBatch batch)
        {
            //UI
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");
            Text text = new Text(this, "Position: ", new Vector2(0f, 0f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);
            text.tag = "positionText";
            Button button = new Button(this, "Menu!", "block", () => GameStateManager.RequestChange("menu", CHANGETYPE.LOAD),
                font, new Vector2(14, 0), new Vector2(2, 1));
            button.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            //Objects
            GameObject stone0 = new GameObject("stone", this, 2, true);
            stone0.Pos = new Vector2(0, 8);
            stone0.Size = new Vector2(8, 1);
            stone0.AddComponent(new CRender("block"));
            stone0.AddComponent(new CAABB());
            GameObject stone1 = new GameObject("stone", this, 2, true);
            stone1.Pos = new Vector2(9, 7);
            stone1.Size = new Vector2(2, 2);
            stone1.AddComponent(new CRender("block"));
            stone1.AddComponent(new CAABB());
            GameObject stone2 = new GameObject("stone", this, 2, true);
            stone2.Pos = new Vector2(8, 5);
            stone2.Size = new Vector2(3, 0.3f);
            stone2.AddComponent(new CRender("block"));
            stone2.AddComponent(new CAABB());
            GameObject stone3 = new GameObject("stone", this, 2, true);
            stone3.Pos = new Vector2(12, 3);
            stone3.Size = new Vector2(3, 0.3f);
            stone3.AddComponent(new CRender("block"));
            stone3.AddComponent(new CAABB());
            GameObject stone4 = new GameObject("stone", this, 2, true);
            stone4.Pos = new Vector2(2, 4);
            stone4.Size = new Vector2(3, 0.3f);
            stone4.AddComponent(new CRender("block"));
            stone4.AddComponent(new CAABB());
            GameObject killer = new GameObject("killer", this, 2);
            killer.AddComponent(new CRender("suprise"));
            killer.AddComponent(new CAABB());
            killer.Pos = new Vector2(3, 5);
            killer.Size = new Vector2(1, 1);
            (killer.Renderer as CRender).colour = Color.Red;
            GameObject player = new GameObject("player", this, 1);
            CAnimatedSprite anim = new CAnimatedSprite();
            anim.AddAnimation("fallPanic", "playerFallPanic");
            anim.AddAnimation("walking", "playerWalking");
            anim.AddAnimation("crouching", "playerCrouching");
            anim.AddAnimation("crawling", "playerCrawling");
            anim.AddAnimation("sliding", "playerSliding");
            anim.AddAnimation("airborn", "playerAirborn");
            anim.AddAnimation("melee", "playerMelee");
            anim.PlayAnimation("walking", 5);
            player.AddComponent(anim);
            player.AddComponent(new CPlayerMovement(3.0f));
            player.AddComponent(new CAABB());
            player.AddComponent(new CShoot());
            player.AddComponent(new CMeleeAttack());
            player.AddComponent(new CHealthPool(1000, player));
            player.AddComponent(new CManaPool(100, player));
            player.AddComponent(new Components.General.CMagicness());
            player.AddComponent(new Components.General.CFaction("friendly"));
            player.Pos = new Vector2(1, 1);
            player.Size = new Vector2(0.5f, 1.0f);
            GameObject robotBoss = new GameObject("RobotBoss", this, 2);
            robotBoss.AddComponent(new CRender("player"));
            robotBoss.AddComponent(new CRobotBoss(3));
            robotBoss.AddComponent(new CRaycasts());
            robotBoss.AddComponent(new CHealthPool(50, robotBoss));
            robotBoss.AddComponent(new CAABB());
            robotBoss.AddComponent(new CShoot());
            robotBoss.Pos = new Vector2(12.5f, 2f);
            robotBoss.Size = new Vector2(0.5f, 1f);
            
        }

        public override void Unload()
        {
            
        }

        public override void Update(float time)
        {
            //Camera.SetCameraTopLeft(new Vector2(0, 0));
            Text text = ui.FindWithTag("positionText") as Text;
            GameObject player = objects.FindWithTag("player");
            text.text = "Position: " + MathH.Float(player.Pos.X, 2) + " , " + MathH.Float(player.Pos.Y, 2);
            if (Input.GetKey(PressAction.PRESSED, Keys.P))
            {
                if (Debug.Mode == DEBUGMODE.PROFILING)
                    Debug.FullDebugMode();
                else Debug.ProfilingMode();
            }

            if (shakeTime > 0)
                ShakeCamera(strength, time);

            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }

        private Vector2 returnPos;
        private float shakeTime = 0;
        private int strength;

        private void ShakeCamera(int strength, float time)
        {
            shakeTime -= time;
            Console.WriteLine(strength);
            Camera.SetCameraTopLeft(Grid.ToGridSpace(returnPos + Math.Min((.1f + shakeTime), 2) * new Vector2(MathH.random.Next(-strength, strength), MathH.random.Next(-strength, strength))));
            if (shakeTime <= 0)
            {
                Camera.SetCameraTopLeft(Grid.ToGridSpace(returnPos));
            }
        }

        public void ShakeCamera(float shakeTime, int strength)
        {
            if(this.shakeTime <= 0 || strength > this.strength)
            {
                this.strength = strength;
                this.shakeTime = shakeTime;
                
                if(!(strength > this.strength))
                    returnPos = Camera.TopLeft;
            }
        }
    }
}
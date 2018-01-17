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

        private UITextureElement healthbar, manabar, fitness, healing, lightning;
        private GameObject player;
        private CMagicness magicness;
        private CHealthPool healthpool;
        private CManaPool manapool;
        private Color cGreen = new Color(0, 255, 0),
            cRed = new Color(255, 0, 0),
            cOrange = new Color(255, 255, 0);
        
        public override void Load(SpriteBatch batch)
        {
            //UI
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");
            Button button = new Button(this, "Menu!", "block", () => GameStateManager.RequestChange("menu", CHANGETYPE.LOAD),
                font, new Vector2(14, 0), new Vector2(2, 1));
            button.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            healthbar = new UITextureElement(this, "block", Vector2.Zero, Vector2.Zero);
            healthbar.colour = new Color(0, 255, 0);
            manabar = new UITextureElement(this, "block", Vector2.Zero, Vector2.Zero);
            manabar.colour = new Color(0, 0, 255);
            fitness = new UITextureElement(this, "block", new Vector2(2.6f, 8f), new Vector2(1f));
            healing = new UITextureElement(this, "block", new Vector2(3.8f, 8f), new Vector2(1f));
            lightning = new UITextureElement(this, "block", new Vector2(5f, 8f), new Vector2(1f));
            //Objects
            GameObject stone0 = new GameObject("stone", this, 2, true);
            stone0.Pos = new Vector2(0);
            stone0.Size = new Vector2(16, 1);
            stone0.AddComponent(new CRender("block"));
            stone0.AddComponent(new CAABB());
            GameObject stone1 = new GameObject("stone", this, 2, true);
            stone1.Pos = new Vector2(0);
            stone1.Size = new Vector2(1, 9);
            stone1.AddComponent(new CRender("block"));
            stone1.AddComponent(new CAABB());
            GameObject stone2 = new GameObject("stone", this, 2, true);
            stone2.Pos = new Vector2(15, 0);
            stone2.Size = new Vector2(1, 9);
            stone2.AddComponent(new CRender("block"));
            stone2.AddComponent(new CAABB());
            GameObject stone3 = new GameObject("stone", this, 2, true);
            stone3.Pos = new Vector2(0, 8);
            stone3.Size = new Vector2(16, 1f);
            stone3.AddComponent(new CRender("block"));
            stone3.AddComponent(new CAABB());

            player = new GameObject("player", this, 1);
            CAnimatedSprite anim = new CAnimatedSprite();
            anim.AddAnimation("fallPanic", "playerFallPanic");
            anim.AddAnimation("standingRight", "playerStandingRight");
            anim.AddAnimation("standingLeft", "playerStandingLeft");
            anim.AddAnimation("runningRight", "playerRunningRight");
            anim.AddAnimation("runningLeft", "playerRunningLeft");
            anim.AddAnimation("crouching", "playerCrouching");
            anim.AddAnimation("crawlingRight", "playerCrawlingRight");
            anim.AddAnimation("crawlingLeft", "playerCrawlingLeft");
            anim.AddAnimation("sliding", "playerSliding");
            anim.AddAnimation("airborneRight", "playerAirborneRight");
            anim.AddAnimation("airborneLeft", "playerAirborneLeft");
            anim.AddAnimation("wallSlidingRight", "playerWallSlidingRight");
            anim.AddAnimation("wallSlidingLeft", "playerWallSlidingLeft");
            anim.AddAnimation("melee", "playerMelee");
            anim.PlayAnimation("runningRight", 12);
            player.AddComponent(anim);
            magicness = new CMagicness();
            healthpool = new CHealthPool(100);
            manapool = new CManaPool(100, player);
            player.AddComponent(new CPlayerMovement(3.0f));
            player.AddComponent(new CAABB());
            player.AddComponent(new CShoot());
            player.AddComponent(new CMeleeAttack());
            player.AddComponent(new CDamageDealer(50, false));
            player.AddComponent(healthpool);
            player.AddComponent(manapool);
            player.AddComponent(magicness);
            player.AddComponent(new CFaction("friendly"));
            player.AddComponent(new CCamera());
            player.Pos = new Vector2(1, 1);
            player.Size = new Vector2(0.5f, 1.0f);
            
            GameObject cyborgBoss = new GameObject("robotboss", this, 2);
            cyborgBoss.AddComponent(new CRender("player"));
            cyborgBoss.AddComponent(new CRobotBoss(3));
            cyborgBoss.AddComponent(new CRaycasts());
            cyborgBoss.AddComponent(new CHealthPool(50));
            cyborgBoss.AddComponent(new CDamageDealer(50, false));
            cyborgBoss.AddComponent(new CAABB());
            cyborgBoss.AddComponent(new CShoot());
            cyborgBoss.Pos = new Vector2(12.5f, 2f);
            cyborgBoss.Size = new Vector2(0.5f, 1f);
            
            GameObject respawn0 = new GameObject("checkpoint", this, 2);
            respawn0.Size = new Vector2(0.5f, 1);
            respawn0.Pos = new Vector2(0, 7);
            respawn0.AddComponent(new CAABB());
            respawn0.AddComponent(new CRender("suprise"));
            GameObject respawn1 = new GameObject("checkpoint", this, 2);
            respawn1.Size = new Vector2(0.5f, 1);
            respawn1.Pos = new Vector2(14, 2);
            respawn1.AddComponent(new CAABB());
            respawn1.AddComponent(new CRender("suprise"));

            //testing
            /*GameObject floor = new GameObject("stone", this, 2, true);
            floor.Pos = new Vector2(-100, 8);
            floor.Size = new Vector2(100, 1);
            floor.AddComponent(new CRender("block"));
            floor.AddComponent(new CAABB());*/
            GameObject testDoor = new GameObject("bossdoor", this, 0, true);
            testDoor.Pos = new Vector2(-4f, 8f - 5f);
            testDoor.Size = new Vector2(1f, 5f);
            testDoor.AddComponent(new CGrowingDoor());
            //Debug.FullDebugMode();
            AudioManager.PlayTrack("moonlightsonata");
            AudioManager.SetMasterVolume(0f);
        }
        
        public override void Unload() { }

        public override void Update(float time)
        {
            float health = healthpool.HealhPercent;
            float mana = manapool.ManaPercentage;
            healthbar.Size = new Vector2(1f, 3f * health);
            healthbar.Pos = new Vector2(0.2f, 9f - healthbar.Size.Y);
            manabar.Size = new Vector2(1f, 3f * mana);
            manabar.Pos = new Vector2(1.4f, 9f - manabar.Size.Y);
            if (magicness.UnlockedFitness)
                fitness.Size = new Vector2(1f);
            else fitness.Size = new Vector2(0f);
            if (magicness.UnlockedHealing)
                healing.Size = new Vector2(1f);
            else healing.Size = new Vector2(0f);
            if (magicness.UnlockedLightning)
                lightning.Size = new Vector2(1f);
            else lightning.Size = new Vector2(0f);
            if (magicness.CanHeal)
                healing.colour = cGreen;
            else healing.colour = cRed;
            if (magicness.CanLightning)
                lightning.colour = cGreen;
            else lightning.colour = cRed;
            if (magicness.CanDoublejump)
                fitness.colour = cGreen;
            else if (magicness.CanDash)
                fitness.colour = cOrange;
            else fitness.colour = cRed;

            if (Input.GetKey(PressAction.PRESSED, Keys.P))
            {
                if (Debug.Mode == DEBUGMODE.PROFILING)
                    Debug.FullDebugMode();
                else Debug.ProfilingMode();
            }

            if (shakeTime > 0)
                ShakeCamera(strength, time);

            if(Input.GetKey(PressAction.PRESSED, Keys.X))
                objects.FindWithTag("bossdoor").GetComponent<CGrowingDoor>().Close();
            if (Input.GetKey(PressAction.PRESSED, Keys.C))
                objects.FindWithTag("bossdoor").GetComponent<CGrowingDoor>().Open();
            if (Input.GetKey(PressAction.DOWN, Keys.O))
                Debug.showAtlas = true;
            else Debug.showAtlas = false;
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
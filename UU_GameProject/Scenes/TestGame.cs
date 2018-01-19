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

        private ChunkManager chunks;
        private UITextureElement healthbar, manabar, fitness, healing, lightning;
        private GameObject player;
        private CMagicness magicness;
        private CHealthPool healthpool;
        private CManaPool manapool;
        private CMopWeapon mopweapon;
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
            mopweapon = new CMopWeapon();
            player.AddComponent(new CPlayerMovement(3.0f));
            player.AddComponent(new CAABB());
            player.AddComponent(new CShoot());
            player.AddComponent(new CMeleeAttack());
            player.AddComponent(new CDamageDealer(50, false));
            player.AddComponent(healthpool);
            player.AddComponent(manapool);
            player.AddComponent(magicness);
            player.AddComponent(mopweapon);
            player.AddComponent(new CFaction("friendly"));
            player.AddComponent(new CCamera());
            player.Pos = new Vector2(1, 1);
            player.Size = new Vector2(0.5f, 1.0f);

            Vector2 chunkSize = new Vector2(16, 16);
            ChunkFactory builder = new ChunkFactory(this, chunkSize);
            AddSources(builder);

            string baseurl = "../../../../Content/Levels/";
            chunks = new ChunkManager();
            chunks.Discover(baseurl, builder, player);
            
            AudioManager.PlayTrack("moonlightsonata");
            AudioManager.SetMasterVolume(0f);
            Debug.FullDebugMode();
        }
        
        private void AddSources(ChunkFactory builder)
        {
            builder.AddSource("!player", 5, false, Dec_Player);
            builder.AddSource("solid", 10, true,
                delegate (ReplacerInput i) {
                    return Catalog.ReplacerBlock(i, BASETILES.STONE, LAYERTILES.CRACKS, LAYERTILES.ICE, TOPTILES.SNOW);
                });
            builder.AddSource("spawn", 10, false, Dec_Spawner);
            builder.AddSource("door", 10, true, Dec_Door);
            builder.AddSource("!renemy", 5, false, Rep_RangedEnemy);
            builder.AddSource("!nenemy", 5, false, Rep_NormalEnemy);
            builder.AddSource("!aenemy", 5, false, Rep_ArmourEnemy);
            builder.AddSource("!rboss", 5, false, Rep_RobotBoss);
            builder.AddSource("!tree", 50, true, Catalog.ReplacerTree0);
        }

        private void Dec_Player(GameObject o)
        {
            player.Pos = o.Pos - player.Size * new Vector2(0.5f, 1f);
        }

        private void Dec_Spawner(GameObject o)
        {
            o.AddComponent(new CAABB());
            o.AddComponent(new CRender("suprise"));
            o.tag = "checkpoint";
        }

        private void Dec_Door(GameObject o)
        {
            o.tag = "bossdoor";
            o.AddComponent(new CGrowingDoor());
        }

        private GameObject[] Rep_RangedEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Renemy", this, 2);
            enemy.AddComponent(new CRender("player"));
            enemy.AddComponent(new CRangedEnemyAI(ENEMY.MAGIC));
            enemy.AddComponent(new CHealthPool(25));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CShoot());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.5f, 1.0f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_NormalEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Nenemy", this, 2);
            enemy.AddComponent(new CRender("player"));
            enemy.AddComponent(new CNormalEnemyAI(ENEMY.MAGIC));
            enemy.AddComponent(new CHealthPool(50));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CMeleeAttack());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.5f, 1.0f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_ArmourEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Aenemy", this, 2);
            enemy.AddComponent(new CRender("player"));
            enemy.AddComponent(new CArmouredEnemyAI(ENEMY.MAGIC));
            enemy.AddComponent(new CHealthPool(1));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CMeleeAttack());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.5f, 1.0f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_RobotBoss(ReplacerInput i)
        {
            GameObject robotBoss = new GameObject("RobotBoss", this, 2);
            CAnimatedSprite animBoss = new CAnimatedSprite();
            robotBoss.AddComponent(new CRobotBoss(3));
            robotBoss.AddComponent(new CRaycasts());
            robotBoss.AddComponent(new CHealthPool(1500));
            robotBoss.AddComponent(new CAABB());
            robotBoss.AddComponent(new CShoot());
            robotBoss.AddComponent(new CFaction("enemy"));
            robotBoss.AddComponent(new CMeleeAttack());
            animBoss.AddAnimation("walking", "robotBossWalking");
            animBoss.AddAnimation("flying", "robotBossFlying");
            animBoss.AddAnimation("falling", "robotBossFalling");
            animBoss.PlayAnimation("walking", 2);
            robotBoss.AddComponent(animBoss);
            robotBoss.Size = new Vector2(3f, 3f);
            robotBoss.Pos = i.obj.pos - robotBoss.Size / 2f;
            return new GameObject[] { robotBoss };
        }

        public override void Unload() { }

        public override void Update(float time)
        {
            base.Update(time);
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

            if (Input.GetKey(PressAction.PRESSED, Keys.V))
                foreach(GameObject obj in objects.FindAllWithTag("cyborgboss"))
                    obj.GetComponent<CCyborgBoss>().Split();

            if(Input.GetKey(PressAction.PRESSED, Keys.X))
            {
                GameObject o = objects.FindWithTag("bossdoor");
                if(o != null)
                {
                    o.GetComponent<CGrowingDoor>().Close();
                }
            }

            if (Input.GetKey(PressAction.DOWN, Keys.O))
                Debug.showAtlas = true;
            else Debug.showAtlas = false;

            chunks.Update();
            TaskEngine.UpdateAll();
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}

//GameObject cyborgBoss = new GameObject("robotboss", this, 2);
//cyborgBoss.AddComponent(new CRender("player"));
//cyborgBoss.AddComponent(new CRobotBoss(3));
//cyborgBoss.AddComponent(new CRaycasts());
//cyborgBoss.AddComponent(new CHealthPool(50));
//cyborgBoss.AddComponent(new CDamageDealer(50, false));
//cyborgBoss.AddComponent(new CAABB());
//cyborgBoss.AddComponent(new CShoot());
//cyborgBoss.Pos = new Vector2(12.5f, 2f);
//cyborgBoss.Size = new Vector2(0.5f, 1f);

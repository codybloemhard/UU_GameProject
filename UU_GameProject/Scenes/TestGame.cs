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
        private GameObject player, sky;
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
            healthbar = new UITextureElement(this, "sky", Vector2.Zero, Vector2.Zero);
            healthbar.colour = new Color(0, 255, 0);
            manabar = new UITextureElement(this, "sky", Vector2.Zero, Vector2.Zero);
            manabar.colour = new Color(255, 0, 255);
            fitness = new UITextureElement(this, "sky", new Vector2(2.6f, 8f), new Vector2(1f));
            healing = new UITextureElement(this, "sky", new Vector2(3.8f, 8f), new Vector2(1f));
            lightning = new UITextureElement(this, "sky", new Vector2(5f, 8f), new Vector2(1f));
            //objects
            sky = new GameObject(this, 100);
            sky.AddComponent(new CRender("background"));
            sky.Size = new Vector2(16, 16);
            player = new GameObject("player", this, 10);
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
            
            //AudioManager.PlayTrack("moonlightsonata");
            AudioManager.SetMasterVolume(1f);
            //Debug.FullDebugMode();
        }
        
        private void AddSources(ChunkFactory builder)
        {
            builder.AddSource("!player", 15, false, Dec_Player);
            for(int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < 4; k++)
                        for (int l = 0; l < 3; l++)
                        {
                            BASETILES baset = (BASETILES)i;
                            LAYERTILES layert0 = (LAYERTILES)j;
                            LAYERTILES layert1 = (LAYERTILES)k;
                            TOPTILES topt = (TOPTILES)l;
                            builder.AddSource("solid" + i + j + k + l, 10, true, delegate (ReplacerInput inp) {
                                return Catalog.ReplacerBlock(inp, baset, layert0, layert1, topt);
                            });
                        }
            builder.AddSource("spawn", 15, false, Dec_Spawner);
            builder.AddSource("door", 15, true, Dec_Door);
            builder.AddSource("bosstrigger", 15, false, Dec_Bosstrigger);
            builder.AddSource("!renemy", 15, false, Rep_RangedEnemy);
            builder.AddSource("!nenemy", 15, false, Rep_NormalEnemy);
            builder.AddSource("!aenemy", 15, false, Rep_ArmourEnemy);
            builder.AddSource("!rboss", 15, false, Rep_RobotBoss);
            builder.AddSource("!mboss", 15, false, Rep_MageBoss);
            builder.AddSource("!cboss", 15, false, Rep_CyborgBoss);

            builder.AddSource("!tree0", 50, true, Catalog.ReplacerTree0);
            builder.AddSource("!tree1", 50, true, Catalog.ReplacerTree1);
            builder.AddSource("!tree2", 50, true, Catalog.ReplacerTree2);
            builder.AddSource("!tree3", 50, true, Catalog.ReplacerTree3);
            builder.AddSource("!tree4", 50, true, Catalog.ReplacerTree4);
            builder.AddSource("!tree5", 50, true, Catalog.ReplacerTree5);
            builder.AddSource("!tree6", 50, true, Catalog.ReplacerTree6);
            builder.AddSource("!tree7", 50, true, Catalog.ReplacerTree7);
            builder.AddSource("!tree8", 50, true, Catalog.ReplacerTree8);
            builder.AddSource("!tree9", 50, true, Catalog.ReplacerTree9);
            builder.AddSource("!flower", 5, true, Catalog.ReplacerFlower);
            builder.AddSource("!grass", 5, true, Catalog.ReplacerGrassPlant);
            builder.AddSource("!grassdot", 5, true, Catalog.ReplacerGrassDot);
            builder.AddSource("!grasshigh", 5, true, Catalog.ReplacerGrassHigh);
            builder.AddSource("!snowman", 15, true, Catalog.ReplacerSnowman);
            builder.AddSource("!boulder", 20, true, Catalog.ReplacerBoulder);
            builder.AddSource("!stone", 20, true, Catalog.ReplacerStone);
            builder.AddSource("!snowystone", 20, true, Catalog.ReplacerSnowyStone);
            builder.AddSource("!stonefrosty", 20, true, Catalog.ReplacerFrostyStone);
            builder.AddSource("!stoneshard", 20, true, Catalog.ReplacerStoneShard);
            builder.AddSource("!cloud", 60, true, Catalog.ReplacerCloud);
            builder.AddSource("!bush", 20, true, Catalog.ReplacerBush);
        }

        private void Dec_Player(GameObject o)
        {
            if (player.GetComponent<CPlayerMovement>().spawned == false)
            {
                player.Pos = o.Pos - player.Size * new Vector2(0.5f, 1f);
                player.GetComponent<CPlayerMovement>().spawned = true;
            }
        }

        private void Dec_Spawner(GameObject o)
        {
            o.AddComponent(new CAABB());
            o.AddComponent(new CRender("block"));
            o.tag = "checkpoint";
        }

        private void Dec_Bosstrigger(GameObject o)
        {
            o.AddComponent(new CBossTrigger());
            o.AddComponent(new CAABB());
        }

        private void Dec_Door(GameObject o)
        {
            o.tag = "bossdoorsolid";
            o.AddComponent(new CGrowingDoor());
        }

        private GameObject[] Rep_RangedEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Renemy", this, 2);
            CAnimatedSprite animRangedEnemy = new CAnimatedSprite();
            animRangedEnemy.AddAnimation("redMageStandingRight", "redMageStandingRight");
            animRangedEnemy.AddAnimation("redMageStandingLeft", "redMageStandingLeft");
            animRangedEnemy.AddAnimation("redMageCastingRight", "redMageCastingRight");
            animRangedEnemy.AddAnimation("redMageCastingLeft", "redMageCastingLeft");
            animRangedEnemy.AddAnimation("greenMageStandingRight", "greenMageStandingRight");
            animRangedEnemy.AddAnimation("greenMageStandingLeft", "greenMageStandingLeft");
            animRangedEnemy.AddAnimation("greenMageCastingRight", "greenMageCastingRight");
            animRangedEnemy.AddAnimation("greenMageCastingLeft", "greenMageCastingLeft");
            animRangedEnemy.AddAnimation("purpleMageStandingRight", "purpleMageStandingRight");
            animRangedEnemy.AddAnimation("purpleMageStandingLeft", "purpleMageStandingLeft");
            animRangedEnemy.AddAnimation("purpleMageCastingRight", "purpleMageCastingRight");
            animRangedEnemy.AddAnimation("purpleMageCastingLeft", "purpleMageCastingLeft");
            animRangedEnemy.PlayAnimation("redMageStandingRight", 6);
            enemy.AddComponent(animRangedEnemy);
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
            CAnimatedSprite animNormalEnemy = new CAnimatedSprite();
            animNormalEnemy.AddAnimation("redSlimeMovingRight", "redSlimeMovingRight");
            animNormalEnemy.AddAnimation("redSlimeMovingLeft", "redSlimeMovingLeft");
            animNormalEnemy.AddAnimation("robotSlimeMovingRight", "robotSlimeMovingRight");
            animNormalEnemy.AddAnimation("robotSlimeMovingLeft", "robotSlimeMovingLeft");
            animNormalEnemy.PlayAnimation("redSlimeMovingRight", 4);
            enemy.AddComponent(animNormalEnemy);
            enemy.AddComponent(new CNormalEnemyAI(ENEMY.MAGIC));
            enemy.AddComponent(new CHealthPool(50));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CMeleeAttack());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.5f, 0.5f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_ArmourEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Aenemy", this, i.layer);
            CAnimatedSprite animArmourEnemy = new CAnimatedSprite();
            animArmourEnemy.AddAnimation("robotSlimeMovingRight", "robotSlimeMovingRight");
            animArmourEnemy.AddAnimation("robotSlimeMovingLeft", "robotSlimeMovingLeft");
            animArmourEnemy.PlayAnimation("robotSlimeMovingRight", 4);
            enemy.AddComponent(animArmourEnemy);
            enemy.AddComponent(new CArmouredEnemyAI(ENEMY.MAGIC));
            enemy.AddComponent(new CHealthPool(100));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CMeleeAttack());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.5f, 1.0f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_RobotBoss(ReplacerInput i)
        {
            GameObject robotBoss = new GameObject("boss", this, i.layer);
            CAnimatedSprite animBoss = new CAnimatedSprite();
            robotBoss.AddComponent(new CRobotBoss(3));
            robotBoss.AddComponent(new CRaycasts());
            robotBoss.AddComponent(new CHealthPool(50));
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
            robotBoss.active = false;
            return new GameObject[] { robotBoss };
        }

        private GameObject[] Rep_MageBoss(ReplacerInput i)
        {
            GameObject mageBoss = new GameObject("boss", this, 2);
            CAnimatedSprite animBoss = new CAnimatedSprite();
            animBoss.AddAnimation("hovering", "mageBossHovering");
            animBoss.AddAnimation("fireball", "mageBossFireball");
            animBoss.AddAnimation("lightning", "mageBossLightning");
            animBoss.PlayAnimation("hovering", 6);
            mageBoss.AddComponent(animBoss);
            mageBoss.AddComponent(new CHealthPool(1500));
            mageBoss.AddComponent(new CAABB());
            mageBoss.AddComponent(new CFaction("enemy"));
            mageBoss.AddComponent(new CMageBoss());
            mageBoss.Size = new Vector2(2);
            mageBoss.Pos = i.obj.pos - mageBoss.Size / 2;
            mageBoss.active = false;
            return new GameObject[] { mageBoss };
        }

        private GameObject[] Rep_CyborgBoss(ReplacerInput i)
        {
            GameObject cyborgBoss = new GameObject("boss", this, 2);
            cyborgBoss.AddComponent(new CRender("block"));
            cyborgBoss.AddComponent(new CHealthPool(500));
            cyborgBoss.AddComponent(new CAABB());
            cyborgBoss.AddComponent(new CFaction("enemy"));
            cyborgBoss.AddComponent(new CCyborgBoss(4, 1));
            cyborgBoss.AddComponent(new CRaycasts());
            cyborgBoss.Size = new Vector2(4);
            cyborgBoss.Pos = i.obj.pos - cyborgBoss.Size / 2;
            cyborgBoss.active = false;    
            return new GameObject[] { cyborgBoss };
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
            sky.Pos = Camera.TopLeft;

            if (Input.GetKey(PressAction.PRESSED, Keys.P))
            {
                if (Debug.Mode == DEBUGMODE.PROFILING)
                    Debug.FullDebugMode();
                else Debug.ProfilingMode();
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

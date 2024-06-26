﻿using System;
using System.IO;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    public class MainGame : GameState
    {
        public MainGame() : base() { }

        private ChunkManager chunks;
        private UITextureElement healthbar, healthBarBackground, manabar, manaBarBackground, fitness, healing, lightning, healthBarOverlay, manaBarOverlay, healOverlay, fitnessOverlay, lightningBoltOverlay;
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
            Button button = new Button(this, "Exit", "Menu_Button_3", () => GameStateManager.RequestChange("menu", CHANGETYPE.LOAD),
                font, new Vector2(13, 0), new Vector2(3f, .6f));
            button.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            healthBarBackground = new UITextureElement(this, "Bar_Background", new Vector2(0, 6.53f), new Vector2(0.65f, 2.47f));
            healthbar = new UITextureElement(this, "Healthpool_Bar", new Vector2(0, 6.53f), new Vector2(0.65f, 2.47f));
            healthBarOverlay = new UITextureElement(this, "Health_Bar_Overlay", new Vector2(0, 6.4f), new Vector2(0.65f, 2.6f));
            manaBarBackground = new UITextureElement(this, "Bar_Background", new Vector2(0.65f, 6.53f), new Vector2(0.65f, 2.47f));
            manabar = new UITextureElement(this, "Manapool_Bar", new Vector2(0.65f, 6.53f), new Vector2(0.65f, 2.47f));
            manaBarOverlay = new UITextureElement(this, "Mana_Bar_Overlay", new Vector2(0.65f, 6.4f), new Vector2(0.65f, 2.6f));
            fitness = new UITextureElement(this, "DJump_Bar_Filler", new Vector2(1.3f, 8.25f), new Vector2(0.8f, 0.7f));
            fitnessOverlay = new UITextureElement(this, "DJump_Bar_Overlay", new Vector2(1.3f, 8.1f), new Vector2(0.8f, 0.9f));
            healing = new UITextureElement(this, "Heal_Bar_Filler", new Vector2(2.1f, 8.25f), new Vector2(0.8f, 0.7f));
            healOverlay = new UITextureElement(this, "Heal_Bar_Overlay", new Vector2(2.1f, 8.1f), new Vector2(0.8f, 0.9f));
            lightning = new UITextureElement(this, "Bolt_Bar_Filler", new Vector2(2.9f, 8.25f), new Vector2(0.8f, 0.7f));
            lightningBoltOverlay = new UITextureElement(this, "Bolt_Bar_Overlay", new Vector2(2.9f, 8.1f), new Vector2(0.8f, 0.9f));
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
            healthpool = new CHealthPool(300);
            manapool = new CManaPool(100, player);
            mopweapon = new CMopWeapon();
            player.AddComponent(new CRaycasts());
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
            
            AudioManager.SetMasterVolume(1f);
            Debug.FullDebugMode();
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
            builder.AddSource("spawn", 1, false, Dec_Spawner);
            builder.AddSource("radar", 21, false, Dec_Radar);
            builder.AddSource("door", 15, true, Dec_Door);
            builder.AddSource("bosssignr", 5, false, Dec_BossSignRight);
            builder.AddSource("bosssignl", 5, false, Dec_BossSignLeft);
            builder.AddSource("bosstrigger", 15, false, Dec_Bosstrigger);
            builder.AddSource("spike", 5, false, Dec_Spike);
            builder.AddSource("!rmrenemy", 15, false, Rep_RedMagicRangedEnemy);
            builder.AddSource("!gmrenemy", 15, false, Rep_GreenMagicRangedEnemy);
            builder.AddSource("!pmrenemy", 15, false, Rep_PurpleMagicRangedEnemy);
            builder.AddSource("!crenemy", 15, false, Rep_CyborgRangedEnemy);
            builder.AddSource("!mnenemy", 15, false, Rep_MagicNormalEnemy);
            builder.AddSource("!rnenemy", 15, false, Rep_RobotNormalEnemy);
            builder.AddSource("!cnenemy", 15, false, Rep_CyborgNormalEnemy);
            builder.AddSource("!caenemy", 15, false, Rep_CyborgArmourEnemy);
            builder.AddSource("!raenemy", 15, false, Rep_RobotArmourEnemy);
            builder.AddSource("!rboss", 25, false, Rep_RobotBoss);
            builder.AddSource("!mboss", 25, false, Rep_MageBoss);
            builder.AddSource("!cboss", 25, false, Rep_CyborgBoss);
            builder.AddSource("!sboss", 25, false, Rep_SnowmanBoss);
            builder.AddSource("!tutosign1", 17, true, Dec_TutorialSign);
            builder.AddSource("!tutosign2", 17, true, Dec_TutorialSign_2);
            builder.AddSource("!tutosign3", 17, true, Dec_TutorialSign_3);
            builder.AddSource("!bosssignr", 17, true, Dec_BossSignRight);
            builder.AddSource("!bosssignl", 17, true, Dec_BossSignLeft);
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
            builder.AddSource("!grassdot", 20, true, Catalog.ReplacerGrassDot);
            builder.AddSource("!grasshigh", 20, true, Catalog.ReplacerGrassHigh);
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
            CAnimatedSprite anim = new CAnimatedSprite();
            anim.AddAnimation("respawnpointOff", "respawnpointOff");
            anim.AddAnimation("respawnpointOn", "respawnpointOn");
            anim.PlayAnimation("respawnpointOff", 1);
            o.AddComponent(anim);
            o.tag = "checkpoint";
        }

        private void Dec_Radar(GameObject o)
        {
            CAnimatedSprite anim = new CAnimatedSprite();
            anim.AddAnimation("radarBase", "radarBase");
            anim.PlayAnimation("radarBase", 1);
            o.AddComponent(anim);
            o.AddComponent(new CMap(new Vector2(-32), 5));
        }

        private void Dec_TutorialSign(GameObject o)
        {
            o.AddComponent(new CRender("tutorialSign1"));
            o.Size = new Vector2(2, 2);
        }

        private void Dec_TutorialSign_2(GameObject o)
        {
            o.AddComponent(new CRender("tutorialSign2"));
            o.Size = new Vector2(2, 2);
        }

        private void Dec_TutorialSign_3(GameObject o)
        {
            o.AddComponent(new CRender("tutorialSign3"));
            o.Size = new Vector2(2, 2);
        }

        private void Dec_BossSignRight(GameObject o)
        {
            o.AddComponent(new CRender("bossSignRight"));
        }

        private void Dec_BossSignLeft(GameObject o)
        {
            o.AddComponent(new CRender("bossSignLeft"));
        }

        private void Dec_Bosstrigger(GameObject o)
        {
            o.AddComponent(new CBossTrigger());
            o.AddComponent(new CAABB());
        }

        private void Dec_Spike(GameObject o)
        {
            o.AddComponent(new CAABB());
            o.AddComponent(new CRender("spike"));
            o.AddComponent(new CSpike());
        }

        private void Dec_Door(GameObject o)
        {
            o.tag = "bossdoorsolid";
            o.AddComponent(new CGrowingDoor());
        }

        private GameObject[] Rep_RedMagicRangedEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Renemy", this, 2);
            CAnimatedSprite animRangedEnemy = new CAnimatedSprite();
            animRangedEnemy.AddAnimation("redMageStandingRight", "redMageStandingRight");
            animRangedEnemy.AddAnimation("redMageStandingLeft", "redMageStandingLeft");
            animRangedEnemy.AddAnimation("redMageCastingRight", "redMageCastingRight");
            animRangedEnemy.AddAnimation("redMageCastingLeft", "redMageCastingLeft");
            animRangedEnemy.PlayAnimation("redMageStandingRight", 6);
            enemy.AddComponent(animRangedEnemy);
            enemy.AddComponent(new CRangedEnemyAI(ENEMY.MAGIC, "redMage"));
            enemy.AddComponent(new CHealthPool(25));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CShoot());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.5f, 1.0f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_GreenMagicRangedEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Renemy", this, 2);
            CAnimatedSprite animRangedEnemy = new CAnimatedSprite();
            animRangedEnemy.AddAnimation("greenMageStandingRight", "greenMageStandingRight");
            animRangedEnemy.AddAnimation("greenMageStandingLeft", "greenMageStandingLeft");
            animRangedEnemy.AddAnimation("greenMageCastingRight", "greenMageCastingRight");
            animRangedEnemy.AddAnimation("greenMageCastingLeft", "greenMageCastingLeft");
            animRangedEnemy.PlayAnimation("greenMageStandingRight", 6);
            enemy.AddComponent(animRangedEnemy);
            enemy.AddComponent(new CRangedEnemyAI(ENEMY.MAGIC, "greenMage"));
            enemy.AddComponent(new CHealthPool(25));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CShoot());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.5f, 1.0f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_PurpleMagicRangedEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Renemy", this, 2);
            CAnimatedSprite animRangedEnemy = new CAnimatedSprite();
            animRangedEnemy.AddAnimation("purpleMageStandingRight", "purpleMageStandingRight");
            animRangedEnemy.AddAnimation("purpleMageStandingLeft", "purpleMageStandingLeft");
            animRangedEnemy.AddAnimation("purpleMageCastingRight", "purpleMageCastingRight");
            animRangedEnemy.AddAnimation("purpleMageCastingLeft", "purpleMageCastingLeft");
            animRangedEnemy.PlayAnimation("purpleMageStandingRight", 6);
            enemy.AddComponent(animRangedEnemy);
            enemy.AddComponent(new CRangedEnemyAI(ENEMY.MAGIC, "purpleMage"));
            enemy.AddComponent(new CHealthPool(25));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CShoot());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.5f, 1.0f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_CyborgRangedEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Renemy", this, 2);
            CAnimatedSprite animRangedEnemy = new CAnimatedSprite();
            animRangedEnemy.AddAnimation("rangedCyborgRight", "rangedCyborgRight");
            animRangedEnemy.AddAnimation("rangedCyborgLeft", "rangedCyborgLeft");
            animRangedEnemy.PlayAnimation("rangedCyborgRight", 6);
            enemy.AddComponent(animRangedEnemy);
            enemy.AddComponent(new CRangedEnemyAI(ENEMY.MAGIC, "cyborg"));
            enemy.AddComponent(new CHealthPool(25));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CShoot());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(1.0f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_MagicNormalEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Nenemy", this, 2);
            CAnimatedSprite animNormalEnemy = new CAnimatedSprite();
            animNormalEnemy.AddAnimation("redSlimeMovingRight", "redSlimeMovingRight");
            animNormalEnemy.AddAnimation("redSlimeMovingLeft", "redSlimeMovingLeft");
            animNormalEnemy.PlayAnimation("redSlimeMovingRight", 4);
            enemy.AddComponent(animNormalEnemy);
            enemy.AddComponent(new CNormalEnemyAI(ENEMY.MAGIC, "magic"));
            enemy.AddComponent(new CHealthPool(50));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CMeleeAttack());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.5f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_RobotNormalEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Nenemy", this, 2);
            CAnimatedSprite animNormalEnemy = new CAnimatedSprite();
            animNormalEnemy.AddAnimation("robotSlimeMovingRight", "robotSlimeMovingRight");
            animNormalEnemy.AddAnimation("robotSlimeMovingLeft", "robotSlimeMovingLeft");
            animNormalEnemy.PlayAnimation("robotSlimeMovingRight", 4);
            enemy.AddComponent(animNormalEnemy);
            enemy.AddComponent(new CNormalEnemyAI(ENEMY.MAGIC, "robot"));
            enemy.AddComponent(new CHealthPool(50));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CMeleeAttack());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.5f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_CyborgNormalEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Nenemy", this, 2);
            CAnimatedSprite animNormalEnemy = new CAnimatedSprite();
            animNormalEnemy.AddAnimation("normalCyborgRight", "normalCyborgRight");
            animNormalEnemy.AddAnimation("normalCyborgLeft", "normalCyborgLeft");
            animNormalEnemy.PlayAnimation("normalCyborgRight", 4);
            enemy.AddComponent(animNormalEnemy);
            enemy.AddComponent(new CNormalEnemyAI(ENEMY.MAGIC, "cyborg"));
            enemy.AddComponent(new CHealthPool(50));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CMeleeAttack());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.5f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_CyborgArmourEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Aenemy", this, i.layer);
            CAnimatedSprite animArmourEnemy = new CAnimatedSprite();
            animArmourEnemy.AddAnimation("armoredCyborgRight", "armoredCyborgRight");
            animArmourEnemy.AddAnimation("armoredCyborgLeft", "armoredCyborgLeft");
            animArmourEnemy.PlayAnimation("armoredCyborgRight", 8);
            enemy.AddComponent(animArmourEnemy);
            enemy.AddComponent(new CArmouredEnemyAI(ENEMY.MAGIC, "cyborg"));
            enemy.AddComponent(new CHealthPool(100));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CMeleeAttack());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.8f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }

        private GameObject[] Rep_RobotArmourEnemy(ReplacerInput i)
        {
            GameObject enemy = new GameObject("Aenemy", this, i.layer);
            CAnimatedSprite animArmourEnemy = new CAnimatedSprite();
            animArmourEnemy.AddAnimation("armoredRobotRight", "armoredRobotRight");
            animArmourEnemy.AddAnimation("armoredRobotLeft", "armoredRobotLeft");
            animArmourEnemy.PlayAnimation("armoredRobotRight", 8);
            enemy.AddComponent(animArmourEnemy);
            enemy.AddComponent(new CArmouredEnemyAI(ENEMY.MAGIC, "robot"));
            enemy.AddComponent(new CHealthPool(100));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CMeleeAttack());
            enemy.AddComponent(new CFaction("enemy"));
            enemy.Size = new Vector2(0.7f);
            enemy.Pos = i.obj.pos - enemy.Size * new Vector2(0.5f, 1f);
            return new GameObject[] { enemy };
        }
        
        private GameObject[] Rep_RobotBoss(ReplacerInput i)
        {
            GameObject robotBoss = new GameObject("boss", this, i.layer);
            CAnimatedSprite animBoss = new CAnimatedSprite();
            robotBoss.AddComponent(new CRobotBoss(3));
            robotBoss.AddComponent(new CRaycasts());
            robotBoss.AddComponent(new CHealthPool(800));
            robotBoss.AddComponent(new CDamageDealer(50, false));
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
            mageBoss.AddComponent(new CHealthPool(500));
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
            CAnimatedSprite animBoss = new CAnimatedSprite();
            animBoss.AddAnimation("cyborgBossBouncing4", "cyborgBossBouncing4");
            animBoss.PlayAnimation("cyborgBossBouncing4", 8);
            cyborgBoss.AddComponent(animBoss);
            cyborgBoss.AddComponent(new CAABB());
            cyborgBoss.AddComponent(new CFaction("enemy"));
            cyborgBoss.AddComponent(new CCyborgBoss(5, 1));
            cyborgBoss.AddComponent(new CDamageDealer(20, false));
            cyborgBoss.AddComponent(new CRaycasts());
            cyborgBoss.AddComponent(new CHealthPool(100));
            cyborgBoss.Size = new Vector2(4);
            cyborgBoss.Pos = i.obj.pos - cyborgBoss.Size / 2;
            cyborgBoss.active = false;
            return new GameObject[] { cyborgBoss };
        }

        private GameObject[] Rep_SnowmanBoss(ReplacerInput i)
        {
            GameObject snowman = new GameObject("boss", this, 2);
            CAnimatedSprite animBoss = new CAnimatedSprite();
            animBoss.AddAnimation("snowmanBossIdle", "snowmanBossIdle");
            animBoss.PlayAnimation("snowmanBossIdle", 8);
            snowman.AddComponent(animBoss);
            snowman.AddComponent(new CAABB());
            snowman.AddComponent(new CDamageDealer(30, false));
            snowman.AddComponent(new CHealthPool(1000));
            snowman.AddComponent(new CFaction("enemy"));
            snowman.AddComponent(new CSnowmanBoss());
            snowman.Size = new Vector2(3, 6);
            snowman.Pos = i.obj.pos - snowman.Size;
            return new GameObject[] { snowman };
        }

        public override void Unload() { }

        public override void Update(float time)
        {
            base.Update(time);
            float health = healthpool.HealhPercent;
            healthbar.Pos = new Vector2(0f, 9f - 2.47f * health);
            float mana = manapool.ManaPercentage;
            manabar.Pos = new Vector2(0.65f, 9f - 2.47f * mana);

            if (magicness.UnlockedFitness)
                fitness.Size = new Vector2(0.8f);
            else fitness.Size = new Vector2(0f);
            if (magicness.UnlockedHealing)
                healing.Size = new Vector2(0.8f);
            else healing.Size = new Vector2(0f);
            if (magicness.UnlockedLightning)
                lightning.Size = new Vector2(0.8f);
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
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}

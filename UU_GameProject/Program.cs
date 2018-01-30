using System;
using Core;

namespace UU_GameProject
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Run run = new Run();
        }
    }

    public class Run
    {
        private GameWindow game;
        
        public Run()
        {
            game = new GameWindow(1500, false, 4096);
            game.SetLoad(Load);
            game.Run();
        }
        
        private void Load()
        {
            PrerunGenerationCycle generator = new PrerunGenerationCycle();
            generator.GenTest();
            TextureManager.LoadTexture("block", "block");
            TextureManager.LoadTexture("suprise", "suprise");
            TextureManager.LoadTexture("cross", "cross");
            TextureManager.LoadTexture("player", "playerConcept");
            TextureManager.LoadTexture("Menu_Button_1", "Menu_Button_1");
            TextureManager.LoadTexture("Menu_Button_2", "Menu_Button_2");
            TextureManager.LoadTexture("Menu_Button_3", "Menu_Button_3");
            TextureManager.LoadTexture("menu", "menutile");
            TextureManager.LoadTexture("background", "Background");
            TextureManager.LoadTexture("tutorialSign1", "Tutorial_Sign");
            TextureManager.LoadTexture("tutorialSign2", "Tutorial_Sign_2");
            TextureManager.LoadTexture("tutorialSign3", "Tutorial_Sign_3");
            TextureManager.LoadTexture("bossSignRight", "Boss_Sign_Right");
            TextureManager.LoadTexture("bossSignLeft", "Boss_Sign_Left");
            TextureManager.LoadTexture("sky", "sky");
            TextureManager.LoadTexture("spike", "Spikey_Bois");
            TextureManager.LoadTexture("radarDot", "Radar_Dot", 2, 2);
            TextureManager.LoadTexture("radarBase", "Radar_Probe_Base", 4, 1);
            TextureManager.LoadTexture("respawnpointOn", "Spawnpoint_Unlocked", 2, 1);
            TextureManager.LoadTexture("respawnpointOff", "Spawnpoint_Locked", 2, 1);
            TextureManager.LoadTexture("fireball", "Fireball", 2, 1);
            TextureManager.LoadTexture("lightningStrike", "Lightning_Strike", 5, 4);
            TextureManager.LoadTexture("lightningSpawner", "Lightning_Spawner", 2, 1);
            TextureManager.LoadTexture("fireballMirrored", "Fireball_Mirror", 2, 1);
            TextureManager.LoadTexture("playerStandingRight", "Player_Standing_Right", 2, 1);
            TextureManager.LoadTexture("playerStandingLeft", "Player_Standing_Left", 2, 1);
            TextureManager.LoadTexture("playerRunningRight", "Player_Running_Right", 8, 1);
            TextureManager.LoadTexture("playerRunningLeft", "Player_Running_Left", 8, 1);
            TextureManager.LoadTexture("playerCrouching", "playerCrouchingSimple", 2, 1);
            TextureManager.LoadTexture("playerCrawlingRight", "Player_Crawling_Right", 2, 1);
            TextureManager.LoadTexture("playerCrawlingLeft", "Player_Crawling_Left", 2, 1);
            TextureManager.LoadTexture("playerSliding", "playerSlidingSimple", 2, 1);
            TextureManager.LoadTexture("playerMelee", "playerMeleeSimple", 4, 1);
            TextureManager.LoadTexture("playerAirborneRight", "Player_Airborne_Right", 2, 1);
            TextureManager.LoadTexture("playerAirborneLeft", "Player_Airborne_Left", 2, 1);
            TextureManager.LoadTexture("playerWallSlidingRight", "Player_Wall_Sliding_Right", 2, 1);
            TextureManager.LoadTexture("playerWallSlidingLeft", "Player_Wall_Sliding_Left", 2, 1);
            TextureManager.LoadTexture("playerFallPanic", "playerFallPanicSimple", 4, 1);
            TextureManager.LoadTexture("playerWeapon", "Player_Weapon", 2, 1);
            TextureManager.LoadTexture("playerWeaponLit", "Player_Weapon_Lit", 2, 1);
            TextureManager.LoadTexture("playerWeaponLightning", "Player_Weapon_Lightning", 2, 1);
            TextureManager.LoadTexture("cyborgBossBouncing1", "Cyborg_Boss_Slime_Bouncing_1", 8, 1);
            TextureManager.LoadTexture("cyborgBossBouncing2", "Cyborg_Boss_Slime_Bouncing_2", 8, 1);
            TextureManager.LoadTexture("cyborgBossBouncing3", "Cyborg_Boss_Slime_Bouncing_3", 8, 1);
            TextureManager.LoadTexture("cyborgBossBouncing4", "Cyborg_Boss_Slime_Bouncing_4", 8, 1);
            TextureManager.LoadTexture("robotBossWalking", "Spider_Boss_Walking", 2, 1);
            TextureManager.LoadTexture("robotBossFlying", "Spider_Boss_Flying", 2, 1);
            TextureManager.LoadTexture("robotBossFalling", "Spider_Boss_Falling", 2, 1);
            TextureManager.LoadTexture("robotBossLasers", "Spider_Boss_Lasers", 2, 1);
            TextureManager.LoadTexture("mageBossHovering", "Arch_Mage_Hovering", 8, 1);
            TextureManager.LoadTexture("mageBossFireball", "Arch_Mage_Fireball", 8, 1);
            TextureManager.LoadTexture("mageBossLightning", "Arch_Mage_Lightning", 8, 1);
            TextureManager.LoadTexture("snowmanBossIdle", "Snowman_Boss_Idle", 2, 1);
            TextureManager.LoadTexture("redMageStandingRight", "Red_Mage_Standing_Right", 4, 1);
            TextureManager.LoadTexture("redMageStandingLeft", "Red_Mage_Standing_Left", 4, 1);
            TextureManager.LoadTexture("redMageCastingRight", "Red_Mage_Casting_Right", 4, 1);
            TextureManager.LoadTexture("redMageCastingLeft", "Red_Mage_Casting_Left", 4, 1);
            TextureManager.LoadTexture("greenMageStandingRight", "Green_Mage_Standing_Right", 4, 1);
            TextureManager.LoadTexture("greenMageStandingLeft", "Green_Mage_Standing_Left", 4, 1);
            TextureManager.LoadTexture("greenMageCastingRight", "Green_Mage_Casting_Right", 4, 1);
            TextureManager.LoadTexture("greenMageCastingLeft", "Green_Mage_Casting_Left", 4, 1);
            TextureManager.LoadTexture("purpleMageStandingRight", "Purple_Mage_Standing_Right", 4, 1);
            TextureManager.LoadTexture("purpleMageStandingLeft", "Purple_Mage_Standing_Left", 4, 1);
            TextureManager.LoadTexture("purpleMageCastingRight", "Purple_Mage_Casting_Right", 4, 1);
            TextureManager.LoadTexture("purpleMageCastingLeft", "Purple_Mage_Casting_Left", 4, 1);
            TextureManager.LoadTexture("redSlimeMovingRight", "Red_Slime_Moving_Right", 8, 1);
            TextureManager.LoadTexture("redSlimeMovingLeft", "Red_Slime_Moving_Left", 8, 1);
            TextureManager.LoadTexture("robotSlimeMovingRight", "Robot_Slime_Moving_Right", 8, 1);
            TextureManager.LoadTexture("robotSlimeMovingLeft", "Robot_Slime_Moving_Left", 8, 1);
            TextureManager.LoadTexture("armoredCyborgRight", "Armored_Cyborg_Slime_Moving_Right", 8, 1);
            TextureManager.LoadTexture("armoredCyborgLeft", "Armored_Cyborg_Slime_Moving_Left", 8, 1);
            TextureManager.LoadTexture("armoredRobotRight", "Armored_Robot_Slime_Moving_Right", 8, 1);
            TextureManager.LoadTexture("armoredRobotLeft", "Armored_Robot_Slime_Moving_Left", 8, 1);
            TextureManager.LoadTexture("rangedCyborgRight", "Cyborg_Ranged_Right", 8, 1);
            TextureManager.LoadTexture("rangedCyborgLeft", "Cyborg_Ranged_Left", 8, 1);
            TextureManager.LoadTexture("normalCyborgRight", "Cyborg_Slime_Moving_Right", 8, 1);
            TextureManager.LoadTexture("normalCyborgLeft", "Cyborg_Slime_Moving_Left", 8, 1);
            TextureManager.LoadTexture("bullet", "Bullet", 2, 1);
            TextureManager.LoadTexture("repeat", "block", 0, 0);
            AudioManager.LoadTrack("moonlightsonata", "beethoven");
            AudioManager.LoadEffect("jump", "jump");
            AudioManager.LoadEffect("dead", "dead");
            AudioManager.LoadEffect("healing", "healing");
            AudioManager.LoadEffect("hit", "hit");
            AudioManager.LoadEffect("kill", "kill");
            AudioManager.LoadEffect("lightning", "lightning");
            AudioManager.LoadEffect("melee", "melee");
            AudioManager.LoadEffect("shoot", "shoot"); 
            TestMenu testMenu = new TestMenu();
            TestOptions testOptions = new TestOptions();
            TestGame testGame = new TestGame();
            ShowCaseScene showcase = new ShowCaseScene();
            TestGameOver testGameOver = new TestGameOver();
            LevelEditor editor = new LevelEditor();
            LevelTest levelTest = new LevelTest();
            game.states.AddState("editor", editor);
            game.states.AddState("leveltest", levelTest);
            game.states.AddState("gameover", testGameOver);
            game.states.AddState("menu", testMenu);
            game.states.AddState("options", testOptions);
            game.states.AddState("game", testGame);
            game.states.AddState("show", showcase);
            game.states.SetStartingState("menu");
            DataManager.SetData<float>("mastervolume", 1f);
            DataManager.SetData<float>("trackvolume", 1f);
            DataManager.SetData<float>("effectvolume", 1f);
        }
    }
}
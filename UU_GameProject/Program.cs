﻿using System;
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
            game = new GameWindow(1600);
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
            TextureManager.LoadTexture("menu", "menutile");
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
            Select select = new Select();
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
            game.states.AddState("select", select);
            game.states.AddState("game", testGame);
            game.states.AddState("show", showcase);
            game.states.SetStartingState("game");
            DataManager.SetData<float>("mastervolume", 1f);
            DataManager.SetData<float>("trackvolume", 1f);
            DataManager.SetData<float>("effectvolume", 1f);
        }
    }
}
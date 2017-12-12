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
            game = new GameWindow(1200);
            game.SetLoad(Load);
            game.Run();
        }

        private void Load()
        {
            TextureManager.LoadTexture("block", "block");
            TextureManager.LoadTexture("suprise", "suprise");
            TextureManager.LoadTexture("player", "playerConcept");
            TextureManager.LoadTexture("playerCrouched", "playerCrouchedConcept");
            TestMenu testMenu = new TestMenu();
            TestGame testGame = new TestGame();
            TestGameOver testGameOver = new TestGameOver();
            game.states.AddState("gameover", testGameOver);
            game.states.AddState("menu", testMenu);
            game.states.AddState("game", testGame);
            game.states.SetStartingState("menu");
        }
    }
}

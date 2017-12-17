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
            game = new GameWindow(1000);
            game.SetLoad(Load);
            game.Run();
        }
        
        private void Load()
        {
            PrerunGenerationCycle generator = new PrerunGenerationCycle();
            generator.GenTest();
            TextureManager.LoadTexture("block", "block");
            TextureManager.LoadTexture("suprise", "suprise");
            TextureManager.LoadTexture("player", "playerConcept");
            TextureManager.LoadTexture("playerWalking", "playerWalkingSimple", 4, 1);
            TextureManager.LoadTexture("playerCrouching", "playerCrouchingSimple", 1, 1);
            TextureManager.LoadTexture("playerCrawling", "playerCrawlingSimple", 4, 1);
            TextureManager.LoadTexture("playerSliding", "playerSlidingSimple", 4, 1);
            TextureManager.LoadTexture("playerMelee", "playerMeleeSimple", 4, 1);
            TextureManager.LoadTexture("playerAirborn", "playerAirbornimple", 4, 1);
            TextureManager.LoadTexture("playerFallPanic", "playerFallPanicSimple", 4, 1);
            TestMenu testMenu = new TestMenu();
            TestGame testGame = new TestGame();
            ShowCaseScene showcase = new ShowCaseScene();
            TestGameOver testGameOver = new TestGameOver();
			
            game.states.AddState("gameover", testGameOver);
            game.states.AddState("menu", testMenu);
            game.states.AddState("game", testGame);
            game.states.AddState("show", showcase);
            game.states.SetStartingState("game");
        }
    }
}

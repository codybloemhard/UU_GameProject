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
            TextureManager.LoadTexture("player", "playerConcept");
            TextureManager.LoadTexture("playerWalking", "playerWalkingSimple", 4, 1);
            TextureManager.LoadTexture("playerCrouching", "playerCrouchingSimple", 2, 1);
            TextureManager.LoadTexture("playerCrawling", "playerCrawlingSimple", 4, 1);
            TextureManager.LoadTexture("playerSliding", "playerSlidingSimple", 2, 1);
            TextureManager.LoadTexture("playerMelee", "playerMeleeSimple", 4, 1);
            TextureManager.LoadTexture("playerAirborn", "playerAirbornSimple", 4, 1);
            TextureManager.LoadTexture("playerFallPanic", "playerFallPanicSimple", 4, 1);
            TestMenu testMenu = new TestMenu();
            TestOptions testOptions = new TestOptions();
            Select select = new Select();
            TestGame testGame = new TestGame();
            LevelEditor levelEditor = new LevelEditor();
            ShowCaseScene showcase = new ShowCaseScene();
            TestGameOver testGameOver = new TestGameOver();
			
            game.states.AddState("editor", levelEditor);
            game.states.AddState("gameover", testGameOver);
            game.states.AddState("menu", testMenu);
            game.states.AddState("options", testOptions);
            game.states.AddState("select", select);
            game.states.AddState("game", testGame);
            game.states.AddState("show", showcase);
            game.states.SetStartingState("menu");
        }
    }
}

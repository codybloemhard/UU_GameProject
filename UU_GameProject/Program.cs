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
            TextureManager.LoadTexture("playerCrouched", "playerCrouchedConcept");
            TestMenu testMenu = new TestMenu();
            TestGame testGame = new TestGame();
            LevelEditor levelEditor = new LevelEditor();
            ShowCaseScene showcase = new ShowCaseScene();
            TestGameOver testGameOver = new TestGameOver();
          
	        game.states.AddState("editor", levelEditor);
            game.states.AddState("gameover", testGameOver);
            game.states.AddState("menu", testMenu);
            game.states.AddState("game", testGame);
            game.states.AddState("show", showcase);
            game.states.SetStartingState("show");
        }
    }
}

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
            game = new GameWindow(1920);
            game.SetLoad(Load);
            game.Run();
        }

        private void Load()
        {
            PrerunGenerationCycle generator = new PrerunGenerationCycle();
            generator.GenTest();
            TextureManager.LoadTexture("block", "block");
            TextureManager.LoadTexture("suprise", "suprise");
            TextureManager.LoadTexture("dude", "player");
            TestMenu testMenu = new TestMenu();
            TestGame testGame = new TestGame();
            ShowCaseScene showcase = new ShowCaseScene();
            game.states.AddState("menu", testMenu);
            game.states.AddState("game", testGame);
            game.states.AddState("show", showcase);
            game.states.SetStartingState("show");
        }
    }
}

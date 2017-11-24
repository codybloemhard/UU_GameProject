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
            TestMenu testMenu = new TestMenu();
            TestGame testGame = new TestGame();
            game.states.AddState("menu", testMenu);
            game.states.AddState("game", testGame);
            game.states.SetStartingState("menu");
        }
    }
}

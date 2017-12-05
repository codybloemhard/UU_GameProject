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
            TestMenu testMenu = new TestMenu();
            TestGame testGame = new TestGame();
            LevelEditor levelEditor = new LevelEditor();
            game.states.AddState("menu", testMenu);
            game.states.AddState("game", testGame);
            game.states.AddState("editor", levelEditor);
            game.states.SetStartingState("editor");
        }
    }
}

using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    public class TestMenu : GameState
    {
        private Text text, testTimer;
        private Button buttonStart, buttonOptions, buttonContinue, buttonQuit;


        public TestMenu() : base() { }
        
        public override void Load(SpriteBatch batch)
        {
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");
            text = new Text(this, "The Crashed Janitor", new Vector2(0f, 1f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);
            text.tag = "exampleTag";
            testTimer = new Text(this, "time: ", new Vector2(0, 8), new Vector2(16f, 1f), font);
            testTimer.colour = new Color(0, 255, 0);
            testTimer.tag = "timerText";

            //buttons
            buttonStart = new Button(this, "New game", "menutile2", () => GameStateManager.RequestChange("select", CHANGETYPE.LOAD),
                font, new Vector2(6, 3), new Vector2(4, 1f));
            buttonStart.SetupColours(Color.BlanchedAlmond, Color.BurlyWood, Color.DarkGray, Color.Red);

            buttonContinue = new Button(this, "Continue. Most certainly implemented", "menutile2", () => GameStateManager.RequestChange("game", CHANGETYPE.LOAD),
                font, new Vector2(6, 4.2f), new Vector2(4, 1f));
            buttonContinue.SetupColours(Color.BlanchedAlmond, Color.BurlyWood, Color.DarkGray, Color.Red);

            buttonOptions = new Button(this, "Options", "menutile2", () => GameStateManager.RequestChange("options", CHANGETYPE.LOAD), 
                font, new Vector2(6, 5.4f), new Vector2(4, 1f));
            buttonOptions.SetupColours(Color.BlanchedAlmond, Color.BurlyWood, Color.DarkGray, Color.Red);

            buttonQuit = new Button(this, "Exit. Also definetly implemented", "menutile2", () => GameStateManager.RequestChange("game", CHANGETYPE.LOAD),
                font, new Vector2(6, 6.6f), new Vector2(4, 1f));
            buttonQuit.SetupColours(Color.BlanchedAlmond, Color.BurlyWood, Color.DarkGray, Color.Red);

            //others
            Timers.Add("timer", 10, changeTextColour);
            Camera.SetCameraTopLeft(new Vector2(0, 0));
        }

        private void changeTextColour()
        {
            ui.FindWithTag("timerText").colour = new Color(255, 0, 0);
        }

        public override void Unload()
        {
            Timers.FindWithTag("timer").Reset();
        }

        public override void Update(float time)
        {
            string msg = "Time: " + MathH.Float(Timers.FindWithTag("timer").TimeLeft, 2);
            (ui.FindWithTag("timerText") as Text).text = msg;
            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
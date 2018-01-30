using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    public class TestMenu : GameState
    {
        public TestMenu() : base() { }

        public override void Load(SpriteBatch batch)
        {
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");
            Text text = new Text(this, "The Crashed Janitor", new Vector2(0f, 1f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);
            text.tag = "exampleTag";
            Text testTimer = new Text(this, "time: ", new Vector2(0, 8), new Vector2(16f, 1f), font);
            testTimer.colour = new Color(0, 255, 0);
            testTimer.tag = "timerText";

            UITextureElement background = new UITextureElement(this, "Menu_Background", Vector2.Zero, new Vector2(16, 9));

            //buttons
            Button buttonStart = new Button(this, "Start", "Menu_Button_2", () => GameStateManager.RequestChange("game", CHANGETYPE.LOAD),
                font, new Vector2(5, 3.6f), new Vector2(6, 1f));
            buttonStart.SetupColours(Color.BlanchedAlmond, Color.BurlyWood, Color.DarkGray, Color.Yellow);

            Button buttonOptions = new Button(this, "Options", "Menu_Button_3", () => GameStateManager.RequestChange("options", CHANGETYPE.LOAD), 
                font, new Vector2(5, 5.0f), new Vector2(6, 1f));
            buttonOptions.SetupColours(Color.BlanchedAlmond, Color.BurlyWood, Color.DarkGray, Color.Yellow);

            Button buttonQuit = new Button(this, "Exit", "Menu_Button_1", () => GameStateManager.RequestChange("editor", CHANGETYPE.LOAD),
                font, new Vector2(5, 6.4f), new Vector2(6, 1f));
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
            //(ui.FindWithTag("timerText") as Text).text = msg;
            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
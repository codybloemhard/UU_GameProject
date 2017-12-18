using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    public class TestInGameOptions : GameState
    {
        private Text text, text2;
        private Button buttonMainmenu, fullscreen;
        private SliderBar audioslider;
        private bool Fullscreen;
        private uint width, height;


        public TestInGameOptions() : base() { }

        public override void Load(SpriteBatch batch)
        {
            width = 1000;
            height = width / 16 * 9;
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");
            text = new Text(this, "Options", new Vector2(0f, 1f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);

            fullscreen = new Button(this, "Full screen", "block", () => setFullscreen(),
                font, new Vector2(6, 3f), new Vector2(4, 1.2f));
            fullscreen.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);

            text2 = new Text(this, "Audio", new Vector2(0f, 4.5f), new Vector2(16f, 1f), font);
            text2.colour = new Color(255, 0, 0);
            audioslider = new SliderBar(this, "block", "block", 1, new Vector2(10, 4.5f), new Vector2(4, 1.2f), "x");
            audioslider.colour = Color.Red;

            buttonMainmenu = new Button(this, "Main menu", "block", () => GameStateManager.RequestChange("menu", CHANGETYPE.LOAD),
                font, new Vector2(12, 7.8f), new Vector2(4, 1.2f));
            buttonMainmenu.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);


        }

        private void setFullscreen()
        {
            if (!Fullscreen)
            {
                Camera.SetupResolution((uint)Camera.ScreenSize.X, (uint)Camera.ScreenSize.Y, true);
                Fullscreen = true;
            }
            else
            {
                Camera.SetupResolution(width, height, false);
                Fullscreen = false;
            }
        }

        public void setWidth(uint x)
        {
            width = x;
            height = width / 16 * 9;
            if (!Fullscreen)
                Camera.SetupResolution(width, height, false);
        }

        public override void Unload()
        { }

        public override void Update(float time)
        {
            base.Update(time);
            //float percentage = 1 - audioslider.GetValue;
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
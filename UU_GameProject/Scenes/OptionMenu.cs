using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    public class OptionMenu : GameState
    {
        private Text text, text2, text3, text4;
        private Button buttonMainmenu, fullscreen;
        private SliderBar masteraudio, musicaudio, sfaudio;
        private bool Fullscreen;
        private uint width, height;

        public OptionMenu() : base() { }

        public override void Load(SpriteBatch batch)
        {
            width = 1000;
            height = width / 16 * 9;
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");
            text = new Text(this, "Options", new Vector2(0f, 0.5f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);

            UITextureElement background = new UITextureElement(this, "Background", Vector2.Zero, new Vector2(16, 16));

            //buttons
            fullscreen = new Button(this, "Full screen", "Menu_Button_2", () => setFullscreen(),
                font, new Vector2(6, 2f), new Vector2(4, 0.6f));
            fullscreen.SetupColours(Color.White, Color.BurlyWood, Color.DarkGray, Color.White);

            text2 = new Text(this, "Audio", new Vector2(-2f, 3.4f), new Vector2(16f, 1f), font);
            text2.colour = new Color(255, 0, 0);
            masteraudio = new SliderBar(this, "Spot1", "Spunge", 1, new Vector2(8, 3.4f), new Vector2(4, 1.0f), "x");
            masteraudio.colour = Color.BurlyWood;
            masteraudio.Value = DataManager.GetData<float>("mastervolume");

            text3 = new Text(this, "Music", new Vector2(-2f, 4.6f), new Vector2(16f, 1f), font);
            text3.colour = new Color(255, 0, 0);
            musicaudio = new SliderBar(this, "Spot2", "Spunge", 1, new Vector2(8, 4.6f), new Vector2(4, 1.0f), "x");
            musicaudio.colour = Color.BurlyWood;
            musicaudio.Value = DataManager.GetData<float>("trackvolume");

            text4 = new Text(this, "Sound Effects", new Vector2(-2f, 5.8f), new Vector2(16f, 1f), font);
            text4.colour = new Color(255, 0, 0);
            sfaudio = new SliderBar(this, "Spot3", "Spunge", 1, new Vector2(8, 5.8f), new Vector2(4, 1.0f), "x");
            sfaudio.colour = Color.BurlyWood;
            sfaudio.Value = DataManager.GetData<float>("effectvolume");

            buttonMainmenu = new Button(this, "Main menu", "Menu_Button_2", () => GameStateManager.RequestChange("menu", CHANGETYPE.LOAD),
                font, new Vector2(12, 7.8f), new Vector2(4, 0.6f));
            buttonMainmenu.SetupColours(Color.White, Color.BurlyWood, Color.DarkGray, Color.White);
        }

        private void setFullscreen()
        {
            if (!Fullscreen)
            {
                Camera.SetupResolution((uint)Camera.ScreenSize.X, (uint)Camera.ScreenSize.Y, true);
                Fullscreen = true;
            } else
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

        public override void Unload() { }

        public override void Update(float time)
        {
            base.Update(time);
            AudioManager.SetMasterVolume(masteraudio.Value);
            AudioManager.SetEffectVolume(sfaudio.Value);
            AudioManager.SetTrackVolume(musicaudio.Value);
            DataManager.SetData<float>("mastervolume", masteraudio.Value);
            DataManager.SetData<float>("trackvolume", musicaudio.Value);
            DataManager.SetData<float>("effectvolume", sfaudio.Value);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
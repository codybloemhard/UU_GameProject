using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    public class TestOptions : GameState
    {
        private Text text;
        private Button buttonMainmenu;


        public TestOptions() : base() { }

        public override void Load(SpriteBatch batch)
        {
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");
            text = new Text(this, "Options", new Vector2(0f, 1f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);
            buttonMainmenu = new Button(this, "Main menu", "block", () => GameStateManager.RequestChange("menu", CHANGETYPE.LOAD),
                font, new Vector2(12, 7.8f), new Vector2(4, 1.2f));
            buttonMainmenu.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);


        }

        public override void Unload()
        { }

        public override void Update(float time)
        { base.Update(time); }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
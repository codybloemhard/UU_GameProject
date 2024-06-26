﻿using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    public class GameOverMenu : GameState
    {
        public GameOverMenu() : base() { }

        public override void Load(SpriteBatch batch)
        {
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");
            Text text = new Text(this, "You died!", new Vector2(0f, 2f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);
            text.tag = "exampleTag";
            Button button = new Button(this, "Back to main menu!", "block", () => GameStateManager.RequestChange("menu", CHANGETYPE.LOAD),
                font, new Vector2(6, 4), new Vector2(4, 3));
            button.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
        }

        public override void Unload() { }

        public override void Update(float time)
        {
            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
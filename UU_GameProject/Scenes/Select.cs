using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    class Select : GameState
    {
        private Button dude, dudette, thing, thing2;
        private Text text;

        public Select() : base() { }

        public override void Load(SpriteBatch batch)
        {
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");

            text = new Text(this, "Select character concept menu", new Vector2(0f, 0.5f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);

            dude = new Button(this, "", "block", () => GameStateManager.RequestChange("game", CHANGETYPE.LOAD),
                font, new Vector2(4, 2), new Vector2(3, 5f));
            dude.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            thing = new Button(this, "", "playerConcept", () => GameStateManager.RequestChange("game", CHANGETYPE.LOAD),
                font, new Vector2(4.25f, 2), new Vector2(2.5f, 5f));
            thing.SetupColours(Color.Blue, Color.Blue, Color.Blue, Color.Blue);

            dudette = new Button(this, "", "block", () => GameStateManager.RequestChange("game", CHANGETYPE.LOAD),
                font, new Vector2(9, 2f), new Vector2(3, 5f));
            dudette.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            thing2 = new Button(this, "", "playerConcept", () => GameStateManager.RequestChange("game", CHANGETYPE.LOAD),
                font, new Vector2(9.25f, 2f), new Vector2(2.5f, 5f));
            thing2.SetupColours(Color.DeepPink, Color.DeepPink, Color.DeepPink, Color.DeepPink);
        }

        public override void Unload()
        {

        }

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

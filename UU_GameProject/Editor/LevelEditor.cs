using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Core;
using System.IO;
using System.Collections.Generic;

namespace UU_GameProject
{
    public class LevelEditor : GameState
    {
        public override void Load(SpriteBatch batch)
        {
            Button button = new Button(this, "Finish", "block", () => Finish(true),
                AssetManager.GetResource<SpriteFont>("mainFont"), new Vector2(14, 0), new Vector2(2, 1));
            Button quit = new Button(this, "Cancel", "block", () => Finish(false),
                AssetManager.GetResource<SpriteFont>("mainFont"), new Vector2(12, 0), new Vector2(2, 1));
            button.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            quit.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
        }

        public override void Unload() { }

        public override void Update(float time)
        {
            base.Update(time);
            if (Input.GetKey(PressAction.PRESSED, Keys.Q))
            {
                GameObject newObject = new GameObject("new", this, 0, true);
                newObject.AddComponent(new CRender("block"));
                newObject.AddComponent(new CAABB());
                newObject.AddComponent(new CLevelEditorObject(newObject));
                newObject.Pos = Input.GetMousePosition();
                newObject.Size = new Vector2(1f, 1f);
            }
            if (Input.GetKey(PressAction.DOWN, Keys.Left))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(-0.01f, 0));
            else if (Input.GetKey(PressAction.DOWN, Keys.Right))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(+0.01f, 0));
            if (Input.GetKey(PressAction.DOWN, Keys.Up))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(0, -0.01f));
            else if (Input.GetKey(PressAction.DOWN, Keys.Down))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(0, +0.01f));
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }

        public void Finish(bool save)
        {
            if (save)
            {
                int x = 0, y = 0;
                try
                {
                    int.TryParse(Console.ReadLine(), out x);
                    int.TryParse(Console.ReadLine(), out y);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not set chunk position!");
                }
                Console.WriteLine("Saving chunk with position (" + x + "," + y + ").");
                LevelLogic.WriteChunk("chunk" + x + y + ".lvl", x, y);
            }
            GameStateManager.RequestChange("leveltest", CHANGETYPE.LOAD);
        }
    }
}
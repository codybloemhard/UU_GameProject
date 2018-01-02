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
        }

        public override void Unload() { }

        public override void Update(float time)
        {
            base.Update(time);
            if (Input.GetKey(PressAction.PRESSED, Keys.Enter))
            {
                GameObject newObject = new GameObject("new", this, 0, true);
                newObject.AddComponent(new CRender("block"));
                newObject.AddComponent(new CAABB());
                newObject.AddComponent(new CLevelEditorObject(newObject));
                newObject.Pos = Input.GetMousePosition();
                newObject.Size = new Vector2(1f, 1f);
            }
            if (Input.GetKey(PressAction.DOWN, Keys.Right))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(0.01f, 0));
            else if (Input.GetKey(PressAction.DOWN, Keys.Left))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(-0.01f, 0));
            if (Input.GetKey(PressAction.DOWN, Keys.Up))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(0, -0.01f));
            else if (Input.GetKey(PressAction.DOWN, Keys.Down))
                Camera.SetCameraTopLeft(Grid.ToGridSpace(Camera.TopLeft) + new Vector2(0, 0.01f));
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }

        public void Finish(bool save)
        {
            if(save) LevelLogic.WriteLevel(LevelLogic.testurl);
            GameStateManager.RequestChange("leveltest", CHANGETYPE.LOAD);
        }
    }
}
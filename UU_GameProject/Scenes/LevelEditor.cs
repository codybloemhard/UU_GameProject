using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Core;
using System.IO;
using System.Collections.Generic;

namespace UU_GameProject
{
    class LevelEditor : GameState
    {
        List<GameObject> allObjects = new List<GameObject>();

        public override void Load(SpriteBatch batch)
        {
            Button button = new Button(this, "Finish", "block", () => Finish(),
                AssetManager.GetResource<SpriteFont>("mainFont"), new Vector2(14, 0), new Vector2(2, 1));
            Console.WriteLine(AssetManager.content.RootDirectory);
        }

        public override void Unload()
        {
        }

        public override void Update(float time)
        {
            Camera.SetCameraTopLeft(new Vector2(0, 0));
            if (Input.GetKey(PressAction.PRESSED, Keys.Enter))
            {
                GameObject newObject = new GameObject("new", this, 0, true);
                allObjects.Add(newObject);
                newObject.AddComponent(new CRender("block"));
                newObject.AddComponent(new CAABB());                
                newObject.AddComponent(new CLevelEditorObject());
                newObject.Pos = Input.GetMousePosition();
                newObject.Size = new Vector2(1, 1);
            }
            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }

        public void Finish()
        {

            using (StreamWriter fileWriter = new StreamWriter("../../../../Content/level.txt", false))
            {
                fileWriter.AutoFlush = true;

                for(int i = 0; i < allObjects.Count; i++)
                {
                    fileWriter.Write(allObjects[i].tag + "|");
                    fileWriter.Write(allObjects[i].Pos.X + "/" +allObjects[i].Pos.Y + "|");
                    fileWriter.WriteLine(allObjects[i].Size.X + "/" + allObjects[i].Size.Y);
                }
            }
            GameStateManager.RequestChange("game", CHANGETYPE.LOAD);
        }
    }
}
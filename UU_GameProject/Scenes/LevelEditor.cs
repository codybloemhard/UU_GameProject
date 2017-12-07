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
                Console.Write("Texture: ");
                string texture = Console.ReadLine();
                Console.Write("Width: ");
                float width = float.Parse(Console.ReadLine());
                Console.Write("Height: ");
                float height = float.Parse(Console.ReadLine());

                GameObject newObject = new GameObject("new", this, 0, true);
                newObject.AddComponent(new CRender(texture));
                newObject.AddComponent(new CAABB());                
                newObject.AddComponent(new CLevelEditorObject());

                newObject.Pos = Input.GetMousePosition() - new Vector2(0.5f, 0.5f);
                newObject.Size = new Vector2(width, height);
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
                fileWriter.WriteLine("blegh");
            }

            using (StreamReader fileReader = new StreamReader("../../../../Content/level.txt"))
            {
                Console.WriteLine(fileReader.ReadLine());
            }

            GameStateManager.RequestChange("game", CHANGETYPE.LOAD);
        }
    }
}
﻿using System;
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
        private const string baseurl = "../../../../Content/Levels/";

        public override void Load(SpriteBatch batch)
        {
            Button test = new Button(this, "Test", "block", () => GameStateManager.RequestChange("game", CHANGETYPE.LOAD),
                AssetManager.GetResource<SpriteFont>("mainFont"), new Vector2(14, 0), new Vector2(2, 1));
            Button save = new Button(this, "Save", "block", () => Save(true),
                AssetManager.GetResource<SpriteFont>("mainFont"), new Vector2(12, 0), new Vector2(2, 1));
            Button load = new Button(this, "Load", "block", () => LevelLogic.EditChunk(Console.ReadLine(), baseurl, this),
                AssetManager.GetResource<SpriteFont>("mainFont"), new Vector2(10, 0), new Vector2(2, 1));
            Button unload = new Button(this, "Unload", "block", () => Unload(),
                AssetManager.GetResource<SpriteFont>("mainFont"), new Vector2(8, 0), new Vector2(2, 1));
            test.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            save.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            load.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            unload.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
        }

        //emptying editor
        public override void Unload()
        {
            throw new Exception("Disbled for fixing, needs rewrite");
            //StaticObjects and Objects private now
            /*for (int i = objects.StaticObjects.Count -1; i>=0; i--)
            {
                objects.StaticObjects[i].GetComponent<CLevelEditorObject>().Destroy();
            }
            for (int i = objects.Objects.Count - 1; i >= 0; i--)
            {
                objects.Objects[i].GetComponent<CLevelEditorObject>().Destroy();
            }*/
        }

        public override void Update(float time)
        {
            base.Update(time);
            //creating new blocks
            if(Input.GetKey(PressAction.PRESSED, Keys.W) && !CLevelEditorObject.Handling)
            {
                GameObject newObject = new GameObject("spawner", this, 0, true);
                newObject.AddComponent(new CRender("cross"));
                newObject.AddComponent(new CAABB());
                newObject.AddComponent(new CLevelEditorObject(newObject, true));
                newObject.Pos = new Vector2(Math.Max(Math.Min(Input.GetMouseWorldPosition().X, 15), 0), Math.Max(Math.Min(Input.GetMouseWorldPosition().Y, 15), 0));
                newObject.Size = new Vector2(1f, 1f);
            }
            if (Input.GetKey(PressAction.PRESSED, Keys.Q) && !CLevelEditorObject.Handling)
            {
                GameObject newObject = new GameObject("new", this, 0, true);
                newObject.AddComponent(new CRender("block"));
                newObject.AddComponent(new CAABB());
                newObject.AddComponent(new CLevelEditorObject(newObject, false));
                newObject.Pos = new Vector2(Math.Max(Math.Min(Input.GetMouseWorldPosition().X, 15), 0), Math.Max(Math.Min(Input.GetMouseWorldPosition().Y, 15), 0));
                newObject.Size = new Vector2(1f, 1f);
            }

            //moving camera
            if (Input.GetKey(PressAction.DOWN, Keys.Left))
                Camera.SetCameraTopLeft(Camera.TopLeft + new Vector2(-0.01f, 0));
            else if (Input.GetKey(PressAction.DOWN, Keys.Right))
                Camera.SetCameraTopLeft(Camera.TopLeft + new Vector2(+0.01f, 0));
            if (Input.GetKey(PressAction.DOWN, Keys.Up))
                Camera.SetCameraTopLeft(Camera.TopLeft + new Vector2(0, -0.01f));
            else if (Input.GetKey(PressAction.DOWN, Keys.Down))
                Camera.SetCameraTopLeft(Camera.TopLeft + new Vector2(0, +0.01f));

            //adding clarity lines
            lineRenderer.Clear();
            lineRenderer.Add(new Line(new Vector2(0) - Camera.TopLeft, new Vector2(16, 0) - Camera.TopLeft, Color.Red));
            lineRenderer.Add(new Line(new Vector2(0) - Camera.TopLeft, new Vector2(0, 16) - Camera.TopLeft, Color.Red));
            lineRenderer.Add(new Line(new Vector2(16) - Camera.TopLeft, new Vector2(16, 0) - Camera.TopLeft, Color.Red));
            lineRenderer.Add(new Line(new Vector2(16) - Camera.TopLeft, new Vector2(0, 16) - Camera.TopLeft, Color.Red));
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);

        }

        public void LoadNewFile(string path)
        {

        }
        
        //saves the current chunk
        public void Save(bool save)
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
                LevelLogic.WriteChunk(CLevelEditorObject.objectList, baseurl + "chunk" + x + y + ".lvl", x, y);
            }
        }
        
        private void TestChunks()//write 441 random chunks
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(new GameObject("!", this, 0));
            for (int i = 1; i < 10; i++)
            {
                GameObject go = new GameObject("solid", this, 0);
                go.Size = new Vector2(1, 1);
                list.Add(go);
            }
            for (int x = -10; x <= 10; x++)
                for (int y = -10; y <= 10; y++)
                {
                    for (int i = 1; i < 10; i++)
                    {
                        list[i].Pos = new Vector2((float)MathH.random.NextDouble() * 16,
                                                    (float)MathH.random.NextDouble() * 16);
                    }
                    list[0].Pos = list[1].Pos + (list[1].Size * new Vector2(0.5f, 0f)) - new Vector2(0.5f);
                    LevelLogic.WriteChunk(list, baseurl + "chunk" + x + y + ".lvl", x, y);
                }
        }
    }
}
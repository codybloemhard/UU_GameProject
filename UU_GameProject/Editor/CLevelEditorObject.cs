﻿using System;
using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UU_GameProject
{
    public class CLevelEditorObject : Component
    {
        private int select;
        private float precision = 2;
        private string backup;
        private static bool staticGrabbed, handling;
        private bool grabbed, axisAligned, spawner;
        private static GameObject selected;
        private Vector2 grabPoint;
        private MultipleLinesText properties;
        private float input;
        public static List<GameObject> objectList = new List<GameObject>();

        public CLevelEditorObject(GameObject GO, bool spawner)
        {
            List<string> text = new List<string>();
            text.Add("XSize:");
            text.Add("YSize:");
            text.Add("Tag:");
            properties = new MultipleLinesText(GO.Context, text, new Vector2(10, 3), new Vector2(0, 0), AssetManager.GetResource<SpriteFont>("mainFont"));
            properties.Pos = new Vector2(16, 9) - properties.Size;
            selected = GO;
            objectList.Add(GO);
            this.spawner = spawner;
        }

        public override void Update(float time)
        {
            base.Update(time);

            //moving around properties
            if (!(GO.Pos.X + GO.Size.X < 16 - properties.Size.X || GO.Pos.Y + GO.Size.Y < properties.Pos.Y))
                properties.Pos = new Vector2(0, 9) - new Vector2(0, properties.Size.Y);
            else 
                properties.Pos = new Vector2(16, 9) - properties.Size;

            Vector2 mousePos = Input.GetMouseWorldPosition();

            //selecging and grabbing objects
            if (Input.GetMouseButton(PressAction.PRESSED, MouseButton.LEFT))
            {
                if (GO.GetAABB().Inside(mousePos))
                {
                    selected = GO;
                    if (!staticGrabbed)
                    {
                        staticGrabbed = true;
                        grabbed = true;
                        grabPoint = mousePos - GO.Pos;
                    }
                }
                else if (selected == GO && properties.Hover == -1) selected = null;
            }
            else if (Input.GetMouseButton(PressAction.RELEASED, MouseButton.LEFT))
            {
                grabbed = staticGrabbed = false;
                grabPoint = Vector2.Zero;
            }

            //selected objects are grey, not destroying object while typing properties
            if (selected == GO)
            {
                HandleProperties(out handling);
                (GO.Renderer as CRender).colour = new Color(180, 180, 180);
                if(!handling)
                    if (Input.GetKey(PressAction.PRESSED, Keys.E))
                        Destroy();
                properties.active = true;
            }
            else
            {
                (GO.Renderer as CRender).colour = Color.White;
                properties.active = false;
            }

            //moving around, axis aligned or not
            if (Input.GetKey(PressAction.DOWN, Keys.LeftShift))
                axisAligned = true;
            else axisAligned = false;

            if (grabbed)
            {
                if (!axisAligned)
                    GO.Pos = new Vector2(Math.Max(Math.Min(mousePos.X - grabPoint.X, 16 - GO.Size.X), 0), Math.Max(Math.Min(mousePos.Y - grabPoint.Y, 16 - GO.Size.Y), 0));
                else
                    GO.Pos = new Vector2(Math.Max(Math.Min((int)(mousePos.X*precision) - (int)(grabPoint.X* precision), (16 - GO.Size.X) * precision), 0), Math.Max(Math.Min((int)(mousePos.Y* precision) - (int)(grabPoint.Y* precision), (16 - GO.Size.Y) * precision), 0))/ precision;
            }
        }

        //destroy object
        public void Destroy()
        {
            GO.Destroy();
            objectList.Remove(GO);
            properties.Destroy();
        }

        //selecting lines in the properties
        public void HandleProperties(out bool handling)
        {
            if(Input.GetKey(PressAction.PRESSED, Keys.Enter))
            {
                if (properties.selected != -1)
                {
                    HandleInput();
                    properties.text[properties.selected] = backup;
                    properties.selected = -1;
                }
            }

            if (Input.GetMouseButton(PressAction.PRESSED, MouseButton.LEFT))
            {
                if (properties.Hover != -1)
                {
                    if (properties.Hover != properties.selected)
                    {
                        if (properties.selected != -1)
                        {
                            HandleInput();
                            properties.text[properties.selected] = backup;
                        }
                        backup = properties.text[properties.Hover];
                    }
                    properties.text[properties.Hover] = "";
                }
                else if (properties.selected != -1)
                {
                    HandleInput();
                    properties.text[properties.selected] = backup;
                }
                properties.selected = properties.Hover;
            }

            if (Input.GetKey(PressAction.PRESSED, Keys.NumPad1))
                select = 0;
            else if (Input.GetKey(PressAction.PRESSED, Keys.NumPad2))
                select = 1;
            else if (Input.GetKey(PressAction.PRESSED, Keys.NumPad3))
                select = 2;
            else select = -1;

            if (select != -1)
            {
                if (select != properties.selected)
                {
                    if (properties.selected != -1)
                    {
                        HandleInput();
                        properties.text[properties.selected] = backup;
                    }
                    backup = properties.text[select];
                }
                properties.text[select] = "";
                properties.selected = select;
            }
            
            if (properties.selected != -1)
                properties.text[properties.selected] = Input.Type(properties.text[properties.selected]);
            if (properties.selected == -1)
                handling = false;
            else handling = true;
        }

        //handling the properties input
        public void HandleInput()
        {
            if (properties.text[properties.selected] != "")
            {
                if (backup == "XSize:")
                {
                    if (float.TryParse(properties.text[properties.selected], out input))
                    {
                        GO.Size = new Vector2(input, GO.Size.Y);
                        Console.WriteLine("XSize set to: " + properties.text[properties.selected]);
                    }
                    else
                        Console.WriteLine("'" + properties.text[properties.selected] + "'" + " is not a correct value");
                }
                if (backup == "YSize:")
                {
                    if (float.TryParse(properties.text[properties.selected], out input))
                    {
                        GO.Size = new Vector2(GO.Size.X, input);
                        Console.WriteLine("YSize set to: " + properties.text[properties.selected]);
                    }
                    else
                        Console.WriteLine("'" + properties.text[properties.selected] + "'" + " is not a correct value");

                }
                if (backup == "Tag:")
                {
                    if(!spawner)
                        GO.tag = properties.text[properties.selected];
                    else
                        GO.tag = "!" + properties.text[properties.selected];
                    Console.WriteLine("Tag set to: " + properties.text[properties.selected]);
                }
            }
        }


        public static bool StaticGrabbed { get { return staticGrabbed; } }
        public static bool Handling { get { return handling; } }
    }
}
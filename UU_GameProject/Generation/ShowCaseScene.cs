using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Core;

namespace UU_GameProject
{
    public class ShowCaseScene : GameState
    {
        public ShowCaseScene() : base() { }

        public override void Load(SpriteBatch batch)
        {
            GameStateManager.SetRenderingMode(BlendState.NonPremultiplied, SamplerState.PointWrap);
            Debug.ProfilingMode();
            GameObject backg = new GameObject(this, 20, true);
            backg.AddComponent(new CRender("block"));
            backg.Renderer.colour = new Color(0.7f, 0.7f, 0.7f);
            backg.Pos = Vector2.Zero;
            backg.Size = new Vector2(16, 9); 
        }
        
        public override void Unload() { }
        
        public override void Update(float time)
        {
            base.Update(time);
            if (Input.GetKey(PressAction.DOWN, Keys.P))
            {
                Debug.showAtlas = true;
                Debug.printData = true;
            }
            else
            {
                Debug.showAtlas = false;
                Debug.printData = false;
            }
            if(Input.GetKey(PressAction.PRESSED, Keys.Enter))
            {
                /*GameObject[] old = objects.FindAllWithTag("_test");
                if(old != null) foreach (GameObject o in old) o.Destroy();
                old = objects.FindAllWithTag("tree");
                if (old != null) foreach (GameObject o in old) o.Destroy();
                old = objects.FindAllWithTag("_child");
                if (old != null) foreach (GameObject o in old) o.Destroy();

                Catalog.CreateTree0(this, new Vector2(2, 9), new Vector2(0.1f), 0, "tree");
                Catalog.CreateTree1(this, new Vector2(5, 9), new Vector2(0.2f), 0, "tree");
                Catalog.CreateTree2(this, new Vector2(8, 9), new Vector2(0.2f), 0, "tree");
                Catalog.CreateTree3(this, new Vector2(11, 9), new Vector2(0.2f), 0, "tree");
                Catalog.CreateTree4(this, new Vector2(14, 9), new Vector2(0.2f), 0, "tree");
                Catalog.CreateTree5(this, new Vector2(2, 4), new Vector2(0.2f), 0, "tree");
                Catalog.CreateTree6(this, new Vector2(5, 4), new Vector2(0.2f), 0, "tree");
                Catalog.CreateTree7(this, new Vector2(8, 4), new Vector2(0.2f), 0, "tree");
                Catalog.CreateTree8(this, new Vector2(11, 4), new Vector2(0.2f), 0, "tree");
                Catalog.CreateTree9(this, new Vector2(14, 4), new Vector2(0.2f), 0, "tree");
            */
            }
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
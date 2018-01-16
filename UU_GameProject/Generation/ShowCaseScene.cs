using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Core;
//<author:cody>
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
            backg.Size = new Vector2(80, 30);
            backg.Pos = new Vector2(0, -backg.Size.Y);

            GameObject player = new GameObject(this, 10);
            player.AddComponent(new CFreeCamera());
            player.AddComponent(new CRender("player"));
            player.Size = new Vector2(0.5f, 1f);
            player.Pos = new Vector2(2, 2);
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
                GameObject[] old = objects.FindAllWithTag("_test");
                if(old != null) foreach (GameObject o in old) o.Destroy();
                old = objects.FindAllWithTag("_tree");
                if (old != null) foreach (GameObject o in old) o.Destroy();
                Catalog.CreateFromReplacer(this, new Vector2(4, 0), Catalog.ReplacerTree0);
                Catalog.CreateFromReplacer(this, new Vector2(10, 0), Catalog.ReplacerTree1);
                Catalog.CreateFromReplacer(this, new Vector2(18, 0), Catalog.ReplacerTree2);
                Catalog.CreateFromReplacer(this, new Vector2(24, 0), Catalog.ReplacerTree3);
                Catalog.CreateFromReplacer(this, new Vector2(32, 0), Catalog.ReplacerTree4);
                Catalog.CreateFromReplacer(this, new Vector2(38, 0), Catalog.ReplacerTree5);
                Catalog.CreateFromReplacer(this, new Vector2(48, 0), Catalog.ReplacerTree6);
                Catalog.CreateFromReplacer(this, new Vector2(60, 0), Catalog.ReplacerTree7);
                Catalog.CreateFromReplacer(this, new Vector2(68, 0), Catalog.ReplacerTree8);
                Catalog.CreateFromReplacer(this, new Vector2(74, 0), Catalog.ReplacerTree9);
            }
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
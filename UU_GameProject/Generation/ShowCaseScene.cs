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
            GameObject backg = new GameObject(this, 10, true);
            backg.AddComponent(new CRender("block"));
            backg.Pos = Vector2.Zero;
            backg.Size = new Vector2(16, 9);
            //ShowCase.CreateRow(this, "_stonesnowy", 16, 2, 1f);
            //ShowCase.CreateRow(this, "_stone", 16, 2, 1f);
            //ShowCase.CreateRow(this, "_frostydirt", 16, 2, 1f);
        }
        
        public override void Unload() { }
        
        public override void Update(float time)
        {
            base.Update(time);
            if (Input.GetKey(PressAction.DOWN, Keys.P)) Debug.showAtlas = true;
            else Debug.showAtlas = false;
            if(Input.GetKey(PressAction.PRESSED, Keys.Enter))
            {
                GameObject[] old = objects.FindAllWithTag("_test");
                if(old != null) foreach (GameObject o in old) o.Destroy();
                Catalog.CreateDirtGrassBlock(this, 0, 0, 0, "_test");
                Catalog.CreateFrostyDirt(this, 1, 0, 0, "_test");
                Catalog.CreateSnowyDirt(this, 2, 0, 0, "_test");

                Catalog.CreateSnowman(this, 0, 1, 0, "_test", 1f);
                Catalog.CreateStoneShard(this, 2, 1, 0, "_test");
                Catalog.CreateStone(this, 3, 1, 0, "_test");
                Catalog.CreateSnowyStone(this, 4, 1, 0, "_test");
                Catalog.CreateFrostyStone(this, 5, 1, 0, "_test");

                Catalog.CreateBush(this, 1, 2, 0, "_test");

                Catalog.CreateBoulder(this, 0, 4, 0, "_test");
                Catalog.CreateCloud(this, 2, 4, 0, "_test");
            }
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
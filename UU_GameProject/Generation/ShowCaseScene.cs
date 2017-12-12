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
                Catalog.CreateSnowman(this, 0, 0, 0, "_test", 1f);
                Catalog.CreateSnowman(this, 1, 0, 0, "_test", 1f);
                Catalog.CreateSnowman(this, 2, 0, 0, "_test", 1f);
                Catalog.CreateSnowman(this, 3, 0, 0, "_test", 1f);
                Catalog.CreateSnowman(this, 4, 0, 0, "_test", 1f);
                Catalog.CreateSnowman(this, 5, 0, 0, "_test", 1f);
                Catalog.CreateSnowman(this, 6, 0, 0, "_test", 1f);
                Catalog.CreateSnowman(this, 7, 0, 0, "_test", 1f);

                Catalog.CreateBush(this, 8, 0, 0, "_test");
                Catalog.CreateBush(this, 9, 0, 0, "_test");
                Catalog.CreateBush(this, 10, 0, 0, "_test");
                Catalog.CreateBush(this, 11, 0, 0, "_test");
                Catalog.CreateBush(this, 12, 0, 0, "_test");
                Catalog.CreateBush(this, 13, 0, 0, "_test");
                Catalog.CreateBush(this, 14, 0, 0, "_test");
                Catalog.CreateBush(this, 15, 0, 0, "_test");

                Catalog.CreateStoneShard(this, 8, 1, 0, "_test");
                Catalog.CreateStoneShard(this, 9, 1, 0, "_test");
                Catalog.CreateStoneShard(this, 10, 1, 0, "_test");
                Catalog.CreateStoneShard(this, 11, 1, 0, "_test");
                Catalog.CreateStoneShard(this, 12, 1, 0, "_test");
                Catalog.CreateStoneShard(this, 13, 1, 0, "_test");
                Catalog.CreateStoneShard(this, 14, 1, 0, "_test");
                Catalog.CreateStoneShard(this, 15, 1, 0, "_test");

                Catalog.CreateStone(this, 0, 2, 0, "_test");
                Catalog.CreateStone(this, 1, 2, 0, "_test");
                Catalog.CreateStone(this, 2, 2, 0, "_test");
                Catalog.CreateStone(this, 3, 2, 0, "_test");
                Catalog.CreateStone(this, 4, 2, 0, "_test");
                Catalog.CreateStone(this, 5, 2, 0, "_test");
                Catalog.CreateStone(this, 6, 2, 0, "_test");
                Catalog.CreateStone(this, 7, 2, 0, "_test");

                Catalog.CreateSnowyStone(this, 8, 2, 0, "_test");
                Catalog.CreateSnowyStone(this, 9, 2, 0, "_test");
                Catalog.CreateSnowyStone(this, 10, 2, 0, "_test");
                Catalog.CreateSnowyStone(this, 11, 2, 0, "_test");
                Catalog.CreateSnowyStone(this, 12, 2, 0, "_test");
                Catalog.CreateSnowyStone(this, 13, 2, 0, "_test");
                Catalog.CreateSnowyStone(this, 14, 2, 0, "_test");
                Catalog.CreateSnowyStone(this, 15, 2, 0, "_test");
            }
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
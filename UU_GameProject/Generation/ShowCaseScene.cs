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
            backg.Size = new Vector2(10, 10);
            ShowCase.CreateRow(this, "_stonesnowy", 16, 2, 1f);
            ShowCase.CreateRow(this, "_stone", 16, 2, 1f);
            ShowCase.CreateRow(this, "_dirtgrassblock", 8, 2, 1f);
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
                Catalog.CreateBush(this, 0, 0, 0, "_test");
                Catalog.CreateSnowman(this, 1, 1, 0, "_test");
            }
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
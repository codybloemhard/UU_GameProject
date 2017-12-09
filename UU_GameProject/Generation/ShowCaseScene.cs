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
            for(int i = 0; i < 4; i++)
            {
                GameObject go = new GameObject("", this, 2);
                go.AddComponent(new CRender("_boulder" + i));
                go.Pos = new Vector2(i * 4, 0);
                go.Size = new Vector2(4, 4);
            }
            for (int i = 0; i < 16; i++)
            {
                GameObject go = new GameObject("", this, 2);
                go.AddComponent(new CRender("_stone" + i));
                go.Pos = new Vector2(i * 1, 4);
                go.Size = new Vector2(1, 1);
            }
            for (int i = 0; i < 16; i++)
            {
                GameObject go = new GameObject("", this, 2);
                go.AddComponent(new CRender("_stoneshard" + i));
                go.Pos = new Vector2(i * 1, 5);
                go.Size = new Vector2(1, 1);
            }
            for (int i = 0; i < 16; i++)
            {
                GameObject go = new GameObject("", this, 2);
                go.AddComponent(new CRender("_bushleaf" + (i % 8)));
                go.Pos = new Vector2(i * 1, 6);
                go.Size = new Vector2(1, 1);
            }
            for (int i = 0; i < 32; i++)
            {
                GameObject go = new GameObject("", this, 2);
                go.AddComponent(new CRender("_berry" + (i % 8)));
                go.Pos = new Vector2(i * 0.5f, 7);
                go.Size = new Vector2(0.5f, 0.5f);
            }
            for (int i = 0; i < 4; i++)
            {
                GameObject go = new GameObject("", this, 1);
                go.AddComponent(new CRender("_cloud" + i));
                go.Pos = new Vector2(i * 4, 3);
                go.Size = new Vector2(4, 4);
            }
        }

        public override void Unload() { }

        public override void Update(float time)
        {
            base.Update(time);
            if (Input.GetKey(PressAction.DOWN, Keys.P)) Debug.showAtlas = true;
            else Debug.showAtlas = false;
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
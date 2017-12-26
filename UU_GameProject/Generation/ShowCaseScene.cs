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

        LSystem lsys;

        public override void Load(SpriteBatch batch)
        {
            GameStateManager.SetRenderingMode(BlendState.NonPremultiplied, SamplerState.PointWrap);
            Debug.ProfilingMode();
            GameObject backg = new GameObject(this, 10, true);
            backg.AddComponent(new CRender("block"));
            backg.Pos = Vector2.Zero;
            backg.Size = new Vector2(16, 9);
            //ShowCase.CreateRow(this, "_crackedlayer", 8, 2, 1f);
            //ShowCase.CreateRow(this, "_stone", 16, 2, 1f);
            //ShowCase.CreateRow(this, "_frostydirt", 16, 2, 1f);
            lsys = new LSystem("A");
            lsys.AddRule('A', "AB");
            lsys.AddRule('B', "A");
            GameObject p = new GameObject("p", this, 0);
            p.AddComponent(new CRender("block"));
            p.Pos = new Vector2(1,1);
            p.Size = new Vector2(0.2f, 0.2f);
            p.Renderer.colour = Color.Red;
            GameObject q = new GameObject("q", this, 0);
            q.AddComponent(new CRender("block"));
            q.Pos = new Vector2(3, 4);
            q.Size = new Vector2(0.2f, 0.2f);
            q.Renderer.colour = Color.Red;
            GameObject r = new GameObject("r", this, 1);
            r.AddComponent(new CRender("block"));
            r.Renderer.colour = Color.Blue;
            lsys.FromToTranslation(r, p.Pos, q.Pos);
        }
        
        public override void Unload() { }
        
        public override void Update(float time)
        {
            base.Update(time);
            if (Input.GetKey(PressAction.DOWN, Keys.P)) Debug.showAtlas = true;
            else Debug.showAtlas = false;
            GameObject p = objects.FindWithTag("p");
            GameObject q = objects.FindWithTag("q");
            q.Pos = Input.GetMousePosition();
            GameObject r = objects.FindWithTag("r");
            lsys.FromToTranslation(r, p.Pos, q.Pos);
            if(Input.GetKey(PressAction.PRESSED, Keys.Enter))
            {
                GameObject[] old = objects.FindAllWithTag("_test");
                if(old != null) foreach (GameObject o in old) o.Destroy();
                Catalog.CreateBlock(this, 0x0, 0, 0, "_test", BASETILES.DIRT, LAYERTILES.NONE, LAYERTILES.NONE);
                Catalog.CreateBlock(this, 0x1, 0, 0, "_test", BASETILES.DIRT, LAYERTILES.NONE, LAYERTILES.NONE, TOPTILES.GRASS);
                Catalog.CreateBlock(this, 0x2, 0, 0, "_test", BASETILES.DIRT, LAYERTILES.ICE, LAYERTILES.NONE);
                Catalog.CreateBlock(this, 0x3, 0, 0, "_test", BASETILES.DIRT, LAYERTILES.ICETOP, LAYERTILES.NONE);
                Catalog.CreateBlock(this, 0x4, 0, 0, "_test", BASETILES.DIRT, LAYERTILES.ICETOP, LAYERTILES.NONE, TOPTILES.SNOW);
                Catalog.CreateBlock(this, 0x5, 0, 0, "_test", BASETILES.DIRT, LAYERTILES.NONE, LAYERTILES.NONE, TOPTILES.SNOW);
                Catalog.CreateBlock(this, 0x6, 0, 0, "_test", BASETILES.STONE, LAYERTILES.CRACKS, LAYERTILES.NONE);
                Catalog.CreateBlock(this, 0x7, 0, 0, "_test", BASETILES.STONE, LAYERTILES.CRACKS, LAYERTILES.ICE);
                Catalog.CreateBlock(this, 0x8, 0, 0, "_test", BASETILES.STONE, LAYERTILES.CRACKS, LAYERTILES.ICETOP);
                Catalog.CreateBlock(this, 0x9, 0, 0, "_test", BASETILES.STONE, LAYERTILES.CRACKS, LAYERTILES.ICETOP, TOPTILES.SNOW);
                Catalog.CreateBlock(this, 0xA, 0, 0, "_test", BASETILES.STONE, LAYERTILES.CRACKS, LAYERTILES.ICE, TOPTILES.SNOW);
                Catalog.CreateBlock(this, 0xB, 0, 0, "_test", BASETILES.STONE, LAYERTILES.CRACKS, LAYERTILES.NONE, TOPTILES.SNOW);
                Catalog.CreateBlock(this, 0xC, 0, 0, "_test", BASETILES.STONE, LAYERTILES.CRACKS, LAYERTILES.CRACKS);
                Catalog.CreateBlock(this, 0xD, 0, 0, "_test", BASETILES.STONE, LAYERTILES.CRACKS, LAYERTILES.NONE, TOPTILES.GRASS);
                Catalog.CreateBlock(this, 0xE, 0, 0, "_test", BASETILES.SAND);
                Catalog.CreateBlock(this, 0xF, 0, 0, "_test", BASETILES.SANDSTONE, LAYERTILES.CRACKS);

                /*Catalog.CreateSnowman(this, 1, 1, 0, "_test", 1f);
                Catalog.CreateSnowman(this, 3, 1, 0, "_test", 1f);
                Catalog.CreateSnowman(this, 5, 1, 0, "_test", 1f);
                Catalog.CreateSnowman(this, 7, 1, 0, "_test", 1f);*/

                /*Catalog.CreateStoneShard(this, 3, 1, 0, "_test");
                Catalog.CreateStone(this, 4, 1, 0, "_test");
                Catalog.CreateSnowyStone(this, 5, 1, 0, "_test");
                Catalog.CreateFrostyStone(this, 6, 1, 0, "_test");*/

                Catalog.CreateCloud(this, 1, 3, 0, "_test");
                Catalog.CreateCloud(this, 3, 3, 0, "_test");
                Catalog.CreateCloud(this, 5, 3, 0, "_test");
                Catalog.CreateCloud(this, 7, 3, 0, "_test");
                //Catalog.CreateCloud(this, 2, 4, 0, "_test");
            }
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
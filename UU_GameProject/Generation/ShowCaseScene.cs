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
            //backg.Size = new Vector2(16, 9);
            //ShowCase.CreateRow(this, "_branch", 8, 2, 1f);
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
            }
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
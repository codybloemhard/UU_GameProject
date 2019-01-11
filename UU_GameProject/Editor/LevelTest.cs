using System;
using System.IO;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
//<author:cody>
namespace UU_GameProject
{
    public class LevelTest : GameState
    {
        private ChunkManager chunks;

        public LevelTest() : base() { }

        public override void Load(SpriteBatch batch)
        {
            GameObject player = new GameObject(this, 0);
            player.AddComponent(new CFreeCamera());
            player.AddComponent(new CRender("player"));
            player.Size = new Vector2(1f);
            player.Pos = new Vector2(2, 2);

            Vector2 chunkSize = new Vector2(16, 16);
            ChunkFactory builder = new ChunkFactory(this, chunkSize);
            builder.AddSource("solid", 10, true, 
                delegate(ReplacerInput i) {
                return Catalog.ReplacerBlock(i, BASETILES.STONE, LAYERTILES.CRACKS, LAYERTILES.ICE, TOPTILES.SNOW);
            });
            builder.AddSource("!tree", 10, true, Catalog.ReplacerTree0);
            string baseurl = "../../../../Content/Levels/";
            chunks = new ChunkManager();
            chunks.Discover(baseurl, builder, player);
            Debug.FullDebugMode();
        }
        
        private void dectest(GameObject o)
        {
            o.Size = new Vector2(1f);
            o.AddComponent(new CRender("_grassdot0"));
        }

        public override void Unload() { }

        public override void Update(float time)
        {
            base.Update(time);
            chunks.Update();
            //TaskEngine.UpdateAll();
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
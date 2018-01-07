using System;
using System.IO;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;

namespace UU_GameProject
{
    public class LevelTest : GameState
    {
        private ChunkManager chunks;

        public LevelTest() : base() { }

        public override void Load(SpriteBatch batch)
        {
            /*GameObject player = new GameObject("player", this, 1);
            CAnimatedSprite anim = new CAnimatedSprite();
            anim.AddAnimation("fallPanic", "playerFallPanic");
            anim.AddAnimation("walking", "playerWalking");
            anim.AddAnimation("crouching", "playerCrouching");
            anim.AddAnimation("crawling", "playerCrawling");
            anim.AddAnimation("sliding", "playerSliding");
            anim.AddAnimation("airborn", "playerAirborn");
            anim.AddAnimation("melee", "playerMelee");
            anim.PlayAnimation("walking", 5);
            player.AddComponent(anim);
            player.AddComponent(new CPlayerMovement(3.0f));
            player.AddComponent(new CAABB());
            player.AddComponent(new CShoot());
            player.AddComponent(new CMeleeAttack());
            player.AddComponent(new CHealthPool(100, player));
            player.AddComponent(new CManaPool(100, player));
            player.AddComponent(new Components.General.CMagicness());
            player.AddComponent(new Components.General.CFaction("friendly"));
            player.Pos = new Vector2(1, 1);
            player.Size = new Vector2(0.5f, 1.0f);*/

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
            builder.AddSource("!", 10, true, Catalog.ReplacerTree0);
            string baseurl = "../../../../Content/Levels/";
            chunks = new ChunkManager();
            chunks.Discover(baseurl, builder, player);
            //Debug.FullDebugMode();
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
            if (Input.GetKey(PressAction.PRESSED, Keys.Q))
            {
                Task.Factory.StartNew(test);
                Task.Factory.StartNew(test);
            }
        }

        private void test()
        {
            for (int i = 0; i < 100; i++)
            {
                GameObject a = new GameObject(this, 0);
                a.Pos = new Vector2((float)MathH.random.NextDouble() * 16, (float)MathH.random.NextDouble() * 16);
                a.Size = new Vector2(0.5f);
                a.AddComponent(new CRender("block"));
                a.AddComponent(new CAABB());
            }
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
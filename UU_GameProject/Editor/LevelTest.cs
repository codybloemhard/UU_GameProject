using System;
using System.IO;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    public class LevelTest : GameState
    {
        public LevelTest() : base() { }

        public override void Load(SpriteBatch batch)
        {
            GameObject player = new GameObject("player", this, 1);
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
            player.Size = new Vector2(0.5f, 1.0f);
            
            Vector2 chunkSize = new Vector2(32, 32);
            ChunkFactory builder = new ChunkFactory(this, chunkSize);
            builder.AddSource("solid", 10, true, SolidBuilder);
            Chunk chunk = LevelLogic.ReadChunk("test.lvl");
            LoadedChunk lc = builder.BuildChunk(chunk);

            ChunkManager chunks = new ChunkManager();
            chunks.SetFactory(builder);
            chunks.Discover(LevelLogic.baseurl);
        }
        
        public void SolidBuilder(GameObject o)
        {
            o.AddComponent(new CRender("_dirt0"));
            o.AddComponent(new CAABB());
        }

        public override void Unload()
        {

        }

        public override void Update(float time)
        {
            Camera.SetCameraTopLeft(new Vector2(0, 0));
            if (Input.GetKey(PressAction.PRESSED, Keys.P))
            {
                if (Debug.Mode == DEBUGMODE.PROFILING)
                    Debug.FullDebugMode();
                else Debug.ProfilingMode();
            }
            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }
    }
}
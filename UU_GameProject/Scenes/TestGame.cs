using System;
using System.IO;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UU_GameProject
{
    public class TestGame : GameState
    {
        public TestGame() : base() { }

        public override void Load(SpriteBatch batch)
        {
            //UI
            SpriteFont font = AssetManager.GetResource<SpriteFont>("mainFont");
            Text text = new Text(this, "Position: ", new Vector2(0f, 0f), new Vector2(16f, 1f), font);
            text.colour = new Color(0, 255, 0);
            text.tag = "positionText";
            Button button = new Button(this, "Menu!", "block", () => GameStateManager.RequestChange("menu", CHANGETYPE.LOAD),
                font, new Vector2(14, 0), new Vector2(2, 1));
            button.SetupColours(Color.Gray, Color.White, Color.DarkGray, Color.Red);
            //Objects
            GameObject stone0 = new GameObject("stone", this, 2, true);
            stone0.Pos = new Vector2(0, 8);
            stone0.Size = new Vector2(8, 1);
            stone0.AddComponent(new CRender("block"));
            stone0.AddComponent(new CAABB());
            GameObject stone1 = new GameObject("stone", this, 2, true);
            stone1.Pos = new Vector2(9, 7);
            stone1.Size = new Vector2(2, 2);
            stone1.AddComponent(new CRender("block"));
            stone1.AddComponent(new CAABB());
            GameObject stone2 = new GameObject("stone", this, 2, true);
            stone2.Pos = new Vector2(8, 5);
            stone2.Size = new Vector2(3, 0.2f);
            stone2.AddComponent(new CRender("block"));
            stone2.AddComponent(new CAABB());
            GameObject stone3 = new GameObject("stone", this, 2, true);
            stone3.Pos = new Vector2(12, 3);
            stone3.Size = new Vector2(3, 0.2f);
            stone3.AddComponent(new CRender("block"));
            stone3.AddComponent(new CAABB());
            GameObject stone4 = new GameObject("stone", this, 2, true);
            stone4.Pos = new Vector2(2, 4);
            stone4.Size = new Vector2(3, 0.2f);
            stone4.AddComponent(new CRender("block"));
            stone4.AddComponent(new CAABB());
            GameObject killer = new GameObject("killer", this, 2);
            killer.AddComponent(new CRender("suprise"));
            killer.AddComponent(new CAABB());
            killer.Pos = new Vector2(3, 5);
            killer.Size = new Vector2(1, 1);
            (killer.Renderer as CRender).colour = Color.Red;
            GameObject player = new GameObject("player", this, 1);
            CAnimatedSprite anim = new CAnimatedSprite();
            anim.AddAnimation("fallPanic", "playerFallPanic");
            anim.AddAnimation("walking", "playerWalking");
            anim.PlayAnimation("walking", 5);
            player.AddComponent(anim);
            player.AddComponent(new CPlayerMovement(3.0f));
            player.AddComponent(new CAABB());
            player.AddComponent(new CShoot());
            player.AddComponent(new CMeleeAttack());
            player.AddComponent(new CHealthPool(100, player));
            player.AddComponent(new CManaPool(100, player));
            player.Pos = new Vector2(1, 1);
            player.Size = new Vector2(0.5f, 1.0f);

            GameObject enemy = new GameObject("Nenemy", this, 2);
            enemy.AddComponent(new CRender("player"));
            enemy.AddComponent(new CNormalEnemyAI(2f));
            enemy.AddComponent(new CHealthPool(50, enemy));
            enemy.AddComponent(new CAABB());
            enemy.AddComponent(new CMeleeAttack());
            enemy.Pos = new Vector2(12.5f, 1.99f);
            enemy.Size = new Vector2(0.5f, 1.0f);
            GameObject enemy1 = new GameObject("Renemy", this, 2);
            enemy1.AddComponent(new CRender("player"));
            enemy1.AddComponent(new CRangedEnemyAI(2.5f));
            enemy1.AddComponent(new CHealthPool(25, enemy1));
            enemy1.AddComponent(new CAABB());
            enemy1.AddComponent(new CShoot());
            enemy1.Pos = new Vector2(9.5f, 4.0f);
            enemy1.Size = new Vector2(0.5f, 1.0f);
            GameObject enemy2 = new GameObject("Aenemy", this, 2);
            enemy2.AddComponent(new CRender("player"));
            enemy2.AddComponent(new CArmouredEnemyAI(1.75f));
            enemy2.AddComponent(new CHealthPool(100, enemy2));
            enemy2.AddComponent(new CAABB());
            enemy2.AddComponent(new CMeleeAttack());
            enemy2.Pos = new Vector2(2.5f, 3.0f);
            enemy2.Size = new Vector2(0.5f, 1.0f);
        }

        public override void Unload()
        {
            
        }

        public override void Update(float time)
        {
            Camera.SetCameraTopLeft(new Vector2(0, 0));
            Text text = ui.FindWithTag("positionText") as Text;
            GameObject player = objects.FindWithTag("player");
            text.text = "Position: " + MathH.Float(player.Pos.X, 2) + " , " + MathH.Float(player.Pos.Y, 2);
            if (Input.GetKey(PressAction.PRESSED, Keys.P))
            {
                if (Debug.Mode == DEBUGMODE.PROFILING)
                    Debug.FullDebugMode();
                else Debug.ProfilingMode();
            }
            if(Input.GetKey(PressAction.PRESSED, Keys.X))
            {
                
            }
            base.Update(time);
        }

        public override void Draw(float time, SpriteBatch batch, GraphicsDevice device)
        {
            base.Draw(time, batch, device);
        }

        public void LoadFromFile(string path)
        {
            using (StreamReader fileReader = new StreamReader(path))
            {
                string line = fileReader.ReadLine();
                while (line != null)
                {
                    string[] properties = line.Split('|');
                    for(int i = 0; i < properties.Length; i++)
                    {
                        
                    }
                }
            }

        }
    }
}
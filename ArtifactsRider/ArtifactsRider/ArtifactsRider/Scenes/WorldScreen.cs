using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VAPI;
using ArtifactsRider.MapManager;
using VAPI.RenderEffects;
using VAPI.RenderEffects.SceneSwitchEffects;
using FarseerPhysics.DebugViews;
using ArtifactsRider.Items;
using System.Threading;


namespace ArtifactsRider.Scenes
{
    public class WorldScreen : GameScreen
    {
        Map Map;
        int mapSize = 64;
        int tileSize = 64;
        float ambient = 0.01f;

        public LightingEngine LE;
        public BlurSwitchEffect SE;
        public SpotLight PlayerLight;
        public SpotLight TmpLight;

        public static bool debug = false;

        public Window DebugWindow = new Window(new Rectangle(100, 100, 400, 800));
        public CheckBox FarseerDebugCheck = new CheckBox(new Rectangle(10, 10, 16, 16), "GUI/checkboxOn", "GUI/checkboxOff", false);

        DebugViewXNA Debug;

        public WorldScreen(Game Game)
            : base(Game, GeneralManager.ScreenX, GeneralManager.ScreenY)
        {
            GeneralManager.LoadAnimation("TestAnim", Vector2.One * 64, 6, 80);
            GeneralManager.LoadAnimation("PlayerIdle", Vector2.One * 64, 1, 10000);

            DebugWindow.Visible = false;
            DebugWindow.AddGUI(FarseerDebugCheck, DebugWindow);
            AddGUI(DebugWindow);

            LE = new LightingEngine();
            Color a = new Color(ambient, ambient, ambient);
            LE.SetAmbient(a, a);
            //LE.SetAmbient(Color.Gray, Color.Gray);
            Renderer.AddRendererEffect(LE, "LightingEngine");

            LoadContent();

            Map = new Map(this, tileSize, mapSize);
            //Renderer.AddPostProcess(new BlurEffect(), "Blur");
            SE = new BlurSwitchEffect();
            SE.MaxTime = 1f;
            SE.TurnOffAction = delegate () {Renderer.RemovePostProcess("Switch");};
            Renderer.AddPostProcess(SE, "Switch");

            Camera.Init(GeneralManager.GetPartialRect(0, 0, 1, 1), new Rectangle(0, 0, tileSize * mapSize, tileSize * mapSize));

            PlayerLight = new SpotLight()
            {
                IsEnabled = true,
                Color = new Vector4(0.9f, .7f, .7f, 1f),
                Power = .6f,
                LightDecay = 600,
                Position = new Vector3(500, 400, 20),
                SpotAngle = 1.5f,
                SpotDecayExponent = 3,
                Direction = new Vector3(0.244402379f, 0.969673932f, 0)
            };
            LE.AddLight(PlayerLight);

            Debug = new FarseerPhysics.DebugViews.DebugViewXNA(Map.PhysicalWorld);
            Debug.AppendFlags(FarseerPhysics.DebugViewFlags.Shape);
            Debug.AppendFlags(FarseerPhysics.DebugViewFlags.AABB);
            Debug.AppendFlags(FarseerPhysics.DebugViewFlags.PerformanceGraph);
            Debug.AppendFlags(FarseerPhysics.DebugViewFlags.Joint);
            Debug.AppendFlags(FarseerPhysics.DebugViewFlags.ContactPoints);
            Debug.DefaultShapeColor = Color.White;
            Debug.SleepingShapeColor = Color.LightGray;
            Debug.LoadContent(Parent.GraphicsDevice, Parent.Content);

            //Weapon w = new Weapon("test", "Gun", 100, 10, Hands.One, 10, 600, 10, 10, 10);
            //Serializer.Serialize<Weapon>("weapon.xml", w);
        }

        public override void Draw(GameTime GameTime)
        {
            Map.Draw(GameTime);

            base.Draw(GameTime);
        }

        public override void Update(GameTime GameTime)
        {
            if (GeneralManager.CheckKeyEdge(Keys.P))
            {
                debug = !debug;
                DebugWindow.Visible = !DebugWindow.Visible;
            }
            Map.Update(GameTime);
            PlayerLight.Position = new Vector3(Map.Player.GetPosition(), 0);
            PlayerLight.Direction = new Vector3(GeneralManager.MousePos + new Vector2(Camera.GetRect.X, Camera.GetRect.Y), 0) - PlayerLight.Position + new Vector3(0, 0, 10);
            PlayerLight.Direction.Normalize();

            base.Update(GameTime);
        }

        public override bool HandleInput()
        {
            bool Result = false;

            if (base.HandleInput())
            {
                Result = true;
            }
            else if (Map.HandleInput())
            {
                Result = true;
            }

            return Result;
        }

        public override void EndDraw(GameTime GameTime)
        {
            if (debug)
            {
                if (FarseerDebugCheck.State)
                {
                    Matrix ViewMatrix = Matrix.CreateOrthographic(GeneralManager.ScreenX, -GeneralManager.ScreenY, 0, 100) * Matrix.CreateTranslation(new Vector3(-1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-2f * (float)Camera.GetRect.X / GeneralManager.ScreenX, 2f * (float)Camera.GetRect.Y / GeneralManager.ScreenY, 0));

                    Debug.RenderDebugData(ref ViewMatrix);
                }
                base.EndDraw(GameTime);


                Renderer.SpriteBatch.Begin();

                Renderer.SpriteBatch.DrawString(GeneralManager.Fonts["Fonts/28DaysLater"], "FPS : " + GeneralManager.FPS.ToString(), new Vector2(25, 25), Color.White);

                Renderer.SpriteBatch.End();
            }

        }
        

        private void LoadContent()
        {
            SoundEngine.LoadSound("zombie_walk.wav", "Content/Sounds/Mobs/zombie_walk.wav");
            SoundEngine.LoadSound("pistol_shoot.wav", "Content/Sounds/Mobs/PistolShoot.wav");
            SoundEngine.LoadSound("door_open.mp3", "Content/Sounds/Entities/door.mp3");
            SoundEngine.LoadSound("door_close.mp3", "Content/Sounds/Entities/door 1.mp3");
        }

        private void FreeContent()
        {

        }
   
    }
}

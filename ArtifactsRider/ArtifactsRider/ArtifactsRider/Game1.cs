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
using ArtifactsRider.Scenes;
using System.IO;
using Newtonsoft.Json;
using ArtifactsRider.MapManager;
using ArtifactsRider.Mobs;
using System.Diagnostics;


namespace ArtifactsRider
{
    /// Main class for our game.
    public class Instance : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics; /**< Graphics Device Manager - we need it in VAPI and to change resolution at startup */

        int _total_frames = 0;      /**< Total frames in second */
        float _elapsed_time = 0.0f; /**< Elapsed time after last update loop */

        ///Constructor
        public Instance()
        {
            graphics = new GraphicsDeviceManager(this);     //We're taking main Graphics Device Manager
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;       //Setting width of Resolution
            graphics.PreferredBackBufferHeight = 786;       //Setting height of Resolution

            IsMouseVisible = true;                          //Showing mouse cursor
            graphics.SynchronizeWithVerticalRetrace = true; //Enabling V-Sync
            graphics.ApplyChanges();                        //Applying changes to graphics device manager
        }

        /// Initialize method. We're using it to init VAPI and Renderer, also to load Fonts and Textures
        protected override void Initialize()
        {
            GeneralManager.Initalize(Content, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, this);
            Renderer.Init(graphics.GraphicsDevice);

            LoadTextures();
            LoadFonts();
       
            GeneralManager.CurrentScreen = new MainMenuScreen(this);            //Setting first scene to load and activating it

            base.Initialize();
        }
       
        /// Method for loading all textures in Content folder
        private static void LoadTextures()
        {
            DirectoryInfo dir = new DirectoryInfo(GeneralManager.Content.RootDirectory + "/Textures");
            Logger.Write("Root : " + dir.Name);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            List<DirectoryInfo> dirs = dir.GetDirectories().ToList();

            bool NothingAdded = false;

            while (!NothingAdded)
            {
                List<DirectoryInfo> NewDirs = new List<DirectoryInfo>();

                foreach (DirectoryInfo DInf in dirs)
                {
                    foreach (DirectoryInfo D in DInf.GetDirectories())
                    {
                        bool WasAdded = false;
                        foreach (DirectoryInfo Check in dirs)
                        {
                            if (Check.FullName ==  D.FullName)
                            {
                                WasAdded = true;
                                break;
                            }
                        }

                        if(!WasAdded)
                            NewDirs.Add(D);
                    }
                }

                NothingAdded = NewDirs.Count == 0;

                dirs.AddRange(NewDirs);
            }

            foreach (DirectoryInfo d in dirs)
            {
                FileInfo[] files = d.GetFiles("*.*");

                int Start = Directory.GetCurrentDirectory().Length + 9;

                foreach (FileInfo file in files)
                {
                    string key = d.FullName.Substring(Start) + "/" + file.Name.Split(new char[] { '.' })[0];
                    Logger.Write("Loading : " + key);

                    GeneralManager.LoadTex(key);
                }
            }
        }

        private static void LoadFonts()     /**< Method for loading Fonts */
        {
            GeneralManager.LoadFont("Fonts/28DaysLater");
            GeneralManager.LoadFont("Fonts/PR_Viking");
        }

        /// <summary>
        /// Main Update loop
        /// </summary>
        /// <param name="gameTime">Elasped time after last Update loop</param>
        protected override void Update(GameTime gameTime)
        {
             _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                GeneralManager.FPS = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }

            this.Window.Title = "Artifacts Rider   FPS : " + GeneralManager.FPS.ToString();

            GeneralManager.Update(gameTime);
            GeneralManager.CurrentScreen.HandleInput();
            GeneralManager.CurrentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Main Draw loop
        /// </summary>
        /// <param name="gameTime">Elasped time after last Update loop</param>
        protected override void Draw(GameTime gameTime)
        {
            _total_frames++;

            Renderer.BeginDraw();

            GeneralManager.CurrentScreen.Draw(gameTime);    /// Drawing current screen

            Renderer.FinishDraw();

            GeneralManager.CurrentScreen.EndDraw(gameTime); /// Drawing effects and overlays

            base.Draw(gameTime);
        }
    }
}

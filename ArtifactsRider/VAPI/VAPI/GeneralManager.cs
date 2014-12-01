using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Resources;
using System.Reflection;
namespace VAPI
{
    /// \brief General game manager
    /// 
    /// Place for utilities, loaded content, etc.
    public class GeneralManager
    {
        public static GameScreen CurrentScreen;         /**< Active screen */
        public static Dictionary<string, GameScreen> GameScreens = new Dictionary<string, GameScreen>();    /**< Dictionary of screens */
        static KeyboardState CurrentKeyboardState;      /**< Current state of Keyboard */
        static KeyboardState OldKeyboardState;          /**< Old state of Keyboard */
        public static MouseState MouseState;            /**< Current state of mouse */
        public static MouseState OldMouseState;         /**< Old state of mouse */
        public static ContentManager Content;           /**< Reference to main Content Manager */
        public static Vector2 MousePos;                 /**< Mouse position */

        public static Dictionary<string, Texture2D> Textures;   /**< Dictionary of loaded textures */
        public static Dictionary<string, SpriteFont> Fonts;     /**< Dictionary of loaded fonts */
        public static Dictionary<string, Effect> Effects;       /**< Dictionary of loaded effects */
        public static Dictionary<string, Animation> Animations; /**< Dictionary of loaded animations */

        public static Game Game;                        /**< Reference to main Instance */

        public static int ScreenX;                      /**< Width of screen */
        public static int ScreenY;                      /**< Heihgt of screen */

        public static float FPS;                        /**< Actual frames per second */

        /// <summary>
        /// Method to Initialize GeneralManager
        /// </summary>
        /// <param name="_Content">Reference to Content Manager</param>
        /// <param name="_ScreenX">Width of screen</param>
        /// <param name="_ScreenY">Height of Screen</param>
        /// <param name="_Game">Reference to main Instance</param>
        public static void Initalize(ContentManager _Content, int _ScreenX, int _ScreenY, Game _Game)
        {
            Content = _Content;
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
            Effects = new Dictionary<string, Effect>();
            Animations = new Dictionary<string, Animation>();

            ScreenX = _ScreenX;
            ScreenY = _ScreenY;

            Game = _Game;
            //GDevice = _Game.GraphicsDevice;

            Logger.Init();
        }

        /// <summary>
        /// Adding texture to Textures dictionary
        /// </summary>
        /// <param name="Name">Name of texture to load</param>
        public static void LoadTex(string Name)
        {
            var n = Name.Split(new char[]{'/','\\'});
            string str ="";
            for (int i = 1; i < n.Count()-1; i++)
            {
                str += n[i] + '/';
            }
            str += n[n.Count()-1];

            Textures.Add(str, Content.Load<Texture2D>(Name));
            Logger.Write("Loaded " + Name + " Texture");
        }

        /// <summary>
        /// Adding font to Fonts dictionary
        /// </summary>
        /// <param name="Name">Name of Font to load</param>
        public static void LoadFont(string Name)
        {
            SpriteFont Font = Content.Load<SpriteFont>(Name);
            Fonts.Add(Name, Font);

            Logger.Write("Loaded " + Name + " Font");
        }

        /// <summary>
        /// Addinf effect to Effects dictionary
        /// </summary>
        /// <param name="Name">Name of Effect to load</param>
        public static void LoadEffect(string Name)
        {
            Effects.Add(Name, Content.Load<Effect>(Name));

            Logger.Write("Loaded " + Name + " Effect");
            
        }

        /// <summary>
        /// Adding Animation to Animations dictionary
        /// </summary>
        /// <param name="Name">Name of Animation to load</param>
        /// <param name="FrameSize">Width and Height of single Frame</param>
        /// <param name="MaxFrames">Max amount of frames in Animation</param>
        /// <param name="MsPerFrame">Milliseconds between Animation frames</param>
        public static void LoadAnimation(string Name, Vector2 FrameSize, int MaxFrames, int MsPerFrame)
        {
            Animation Anim = new Animation();
            Anim.Load(Name, FrameSize, MaxFrames, MsPerFrame);
            Animations.Add(Name, Anim);
        }

        /// <summary>
        /// Method to check, if Key was pressed and then released
        /// </summary>
        /// <param name="Key">Key to check</param>
        /// <returns>True, if Key was pressed and then released. False otherwise</returns>
        public static bool CheckKeyEdge(Keys Key)
        {
            return CurrentKeyboardState.IsKeyDown(Key) && ! OldKeyboardState.IsKeyDown(Key);
        }

        /// <summary>
        /// Method to check, if Key is pressed
        /// </summary>
        /// <param name="Key">Key to check</param>
        /// <returns>True, if Key is pressed. False otherwise</returns>
        public static bool CheckKey(Keys Key)
        {
            return CurrentKeyboardState.IsKeyDown(Key);
        }

        /// <summary>
        /// Updating General Manager
        /// </summary>
        /// <param name="GameTime">Elapsed time since last Update loop.</param>
        public static void Update(GameTime GameTime)
        {
            Renderer.Update(GameTime);
            UpdateKeyboard();
            UpdateMouse();
        }

        /// <summary>
        /// Updating Keyboard state
        /// </summary>
        static void UpdateKeyboard()
        {
            OldKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Updating Mouse state
        /// </summary>
        static void UpdateMouse()
        {
            OldMouseState = MouseState;
            MouseState = Mouse.GetState();

            MousePos = new Vector2(MouseState.X, MouseState.Y);
        }

        /// <summary>
        /// Checking, if Left Mouse Button is pressed
        /// </summary>
        /// <returns>True, if LMB is pressed. False otherwise</returns>
        public static bool IsLMBClicked()
        {
            return MouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Checking, if Right Mouse Button is pressed
        /// </summary>
        /// <returns>True, if RMB is pressed. False otherwise</returns>
        public static bool IsRMBClicked()
        {
            return MouseState.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Checking, if Left Mouse Button was pressed and then released
        /// </summary>
        /// <returns>True, if LMB was pressed and then released. False otherwise</returns>
        public static bool IsLMBClickedEdge()
        {
            return MouseState.LeftButton == ButtonState.Pressed && !(OldMouseState.LeftButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Checking, if Right Mouse Button was pressed and then released
        /// </summary>
        /// <returns>True, if RMB was pressed and then released. False otherwise</returns>
        public static bool IsRMBClickedEdge()
        {
            return MouseState.RightButton == ButtonState.Pressed && !(OldMouseState.RightButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Getting percentage of Screen Width
        /// </summary>
        /// <param name="Part">Percentage of Screen Width. Range 0.0 - 1.0</param>
        /// <returns>Percentage of Screen Width</returns>
        static public int GetPartialWidth(float Part)
        {
            return (int)(ScreenX * Part);
        }

        /// <summary>
        /// Getting percentage of Screen Height
        /// </summary>
        /// <param name="Part">Percentage of Screen Height. Range 0.0 - 1.0</param>
        /// <returns>Percentage of Screen Height</returns>
        static public int GetPartialHeight(float Part)
        {
            return (int)(ScreenX * Part);
        }

        /// <summary>
        /// Getting percentage of Screen resolution
        /// </summary>
        /// <param name="PartX">Percentage of Screen Width. Range 0.0 - 1.0</param>
        /// <param name="PartY">Percentage of Screen Heigth. Range 0.0 - 1.0</param>
        /// <returns>Vector with percentages</returns>
        static public Vector2 GetPartialVector(float PartX, float PartY)
        {
            return new Vector2(ScreenX * PartX, ScreenY * PartY);
        }


        static public Rectangle GetPartialRect(float PartX, float PartY, float PartWidth, float PartHeight)
        {
            return new Rectangle((int)(ScreenX * PartX), (int)(ScreenY * PartY), (int)(ScreenX * PartWidth), (int)(ScreenY * PartHeight));
        }

        /// <summary>
        /// Getting half of Screen Width
        /// </summary>
        public static int HalfWidth
        {
            get
            {
                return (int)ScreenX / 2;
            }
        }

        /// <summary>
        /// Getting half of Screen Height
        /// </summary>
        public static int HalfHeight
        {
            get
            {
                return (int)ScreenY / 2;
            }
        }
    }
}

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
using VAPI.RenderEffects.SceneSwitchEffects;

namespace VAPI
{
    /// <summary>
    /// Base class for GameScreen
    /// </summary>
    public class GameScreen
    {
        protected Game Parent;          /**< Reference to main Instance */

        public LinkedList<GUIComponent> GUIComponents;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Game">Reference to main Instance</param>
        /// <param name="SizeX">Width of screen</param>
        /// <param name="SizeY">Height of screen</param>
        public GameScreen(Game Game, int SizeX, int SizeY)
        {
            GUIComponents = new LinkedList<GUIComponent>();
            this.Parent = Game;
        }

        /// <summary>
        /// Base method for handling Input of GUI
        /// </summary>
        /// <returns>True, if something was clicked</returns>
        public virtual bool HandleInput()
        {
            foreach (GUIComponent G in GUIComponents)
            {
                if (G.HandleInput())
                {
                    return true;
                }
            }
            return false; 
        }

        /// <summary>
        /// Base method for Update loop
        /// </summary>
        /// <param name="GameTime">Time that passed since last Update loop</param>
        public virtual void Update(GameTime GameTime)
        {
            foreach (GUIComponent G in GUIComponents)
            {
                G.Update(GameTime);
                G.IsActive = false;
            }

            foreach (GUIComponent G in GUIComponents)
            {
                if (G.CheckActive())
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Base method for Drawing
        /// </summary>
        /// <param name="GameTime">Time that passed since last Update loop</param>
        public virtual void Draw(GameTime GameTime)
        {
            Parent.GraphicsDevice.Clear(Color.Black);
            int i = GUIComponents.Count -1;
            List<GUIComponent> GC = GUIComponents.ToList<GUIComponent>();

            foreach (GUIComponent G in GUIComponents)
            {
                GC[i].Draw();
                
                i--;
            }
        }

        /// <summary>
        /// Base method for EndDraw
        /// </summary>
        /// <param name="GameTime">Time that passed since last Update loop</param>
        public virtual void EndDraw(GameTime GameTime)
        {
            // must draw only on spritebatch, do not use renderer!!! (wolololo)
        }

        /// <summary>
        /// Base method for adding GUI element
        /// </summary>
        /// <param name="GUI">Component, that we want to add.</param>
        public void AddGUI(GUIComponent GUI)
        {
            this.GUIComponents.AddFirst(GUI);
        }

        /// <summary>
        /// Base method for removing GUI element
        /// </summary>
        /// <param name="GUI">Component, that we want to remove</param>
        public void RemoveGUI(GUIComponent GUI)
        {
            GUIComponents.Remove(GUI);
        }

        /// <summary>
        /// Base method for switching to scene
        /// </summary>
        /// <param name="Screen">GameScreen that we want to switch to.</param>
        public void SwitchTo(GameScreen Screen)
        {
            
            GeneralManager.CurrentScreen = Screen;
        }       
    }
}

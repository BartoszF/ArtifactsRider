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

namespace VAPI
{
    /// <summary>
    /// Base GUIComponent class
    /// </summary>
    public abstract class GUIComponent
    {
        public bool IsActive = false;       /**< Is it active ? */
        public Rectangle Position;          /**< Postion */

        public abstract bool HandleInput(); /**< Method for Handling Input */

        public abstract void Draw();        /**< Main draw loop */

        public abstract void Update(GameTime gameTime);     /**< Main Update loop */

        public abstract bool CheckActive(); /**< Check if active */
    }   
}

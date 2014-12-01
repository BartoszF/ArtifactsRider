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
    public class Label : GUIComponent
    {
        public SpriteFont Font;

        public string Name;

        public Label()
        {
            // TODO: Construct any child components here
        }


        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            Renderer.PostDrawFont(GeneralManager.Fonts["Fonts/TestFont"], Position, Name);
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool HandleInput()
        {
            return false;
        }
    }
}

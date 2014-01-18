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
namespace ArtifactsRider
{
    class GameWorld
    {
        GameScreen ParentScreen;

        public GameWorld(GameScreen _ParentScreen)
        {
            this.ParentScreen = _ParentScreen;
        }

        public void Update(GameTime GameTime)
        {
        }

        public void Draw(SpriteBatch SpriteBatch)
        {
        }

        public void PollInput()
        {
        }
    }
}

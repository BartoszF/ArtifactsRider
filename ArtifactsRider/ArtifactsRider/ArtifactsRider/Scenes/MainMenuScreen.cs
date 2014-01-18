﻿using System;
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

namespace ArtifactsRider.Scenes
{
    class MainMenuScreen : GameScreen
    {
        public MainMenuScreen(Game Game)
            : base(Game, GeneralManager.ScreenX, GeneralManager.ScreenY)
        {
        }
    }
}

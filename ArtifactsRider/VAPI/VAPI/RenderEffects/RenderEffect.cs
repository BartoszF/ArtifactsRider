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
    public abstract class RenderEffect
    {
        public abstract void BeginEffect();

        public abstract void EndEffect();

        public abstract void UpdateEffect(GameTime GameTime);

        public abstract void Dispose();
    }
}

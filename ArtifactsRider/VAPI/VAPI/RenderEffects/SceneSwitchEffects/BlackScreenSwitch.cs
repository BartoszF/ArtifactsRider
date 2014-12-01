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

namespace VAPI.RenderEffects.SceneSwitchEffects
{
    public class BlackScreenSwitch : SceneSwitchEffect
    {
        Texture2D BlackTex;

        public BlackScreenSwitch()
            : base()
        {
            BlackTex = new Texture2D(Renderer.GD, 1, 1);
            BlackTex.SetData<Color>(new Color[] { Color.Black });
            this._MaxTime = 0.5f;
            this._Time = 0.5f;
        }

        public override void BeginEffect()
        {
        }

        public override void Dispose()
        {
        }

        public override void EndEffect()
        {
            Renderer.SpriteBatch.Begin();

            if(SwitchState == State.SwitchOff)
                Renderer.SpriteBatch.Draw(BlackTex, GeneralManager.GetPartialRect(0, 0, 1, 1), new Color(1f, 1f, 1f, 1f - Time/MaxTime));
            else
                Renderer.SpriteBatch.Draw(BlackTex, GeneralManager.GetPartialRect(0, 0, 1, 1), new Color(1f, 1f, 1f, Time / MaxTime));
            Renderer.SpriteBatch.End();
        }

        public override void UpdateEffect(GameTime GameTime)
        {
            
            base.UpdateEffect(GameTime);
        }
    }
}

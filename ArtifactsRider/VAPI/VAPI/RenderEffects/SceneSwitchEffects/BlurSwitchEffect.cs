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
    public class BlurSwitchEffect : SceneSwitchEffect
    {
        Effect BlurEffect;
        EffectParameter Value;

        public BlurSwitchEffect()
            : base()
        {
            if (!GeneralManager.Effects.ContainsKey("Effects/Blur"))
            {
                GeneralManager.LoadEffect("Effects/Blur");
            }

            BlurEffect = GeneralManager.Effects["Effects/Blur"];
            Value = BlurEffect.Parameters["Param"];
            this._MaxTime = 0.5f;
            this._Time = 0.5f;
        }

        public override void BeginEffect()
        {
            BlurEffect.Techniques[0].Passes[0].Apply();
        }

        public override void Dispose()
        {
        }

        public override void EndEffect()
        {

        }

        public override void UpdateEffect(GameTime GameTime)
        {
            Value.SetValue (Time / MaxTime);
            base.UpdateEffect(GameTime);
        }
    }
}

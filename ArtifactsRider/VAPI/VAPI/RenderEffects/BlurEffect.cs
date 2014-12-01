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

namespace VAPI.RenderEffects
{
    public class BlurEffect : RenderEffect
    {
        Effect BloomEffect;
        EffectParameter Value;
        public BlurEffect()
        {
            if (!GeneralManager.Effects.ContainsKey("Effects/Blur"))
            {
                GeneralManager.LoadEffect("Effects/Blur");
            }

            BloomEffect = GeneralManager.Effects["Effects/Blur"];
            Value = BloomEffect.Parameters["Param"];
            Value.SetValue(0.05f);
        }

        public override void BeginEffect()
        {
            for (int i = 0; i < BloomEffect.CurrentTechnique.Passes.Count; i++)
            {
                BloomEffect.CurrentTechnique.Passes[i].Apply();

            }
        }

        public override void EndEffect()
        {
        }

        public override void UpdateEffect(GameTime GameTime)
        {
        }

        public override void Dispose()
        {
        }
    }
}

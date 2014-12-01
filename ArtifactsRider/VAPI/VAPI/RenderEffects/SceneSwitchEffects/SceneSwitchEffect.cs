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
    public class SceneSwitchEffect : RenderEffect
    {
        protected float _Time;
        protected float _MaxTime;
        public enum State
        {
            SwitchOn,
            SwitchOff,
            None
        }


        public State SwitchState;


        public delegate void SwitchAction();

        public SwitchAction TurnOffAction;


        public override void BeginEffect()
        {
        }

        public override void Dispose()
        {
        }

        public override void EndEffect()
        {
        }

        public override void UpdateEffect(GameTime GameTime)
        {

            if (this.SwitchState == State.SwitchOn)
            {
                if (_Time > 0)
                {
                    _Time -= 0.01f;
                }
                else
                {
                    SwitchState = SceneSwitchEffect.State.None;
                }
            }
            else if (this.SwitchState == State.SwitchOff)
            {
                if (_Time > 0)
                {
                    _Time -= 0.01f;
                }
                else
                {
                    SwitchState = SceneSwitchEffect.State.None;
                    TurnOffAction();
                    _Time = 0f;
                }
            }
            else
            {
            }

            Time = MathHelper.Clamp(Time, 0, MaxTime);
        }

        public float Time
        {
            get
            {
                return _Time;
            }
            set
            {
                if (value > MaxTime)
                {
                    _Time = _MaxTime;
                }
                else
                {
                    _Time = value;
                }
            }
        }

        public float MaxTime
        {
            get
            {
                return _MaxTime;
            }
            set
            {
                _MaxTime = value;
                _Time = value;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAPI;
using Microsoft.Xna.Framework;

using VAPI.RenderEffects.SceneSwitchEffects;
using IrrKlang;

namespace ArtifactsRider.Scenes
{
    public class IntroScreen : GameScreen
    {
        BlackScreenSwitch BSS;
        ISound m;
        float timer;

        public IntroScreen(Game Game)
            : base(Game, GeneralManager.ScreenX, GeneralManager.ScreenY)
        {
            BSS = new BlackScreenSwitch();
            BSS.SwitchState = SceneSwitchEffect.State.SwitchOn;
            BSS.MaxTime = 1f;
            m = SoundEngine.PlaySound(Vector2.Zero,"Content/Sounds/Logos jingle.mp3");

            Renderer.AddPostProcess(BSS, "Switch");
        }

        void SwitchOffEnd()
        {
            Renderer.RemovePostProcess("Switch");
            m.Stop();
            this.SwitchTo(new MainMenuScreen(this.Parent));
        }

        public override void Update(GameTime GameTime)
        {
            if (timer <= 4000f)
            {
                timer += GameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                if (BSS.SwitchState != SceneSwitchEffect.State.SwitchOff)
                {
                    BSS.SwitchState = SceneSwitchEffect.State.SwitchOff;
                    BSS.MaxTime = 1.5f;
                    BSS.TurnOffAction = SwitchOffEnd;
                }
            }
            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            Renderer.Draw(GeneralManager.Textures["GUI/Logos"], new Rectangle(0, 0, GeneralManager.ScreenX, GeneralManager.ScreenY));
            base.Draw(GameTime);
        }
    }
}

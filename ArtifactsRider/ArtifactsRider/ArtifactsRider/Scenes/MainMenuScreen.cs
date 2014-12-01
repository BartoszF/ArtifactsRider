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
using VAPI.RenderEffects;
using VAPI.RenderEffects.Particle;
using VAPI.RenderEffects.SceneSwitchEffects;
using IrrKlang;

namespace ArtifactsRider.Scenes
{
    class MainMenuScreen : GameScreen
    {
        Button NewGameButton;
        Button LoadGameButton;
        Button OptionsButton;
        Button CreditsButton;
        Button ExitButton;

        BlackScreenSwitch BSS;

        float CloudsOffset;
        float FogOffset;

        ISound m;

        float MouseOffset;

        public MainMenuScreen(Game Game)
            : base(Game, GeneralManager.ScreenX, GeneralManager.ScreenY)
        {
            NewGameButton = new Button(GeneralManager.GetPartialRect(0,0.4f, 0.2f, 0.1f), " New Game ", GeneralManager.Textures["GUI/MainMenuGraphics/Button"], Color.Gray, Color.White, GeneralManager.Fonts["Fonts/28DaysLater"]);
            NewGameButton.Action = NewGameButtonClick;
            NewGameButton.TextOffset = new Vector2(0, 10);

            LoadGameButton = new Button(GeneralManager.GetPartialRect(0, 0.5f, 0.2f, 0.1f), " Load Game ", GeneralManager.Textures["GUI/MainMenuGraphics/Button"], Color.Gray, Color.White, GeneralManager.Fonts["Fonts/28DaysLater"]);
            LoadGameButton.Action = NewGameButtonClick;
            LoadGameButton.TextOffset = new Vector2(0, 10);

            OptionsButton = new Button(GeneralManager.GetPartialRect(0, 0.6f, 0.2f, 0.1f), "  Options  ", GeneralManager.Textures["GUI/MainMenuGraphics/Button"], Color.Gray, Color.White, GeneralManager.Fonts["Fonts/28DaysLater"]);
            OptionsButton.Action = NewGameButtonClick;
            OptionsButton.TextOffset = new Vector2(0, 10);

            CreditsButton = new Button(GeneralManager.GetPartialRect(0, 0.7f, 0.2f, 0.1f), "  Credits  ", GeneralManager.Textures["GUI/MainMenuGraphics/Button"], Color.Gray, Color.White, GeneralManager.Fonts["Fonts/28DaysLater"]);
            CreditsButton.Action = NewGameButtonClick;
            CreditsButton.TextOffset = new Vector2(0, 10);

            ExitButton = new Button(GeneralManager.GetPartialRect(0, 0.8f, 0.2f, 0.1f), "    Exit    ", GeneralManager.Textures["GUI/MainMenuGraphics/Button"], Color.Gray, Color.White, GeneralManager.Fonts["Fonts/28DaysLater"]);
            ExitButton.Action = ExitGameButtonClick;
            ExitButton.TextOffset = new Vector2(0, 10);

            this.AddGUI(NewGameButton);
            this.AddGUI(LoadGameButton);
            this.AddGUI(OptionsButton);
            this.AddGUI(CreditsButton);
            this.AddGUI(ExitButton);

            BSS = new BlackScreenSwitch();
            BSS.SwitchState = SceneSwitchEffect.State.SwitchOn;

            Renderer.AddPostProcess(BSS, "Switch");
            m = SoundEngine.PlayLooped("Content/Sounds/Ambient/menu_theme.mp3");
        }

        void NewGameButtonClick()
        {
            if (BSS.SwitchState != SceneSwitchEffect.State.SwitchOff)
            {
                BSS.SwitchState = SceneSwitchEffect.State.SwitchOff;
                BSS.MaxTime = 1f;
                BSS.TurnOffAction = SwitchOffNewGame;
            }
        }

        void ExitGameButtonClick()
        {
            if (BSS.SwitchState != SceneSwitchEffect.State.SwitchOff)
            {
                BSS.SwitchState = SceneSwitchEffect.State.SwitchOff;
                BSS.MaxTime = 1f;
                BSS.TurnOffAction = SwitchOffExit;
            }
        }

        void SwitchOffNewGame()
        {
            Renderer.RemovePostProcess("Switch");
            m.Stop();
            this.SwitchTo(new WorldScreen(this.Parent));
        }

        void SwitchOffExit()
        {
            Renderer.RemovePostProcess("Switch");
            m.Stop();
            GeneralManager.Game.Exit();
            //this.SwitchTo(new WorldScreen(this.Parent));
        }

        public override void Update(GameTime GameTime)
        {
            CloudsOffset += 0.0001f;
            if (CloudsOffset > 1)
            {
                CloudsOffset -= 1;
            }

            FogOffset += 0.0003f;
            if (FogOffset > 1)
            {
                FogOffset -= 1;
            }

            MouseOffset = (float)GeneralManager.MousePos.X / (float)GeneralManager.ScreenX - 0.5f;        

            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)   
        {

            Renderer.Draw(GeneralManager.Textures["GUI/MainMenuGraphics/CloudLayer"], GeneralManager.GetPartialRect(CloudsOffset + MouseOffset / 30f + 0.05f, 0, 1.1f, 1));
            Renderer.Draw(GeneralManager.Textures["GUI/MainMenuGraphics/CloudLayer"], GeneralManager.GetPartialRect(CloudsOffset - 1 + MouseOffset / 30f - 0.05f, 0, 1.1f, 1));


            Renderer.Draw(GeneralManager.Textures["GUI/MainMenuGraphics/CityMainLayer"], GeneralManager.GetPartialRect(MouseOffset / 150f - 0.05f, 0, 1.1f, 1));

            Renderer.Draw(GeneralManager.Textures["GUI/MainMenuGraphics/CitySecondaryLayer"], GeneralManager.GetPartialRect(-MouseOffset / 100f - 0.05f, 0, 1.1f, 1));

            Rectangle FogPos = GeneralManager.GetPartialRect(FogOffset - 1 - MouseOffset / 30f - 0.05f, 0, 1.1f, 1);
            Renderer.Draw(GeneralManager.Textures["GUI/MainMenuGraphics/FogLayer"], FogPos);
            Renderer.Draw(GeneralManager.Textures["GUI/MainMenuGraphics/FogLayer"], new Rectangle(FogPos.X + (int)(GeneralManager.ScreenX * 1.1f), FogPos.Y, FogPos.Width, FogPos.Height));



            base.Draw(GameTime);
        }

    }
}

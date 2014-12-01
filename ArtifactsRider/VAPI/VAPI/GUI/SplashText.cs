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
    public class SplashText : GUIComponent 
    {
        
        public float LifeTime;
        public float CurrentLife;
        public string Name;
        public float Opacity;
        public float OpacityChange;
        public Vector2 Speed;
        public float Scale;
        public float ScaleChange;
        public Color BaseColor;
        public Color TargetColor;
        SpriteFont Font;


        public SplashText(SpriteFont Font)
        {
            this.Font = Font;
        }

        public override void Update(GameTime gameTime)
        {
            Position = Helper.AddRectPos(Position, new Rectangle((int)Speed.X,(int)Speed.Y,0,0));
            Opacity += OpacityChange;
            Scale += ScaleChange;
            CurrentLife += gameTime.ElapsedGameTime.Milliseconds;
            if (CurrentLife > LifeTime)
            {
                // TODO - dodać usuwanie
            }

        }

        public override void Draw()
        {
            float Progress = CurrentLife/LifeTime;
            Renderer.PostDrawFont(Font, Position, this.Name, new Color(BaseColor.R * (1f - Progress) + TargetColor.R * Progress, BaseColor.G * (1f - Progress) + TargetColor.G * Progress, BaseColor.B * (1f - Progress) + TargetColor.B * Progress, Opacity));
            //SpriteBatch.DrawString(Font, Name, Position, new Color(BaseColor.R * (1f - Progress) + TargetColor.R * Progress, BaseColor.G * (1f - Progress) + TargetColor.G * Progress, BaseColor.B * (1f - Progress) + TargetColor.B * Progress, Opacity), 0f, Font.MeasureString(Name) * Scale / 2f, Scale, SpriteEffects.None, 0.1f);

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

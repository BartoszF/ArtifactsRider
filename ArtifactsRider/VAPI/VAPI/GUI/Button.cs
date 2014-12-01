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
using VAPI.RenderEffects;

namespace VAPI
{
    public class Button:GUIComponent
    {
        public string Text;
        public Texture2D Background;
        public Color ButtonColor;
        public Color ActiveColor;
        public SpriteFont Font;
        public Vector2 TextOffset;

        public delegate void ButtonAction();

        public ButtonAction Action;

        public Button(Rectangle Position, string Text, Texture2D Background, Color ButtonColor, Color ActiveColor, SpriteFont Font)
        {
            this.Position = Position;
            this.Text = Text;
            this.Background = Background;
            this.ButtonColor = ButtonColor;
            this.ActiveColor = ActiveColor;
            if (Font != null)
            {
                this.Font = Font;
            }
        }

        public override void Draw()
        {
            // TODO Calc Scale
            //=======
            if (!IsActive)
            {
                Renderer.PostDraw(Background, Position, ButtonColor);
                if (Font != null)
                {
                    Renderer.PostDrawFont(Font, Helper.AddRectPos(Position, TextOffset), Text, ButtonColor);
                }
            }
            else
            {
                Renderer.PostDraw(Background, Position, ActiveColor);
                if (Font != null)
                {
                    Renderer.PostDrawFont(Font, Helper.AddRectPos(Position, TextOffset), Text, ActiveColor);
                }
            
            }
        }
        public override void Update(GameTime gameTime)
        {
            
        }

        public override bool CheckActive()
        {
            
            IsActive = Helper.CheckIfInside(Position, GeneralManager.MousePos);
            return IsActive;
        }

        public override bool HandleInput()
        {
            if (GeneralManager.CheckKeyEdge(Keys.Enter) && IsActive || Helper.CheckLMBClick(Position))
            {
                if (Action != null)
                {
                    Action();
                }
                return true;
            }
            return false;
        }
    }
}

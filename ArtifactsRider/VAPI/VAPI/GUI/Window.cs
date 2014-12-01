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
    /// <summary>
    /// Window class
    /// </summary>
    public class Window : GUIComponent
    {
        public Stack<GUIComponent> GUIComponents;       /**< Stack of GUIComponents \"Docked\" in window */
        public bool Visible = true;
        public Texture2D BgTex;

        public Window(Rectangle Position, Color col)
        {
            GUIComponents = new Stack<GUIComponent>();
            this.Position = Position;
            BgTex = new Texture2D(Renderer.GD, 1, 1);
            BgTex.SetData<Color>(new[] { col });
        }

        public Window(Rectangle Position)
        {
            GUIComponents = new Stack<GUIComponent>();
            this.Position = Position;
            BgTex = GeneralManager.Textures["GUI/windowBg"];
        }

        public Window(Rectangle Position, string TextureName)
        {
            GUIComponents = new Stack<GUIComponent>();
            this.Position = Position;
            this.BgTex = GeneralManager.Textures[TextureName];
        }

        public override void Draw()
        {
            if (Visible)
            {
                Renderer.PostDraw(BgTex, Position);
                //SpriteBatch.Draw(BgTex, Position, Color.White);

                foreach (GUIComponent G in GUIComponents)
                {
                    G.Draw();
                }
            }
        }

        public override bool HandleInput()
        {
            if (Visible)
            {
                if (Position.Contains(Helper.VectorToPoint(GeneralManager.MousePos)) && GeneralManager.IsLMBClicked())
                {
                    foreach (GUIComponent G in GUIComponents)
                    {
                        if (G.HandleInput()) break;
                    }
                    return true;
                }
            }
            return false;
        }

        public override bool CheckActive()
        {
            if (Visible)
            {
                return Position.Contains(Helper.VectorToPoint(GeneralManager.MousePos));
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            if (Visible)
            {
                foreach (GUIComponent G in GUIComponents)
                {
                    G.Update(gameTime);
                }
            }
        }

        public void AddGUI(GUIComponent GUI, GUIComponent parent)
        {
            if (parent == null)
                this.GUIComponents.Push(GUI);
            else
            {
                GUI.Position = Helper.AddRectPos(GUI.Position, parent.Position);
                this.GUIComponents.Push(GUI);
            }
        }

        public void AddGUI(GUIComponent GUI)
        {

            GUI.Position = Helper.AddRectPos(GUI.Position, this.Position);
            this.GUIComponents.Push(GUI);
        }
    }
}

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
    /// <summary>
    /// Class to unify drawing. 
    /// </summary>
    public class Renderer
    {
        public static SpriteBatch SpriteBatch;              /**< Reference to SpriteBatch instance */
        public static GraphicsDevice GD;                    /**< Reference to GraphicsDevice */
        public static RenderTarget2D MainRenderTarget;      /**< Reference to main RenderTarget2D */

        static Dictionary<float, Queue<DrawElement>> DrawingQueue = new Dictionary<float, Queue<DrawElement>>();        /**< Layers of Drawing Elements Queue */
        static Dictionary<float, Queue<DrawElement>> PostDrawingQueue = new Dictionary<float, Queue<DrawElement>>();    /**< Layers of Drawing Post Elements Queue */

        static Dictionary<string, RenderEffect> RenderEffects;      /**< Render Effects */
        static Dictionary<string, RenderEffect> PostProcess;        /**< Post Processes */

        #region DrawFunctions

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Vector2</param>
        public static void Draw(Texture2D Tex, Vector2 Position)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color.White;

            AddToQueue(DE, 0);

        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to Draw</param>
        /// <param name="Position">Position in Rectangle</param>
        public static void Draw(Texture2D Tex, Rectangle Position)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color.White;
            AddToQueue(DE, 0);
        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw region from</param>
        /// <param name="Position">Position in Vector2</param>
        /// <param name="Region">Region to copy from texture</param>
        public static void Draw(Texture2D Tex, Vector2 Position, Rectangle Region)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color.White;
            DE.Region = Region;

            AddToQueue(DE, 0);

        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw region from</param>
        /// <param name="Position">Position in Rectangle</param>
        /// <param name="Region">Region to copy from texture</param>
        public static void Draw(Texture2D Tex, Rectangle Position, Rectangle Region)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color.White;
            DE.Region = Region;

            AddToQueue(DE, 0);
        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Vector2</param>
        /// <param name="Color">Color to draw with</param>
        public static void Draw(Texture2D Tex, Vector2 Position, Color Color)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color;
            AddToQueue(DE, 0);
        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Rectangle</param>
        /// <param name="Color">Color to draw with</param>
        public static void Draw(Texture2D Tex, Rectangle Position, Color Color)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color;
            AddToQueue(DE, 0);
        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Vector2</param>
        /// <param name="Color">Color to draw with</param>
        /// <param name="Z">Layer on wich to draw</param>
        public static void Draw(Texture2D Tex, Vector2 Position, Color Color, float Z)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color;
            AddToQueue(DE, Z);
        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Rectangle</param>
        /// <param name="Color">Color to draw with</param>
        /// <param name="Z">Layer on wich to draw</param>
        public static void Draw(Texture2D Tex, Rectangle Position, Color Color, float Z)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color;
            AddToQueue(DE, Z);
        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Vector2</param>
        /// <param name="Angle">Angle to draw with</param>
        /// <param name="Center">Origin of texture</param>
        static void Draw(Texture2D Tex, Vector2 Position, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color.White;
            DE.Angle = Angle;
            DE.RotationCenter = Center;

            AddToQueue(DE, 0);

        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Rectangle</param>
        /// <param name="Angle">Angle to draw with</param>
        /// <param name="Center">Origin of texture</param>
        public static void Draw(Texture2D Tex, Rectangle Position, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color.White;
            DE.Angle = Angle;
            DE.RotationCenter = Center;
            AddToQueue(DE, 0);
        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Rectangle</param>
        /// <param name="color">Color to draw with</param>
        /// <param name="Angle">Angle to draw with</param>
        /// <param name="Center">Origin of texture</param>
        /// <param name="Z">Layer on wich to draw</param>
        public static void Draw(Texture2D Tex, Rectangle Position, Color color, float Angle, Vector2 Center, float Z)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = color;
            DE.Angle = Angle;
            DE.RotationCenter = Center;
            AddToQueue(DE, Z);
        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Vector2</param>
        /// <param name="Region">Region</param>
        /// <param name="Angle">Angle to drawi with</param>
        /// <param name="Center">Origin of texture</param>
        public static void Draw(Texture2D Tex, Vector2 Position, Rectangle Region, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color.White;
            DE.Region = Region;
            DE.Angle = Angle;
            DE.RotationCenter = Center;

            AddToQueue(DE, 0);

        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Rectangle</param>
        /// <param name="Region">Region</param>
        /// <param name="Angle">Angle to draw with</param>
        /// <param name="Center">Origin of texture</param>
        /// <param name="Z">Layer on wich to draw</param>
        public static void Draw(Texture2D Tex, Rectangle Position, Rectangle Region, float Angle, Vector2 Center, float Z)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color.White;
            DE.Region = Region;
            DE.Angle = Angle;
            DE.RotationCenter = Center;

            AddToQueue(DE, Z);

        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Rectangle</param>
        /// <param name="Region">Region</param>
        /// <param name="Angle">Angle to draw with</param>
        /// <param name="Center">Origin of texture</param>
        public static void Draw(Texture2D Tex, Rectangle Position, Rectangle Region, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color.White;
            DE.Region = Region;
            DE.Angle = Angle;
            DE.RotationCenter = Center;

            AddToQueue(DE, 0);
        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Vector2</param>
        /// <param name="Color">Color to draw with</param>
        /// <param name="Angle">Angle to draw with</param>
        /// <param name="Center">Origin of texture</param>
        public static void Draw(Texture2D Tex, Vector2 Position, Color Color, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color;
            DE.Angle = Angle;
            DE.RotationCenter = Center;

            AddToQueue(DE, 0);
        }

        /// <summary>
        /// Draw Function
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Rectangle</param>
        /// <param name="Color">Color to draw with</param>
        /// <param name="Angle">Angle to draw with</param>
        /// <param name="Center">Origin of texture</param>
        public static void Draw(Texture2D Tex, Rectangle Position, Color Color, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color;
            DE.Angle = Angle;
            DE.RotationCenter = Center;

            AddToQueue(DE, 0);
        }


        #endregion

        #region PostDrawFunctions

        /// <summary>
        /// Drawing after main Draw
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Vector2</param>
        public static void PostDraw(Texture2D Tex, Vector2 Position)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color.White;

            AddToPostQueue(DE, 0);
        }

        /// <summary>
        /// Drawing after main Draw
        /// </summary>
        /// <param name="Tex">Texture to draw</param>
        /// <param name="Position">Position in Rectangle</param>
        public static void PostDraw(Texture2D Tex, Rectangle Position)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color.White;
            AddToPostQueue(DE, 0);
        }

        public static void PostDraw(Texture2D Tex, Rectangle Position, bool Sciss, Rectangle? scissRect)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color.White;
            if (Sciss && scissRect.HasValue)
            {
                DE.Scissor = scissRect;
            }

            AddToPostQueue(DE, 0);
        }

        public static void PostDraw(Texture2D Tex, Vector2 Position, Rectangle Region)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color.White;
            DE.Region = Region;

            AddToPostQueue(DE, 0);

        }

        public static void PostDraw(Texture2D Tex, Rectangle Position, Rectangle Region)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color.White;
            DE.Region = Region;

            AddToPostQueue(DE, 0);
        }

        public static void PostDraw(Texture2D Tex, Vector2 Position, Color Color)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color;
            AddToPostQueue(DE, 0);
        }

        public static void PostDraw(Texture2D Tex, Rectangle Position, Color Color)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color;
            AddToPostQueue(DE, 0);
        }

        public static void PostDraw(Texture2D Tex, Vector2 Position, Color Color, float Z)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color;
            AddToPostQueue(DE, Z);
        }

        public static void PostDraw(Texture2D Tex, Rectangle Position, Color Color, float Z)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color;
            AddToPostQueue(DE, Z);
        }

        public static void PostDraw(Texture2D Tex, Vector2 Position, Color Color, float Z, Texture2D Normal)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color;
            AddToPostQueue(DE, Z);
        }

        public static void PostDraw(Texture2D Tex, Rectangle Position, Color Color, float Z, Texture2D Normal)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color;
            AddToPostQueue(DE, Z);
        }

        static void PostDraw(Texture2D Tex, Vector2 Position, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color.White;
            DE.Angle = Angle;
            DE.RotationCenter = Center;

            AddToPostQueue(DE, 0);

        }

        public static void PostDraw(Texture2D Tex, Rectangle Position, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color.White;
            DE.Angle = Angle;
            DE.RotationCenter = Center;
            AddToPostQueue(DE, 0);
        }

        public static void PostDraw(Texture2D Tex, Vector2 Position, Rectangle Region, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color.White;
            DE.Region = Region;
            DE.Angle = Angle;
            DE.RotationCenter = Center;

            AddToPostQueue(DE, 0);

        }

        public static void PostDraw(Texture2D Tex, Rectangle Position, Rectangle Region, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color.White;
            DE.Region = Region;
            DE.Angle = Angle;
            DE.RotationCenter = Center;

            AddToPostQueue(DE, 0);
        }

        public static void PostDraw(Texture2D Tex, Vector2 Position, Color Color, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color;
            DE.Angle = Angle;
            DE.RotationCenter = Center;

            AddToPostQueue(DE, 0);
        }

        public static void PostDraw(Texture2D Tex, Rectangle Position, Color Color, float Angle, Vector2 Center)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = Tex;
            DE.Pos = Position;
            DE.Color = Color;
            DE.Angle = Angle;
            DE.RotationCenter = Center;

            AddToPostQueue(DE, 0);
        }

        #endregion

        #region Drawing Fonts

        public static void DrawFont(SpriteFont Font, Vector2 Position, string Text)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = null;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color.White;
            DE.Text = Text;
            DE.Font = Font;

            AddToQueue(DE, 0);
        }

        public static void DrawFont(SpriteFont Font, Rectangle Position, string Text)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = null;
            DE.Pos = Position;
            DE.Color = Color.White;
            DE.Text = Text;
            DE.Font = Font;

            AddToQueue(DE, 0);
        }

        public static void DrawFont(SpriteFont Font, Vector2 Position, string Text, Color Color)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = null;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color;
            DE.Text = Text;
            DE.Font = Font;

            AddToQueue(DE, 0);
        }

        public static void DrawFont(SpriteFont Font, Rectangle Position, string Text, Color Color)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = null;
            DE.Pos = Position;
            DE.Color = Color;
            DE.Text = Text;
            DE.Font = Font;

            AddToQueue(DE, 0);
        }

        #endregion

        #region PostDrawing Fonts

        public static void PostDrawFont(SpriteFont Font, Vector2 Position, string Text)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = null;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color.White;
            DE.Text = Text;
            DE.Font = Font;

            AddToPostQueue(DE, 0);
        }

        public static void PostDrawFont(SpriteFont Font, Vector2 Position, string Text, bool Sciss, Rectangle? scissRect)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = null;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color.White;
            DE.Text = Text;
            DE.Font = Font;

            if (Sciss && scissRect.HasValue)
            {
                DE.Scissor = scissRect;
            }

            AddToPostQueue(DE, 0);
        }

        public static void PostDrawFont(SpriteFont Font, Rectangle Position, string Text)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = null;
            DE.Pos = Position;
            DE.Color = Color.White;
            DE.Text = Text;
            DE.Font = Font;

            AddToPostQueue(DE, 0);
        }

        public static void PostDrawFont(SpriteFont Font, Vector2 Position, string Text, Color Color)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = null;
            DE.Pos = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
            DE.Color = Color;
            DE.Text = Text;
            DE.Font = Font;

            AddToPostQueue(DE, 0);
        }

        public static void PostDrawFont(SpriteFont Font, Rectangle Position, string Text, Color Color)
        {
            DrawElement DE = new DrawElement();

            DE.Texture = null;
            DE.Pos = Position;
            DE.Color = Color;
            DE.Text = Text;
            DE.Font = Font;

            AddToPostQueue(DE, 0);
        }

        #endregion

        #region DrawingEngine

        public static void BeginDraw()
        {
            //DrawingQueue = new Dictionary<float, Queue<DrawElement>>();
            //PostDrawingQueue = new Dictionary<float, Queue<DrawElement>>();
        }

        public static void FinishDraw()
        {
            //MainRenderTarget 
            GD.SetRenderTarget(MainRenderTarget);
            GD.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            foreach (KeyValuePair<string, RenderEffect> RE in RenderEffects)
            {
                RE.Value.BeginEffect();
            }

            if (DrawingQueue != null)
            {
                var list = DrawingQueue.Keys.ToList();
                list.Sort();

                foreach (var DL in list)
                {
                    while (DrawingQueue[DL].Count > 0)
                    {
                        SolveDraw(DrawingQueue[DL].Dequeue());
                    }
                }
                SpriteBatch.End();

                foreach (KeyValuePair<string, RenderEffect> RE in RenderEffects)
                {
                    RE.Value.EndEffect();
                }

                GD.SetRenderTarget(null);


                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                foreach (KeyValuePair<string, RenderEffect> RE in PostProcess)
                {
                    RE.Value.BeginEffect();
                }

                SpriteBatch.Draw(MainRenderTarget, Vector2.Zero, Color.White);
                SpriteBatch.End();


                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                foreach (KeyValuePair<float, Queue<DrawElement>> DL in PostDrawingQueue)
                {
                    while(DL.Value.Count > 0)
                    {
                        PostSolveDraw(DL.Value.Dequeue());
                    }
                }

                SpriteBatch.End();

                foreach (KeyValuePair<string, RenderEffect> RE in PostProcess)
                {
                    RE.Value.EndEffect();
                }

                DrawingQueue.Clear();
                PostDrawingQueue.Clear();
                //DrawingQueue = null;
                //PostDrawingQueue = null;
                //GC.Collect();
            }
        }

        public static RenderEffect GetRenderEffect(string Name)
        {
            return RenderEffects[Name];
        }

        public static RenderEffect GetPostProcess(string Name)
        {
            return PostProcess[Name];
        }

        static void SolveDraw(DrawElement DE)
        {
            if (DE.Region == null)
            {
                if (DE.Pos.Width == 0 && DE.Pos.Height == 0)
                {
                    if (DE.Font != null)
                    {
                        //Jeżeli vector i tekst
                        SpriteBatch.DrawString(DE.Font, DE.Text, new Vector2(DE.Pos.X - Camera.GetRect.X, DE.Pos.Y - Camera.GetRect.Y), DE.Color);
                    }
                    else
                    {
                        //jeżeli vector i tekstura
                        SpriteBatch.Draw(DE.Texture, Helper.GetTopLeftFromRect(DE.Pos) - new Vector2(Camera.GetRect.X, Camera.GetRect.Y), null, DE.Color, DE.Angle, DE.RotationCenter, Vector2.One, SpriteEffects.None, 0f);
                    }
                }
                else
                {
                    if (DE.Font != null)
                    {
                        //jeżeli rectangle i text
                        float Scale = (DE.Font.MeasureString(DE.Text).X) / (float)(DE.Pos.Width);
                        int CurrentLenght = DE.Pos.X;
                        SpriteBatch.DrawString(DE.Font, DE.Text, new Vector2(DE.Pos.X, DE.Pos.Y) - new Vector2(Camera.GetRect.X, Camera.GetRect.Y), DE.Color, 0f, Vector2.Zero, 1f/Scale, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        //jeżeli rectangle i tekstura
                        SpriteBatch.Draw(DE.Texture, Helper.SubRectPos(DE.Pos, Camera.GetRect), null, DE.Color, DE.Angle, DE.RotationCenter, SpriteEffects.None, 0f);
                    }
                }
            }
            else if(DE.Font == null) // If there is region 
            {
                if (DE.Pos.Width == 0 && DE.Pos.Height == 0)
                {
                    SpriteBatch.Draw(DE.Texture, Helper.GetTopLeftFromRect(DE.Pos) - new Vector2(Camera.GetRect.X, Camera.GetRect.Y), DE.Region, DE.Color, DE.Angle, DE.RotationCenter, Vector2.One, SpriteEffects.None, 0f);
                }
                else
                {
                    SpriteBatch.Draw(DE.Texture, Helper.SubRectPos(DE.Pos, Camera.GetRect), DE.Region, DE.Color, DE.Angle, DE.RotationCenter, SpriteEffects.None, 0f);
                }
            }
        }

        static void PostSolveDraw(DrawElement DE)
        {
            Rectangle currentRect = Rectangle.Empty;
            if (DE.Scissor.HasValue)
            {
                RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };

                SpriteBatch.End();

                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                      null, null, _rasterizerState);

                currentRect = SpriteBatch.GraphicsDevice.ScissorRectangle;

                SpriteBatch.GraphicsDevice.ScissorRectangle = DE.Scissor.Value;
            }

            if (DE.Region == null)
            {
                if (DE.Pos.Width == 0 && DE.Pos.Height == 0)
                {
                    if (DE.Font != null)
                    {
                        //Jeżeli vector i tekst
                        SpriteBatch.DrawString(DE.Font, DE.Text, new Vector2(DE.Pos.X, DE.Pos.Y), DE.Color);
                    }
                    else
                    {
                        //jeżeli vector i tekstura
                        SpriteBatch.Draw(DE.Texture, Helper.GetTopLeftFromRect(DE.Pos), null, DE.Color, DE.Angle, DE.RotationCenter, Vector2.One, SpriteEffects.None, 0f);
                    }
                }
                else
                {
                    if (DE.Font != null)
                    {
                        //jeżeli rectangle i text
                        float Scale = (DE.Font.MeasureString(DE.Text).X) / (float)(DE.Pos.Width);
                        int CurrentLenght = DE.Pos.X;
                        SpriteBatch.DrawString(DE.Font, DE.Text, new Vector2(DE.Pos.X, DE.Pos.Y), DE.Color, 0f, Vector2.Zero, 1f/Scale, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        //jeżeli rectangle i tekstura
                        SpriteBatch.Draw(DE.Texture, DE.Pos, null, DE.Color, DE.Angle, DE.RotationCenter, SpriteEffects.None, 0f);
                    }
                }
            }
            else if (DE.Font == null) // If there is region 
            {
                if (DE.Pos.Width == 0 && DE.Pos.Height == 0)
                {
                    SpriteBatch.Draw(DE.Texture, Helper.GetTopLeftFromRect(DE.Pos), DE.Region, DE.Color, DE.Angle, DE.RotationCenter, Vector2.One, SpriteEffects.None, 0f);
                }
                else
                {
                    SpriteBatch.Draw(DE.Texture, DE.Pos, DE.Region, DE.Color, DE.Angle, DE.RotationCenter, SpriteEffects.None, 0f);
                }
            }

            if (DE.Scissor.HasValue)
            {
                SpriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
                SpriteBatch.End();
                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            }
        }

        public static void Init(GraphicsDevice _GD)
        {
            GD = _GD;
            SpriteBatch = new SpriteBatch(GD);      
            RenderEffects = new Dictionary<string, RenderEffect>();
            PostProcess = new Dictionary<string, RenderEffect>();
            MainRenderTarget = new RenderTarget2D(GD, GeneralManager.ScreenX, GeneralManager.ScreenY, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

        public static void AddRendererEffect(RenderEffect RE, string Name)
        {
            if(!RenderEffects.ContainsKey(Name))
                RenderEffects.Add(Name, RE);
            else
                RenderEffects[Name] = RE;
        }

        public static void RemoveRendererEffect(string Name)
        {
            RenderEffect RE = RenderEffects[Name];
            RenderEffects.Remove(Name);
            RE.Dispose();
        }

        public static void AddPostProcess(RenderEffect RE, string Name)
        {
            if (!PostProcess.ContainsKey(Name))
                PostProcess.Add(Name, RE);
            else
                PostProcess[Name] = RE;
        }

        public static void RemovePostProcess(string Name)
        {
            RenderEffect RE = PostProcess[Name];
            PostProcess.Remove(Name);
            RE.Dispose();
        }

        static void AddToQueue(DrawElement DE, float Z)
        {
            if (DrawingQueue.ContainsKey(Z))
            {
                DrawingQueue[Z].Enqueue(DE);
            }
            else
            {
                Queue<DrawElement> DEList = new Queue<DrawElement>();
                DEList.Enqueue(DE);
                DrawingQueue.Add(Z, DEList);
            }
        }

        static void AddToPostQueue(DrawElement DE, float Z)
        {
            if (PostDrawingQueue.ContainsKey(Z))
            {
                PostDrawingQueue[Z].Enqueue(DE);
            }
            else
            {
                Queue<DrawElement> DEList = new Queue<DrawElement>();
                DEList.Enqueue(DE);
                PostDrawingQueue.Add(Z, DEList);
            }
        }

        public static void Update(GameTime GameTime)
        {
            try
            {

                foreach (KeyValuePair<string, RenderEffect> RE in RenderEffects)
                {
                    RE.Value.UpdateEffect(GameTime);
                }

                foreach (KeyValuePair<string, RenderEffect> RE in PostProcess)
                {
                    RE.Value.UpdateEffect(GameTime);
                }
            }
            catch(Exception e)
            {
                Logger.Write(e.Message);
            }
        }
    }

    /// 
    /// \brief Element to be drawn.
    /// 
    /// Unifies drawing every element, without need to fill every property
    /// 
    internal struct DrawElement
    {
        public Texture2D Texture;       /**< Texture */
        public Rectangle Pos;           /**< Position */
        public Rectangle? Region;       /**< Region */
        public Rectangle? Scissor;      /**< Rectangle for Scissor Test */
        public Color Color;             /**< Color */
        public string Text;             /**< Text - if we are drawing font */
        public SpriteFont Font;         /**< Font - if we are drawing font */
        public float Angle;             /**< Angle */
        public Vector2 RotationCenter;  /**< Origin */
    }

#endregion
}

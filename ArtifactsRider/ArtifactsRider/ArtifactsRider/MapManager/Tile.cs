using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VAPI;
using VAPI.RenderEffects;
using Krypton;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

namespace ArtifactsRider.MapManager
{
    /// <summary>
    /// Base class for Tiles
    /// </summary>
    public class Tile
    {
        public Rectangle position;      /**< Position */
        public float Z;                 /**< Z Order */
        public bool Isolated;           /**< If tile is surrounded with walls ? */
        public bool WasVisible;         /**< If tile was Visible ? */
        public bool IsVisible;          /**< If tile is Visible ? */

        /// <summary>
        /// Is Tile soild ?
        /// </summary>
        public bool isSolid
        {
            get
            {
                return Cost == 0;
            }
            set
            {
                if (value == false)
                {
                    Cost = 30;
                }
                else
                {
                    Cost = 0;
                }
            }
        }
        public string name;             /**< Name of tile */
        string texName;                 /**< Name of texture */
        string normName;                /**< Name of normalMap */
        public int Cost;                /**< Cost to walk on this tile */
        private ShadowHull _Hull;       /**< Hull of tile to cast Shadow */
        /// <summary>
        /// Getter of ShadowHull
        /// </summary>
        public ShadowHull Hull
        {
            get
            {
                return _Hull;
            }
        }

        private Fixture _Fixture;       /**< Physics Fixture */
        public Fixture Fixture
        {
            get
            {
                return _Fixture;
            }
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="pos">Position</param>
        /// <param name="name">Name</param>
        /// <param name="Cost">Cost</param>
        public Tile(Rectangle pos, string name, int Cost)
        {
            this.position = pos;
            this.name = name;
            this.texName = "Tiles/" + this.name;
            this.normName = "Tiles/" + this.name + "_normal";
            this.Cost = Cost;
            if (Cost == 0) Z = 2f;
        }

        /// <summary>
        /// Update loop
        /// </summary>
        /// <param name="gameTime">Time that passed since last Update loop</param>
        public void Update(GameTime gameTime)
        {
            WasVisible = IsVisible;
            IsVisible = Camera.GetRect.Intersects(position);

            if (isSolid && !Isolated)
            {
                if (!WasVisible && IsVisible)
                {
                    Vector2 Size = 2 * new Vector2((float)position.Width / (float)GeneralManager.ScreenX, (float)position.Height / (float)GeneralManager.ScreenY);
                    SetRectHull(Size, 2 * new Vector2((float)position.X / (float)GeneralManager.ScreenX, (float)-position.Y / (float)GeneralManager.ScreenY) - new Vector2(1f, -1f) + new Vector2(Size.X / 2, -Size.Y / 2));
                }
                else if (WasVisible && !IsVisible)
                {
                    RemoveHull();
                }
            }
        }

        /// <summary>
        /// Draw method
        /// </summary>
        /// <param name="gameTime">Time that passed since last Update loop</param>
        public void Draw(GameTime gameTime)
        {
            if (!Isolated && IsVisible)
            {
                Rectangle r = new Rectangle(position.X, position.Y, position.Width, position.Height);

                Renderer.Draw(GeneralManager.Textures[texName], r,Color.White,Z);

                LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");
                LE.DrawNormal(GeneralManager.Textures[normName], r);
            }
        }

        /// <summary>
        /// Set ShadowHull of Tile
        /// </summary>
        /// <param name="Size">Size</param>
        /// <param name="Pos">Position</param>
        public void SetRectHull(Vector2 Size, Vector2 Pos)
        {
            ShadowHull H = ShadowHull.CreateRectangle(Size);
            H.Position = Pos;
            LightingEngine LE = ((LightingEngine)Renderer.GetRenderEffect("LightingEngine"));
            if (Hull != null)
            {
                LE.RemoveHull(Hull);
            }
            LE.AddHull(H);
            this._Hull = H;
            
        }

        /// <summary>
        /// Remove ShadowHull
        /// </summary>
        public void RemoveHull()
        {
            LightingEngine LE = ((LightingEngine)Renderer.GetRenderEffect("LightingEngine"));
            LE.RemoveHull(Hull);
            this._Hull = null;
        }

        /// <summary>
        /// Set Physics Fixture and add to physical World
        /// </summary>
        /// <param name="PhysicalWorld">Physical World to add this Tile</param>
        public void SetPhysics(World PhysicalWorld)
        {
            RemovePhysics(PhysicalWorld);

            Body BodyDec = new Body(PhysicalWorld);
            BodyDec.BodyType = BodyType.Static;

            Vertices V= new Vertices();
            V.Add(new Vector2(position.X,position.Y)); 
            V.Add(new Vector2(position.X + position.Width,position.Y));
            V.Add(new Vector2(position.X + position.Width, position.Y + position.Height));
            V.Add(new Vector2(position.X, position.Y + position.Height));

            _Fixture = BodyDec.CreateFixture(new PolygonShape(V, 0.5f));
        }

        /// <summary>
        /// Remove from physical World
        /// </summary>
        /// <param name="PhysicalWorld"></param>
        public void RemovePhysics(World PhysicalWorld)
        {
            if (Fixture != null)
            {
                PhysicalWorld.RemoveBody(Fixture.Body);
                _Fixture = null;
            }
        }

        public string Serialize()
        {
            return "";
        }
    }
}

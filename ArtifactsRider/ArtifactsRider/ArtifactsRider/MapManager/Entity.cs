using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using VAPI;
using VAPI.RenderEffects;

namespace ArtifactsRider.MapManager
{
    /// <summary>
    /// Base class for Entities
    /// </summary>
    public class Entity
    {
        protected string Name;      /**< Name of Entitiy */
        protected Map Map;          /**< Map reference */
        Vector2 size;               /**< Size of Entity */

        bool highlight = false;     /**< If we need to highlight object ? */

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Map">Map</param>
        public Entity(Map Map)
        {
            this.Map = Map;
            Map.AddEntity(this);
        }

        /// <summary>
        /// Position of Entity
        /// </summary>
        public Rectangle Position
        {
            get
            {
                if (Fixture != null)
                {
                    return new Rectangle((int)Fixture.Body.Position.X, (int)Fixture.Body.Position.Y, (int)size.X, (int)size.Y);
                }
                else
                {
                    return _Position;
                }
            }
            set
            {
                if (Fixture != null)
                {
                    Fixture.Body.SetTransform(new Vector2(value.X, value.Y), 0f);
                    size.X = value.Width;
                    size.Y = value.Height;
                }
                else
                {
                    _Position = value;
                    size.X = value.Width;
                    size.Y = value.Height;
                }
            }
        }
        private Fixture _Fixture;       /**< Physics Fixture */
        /// <summary>
        /// Properties of Physics Fixture 
        /// </summary>
        public Fixture Fixture
        {
            get
            {
                return _Fixture;
            }
            set
            {
                _Fixture = value;

                if (Fixture != null)
                {
                    Fixture.UserData = this;
                    Fixture.OnCollision += new OnCollisionEventHandler((Fixture A, Fixture B, Contact C) =>
                    {
                        if (A == Fixture)
                        {
                            if (B.UserData != null)
                            {
                                Interact((Entity)B.UserData);
                            }

                            Collide(B);
                        }
                        else if (B == Fixture)
                        {
                            if (A.UserData != null)
                            {
                                Interact((Entity)A.UserData);
                            }

                            Collide(A);
                        }
                        return true;
                    }

                        );
                }
            }
        }
        Rectangle _Position;        /**< Position of Entity */

        /// <summary>
        /// Get position in Vector2
        /// </summary>
        public virtual Vector2 GetPosition()
        {
            return new Vector2(Position.X, Position.Y);
        }
        /// <summary>
        ///Get position in Rectangle
        /// </summary>
        public virtual Rectangle GetRectangle()
        {
            return Position;
        }
        /// <summary>
        /// Get name
        /// </summary>
        /// <returns>Name of Entity</returns>
        public virtual string GetName()
        {
            return Name;
        }

        /// <summary>
        /// Set name
        /// </summary>
        /// <param name="Name">Name</param>
        public virtual void SetName(string Name)
        {
            this.Name = Name;
        }

        /// <summary>
        /// Base method of Update loop
        /// </summary>
        /// <param name="GameTime">Time that passed since last Update loop</param>
        public virtual void Update(GameTime GameTime)
        {
            
        }

        /// <summary>
        /// Base method to draw
        /// </summary>
        /// <param name="GameTime">Time that passed since last Update loop</param>
        public virtual void Draw(GameTime GameTime)
        {
            if (highlight)
            {
                Rectangle p = new Rectangle(Position.X - Position.Width / 2 - 4, Position.Y - Position.Height/2 - 4, Position.Width + 8, Position.Height + 8);
                Renderer.Draw(GeneralManager.Textures["GUI/hl lu"], new Rectangle(p.X, p.Y, 10, 10), Color.White, 100);
                Renderer.Draw(GeneralManager.Textures["GUI/hl ru"], new Rectangle(p.X + Position.Width - 2, p.Y, 10, 10), Color.White, 100);
                Renderer.Draw(GeneralManager.Textures["GUI/hl ld"], new Rectangle(p.X, p.Y + Position.Height - 2, 10, 10), Color.White, 100);
                Renderer.Draw(GeneralManager.Textures["GUI/hl rd"], new Rectangle(p.X + Position.Width -2, p.Y + Position.Height -2, 10, 10), Color.White, 100);
            }

            highlight = false;
        }

        public virtual string Serialize()
        {
            return "";
        }

        /// <summary>
        /// Base method for Interacting with other Entity
        /// </summary>
        /// <param name="E">Entity to Interact with</param>
        public virtual bool Interact(Entity E)
        {
            return false;
        }

        /// <summary>
        /// Base method for Colliding with other Entity
        /// </summary>
        /// <param name="F">Physics Fixture to collide</param>
        public virtual void Collide(Fixture F)
        {

        }

        /// <summary>
        /// Removing Entity
        /// </summary>
        public virtual void Dispose()
        {
            Map.Entities.Remove(this);
            if (Fixture != null)
            {
                Map.PhysicalWorld.RemoveBody(Fixture.Body);
                Fixture = null;
            }
        }

        /// <summary>
        /// Base method for hovering mouse on Entity
        /// </summary>
        /// <param name="p">Player that hovers mouse on this</param>
        public virtual void Sensor(Player p)
        {
            highlight = true;
        }

        /// <summary>
        /// Base method of using Entity
        /// </summary>
        /// <param name="P">Player that is using this Entity</param>
        public virtual void PlayerUse(Player P)
        {

        }
    }
}

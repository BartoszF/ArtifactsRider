using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VAPI;
using System.IO;
using VAPI.RenderEffects;
using ArtifactsRider.MapManager;

namespace ArtifactsRider
{
    /// <summary>
    /// Base class for Mobs. Derived from Entity.
    /// </summary>
    public abstract class Mob : Entity
    {
        public float speed = 6000f;                                     /**< Speed of mob */
        public float timer = Helper.GetRandom() % 500, maxTime = 500f;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Map">In which map, mob will be created</param>
        public Mob(Map Map) : base(Map)
        {
        }


        public abstract int GetHP();
        public abstract int GetDef();
        public abstract int GetDamage();
        public abstract int GetLucky();

        public abstract void SetHP(int hp);
        public abstract void SetDef(int d);
        public abstract void SetDamage(int d);
        public abstract void SetLucky(int l);

        public abstract void PlaySound(string name);
        public abstract void StopSound(string name);

        /// <summary>
        /// Move up method
        /// </summary>
        public virtual void MoveTop()
        {
        }

        /// <summary>
        /// Move down method
        /// </summary>
        public virtual void MoveDown()
        {
        }

        /// <summary>
        /// Move left method
        /// </summary>
        public virtual void MoveLeft()
        {
        }

        /// <summary>
        /// Move right method
        /// </summary>
        public virtual void MoveRight()
        {
        }

        /// <summary>
        /// Update loop for Mob
        /// </summary>
        /// <param name="GameTime"></param>
        public override void Update(GameTime GameTime)
        {
            Fixture.Body.LinearVelocity *= 0.9f;

            if (timer <= 0)
            {
                timer = maxTime;
            }
            else
            {
                timer -= GameTime.ElapsedGameTime.Milliseconds;
            }
            base.Update(GameTime);
        }


    }
}

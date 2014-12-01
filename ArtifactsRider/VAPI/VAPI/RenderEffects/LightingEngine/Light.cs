using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Krypton.Lights;
using Krypton;

namespace VAPI.RenderEffects
{
    public enum LightType
    {
        Point,
        Spot
    }

    public abstract class Light
    {
        protected float _initialPower;


        public Vector3 Position 
        { 
            get
            {
                return new Vector3(KryptonLight.Position.X * GeneralManager.ScreenX / 2 +1, KryptonLight.Position.Y * GeneralManager.ScreenY /-2 -1, 0) + new Vector3(GeneralManager.ScreenX/2, GeneralManager.ScreenY/2, 0);//KryptonLight.Position / new VecGeneralManager.S
            }
            set
            {
                KryptonLight.Position = new Vector2(2 * value.X / GeneralManager.ScreenX - 1, -2 * value.Y / GeneralManager.ScreenY + 1);
            }
        }

        public Vector4 Color;
       /* {
            get
            {
                return new Vector4(KryptonLight.Color.R, KryptonLight.Color.G, KryptonLight.Color.B, KryptonLight.Color.A);
            }
            set
            {
                KryptonLight.Color = new Color(value.X, value.Y , value.Z, value.W);
            }
        }*/

        public Light2D KryptonLight;

        [ContentSerializerIgnore]
        public float ActualPower { get; set; }

        /// <summary>
        /// The Power is the Initial Power of the Light
        /// </summary>
        public float Power
        {
            get { return _initialPower; }
            set
            {
                _initialPower = value;
                ActualPower = _initialPower;
                KryptonLight.Range = value * 2f;
            }
        }

        public int LightDecay { get; set; }

        [ContentSerializerIgnore]
        public LightType LightType { get; private set; }

        [ContentSerializer(Optional = true)]
        bool _IsEnabled;
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
                KryptonLight.IsOn = value;
            }
        }

        /// <summary>
        /// Gets or sets the spot direction. The axis of the cone.
        /// </summary>
        /// <value>The spot direction.</value>
        [ContentSerializer(Optional = true)]
        public Vector3 Direction
        {
            get
            {
                return new Vector3(Helper.GetVectorFromAngle(KryptonLight.Angle + (float)Math.PI / 2), 0);
            }
            set
            {
                KryptonLight.Angle = Helper.GetAngleFromVector(new Vector2(value.X, value.Y)) - (float)Math.PI / 2;
            }
        }

        protected Light(LightType lightType)
        {
            LightType = lightType;


            Texture2D LightTexture = LightTextureBuilder.CreatePointLight(Renderer.GD, 512);

            KryptonLight = new Light2D()
            {
                Texture = LightTexture,
                Range = (float)(1.5f),
                Color = new Color(0.8f, 0.8f, 0.8f, 1f),
                Intensity = 1f,
                Angle = MathHelper.TwoPi * 0.5f,
                X = (float)(0),
                Y = (float)(0),
            };
            
        }

        public void EnableLight(bool enabled, float timeToEnable)
        {
            // If the light must be turned on
            IsEnabled = enabled;
        }

        /// <summary>
        /// Updates the Light.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;
        }

        /// <summary>
        /// Copy all the base fields.
        /// </summary>
        /// <param name="light">The light.</param>
        protected void CopyBaseFields(Light light)
        {
            light.Color = this.Color;
            light.IsEnabled = this.IsEnabled;
            light.LightDecay = this.LightDecay;
            light.LightType = this.LightType;
            light.Position = this.Position;
            light.Power = this.Power;
        }

        public abstract Light DeepCopy();
    }
}
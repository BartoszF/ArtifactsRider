using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Krypton;
using Krypton.Common;
using Krypton.Lights;

namespace VAPI.RenderEffects
{
    public class LightingEngine : RenderEffect
    {
        private Krypton.KryptonEngine Krypton;

        private GameTime GameTime;

        private Dictionary<float,List<NormalMap>> Normals;

        private RenderTarget2D _colorMapRenderTarget;
        private RenderTarget2D _normalMapRenderTarget;
        private RenderTarget2D _shadowMapRenderTarget;

        private List<Light> _lights = new List<Light>();
        private Color _ambientLight = new Color(.2f, .2f, .2f, 1);
        private float _specularStrength = .8f;

        private Effect _lightEffect;
        private Effect _lightCombinedEffect;

        private EffectTechnique _lightEffectTechniquePointLight;
        private EffectTechnique _lightEffectTechniqueSpotLight;
        private EffectParameter _lightEffectParameterStrength;
        private EffectParameter _lightEffectParameterPosition;
        private EffectParameter _lightEffectParameterLightColor;
        private EffectParameter _lightEffectParameterLightDecay;
        private EffectParameter _lightEffectParameterScreenWidth;
        private EffectParameter _lightEffectParameterScreenHeight;
        private EffectParameter _lightEffectParameterNormapMap;

        private EffectParameter _lightEffectParameterConeAngle;
        private EffectParameter _lightEffectParameterConeDecay;
        private EffectParameter _lightEffectParameterConeDirection;

        private EffectTechnique _lightCombinedEffectTechnique;
        private EffectParameter _lightCombinedEffectParamAmbient;
        private EffectParameter _lightCombinedEffectParamLightAmbient;
        private EffectParameter _lightCombinedEffectParamAmbientColor;
        private EffectParameter _lightCombinedEffectParamColorMap;
        private EffectParameter _lightCombinedEffectParamShadowMap;
        private EffectParameter _lightCombinedEffectParamNormalMap;

        public VertexPositionColorTexture[] Vertices;
        public VertexBuffer VertexBuffer;

        private SpriteBatch SB;

        public void SetAmbient(Color Value, Color KryptonVal)
        {
            _ambientLight = Value;
            Krypton.AmbientColor = KryptonVal;
        }


        public LightingEngine()
        {
            PresentationParameters pp = Renderer.GD.PresentationParameters;
            SurfaceFormat format = pp.BackBufferFormat;

            Krypton = new KryptonEngine(GeneralManager.Game, "Effects/KryptonEffect");
            Krypton.Initialize();

            Krypton.AmbientColor = Color.Black; 

            Vertices = new VertexPositionColorTexture[4];
            Vertices[0] = new VertexPositionColorTexture(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0));
            Vertices[1] = new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0));
            Vertices[2] = new VertexPositionColorTexture(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1));
            Vertices[3] = new VertexPositionColorTexture(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1));
            VertexBuffer = new VertexBuffer(Renderer.GD, typeof(VertexPositionColorTexture), Vertices.Length, BufferUsage.None);
            VertexBuffer.SetData(Vertices);

            _colorMapRenderTarget = new RenderTarget2D(Renderer.GD, GeneralManager.ScreenX, GeneralManager.ScreenY);
            _normalMapRenderTarget = new RenderTarget2D(Renderer.GD, GeneralManager.ScreenX, GeneralManager.ScreenY);
            _shadowMapRenderTarget = new RenderTarget2D(Renderer.GD, GeneralManager.ScreenX, GeneralManager.ScreenY, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            _lightEffect = GeneralManager.Content.Load<Effect>("Effects/MultiTarget");
            _lightCombinedEffect = GeneralManager.Content.Load<Effect>("Effects/DeferredCombined");

            // Point light technique
            _lightEffectTechniquePointLight = _lightEffect.Techniques["DeferredPointLight"];

            // Spot light technique
            _lightEffectTechniqueSpotLight = _lightEffect.Techniques["DeferredSpotLight"];

            // Shared light properties
            _lightEffectParameterLightColor = _lightEffect.Parameters["lightColor"];
            _lightEffectParameterLightDecay = _lightEffect.Parameters["lightDecay"];
            _lightEffectParameterNormapMap = _lightEffect.Parameters["NormalMap"];
            _lightEffectParameterPosition = _lightEffect.Parameters["lightPosition"];
            _lightEffectParameterScreenHeight = _lightEffect.Parameters["screenHeight"];
            _lightEffectParameterScreenWidth = _lightEffect.Parameters["screenWidth"];
            _lightEffectParameterStrength = _lightEffect.Parameters["lightStrength"];

            // Spot light parameters
            _lightEffectParameterConeDirection = _lightEffect.Parameters["coneDirection"];
            _lightEffectParameterConeAngle = _lightEffect.Parameters["coneAngle"];
            _lightEffectParameterConeDecay = _lightEffect.Parameters["coneDecay"];

            _lightCombinedEffectTechnique = _lightCombinedEffect.Techniques["DeferredCombined2"];
            _lightCombinedEffectParamAmbient = _lightCombinedEffect.Parameters["ambient"];
            _lightCombinedEffectParamLightAmbient = _lightCombinedEffect.Parameters["lightAmbient"];
            _lightCombinedEffectParamAmbientColor = _lightCombinedEffect.Parameters["ambientColor"];
            _lightCombinedEffectParamColorMap = _lightCombinedEffect.Parameters["ColorMap"];
            _lightCombinedEffectParamShadowMap = _lightCombinedEffect.Parameters["ShadingMap"];
            _lightCombinedEffectParamNormalMap = _lightCombinedEffect.Parameters["NormalMap"];

            SB = new SpriteBatch(Renderer.GD);

            Normals = new Dictionary<float, List<NormalMap>>();

        }

        public void AddHull(ShadowHull Hull)
        {
            Krypton.Hulls.Add(Hull);
        }

        public void RemoveHull(ShadowHull Hull)
        {
            Krypton.Hulls.Remove(Hull);
        }


        public override void BeginEffect()
        {
        }

        public Light GetLight(int id)
        {
            return _lights[id];
        }

        public void RemoveLight(Light Light)
        {
            _lights.Remove(Light);
        }

        public void AddLight(Light Light)
        {
            _lights.Add(Light);

            Krypton.Lights.Add(Light.KryptonLight);
        }

        public override void EndEffect()
        {
            // Set the render targets
            Renderer.GD.SetRenderTarget(_colorMapRenderTarget);
            
            // Clear all render targets
           //Renderer.GD.Clear(Color.Transparent);

            SB.Begin();
            SB.Draw((Texture2D)Renderer.MainRenderTarget, Vector2.Zero, Color.White);
            SB.End();
            
            Renderer.GD.SetRenderTarget(_normalMapRenderTarget);

            // Clear all render targets
            //Renderer.GD.Clear(Color.Transparent);

            DrawAllNormals();

            // Deactive the rander targets to resolve them
            //Renderer.GD.SetRenderTarget(null);

            GenerateShadowMap();

            //Renderer.GD.Clear(Color.Black);

            // Finally draw the combined Maps onto the screen
            DrawCombinedMaps();
        }

        public override void UpdateEffect(GameTime GameTime)
        {
            this.GameTime = GameTime;
            Krypton.Update(GameTime);
        }

        public void DrawAllNormals()
        {

            SB.Begin();
            //foreach (NormalMap NM in Normals)
            var list = Normals.Keys.ToList();
            list.Sort();

            foreach (var z in list)
            {
                foreach (NormalMap NM in Normals[z])
                {
                    if (NM.Region == null)
                    {
                        if (NM.Position.Width == 0 && NM.Position.Height == 0)
                        {
                            SB.Draw(NM.Normal, new Vector2(NM.Position.X, NM.Position.Y) - new Vector2(Camera.GetRect.X, Camera.GetRect.Y), null, Color.White, NM.Angle, NM.RotationCenter, 1f, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            SB.Draw(NM.Normal, Helper.SubRectPos(NM.Position, Camera.GetRect), null, Color.White, NM.Angle, NM.RotationCenter, SpriteEffects.None, 0f);
                        }
                    }
                    else
                    {
                        if (NM.Position.Width == 0 && NM.Position.Height == 0)
                        {
                            SB.Draw(NM.Normal, new Vector2(NM.Position.X, NM.Position.Y) - new Vector2(Camera.GetRect.X, Camera.GetRect.Y), NM.Region, Color.White, NM.Angle, NM.RotationCenter, 1f, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            SB.Draw(NM.Normal, Helper.SubRectPos(NM.Position, Camera.GetRect), NM.Region, Color.White, NM.Angle, NM.RotationCenter, SpriteEffects.None, 0f);
                        }
                    }
                }
            }


            SB.End();


            Normals = new Dictionary<float, List<NormalMap>>();
        }

        private void DrawCombinedMaps()
        {
            Matrix view = Matrix.CreateTranslation(new Vector3(-2f * (float)Camera.GetRect.X / (float)GeneralManager.ScreenX, 2f * (float)Camera.GetRect.Y / (float)GeneralManager.ScreenY, 0));


            _lightCombinedEffect.CurrentTechnique = _lightCombinedEffectTechnique;
            _lightCombinedEffectParamAmbient.SetValue(1f);
            _lightCombinedEffectParamLightAmbient.SetValue(4);
            _lightCombinedEffectParamAmbientColor.SetValue(_ambientLight.ToVector4());
            _lightCombinedEffectParamColorMap.SetValue(_colorMapRenderTarget);
            _lightCombinedEffectParamShadowMap.SetValue(_shadowMapRenderTarget);
            _lightCombinedEffectParamNormalMap.SetValue(_normalMapRenderTarget);
            _lightCombinedEffect.CurrentTechnique.Passes[0].Apply();


            Renderer.GD.SetRenderTarget(Renderer.MainRenderTarget);

            SB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, _lightCombinedEffect);
            SB.Draw(_colorMapRenderTarget, Vector2.Zero, Color.White);
            Krypton.Bluriness = 15;
            Krypton.Matrix = view;
            Krypton.LightMapPrepare();
            Krypton.Draw(GameTime);
            SB.End();
        }

        private Texture2D GenerateShadowMap()
        {
            Renderer.GD.SetRenderTarget(_shadowMapRenderTarget);
            Renderer.GD.Clear(Color.Transparent);

            //BoundingRect CameraBR = new BoundingRect();
            //CameraBR.Min = new Vector2(2f * (float)Camera.GetRect.X / (float)GeneralManager.ScreenX,2f * (float)Camera.GetRect.Y / (float)GeneralManager.ScreenY) + new Vector2(-1, -1);
            //CameraBR.Max = new Vector2(2f * (float)(Camera.GetRect.X + Camera.GetRect.Width) / (float)GeneralManager.ScreenX, 2f * (float)(Camera.GetRect.Y + Camera.GetRect.Height) / (float)GeneralManager.ScreenY) + new Vector2(-1, -1);


            foreach (var light in _lights)
            {
                if (!light.IsEnabled) continue;
                //if (!light.KryptonLight.Bounds.Intersects(CameraBR)) continue;

                Renderer.GD.SetVertexBuffer(VertexBuffer);

                // Draw all the light sources
                _lightEffectParameterStrength.SetValue(light.ActualPower);
                _lightEffectParameterPosition.SetValue(light.Position + new Vector3(-Camera.GetRect.X, -Camera.GetRect.Y, 0));
                _lightEffectParameterLightColor.SetValue(light.Color);
                _lightEffectParameterLightDecay.SetValue(light.LightDecay); // Value between 0.00 and 2.00
                _lightEffect.Parameters["specularStrength"].SetValue(_specularStrength);

                if (light.LightType == LightType.Point)
                {
                    _lightEffect.CurrentTechnique = _lightEffectTechniquePointLight;
                }
                else
                {
                    _lightEffect.CurrentTechnique = _lightEffectTechniqueSpotLight;
                    _lightEffectParameterConeAngle.SetValue(((SpotLight)light).SpotAngle);
                    _lightEffectParameterConeDecay.SetValue(((SpotLight)light).SpotDecayExponent);
                    _lightEffectParameterConeDirection.SetValue(((SpotLight)light).Direction);
                }

                _lightEffectParameterScreenWidth.SetValue(GeneralManager.ScreenX);
                _lightEffectParameterScreenHeight.SetValue(GeneralManager.ScreenY);
                _lightEffect.Parameters["ambientColor"].SetValue(_ambientLight.ToVector4());
                _lightEffectParameterNormapMap.SetValue(_normalMapRenderTarget);
                _lightEffect.Parameters["ColorMap"].SetValue(_colorMapRenderTarget);
                _lightEffect.CurrentTechnique.Passes[0].Apply();

                // Add Belding (Black background)
                Renderer.GD.BlendState = BlendBlack;

                // Draw some magic
                Renderer.GD.DrawUserPrimitives(PrimitiveType.TriangleStrip, Vertices, 0, 2);
            }

            // Deactive the rander targets to resolve them
            Renderer.GD.SetRenderTarget(null);

            return _shadowMapRenderTarget;
        }

        public static BlendState BlendBlack = new BlendState()
        {
            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,

            AlphaBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.One
        };

        public override void Dispose()
        {
            _colorMapRenderTarget.Dispose();
            _shadowMapRenderTarget.Dispose();
            _normalMapRenderTarget.Dispose();
            
        }

        public void AddNormal(NormalMap NM, float Z)
        {
            if (Normals.ContainsKey(Z))
            {
                Normals[Z].Add(NM);
            }
            else
            {
                Normals.Add(Z, new List<NormalMap>());
                Normals[Z].Add(NM);
            }
        }

        public void DrawNormal(Texture2D Tex, Vector2 Pos)
        {
            NormalMap NM = new NormalMap();
            NM.Normal = Tex;
            NM.Position = new Rectangle((int)Pos.X, (int)Pos.Y, 0, 0);
            AddNormal(NM, 0);
            //Normals.Add(NM);
        }

        public void DrawNormal(Texture2D Tex, Rectangle Pos)
        {
            NormalMap NM = new NormalMap();
            NM.Normal = Tex;
            NM.Position = Pos;
            AddNormal(NM, 0);
            //Normals.Add(NM);
        }

        public void DrawNormal(Texture2D Tex, Vector2 Pos, Rectangle? Region)
        {
            NormalMap NM = new NormalMap();
            NM.Normal = Tex;
            NM.Position = new Rectangle((int)Pos.X, (int)Pos.Y, 0, 0);
            NM.Region = Region;
            AddNormal(NM, 0);
            //Normals.Add(NM);
        }

        public void DrawNormal(Texture2D Tex, Rectangle Pos, Rectangle? Region)
        {
            NormalMap NM = new NormalMap();
            NM.Normal = Tex;
            NM.Position = Pos;
            NM.Region = Region;
            AddNormal(NM, 0);
            //Normals.Add(NM);
        }

        public void DrawNormal(Texture2D Tex, Vector2 Pos, Rectangle? Region, float Angle, Vector2 Center)
        {
            NormalMap NM = new NormalMap();
            NM.Normal = Tex;
            NM.Position = new Rectangle((int)Pos.X, (int)Pos.Y, 0, 0);
            NM.Region = Region;
            NM.Angle = Angle;
            NM.RotationCenter = Center;
            AddNormal(NM, 0);
            //Normals.Add(NM);
        }

        public void DrawNormal(Texture2D Tex, Rectangle Pos, Rectangle? Region, float Angle, Vector2 Center)
        {
            NormalMap NM = new NormalMap();
            NM.Normal = Tex;
            NM.Position = Pos;
            NM.Region = Region;
            NM.Angle = Angle;
            NM.RotationCenter = Center;
            AddNormal(NM, 0);
            //Normals.Add(NM);
        }

        public void DrawNormal(Texture2D Tex, Rectangle Pos, Rectangle? Region, float Angle, Vector2 Center, float Z)
        {
            NormalMap NM = new NormalMap();
            NM.Normal = Tex;
            NM.Position = Pos;
            NM.Region = Region;
            NM.Angle = Angle;
            NM.RotationCenter = Center;
            AddNormal(NM, Z);
           // Normals.Add(NM);
        }
    }

    public struct NormalMap
    {
        public Texture2D Normal;
        public Rectangle Position;
        public float Angle;
        public Vector2 RotationCenter;
        public Rectangle? Region;
    }
}

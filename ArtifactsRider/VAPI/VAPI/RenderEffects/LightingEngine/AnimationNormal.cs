using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VAPI.RenderEffects
{
    public class AnimationNormal
    {
        public Vector2 FrameSize;
        public int CurrentFrame;
        public int MaxFrames;
        public int MilisecondsPerFrame;
        public int CurrentTimeCounter;
        public float Angle;
        public Vector2 Center;

        public Rectangle Position;

        Texture2D BaseTex;
        List<Rectangle> Regions;
        LightingEngine LE;

        public void Load(string TexName, Vector2 FrameSize, int MaxFrames, int MsPerFrame, LightingEngine LE)
        {
            this.FrameSize = FrameSize;
            CurrentFrame = 0;
            this.MaxFrames = MaxFrames;
            BaseTex = GeneralManager.Content.Load<Texture2D>("Textures/Animations/" + TexName);
            MilisecondsPerFrame = MsPerFrame;
            Regions = new List<Rectangle>();

            int SizeX = (BaseTex.Width / (int)FrameSize.X);
            int SizeY = (BaseTex.Height / (int)FrameSize.Y);

            for (int i = 0; i < MaxFrames; i++)
            {
                Regions.Add(new Rectangle((i % SizeX) * (int)FrameSize.X, (i / SizeX) * (int)FrameSize.Y, (int)FrameSize.X, (int)FrameSize.Y));
            }

            this.LE = LE;
        }

        public void Draw(GameTime GameTime)
        {
            LE.DrawNormal(BaseTex, Position, Regions[CurrentFrame], Angle, Center);
            //Renderer.Draw(BaseTex, Position, Regions[CurrentFrame], Angle, Center);
        }

        public void Draw(GameTime GameTime, float Z)
        {
            LE.DrawNormal(BaseTex, Position, Regions[CurrentFrame], Angle, Center, Z);
            //Renderer.Draw(BaseTex, Position, Regions[CurrentFrame], Angle, Center);
        }

        public void Update(GameTime GameTime)
        {
            CurrentTimeCounter += GameTime.ElapsedGameTime.Milliseconds;
            while (CurrentTimeCounter >= MilisecondsPerFrame)
            {
                CurrentTimeCounter -= MilisecondsPerFrame;
                NextFrame();
            }
        }

        void NextFrame()
        {
            CurrentFrame = (CurrentFrame + 1) % MaxFrames;
        }
    }
}

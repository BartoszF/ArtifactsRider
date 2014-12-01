using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAPI;
using VAPI.RenderEffects;
using Microsoft.Xna.Framework;

namespace ArtifactsRider.MapManager.Entities
{
    class FloorFan : Entity
    {
        Animation FanAnim;
        AnimationNormal FanNormalAnim;

        public FloorFan(Rectangle r, Map m)
            : base(m)
        {

            LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");

            FanAnim = new Animation();
            FanNormalAnim = new AnimationNormal();

            FanAnim.Load("FloorFan", Vector2.One * 128, 6, 50);
            FanNormalAnim.Load("FloorFan_normal", Vector2.One * 128, 6, 50, LE);


            Position = r;
            FanAnim.Position = Position;
            FanNormalAnim.Position = Position;

            SetName("FloorFan");
        }

        public override string GetName()
        {
            return Name;
        }

        public override void SetName(string Name)
        {
            this.Name = Name;
        }

        public override void Update(GameTime GameTime)
        {
            FanAnim.Update(GameTime);
            FanNormalAnim.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");

            FanAnim.Draw(GameTime, 0.8f);
            FanNormalAnim.Draw(GameTime, 0.8f);
        }

        public override string Serialize()
        {
            return "";
        }

        public override bool Interact(Entity E)
        {
            return base.Interact(E);
        }
    }
}

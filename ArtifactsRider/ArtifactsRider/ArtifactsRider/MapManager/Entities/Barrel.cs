using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using VAPI.RenderEffects;
using VAPI;

namespace ArtifactsRider.MapManager.Entities
{
    public class Barrel : Entity
    {
        public Barrel(Rectangle r, Map m)
            : base(m)
        {
            Body BodyDec = new Body(m.PhysicalWorld);
            BodyDec.BodyType = BodyType.Dynamic;

            Fixture = BodyDec.CreateFixture(new CircleShape(15f, 100.5f));
            Fixture.Restitution = 1f;
            Fixture.Friction = 10f;

            Position = r;

            SetName("barrel");
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
            Fixture.Body.LinearVelocity *= 0.9f;
            Fixture.Body.AngularVelocity *= 0.95f;
        }

        public override void Draw(GameTime GameTime)
        {
            LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");

            Renderer.Draw(GeneralManager.Textures["Entities/" + GetName()], Position, Color.White,Fixture.Body.Rotation,new Vector2(Position.Width/2,Position.Height/2));
            LE.DrawNormal(GeneralManager.Textures["Entities/" + GetName() + "_normal"], Position,null, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));
        }

        public override string Serialize()
        {
            return "";
        }
    }
}

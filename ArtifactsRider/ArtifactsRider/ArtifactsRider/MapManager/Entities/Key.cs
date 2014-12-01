using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ArtifactsRider.MapManager;
using VAPI;
using VAPI.RenderEffects;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;

namespace ArtifactsRider.Items
{
    public class Key : Entity
    {
        public Light light;
        public float angle = 0;
        public float power = 0.6f;

        public bool pickedUp = false;

        public Key(Rectangle pos, Map map)
            : base(map)
        {
            Body BodyDec = new Body(map.PhysicalWorld);
            BodyDec.BodyType = BodyType.Static;

            PolygonShape S = new PolygonShape(1f);
            S.SetAsBox(pos.Width / 2, pos.Height / 2);

            Fixture = BodyDec.CreateFixture(S);
            Fixture.Restitution = 1f;
            Fixture.Friction = 10f;

            this.Position = pos;

            SetName("Key");

            LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");

            light = new PointLight()
            {
                IsEnabled = true,
                Color = new Vector4(0.95f, .7f, .05f, 1f),
                Power = power,
                LightDecay = 350,
                Position = new Vector3(Position.X, Position.Y, 20),
                Direction = new Vector3(0,0, 0)
            };

            LE.AddLight(light);
        }

        public override void Update(GameTime GameTime)
        {
            if (!pickedUp)
            {
                angle = angle + (angle * (float)Math.Sin(GameTime.TotalGameTime.Seconds));
                power = power + (power * (float)Math.Sin(GameTime.TotalGameTime.Seconds));

                light.Power = power;
                Fixture.Body.Rotation = angle;
                base.Update(GameTime);
            }
        }

        public override void Draw(GameTime GameTime)
        {
            if (!pickedUp)
            {
                LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");

                Renderer.Draw(GeneralManager.Textures["Entities/" + GetName()], Position, Color.White, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));
                LE.DrawNormal(GeneralManager.Textures["Entities/" + GetName() + "_normal"], Position, null, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));

                base.Draw(GameTime);
            }
        }

        public override void PlayerUse(Player P)
        {
            pickedUp = true;
            P.showEndMsg = true;
            LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");
            LE.RemoveLight(light);
            base.PlayerUse(P);
        }

        public override void Sensor(Player p)
        {
            base.Sensor(p);
        }
    }
}

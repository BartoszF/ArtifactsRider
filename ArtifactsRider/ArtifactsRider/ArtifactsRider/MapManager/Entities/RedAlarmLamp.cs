using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VAPI;
using VAPI.RenderEffects;

namespace ArtifactsRider.MapManager.Entities
{
    class RedAlarmLamp : Entity
    {
        Light AlertLight;
        float Angle = 0f;

        public RedAlarmLamp(Rectangle r, Map m)
            : base(m)
        {
            Body BodyDec = new Body(m.PhysicalWorld);
            BodyDec.BodyType = BodyType.Static;

            PolygonShape S = new PolygonShape(1f);
            S.SetAsBox(r.Width / 2, r.Height / 2);

            Fixture = BodyDec.CreateFixture(S);
            Fixture.Restitution = 1f;
            Fixture.Friction = 10f;

            Position = r;

            SetName("RedAlarmLamp");


            AlertLight = new SpotLight()
            {
                IsEnabled = true,
                Color = new Vector4(0.9f, .1f, .1f, 1f),
                Power = .6f,
                LightDecay = 600,
                Position = new Vector3(r.X, r.Y, 20),
                SpotAngle = 1.5f,
                SpotDecayExponent = 3,
                Direction = new Vector3(0.244402379f, 0.969673932f, 0)
            };

            LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");
            LE.AddLight(AlertLight);
            
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
            AlertLight.Direction = new Vector3(Helper.GetVectorFromAngle(Angle), 10f);
            Angle += 0.1f;
        }

        public override void Draw(GameTime GameTime)
        {
            LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");

            if (AlertLight.IsEnabled)
            {
                Renderer.Draw(GeneralManager.Textures["Entities/" + GetName()], Position, Color.White, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));
            }
            else
            {
                Renderer.Draw(GeneralManager.Textures["Entities/" + GetName() + "Broken"], Position, Color.White, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));
            }
            LE.DrawNormal(GeneralManager.Textures["Entities/" + GetName() + "_normal"], Position,null, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));
        }

        public override string Serialize()
        {
            return "";
        }

        public override bool Interact(Entity E)
        {
            if (E is Bullet)
            {
                AlertLight.IsEnabled = false;
                SoundEngine.PlaySound(GetPosition(), "Content/Sounds/Entities/bulbsmash.mp3");
                return true;
            }
            return base.Interact(E);
        }
    }
}

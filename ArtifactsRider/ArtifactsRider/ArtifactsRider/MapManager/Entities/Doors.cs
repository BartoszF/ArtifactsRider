using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VAPI;
using VAPI.RenderEffects;
using FarseerPhysics.Common;

namespace ArtifactsRider.MapManager.Entities
{
    class Door : Entity
    {
        private bool _Opened;
        public bool Opened
        {
            get
            {
                return _Opened;
            }
            set
            {
                Rectangle R = Position;
                float Rotation = Fixture.Body.Rotation;
                if (value)
                {
                    Map.PhysicalWorld.RemoveBody(Fixture.Body);
                    Fixture = null;

                    Body BodyDec = new Body(Map.PhysicalWorld);
                    BodyDec.BodyType = BodyType.Static;

                    PolygonShape S = new PolygonShape(1f);
                    Vertices V = new Vertices();
                    V.Add(new Vector2(-32, -11));
                    V.Add(new Vector2(-30, -11));
                    V.Add(new Vector2(-30, 11));
                    V.Add(new Vector2(-32, 11));
                    S.Set(V);

                    Fixture = BodyDec.CreateFixture(S);
                    Fixture.Restitution = 1f;
                    Fixture.Friction = 10f;

                    SoundEngine.PlaySound(this.GetPosition(), "door_close.mp3",200f);
                }
                else
                {
                    Map.PhysicalWorld.RemoveBody(Fixture.Body);
                    Fixture = null;

                    Body BodyDec = new Body(Map.PhysicalWorld);
                    BodyDec.BodyType = BodyType.Static;

                    PolygonShape S = new PolygonShape(1f);
                    S.SetAsBox(32, 11);

                    Fixture = BodyDec.CreateFixture(S);
                    Fixture.Restitution = 1f;
                    Fixture.Friction = 10f;

                    SoundEngine.PlaySound(this.GetPosition(), "door_open.mp3",200f);
                }

                Position = R;
                Fixture.Body.Rotation = Rotation;
                _Opened = value;
            }
        }

        public DoorButton DoorButton;

        public Door(Rectangle r, Map m)
            : base(m)
        {
            Body BodyDec = new Body(m.PhysicalWorld);
            BodyDec.BodyType = BodyType.Static;

            PolygonShape S = new PolygonShape(1f);
            S.SetAsBox(r.Width / 2, 11);

            Fixture = BodyDec.CreateFixture(S);
            Fixture.Restitution = 1f;
            Fixture.Friction = 10f;

            Position = r;

            SetName("Door");
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
        }

        public override void Draw(GameTime GameTime)
        {
            LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");

            if (!Opened)
            {
                Renderer.Draw(GeneralManager.Textures["Entities/" + GetName() + "Closed"], Position, Color.White, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));
                LE.DrawNormal(GeneralManager.Textures["Entities/" + GetName() + "Closed" + "_normal"], Position, null, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));
            }
            else
            {
                Renderer.Draw(GeneralManager.Textures["Entities/" + GetName() + "Open"], Position, Color.White, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));
                LE.DrawNormal(GeneralManager.Textures["Entities/" + GetName() + "Open" + "_normal"], Position, null, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));
            }
        }

        public override string Serialize()
        {
            return "";
        }
    }
}

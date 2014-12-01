using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VAPI;
using VAPI.RenderEffects;

namespace ArtifactsRider.MapManager.Entities
{
    class DoorButton : Entity
    {
        public Door Doors; 

        public DoorButton(Rectangle r, Map m)
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

            SetName("DoorButton");
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

            Renderer.Draw(GeneralManager.Textures["Entities/" + GetName()], Position, Color.White,Fixture.Body.Rotation,new Vector2(Position.Width/2,Position.Height/2));
            LE.DrawNormal(GeneralManager.Textures["Entities/" + GetName() + "_normal"], Position,null, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));

            base.Draw(GameTime); //We must do it, if we want to draw highlight on Entity
        }

        public override string Serialize()
        {
            return "";
        }

        public override bool Interact(Entity E)
        {
            return base.Interact(E);
        }

        public override void Sensor(Player p)
        {
            base.Sensor(p);
        }

        public override void PlayerUse(Player P)
        {
            Doors.Opened = !Doors.Opened;
            base.PlayerUse(P);
        }
    }
}

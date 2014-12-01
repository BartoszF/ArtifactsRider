using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VAPI.RenderEffects;
using VAPI;

namespace ArtifactsRider.MapManager.Entities
{
    public class Bullet : Entity
    {
        Entity Owner;
        int LifeLeft;

        public Bullet(Map Map, Entity Owner)
            : base(Map)
        {
            LifeLeft = 1000;

            Body BodyDec = new Body(Map.PhysicalWorld);
            BodyDec.BodyType = BodyType.Dynamic;
            BodyDec.IsBullet = true;

            this.SetName("bullet");

            Fixture = BodyDec.CreateFixture(new CircleShape(0.6f, 0.5f));
            //Fixture.Restitution = 0f;

            if (Owner.Fixture != null)
            {
                Fixture.IgnoreCollisionWith(Owner.Fixture);
            }

            this.Owner = Owner;
        }

        public override bool Interact(Entity E)
        {
            if (E is Mob && E != Owner)
            {
                (E as Mob).Dispose();
                Dispose();
                return true;
            }
            return base.Interact(E);
        }

        public override void Update(GameTime GameTime)
        {
            LifeLeft -= GameTime.ElapsedGameTime.Milliseconds;

            if (LifeLeft <= 0)
            {
                Dispose();
                return;
            }

            
            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");

            Renderer.Draw(GeneralManager.Textures["Particles/" + GetName()], Position, Color.White, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2),10);
            LE.DrawNormal(GeneralManager.Textures["Particles/" + GetName() + "_normal"], Position, null, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2),10);

            base.Draw(GameTime);
        }

        public override void Collide(Fixture F)
        {
            if (!F.IsSensor)
            {
                LifeLeft = -1;
                base.Collide(F);
            }
        }

    }
}

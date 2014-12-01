using System;
using System.Collections.Generic;
using ArtifactsRider.MapManager;
using ArtifactsRider.Scenes;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using IrrKlang;
using Microsoft.Xna.Framework;
using VAPI;
using VAPI.Algorithms;
using VAPI.RenderEffects;

namespace ArtifactsRider.Mobs
{
    class ZombieBig : Mob
    {
        List<PathFinderNode> nodes;
        Dictionary<string, ISound> sounds;

        int hp, def, damage, lucky;

        Animation walkAnim;
        AnimationNormal walkNormalAnim;

        public ZombieBig(Rectangle pos, Map m)
            : base(m)
        {
            Body BodyDec = new Body(m.PhysicalWorld);
            BodyDec.BodyType = BodyType.Dynamic;
            speed = 12000f;

            Fixture = BodyDec.CreateFixture(new CircleShape(22f, 1.5f));
            //Fixture.Restitution = 0f;

            walkAnim = new Animation();
            walkNormalAnim = new AnimationNormal();

            walkAnim.Load("ZombieBig", Vector2.One * 64, 21, 40);
            walkNormalAnim.Load("ZombieBig_normal", Vector2.One * 64, 21, 40, (LightingEngine)Renderer.GetRenderEffect("LightingEngine"));

            walkAnim.Center = Vector2.One * 32;
            walkNormalAnim.Center = Vector2.One * 32;

            Position = pos;
            hp = 100;
            def = 1;
            damage = 2;
            lucky = 1;

            SetName("zombieBig");

            sounds = new Dictionary<string, ISound>();

            //sounds.Add("Sounds/Mobs/zombie_walk", GeneralManager.Sounds["Sounds/Mobs/zombie_walk"].CreateInstance());
            //sounds.Add("zombie_moan", GeneralManager.Sounds["Sounds/Mobs/zombie_moan"].CreateInstance());

            //pfinder = new PathFinderFast(m.GetChunk().GetCostArray());
        }


        public override int GetHP() { return hp; }
        public override int GetDef() { return def; }
        public override int GetDamage() { return damage; }
        public override int GetLucky() { return lucky; }

        public override void SetHP(int hp) { this.hp = hp; }
        public override void SetDef(int d) { this.def = d; }
        public override void SetDamage(int d) { this.damage = d; }
        public override void SetLucky(int l) { this.lucky = l; }

        public override void PlaySound(string name)
        {
            if (!sounds.ContainsKey(name))
            {
                sounds.Add(name, SoundEngine.PlaySound(this.GetPosition(), name));
            }
            else
            {
                sounds[name] = SoundEngine.PlaySound(this.GetPosition(), name);
            }
        }

        public override void StopSound(string name)
        {
            sounds[name].Stop();
        }

        public bool PlayingSound(string name)
        {
            if (!sounds.ContainsKey(name)) return false;
            else return !sounds[name].Finished;
        }

        public override bool Interact(Entity E)
        {
            if (E is Player)
            {
                Mob m = E as Mob;
                if (m.GetName() == "player")
                {
                    m.SetHP(m.GetHP() - m.GetDamage());
                }
                E.Fixture.Body.ApplyLinearImpulse(new Vector2(Position.X - E.Position.X, Position.Y - E.Position.Y) * -10000f);
                return true;
            }

            return base.Interact(E);
        }


        public override void Update(GameTime gameTime)
        {
            walkAnim.Update(gameTime);
            walkNormalAnim.Update(gameTime);

            walkAnim.Angle = -Helper.GetAngleFromVector(Fixture.Body.LinearVelocity) ;
            walkNormalAnim.Angle = walkAnim.Angle;

            if ((walkAnim.CurrentFrame == 1 || walkAnim.CurrentFrame == 4) && (Math.Abs(Map.Player.Position.X - Position.X) < 1000) && (Math.Abs(Map.Player.Position.Y - Position.Y) < 1000) && !PlayingSound("Content/Sounds/Mobs/zombie_walk.wav"))
            {
                //PlaySound("zombie_walk.wav");
                PlaySound("Content/Sounds/Mobs/zombie_walk.wav");
            }

            if (timer <= 1 && (Math.Abs(Map.Player.Position.X - Position.X) < 1000) && (Math.Abs(Map.Player.Position.Y - Position.Y) < 1000) )
            {
                Point start = new Point(Position.X / Map.GetTileSize(), Position.Y / Map.GetTileSize());
                Point end = new Point(Map.Player.GetRectangle().X / Map.GetTileSize(), Map.Player.GetRectangle().Y / Map.GetTileSize());

                var RetNodes = Map.pfinder.FindPath(start, end);
                if (RetNodes != null)
                {
                    nodes = new List<PathFinderNode>(RetNodes);
                }
                else
                {
                    nodes = null;
                }
            }

            if (nodes != null && nodes.Count > 1)
            {
                int c = nodes.Count - 2;


                if (Position.X < nodes[c].X * Map.GetTileSize() + 32) MoveRight();
                else if (Position.X > nodes[c].X * Map.GetTileSize() + 32) MoveLeft();
                if (Position.Y < nodes[c].Y * Map.GetTileSize() + 32) MoveDown();
                else if (Position.Y > nodes[c].Y * Map.GetTileSize() + 32) MoveTop();
                //Position = new Rectangle(nodes[0].X * Map.GetTileSize(), nodes[0].Y * Map.GetTileSize(), Position.Width, Position.Height);
            }
            else if (nodes != null && nodes.Count <= 1)
            {
                if (Map.Player.Position.X < Position.X) MoveLeft();
                else if(Map.Player.Position.X > Position.X) MoveRight();

                if (Map.Player.Position.Y < Position.Y) MoveTop();
                else if(Map.Player.Position.Y > Position.Y) MoveDown();
            }


            base.Update(gameTime); 
        }

        public override void Draw(GameTime gameTime)
        {
            //LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");

            //Renderer.Draw(GeneralManager.Textures[texName], Position);
            //LE.DrawNormal(GeneralManager.Textures[normalName], Position);
            walkAnim.Position = Position;
            walkNormalAnim.Position = Position;

            walkAnim.Draw(gameTime,1.1f);
            walkNormalAnim.Draw(gameTime,1.1f);

            if (WorldScreen.debug && nodes != null)
            {
                foreach (PathFinderNode p in nodes)
                {
                    Renderer.Draw(GeneralManager.Textures["Editor/vertex"], new Vector2(p.X * 64 + 32, p.Y * 64 + 32));
                }
            }
        }

        public override string Serialize()
        {
            string s = typeof(Zombie).Name + " " + Position.X + " " + Position.Y + " " + Position.Width + " " + Position.Height + " "+ hp + " " + def + " " + damage + " " + lucky;
            return s;
        }

        public override void MoveDown()
        {
            Fixture.Body.ApplyLinearImpulse(new Vector2(0, speed));
            base.MoveDown();
        }

        public override void MoveTop()
        {
            Fixture.Body.ApplyLinearImpulse(new Vector2(0, -speed));
            base.MoveTop();
        }

        public override void MoveLeft()
        {
            Fixture.Body.ApplyLinearImpulse(new Vector2(-speed, 0));
            base.MoveLeft();
        }

        public override void MoveRight()
        {
            Fixture.Body.ApplyLinearImpulse(new Vector2(speed, 0));
            base.MoveRight();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VAPI.RenderEffects;
using VAPI;
using ArtifactsRider.MapManager;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using ArtifactsRider.MapManager.Entities;
using FarseerPhysics.Dynamics.Contacts;
using ArtifactsRider.Items;
using ArtifactsRider.Items.Weapons;
using System.Diagnostics;

namespace ArtifactsRider
{
    [Serializable]
    public class Player : Mob
    {
        [NonSerialized]
        public Map map;

        int hp, def, damage, lucky;
        string texName, normalName;

        public Stopwatch stoper;

        public Animation WalkAnim;
        public AnimationNormal WalkNormalAnim;

        public Animation IdleAnim;
        public AnimationNormal IdleNormalAnim;

        public Light ShootLight;

        public bool IsWalking;

        Weapon weapon;

        public Key key;
        public bool showEndMsg = false;

        public Fixture PlayerSensor;

        public Player(Rectangle pos, Map m) : base(m)
        {
            Body BodyDec = new Body(m.PhysicalWorld);
            BodyDec.BodyType = BodyType.Dynamic;

            Fixture = BodyDec.CreateFixture(new CircleShape(15f, 1.0f));

            Body BodyDecSensor = new Body(m.PhysicalWorld);
            BodyDecSensor.BodyType = BodyType.Dynamic;

            PlayerSensor = BodyDecSensor.CreateFixture(new CircleShape(15f, .0f));
            PlayerSensor.Body.IsSensor = true;
            PlayerSensor.OnCollision += OnSensorCollide;

            this.Name = "player";
            this.texName = "Mobs/" + Name;
            this.normalName = "Mobs/" + Name + "_normal";

            this.Position = pos;
            hp = 100;
            def = 10;
            damage = 5;
            lucky = 123135;
            this.map = m;
            speed = 15500f;

            weapon = new AK47();

            IdleAnim = new Animation();
            IdleNormalAnim = new AnimationNormal();

            WalkAnim = new Animation();
            WalkNormalAnim = new AnimationNormal();

            IdleAnim = GeneralManager.Animations["PlayerIdle"];
            IdleAnim.Center = Vector2.One * 32;

            IdleNormalAnim.Load("PlayerIdle_normal", Vector2.One * 64, 1, 10000, (LightingEngine)Renderer.GetRenderEffect("LightingEngine"));
            IdleNormalAnim.Center = Vector2.One * 32;

            WalkAnim = GeneralManager.Animations["TestAnim"];
            WalkAnim.Center = Vector2.One * 32;

            WalkNormalAnim.Load("TestAnim_normal", Vector2.One * 64, 6, 80, (LightingEngine)Renderer.GetRenderEffect("LightingEngine"));
            WalkNormalAnim.Center = Vector2.One * 32;

            LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");

            ShootLight = new SpotLight()
            {
                IsEnabled = true,
                Color = new Vector4(0.95f, .7f, .05f, 1f),
                Power = .6f,
                LightDecay = 600,
                Position = new Vector3(500, 400, 20),
                SpotAngle = 2f * 3.1415f,
                SpotDecayExponent = 3,
                Direction = new Vector3(0.244402379f, 0.969673932f, 0)
            };

            LE.AddLight(ShootLight);

            stoper = new Stopwatch();
            stoper.Start();
            
        }

        bool OnSensorCollide(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (Vector2.Distance(PlayerSensor.Body.Position, Fixture.Body.Position) <= 60)
            {
                if (fixtureA == PlayerSensor)
                {
                    if (fixtureB.UserData != null)
                    {
                        (fixtureB.UserData as Entity).Sensor(this);
                    }
                }

                if (GeneralManager.CheckKeyEdge(Keys.E))
                {
                    if (fixtureA == PlayerSensor)
                    {
                        if (fixtureB.UserData != null)
                        {
                            (fixtureB.UserData as Entity).PlayerUse(this);
                        }
                    }
                    else
                    {
                        if (fixtureA.UserData != null)
                        {
                            (fixtureA.UserData as Entity).PlayerUse(this);
                        }
                    }
                }
            }

            return false;
        }

        public override Rectangle GetRectangle()
        {
            return Position;
        }
        public override string GetName()
        {
            return Name;
        }

        public override void PlaySound(string name)
        {
               SoundEngine.PlaySound(this.GetPosition(), name);
        }
        public override void StopSound(string name)
        {

        }

        public override int GetHP() { return hp; }
        public override int GetDef() { return def; }
        public override int GetDamage() { return damage; }
        public override int GetLucky() { return lucky; }

        public override void SetHP(int hp) { this.hp = hp; }
        public override void SetDef(int d) { this.def = d; }
        public override void SetDamage(int d) { this.damage = d; }
        public override void SetLucky(int l) { this.lucky = l; }

        public bool HandleInput()
        {
            bool Result = false;
            if (hp > 0)
            {
                if (GeneralManager.CheckKey(Keys.D))
                {
                    MoveRight();
                    Result = true;
                }
                else if (GeneralManager.CheckKey(Keys.A))
                {
                    MoveLeft();
                    Result = true;
                }

                if (GeneralManager.CheckKey(Keys.S))
                {
                    MoveDown();
                    Result = true;
                }
                else if (GeneralManager.CheckKey(Keys.W))
                {
                    MoveTop();
                    Result = true;
                }

                if (GeneralManager.IsLMBClicked())
                {
                    ShootBullet(new Vector2(Camera.GetRect.X + GeneralManager.MousePos.X, Camera.GetRect.Y + GeneralManager.MousePos.Y));
                    Result = true;
                }
            }

            return Result;
        }

        public void ShootBullet(Vector2 ShootPos)
        {
            if (weapon.actCD <= 0)
            {
                Bullet B = new Bullet(Map, this);
                
                Vector2 ShootForce = (ShootPos - Helper.GetTopLeftFromRect(Position));
                ShootForce.Normalize();
                ShootForce *= 1000f * weapon.damageValue;

                Vector2 Offset = Helper.RotateVector(-Helper.GetAngleFromVector(ShootForce), new Vector2(-15, 0), Vector2.Zero);
                B.Position = new Rectangle(Position.X + (int)Offset.X, Position.Y + (int)Offset.Y, 10, 50);

                B.Fixture.Body.ApplyLinearImpulse(ShootForce);
                B.Fixture.Body.Rotation = -Helper.GetAngleFromVector(ShootForce) + (float)Math.PI;

                ShootLight.Power = 3f;

                weapon.actCD = weapon.cooldown;
                PlaySound("pistol_shoot.wav");
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (GeneralManager.CheckKeyEdge(Keys.Escape)) GeneralManager.Game.Exit();
            if (hp > 0)
            {
                IsWalking = Fixture.Body.LinearVelocity.X > 1f || Fixture.Body.LinearVelocity.Y > 1f;

                PlayerSensor.Body.Position = Fixture.Body.Position + GeneralManager.MousePos + new Vector2(Camera.GetRect.X, Camera.GetRect.Y) - GetPosition();

                SoundEngine.SetPosition(this.GetPosition(), 0); //We need to have angle from ACTUAL animation;

                Camera.GetRect = new Rectangle(Position.X - GeneralManager.ScreenX / 2 + Position.Width / 2, Position.Y - GeneralManager.ScreenY / 2 + Position.Height / 2, Camera.GetRect.Width, Camera.GetRect.Height);

                if (IsWalking)
                {
                    WalkAnim.Angle = -Helper.GetAngleFromVector(GeneralManager.MousePos + new Vector2(Camera.GetRect.X, Camera.GetRect.Y) - GetPosition()) + (float)Math.PI;
                    WalkNormalAnim.Angle = WalkAnim.Angle;

                    WalkAnim.Update(gameTime);
                    WalkNormalAnim.Update(gameTime);
                }
                else
                {
                    IdleAnim.Angle = -Helper.GetAngleFromVector(GeneralManager.MousePos + new Vector2(Camera.GetRect.X, Camera.GetRect.Y) - GetPosition()) + (float)Math.PI;
                    IdleNormalAnim.Angle = IdleAnim.Angle;

                    IdleAnim.Update(gameTime);
                    IdleNormalAnim.Update(gameTime);
                }

                ShootLight.Position = new Vector3(GetPosition(), 0);
                ShootLight.Direction = new Vector3(GeneralManager.MousePos + new Vector2(Camera.GetRect.X, Camera.GetRect.Y), 0) - ShootLight.Position + new Vector3(0, 0, 10);
                ShootLight.Direction.Normalize();

                ShootLight.Power *= 0.8f;

                weapon.actCD -= gameTime.ElapsedGameTime.Milliseconds;

                //if (key.Position.X > Position.X) Logger.Write(key.Position.X + " RIGHT " + Position.X);
                //if (key.Position.X < Position.X) Logger.Write(key.Position.X + " Left " + Position.X);

                //if (key.Position.Y > Position.Y) Logger.Write(key.Position.Y + " Down " + Position.Y);
                //if (key.Position.Y < Position.Y) Logger.Write(key.Position.Y + " Up " + Position.Y);

                if (key.pickedUp && showEndMsg && GeneralManager.CheckKey(Keys.Enter))
                {
                    showEndMsg = false;
                }

            }

            base.Update(gameTime); 
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsWalking)
            {
                WalkAnim.Position = Position;
                WalkNormalAnim.Position = Position;

                WalkAnim.Draw(gameTime,1.2f);
                WalkNormalAnim.Draw(gameTime,1.2f);
            }
            else
            {
                IdleAnim.Position = Position;
                IdleNormalAnim.Position = Position;

                IdleAnim.Draw(gameTime,1.2f);
                IdleNormalAnim.Draw(gameTime,1.2f);
            }

            if (key.pickedUp && showEndMsg)
            {
                stoper.Stop();
                Renderer.PostDrawFont(GeneralManager.Fonts["Fonts/28DaysLater"], new Vector2(100, 100), "You Found the Key in " + stoper.Elapsed.ToString() + " ! \n You can explore futher, by pressing Enter", Color.White);
            }

            if (hp < 0)
            {
                Renderer.PostDrawFont(GeneralManager.Fonts["Fonts/28DaysLater"], new Vector2(100, 100), "You died ;(\n  Press ESC to exit.", Color.Red);
            }
        }

        public override string Serialize()
        {
            string s = Position.X + " " + Position.Y + " " + Position.Width + " " + Position.Height + " " + hp + " " + def + " " + damage + " " + lucky;
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

        public override bool Interact(Entity E)
        {
            return base.Interact(E);
        }
    }
}

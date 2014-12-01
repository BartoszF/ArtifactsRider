using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using VAPI.RenderEffects;
using Microsoft.Xna.Framework;
using VAPI;
using System.Collections.Generic;

namespace ArtifactsRider.MapManager.Entities
{
    class Chest : Entity
    {
        Window ChestWindow;
        ListBox list;
        Button CloseButton;

        List<Item> items;


        public Chest(Rectangle r,Map m)
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

            SetName("Chest");

            items = new List<Item>();

            ChestWindow = new Window(GeneralManager.GetPartialRect(0.4f, 0.2f, 0.2f, 0.6f));
            ChestWindow.BgTex = GeneralManager.Textures["GUI/InGameGUI/ChestMenuBg"];

            ChestWindow.Visible = false;

            CloseButton = new Button(new Rectangle(ChestWindow.Position.Width - 32, 8, 24, 24), "", GeneralManager.Textures["GUI/InGameGUI/CloseButton"], Color.Gray, Color.White, null);
            CloseButton.Action = CloseChestWindow;
            ChestWindow.AddGUI(CloseButton );

            list = new ListBox(new Rectangle(16,16,ChestWindow.Position.Width - 24,ChestWindow.Position.Height - 32));
            ChestWindow.AddGUI(list);

            Map.Parent.AddGUI(ChestWindow);
        }

        public void CloseChestWindow()
        {
            ChestWindow.Visible = false;
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

            Renderer.Draw(GeneralManager.Textures["Entities/" + GetName()], Position, Color.White, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));
            LE.DrawNormal(GeneralManager.Textures["Entities/" + GetName() + "_normal"], Position, null, Fixture.Body.Rotation, new Vector2(Position.Width / 2, Position.Height / 2));

            base.Draw(GameTime);
        }

        public override string Serialize()
        {
            return "";
        }

        public override bool Interact(Entity E)
        {
            return base.Interact(E);
        }

        public override void PlayerUse(Player P)
        {
            ChestWindow.Visible = true;
            base.PlayerUse(P);
        }

        public void AddItem(Item i)
        {
            items.Add(i);
            list.AddItem(i.ItemIcon, i.name + "\n" + i.type);
        }
    }
}

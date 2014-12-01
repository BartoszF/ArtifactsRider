using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VAPI
{
    public class Item
    {
        public Texture2D tex;
        public string desc;
    }

    public class ListBox : GUIComponent
    {
        public List<Item> items;

        public ListBox(Rectangle position)
            : base()
        {
            this.Position = position;
            items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public void AddItem(Texture2D t, string desc)
        {
            Item item = new Item();
            item.tex = t;
            item.desc = desc;
            items.Add(item);
        }

        public override bool CheckActive()
        {
            return true;
        }

        public override bool HandleInput()
        {
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw()
        {
            for (int y = 0; y < items.Count; y++)
            {
                Renderer.PostDraw(items[y].tex, new Rectangle(Position.X, Position.Y + y * 64, 64, 64), true, Position);
                Renderer.PostDrawFont(GeneralManager.Fonts["Fonts/28DaysLater"], new Vector2(Position.X + 68, Position.Y + y * 64), items[y].desc, true, Position);
            }
        }
    }
}

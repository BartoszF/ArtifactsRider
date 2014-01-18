using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArtifactsRider.MapManager
{
    class Chunk
    {
        Rectangle position;
        List<Entity> entites;

        public Chunk(Rectangle pos)
        {
            this.position = pos;
            entites = new List<Entity>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Entity e in entites)
            {
                e.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            foreach (Entity e in entites)
            {
                e.Draw(sb,gameTime);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Newtonsoft.Json;

namespace ArtifactsRider.MapManager
{
    class Map
    {
        Chunk[,] chunks = new Chunk[3,3];       //I think, that we want only 3x3 chunks loaded at once;

        public void Update(GameTime gameTime)
        {
            //Now i need player position/camera position to determine, if i need to load chunks
            foreach (Chunk c in chunks)
            {
                c.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            foreach (Chunk c in chunks)
            {
                c.Draw(sb, gameTime);
            }
        }

        public Chunk LoadChunk(int x, int y)
        {
            //Path only for testing
            StreamReader streamReader = new StreamReader("Chunks/"+x.ToString() + " "+y.ToString()+".chunk");
            //
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            Chunk c = JsonConvert.DeserializeObject<Chunk>(text);
            return c;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VAPI;
using VAPI.RenderEffects;
using System.Xml.Serialization;
using System.IO;
using ArtifactsRider.Mobs;
using Krypton;
using ArtifactsRider.Generators;

namespace ArtifactsRider.MapManager
{
    /// <summary>
    /// Class for chunk in map
    /// </summary>
    [Serializable]
    public class Chunk
    {
        public Rectangle position;          /**< Position on map (actually unused) */

        public int tileSize;                /**< Size of single tile */
        public Tile[,] tiles;               /**< Array of tiles */

        public Map map;                     /**< Reference to map */

        public Vector2 Size
        {
            get
            {
                return new Vector2(position.Width / tileSize, position.Height / tileSize);
            }
        }

        /// <summary>
        /// Accesor of tiles
        /// </summary>
        /// <param name="x">X position (in array) of tile</param>
        /// <param name="y">Y position (in array) of tile</param>
        /// <returns>Tile on X,Y position in array</returns>
        public Tile this[int x, int y]
        {
            get
            {
                return tiles[x, y];
            }
            set
            {
                tiles[x, y] = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pos">Position in map</param>
        /// <param name="Parent">Reference to map instance</param>
        /// <param name="tileSize">Size of single tile</param>
        public Chunk(Rectangle pos, Map Parent, int tileSize)
        {
            this.position = pos;
            this.tileSize = tileSize;
            this.map = Parent;

            tiles = new Tile[pos.Width / tileSize, pos.Height / tileSize];

            GeneratorBase E = new DungeonGenerator();

            E.Generate(this);

            CheckIsolation();

            SaveImage("Content/Maps");
        }
        
        /// <summary>
        /// Update loop
        /// </summary>
        /// <param name="gameTime">Time that passed since last Update loop</param>
        public void Update(GameTime gameTime)
        {
            foreach (Tile t in tiles)
            {
                if (t != null)
                {
                    t.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Draw loop
        /// </summary>
        /// <param name="gameTime">Time that passed since last Update loop</param>
        public void Draw(GameTime gameTime)
        {
            for (int y = (int)MathHelper.Clamp(Camera.GetRect.Y / tileSize, 0, map.GetSize()); y < MathHelper.Clamp(((Camera.GetRect.Y + Camera.GetRect.Height) / tileSize) + 1,0,position.Height/tileSize); y++)
            {
                for (int x = (int)MathHelper.Clamp(Camera.GetRect.X / tileSize, 0, map.GetSize()); x < MathHelper.Clamp(((Camera.GetRect.X + Camera.GetRect.Width) / tileSize) + 1, 0, position.Width / tileSize); x++)
                {
                    if (tiles[x, y].Fixture == null && tiles[x,y].isSolid && !tiles[x,y].Isolated)
                    {
                        tiles[x, y].SetPhysics(map.PhysicalWorld);
                    }

                    tiles[x, y].Draw(gameTime);
                }
            }
        }

        public void Save(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(map.GetSize() + " " + map.GetTileSize());

                for (int y = 0; y < map.GetSize(); y++)
                {
                    for (int x = 0; x < map.GetSize(); x++)
                    {
                        sw.WriteLine(tiles[x, y].Serialize());
                    }
                }
            }
        }

        public void SaveImage(string path)
        {
            Texture2D t = new Texture2D(Renderer.GD, (int)Size.X, (int)Size.Y);

            Color[] data = new Color[(int)Size.X * (int)Size.Y];
            int i = 0;

            for (int y = 0; y < (int)Size.Y; y++)
            {
                for (int x = 0; x < (int)Size.X; x++)
                {
                    if (GetCostArray()[x, y] == 0)
                        data[i++] = new Color(0, 0, 0);
                    else
                    {
                        data[i++] = new Color(255, 255, 255);
                    }
                }
            }

            t.SetData<Color>(data);

            DateTime date = DateTime.Now; //Get the date for the file name
            if(!Directory.Exists(path + "/Mapa "))
            {
                Directory.CreateDirectory(path + "/Mapa ");
            }
            Stream stream = File.Create(path + "/Mapa " + date.ToString("dd-MM-yy H_mm_ss") + ".png");

            //Save as PNG
            t.SaveAsPng(stream, (int)Size.X, (int)Size.Y);
            Logger.Write("Saved map png");
            stream.Dispose();
            t.Dispose();
        }

        /// <summary>
        /// Method to replace tile
        /// </summary>
        /// <param name="x">X position (in array)</param>
        /// <param name="y">Y position (in array)</param>
        /// <param name="Tile">New tile</param>
        public void SetTile(int x, int y, Tile Tile)
        {
            tiles[x, y] = Tile;
        }

        /// <summary>
        /// Check if tiles are surrounded with wall tiles
        /// </summary>
        public void CheckIsolation()
        {
            for (int y = 0; y < map.GetSize(); y++)
            {
                for (int x = 0; x < map.GetSize(); x++)
                {
                    if (x > 0 && x < map.GetSize()-2 && y > 0 && y < map.GetSize()-2)
                    {
                        tiles[x, y].Isolated = tiles[x,y].isSolid && tiles[x - 1, y].isSolid && tiles[x + 1, y].isSolid && tiles[x, y - 1].isSolid && tiles[x, y + 1].isSolid &&
                                                    tiles[x - 1, y - 1].isSolid && tiles[x - 1, y + 1].isSolid && tiles[x + 1, y - 1].isSolid && tiles[x + 1, y + 1].isSolid;
                    }
                    else if (x == 0)
                    {
                        if (y == 0)
                        {
                            tiles[x, y].Isolated = tiles[x, y].isSolid && tiles[x + 1, y].isSolid && tiles[x, y + 1].isSolid && tiles[x + 1, y + 1].isSolid;
                        }
                        else if (y == map.GetSize() - 1)
                        {
                            tiles[x, y].Isolated = tiles[x, y].isSolid && tiles[x + 1, y].isSolid && tiles[x, y - 1].isSolid && tiles[x + 1, y - 1].isSolid;
                        }
                        else
                        {
                            tiles[x, y].Isolated = tiles[x, y].isSolid && tiles[x + 1, y].isSolid && tiles[x, y + 1].isSolid && tiles[x, y - 1].isSolid && tiles[x + 1, y + 1].isSolid && tiles[x + 1, y - 1].isSolid;
                        }
                    }
                    else if (x == map.GetSize()-1)
                    {
                        if (y == 0)
                        {
                            tiles[x, y].Isolated = tiles[x, y].isSolid && tiles[x - 1, y].isSolid && tiles[x, y + 1].isSolid && tiles[x - 1, y + 1].isSolid;
                        }
                        else if (y == map.GetSize() - 1)
                        {
                            tiles[x, y].Isolated = tiles[x, y].isSolid && tiles[x - 1, y].isSolid && tiles[x, y - 1].isSolid && tiles[x - 1, y - 1].isSolid;
                        }
                        else
                        {
                            tiles[x, y].Isolated = tiles[x, y].isSolid && tiles[x - 1, y].isSolid && tiles[x, y - 1].isSolid && tiles[x, y - 1].isSolid && tiles[x - 1, y + 1].isSolid && tiles[x - 1, y - 1].isSolid;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if tile is surrounded with wall tiles
        /// </summary>
        /// <param name="t">Tile to check</param>
        /// <returns>True if surrounded.</returns>
        public bool CheckIsolation(Tile t)
        {
            if (t.position.X > 0 && t.position.Y > 0 && t.position.X < (map.GetSize()-1) * map.GetTileSize() && t.position.Y < (map.GetSize()-1) * map.GetTileSize())
            {
                int x = t.position.X / map.GetTileSize();
                int y = t.position.Y / map.GetTileSize();

                t.Isolated = tiles[x, y].isSolid && tiles[x - 1, y].isSolid && tiles[x + 1, y].isSolid && tiles[x, y - 1].isSolid && tiles[x, y + 1].isSolid &&
                                                    tiles[x - 1, y - 1].isSolid && tiles[x - 1, y + 1].isSolid && tiles[x + 1, y - 1].isSolid && tiles[x + 1, y + 1].isSolid;

                if (t.Isolated) t = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get array of walk costs
        /// </summary>
        /// <returns>Array of bytes</returns>
        public byte[,] GetCostArray()
        {
            byte[,] CostArray = new byte[position.Width / tileSize, position.Height / tileSize];

            for (int y = 0; y < position.Height / tileSize; y++)
            {
                for (int x = 0; x < position.Width / tileSize; x++)
                {
                    CostArray[x, y] = Convert.ToByte(tiles[x, y].Cost);
                }
            }

            return CostArray;
        }

        public void Load(string path)
        {
           
        }
    }
}

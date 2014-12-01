using System.Collections.Generic;
using ArtifactsRider.MapManager.Entities;
using ArtifactsRider.Mobs;
using ArtifactsRider.Scenes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VAPI;
using VAPI.Algorithms;
using ArtifactsRider.Items;

namespace ArtifactsRider.MapManager
{
    /// <summary>
    /// Class for Map
    /// </summary>
    public class Map
    {
        public Chunk chunk;         /**< Chunk reference */
        public Player Player;       /**< Player reference */           
        int tileSize;               /**< Size of single tile */
        int size;                   /**< Size of map */
        public World PhysicalWorld; /**< Physical World reference */
        public PathFinderFast pfinder;  /**< PathFinding Engine */

        public WorldScreen Parent;  /**< Reference to World Screen */

        public List<Entity> Entities;   /**< List of entities */

        ///
        ///<summary>Accesor of tile array </summary>
        ///
        public Tile this[int x, int y]
        {
            get
            {
                return chunk[x, y];
            }
            set
            {
                chunk[x, y] = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="WS">Reference to World Screen</param>
        /// <param name="tileSize">Size of single tile</param>
        /// <param name="size">Size of map</param>
        public Map(WorldScreen WS, int tileSize, int size)
        {
            this.tileSize = tileSize;
            this.size = size;

            this.Entities = new List<Entity>();

            PhysicalWorld = new World(new Vector2(0, 0));

            Player = new Player(new Rectangle(6 * 64, 6 * 64, 64, 64), this);
            AddEntity(Player);

            chunk = new Chunk(new Rectangle(0, 0, size * tileSize, size * tileSize), this, tileSize);
            Parent = WS;

            pfinder = new PathFinderFast(GetChunk().GetCostArray());

            for (int i = 0; i < 25; i++)
            {
                Rectangle r = new Rectangle((int)Helper.GetRandomTo(size * tileSize), (int)Helper.GetRandomTo(size * tileSize), 64, 64);
                Rectangle t = new Rectangle(r.X / tileSize, r.Y / tileSize, tileSize, tileSize);

                while (chunk.tiles[t.X,t.Y].isSolid)
                {
                    r = new Rectangle((int)Helper.GetRandomTo(size * tileSize), (int)Helper.GetRandomTo(size * tileSize), 64, 64);
                    t = new Rectangle(r.X / tileSize, r.Y / tileSize, tileSize, tileSize);
                }
                new ZombieBig(r, this);
            }

            new Barrel(new Rectangle(Player.Position.X + 64, Player.Position.Y + 64, 32, 32), this);
            new FloorFan(new Rectangle(Player.Position.X + 64, Player.Position.Y + 64, 128, 128), this);
            //chunk.Save("Content/Chunks/00.xml");
        }

        /// <summary>
        /// Method for getting tile size
        /// </summary>
        /// <returns>Tile size</returns>
        public int GetTileSize() { return tileSize; }
        /// <summary>
        /// Method for getting map size
        /// </summary>
        /// <returns>Map size</returns>
        public int GetSize() { return size; }
        /// <summary>
        /// Method for setting tile size
        /// </summary>
        /// <param name="t">Tile size</param>
        public void SetTileSize(int t) { tileSize = t; }
        /// <summary>
        /// Method for settin map size
        /// </summary>
        /// <param name="s">Map size</param>
        public void SetSize(int s) { size = s; }
        /// <summary>
        /// Method for getting chunk
        /// </summary>
        /// <returns>Chunk</returns>
        public Chunk GetChunk() { return chunk; }

        /// <summary>
        /// Method for addin Entity
        /// </summary>
        /// <param name="E">Entity to add</param>
        public void AddEntity(Entity E)
        {
            Entities.Add(E);
        }

        /// <summary>
        /// Method for Handling Input
        /// </summary>
        /// <returns>True, if something "special" was clicked</returns>
        public bool HandleInput()
        {
            return Player.HandleInput();
        }

        /// <summary>
        /// Main update loop
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public void Update(GameTime gameTime)
        {

            chunk.Update(gameTime);

            PhysicalWorld.Step(1f/60f);


            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].Update(gameTime);
            }
            //p.Update(gameTime);
        }

        /// <summary>
        /// Main draw loop
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public void Draw(GameTime gameTime)
        {
            chunk.Draw(gameTime);

            foreach (Entity E in Entities)
            {
                E.Draw(gameTime);
            }
            //p.Draw(gameTime);
        }
    }
}

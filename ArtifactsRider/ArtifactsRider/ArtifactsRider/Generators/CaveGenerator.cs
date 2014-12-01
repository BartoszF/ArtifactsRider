using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtifactsRider.MapManager;
using Microsoft.Xna.Framework;

namespace ArtifactsRider.Generators
{
    /// <summary>
    /// Derived class to generate Caves
    /// </summary>
    class CaveGenerator : GeneratorBase
    {
        Random rand = new Random();     /**< Pseudo random number generator */

        public int PercentAreWalls;     /**< Percent of all tiles to be walls */
        int[,] Map;                     /**< 2D Array to mark tiles */

        /// <summary>
        /// Constructor
        /// </summary>
        public CaveGenerator() : base()
        {
            PercentAreWalls = 40;
            
            //RandomFillMap();
        }

        /// <summary>
        /// Overrided method to generate
        /// </summary>
        /// <param name="Chunk">Chunk to generate in</param>
        public override void Generate(Chunk Chunk)
        {
            Map = new int[(int)Chunk.Size.X, (int)Chunk.Size.Y];        ///Initializing array of tiles
            BlankMap(Chunk);
            RandomFillMap(Chunk);
            MakeCaverns(Chunk);
            Finish(Chunk);

        }

        /// <summary>
        /// Finish generating
        /// </summary>
        /// <param name="Chunk">Chunk</param>
        void Finish(Chunk Chunk)
        {
            for (int column = 0, row = 0; row <= Chunk.Size.Y - 1; row++)
            {
                for (column = 0; column <= Chunk.Size.X - 1; column++)
                {
                    switch (Map[column, row])
                    {
                        case 0:
                            Chunk.SetTile(column, row, new Tile(new Rectangle(Chunk.position.X + column * Chunk.tileSize, Chunk.position.Y + row * Chunk.tileSize, Chunk.tileSize, Chunk.tileSize), "dungFloor", 30));
                            break;
                        case 1:
                            Chunk.SetTile(column, row, new Tile(new Rectangle(Chunk.position.X + column * Chunk.tileSize, Chunk.position.Y + row * Chunk.tileSize, Chunk.tileSize, Chunk.tileSize), "dungWall", 0));
                            break;
                    };
                }
            }
        }

        /// <summary>
        /// Method to make caverns in map
        /// </summary>
        /// <param name="Chunk">Chunk to make caverns in</param>
        public void MakeCaverns(Chunk Chunk)
        {
            // By initilizing column in the outter loop, its only created ONCE
            for (int column = 0, row = 0; row <= Chunk.Size.Y - 1; row++)
            {
                for (column = 0; column <= Chunk.Size.X - 1; column++)
                {
                    Map[column, row] = PlaceWallLogic(Chunk, column, row);
                }
            }
        }

        public int PlaceWallLogic(Chunk Chunk, int x, int y)
        {
            int numWalls = GetAdjacentWalls(Chunk, x, y, 1, 1);

            if (Map[x, y] == 1)
            {
                if (numWalls >= 4)
                {
                    return 1;
                }
                if (numWalls < 2)
                {
                    return 0;
                }

            }
            else
            {
                if (numWalls >= 5)
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Getting number of adjacent walls
        /// </summary>
        /// <param name="Chunk">Chunk</param>
        /// <param name="x">X position of tile</param>
        /// <param name="y">Y position of tile</param>
        /// <param name="scopeX">Width/2 of scope</param>
        /// <param name="scopeY">Height/2 of scope</param>
        /// <returns>Adjacent walls as int</returns>
        public int GetAdjacentWalls(Chunk Chunk, int x, int y, int scopeX, int scopeY)
        {
            int startX = x - scopeX;
            int startY = y - scopeY;
            int endX = x + scopeX;
            int endY = y + scopeY;

            int iX = startX;
            int iY = startY;

            int wallCounter = 0;

            for (iY = startY; iY <= endY; iY++)
            {
                for (iX = startX; iX <= endX; iX++)
                {
                    if (!(iX == x && iY == y))
                    {
                        if (IsWall(Chunk, iX, iY))
                        {
                            wallCounter += 1;
                        }
                    }
                }
            }
            return wallCounter;
        }

        /// <summary>
        /// Check if tile is wall
        /// </summary>
        /// <param name="Chunk">Chunk</param>
        /// <param name="x">X Position of tile</param>
        /// <param name="y">Y Position of tile</param>
        /// <returns>True, if tile is a wall</returns>
        bool IsWall(Chunk Chunk, int x, int y)
        {
            // Consider out-of-bound a wall
            if (IsOutOfBounds(Chunk, x, y))
            {
                return true;
            }

            if (Map[x, y] == 1)
            {
                return true;
            }

            if (Map[x, y] == 0)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Check if X and Y are out of chunk bounds
        /// </summary>
        /// <param name="Chunk">Chunk to check</param>
        /// <param name="x">X Position of tile</param>
        /// <param name="y">Y Position of tile</param>
        /// <returns>True, if tile is out of chunk bounds</returns>
        bool IsOutOfBounds(Chunk Chunk, int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return true;
            }
            else if (x > Chunk.Size.X - 1 || y > Chunk.Size.Y - 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Saving chunk to string
        /// </summary>
        /// <param name="Chunk">Chunk to save</param>
        /// <returns>Chunk in string</returns>
        string MapToString(Chunk Chunk)
        {
            string returnString = string.Join(" ", // Seperator between each element
                                              "Width:",
                                              Chunk.Size.X.ToString(),
                                              "\tHeight:",
                                              Chunk.Size.Y.ToString(),
                                              "\t% Walls:",
                                              PercentAreWalls.ToString(),
                                              Environment.NewLine
                                             );

            List<string> mapSymbols = new List<string>();
            mapSymbols.Add(".");
            mapSymbols.Add("#");
            mapSymbols.Add("+");

            for (int column = 0, row = 0; row < Chunk.Size.Y; row++)
            {
                for (column = 0; column < Chunk.Size.X; column++)
                {
                    returnString += mapSymbols[Map[column, row]];
                }
                returnString += Environment.NewLine;
            }
            return returnString;
        }

        /// <summary>
        /// Method to clear map
        /// </summary>
        /// <param name="Chunk">Chunk to clear</param>
        public void BlankMap(Chunk Chunk)
        {
            for (int column = 0, row = 0; row < Chunk.Size.Y; row++)
            {
                for (column = 0; column < Chunk.Size.X; column++)
                {
                    Map[column, row] = 0;
                }
            }
        }

        /// <summary>
        /// Filling map with random tiles
        /// </summary>
        /// <param name="Chunk">Chunk to fill</param>
        public void RandomFillMap(Chunk Chunk)
        {
            // New, empty map
            Map = new int[(int)Chunk.Size.X, (int)Chunk.Size.Y];

            int mapMiddle = 0; // Temp variable
            for (int column = 0, row = 0; row < Chunk.Size.Y; row++)
            {
                for (column = 0; column < Chunk.Size.X; column++)
                {
                    // If coordinants lie on the the edge of the map (creates a border)
                    if (column == 0)
                    {
                        Map[column, row] = 1;
                    }
                    else if (row == 0)
                    {
                        Map[column, row] = 1;
                    }
                    else if (column == Chunk.Size.X - 1)
                    {
                        Map[column, row] = 1;
                    }
                    else if (row == Chunk.Size.Y - 1)
                    {
                        Map[column, row] = 1;
                    }
                    // Else, fill with a wall a random percent of the time
                    else
                    {
                        mapMiddle = (int)(Chunk.Size.Y / 2);

                        if (row == mapMiddle)
                        {
                            Map[column, row] = 0;
                        }
                        else
                        {
                            Map[column, row] = RandomPercent(PercentAreWalls);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if value is between 1-101
        /// </summary>
        /// <param name="percent">Value to check</param>
        /// <returns>True, if percent is between 1-101. False otherwise</returns>
        int RandomPercent(int percent)
        {
            if (percent >= rand.Next(1, 101))
            {
                return 1;
            }
            return 0;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VAPI;
using ArtifactsRider.MapManager;
using VAPI.RenderEffects;
using ArtifactsRider.MapManager.Entities;
using ArtifactsRider.Items;

namespace ArtifactsRider.Generators
{
    /// <summary>
    /// Derived class for generating dungeons
    /// </summary>
    public class DungeonGenerator : GeneratorBase
    {
        /// <summary>
        /// Enum for directions
        /// </summary>
        public enum Direction
        {
            North,
            South,
            West,
            East
        }

        List<Vector2> LightList;                            /**< List of lights added */
        List<KeyValuePair<Vector2, Direction>> AlarmLights; /**< List of alarm lights added */
        Chunk Chunk;                                        /**< Chunk reference */

        ///<summary>
        ///Enum of possible tiles
        ///</summary>
        public enum Tile
        {
            Corridor,
            Unused,
            DirtFloor,
            DirtWall,
            StoneWall,
            Door,
            Upstairs,
            Downstairs,
            Chest
        }

       // misc. messages to print
       const string MsgXSize = "X size of dungeon: \t";
   
       const string MsgYSize = "Y size of dungeon: \t";
   
       const string MsgMaxObjects = "max # of objects: \t";
   
       const string MsgNumObjects = "# of objects made: \t";
   
       // max size of the map
       int xmax = 80; /**< Columns */
       int ymax = 80; /**< Rows */
   
       // size of the map
       int _xsize;
       int _ysize;
   
       // number of "objects" to generate on the map
       int _objects;

       //if we placed a key
       bool placedKey = false;
       //chance for placing key
       const int chanceKey = 50;
   
       // define the %chance to generate either a room or a corridor on the map
       // BTW, rooms are 1st priority so actually it's enough to just define the chance
       // of generating a room
       const int ChanceRoom = 90;
   
       // our map
       Tile[] _dungeonMap = { };
   
   
   
       public int Corridors
       {
           get;
           private set;
       }

       public override void Generate(Chunk Chunk)
       {
           this.Chunk = Chunk;
           xmax = (int)Chunk.Size.X;
           ymax = (int)Chunk.Size.Y;

           LightList = new List<Vector2>();
           AlarmLights = new List<KeyValuePair<Vector2, Direction>>();

           CreateDungeon((int)Chunk.Size.X, (int)Chunk.Size.Y, 100);
           ConvertDungeon(Chunk);

           LightingEngine LE = (LightingEngine)Renderer.GetRenderEffect("LightingEngine");
           foreach (Vector2 V in LightList)
           {
               PointLight L = new PointLight()
               {
                   IsEnabled = true,
                   Color = new Vector4(0.9f, .2f, .2f, 1f),
                   Power = .85f,
                   LightDecay = 300,
                   Position = new Vector3(V.X * Chunk.tileSize, V.Y * Chunk.tileSize, 1),
               };

               LE.AddLight(L);
           }

           foreach (KeyValuePair<Vector2, Direction> Alarm in AlarmLights)
           {
               switch (Alarm.Value)
               {
                   case Direction.North:
                       for (int i = -2; i <= 2; i++)
                       {
                           if (!(GetCellType((int)Alarm.Key.X + i, (int)Alarm.Key.Y) == Tile.DirtFloor || GetCellType((int)Alarm.Key.X + i, (int)Alarm.Key.Y) == Tile.Corridor) ||
                               (GetCellType((int)Alarm.Key.X + i, (int)Alarm.Key.Y - 1) == Tile.DirtFloor || GetCellType((int)Alarm.Key.X + i, (int)Alarm.Key.Y - 1) == Tile.Corridor))
                           {
                               break;
                           }
                       }

                       new RedAlarmLamp(new Rectangle((int)Alarm.Key.X * 64 + 32, (int)Alarm.Key.Y * 64 + 4, 16,8), Chunk.map);

                       break;
                   case Direction.South:

                       for (int i = -2; i <= 2; i++)
                       {
                           if (!(GetCellType((int)Alarm.Key.X + i, (int)Alarm.Key.Y + 1) == Tile.DirtFloor || GetCellType((int)Alarm.Key.X + i, (int)Alarm.Key.Y + 1) == Tile.Corridor) ||
                               (GetCellType((int)Alarm.Key.X + i, (int)Alarm.Key.Y) == Tile.DirtFloor || GetCellType((int)Alarm.Key.X + i, (int)Alarm.Key.Y ) == Tile.Corridor))
                           {
                               break;
                           }
                       }

                       RedAlarmLamp tmp = new RedAlarmLamp(new Rectangle((int)Alarm.Key.X * 64 + 32, (int)Alarm.Key.Y * 64 + 60, 16,8), Chunk.map);
                       tmp.Fixture.Body.Rotation = (float)Math.PI;

                       break;
                   case Direction.East:

                       break;
                   case Direction.West:

                       break;
               }
           }

       }

       void ConvertDungeon(Chunk C)
       {
           for (int y = 0; y < ymax; y++)
           {
               for (int x = 0; x < xmax; x++)
               {
                   switch (GetCellType(x, y))
                   {
                       case Tile.Corridor:
                           C.SetTile(x, y, new MapManager.Tile(new Rectangle(C.position.X + x * C.tileSize, C.position.Y + y * C.tileSize, C.tileSize, C.tileSize), "dungFloor", 30));
                           break;
                       case Tile.DirtFloor:
                           C.SetTile(x, y, new MapManager.Tile(new Rectangle(C.position.X + x * C.tileSize, C.position.Y + y * C.tileSize, C.tileSize, C.tileSize), "dungFloor", 30));
                           break;
                       case Tile.DirtWall:
                           C.SetTile(x, y, new MapManager.Tile(new Rectangle(C.position.X + x * C.tileSize, C.position.Y + y * C.tileSize, C.tileSize, C.tileSize), "dungWall", 0));
                           break;
                       case Tile.StoneWall:
                           C.SetTile(x, y, new MapManager.Tile(new Rectangle(C.position.X + x * C.tileSize, C.position.Y + y * C.tileSize, C.tileSize, C.tileSize), "dungWall", 0));
                           break;
                       case Tile.Unused:
                           C.SetTile(x, y, new MapManager.Tile(new Rectangle(C.position.X + x * C.tileSize, C.position.Y + y * C.tileSize, C.tileSize, C.tileSize), "dungWall", 0));
                           break;
                       case Tile.Upstairs:
                           C.SetTile(x, y, new MapManager.Tile(new Rectangle(C.position.X + x * C.tileSize, C.position.Y + y * C.tileSize, C.tileSize, C.tileSize), "dungFloor", 30));
                           C.map.Player.Position = new Rectangle(x * C.tileSize, y * C.tileSize,C.tileSize,C.tileSize);
                           break;
                       default:
                           C.SetTile(x, y, new MapManager.Tile(new Rectangle(C.position.X + x * C.tileSize, C.position.Y + y * C.tileSize, C.tileSize, C.tileSize), "dungFloor", 30));
                           break;

                   }
               }
           }
       }

       public static bool IsWall(int x, int y, int xlen, int ylen, int xt, int yt, Direction d)
       {
           Func<int, int, int> a = GetFeatureLowerBound;
           
           Func<int, int, int> b = IsFeatureWallBound;
           switch (d)
           {
               case Direction.North:
                   return xt == a(x, xlen) || xt == b(x, xlen) || yt == y || yt == y - ylen + 1;
               case Direction.East:
                   return xt == x || xt == x + xlen - 1 || yt == a(y, ylen) || yt == b(y, ylen);
               case Direction.South:
                   return xt == a(x, xlen) || xt == b(x, xlen) || yt == y || yt == y + ylen - 1;
               case Direction.West:
                   return xt == x || xt == x - xlen + 1 || yt == a(y, ylen) || yt == b(y, ylen);
           }
           
           throw new InvalidOperationException();
       }
   
       public static int GetFeatureLowerBound(int c, int len)
       {
           return c - len / 2;
       }
   
       public static int IsFeatureWallBound(int c, int len)
       {
           return c + (len - 1) / 2;
       }
   
       public static int GetFeatureUpperBound(int c, int len)
       {
           return c + (len + 1) / 2;
       }
   
       public static IEnumerable<Vector2> GetRoomPoints(int x, int y, int xlen, int ylen, Direction d)
       {
           // north and south share the same x strategy
           // east and west share the same y strategy
           Func<int, int, int> a = GetFeatureLowerBound;
           Func<int, int, int> b = GetFeatureUpperBound;
   
           switch (d)
           {
               case Direction.North:
                   for (var xt = a(x, xlen); xt < b(x, xlen); xt++) for (var yt = y; yt > y - ylen; yt--) yield return new Vector2 { X = xt, Y = yt };
                   break;
               case Direction.East:
                   for (var xt = x; xt < x + xlen; xt++) for (var yt = a(y, ylen); yt < b(y, ylen); yt++) yield return new Vector2 { X = xt, Y = yt };
                   break;
               case Direction.South:
                   for (var xt = a(x, xlen); xt < b(x, xlen); xt++) for (var yt = y; yt < y + ylen; yt++) yield return new Vector2 { X = xt, Y = yt };
                   break;
               case Direction.West:
                   for (var xt = x; xt > x - xlen; xt--) for (var yt = a(y, ylen); yt < b(y, ylen); yt++) yield return new Vector2 { X = xt, Y = yt };
                   break;
               default:
                   yield break;
           }
       }
   
       public Tile GetCellType(int x, int y)
       {
           try
           {
               return this._dungeonMap[x + this._xsize * y];
           }
           catch (IndexOutOfRangeException)
           {
               return Tile.Unused;
           }
       }
   
   
       public bool MakeCorridor(int x, int y, int length, Direction direction)
       {
           // define the dimensions of the corridor (er.. only the width and height..)
           int len = Helper.GetRandom()%5 + 2;
           const Tile Floor = Tile.Corridor;
   
           int xtemp;
           int ytemp = 0;

           DoorButton Button;
           DoorButton Button2;
           Door Door;

           switch (direction)
           {
               case Direction.North:
                   // north
                   // check if there's enough space for the corridor
                   // start with checking it's not out of the boundaries
                   if (x < 0 || x > this._xsize) return false;
                   xtemp = x;

                   // same thing here, to make sure it's not out of the boundaries
                   for (ytemp = y; ytemp > (y - len); ytemp--)
                   {
                       if (ytemp < 0 || ytemp > this._ysize) return false; // oh boho, it was!
                       if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                   }

                   // if we're still here, let's start building
                   Corridors++;
                   for (ytemp = y; ytemp > (y - len); ytemp--)
                   {
                       this.SetCell(xtemp, ytemp, Floor);
                   }

                   if (GetCellType(x - 1, y + 2) == Tile.Corridor || GetCellType(x - 1, y + 2) == Tile.DirtFloor )
                   {
                       Button = new DoorButton(new Rectangle((x - 1) * 64 + 32, (y + 2) * 64 + 8, 16, 16), Chunk.map);
                   }
                   else if (GetCellType(x + 1, y + 2) == Tile.Corridor || GetCellType(x + 1, y + 2) == Tile.DirtFloor )
                   {
                       Button = new DoorButton(new Rectangle((x + 1) * 64 + 32, (y + 2) * 64 + 8, 16, 16), Chunk.map);
                   }
                   else
                   {
                       break;
                   }

                   if (GetCellType(x, y + 1) == Tile.Corridor || GetCellType(x, y + 1) == Tile.DirtFloor)
                   {
                       Button2 = new DoorButton(new Rectangle(x * 64 + 8, (y + 1) * 64, 16, 16), Chunk.map);
                   }
                   else
                   {
                       Chunk.map.Entities.Remove(Button);
                       Button = null;
                       break;
                   }

                   Door = new Door(new Rectangle(x * 64 + 32, (y + 1) * 64 + 32, 64, 64), Chunk.map);

                   Door.DoorButton = Button;
                   Button.Doors = Door;
                   Button2.Doors = Door;

                   Button2.Fixture.Body.Rotation = 1.5f * (float)Math.PI;

                   break;

               case Direction.East:
                   // east
                   if (y < 0 || y > this._ysize) return false;
                   ytemp = y;

                   for (xtemp = x; xtemp < (x + len); xtemp++)
                   {
                       if (xtemp < 0 || xtemp > this._xsize) return false;
                       if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                   }

                   Corridors++;
                   for (xtemp = x; xtemp < (x + len); xtemp++)
                   {
                       this.SetCell(xtemp, ytemp, Floor);
                   }

                   if (GetCellType(x - 2, y -1) == Tile.Corridor || GetCellType(x - 2, y - 1) == Tile.DirtFloor)
                   {
                       Button = new DoorButton(new Rectangle((x - 2) * 64 + 56, (y - 1) * 64 + 32, 16, 16), Chunk.map);
                   }
                   else if (GetCellType(x - 2, y + 1) == Tile.Corridor || GetCellType(x - 2, y + 1) == Tile.DirtFloor)
                   {
                       Button = new DoorButton(new Rectangle((x - 2) * 64 + 56, (y + 1) * 64 + 32, 16, 16), Chunk.map);
                   }
                   else
                   {
                       break;
                   }

                   if (GetCellType(x, y) == Tile.Corridor || GetCellType(x, y) == Tile.DirtFloor)
                   {
                       Button2 = new DoorButton(new Rectangle(x * 64, y * 64 + 56, 16, 16), Chunk.map);
                   }
                   else
                   {
                       Chunk.map.Entities.Remove(Button);
                       Button = null;
                       break;
                   }

                   Door = new Door(new Rectangle((x - 1) * 64 + 32, (y) * 64 + 32, 64, 64), Chunk.map);

                   Door.Fixture.Body.Rotation = (float)Math.PI / 2f;
                   Button.Fixture.Body.Rotation = (float)Math.PI / 2f;
                   Button2.Fixture.Body.Rotation = (float)Math.PI;

                   Door.DoorButton = Button;
                   Button.Doors = Door;
                   Button2.Doors = Door;

                   break;

               case Direction.South:
                   // south
                   if (x < 0 || x > this._xsize) return false;
                   xtemp = x;

                   for (ytemp = y; ytemp < (y + len); ytemp++)
                   {
                       if (ytemp < 0 || ytemp > this._ysize) return false;
                       if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                   }

                   Corridors++;
                   for (ytemp = y; ytemp < (y + len); ytemp++)
                   {
                       this.SetCell(xtemp, ytemp, Floor);
                   }

                   if (GetCellType(x - 1, y - 2) == Tile.Corridor || GetCellType(x - 1, y - 2) == Tile.DirtFloor )
                   {
                       Button = new DoorButton(new Rectangle((x - 1) * 64 + 32, (y - 2) * 64 + 56, 16, 16), Chunk.map);
                   }
                   else if (GetCellType(x + 1, y - 2) == Tile.Corridor || GetCellType(x + 1, y - 2) == Tile.DirtFloor )
                   {
                       Button = new DoorButton(new Rectangle((x + 1) * 64 + 32, (y - 2) * 64 + 56, 16, 16), Chunk.map);
                   }
                   else
                   {
                       break;
                   }

                   if (GetCellType(x, y) == Tile.Corridor || GetCellType(x, y) == Tile.DirtFloor)
                   {
                       Button2 = new DoorButton(new Rectangle(x * 64 + 8, y * 64, 16, 16), Chunk.map);
                   }
                   else
                   {
                       Chunk.map.Entities.Remove(Button);
                       Button = null;
                       break;
                   }

                   Door = new Door(new Rectangle(x * 64 + 32, (y - 1) * 64 + 32, 64, 64), Chunk.map);

                   Door.DoorButton = Button;
                   Button.Doors = Door;
                   Button2.Doors = Door;

                   Button.Fixture.Body.Rotation = (float)Math.PI;
                   Button2.Fixture.Body.Rotation = (float)Math.PI * 1.5f;
                   break;

               case Direction.West:
                   // west
                   if (ytemp < 0 || ytemp > this._ysize) return false;
                   ytemp = y;

                   for (xtemp = x; xtemp > (x - len); xtemp--)
                   {
                       if (xtemp < 0 || xtemp > this._xsize) return false;
                       if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                   }

                   Corridors++;
                   for (xtemp = x; xtemp > (x - len); xtemp--)
                   {
                       this.SetCell(xtemp, ytemp, Floor);
                   }

                   if (GetCellType(x + 2, y -1) == Tile.Corridor || GetCellType(x + 2, y - 1) == Tile.DirtFloor)
                   {
                       Button = new DoorButton(new Rectangle((x + 2) * 64 + 8, (y - 1) * 64 + 32, 16, 16), Chunk.map);
                   }
                   else if (GetCellType(x + 2, y + 1) == Tile.Corridor || GetCellType(x + 2, y + 1) == Tile.DirtFloor )
                   {
                       Button = new DoorButton(new Rectangle((x + 2) * 64 + 8, (y + 1) * 64 + 32, 16, 16), Chunk.map);
                   }
                   else
                   {
                       break;
                   }

                   if (GetCellType(x, y) == Tile.Corridor || GetCellType(x, y) == Tile.DirtFloor)
                   {
                       Button2 = new DoorButton(new Rectangle((x+1) * 64, y * 64 + 56, 16, 16), Chunk.map);
                   }
                   else
                   {
                       Chunk.map.Entities.Remove(Button);
                       Button = null;
                       break;
                   }

                   Door = new Door(new Rectangle((x + 1) * 64 + 32, (y) * 64 + 32, 64, 64), Chunk.map);

                   Door.Fixture.Body.Rotation = (float)-Math.PI / 2f;
                   Button.Fixture.Body.Rotation = (float)-Math.PI / 2f;
                   Button2.Fixture.Body.Rotation = (float)Math.PI;

                   Door.DoorButton = Button;
                   Button.Doors = Door;
                   Button2.Doors = Door;

                   break;
           }
           // woot, we're still here! let's tell the other guys we're done!!
           return true;
       }

       public IEnumerable<Tuple<Vector2, Direction>> GetSurroundingPoints(Vector2 v)
       {
           var points = new[]
                            {
                                Tuple.Create(new Vector2 { X = v.X, Y = v.Y + 1 }, Direction.North),
                                Tuple.Create(new Vector2 { X = v.X - 1, Y = v.Y }, Direction.East),
                                Tuple.Create(new Vector2 { X = v.X , Y = v.Y-1 }, Direction.South),
                                Tuple.Create(new Vector2 { X = v.X +1, Y = v.Y  }, Direction.West),
                                
                            };
           return points.Where(p => InBounds(p.Item1));
       }
       public IEnumerable<Tuple<Vector2, Direction, Tile>> GetSurroundings(Vector2 v)
       {
           return
               this.GetSurroundingPoints(v).Select(r => Tuple.Create(r.Item1, r.Item2, this.GetCellType((int)r.Item1.X, (int)r.Item1.Y)));
       }
       public bool InBounds(int x, int y)
       {
           return x > 0 && x < this.xmax && y > 0 && y < this.ymax;
       }
       public bool InBounds(Vector2 v)
       {
           return this.InBounds((int)v.X, (int)v.Y);
       }
       public bool MakeRoom(int x, int y, int xlength, int ylength, Direction direction)
       {
           // define the dimensions of the room, it should be at least 4x4 tiles (2x2 for walking on, the rest is walls)
           int xlen = 8 + Helper.GetRandom() % xlength;
           int ylen = 8 + Helper.GetRandom() % ylength;
           // the tile type it's going to be filled with
           const Tile Floor = Tile.DirtFloor;
           const Tile Wall = Tile.DirtWall;
           // choose the way it's pointing at
           var points = GetRoomPoints(x, y, xlen, ylen, direction).ToArray();

           // Check if there's enough space left for it
           if (
               points.Any(
                   s =>
                   s.Y < 0 || s.Y > this._ysize || s.X < 0 || s.X > this._xsize || this.GetCellType((int)s.X, (int)s.Y) != Tile.Unused)) return false;
           
           foreach (var p in points)
           {
               this.SetCell((int)p.X, (int)p.Y, IsWall(x, y, xlen, ylen, (int)p.X, (int)p.Y, direction) ? Wall : Floor);
           }

           CreateRoomLight(x, y, direction, xlen, ylen);
           CreateAlarmLights(x, y, direction, xlen, ylen);

           if (!placedKey)
           {
               int c = (int)Helper.GetRandomTo(100);
               if (c < chanceKey)
               {
                   placedKey = true;
                   int posX = x;
                   int posY = y;
                   switch (direction)
                   {
                       case Direction.North:
                           y -= ylen / 2;
                           break;
                       case Direction.South:
                           y += ylen / 2;
                           break;
                       case Direction.West:
                           x -= xlen / 2;
                           break;
                       case Direction.East:
                           x += xlen / 2;
                           break;
                   }

                   Key key = new Key(new Rectangle(x * 32, y * 32, 32, 32), Chunk.map);
                   Chunk.map.Player.key = key;
                   placedKey = true;
                   Logger.Write(x + " " + y);
               }
           }

           // yay, all done
           return true;
       }

       private void CreateAlarmLights(int x, int y, Direction direction, int xlen, int ylen)
       {
           Vector2 Pos = new Vector2(x, y);
           switch (direction)
           {
               case Direction.North:
                   Pos.Y -= ylen - 2;
                   break;
               case Direction.South:
                   Pos.Y += ylen - 2;
                   break;
               case Direction.West:
                   Pos.X -= xlen - 2;
                   break;
               case Direction.East:
                   Pos.X += xlen - 2;
                   break;
           }

           AlarmLights.Add(new KeyValuePair<Vector2, Direction>(Pos, direction));
       }

       private void CreateRoomLight(int x, int y, Direction direction, int xlen, int ylen)
       {
           if (Helper.GetRandom() % 4 == 0)
           {
               Vector2 Pos = new Vector2(x, y);
               switch (direction)
               {
                   case Direction.North:
                       Pos.Y -= ylen / 2;
                       break;
                   case Direction.South:
                       Pos.Y += ylen / 2;
                       break;
                   case Direction.West:
                       Pos.X -= xlen / 2;
                       break;
                   case Direction.East:
                       Pos.X += xlen / 2;
                       break;
               }
               LightList.Add(Pos);
           }
       }
       public Tile[] GetDungeon()
       {
           return this._dungeonMap;
       }
       
       public Direction RandomDirection()
       {
           int dir = Helper.GetRandom()%4;
           switch (dir)
           {
               case 0:
                   return Direction.North;
               case 1:
                   return Direction.East;
               case 2:
                   return Direction.South;
               case 3:
                   return Direction.West;
               default:
                   throw new InvalidOperationException();
           }
       }
       //and here's the one generating the whole map
       public bool CreateDungeon(int inx, int iny, int inobj)
       {
           this._objects = inobj < 1 ? 10 : inobj;
           // adjust the size of the map, if it's smaller or bigger than the limits
           if (inx < 3) this._xsize = 3;
           else if (inx > xmax) this._xsize = xmax;
           else this._xsize = inx;
           if (iny < 3) this._ysize = 3;
           else if (iny > ymax) this._ysize = ymax;
           else this._ysize = iny;
           Logger.Write(MsgXSize + this._xsize);
           Logger.Write(MsgYSize + this._ysize);
           Logger.Write(MsgMaxObjects + this._objects);
           // redefine the map var, so it's adjusted to our new map size
           this._dungeonMap = new Tile[this._xsize * this._ysize];
           // start with making the "standard stuff" on the map
           this.Initialize();
           /*******************************************************************************
           And now the code of the random-map-generation-algorithm begins!
           *******************************************************************************/
           // start with making a room in the middle, which we can start building upon
           this.MakeRoom(this._xsize / 2, this._ysize / 2, 15, 10, RandomDirection());
           // keep count of the number of "objects" we've made
           int currentFeatures = 1; // +1 for the first room we just made
           // then we sart the main loop
           for (int countingTries = 0; countingTries < 10000; countingTries++)
           {
               // check if we've reached our quota
               if (currentFeatures == this._objects)
               {
                   break;
               }
               // start with a random wall
               int newx = 0;
               int xmod = 0;
               int newy = 0;
               int ymod = 0;
               Direction? validTile = null;
               // 1000 chances to find a suitable object (room or corridor)..
               for (int testing = 0; testing < 10000; testing++)
               {
                   newx = 1 + Helper.GetRandom() % (this._xsize - 1);
                   newy = 1 + Helper.GetRandom() % (this._ysize - 1);
                   if (GetCellType(newx, newy) == Tile.DirtWall || GetCellType(newx, newy) == Tile.Corridor)
                   {
                       var surroundings = this.GetSurroundings(new Vector2() { X = newx, Y = newy });
                       // check if we can reach the place
                       var canReach =
                           surroundings.FirstOrDefault(s => s.Item3 == Tile.Corridor || s.Item3 == Tile.DirtFloor);
                       if (canReach == null)
                       {
                           continue;
                       }
                       validTile = canReach.Item2;
                       switch (canReach.Item2)
                       {
                           case Direction.North:
                               xmod = 0;
                               ymod = -1;
                               break;
                           case Direction.East:
                               xmod = 1;
                               ymod = 0;
                               break;
                           case Direction.South:
                               xmod = 0;
                               ymod = 1;
                               break;
                           case Direction.West:
                               xmod = -1;
                               ymod = 0;
                               break;
                           default:
                               throw new InvalidOperationException();
                       }

                       // check that we haven't got another door nearby, so we won't get alot of openings besides
                       // each other
                       if (GetCellType(newx, newy + 1) == Tile.Door) // north
                       {
                           validTile = null;
                       }
                       else if (GetCellType(newx - 1, newy) == Tile.Door) // east
                           validTile = null;
                       else if (GetCellType(newx, newy - 1) == Tile.Door) // south
                           validTile = null;
                       else if (GetCellType(newx + 1, newy) == Tile.Door) // west
                           validTile = null;
                      
                       // if we can, jump out of the loop and continue with the rest
                       if (validTile.HasValue) break;
                   }
               }
               if (validTile.HasValue)
               {
                   // choose what to build now at our newly found place, and at what direction
                   int feature = Helper.GetRandom() % 100;
                   if (feature <= ChanceRoom)
                   { // a new room
                       if (this.MakeRoom(newx + xmod, newy + ymod, 12, 8, validTile.Value))
                       {
                           currentFeatures++; // add to our quota
                           // then we mark the wall opening with a door
                           this.SetCell(newx, newy, Tile.Door);
                           // clean up infront of the door so we can reach it
                           this.SetCell(newx + xmod, newy + ymod, Tile.DirtFloor);
                       }
                   }
                   else if (feature >= ChanceRoom)
                   { // new corridor
                       if (this.MakeCorridor(newx + xmod, newy + ymod, 3, validTile.Value))
                       {
                           // same thing here, add to the quota and a door
                           currentFeatures++;
                           this.SetCell(newx, newy, Tile.Door);
                       }
                   }
               }
           }
   
           /*******************************************************************************
           All done with the building, let's finish this one off
           *******************************************************************************/
           AddSprinkles();
   
           // all done with the map generation, tell the user about it and finish
           Logger.Write(MsgNumObjects + currentFeatures);
   
           return true;
       }
   
       void Initialize()
       {
           for (int y = 0; y < this._ysize; y++)
           {
               for (int x = 0; x < this._xsize; x++)
               {
                   // ie, making the borders of unwalkable walls
                   if (y == 0 || y == this._ysize - 1 || x == 0 || x == this._xsize - 1)
                   {
                       this.SetCell(x, y, Tile.StoneWall);
                   }
                   else
                   {                        // and fill the rest with dirt
                       this.SetCell(x, y, Tile.Unused);
                   }
               }
           }
       }
   
       // setting a tile's type
       void SetCell(int x, int y, Tile celltype)
       {
           this._dungeonMap[x + this._xsize * y] = celltype;
       }
   
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       void AddSprinkles()
       {
           // sprinkle out the bonusstuff (stairs, chests etc.) over the map
           int state = 0; // the state the loop is in, start with the stairs
           while (state != 10)
           {
               for (int testing = 0; testing < 1000; testing++)
               {
                   var newx = 1 + Helper.GetRandom() % (this._xsize - 1);
                   int newy = 1 + Helper.GetRandom() % (this._ysize - 2);
   
                   // Logger.Write("x: " + newx + "\ty: " + newy);
                   int ways = 4; // from how many directions we can reach the random spot from
   
                   // check if we can reach the spot
                   if (GetCellType(newx, newy + 1) == Tile.DirtFloor || GetCellType(newx, newy + 1) == Tile.Corridor)
                   {
                       // north
                       if (GetCellType(newx, newy + 1) != Tile.Door)
                           ways--;
                   }
   
                   if (GetCellType(newx - 1, newy) == Tile.DirtFloor || GetCellType(newx - 1, newy) == Tile.Corridor)
                   {
                       // east
                       if (GetCellType(newx - 1, newy) != Tile.Door)
                           ways--;
                   }
   
                   if (GetCellType(newx, newy - 1) == Tile.DirtFloor || GetCellType(newx, newy - 1) == Tile.Corridor)
                   {
                       // south
                       if (GetCellType(newx, newy - 1) != Tile.Door)
                           ways--;
                   }
   
                   if (GetCellType(newx + 1, newy) == Tile.DirtFloor || GetCellType(newx + 1, newy) == Tile.Corridor)
                   {
                       // west
                       if (GetCellType(newx + 1, newy) != Tile.Door)
                           ways--;
                   }
   
                   if (state == 0)
                   {
                       if (ways == 0)
                       {
                           // we're in state 0, let's place a "upstairs" thing
                           this.SetCell(newx, newy, Tile.Upstairs);
                           state = 1;
                           break;
                       }
                   }
                   else if (state == 1)
                   {
                       if (ways == 0)
                       {
                           // state 1, place a "downstairs"
                           this.SetCell(newx, newy, Tile.Downstairs);
                           state = 10;
                           break;
                       }
                   }
               }
           }
       }
    }
}

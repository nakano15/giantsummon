using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon
{
    public class PathFinder
    {
        public static int MaxTileCheck = 500;

        public static List<Breadcrumbs> DoPathFinding(Vector2 StartPosition, int EndPosX, int EndPosY, int JumpDistance = 6, int FallDistance = 6)
        {
            int StartPosX = (int)(StartPosition.X * (1f / 16)), StartPosY = (int)(StartPosition.Y * (1f / 16));
            int Attempts = 0;
            while (CheckForSolidBlocks(StartPosX, StartPosY, 3))
            {
                StartPosY--;
                Attempts++;
                if (Attempts >= 8)
                {
                    //Main.NewText("Inside too many solid tiles.");
                    return new List<Breadcrumbs>();
                }
            }
            Attempts = 0;
            while (!CheckForSolidGroundUnder(StartPosX, StartPosY))
            {
                StartPosY++;
                Attempts++;
                if (Attempts >= 8)
                {
                    //Main.NewText("No floor.");
                    return new List<Breadcrumbs>();
                }
            }
            List<Node> LastNodeList = new List<Node>(), NextNodeList = new List<Node>();
            List<Point> VisitedNodes = new List<Point>();
            NextNodeList.Add(new Node(StartPosX, StartPosY, Node.NONE));
            VisitedNodes.Add(new Point(StartPosX, StartPosY));
            const int MaxDistance = 80; //50;
            Node NodeFound = null;
            int HangPreventer = 0;
            while (NodeFound == null)
            {
                LastNodeList.Clear();
                LastNodeList.AddRange(NextNodeList);
                NextNodeList.Clear();
                if (LastNodeList.Count == 0)
                    break;
                while (LastNodeList.Count > 0)
                {
                    Node n = LastNodeList[0];
                    LastNodeList.RemoveAt(0);
                    int X = n.NodeX, Y = n.NodeY;
                    /*Dust d = Dust.NewDustPerfect(new Vector2(X * 16 + 8, Y * 16 + 8), Terraria.ID.DustID.Flare, Vector2.Zero);
                    d.scale = 6;
                    d.noGravity = true;
                    d.noLight = false;*/
                    if (X == EndPosX && Y == EndPosY)
                    {
                        NodeFound = n;
                        break;
                    }
                    if (Math.Abs(n.NodeX - StartPosX) >= MaxDistance || Math.Abs(n.NodeY - StartPosY) >= MaxDistance) //Skip path finding
                    {
                        continue;
                    }
                    //Need to rework how the breadcrumbs finding works.
                    for (byte dir = 0; dir < 4; dir++)
                    {
                        switch (dir)
                        {
                            case Node.DIR_UP:
                                {
                                    bool HasPlatform = false, HasSolidBlock = false;
                                    int PlatformNodeY = -1;
                                    for (int y = 0; y < JumpDistance; y++)
                                    {
                                        if ((y == 0 ? CheckForSolidBlocks(X, Y, 4) : CheckForSolidBlocksCeiling(X, Y - y)))
                                        {
                                            HasSolidBlock = true;
                                            break;
                                        }
                                        if (CheckForPlatform(X, Y - y) && !CheckForPlatform(X, Y - y + 1) && !CheckForSolidBlocks(X, Y - y - 1))
                                        {
                                            HasPlatform = true;
                                            PlatformNodeY = Y - y - 1;
                                            break;
                                        }
                                        int YCheck = Y - y;
                                        //if (!CheckForSolidBlocks(X, YCheck))
                                        {
                                            for (sbyte x = -1; x < 2; x += 2)
                                            {
                                                if (!CheckForSolidBlocks(X + x, YCheck, PassThroughDoors: true) && CheckForSolidGroundUnder(X + x, YCheck, true) && !VisitedNodes.Contains(new Point(X, YCheck)))
                                                {
                                                    //PossibleToJumpHere
                                                    NextNodeList.Add(new Node(X, YCheck, Node.DIR_UP, n));// x < 0 ? Node.DIR_LEFT : Node.DIR_RIGHT, n));
                                                    VisitedNodes.Add(new Point(X, YCheck));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (HasSolidBlock)
                                        continue;
                                    if (HasPlatform && !VisitedNodes.Contains(new Point(X, PlatformNodeY)))
                                    {
                                        NextNodeList.Add(new Node(X, PlatformNodeY, Node.DIR_UP, n));
                                        VisitedNodes.Add(new Point(X, PlatformNodeY));
                                    }
                                }
                                break;
                            case Node.DIR_DOWN:
                                {
                                    if (CheckForPlatform(X, Y + 1))
                                    {
                                        for (int y = 2; y <= FallDistance; y++)
                                        {
                                            if (CheckForSolidBlocks(X, Y + y))
                                                break;
                                            if (CheckForSolidGroundUnder(X, Y + y) && !VisitedNodes.Contains(new Point(X, Y + y)))
                                            {
                                                NextNodeList.Add(new Node(X, Y + y, Node.DIR_DOWN, n));
                                                VisitedNodes.Add(new Point(X, Y + y));
                                                break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case Node.DIR_RIGHT:
                            case Node.DIR_LEFT:
                                {
                                    sbyte Dir = (sbyte)(dir == Node.DIR_LEFT ? -1 : 1);
                                    int nx = X + Dir, ny = Y;
                                    if((n.NodeDirection == Node.DIR_LEFT && dir == Node.DIR_RIGHT) ||
                                        (n.NodeDirection == Node.DIR_RIGHT && dir == Node.DIR_LEFT))
                                    {
                                        continue;
                                    }
                                    bool Blocked = false;
                                    for(int zy = -1; zy < JumpDistance; zy++)
                                    {
                                        if (zy > 0 && CheckForSolidBlocksCeiling(nx - Dir, ny - zy))
                                        {
                                            Blocked = true;
                                            break;
                                        }
                                        if(!CheckForSolidBlocks(nx, ny - zy, PassThroughDoors: true) && CheckForSolidGroundUnder(nx, ny - zy, PassThroughDoors: true))
                                        {
                                            ny -= zy;
                                            break;
                                        }
                                    }
                                    if (Blocked)
                                        continue;
                                    sbyte MinCheckY = -1, MaxCheckY = 3;
                                    if (!CheckForSolidGroundUnder(nx, ny, true))
                                    {
                                        if (n.NodeDirection != Node.DIR_UP)
                                        {
                                            for (int y = 1; y <= FallDistance; y++)
                                            {
                                                int yc = ny + y;
                                                if (CheckForSolidBlocks(nx, yc, PassThroughDoors: true))
                                                    break;
                                                if (CheckForSolidGroundUnder(nx, yc, true) && !CheckForStairFloor(nx, yc - 1))
                                                {
                                                    if (!VisitedNodes.Contains(new Point(nx, yc)))
                                                    {
                                                        NextNodeList.Add(new Node(nx, yc, Node.DIR_DOWN, n));
                                                        VisitedNodes.Add(new Point(nx, yc));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MinCheckY = 0;
                                            MaxCheckY = 1;
                                        }
                                    }
                                    //else
                                    {
                                        for (int y = MinCheckY; y < MaxCheckY; y++)
                                        {
                                            int yc = ny - y;// * h;
                                            if (CheckForSolidGroundUnder(nx, yc, true) && !CheckForStairFloor(nx, yc - 1) && !CheckForSolidBlocks(nx, yc, PassThroughDoors: true)) //CheckForSolidGroundUnder(nx, yc, true)
                                            {
                                                int xc = nx;
                                                if (!VisitedNodes.Contains(new Point(xc, yc)))
                                                {
                                                    NextNodeList.Add(new Node(xc, yc, dir, n));
                                                    VisitedNodes.Add(new Point(xc, yc));
                                                }
                                            }
                                        }
                                    }
                                }
                                break;

                        }
                    }
                    HangPreventer++;
                    if (HangPreventer >= MaxTileCheck)
                    {
                        //Main.NewText("Path finding hanged with " + NextNodeList.Count + " nodes to check and " + LastNodeList.Count + " nodes to review.");
                        return new List<Breadcrumbs>();
                    }
                }
            }
            List<Breadcrumbs> PathGuide = new List<Breadcrumbs>();
            byte LastDirection = 255;
            /*if (NodeFound == null)
            {
                Main.NewText("No nodes found.");
            }*/
            while (NodeFound != null)
            {
                if (NodeFound.NodeDirection != LastDirection)
                {
                    Breadcrumbs guide = new Breadcrumbs() { X = NodeFound.NodeX, Y = NodeFound.NodeY, NodeOrientation = NodeFound.NodeDirection };
                    PathGuide.Insert(0, guide);
                    LastDirection = NodeFound.NodeDirection;
                }
                NodeFound = NodeFound.LastNode;
            }
            //Main.NewText("Found " + NextNodeList.Count + " nodes to destination!");
            return PathGuide;
        }

        public static bool CheckForSolidGroundUnder(int px, int py, bool PassThroughDoors = false)
        {
            for (sbyte x = -1; x < 2; x++)
            {
                byte State = 0;
                for (int y = 0; y < 2; y++)
                {
                    Tile tile = MainMod.GetTile(px + x, py + y);
                    if (y == 0 && (!tile.active() || (!PassThroughDoors || tile.type != Terraria.ID.TileID.ClosedDoor) || !Main.tileSolid[tile.type]))
                    {
                        State++;
                    }
                    if (y == 1 && tile.active() && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]))
                    {
                        State++;
                    }
                }
                if (State >= 2)
                    return true;
            }
            return false;//CheckForSolidGround(px - 1, py, PassThroughDoors) || CheckForSolidGround(px, py, PassThroughDoors);
        }

        public static bool CheckForStairFloor(int px, int py)
        {
            for (int x = -1; x < 1; x++)
            {
                Tile tile = MainMod.GetTile(px + x, py);
                byte Slope = tile.slope();
                if (tile != null && tile.active() && Terraria.ID.TileID.Sets.Platforms[tile.type] && (Slope == 1 || Slope == 2))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckForSolidBlocks(int tx, int ty, int Height = 3, bool PassThroughDoors = false)
        {
            for (int x = -1; x <= 0; x++)
            {
                for (int y = -(Height - 1); y <= 0; y++)
                {
                    Tile t = MainMod.GetTile(tx + x, ty + y);
                    if (t == null) return false;
                    if (t.active() && Main.tileSolid[t.type] && !Terraria.ID.TileID.Sets.Platforms[t.type] && (!PassThroughDoors || t.type != Terraria.ID.TileID.ClosedDoor))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CheckForSolidBlocksCeiling(int tx, int ty, int Height = 3)
        {
            return CheckForSolidBlocks(tx, ty - Height + 1, 1);
        }

        public static bool CheckForPlatform(int tx, int ty)
        {
            byte PlatformTiles = 0;
            for (int x = -1; x <= 0; x++)
            {
                Tile t = MainMod.GetTile(tx + x, ty);
                if (t == null) return false;
                if (t.active())
                {
                    if (Terraria.ID.TileID.Sets.Platforms[t.type] || (Main.tileSolidTop[t.type] && !Main.tileSolid[t.type]))
                        PlatformTiles++;
                    else if (Main.tileSolid[t.type])
                        return false;
                }
            }
            return PlatformTiles >= 1;
        }

        public static bool AnyPlatform(int tx, int ty)
        {
            byte PlatformTiles = 0;
            for (int x = -1; x <= 0; x++)
            {
                Tile t = MainMod.GetTile(tx + x, ty);
                if (t == null) return false;
                if (t.active() && Terraria.ID.TileID.Sets.Platforms[t.type])
                    PlatformTiles++;
            }
            return PlatformTiles > 0;
        }

        public class Node
        {
            public byte NodeDirection = 0;
            public const byte DIR_UP = 0, DIR_RIGHT = 1, DIR_DOWN = 2, DIR_LEFT = 3, NONE = 255;
            public Node LastNode;
            public int NodeX = 0, NodeY = 0;

            public Node(int NodeX, int NodeY, byte NodeDir = 0, Node LastNode = null)
            {
                NodeDirection = NodeDir;
                this.NodeX = NodeX;
                this.NodeY = NodeY;
                this.LastNode = LastNode;
            }
        }

        public class Breadcrumbs
        {
            public int X = 0, Y = 0;
            public byte NodeOrientation = 0;
        }
    }
}

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
        public static List<Breadcrumbs> DoPathFinding(Vector2 StartPosition, int EndPosX, int EndPosY, int JumpDistance = 6)
        {
            int StartPosX = (int)StartPosition.X / 16, StartPosY = (int)StartPosition.Y / 16;
            int Attempts = 0;
            while (CheckForSolidBlocks(StartPosX, StartPosY, 3))
            {
                StartPosY--;
                Attempts++;
                if (Attempts >= 8)
                {
                    Main.NewText("Inside too many solid tiles.");
                    return new List<Breadcrumbs>();
                }
            }
            Attempts = 0;
            while (!CheckForSolidGround(StartPosX, StartPosY))
            {
                StartPosY++;
                Attempts++;
                if (Attempts >= 8)
                {
                    Main.NewText("No floor.");
                    return new List<Breadcrumbs>();
                }
            }
            List<Node> LastNodeList = new List<Node>(), NextNodeList = new List<Node>();
            List<Point> VisitedNodes = new List<Point>();
            NextNodeList.Add(new Node(StartPosX, StartPosY));
            VisitedNodes.Add(new Point(StartPosX, StartPosY));
            const int MaxDistance = 35;
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
                    if (Math.Abs(n.NodeX - StartPosX) >= MaxDistance || Math.Abs(n.NodeY - StartPosY) >= MaxDistance) //Skip path finding
                    {
                        continue;
                    }
                    int X = n.NodeX, Y = n.NodeY;
                    if (X == EndPosX && Y == EndPosY)
                    {
                        NodeFound = n;
                        break;
                    }
                    for (byte dir = 0; dir < 4; dir++)
                    {
                        switch (dir)
                        {
                            case Node.DIR_UP:
                                {
                                    bool HasPlatform = false, HasSolidBlock = false;
                                    int NextNodeY = -1;
                                    for (int y = -1; y >= -JumpDistance; y--)
                                    {
                                        if (CheckForSolidBlocks(X, Y + y))
                                        {
                                            HasSolidBlock = true;
                                            break;
                                        }
                                        if (CheckForPlatform(X, Y + y) && !CheckForPlatform(X, Y + y - 1))
                                        {
                                            HasPlatform = true;
                                            NextNodeY = Y + y - 1;
                                            break;
                                        }
                                    }
                                    if (HasSolidBlock)
                                        continue;
                                    if (HasPlatform && !VisitedNodes.Contains(new Point(X, NextNodeY)))
                                    {
                                        NextNodeList.Add(new Node(X, NextNodeY, Node.DIR_UP, n));
                                        VisitedNodes.Add(new Point(X, NextNodeY));
                                    }
                                }
                                break;
                            case Node.DIR_RIGHT:
                                {
                                    int nx = X + 1, ny = Y;
                                    for (int y = 0; y < JumpDistance; y++)
                                    {
                                        for (int d = -1; d <= 1; d += 2)
                                        {
                                            int thisypos = ny + y * d;
                                            if (CheckForSolidGround(nx, thisypos) && !CheckForStairFloor(nx, thisypos - 1))
                                            {
                                                int xc = nx, yc = thisypos;
                                                if (!VisitedNodes.Contains(new Point(xc, yc)))
                                                {
                                                    NextNodeList.Add(new Node(xc, yc, Node.DIR_RIGHT, n));
                                                    VisitedNodes.Add(new Point(xc, yc));
                                                }
                                            }
                                            if (y == 0)
                                                break;
                                        }
                                    }
                                }
                                break;
                            case Node.DIR_DOWN:
                                {
                                    if (AnyPlatform(X, Y + 1))
                                    {
                                        for (int y = 2; y <= JumpDistance; y++)
                                        {
                                            if ((CheckForSolidGround(X, Y + y) || CheckForSolidGround(X + 1, Y + y)) && !VisitedNodes.Contains(new Point(X, Y + y)))
                                            {
                                                NextNodeList.Add(new Node(X, Y + y, Node.DIR_DOWN, n));
                                                VisitedNodes.Add(new Point(X, Y + y));
                                                break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case Node.DIR_LEFT:
                                {
                                    int nx = X - 1, ny = Y;
                                    for (int y = 0; y < JumpDistance; y++)
                                    {
                                        for (int d = -1; d <= 1; d += 2)
                                        {
                                            int thisypos = ny + y * d;
                                            if (CheckForSolidGround(nx, thisypos) && !CheckForStairFloor(nx, thisypos - 1))
                                            {
                                                int xc = nx, yc = thisypos;
                                                if (!VisitedNodes.Contains(new Point(xc, yc)))
                                                {
                                                    NextNodeList.Add(new Node(xc, yc, Node.DIR_LEFT, n));
                                                    VisitedNodes.Add(new Point(xc, yc));
                                                }
                                            }
                                            if (y == 0)
                                                break;
                                        }
                                    }
                                }
                                break;

                        }
                    }
                    HangPreventer++;
                    if (HangPreventer >= 1000)
                    {
                        Main.NewText("Path finding hanged with " + NextNodeList.Count + " nodes to check and " + LastNodeList.Count + " nodes to review.");
                        return new List<Breadcrumbs>();
                    }
                }
            }
            List<Breadcrumbs> PathGuide = new List<Breadcrumbs>();
            byte LastDirection = 255;
            if (NodeFound == null)
            {
                Main.NewText("No nodes found.");
            }
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
            return PathGuide;
        }

        public static bool CheckForSolidGround(int px, int py)
        {
            byte State = 0;
            for (int y = 0; y < 2; y++)
            {
                Tile tile = MainMod.GetTile(px, py + y);
                if (y == 0 && (!tile.active() || !Main.tileSolid[tile.type]))
                {
                    State++;
                }
                if (y == 1 && tile.active() && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]))
                {
                    State++;
                }
            }
            return State >= 2;
        }

        public static bool CheckForStairFloor(int px, int py)
        {
            byte State = 0;
            for (int x = -1; x < 1; x++)
            {
                Tile tile = MainMod.GetTile(px + x, py);
                if (tile != null && tile.active() && tile.type == Terraria.ID.TileID.Platforms && (tile.rightSlope() || tile.leftSlope()))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckForSolidBlocks(int tx, int ty, int Height = 3)
        {
            for (int x = -1; x <= 0; x++)
            {
                for (int y = -(Height - 1); y <= 0; y++)
                {
                    Tile t = MainMod.GetTile(tx + x, ty + y);
                    if (t == null) return false;
                    if (t.active() && Main.tileSolid[t.type])
                        return true;
                }
            }
            return false;
        }

        public static bool CheckForPlatform(int tx, int ty)
        {
            byte PlatformTiles = 0;
            for (int x = -1; x <= 0; x++)
            {
                Tile t = MainMod.GetTile(tx + x, ty);
                if (t == null) return false;
                if (t.active() && Main.tileSolidTop[t.type])
                    PlatformTiles++;
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
                if (t.active() && Main.tileSolidTop[t.type])
                    PlatformTiles++;
            }
            return PlatformTiles > 0;
        }

        public class Node
        {
            public byte NodeDirection = 0;
            public const byte DIR_UP = 0, DIR_RIGHT = 1, DIR_DOWN = 2, DIR_LEFT = 3;
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

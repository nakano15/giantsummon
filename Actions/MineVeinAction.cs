using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Actions
{
    public class MineAction : GuardianActions
    {
        private List<Point> MineTiles = new List<Point>();
        private ushort DiggingIndex = 0;

        public MineAction(int OreX, int OreY)
        {
            Tile t = Framing.GetTileSafely(OreX, OreY);
            if(t == null || !t.active())
            {
                InUse = false;
                return;
            }
            int TileID = t.type;
            MineTiles.Add(new Point(OreX, OreY));
            byte TileIndex = 0;
            while(TileIndex < MineTiles.Count && TileIndex < 100)
            {
                for(int i = 0; i < 4; i++)
                {
                    int nx = MineTiles[TileIndex].X, ny = MineTiles[TileIndex].Y;
                    switch (i)
                    {
                        case 0:
                            ny--;
                            break;
                        case 1:
                            nx++;
                            break;
                        case 2:
                            ny++;
                            break;
                        case 3:
                            nx--;
                            break;
                    }
                    Tile New = Framing.GetTileSafely(nx, ny);
                    if(New != null && New.active() && New.type == TileID && !MineTiles.Any(x => x.X == nx && x.Y == ny))
                    {
                        MineTiles.Add(new Point(nx, ny));
                    }
                }
                TileIndex++;
            }
            Main.NewText("Ore Count: " + MineTiles.Count);
            BlockIdleAI = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (guardian.IsAttackingSomething)
            {
                return;
            }
            try
            {
                if (MineTiles.Count > 0)
                {
                    Point TilePos = MineTiles[DiggingIndex];
                    Tile tile = Main.tile[TilePos.X, TilePos.Y];
                    if (!tile.active() || !WorldGen.CanKillTile(TilePos.X, TilePos.Y))
                    {
                        MineTiles.RemoveAt(DiggingIndex);
                        DiggingIndex = 0;
                        return;
                    }
                    else
                    {
                        if (guardian.SelectedItem == -1 || guardian.Inventory[guardian.SelectedItem].pick == 0)
                        {
                            int StrongestPickaxeIndex = -1, StrongestPickaxeValue = 0;
                            for (int i = 0; i < 10; i++)
                            {
                                if (guardian.Inventory[i].type > 0 && guardian.Inventory[i].pick > StrongestPickaxeValue)
                                {
                                    StrongestPickaxeIndex = i;
                                    StrongestPickaxeValue = guardian.Inventory[i].pick;
                                }
                            }
                            if (StrongestPickaxeIndex == -1)
                            {
                                MineTiles.Clear();
                                InUse = false;
                                return;
                            }
                            guardian.SelectedItem = StrongestPickaxeIndex;
                        }
                        int ToolDistance = (int)((5 + guardian.Inventory[guardian.SelectedItem].tileBoost) * guardian.Scale);
                        float DistanceX = (guardian.Position.X - TilePos.X * 16 + 8) * (1f / 16), DistanceY = (guardian.CenterY * (1f / 16) - TilePos.Y * 16 + 8) * (1f / 16);
                        guardian.MoveLeft = guardian.MoveRight = guardian.Jump = false;
                        if (Math.Abs(DistanceX) >= ToolDistance)
                        {
                            /*float AheadX = guardian.Position.X;
                            if (DistanceX > 0)
                                AheadX -= guardian.CollisionWidth * 0.5f + 1;
                            else
                                AheadX += guardian.CollisionWidth * 0.5f + 1;
                            int tx = (int)(AheadX * (1f / 16));
                            for (int y = 0; y < 3; y++)
                            {
                                int ty = (int)(guardian.Position.Y * (1f / 16) - y);
                                if (Main.tile[tx, ty].active() && Main.tileSolid[Main.tile[tx, ty].type])
                                {
                                    for (byte i = 0; i < MineTiles.Count; i++)
                                    {
                                        if (MineTiles[i].X == tx && MineTiles[i].Y == ty)
                                        {
                                            DiggingIndex = i;
                                            return;
                                        }
                                    }
                                    DiggingIndex = (ushort)MineTiles.Count;
                                    MineTiles.Add(new Point(tx, ty));
                                    return;
                                }
                            }*/
                            if (DistanceX > 0)
                            {
                                guardian.MoveLeft = true;
                            }
                            else
                            {
                                guardian.MoveRight = true;
                            }
                        }
                        else if (Math.Abs(DistanceY) >= ToolDistance)
                        {
                            guardian.MoveLeft = guardian.MoveRight = guardian.Jump = false;
                            if (DistanceY > 0) //Above
                            {
                                if(guardian.Velocity.Y == 0 || guardian.JumpHeight > 0)
                                {
                                    guardian.Jump = true;
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            //dig
                            if (guardian.MoveCursorToPosition(new Vector2(TilePos.X * 16 + 8, TilePos.Y * 16 + 8), 4, 4))
                            {
                                guardian.Action = true;
                            }
                        }
                    }
                }
                else
                {
                    InUse = false;
                }
            }
            catch
            {
                InUse = false;
            }
        }
    }
}

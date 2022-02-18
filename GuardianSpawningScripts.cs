using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class GuardianSpawningScripts
    {
        private static readonly char[,] HouseBlueprint = new char[,]
        {
            { ' ', ' ', ' ', ' ', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', ' ', ' ', ' ', ' ' },
            { ' ', ' ', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', ' ', ' ' },
            { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
            { ' ', ' ', 'x', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'x', ' ', ' ' },
            { ' ', ' ', 'x', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'x', ' ', ' ' },
            { ' ', ' ', 'x', 'l', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'l', 'x', ' ', ' ' },
            { ' ', ' ', 'x', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'x', ' ', ' ' },
            { ' ', ' ', 'x', 'w', 'g', 'g', 'g', 'g', 'w', 'w', 'g', 'g', 'g', 'g', 'w', 'x', ' ', ' ' },
            { ' ', ' ', ' ', 'w', 'g', 'g', 'g', 'g', 'w', 'w', 'v', 'g', 'g', 'g', 'w', ' ', ' ', ' ' },
            { ' ', ' ', ' ', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', ' ', ' ', ' ' },
            { ' ', 'w', 'd', 'w', 'w', 'b', 'w', 'w', 'w', 'w', 't', 'w', 'w', 'c', 'w', 'd', 'w', ' ' },
            { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }
        };

        public static int CilleShelterX = -1, CilleShelterY = -1;

        private static byte CilleSpawnCheckDelay = 0;

        private static WorldMod.GuardianTownNpcState CilleTownNpcState = null;

        private static TerraGuardian CilleGuardian = null;

        public static void TrySpawningMichelle()
        {
            if (NpcMod.HasMetGuardian(GuardianBase.Michelle) || WorldMod.IsGuardianNpcInWorld(GuardianBase.Michelle))
                return;
            if (Main.fastForwardTime || Main.eclipse || !Main.dayTime || Main.time >= 27000 || Main.time % 60 > 0)
            {
                return;
            }
            if (Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0)
                return;
            if (Main.rand.Next(17) == 0)
            {
                bool HasPlayerWithDefense = false;
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && Main.player[p].statDefense > 0)
                    {
                        HasPlayerWithDefense = true;
                        break;
                    }
                }
                if (HasPlayerWithDefense)
                {
                    NpcMod.SpawnGuardianNPC(Main.spawnTileX * 16, Main.spawnTileY * 16, GuardianBase.Michelle);
                    //NPC.NewNPC(Main.spawnTileX * 16, Main.spawnTileY * 16, ModContent.NPCType<MichelleGuardian>());
                    Main.NewText("Michelle has logged in.", 255, 255, 0);
                }
            }
        }

        public static void CilleSpawningScripts()
        {
            const byte CheckDelayTime = 150;
            const int CilleID = GuardianBase.Cille;
            CilleSpawnCheckDelay++;
            if (CilleSpawnCheckDelay < CheckDelayTime)
                return;
            CilleSpawnCheckDelay -= CheckDelayTime;
            if (Main.rand.Next(7) > 0)
                return;
            if (!NpcMod.HasMetGuardian(CilleID) || NPC.AnyNPCs(ModContent.NPCType<Npcs.CilleNPC>()))
                return;
            if (NpcMod.HasGuardianNPC(CilleID))
            {
                TerraGuardian tg = NpcMod.GetGuardianNPCCharacter(CilleID);
                if (tg.FriendshipLevel >= tg.Base.MoveInLevel)
                    return;
                for(int i = 0; i < 255; i++)
                {
                    if(Main.player[i].active && Math.Abs(tg.Position.X - Main.player[i].Center.X) < 2000 && Math.Abs(tg.Position.Y - Main.player[i].Center.Y) < 1600)
                    {
                        return;
                    }
                }
            }
            if(CilleShelterX == -1 && CilleShelterY == -1)
            {
                int Left = (int)(Main.leftWorld * (1f / 16) + 130), Right = (int)(Main.rightWorld * (1f / 16) - 130);
                int Top = (int)(Main.worldSurface * 0.35f), Bottom = (int)(Main.worldSurface);
                int PositionX = Main.rand.Next(Left, Right), 
                    PositionY = Main.rand.Next(Top, Bottom);
                Tile tile = Framing.GetTileSafely(PositionX, PositionY);
                if (tile == null || (tile.active() && Main.tileSolid[tile.type]) || tile.wall > 0)
                {
                    return;
                }
                {
                    byte Counter = 0;
                    while (!tile.active() || !Main.tileSolid[tile.type])
                    {
                        PositionY++;
                        tile = Framing.GetTileSafely(PositionX, PositionY);
                        if (Counter++ >= 250)
                            return;
                    }
                }
                tile = Framing.GetTileSafely(PositionX, PositionY);
                switch (tile.type)
                {
                    case Terraria.ID.TileID.CorruptGrass:
                    case Terraria.ID.TileID.Ebonstone:
                    case Terraria.ID.TileID.Ebonsand:
                    case Terraria.ID.TileID.FleshGrass:
                    case Terraria.ID.TileID.Crimtane:
                    case Terraria.ID.TileID.Crimsand:
                    case Terraria.ID.TileID.LeafBlock:
                    case Terraria.ID.TileID.LivingWood:
                    case Terraria.ID.TileID.BlueDungeonBrick:
                    case Terraria.ID.TileID.GreenDungeonBrick:
                    case Terraria.ID.TileID.PinkDungeonBrick:
                        return;
                }
                PositionY--;
                tile = Framing.GetTileSafely(PositionX, PositionY);
                if (tile.wall > 0) return;
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active)
                    {
                        if(Math.Abs(Main.player[i].Center.X - PositionX * 16) < 1000 && Math.Abs(Main.player[i].Center.Y - PositionY * 16) < 1000)
                        {
                            return;
                        }
                    }
                    if(i < 200 && Main.npc[i].active)
                    {
                        if (Math.Abs(Main.npc[i].Center.X - PositionX * 16) < 1000 && Math.Abs(Main.npc[i].Center.Y - PositionY * 16) < 800)
                        {
                            return;
                        }
                    }
                }
                int HouseBottom;
                if (!TryPlacingCilleHouse(PositionX, PositionY, out HouseBottom))
                {
                    return;
                }
                CilleShelterX = PositionX;
                CilleShelterY = HouseBottom;
                /*if (!NpcMod.HasGuardianNPC(CilleID))
                {
                    NpcMod.SpawnGuardianNPC(PositionX * 16, PositionY * 16, CilleID);
                }
                else
                {
                    TerraGuardian Cille = NpcMod.GetGuardianNPCCharacter(CilleID);
                    Cille.Position.X = PositionX * 16;
                    Cille.Position.Y = PositionY * 16;
                    Cille.SetFallStart();
                }*/
                WorldMod.TrySpawningOrMovingGuardianNPC(CilleID, "", CilleShelterX, CilleShelterY, true, true);
            }
            else
            {
                if (!NpcMod.HasGuardianNPC(CilleID))
                {
                    if (!WorldMod.CanGuardianNPCSpawnInTheWorld(CilleID))
                    {
                        if (Main.dayTime && Main.moonPhase != 4)
                        {
                            if (WorldMod.TrySpawningOrMovingGuardianNPC(CilleID, "", CilleShelterX, CilleShelterY, true, true))
                            {
                                //NpcMod.SpawnGuardianNPC(CilleShelterX * 16, CilleShelterY * 16, CilleID);
                            }
                            else
                            {
                                CilleShelterX = CilleShelterY = -1;
                            }
                        }
                    }
                }
                else
                {
                    /*if (!Main.dayTime && !WorldMod.CanGuardianNPCSpawnInTheWorld(CilleID))
                    {
                        WorldMod.GuardianTownNpcState townnpc = CilleGuardian.GetTownNpcInfo;
                        if (townnpc != null)
                        {
                            townnpc.Homeless = true;
                        }
                    }*/
                }
            }
        }

        public static bool TryPlacingCilleHouse(int PositionX, int PositionY, out int HouseBottom)
        {
            HouseBottom = PositionY;
            Tile tile = Framing.GetTileSafely(PositionX, PositionY);
            if (tile == null || (tile.active() && Main.tileSolid[tile.type]))
            {
                return false;
            }
            {
                byte Counter = 0;
                while (!tile.active() || !Main.tileSolid[tile.type])
                {
                    PositionY++;
                    tile = Framing.GetTileSafely(PositionX, PositionY);
                    if (Counter++ >= 250)
                        return false;
                }
            }
            if (tile.slope() > 0)
                return false;
            int Width = HouseBlueprint.GetLength(1), 
                Height = HouseBlueprint.GetLength(0);
            HouseBottom = PositionY - 2;
            PositionX -= Width / 2;
            PositionY -= Height;
            bool Blocked = false;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    tile = Framing.GetTileSafely(PositionX + x, PositionY + y);
                    if (Main.wallHouse[tile.wall])
                        return false;
                    if (tile.liquid > 0 && y < Height - 1)
                    {
                        Blocked = true;
                        break;
                    }
                    if (tile.type == Terraria.ID.TileID.Containers || tile.type == Terraria.ID.TileID.Containers2)
                    {
                        Blocked = true;
                        break;
                    }
                    /*if(tile.active() && Main.tileSolid[tile.type] && (x == 0 || x == Width - 1))
                    {
                        Blocked = true;
                        break;
                    }*/
                }
                if (Blocked)
                    break;
            }
            if (Blocked)
                return false;
            {
                bool BlockedLeft = false, BlockedRight = false;
                int TileX = PositionX - 1, TileY = PositionY + Height - 2;
                tile = Framing.GetTileSafely(TileX, TileY);
                BlockedLeft = (tile != null && tile.active() && Main.tileSolid[tile.type]);
                TileX = PositionX + Width;
                tile = Framing.GetTileSafely(TileX, TileY);
                BlockedRight = (tile != null && tile.active() && Main.tileSolid[tile.type]);
                if (BlockedLeft || BlockedRight)
                    return false;
            }
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    tile = Framing.GetTileSafely(PositionX + x, PositionY + y);
                    WorldGen.KillTile(PositionX + x, PositionY + y, false, false, true);
                    tile.active(false);
                    tile.slope(0);
                    tile.halfBrick(false);
                    tile.liquid = 0;
                    switch (HouseBlueprint[y, x])
                    {
                        case 'x':
                            tile.active(true);
                            tile.type = Terraria.ID.TileID.WoodBlock;
                            break;
                        case 'w':
                            tile.wall = Terraria.ID.WallID.Wood;
                            break;
                    }
                }
            }
            for (int y = Height -1; y >= 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    tile = Framing.GetTileSafely(PositionX + x, PositionY + y);
                    switch (HouseBlueprint[y, x])
                    {
                        case 'c':
                            WorldGen.PlaceTile(PositionX + x, PositionY + y, Terraria.ID.TileID.Chairs);
                            tile.wall = Terraria.ID.WallID.Wood;
                            break;
                        case 't':
                            WorldGen.PlaceTile(PositionX + x, PositionY + y, Terraria.ID.TileID.Tables);
                            tile.wall = Terraria.ID.WallID.Wood;
                            break;
                        case 'b':
                            WorldGen.PlaceTile(PositionX + x, PositionY + y, Terraria.ID.TileID.Beds);
                            tile.wall = Terraria.ID.WallID.Wood;
                            break;
                        case 'v':
                            WorldGen.PlaceTile(PositionX + x, PositionY + y, Terraria.ID.TileID.ClayPot);
                            tile.wall = Terraria.ID.WallID.Glass;
                            break;
                        case 'g':
                            tile.wall = Terraria.ID.WallID.Glass;
                            break;
                        case 'l':
                            tile.type = Terraria.ID.TileID.Torches;
                            tile.active(true);
                            tile.wall = Terraria.ID.WallID.Wood;
                            break;
                        case 'd':
                            WorldGen.PlaceTile(PositionX + x, PositionY + y, Terraria.ID.TileID.ClosedDoor);
                            //tile.type = Terraria.ID.TileID.ClosedDoor;
                            break;
                    }
                }
            }
            
            for (int x = 0; x < Width; x++)
            {
                int TileX = PositionX + x;
                int TileY = PositionY + Height;
                List<int> YToFill = new List<int>();
                ushort TileID = 0;
                tile = Framing.GetTileSafely(TileX, TileY);
                bool AbortFilling = false;
                while (!(tile.active()  && Main.tileSolid[tile.type]))
                {
                    YToFill.Add(TileY++);
                    tile = Framing.GetTileSafely(TileX, TileY);
                    if (YToFill.Count >= 6)
                    {
                        AbortFilling = true;
                        break;
                    }
                }
                if (AbortFilling)
                    continue;
                tile.slope(0);
                tile.halfBrick(false);
                TileID = tile.type;
                if (TileID == Terraria.ID.TileID.Grass)
                    TileID = Terraria.ID.TileID.Dirt;
                if (TileID == Terraria.ID.TileID.JungleGrass || TileID == Terraria.ID.TileID.MushroomGrass)
                    TileID = Terraria.ID.TileID.Mud;
                foreach (int y in YToFill)
                {
                    WorldGen.KillTile(TileX, y, false, false, true);
                    tile = Framing.GetTileSafely(TileX, y);
                    tile.type = TileID;
                    tile.active(true);
                    tile.slope(0);
                    for(int x2 = -1; x2 < 2; x2++)
                    {
                        for(int y2 = -1; y2 < 2; y2++)
                        {
                            WorldGen.TileFrame(TileX + x2, y + y2);
                        }
                    }
                }
            }
            for (sbyte i = -1; i < 2; i += 2) //This method should try making stairs to the house.
            {
                int StairX = PositionX, StairY = PositionY + Height - 1;
                if (i == 1)
                {
                    StairX += Width;
                }
                else
                {
                    StairX--;
                }
                bool HitGround = false;
                List<Microsoft.Xna.Framework.Point> StepPlacementPosition = new List<Microsoft.Xna.Framework.Point>();
                for (int x = 0; x < 8; x++) //Should try in 5 runs hitting the ground. If it hits the ground, then create the stair.
                {
                    int StepX = StairX + x * i, StepY = StairY + Math.Abs(x);
                    tile = Framing.GetTileSafely(StepX, StepY);
                    if(tile.active() && Main.tileSolid[tile.type])
                    {
                        HitGround = true;
                        break;
                    }
                    if (tile.liquid > 0)
                        break;
                    StepPlacementPosition.Add(new Microsoft.Xna.Framework.Point(StepX, StepY));
                }
                if (HitGround)
                {
                    foreach(Microsoft.Xna.Framework.Point step in StepPlacementPosition)
                    {
                        WorldGen.KillTile(step.X, step.Y, false, false, true);
                        tile = Framing.GetTileSafely(step.X, step.Y);
                        tile.active(true);
                        tile.type = Terraria.ID.TileID.Platforms;
                        WorldGen.TileFrame(step.X, step.Y);
                    }
                }
            }
            for (int y = -1; y <= Height; y++)
            {
                for (int x = -1; x <= Width; x++)
                {
                    WorldGen.TileFrame(PositionX + x, PositionY + y, true, true);
                }
            }
            return true;
        }
    }
}

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
            { ' ', ' ', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', ' ', ' ' },
            { ' ', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', ' ' },
            { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' },
            { ' ', 'x', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'x', ' ' },
            { ' ', 'x', 'l', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'l', 'x', ' ' },
            { ' ', 'x', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'x', ' ' },
            { ' ', 'x', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'x', ' ' },
            { ' ', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', ' ' },
            { ' ', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', ' ' },
            { 'w', 'd', 'w', 'w', 'c', 'w', 'w', 't', 'w', 'w', 'w', 'w', 'w', 'w', 'd', 'w' },
            { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }
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
            const int CilleID = GuardianBase.Cille;
            CilleSpawnCheckDelay++;
            if (CilleSpawnCheckDelay < 60)
                return;
            CilleSpawnCheckDelay -= 60;
            if (!NpcMod.HasMetGuardian(CilleID))
                return;
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
                    case Terraria.ID.TileID.EbonstoneBrick:
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
                Main.NewText("Cille's house spawned at X " + CilleShelterX + "  Y " + CilleShelterY); //Remove after debugging.
                if (!NpcMod.HasGuardianNPC(CilleID))
                {
                    NpcMod.SpawnGuardianNPC(PositionX * 16, PositionY * 16, CilleID);
                }
                WorldMod.TrySpawningOrMovingGuardianNPC(CilleID, "", CilleShelterX, CilleShelterY);
                //TODO - Remove after debugging.
                CilleShelterY = CilleShelterX = -1;
            }
            else
            {
                if (!NpcMod.HasGuardianNPC(CilleID) && !WorldMod.CanGuardianNPCSpawnInTheWorld(CilleID))
                {
                    if(Main.dayTime && Main.moonPhase > 3)
                    {
                        WorldMod.TrySpawningOrMovingGuardianNPC(CilleID, "", CilleShelterX, CilleShelterY);
                    }
                }
            }
            /*if (NpcMod.HasMetGuardian(CilleID) && !NPC.AnyNPCs(ModContent.NPCType<Npcs.CilleNPC>()))
            {
                if (!NpcMod.HasGuardianNPC(CilleID)) //Try finding a shelter for cille, and spawning her
                {
                    if (WorldMod.HasEmptyGuardianNPCSlot() && TryFindingHomeForCille())
                    {
                        NpcMod.SpawnGuardianNPC(CilleShelterX, CilleShelterY, CilleID);
                        WorldMod.AllowGuardianNPCToSpawn(CilleID);
                        Main.NewText("Cille has spawned at X: " + CilleShelterX + " Y: " + CilleShelterY);
                        foreach(WorldMod.GuardianTownNpcState tns in WorldMod.GuardianNPCsInWorld)
                        {
                            if (tns != null && tns.CharID.IsSameID(CilleID))
                            {
                                CilleTownNpcState = tns;
                                break;
                            }
                        }
                    }
                }
                else //Enforce where she lives
                {
                    if(CilleGuardian == null)
                    {
                        foreach(TerraGuardian tg in WorldMod.GuardianTownNPC)
                        {
                            if (tg.MyID.IsSameID(CilleID))
                            {
                                CilleGuardian = tg;
                                break;
                            }
                        }
                    }
                    if(CilleTownNpcState != null)
                    {
                        if(CilleShelterX == -1 || CilleShelterY == -1)
                        {
                            if (!TryFindingHomeForCille())
                            {
                                return;
                            }
                        }
                        if (CilleTownNpcState.Homeless)
                        {
                            CilleTownNpcState.Homeless = false;
                            CilleTownNpcState.HomeX = CilleShelterX;
                            CilleTownNpcState.HomeY = CilleShelterY;
                        }
                    }
                    else
                    {
                        foreach (WorldMod.GuardianTownNpcState tns in WorldMod.GuardianNPCsInWorld)
                        {
                            if (tns != null && tns.CharID.IsSameID(CilleID))
                            {
                                CilleTownNpcState = tns;
                                break;
                            }
                        }
                        if (CilleTownNpcState == null && CilleGuardian != null && CilleGuardian.FriendshipLevel < CilleGuardian.Base.MoveInLevel && !WorldMod.CanGuardianNPCSpawnInTheWorld(CilleID))
                        {
                            if (WorldMod.HasEmptyGuardianNPCSlot() && TryFindingHomeForCille())
                            {
                                WorldMod.AllowGuardianNPCToSpawn(CilleID);
                                foreach (WorldMod.GuardianTownNpcState tns in WorldMod.GuardianNPCsInWorld)
                                {
                                    if (tns.CharID.IsSameID(CilleID))
                                    {
                                        CilleTownNpcState = tns;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }*/
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
            HouseBottom = PositionY - 1;
            PositionX -= Width / 2;
            PositionY -= Height;
            bool Blocked = false;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    tile = Framing.GetTileSafely(PositionX + x, PositionY + y);
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
            for (byte Try = 0; Try < 2; Try++)
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        tile = Framing.GetTileSafely(PositionX + x, PositionY + y);
                        if (Try == 0)
                        {
                            if (tile.type == Terraria.ID.TileID.Trees || tile.type == Terraria.ID.TileID.PalmTree || tile.type == Terraria.ID.TileID.PineTree ||
                                tile.type == Terraria.ID.TileID.MushroomTrees)
                            {
                                WorldGen.KillTile(PositionX + x, PositionY + y, false, false, true);
                            }
                            tile.active(false);
                            tile.slope(0);
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
                        else if (Try == 1)
                        {
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
                                case 'l':
                                    tile.type = Terraria.ID.TileID.Torches;
                                    tile.wall = Terraria.ID.WallID.Wood;
                                    break;
                                case 'd':
                                    WorldGen.PlaceTile(PositionX + x, PositionY + y, Terraria.ID.TileID.ClosedDoor);
                                    //tile.type = Terraria.ID.TileID.ClosedDoor;
                                    break;
                            }
                        }
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
                while (!tile.active() || !Main.tileSolid[tile.type])
                {
                    YToFill.Add(PositionY++);
                    tile = Framing.GetTileSafely(TileX, TileY);
                    if (YToFill.Count >= 5)
                    {
                        AbortFilling = true;
                        break;
                    }
                }
                if (AbortFilling)
                    continue;
                if (!tile.active() || !Main.tileSolid[tile.type])
                    continue;
                TileID = tile.type;
                foreach(int y in YToFill)
                {
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
            for(byte i =0; i < 2; i++) //This method should try making stairs to the house.
            {
                int StairX = PositionX - 1, StairY = PositionY + Height - 1;
                if(i == 1)
                {
                    StairX += Width + 1;
                    bool HitGround = false;
                    for(int x = 0; x < 5; x++) //Should try in 5 runs hitting the ground. If it hits the ground, then create the stair.
                    {

                    }
                }
                List<Microsoft.Xna.Framework.Point> StairPlacementPosition = new List<Microsoft.Xna.Framework.Point>();
                
            }
            for (int y = -1; y <= Height; y++)
            {
                for (int x = -1; x <= Width; x++)
                {
                    WorldGen.TileFrame(PositionX + x, PositionY + y, false, true);
                }
            }
            return true;
        }

        private static bool TryFindingHomeForCille() //Returns true if successfull
        {
            int Left = (int)(Main.leftWorld * (1f / 16) + 130), Right = (int)(Main.rightWorld * (1f / 16) - 130);
            int Top = (int)(Main.worldSurface * 0.35f), Bottom = (int)(Main.worldSurface - 4);
            int PositionX = Left + Main.rand.Next(Right - Left), PositionY = Top + Main.rand.Next(Bottom - Top);
            Tile tile = Framing.GetTileSafely(PositionX, PositionY);
            if (tile != null && !tile.active() && tile.liquid == 0 && (tile.wall == Terraria.ID.WallID.DirtUnsafe || tile.wall == Terraria.ID.WallID.DirtUnsafe1 ||
                tile.wall == Terraria.ID.WallID.DirtUnsafe2 || tile.wall == Terraria.ID.WallID.DirtUnsafe3 || tile.wall == Terraria.ID.WallID.DirtUnsafe4))
            {
                while (!tile.active() && tile.liquid == 0 && Main.tileSolid[tile.type])
                {
                    PositionY++;
                    tile = Framing.GetTileSafely(PositionX, PositionY);
                }
                if (PositionY >= Main.worldSurface)
                    return false;
                PositionY -= 3;
                if (tile.liquid > 0)
                    return false;
                CilleShelterX = PositionX;
                CilleShelterY = PositionY;
                return true;
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon
{
    public class AlexRecruitScripts
    {
        public const string AlexOldPartner = "Irene";
        public static readonly char[,] TombstonePedestal = new char[,]{ //Maybe leaving it simple...
            {'T','T'},
            {'T','T'}
        };
            
            /*new char[,]{
            {' ',' ',' ',' '},
            {' ','T','T',' '},
            {' ','T','T',' '},
            {' ','B','B',' '},
            {'B','B','B','B'}
        };*/
        public const string TombstoneText = "Here lies "+AlexOldPartner+".\n\'Brave adventurer and Alex's best friend.\'\n\n\"Take good care of Alex.\"";
        public static int TombstoneTileX = 0, TombstoneTileY = 0;
        public static bool SpawnedTombstone = false;
        public static int AlexNPCPosition = -1;

        public static void Save(Terraria.ModLoader.IO.TagCompound tag)
        {
            tag.Add("TombstoneExists", SpawnedTombstone);
            tag.Add("TombstonePosX", TombstoneTileX);
            tag.Add("TombstonePosY", TombstoneTileY);
        }

        public static void Load(Terraria.ModLoader.IO.TagCompound tag, int ModVersion)
        {
            SpawnedTombstone = tag.GetBool("TombstoneExists");
            TombstoneTileX = tag.GetInt("TombstonePosX");
            TombstoneTileY = tag.GetInt("TombstonePosY");
            AlexNPCPosition = NPC.FindFirstNPC(ModContent.NPCType<Npcs.AlexNPC>());
        }

        public static void TrySpawningTombstone(Terraria.World.Generation.GenerationProgress status)
        {
            const int TombstoneWidth = 2, TombstoneHeight = 2;
            int StartPosX = 0, StartPosY = 0;
        retry:
            int Attempt = 0;
            while (true)
            {
                status.Message = "Trying to spawn a tombstone. (" + (Attempt++) + ")";
                StartPosX = Main.rand.Next((int)Main.leftWorld / 16, (int)Main.rightWorld / 16 - TombstoneWidth);
                StartPosY = Main.rand.Next((int)(Main.worldSurface * 0.35f), (int)Main.worldSurface - TombstoneHeight);
                //StartPosY = (int)Main.topWorld / 16;
                bool AtLeastOnePositionFound = false, PassedSurface = false;
                while (true)
                {
                    bool Blocked = false;
                    for (int x = 0; x < TombstoneWidth; x++)
                    {
                        if (Main.tile[StartPosX + x, StartPosY].active())
                        {
                            Blocked = true;
                            break;
                        }
                    }
                    if (!Blocked)
                    {
                        AtLeastOnePositionFound = true;
                    }
                    else
                    {
                        break;
                    }
                    StartPosY++;
                    if (StartPosY >= Main.worldSurface)
                    {
                        PassedSurface = true;
                        break;
                    }
                }
                if (!AtLeastOnePositionFound || PassedSurface)
                {
                    continue;
                }
                StartPosY -= TombstoneHeight;
                bool SomeTileInTheWay = false;
                for (int y = 0; y < TombstoneHeight; y++)
                {
                    for (int x = 0; x < TombstoneWidth; x++)
                    {
                        if (Main.tile[StartPosX + x, StartPosY + y].active() || 
                            Main.tile[StartPosX + x, StartPosY + y].liquid > 0)
                        {
                            SomeTileInTheWay = true;
                            break;
                        }
                        if (Main.tile[StartPosX + x, StartPosY + y].wall > 0)
                        {
                            SomeTileInTheWay = true;
                            break;
                        }
                    }
                }
                bool NonSolidTileBellow = false;
                for (int x = 0; x < TombstoneWidth; x++)
                {
                    int TileY = StartPosY + TombstoneHeight + 1;
                    if (Main.tile[StartPosX + x, TileY].nactive() || !Main.tileSolid[Main.tile[StartPosX + x, TileY].type])
                    {
                        NonSolidTileBellow = true;
                        break;
                    }
                    if (Main.tile[StartPosX + x, TileY].halfBrick() || Main.tile[StartPosX + x, TileY].slope() > 0)
                    {
                        NonSolidTileBellow = true;
                        break;
                    }
                    if (Main.tile[StartPosX + x, TileY].type == Terraria.ID.TileID.Cloud || Main.tile[StartPosX + x, TileY].type == Terraria.ID.TileID.RainCloud || 
                        Terraria.ID.TileID.Sets.Corrupt[Main.tile[StartPosX + x, TileY].type])
                    {
                        NonSolidTileBellow = true;
                        break;
                    }
                }
                if (!SomeTileInTheWay && !NonSolidTileBellow)
                {
                    break;
                }
                if (Attempt >= 500)
                {
                    status.Message = "Unable to spawn Tombstone.";
                    Attempt = 0;
                    while (Attempt++ < 300)
                    {

                    }
                }
                //status.Message = "Failed at PosX = " + StartPosX + "  PosY = " + StartPosY + ".";
            }
            //status.Message = "Tombstone at PosX = " + StartPosX + "  PosY = " + StartPosY + ".";
            byte TombstoneFrameX = 0, TombstoneFrameY = 0;
            int TombstonePosX = 0, TombstonePosY = 0;
            for (int y = 0; y < TombstoneHeight; y++)
            {
                TombstoneFrameX = 0;
                bool HasTombstoneTile = false;
                for (int x = 0; x < TombstoneWidth; x++)
                {
                    int TilePosX = StartPosX + x, TilePosY = StartPosY + y;
                    Tile tile = Main.tile[TilePosX, TilePosY];
                    switch (TombstonePedestal[x, y])
                    {
                        case 'T':
                            if (TombstoneFrameX == 0 && TombstoneFrameY == 0)
                            {
                                TombstonePosX = TilePosX;
                                TombstonePosY = TilePosY;
                            }
                            tile.active(true);
                            tile.type = TileID.Tombstones;
                            HasTombstoneTile = true;
                            tile.frameX = (short)(TombstoneFrameX * 18);
                            tile.frameY = (short)(TombstoneFrameY * 18);
                            if (TombstoneFrameX == 1 && TombstoneFrameY == 1)
                            {
                                TombstoneTileX = TilePosX;
                                TombstoneTileY = TilePosY + 1;
                            }
                            TombstoneFrameX++;
                            break;
                        case 'B':
                            tile.active(true);
                            tile.type = TileID.StoneSlab;
                            WorldGen.TileFrame(TilePosX, TilePosY, true);
                            break;
                    }
                }
                if (HasTombstoneTile)
                {
                    TombstoneFrameY++;
                }
            }
            if (TombstonePosX == 0 || TombstonePosY == 0 || TombstoneTileX == 0 || TombstoneTileY == 0)
            {
                goto retry;
            }
            int signpos = Sign.ReadSign(TombstonePosX, TombstonePosY);
            if (signpos > -1)
                Sign.TextSign(signpos, TombstoneText);
            NPC.NewNPC(TombstoneTileX * 16, TombstoneTileY * 16, ModContent.NPCType<Npcs.AlexNPC>());
            SpawnedTombstone = true;
        }

        public static void CheckIfAlexIsInTheWorld()
        {
            if (TombstoneTileX > 0 && !NpcMod.HasMetGuardian(5) && !NPC.AnyNPCs(ModContent.NPCType<Npcs.AlexNPC>()))
                NPC.NewNPC(TombstoneTileX * 16, TombstoneTileY * 16, ModContent.NPCType<Npcs.AlexNPC>());
        }
    }
}

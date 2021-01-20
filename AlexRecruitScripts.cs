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
        /*    new char[,]{
            {' ',' ',' ',' '},
            {' ','T','T',' '},
            {' ','T','T',' '},
            {' ','B','B',' '},
            {'B','B','B','B'}
        };*/
        const int TombstoneWidth = 2, TombstoneHeight = 2;
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
            try
            {
                int StartPosX = 0, StartPosY = 0;
                byte RetryCount = 0;
            retry:
                RetryCount++;
                if(RetryCount >= 200)
                {
                    status.Message = "Nowhere to spawn " + AlexOldPartner + "'s tombstone...";
                    System.Threading.Thread.Sleep(3000);
                    return;
                }
                int Attempt = 0;
                while (true)
                {
                    Attempt++;
                    status.Message = "Trying to spawn a tombstone.";
                    StartPosX = Main.rand.Next((int)Main.leftWorld / 16, (int)Main.rightWorld / 16 - TombstoneWidth);
                    StartPosY = Main.rand.Next((int)(Main.worldSurface * 0.35f), (int)Main.worldSurface - TombstoneHeight);
                    //StartPosY = (int)Main.topWorld / 16;
                    bool AtLeastOnePositionFound = false, PassedSurface = false;
                    while (true)
                    {
                        byte Blocked = 0;
                        for (int x = 0; x < TombstoneWidth; x++)
                        {
                            if (Main.tile[StartPosX + x, StartPosY].active())
                            {
                                Blocked++;
                                break;
                            }
                        }
                        if (Blocked == 0)
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
                    while (!NonSolidTileBellow)
                    {
                        int TileY = StartPosY + TombstoneHeight + 1;
                        bool InvalidSpawnPosition = false;
                        for (int x = 0; x < TombstoneWidth; x++)
                        {
                            if (Main.tile[StartPosX + x, TileY].nactive() || !Main.tileSolid[Main.tile[StartPosX + x, TileY].type])
                            {
                                NonSolidTileBellow = true;
                                break;
                            }
                            if (Main.tile[StartPosX + x, TileY].halfBrick() || Main.tile[StartPosX + x, TileY].slope() > 0)
                            {
                                InvalidSpawnPosition = true;
                                break;
                            }
                            if (Main.tile[StartPosX + x, TileY].type == Terraria.ID.TileID.Cloud || Main.tile[StartPosX + x, TileY].type == Terraria.ID.TileID.RainCloud ||
                                Terraria.ID.TileID.Sets.Corrupt[Main.tile[StartPosX + x, TileY].type])
                            {
                                InvalidSpawnPosition = true;
                                break;
                            }
                        }
                        if (InvalidSpawnPosition)
                            goto retry;
                        if (!NonSolidTileBellow)
                            StartPosY++;
                    }
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
                    if (!SomeTileInTheWay && NonSolidTileBellow)
                    {
                        break;
                    }
                    if (Attempt >= 500)
                    {
                        status.Message = "Unable to spawn Tombstone.";
                        Attempt = 0;
                        while (Attempt++ < 600)
                        {

                        }
                        return;
                    }
                }
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
                {
                    if (Main.sign[signpos] == null)
                        Main.sign[signpos] = new Sign();
                    Sign.TextSign(signpos, TombstoneText);
                }
                NPC.NewNPC(TombstoneTileX * 16, TombstoneTileY * 16, ModContent.NPCType<Npcs.AlexNPC>());
                SpawnedTombstone = true;
            }
            catch
            {
                status.Message = AlexOldPartner+"'s tombstone failed to appear in this world...";
                System.Threading.Thread.Sleep(3000);
                SpawnedTombstone = false;
            }
        }

        public static void CheckIfAlexIsInTheWorld()
        {
            if (TombstoneTileX > 0 && !NpcMod.HasMetGuardian(5) && !NPC.AnyNPCs(ModContent.NPCType<Npcs.AlexNPC>()))
                NPC.NewNPC(TombstoneTileX * 16, TombstoneTileY * 16, ModContent.NPCType<Npcs.AlexNPC>());
        }
    }
}

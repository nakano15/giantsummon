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
            {
                int PositionX = 0, PositionY = 0;
                ushort Retry = 0;
                bool Success = false;
                while (!Success)
                {
                    PositionX = Main.rand.Next((int)Main.leftWorld / 16 + 2, (int)Main.rightWorld / 16 - 4);
                    PositionY = Main.rand.Next((int)(Main.worldSurface * 0.35f), (int)(Main.worldSurface * 0.95f));
                    bool BlockedTilePosition = false;
                    for(int x = -1; x < 2; x++)
                    {
                        Tile tile = Main.tile[PositionX + x, PositionY];
                        if((tile.active() && Main.tileSolid[tile.type]) || tile.liquid > 0)
                        {
                            BlockedTilePosition = true;
                            break;
                        }
                    }
                    if (!BlockedTilePosition)
                    {
                        bool GroundBellow = false;
                        while (true)
                        {
                            byte FloorCount = 0;
                            PositionY++;
                            if (PositionY >= Main.worldSurface)
                            {
                                break;
                            }
                            for (int x = -1; x < 2; x++)
                            {
                                Tile tile = Main.tile[PositionX + x, PositionY];
                                if (tile.active() && Main.tileSolid[tile.type] && tile.type != 192 && tile.liquid == 0)
                                {
                                    FloorCount++;
                                }
                            }
                            if (FloorCount == 1)
                            {
                                break;
                            }
                            else if (FloorCount == 2)
                            {
                                GroundBellow = true;
                                break;
                            }
                        }
                        if (GroundBellow)
                        {
                            PositionY -= 1;
                            //PositionX--;
                            TileObject to;
                            TileObject.CanPlace(PositionX, PositionY, TileID.Tombstones, Main.rand.Next(255) == 0 ? 9 : 4, 1, out to);
                            //WorldGen.PlaceTile(PositionX, PositionY, TileID.Tombstones, true, false, -1, Main.rand.Next(255) == 0 ? 9 : 4);
                            //status.Message = "Tombstone tried spawning at X:" + PositionX + "  Y:" + PositionY;
                            if (TileObject.CanPlace(PositionX, PositionY, TileID.Tombstones, Main.rand.Next(255) == 0 ? 9 : 4, 1, out to))//(Main.tile[PositionX, PositionY].active() && Main.tile[PositionX, PositionY].type == TileID.Tombstones)
                            {
                                TileObject.Place(to);
                                status.Message = "Someone's tombstone placed...";
                                //status.Message = "Tombstone placed at X:" + PositionX + "  Y:" + PositionY;
                                System.Threading.Thread.Sleep(1000);
                                TombstoneTileX = PositionX;
                                TombstoneTileY = PositionY;
                                int signpos = Sign.ReadSign(PositionX, PositionY);
                                if(signpos > -1)
                                {
                                    if(Main.sign[signpos] == null)
                                    {
                                        Main.sign[signpos] = new Sign();
                                    }
                                    Sign.TextSign(signpos, TombstoneText);
                                }
                                NPC.NewNPC(TombstoneTileX * 16, TombstoneTileY * 16, ModContent.NPCType<Npcs.AlexNPC>());
                                SpawnedTombstone = true;
                                Success = true;
                            }
                        }
                    }
                    Retry++;
                    if(!Success && Retry >= 1000)
                    {
                        status.Message = "Tombstone Spawn failed...";
                        System.Threading.Thread.Sleep(1000);
                        break;
                    }
                }
                return;
            }
        }

        public static void CheckIfAlexIsInTheWorld()
        {
            if (TombstoneTileX > 0 && !NpcMod.HasMetGuardian(5) && !NPC.AnyNPCs(ModContent.NPCType<Npcs.AlexNPC>()))
                NPC.NewNPC(TombstoneTileX * 16, TombstoneTileY * 16, ModContent.NPCType<Npcs.AlexNPC>());
        }
    }
}

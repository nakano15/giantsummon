using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SubworldLibrary;
using Terraria.ModLoader;
using Terraria;

namespace giantsummon.SubWorlds
{
    public class SubworldBlueprint
    {
        public static void SaveSubworldBlueprint()
        {
            Subworld sw = SLWorld.currentSubworld;
            if (sw is SubworldBase swb)
            {
                string SavePlace = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/" + swb.Name + ".bpm"; //bpm is for blueprint map
                using (FileStream fs = new FileStream(SavePlace, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        for(int y = 0; y < swb.height; y++)
                        {
                            for(int x = 0; x < swb.width; x++)
                            {
                                SaveTileData(writer, Terraria.Main.tile[x, y]);
                            }
                        }
                    }
                }
                Main.NewText("Subworld tile data saved on the Desktop.");
            }
            else
            {
                return;
            }
        }

        public static void LoadSubworldBlueprint(string SavePlace)
        {
            if (!ModContent.FileExists(SavePlace))
                return;
            Subworld sw = SLWorld.currentSubworld;
            if (sw is SubworldBase swb)
            {
                using (Stream fs = ModContent.OpenRead(SavePlace))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        for (int y = 0; y < swb.height; y++)
                        {
                            for (int x = 0; x < swb.width; x++)
                            {
                                Main.tile[x, y] = LoadTileData(reader);
                            }
                        }
                    }
                }
            }
        }

        public static void SaveTileData(BinaryWriter writer, Terraria.Tile tile)
        {
            if(tile == null)
            {
                writer.Write(false);
                return;
            }
            writer.Write(true);
            writer.Write(tile.type);
            writer.Write(tile.wall);
            writer.Write(tile.liquid);
            writer.Write(tile.sTileHeader);
            writer.Write(tile.bTileHeader);
            writer.Write(tile.bTileHeader2);
            writer.Write(tile.bTileHeader3);
            writer.Write(tile.frameX);
            writer.Write(tile.frameY);
        }

        public static Terraria.Tile LoadTileData(BinaryReader reader)
        {
            if (!reader.ReadBoolean())
                return new Terraria.Tile();
            return new Terraria.Tile
            {
                type = reader.ReadUInt16(),
                wall = reader.ReadUInt16(),
                liquid = reader.ReadByte(),
                sTileHeader = reader.ReadUInt16(),
                bTileHeader = reader.ReadByte(),
                bTileHeader2 = reader.ReadByte(),
                bTileHeader3 = reader.ReadByte(),
                frameX = reader.ReadInt16(),
                frameY = reader.ReadInt16()
            };
        }
    }
}

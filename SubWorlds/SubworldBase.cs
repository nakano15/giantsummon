using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

namespace giantsummon.SubWorlds
{
    public class SubworldBase : SubworldLibrary.Subworld
    {
        public override int width => 300;

        public override int height => 300;

        public virtual bool AllowBuildingAndDestruction { get { return true; } }

        public override List<GenPass> tasks
        {
            get
            {
                List<GenPass> GenStuff = new List<GenPass>();
                GenStuff.Add(DummyWorld());
                return GenStuff;
            }
        }

        public virtual void ModifySpawns(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {

        }

        public virtual void UpdatePlayerStatus(Player player)
        {

        }

        private GenPass DummyWorld()
        {
            PassLegacy world = new PassLegacy("Generating Dummy World",
                delegate (GenerationProgress gp)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = height / 2; y < height; y++)
                        {
                            Terraria.Main.tile[x, y].active();
                            Terraria.Main.tile[x, y].type = Terraria.ID.TileID.MarbleBlock;
                        }
                    }
                    Terraria.Main.spawnTileX = width / 2;
                    Terraria.Main.spawnTileY = height / 2 - 1;
                });
            return world;
        }

        public bool CreateHouse(int StartX, int StartY, int Width, int Height = 6, bool DoorLeft = true, bool DoorRight = true)
        {
            //if (StartX < Main.leftWorld || StartY - Height < Main.topWorld || StartX + Width > Main.rightWorld || StartY > Main.bottomWorld)
            //    return false;
            for(int y = -(Height - 1); y <= 0; y++)
            {
                for(int x = 0; x <= Width; x++)
                {
                    Main.tile[StartX + x, StartY + y].active(false);
                    Main.tile[StartX + x, StartY + y].wall = Terraria.ID.WallID.Wood;
                }
            }
            for (int x = -1; x < Width + 1; x++)
            {
                int PX = x + StartX, PY = StartY + 1;
                Main.tile[PX, PY].active(true);
                Main.tile[PX, PY].type = Terraria.ID.TileID.WoodBlock;
                PY = StartY - Height;
                Main.tile[PX, PY].active(true);
                Main.tile[PX, PY].type = Terraria.ID.TileID.WoodBlock;
            }
            for (int y = -Height; y <= 1; y++)
            {
                int PX = StartX - 1, PY = StartY + y;
                Main.tile[PX, PY].active(true);
                if (DoorLeft && y >= -2 && y < 1)
                {
                    Main.tile[PX, PY].type = Terraria.ID.TileID.ClosedDoor;
                }
                else
                {
                    Main.tile[PX, PY].type = Terraria.ID.TileID.WoodBlock;
                }
                PX = StartX + Width + 1;
                Main.tile[PX, PY].active(true);
                if (DoorRight && y >= -2 && y < 1)
                {
                    Main.tile[PX, PY].type = Terraria.ID.TileID.ClosedDoor;
                }
                else
                {
                    Main.tile[PX, PY].type = Terraria.ID.TileID.WoodBlock;
                }
            }
            for (int x = -2; x <= Width + 2; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    switch (y)
                    {
                        case 1:
                            if (x < 0 || x > Width)
                                continue;
                            break;
                        case 2:
                            if (x < 2 || x > Width - 2)
                                continue;
                            break;
                    }
                    int PX = StartX + x, PY = StartY - Height - y;
                    Main.tile[PX, PY].active(true);
                    Main.tile[PX, PY].type = Terraria.ID.TileID.WoodBlock;
                }
            }
            {
                int LanternPositionX = StartX + Width / 2, LanternPositionY = StartY - Height;
                WorldGen.Place1x2(LanternPositionX, LanternPositionY, Terraria.ID.TileID.HangingLanterns, 0);
                /*int TorchPosX = StartX, TorchPosY = StartY - Height + 1;
                Main.tile[TorchPosX, TorchPosY].active(true);
                Main.tile[TorchPosX, TorchPosY].type = Terraria.ID.TileID.Torches;
                TorchPosX += Width;
                Main.tile[TorchPosX, TorchPosY].active(true);
                Main.tile[TorchPosX, TorchPosY].type = Terraria.ID.TileID.Torches;*/
            }
            return true;
        }
    }
}

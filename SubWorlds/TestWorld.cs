using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;

namespace giantsummon.SubWorlds
{
    public class TestWorld : SubworldBase
    {
        public override int width
        {
            get { return 800; }
        }

        public override int height
        {
            get { return 600; }
        }

        public override bool saveModData
        {
            get
            {
                return false;
            }
        }

        public override bool saveSubworld
        {
            get
            {
                return false;
            }
        }

        public override bool noWorldUpdate
        {
            get
            {
                return true;
            }
        }

        public override bool AllowBuildingAndDestruction => false;

        public override List<GenPass> tasks
        {
            get {
                List<GenPass> GenStuff = new List<GenPass>();
                GenStuff.Add(new PassLegacy("Test Gen", delegate (GenerationProgress progress)
                {
                    progress.Message = "Trying to generate Tungsten World";
                    int HeightVariability = 0;
                    byte HeightChangeDelay = 0;
                    const byte HeightChangeStack = 20;
                    int DefaultHeight = height / 5;
                    Random rand = new Random(12345);
                    for (int x = 0; x < width; x++)
                    {
                        if(HeightChangeDelay == 0)
                        {
                            HeightVariability = (int)((rand.NextDouble() - 0.5f) * 3);
                            HeightChangeDelay += HeightChangeStack;
                        }
                        HeightChangeDelay--;
                        for (int y = (DefaultHeight + HeightVariability); y < height; y++)
                        {
                            Main.tile[x, y].active(true);
                            Main.tile[x, y].type = Terraria.ID.TileID.TungstenBrick;
                        }
                        if (HeightChangeDelay == HeightChangeStack / 2)
                        {
                            int TorchY = (DefaultHeight + HeightVariability) - 1;
                            WorldGen.Place1xX(x, TorchY, 93);
                        }
                    }
                    Main.spawnTileX = width / 2;
                    Main.spawnTileY = height / 5 - 3;
                    //
                    progress.Message = "Trying to spawn a house.";
                    int HouseSpawnX = 50, HouseSpawnY = 0;
                    while (!Main.tile[HouseSpawnX, HouseSpawnY + 1].active())
                        HouseSpawnY++;
                    CreateHouse(HouseSpawnX, HouseSpawnY, 16);
                }));
                return GenStuff;
            }
        }

        public override void ModifySpawns(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            pool.Clear();
            pool.Add(Terraria.ID.NPCID.Bunny, 0.6f);
            pool.Add(Terraria.ID.NPCID.Butcher, 0.01f);
            pool.Add(Terraria.ID.NPCID.Zombie, 0.8f);
        }
    }
}

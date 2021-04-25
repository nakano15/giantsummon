using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

namespace giantsummon.Maps
{
    public class TestMap : Compatibility.SubworldLibraryCompatibility.SubworldInfo
    {
        public TestMap() : base("Tungsten World", MainMod.mod, 800, 600)
        {
            SaveModData = false;
            SaveSubworld = false;
            NoWorldUpdate = true;
        }

        public override List<GenPass> Tasks()
        {
            List<GenPass> passes = new List<GenPass>
            {
                TungstenGround
            };
            return passes;
        }

        private GenPass TungstenGround
        {
            get
            {
                return new PassLegacy("GenTungstenWorld", delegate(GenerationProgress progress)
                {
                    progress.Message = "Trying to generate Tungsten World";
                    int HeightVariability = 0;
                    byte HeightChangeDelay = 0;
                    const byte HeightChangeStack = 20;
                    int DefaultHeight = Height / 5;
                    Random rand = new Random(12345);
                    for (int x = 0; x < Width; x++)
                    {
                        if (HeightChangeDelay == 0)
                        {
                            HeightVariability = (int)((rand.NextDouble() - 0.5f) * 3);
                            HeightChangeDelay += HeightChangeStack;
                        }
                        HeightChangeDelay--;
                        for (int y = (DefaultHeight + HeightVariability); y < Height; y++)
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
                    Main.spawnTileX = Width / 2;
                    Main.spawnTileY = Height / 5 - 3;
                });
            }
        }
    }
}

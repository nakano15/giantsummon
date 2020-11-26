using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

namespace giantsummon.SubWorlds
{
    public class TestWorld : SubworldLibrary.Subworld
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
        
        public override List<Terraria.World.Generation.GenPass> tasks
        {
            get {
                List<Terraria.World.Generation.GenPass> GenStuff = new List<Terraria.World.Generation.GenPass>();
                GenStuff.Add(new PassLegacy("Test Gen", delegate(GenerationProgress progress)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = height / 5; y < height; y++)
                        {
                            Main.tile[x, y].active(true);
                            Main.tile[x, y].type = Terraria.ID.TileID.TungstenBrick;
                        }
                    }
                }));
                Main.spawnTileX = width / 2;
                Main.spawnTileY = height / 5 - 1;
                return GenStuff;
            }
        }
    }
}

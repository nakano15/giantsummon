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
    }
}

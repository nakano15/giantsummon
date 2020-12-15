using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

namespace giantsummon.SubWorlds
{
    public class TerraGuardianVillage : SubworldBase
    {
        public override int width => 1200;
        public override int height => 600;
        public override bool saveSubworld => false;
        public override bool saveModData => false;
        public override bool AllowBuildingAndDestruction => false;
        public override bool noWorldUpdate => true;
        public static float VillageTime = 8;
        public const float TimePassDuration = 1.6f;

        public static void UpdateVillageTime()
        {
            VillageTime += TimePassDuration;
            if (VillageTime >= 24)
                VillageTime -= 24;
        }

        public override void Load()
        {
            if (VillageTime < 4.5f)
            {
                Terraria.Main.dayTime = false;
                Terraria.Main.time = (4.5f + VillageTime) * 3600;
            }
            else if (VillageTime >= 19.5f)
            {
                Terraria.Main.dayTime = false;
                Terraria.Main.time = (VillageTime - 19.5f) * 3600;
            }
            else
            {
                Terraria.Main.dayTime = true;
                Terraria.Main.time = (VillageTime - 4.5f) * 3600;
            }
        }

        public override List<GenPass> tasks => base.tasks;

        private GenPass CreateGround()
        {
            return new PassLegacy("ground gen", delegate(GenerationProgress progress)
            {
                progress.Message = "Creating Ground";
                int StartHeight = (int)(height * 0.6f);
                for(int x = 0; x < width; x++)
                {
                    progress.Value = (float)x / width;
                    for(int y = 0; y < height; y++)
                    {
                        Main.tile[x, y].active(true);
                        Main.tile[x, y].type = Terraria.ID.TileID.Dirt;
                    }
                }
            }, 0.1f);
        }

        private GenPass TerraformingWorld()
        {
            return new PassLegacy("terraforming", delegate (GenerationProgress progress)
            {
                progress.Message = "TerraformingGround";
                int StartHeight = (int)(height * 0.6f);
            }, 0.1f);
        }
    }
}

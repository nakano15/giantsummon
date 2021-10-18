using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class TileMod : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail)
            {
                WorldMod.UpdateTileStateOnGuardianHouses(i, j, false);
                if (Main.tileSign[type] && GuardianBountyQuest.SignID > -1 && Sign.ReadSign(i, j, false) == GuardianBountyQuest.SignID)
                {
                    GuardianBountyQuest.SignID = -1;
                }
                if(Main.halloween && (type == Terraria.ID.TileID.ShadowOrbs || type == Terraria.ID.TileID.DemonAltar) && Main.rand.Next(50) == 0)
                {
                    MainMod.TriggerAnyPlayerCompanionJS();
                }
            }
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            WorldMod.UpdateTileStateOnGuardianHouses(i, j, true);
        }

        public override void RightClick(int i, int j, int type)
        {
            if (type == 10)
            {
                Tile tile = Main.tile[i, j];
                if (tile.frameY >= 594 && tile.frameY <= 646 && Main.player[Main.myPlayer].HasItem(1141))
                {
                    GuardianGlobalInfos.AddFeat(FeatMentioning.FeatType.OpenedTemple,
                        Main.player[Main.myPlayer].name, Main.worldName, 8, 10,
                        GuardianGlobalInfos.GetGuardiansInTheWorld());
                }
            }
        }
    }
}

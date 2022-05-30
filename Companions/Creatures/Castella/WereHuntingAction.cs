using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Companions.Creatures.Castella
{
    public class WereHuntingAction : GuardianActions
    {
        public override void Update(TerraGuardian guardian)
        {
            bool IsWere = CastellaBase.OnWerewolfForm(guardian);
            switch (Step)
            {
                case 0:

                    break;
            }
        }

        public override bool? ModifyPlayerHostile(TerraGuardian guardian, Player player)
        {
            return !guardian.IsPlayerBuddy(player);
        }
    }
}

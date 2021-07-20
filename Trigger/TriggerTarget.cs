using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Trigger
{
    public class TriggerTarget
    {
        public TargetTypes TargetType;
        public int TargetID;

        public TriggerTarget()
        {
            TargetType = TargetTypes.Game;
            TargetID = 0;
        }

        public TriggerTarget(Player player)
        {
            TargetType = TargetTypes.Player;
            TargetID = player.whoAmI;
        }

        public TriggerTarget(NPC npc)
        {
            TargetType = TargetTypes.NPC;
            TargetID = npc.whoAmI;
        }

        public TriggerTarget(TerraGuardian tg)
        {
            TargetType = TargetTypes.TerraGuardian;
            TargetID = tg.WhoAmID;
        }

        public bool IsThisCompanion(TerraGuardian tg)
        {
            return TargetType == TargetTypes.TerraGuardian && tg.WhoAmID == TargetID;
        }

        public enum TargetTypes : byte
        {
            Game,
            Player,
            NPC,
            TerraGuardian
        }
    }
}

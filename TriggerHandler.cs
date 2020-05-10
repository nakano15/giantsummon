using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon
{
    public class TriggerHandler
    {
        public static void FirePlayerHurtTrigger(Vector2 Position, Player player, int Damage, bool Critical, bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.PlayerHurt;
            int Value = player.whoAmI;
            int Value2 = Damage;
            FireTrigger(Position, trigger, Value, Value2, Critical ? 1 : 0, PvP ? 1 : 0);
        }

        public static void FirePlayerDownedTrigger(Vector2 Position, Player player, int Damage, bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.PlayerDowned;
            int Value = player.whoAmI;
            int Value2 = Damage;
            FireTrigger(Position, trigger, Value, Value2, PvP ? 1 : 0);
        }

        public static void FirePlayerDeathTrigger(Vector2 Position, Player player, int Damage,  bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.PlayerDies;
            int Value = player.whoAmI;
            int Value2 = Damage;
            FireTrigger(Position, trigger, Value, Value2, PvP ? 1 : 0);
        }

        public static void FireGuardianHurtTrigger(Vector2 Position, TerraGuardian guardian, int Damage, bool Critical, bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.GuardianHurt;
            int Value = guardian.WhoAmID;
            int Value2 = Damage;
            FireTrigger(Position, trigger, Value, Value2, Critical ? 1 : 0, PvP ? 1 : 0);
        }

        public static void FireGuardianDownedTrigger(Vector2 Position, TerraGuardian guardian, int Damage, bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.GuardianDowned;
            int Value = guardian.WhoAmID;
            int Value2 = Damage;
            FireTrigger(Position, trigger, Value, Value2, PvP ? 1 : 0);
        }

        public static void FireGuardianDeathTrigger(Vector2 Position, TerraGuardian guardian, bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.GuardianDies;
            int Value = guardian.WhoAmID;
            FireTrigger(Position, trigger, Value, PvP ? 1 : 0);
        }

        public static void FireNpcHurtTrigger(Vector2 Position, NPC npc, int Damage, bool Critical)
        {
            TriggerTypes trigger = TriggerTypes.NpcHurt;
            int Value = npc.whoAmI;
            int Value2 = Damage;
            FireTrigger(Position, trigger, Value, Value2, Critical ? 1 : 0);
        }

        public static void FireNpcDeathTrigger(Vector2 Position, NPC npc)
        {
            TriggerTypes trigger = TriggerTypes.NpcDies;
            int Value = npc.whoAmI;
            FireTrigger(Position, trigger, Value);
        }

        public static void FireHourChange(int Hour)
        {
            TriggerTypes trigger = TriggerTypes.HourChange;
            FireTrigger(Vector2.Zero, trigger, Hour);
        }

        public static void FireDayNightChange(bool Day)
        {
            TriggerTypes trigger = TriggerTypes.DayNightChange;
            FireTrigger(Vector2.Zero, trigger, Day ? 1 : 0);
        }

        public static void FireTrigger(Vector2 Position, TriggerTypes trigger, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            foreach (int key in MainMod.ActiveGuardians.Keys)
            {
                if (Position == Vector2.Zero || MainMod.ActiveGuardians[key].InPerceptionRange(Position))
                {
                    MainMod.ActiveGuardians[key].DoTrigger(trigger, Value, Value2, Value3, Value4, Value5);
                }
            }
        }
    }

    public enum TriggerTypes : byte
    {
        PlayerSpotted,
        GuardianSpotted,
        NpcSpotted,
        PlayerHurt,
        GuardianHurt,
        NpcHurt,
        PlayerDowned,
        GuardianDowned,
        PlayerDies,
        GuardianDies,
        NpcDies,
        HourChange,
        DayNightChange,
        WeatherChange
    }
}

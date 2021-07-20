using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using giantsummon.Trigger;

namespace giantsummon
{
    public class TriggerHandler
    {
        public static void FirePlayerHurtTrigger(Vector2 Position, Player player, int Damage, bool Critical, bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.Hurt;
            int Value = Damage;
            FireTrigger(Position, trigger, new TriggerTarget(player), Value, Critical ? 1 : 0, PvP ? 1 : 0);
        }

        public static void FirePlayerDownedTrigger(Vector2 Position, Player player, int Damage, bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.Downed;
            int Value = Damage;
            FireTrigger(Position, trigger, new TriggerTarget(player), Value, PvP ? 1 : 0);
        }

        public static void FirePlayerDeathTrigger(Vector2 Position, Player player, int Damage,  bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.Death;
            int Value = Damage;
            FireTrigger(Position, trigger, new TriggerTarget(player), Value, PvP ? 1 : 0);
        }

        public static void FireGuardianHurtTrigger(Vector2 Position, TerraGuardian guardian, int Damage, bool Critical, bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.Hurt;
            int Value = Damage;
            FireTrigger(Position, trigger, new TriggerTarget(guardian), Value, Critical ? 1 : 0, PvP ? 1 : 0);
        }

        public static void FireGuardianDownedTrigger(Vector2 Position, TerraGuardian guardian, int Damage, bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.Downed;
            int Value = Damage;
            FireTrigger(Position, trigger, new TriggerTarget(guardian), Value, PvP ? 1 : 0);
        }

        public static void FireGuardianDeathTrigger(Vector2 Position, TerraGuardian guardian, bool PvP)
        {
            TriggerTypes trigger = TriggerTypes.Death;
            FireTrigger(Position, trigger, new TriggerTarget(guardian), PvP ? 1 : 0);
        }

        public static void FireNpcHurtTrigger(Vector2 Position, NPC npc, int Damage, bool Critical)
        {
            TriggerTypes trigger = TriggerTypes.Hurt;
            int Value = Damage;
            FireTrigger(Position, trigger, new TriggerTarget(npc), Value, Critical ? 1 : 0);
        }

        public static void FireNpcDeathTrigger(Vector2 Position, NPC npc)
        {
            TriggerTypes trigger = TriggerTypes.Death;
            FireTrigger(Position, trigger, new TriggerTarget(npc));
        }

        public static void FirePlayerBuffAcquiredTrigger(Vector2 Position, Player player, int BuffID)
        {
            TriggerTypes trigger = TriggerTypes.AcquiredBuff;
            FireTrigger(Position, trigger, new TriggerTarget(player), BuffID);
        }

        public static void FireNPCBuffAcquiredTrigger(Vector2 Position, NPC npc, int BuffID)
        {
            TriggerTypes trigger = TriggerTypes.AcquiredBuff;
            FireTrigger(Position, trigger, new TriggerTarget(npc), BuffID);
        }

        public static void FireGuardianBuffAcquiredTrigger(Vector2 Position, TerraGuardian tg, int BuffID)
        {
            TriggerTypes trigger = TriggerTypes.AcquiredBuff;
            FireTrigger(Position, trigger, new TriggerTarget(tg), BuffID);
        }

        public static void FirePlayerBuffRemovedTrigger(Vector2 Position, Player player, int BuffID)
        {
            TriggerTypes trigger = TriggerTypes.LostBuff;
            FireTrigger(Position, trigger, new TriggerTarget(player), BuffID);
        }

        public static void FireNPCBuffRemovedTrigger(Vector2 Position, NPC npc, int BuffID)
        {
            TriggerTypes trigger = TriggerTypes.LostBuff;
            FireTrigger(Position, trigger, new TriggerTarget(npc), BuffID);
        }

        public static void FireGuardianBuffRemovedTrigger(Vector2 Position, TerraGuardian tg, int BuffID)
        {
            TriggerTypes trigger = TriggerTypes.LostBuff;
            FireTrigger(Position, trigger, new TriggerTarget(tg), BuffID);
        }

        public static void FireHourChange(int Hour)
        {
            TriggerTypes trigger = TriggerTypes.HourChange;
            FireTrigger(Vector2.Zero, trigger, new TriggerTarget(), Hour);
        }

        public static void FireDayNightChange(bool Day)
        {
            TriggerTypes trigger = TriggerTypes.DayNightChange;
            FireTrigger(Vector2.Zero, trigger, new TriggerTarget(), Day ? 1 : 0);
        }

        public static void FireTrigger(Vector2 Position, TriggerTypes trigger, TriggerTarget Target, int Value = 0, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            List<TerraGuardian> tgs = new List<TerraGuardian>();
            foreach (int key in MainMod.ActiveGuardians.Keys)
            {
                if (Position == Vector2.Zero || MainMod.ActiveGuardians[key].InPerceptionRange(Position) || 
                    ((trigger == TriggerTypes.Death || trigger == TriggerTypes.Spotted) && 
                    (Target.TargetType != TriggerTarget.TargetTypes.TerraGuardian || Target.TargetID != key)))
                {
                    MainMod.ActiveGuardians[key].DoTrigger(trigger, Target, Value, Value2, Value3, Value4, Value5);
                    tgs.Add(MainMod.ActiveGuardians[key]);
                }
            }
            TerraGuardian.DoTriggerGroup(tgs, trigger, Target, Value, Value2, Value3, Value4, Value5);
        }
    }

    public enum TriggerTypes : byte
    {
        Spotted,
        Hurt,
        Downed,
        Death,
        HourChange,
        DayNightChange,
        WeatherChange,
        AcquiredBuff,
        LostBuff
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Companions.Liebre
{
    public class SoulUnloadingAction : GuardianActions
    {
        private int SoulsValue = 0;
        private bool LastWasPlayerMounted = false;

        public override void Update(TerraGuardian guardian)
        {
            LiebreBase.ReaperGuardianData data = (LiebreBase.ReaperGuardianData)guardian.Data;
            switch (Step)
            {
                case 0:
                    if (StepStart)
                    {
                        SoulsValue = data.SoulsLoaded;
                        if (SoulsValue >= LiebreBase.MaxSoulsContainedValue)
                        {
                            guardian.SaySomething("*I must go, now!*");
                        }
                        else if (SoulsValue >= LiebreBase.MaxSoulsContainedValue * 0.9f)
                        {
                            guardian.SaySomething("*I'll unload those souls, I feel like I'm reaching my capacity.*");
                        }
                        else
                        {
                            guardian.SaySomething("*I'll be right back.*");
                        }
                    }
                    IgnoreCombat = true;
                    if(Main.rand.NextDouble() < 0.333)
                    {
                        Dust.NewDust(guardian.TopLeftPosition, guardian.Width, guardian.Height, 192, 0, -0.2f, Scale:Main.rand.NextFloat(0.8f, 1.2f));
                    }
                    if(Time >= 3 * 60)
                    {
                        ChangeStep();
                        data.SoulsLoaded = 0;
                        Invisibility = true;
                        LastWasPlayerMounted = guardian.PlayerMounted;
                        guardian.ToggleMount(true);
                        guardian.ClearMessagesSaid();
                    }
                    break;
                case 1:
                    Invisibility = true;
                    IgnoreCombat = true;
                    Inactivity = true;
                    if (guardian.OwnerPos > -1)
                        guardian.Position = Main.player[guardian.OwnerPos].Bottom;
                    if (Time >= 10 * 60)
                        ChangeStep();
                    break;
                case 2:
                    Invisibility = false;
                    IgnoreCombat = false;
                    Inactivity = false;
                    if (StepStart)
                    {
                        guardian.Spawn();
                        if (guardian.OwnerPos > -1)
                        {
                            guardian.Position = Main.player[guardian.OwnerPos].Bottom;
                            if (LastWasPlayerMounted)
                                guardian.ToggleMount(true, false);
                        }
                    }
                    if (Time >= 30)
                    {
                        if (SoulsValue >= LiebreBase.MaxSoulsContainedValue)
                        {
                            guardian.SaySomething("*I felt like about to blow...*");
                        }
                        else if (SoulsValue >= LiebreBase.MaxSoulsContainedValue * 0.9f)
                        {
                            guardian.SaySomething("*I'm glad I managed to do the delivery in time.*");
                        }
                        else
                        {
                            guardian.SaySomething("*I returned.*");
                        }
                    }
                    if (Time % 10 == 0)
                    {
                        float HeightVal = Time / 10 * 0.1f * guardian.Height;
                        for (int i = -1; i < 2; i ++)
                        {
                            Vector2 EffectPosition = new Vector2(guardian.Position.X, guardian.Position.Y);
                            EffectPosition.Y -= HeightVal;
                            EffectPosition.X += guardian.Width * 0.6f * i;
                            Dust.NewDust(EffectPosition, 1, 1, 192, 0, -0.2f, 192, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                        }
                    }
                    if (Time >= 100)
                    {
                        {
                            float CapacityPercentage = (float)SoulsValue / LiebreBase.MaxSoulsContainedValue;
                            int BuffDuration = (int)(CapacityPercentage * 900) * 60;
                            if (SoulsValue > LiebreBase.MaxSoulsContainedValue)
                                BuffDuration = (int)(BuffDuration * 0.75f);
                            for (int i = 0; i < CapacityPercentage * 5; i++)
                            {
                                int BuffID = Utils.SelectRandom(Main.rand, new int[] { Terraria.ID.BuffID.Lifeforce, Terraria.ID.BuffID.Regeneration,
                                    Terraria.ID.BuffID.Endurance, Terraria.ID.BuffID.ManaRegeneration, Terraria.ID.BuffID.Mining, Terraria.ID.BuffID.ObsidianSkin,
                                    Terraria.ID.BuffID.Thorns});
                                for (int p = 0; p < 255; p++)
                                {
                                    if (Main.player[p].active && !guardian.IsPlayerHostile(Main.player[p]))
                                    {
                                        Main.player[p].AddBuff(BuffID, BuffDuration);
                                    }
                                }
                                foreach (TerraGuardian g in MainMod.ActiveGuardians.Values)
                                {
                                    if (g.OwnerPos == guardian.OwnerPos && !g.IsGuardianHostile(guardian))
                                    {
                                        g.AddBuff(BuffID, BuffDuration);
                                    }
                                }
                            }
                            guardian.SaySomethingCanSchedule("*Take this blessing as a reward for helping me.*");
                        }
                        guardian.DoAction.InUse = false;
                    }
                    break;
            }
        }
    }
}

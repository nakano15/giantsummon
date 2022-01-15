using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon
{
    public class RequestReward
    {
        private static List<RequestReward> RequestRewards = new List<RequestReward>();

        public int itemID = 0, Stack = 1;
        public float AcquisitionChance = 1;
        public Func<Player, GuardianData, bool> CanGetReward = delegate (Player player, GuardianData gd)
        {
            return true;
        };

        public static void InitializeRequestRewards()
        {
            RequestRewards.Clear();
            AddRequestReward(ModContent.ItemType<Items.Consumable.EtherHeart>(), AcquisitionChance: 0.333f).CanGetReward = EtherItemRequirement;
            AddRequestReward(ItemID.LifeCrystal, AcquisitionChance: 0.2f);
            AddRequestReward(ModContent.ItemType<Items.Consumable.EtherHeart>(), AcquisitionChance: 0.333f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return !MainMod.NoEtherItems && Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && gd.Base.IsTerraGuardian;
            };
            AddRequestReward(ItemID.LifeFruit, AcquisitionChance: 0.2f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
            };
            AddRequestReward(ModContent.ItemType<Items.Consumable.SkillResetPotion>(), AcquisitionChance: 0.667f);
            AddRequestReward(ItemID.WoodenCrate, AcquisitionChance: 0.625f);
            AddRequestReward(ItemID.IronCrate, AcquisitionChance: 0.390625f);
            AddRequestReward(ItemID.GoldenCrate, AcquisitionChance: 0.09765625f);
            AddRequestReward(ItemID.CorruptFishingCrate, AcquisitionChance: 0.05f);
            AddRequestReward(ItemID.HallowedFishingCrate, AcquisitionChance: 0.05f).CanGetReward = delegate(Player player, GuardianData gd)
            {
                return Main.hardMode;
            };
            AddRequestReward(ItemID.JungleFishingCrate, AcquisitionChance: 0.05f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return NPC.downedQueenBee;
            };
            AddRequestReward(ItemID.DungeonFishingCrate, AcquisitionChance: 0.05f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return NPC.downedBoss3;
            };
            AddRequestReward(ItemID.FloatingIslandFishingCrate, AcquisitionChance: 0.05f);
            AddRequestReward(ItemID.CookedFish, 3, AcquisitionChance: 0.125f);
            AddRequestReward(ItemID.BowlofSoup, 3, AcquisitionChance: 0.125f);
            if (Main.hardMode)
            {
                AddRequestReward(364, Main.rand.Next(65, 86), 0.45f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return WorldGen.oreTier1 == 107;
                };
                AddRequestReward(365, Main.rand.Next(50, 66), 0.35f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return WorldGen.oreTier1 == 108;
                };
                AddRequestReward(366, Main.rand.Next(40, 56), 0.25f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return WorldGen.oreTier1 == 111;
                });
                AddRequestReward(1104, Main.rand.Next(65, 86), 0.45f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return WorldGen.oreTier1 != 107;
                };
                AddRequestReward(1105, Main.rand.Next(50, 66), 0.35f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return WorldGen.oreTier1 != 108;
                };
                AddRequestReward(1106, Main.rand.Next(40, 56), 0.25f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return WorldGen.oreTier1 != 111;
                });
                AddRequestReward(ItemID.ChlorophyteOre, Main.rand.Next(35, 86), 0.35f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
                };
                AddRequestReward(ItemID.LunarOre, Main.rand.Next(45, 96), 0.7f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return NPC.downedMoonlord;
                };
            }
            else
            {
                AddRequestReward(ItemID.DemoniteOre, Main.rand.Next(20, 41), 0.6f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return !WorldGen.crimson && (NPC.downedBoss1 || NPC.downedBoss2);
                };
                AddRequestReward(ItemID.CrimtaneOre, Main.rand.Next(20, 41), 0.6f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return WorldGen.crimson && (NPC.downedBoss1 || NPC.downedBoss2);
                };
                AddRequestReward(ItemID.Hellstone, Main.rand.Next(30, 51), 0.55f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return NPC.downedBoss2;
                };
                AddRequestReward(ItemID.Obsidian, Main.rand.Next(30, 51), 0.55f).CanGetReward = delegate (Player player, GuardianData gd)
                {
                    return NPC.downedBoss2;
                };
            }
            AddRequestReward(ItemID.Bass, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
            AddRequestReward(ItemID.ArmoredCavefish, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
            AddRequestReward(ItemID.AtlanticCod, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
            AddRequestReward(ItemID.CrimsonTigerfish, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
            AddRequestReward(ItemID.Ebonkoi, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
            AddRequestReward(ItemID.Tuna, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
        }

        private static bool CompanionHasFishingGearRequirement(Player player, GuardianData gd)
        {
            for(int i = 0; i < 50; i++)
            {
                if (gd.Inventory[i].fishingPole > 0)
                    return true;
            }
            return false;
        }

        private static bool EtherItemRequirement(Player player, GuardianData gd)
        {
            return !MainMod.NoEtherItems && gd.Base.IsTerraGuardian;
        }

        public static List<RequestReward> GetPossibleRewards(Player player, GuardianData gd)
        {
            float Chance;
            return GetPossibleRewards(player, gd, out Chance);
        }

        public static List<RequestReward> GetPossibleRewards(Player player, GuardianData gd, out float MaxChance)
        {
            List<RequestReward> rewards = new List<RequestReward>();
            MaxChance = 0;
            foreach(RequestReward r in RequestRewards)
            {
                if (r.CanGetReward(player, gd))
                {
                    MaxChance += r.AcquisitionChance;
                    rewards.Add(r);
                }
            }
            foreach(Reward rw in gd.Base.RewardsList)
            {
                RequestReward newreward = new RequestReward() { itemID = rw.ItemID, Stack = rw.InitialStack, AcquisitionChance = rw.RewardChance };
                rewards.Add(newreward);
            }
            return rewards;
        }

        public static RequestReward AddRequestReward(int ItemID, int Stack = 1, float AcquisitionChance = 1f)
        {
            RequestReward reward = new RequestReward() { itemID = ItemID, Stack = Stack, AcquisitionChance = AcquisitionChance };
            RequestRewards.Add(reward);
            return reward;
        }
    }
}

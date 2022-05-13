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
        public Func<Player, GuardianData, bool> CanGetReward = DefaultTrue;

        public static bool DefaultTrue(Player player, GuardianData gd)
        {
            return true;
        }

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
            AddRequestReward(ModContent.ItemType<Items.Misc.Note>(), AcquisitionChance: 0.01f).CanGetReward = delegate(Player player, GuardianData gd){
				return !NpcMod.HasMetGuardian(GuardianBase.Daphne);
			};
            AddRequestReward(ItemID.WoodenCrate, AcquisitionChance: 0.625f);
            AddRequestReward(ItemID.IronCrate, AcquisitionChance: 0.390625f);
            AddRequestReward(ItemID.GoldenCrate, AcquisitionChance: 0.09765625f);
            AddRequestReward(ItemID.CorruptFishingCrate, AcquisitionChance: 0.05f);
            AddRequestReward(ItemID.HallowedFishingCrate, AcquisitionChance: 0.05f).CanGetReward = delegate (Player player, GuardianData gd)
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
            AddRequestReward(364, Main.rand.Next(65, 86), 0.45f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return Main.hardMode && WorldGen.oreTier1 == 107;
            };
            AddRequestReward(365, Main.rand.Next(50, 66), 0.35f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return Main.hardMode && WorldGen.oreTier1 == 108;
            };
            AddRequestReward(366, Main.rand.Next(40, 56), 0.25f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return Main.hardMode && WorldGen.oreTier1 == 111;
            };
            AddRequestReward(1104, Main.rand.Next(65, 86), 0.45f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return Main.hardMode && WorldGen.oreTier1 != 107;
            };
            AddRequestReward(1105, Main.rand.Next(50, 66), 0.35f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return Main.hardMode && WorldGen.oreTier1 != 108;
            };
            AddRequestReward(1106, Main.rand.Next(40, 56), 0.25f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return Main.hardMode && WorldGen.oreTier1 != 111;
            };
            AddRequestReward(ItemID.ChlorophyteOre, Main.rand.Next(35, 86), 0.35f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
            };
            AddRequestReward(ItemID.LunarOre, Main.rand.Next(45, 96), 0.7f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return Main.hardMode && NPC.downedMoonlord;
            };
            AddRequestReward(ItemID.DemoniteOre, Main.rand.Next(20, 41), 0.6f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return !Main.hardMode && !WorldGen.crimson && (NPC.downedBoss1 || NPC.downedBoss2);
            };
            AddRequestReward(ItemID.CrimtaneOre, Main.rand.Next(20, 41), 0.6f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return !Main.hardMode && WorldGen.crimson && (NPC.downedBoss1 || NPC.downedBoss2);
            };
            AddRequestReward(ItemID.Hellstone, Main.rand.Next(30, 51), 0.55f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return !Main.hardMode && NPC.downedBoss2;
            };
            AddRequestReward(ItemID.Obsidian, Main.rand.Next(30, 51), 0.55f).CanGetReward = delegate (Player player, GuardianData gd)
            {
                return !Main.hardMode && NPC.downedBoss2;
            };
            AddRequestReward(ItemID.Bass, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
            AddRequestReward(ItemID.ArmoredCavefish, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
            AddRequestReward(ItemID.AtlanticCod, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
            AddRequestReward(ItemID.CrimsonTigerfish, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
            AddRequestReward(ItemID.Ebonkoi, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
            AddRequestReward(ItemID.Tuna, 3, 0.66f).CanGetReward = CompanionHasFishingGearRequirement;
            //Boss spawners
            AddRequestReward(ItemID.SuspiciousLookingEye, 1, 0.1f);
            AddRequestReward(ItemID.WormFood, 1, 0.1f).CanGetReward = RequestContainer.CorruptWorldAndBossKilledRequirement;
            AddRequestReward(ItemID.BloodySpine, 1, 0.1f).CanGetReward = RequestContainer.CorruptWorldAndBossKilledRequirement;
            AddRequestReward(ItemID.SlimeCrown, 1, 0.1f).CanGetReward = RequestContainer.KingSlimeKillRequirement;
            AddRequestReward(ItemID.Abeemination, 1, 0.1f).CanGetReward = RequestContainer.QueenBeeKillRequirement;
            //
            AddRequestReward(ItemID.MechanicalSkull, 1, 0.1f).CanGetReward = RequestContainer.HardmodeRequirement;
            AddRequestReward(ItemID.MechanicalWorm, 1, 0.1f).CanGetReward = RequestContainer.HardmodeRequirement;
            AddRequestReward(ItemID.MechanicalEye, 1, 0.1f).CanGetReward = RequestContainer.HardmodeRequirement;
            //
            AddRequestReward(ItemID.LihzahrdPowerCell, 1, 0.1f).CanGetReward = RequestContainer.GolemKillRequirement;
            AddRequestReward(ItemID.CelestialSigil, 1, 0.1f).CanGetReward = RequestContainer.LunaticCultistKillRequirement;
            //Good Loot
            AddRequestReward(ItemID.SlimeStaff, 1, 0.01f);
            AddRequestReward(ItemID.Arkhalis, 1, 0.1f);
            AddRequestReward(ItemID.EnchantedSword, 1, 0.01f);
            AddRequestReward(ItemID.StylistKilLaKillScissorsIWish, 1, 0.01f).CanGetReward = FemaleQuestGiverRequirement;
            AddRequestReward(ItemID.CandyCaneSword, 1, 0.01f).CanGetReward = RequestContainer.XmasRequirement;
            AddRequestReward(ItemID.RedRyder, 1, 0.01f).CanGetReward = RequestContainer.XmasRequirement;
            AddRequestReward(ItemID.BladedGlove, 1, 0.01f).CanGetReward = RequestContainer.HalloweenRequirement;
            AddRequestReward(ItemID.Muramasa, 1, 0.01f).CanGetReward = RequestContainer.SkeletronKillRequirement;
            AddRequestReward(ItemID.FieryGreatsword, 1, 0.01f).CanGetReward = RequestContainer.EvilBossKillRequirement;
            AddRequestReward(ItemID.NightsEdge, 1, 0.01f).CanGetReward = RequestContainer.SkeletronKillRequirement;
            AddRequestReward(ItemID.BladeofGrass, 1, 0.01f).CanGetReward = RequestContainer.AnyFirstBossKillRequirement;
            AddRequestReward(ItemID.BeamSword, 1, 0.01f).CanGetReward = RequestContainer.HardmodeRequirement;
            AddRequestReward(ItemID.Bladetongue, 1, 0.01f).CanGetReward = RequestContainer.HardmodeRequirement;
            AddRequestReward(ItemID.Toxikarp, 1, 0.01f).CanGetReward = RequestContainer.HardmodeRequirement;
            AddRequestReward(ItemID.CrystalSerpent, 1, 0.01f).CanGetReward = RequestContainer.HardmodeRequirement;
            AddRequestReward(ItemID.Sunfury, 1, 0.01f).CanGetReward = RequestContainer.SkeletronKillRequirement;
            AddRequestReward(ItemID.Sunflower, 1, 0.01f).CanGetReward = RequestContainer.SkeletronKillRequirement;
            AddRequestReward(ItemID.HellwingBow, 1, 0.01f).CanGetReward = RequestContainer.SkeletronKillRequirement;
            AddRequestReward(ItemID.Boomstick, 1, 0.01f).CanGetReward = RequestContainer.AnyFirstBossKillRequirement;
            AddRequestReward(ItemID.AleThrowingGlove, 1, 0.01f);
            AddRequestReward(ItemID.PartyGirlGrenade, 1, 0.01f);
            AddRequestReward(ItemID.PainterPaintballGun, 1, 0.01f);
            //Potions
            AddRequestReward(ItemID.LesserHealingPotion, 5, 0.25f);
            AddRequestReward(ItemID.HealingPotion, 5, 0.5f);
            AddRequestReward(ItemID.GreaterHealingPotion, 5, 0.5f).CanGetReward = RequestContainer.HardmodeRequirement;
            AddRequestReward(ItemID.SuperHealingPotion, 5, 0.5f).CanGetReward = RequestContainer.LunaticCultistKillRequirement;
            AddRequestReward(ItemID.LesserManaPotion, 5, 0.25f);
            AddRequestReward(ItemID.ManaPotion, 5, 0.5f);
            AddRequestReward(ItemID.GreaterManaPotion, 5, 0.5f).CanGetReward = RequestContainer.HardmodeRequirement;
            AddRequestReward(ItemID.SuperManaPotion, 5, 0.5f).CanGetReward = RequestContainer.LunaticCultistKillRequirement;
            //Buff Potions
            AddRequestReward(ItemID.ArcheryPotion, 3, 0.1f);
            AddRequestReward(ItemID.BattlePotion, 3, 0.1f);
            AddRequestReward(ItemID.CalmingPotion, 3, 0.1f);
            AddRequestReward(ItemID.CratePotion, 3, 0.1f);
            AddRequestReward(ItemID.TrapsightPotion, 3, 0.1f);
            AddRequestReward(ItemID.EndurancePotion, 3, 0.1f);
            AddRequestReward(ItemID.GillsPotion, 3, 0.1f);
            AddRequestReward(ItemID.GravitationPotion, 3, 0.05f);
            AddRequestReward(ItemID.HunterPotion, 3, 0.1f);
            AddRequestReward(ItemID.InfernoPotion, 3, 0.05f);
            AddRequestReward(ItemID.IronskinPotion, 3, 0.1f);
            AddRequestReward(ItemID.LifeforcePotion, 3, 0.05f);
            AddRequestReward(ItemID.NightOwlPotion, 3, 0.1f);
            AddRequestReward(ItemID.ObsidianSkinPotion, 3, 0.1f);
            AddRequestReward(ItemID.RagePotion, 3, 0.1f);
            AddRequestReward(ItemID.RegenerationPotion, 3, 0.1f);
            AddRequestReward(ItemID.ShinePotion, 3, 0.1f);
            AddRequestReward(ItemID.SpelunkerPotion, 3, 0.1f);
            AddRequestReward(ItemID.SwiftnessPotion, 3, 0.1f);
            AddRequestReward(ItemID.TitanPotion, 3, 0.1f);
            AddRequestReward(ItemID.WaterWalkingPotion, 3, 0.1f);
            AddRequestReward(ItemID.WrathPotion, 3, 0.1f);
        }

        private static bool FemaleQuestGiverRequirement(Player player, GuardianData gd)
        {
            return !gd.Male;
        }

        private static bool MaleQuestGiverRequirement(Player player, GuardianData gd)
        {
            return gd.Male;
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

        public static RequestReward GetAPossibleReward(Player player, GuardianData gd)
        {
            float TotalChance;
            List<RequestReward> Rewards = GetPossibleRewards(player, gd, out TotalChance);
            float Stack = 0;
            float Picked = Main.rand.NextFloat() * TotalChance;
            foreach(RequestReward reward in Rewards)
            {
                if(Picked >= Stack && Picked < Stack + reward.AcquisitionChance)
                {
                    return reward;
                }
                Stack += reward.AcquisitionChance;
            }
            return null;
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
            foreach(RequestReward r in gd.Base.RewardsList)
            {
                if (r.CanGetReward(player, gd))
                {
                    MaxChance += r.AcquisitionChance;
                    rewards.Add(r);
                }
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

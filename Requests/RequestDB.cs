using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria.ID;

namespace giantsummon.Requests
{
    public class RequestDB : RequestContainer
    {
        public RequestDB() : base(MainMod.mod)
        {
            //Travel Request
            AddTravelRequest(0, 0);

            //Hunt Requests
            AddHuntRequest(1000, NPCID.BlueSlime, "Slime");
            AddHuntRequest(1001, NPCID.MotherSlime, InitialCount: 1, RewardValue: 350);
            AddHuntRequest(1002, NPCID.Skeleton);
            //Corruption Mobs
            AddHuntRequest(1003, NPCID.EaterofSouls, InitialCount: 7).CanGetRequest = AnyFirstBossKillRequirementCorruption;
            AddHuntRequest(1004, NPCID.DevourerHead, InitialCount: 3, ExtraFriendshipLevelCount: 0.2f).CanGetRequest = AnyFirstBossKillRequirementCorruption;
            //Crimson Mobs
            AddHuntRequest(1005, NPCID.Crimera, InitialCount: 7).CanGetRequest = AnyFirstBossKillRequirementCrimson;
            AddHuntRequest(1006, NPCID.FaceMonster, InitialCount: 3, ExtraFriendshipLevelCount: 0.2f).CanGetRequest = AnyFirstBossKillRequirementCrimson;
            AddHuntRequest(1007, NPCID.BloodCrawler).CanGetRequest = AnyFirstBossKillRequirementCrimson;
            //Spider
            AddHuntRequest(1008, NPCID.WallCreeper).CanGetRequest = LifeCrystalUsedRequirement;
            //Night
            AddHuntRequest(1009, NPCID.Zombie);
            AddHuntRequest(1010, NPCID.DemonEye);
            AddHuntRequest(1011, NPCID.Raven, InitialCount: 3).CanGetRequest = HalloweenRequirement;
            //Rare
            AddHuntRequest(1012, NPCID.GoblinScout, InitialCount: 1, ExtraFriendshipLevelCount: 0.25f, RewardValue: 500).CanGetRequest = EvilBossKillRequirement;
            //Snow Biome
            AddHuntRequest(1013, NPCID.IceSlime);
            AddHuntRequest(1014, NPCID.IceBat).CanGetRequest = LifeCrystalUsedRequirement;
            AddHuntRequest(1015, NPCID.SnowFlinx, InitialCount: 3, RewardValue: 150).CanGetRequest = LifeCrystalUsedRequirement;
            AddHuntRequest(1016, NPCID.UndeadViking, InitialCount: 3, RewardValue: 150).CanGetRequest = LifeCrystalUsedRequirement;
            //
            AddHuntRequest(1017, NPCID.Nymph, InitialCount: 1, ExtraFriendshipLevelCount: 0, RewardValue: 1000).CanGetRequest = AnyFirstBossKillRequirement;
            //Desert
            AddHuntRequest(1018, NPCID.SandSlime);
            AddHuntRequest(1019, NPCID.Vulture);
            AddHuntRequest(1020, NPCID.Antlion, InitialCount: 3);
            AddHuntRequest(1021, NPCID.WalkingAntlion).CanGetRequest = EoCKillRequirement;
            AddHuntRequest(1022, NPCID.FlyingAntlion).CanGetRequest = EoCKillRequirement;
            //Jungle
            AddHuntRequest(1023, NPCID.JungleSlime, InitialCount: 7).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1024, NPCID.Snatcher).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1025, NPCID.JungleBat, InitialCount: 7).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1026, NPCID.Hornet, InitialCount: 9).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1027, NPCID.ManEater).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1028, NPCID.Piranha);
            //Dungeon
            AddHuntRequest(1029, NPCID.AngryBones, InitialCount: 12).CanGetRequest = SkeletronKillRequirement;
            AddHuntRequest(1030, NPCID.DarkCaster, InitialCount: 7).CanGetRequest = SkeletronKillRequirement;
            AddHuntRequest(1031, NPCID.CursedSkull, InitialCount: 7).CanGetRequest = SkeletronKillRequirement;
            AddHuntRequest(1032, NPCID.DungeonSlime, InitialCount: 3, ExtraFriendshipLevelCount: 0.25f).CanGetRequest = SkeletronKillRequirement;
            //Underworld
            AddHuntRequest(1033, NPCID.Demon, InitialCount: 7).CanGetRequest = EvilBossKillRequirement;
            AddHuntRequest(1034, NPCID.FireImp).CanGetRequest = EvilBossKillRequirement;
            AddHuntRequest(1035, NPCID.LavaSlime).CanGetRequest = EvilBossKillRequirement;
            AddHuntRequest(1036, NPCID.Hellbat).CanGetRequest = EvilBossKillRequirement;
            //Goblin Army Requests
            AddHuntRequest(1037, NPCID.GoblinPeon, InitialCount: 9).CanGetRequest = DefeatedGoblinsRequirement;
            AddHuntRequest(1038, NPCID.GoblinSorcerer).CanGetRequest = DefeatedGoblinsRequirement;
            AddHuntRequest(1039, NPCID.GoblinWarrior, InitialCount: 7).CanGetRequest = DefeatedGoblinsRequirement;
            AddHuntRequest(1040, NPCID.GoblinThief, InitialCount: 7).CanGetRequest = DefeatedGoblinsRequirement;
            AddHuntRequest(1041, NPCID.GoblinArcher).CanGetRequest = DefeatedGoblinsRequirement;
            //Hardmode
            AddHuntRequest(1042, NPCID.PossessedArmor).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1043, NPCID.WanderingEye, InitialCount: 3).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1044, NPCID.Wraith, InitialCount: 3).CanGetRequest = HardmodeRequirement;
            //Underground
            AddHuntRequest(1045, NPCID.ArmoredSkeleton).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1046, NPCID.DiggerHead, InitialCount: 3, ExtraFriendshipLevelCount: 0.2f).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1047, NPCID.GiantBat).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1048, NPCID.AnglerFish).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1049, NPCID.SkeletonArcher, InitialCount: 3).CanGetRequest = HardmodeRequirement;
            //Corruption
            AddHuntRequest(1050, NPCID.Corruptor).CanGetRequest = CorruptWorldHardmodeRequirement;
            AddHuntRequest(1051, NPCID.SeekerHead).CanGetRequest = CorruptWorldHardmodeRequirement;
            AddHuntRequest(1052, NPCID.Clinger).CanGetRequest = CorruptWorldHardmodeRequirement;
            AddHuntRequest(1053, NPCID.CursedHammer, InitialCount: 3, ExtraFriendshipLevelCount: 0.2f).CanGetRequest = CorruptWorldHardmodeRequirement;
            AddHuntRequest(1054, NPCID.BigMimicCorruption, InitialCount: 1, ExtraFriendshipLevelCount: 0, RewardValue: 50000).CanGetRequest = CorruptWorldHardmodeRequirement;
            //Crimson
            AddHuntRequest(1055, NPCID.Herpling).CanGetRequest = CrimsonWorldHardmodeRequirement;
            AddHuntRequest(1056, NPCID.Crimslime).CanGetRequest = CrimsonWorldHardmodeRequirement;
            AddHuntRequest(1057, NPCID.IchorSticker).CanGetRequest = CrimsonWorldHardmodeRequirement;
            AddHuntRequest(1058, NPCID.FloatyGross).CanGetRequest = CrimsonWorldHardmodeRequirement;
            AddHuntRequest(1059, NPCID.CrimsonAxe, InitialCount: 3, ExtraFriendshipLevelCount: 0.2f).CanGetRequest = CrimsonWorldHardmodeRequirement;
            AddHuntRequest(1060, NPCID.BigMimicCrimson, InitialCount: 1, ExtraFriendshipLevelCount: 0, RewardValue: 50000).CanGetRequest = CrimsonWorldHardmodeRequirement;
            //Desert
            AddHuntRequest(1061, NPCID.DesertBeast).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1062, NPCID.DesertScorpionWalk).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1063, NPCID.DesertGhoul).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1064, NPCID.DesertLamiaDark).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1065, NPCID.DuneSplicerHead, InitialCount: 3, ExtraFriendshipLevelCount: 0.2f).CanGetRequest = HardmodeRequirement;
            //Snowland
            AddHuntRequest(1066, NPCID.IceElemental).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1067, NPCID.Wolf).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1068, NPCID.ArmoredViking, InitialCount: 3, ExtraFriendshipLevelCount: 0.2f).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1069, NPCID.IceTortoise).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1070, NPCID.IcyMerman).CanGetRequest = HardmodeRequirement;
            //Hallow
            AddHuntRequest(1071, NPCID.Pixie).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1072, NPCID.Unicorn).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1073, NPCID.Gastropod).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1074, NPCID.IlluminantBat).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1075, NPCID.IlluminantSlime).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1076, NPCID.ChaosElemental, InitialCount: 3, ExtraFriendshipLevelCount: 0.2f).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1077, NPCID.EnchantedSword, InitialCount: 1, ExtraFriendshipLevelCount: 0).CanGetRequest = HardmodeRequirement;
            AddHuntRequest(1078, NPCID.BigMimicHallow, InitialCount: 1, ExtraFriendshipLevelCount: 0, RewardValue: 50000).CanGetRequest = HardmodeRequirement;
            //Extra Desert Spawn
            AddHuntRequest(1079, NPCID.Mummy).CanGetRequest = HardmodeRequirement;
            //Jungle
            AddHuntRequest(1080, NPCID.Herpling).CanGetRequest = AnyMechBossKillRequirement;
            AddHuntRequest(1081, NPCID.GiantTortoise, InitialCount: 3).CanGetRequest = AllMechBossKillRequirement;
            AddHuntRequest(1082, NPCID.GiantFlyingFox).CanGetRequest = AnyMechBossKillRequirement;
            AddHuntRequest(1083, NPCID.Arapaima).CanGetRequest = AnyMechBossKillRequirement;
            AddHuntRequest(1084, NPCID.AngryTrapper).CanGetRequest = AllMechBossKillRequirement;
            AddHuntRequest(1085, NPCID.MossHornet).CanGetRequest = AllMechBossKillRequirement;
            AddHuntRequest(1086, NPCID.JungleCreeper).CanGetRequest = AllMechBossKillRequirement;
            //Lihzahrd Dungeon
            AddHuntRequest(1087, NPCID.Lihzahrd).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1088, NPCID.FlyingSnake).CanGetRequest = PlanteraKillRequirement;
            //Dungeon
            AddHuntRequest(1089, NPCID.BlueArmoredBones, InitialCount: 9).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1090, NPCID.RustyArmoredBonesAxe, InitialCount: 9).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1091, NPCID.HellArmoredBones, InitialCount: 9).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1092, NPCID.Necromancer).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1093, NPCID.RaggedCaster).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1094, NPCID.DiabolistRed).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1095, NPCID.SkeletonCommando, InitialCount: 3).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1096, NPCID.SkeletonSniper, InitialCount: 1).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1097, NPCID.TacticalSkeleton, InitialCount: 3).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1098, NPCID.GiantCursedSkull).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1099, NPCID.BoneLee, InitialCount: 1).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1100, NPCID.DungeonSpirit, InitialCount: 15).CanGetRequest = PlanteraKillRequirement;
            AddHuntRequest(1101, NPCID.Paladin, InitialCount: 1, ExtraFriendshipLevelCount: 0, RewardValue: 10000).CanGetRequest = PlanteraKillRequirement;
            //Underworld
            AddHuntRequest(1102, NPCID.Lavabat, InitialCount: 15).CanGetRequest = AnyMechBossKillRequirement;
            AddHuntRequest(1103, NPCID.RedDevil, InitialCount: 15).CanGetRequest = AnyMechBossKillRequirement;
            //Sky
            AddHuntRequest(1104, NPCID.Harpy).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1105, NPCID.WyvernHead, InitialCount: 1, ExtraFriendshipLevelCount: 0, RewardValue: 8000).CanGetRequest = AnyMechBossKillRequirement;
            //Pirate Army
            AddHuntRequest(1106, NPCID.PirateCorsair).CanGetRequest = DefeatedPiratesRequirement;
            AddHuntRequest(1107, NPCID.PirateCrossbower).CanGetRequest = DefeatedPiratesRequirement;
            AddHuntRequest(1108, NPCID.PirateDeadeye).CanGetRequest = DefeatedPiratesRequirement;
            AddHuntRequest(1109, NPCID.PirateDeckhand).CanGetRequest = DefeatedPiratesRequirement;
            AddHuntRequest(1110, NPCID.Parrot).CanGetRequest = DefeatedPiratesRequirement;
            AddHuntRequest(1111, NPCID.PirateCaptain, InitialCount: 1, ExtraFriendshipLevelCount: 0, RewardValue: 6000).CanGetRequest = DefeatedPiratesRequirement;
            //Martian Madness
            AddHuntRequest(1112, NPCID.GrayGrunt).CanGetRequest = DefeatedMartiansRequirement;
            AddHuntRequest(1113, NPCID.RayGunner).CanGetRequest = DefeatedMartiansRequirement;
            AddHuntRequest(1114, NPCID.MartianOfficer).CanGetRequest = DefeatedMartiansRequirement;
            AddHuntRequest(1115, NPCID.MartianEngineer).CanGetRequest = DefeatedMartiansRequirement;
            AddHuntRequest(1116, NPCID.GigaZapper).CanGetRequest = DefeatedMartiansRequirement;
            AddHuntRequest(1117, NPCID.MartianWalker, InitialCount: 3).CanGetRequest = DefeatedMartiansRequirement;
            AddHuntRequest(1118, NPCID.Scutlix).CanGetRequest = DefeatedMartiansRequirement;

            //Item Requests
            AddItemRequest(2000, ItemID.Gel, ExtraFriendshipLevelCount: 1);
            AddItemRequest(2001, ItemID.Lens, InitialCount: 2);
            //Dungeon
            AddItemRequest(2002, ItemID.Bone, InitialCount: 50).CanGetRequest = SkeletronKillRequirement;
            AddItemRequest(2003, ItemID.GoldenKey, InitialCount: 5, RewardValue: 200).CanGetRequest = SkeletronKillRequirement;
            //Desert
            AddItemRequest(2004, ItemID.AntlionMandible, InitialCount: 7).CanGetRequest = EoCKillRequirement;
            //Corruption
            AddItemRequest(2005, ItemID.RottenChunk, InitialCount: 9).CanGetRequest = AnyFirstBossKillRequirementCorruption;
            //Crimson
            AddItemRequest(2006, ItemID.Vertebrae, InitialCount: 9).CanGetRequest = AnyFirstBossKillRequirementCrimson;
            //Jungle
            AddItemRequest(2007, ItemID.Stinger, InitialCount: 12).CanGetRequest = AnyFirstBossKillRequirement;
            AddItemRequest(2008, ItemID.Vine, InitialCount: 5).CanGetRequest = AnyFirstBossKillRequirement;
            //Sky
            AddItemRequest(2009, ItemID.Feather, InitialCount: 15).CanGetRequest = AnyFirstBossKillRequirement;
            //Hardmode
            AddItemRequest(2010, ItemID.CursedFlame, InitialCount: 15).CanGetRequest = CorruptWorldHardmodeRequirement;
            AddItemRequest(2011, ItemID.Ichor, InitialCount: 15).CanGetRequest = CrimsonWorldHardmodeRequirement;
            AddItemRequest(2012, ItemID.CrystalShard, InitialCount: 15).CanGetRequest = HardmodeRequirement;
            //Souls
            AddItemRequest(2013, ItemID.SoulofLight, InitialCount: 15).CanGetRequest = HardmodeRequirement;
            AddItemRequest(2014, ItemID.SoulofNight, InitialCount: 15).CanGetRequest = HardmodeRequirement;
            AddItemRequest(2015, ItemID.SoulofFlight, InitialCount: 15).CanGetRequest = AllMechBossKillRequirement;
            //Dungeon
            AddItemRequest(2016, ItemID.Ectoplasm, InitialCount: 15).CanGetRequest = PlanteraKillRequirement;
            //Desert
            AddItemRequest(2017, ItemID.FossilOre, InitialCount: 15).CanGetRequest = HardmodeRequirement;
            AddItemRequest(2018, ItemID.DarkShard, InitialCount: 3).CanGetRequest = HardmodeRequirement;
            AddItemRequest(2019, ItemID.LightShard, InitialCount: 15).CanGetRequest = HardmodeRequirement;
            //Temple
            AddItemRequest(2020, ItemID.LihzahrdPowerCell, InitialCount: 5).CanGetRequest = PlanteraKillRequirement;
        }
    }
}

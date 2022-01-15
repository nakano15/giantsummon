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
            AddHuntRequest(1003, NPCID.EaterofSouls, InitialCount: 7).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1004, NPCID.DevourerHead, InitialCount: 3, ExtraFriendshipLevelCount: 0.2f).CanGetRequest = AnyFirstBossKillRequirement;
            //Crimson Mobs
            AddHuntRequest(1005, NPCID.Crimera, InitialCount: 7).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1006, NPCID.FaceMonster, InitialCount: 3, ExtraFriendshipLevelCount: 0.2f).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1007, NPCID.BloodCrawler).CanGetRequest = AnyFirstBossKillRequirement;
            //Spider
            AddHuntRequest(1008, NPCID.WallCreeper);
            //Night
            AddHuntRequest(1009, NPCID.Zombie);
            AddHuntRequest(1010, NPCID.DemonEye);
            AddHuntRequest(1011, NPCID.Raven, InitialCount: 3).CanGetRequest = HalloweenRequirement;
            //Rare
            AddHuntRequest(1012, NPCID.GoblinScout, InitialCount: 1, ExtraFriendshipLevelCount: 0.25f, RewardValue: 500).CanGetRequest = EvilBossKillRequirement;
            //Snow Biome
            AddHuntRequest(1013, NPCID.IceSlime);
            AddHuntRequest(1014, NPCID.IceBat);
            AddHuntRequest(1015, NPCID.SnowFlinx, InitialCount: 3, RewardValue: 150);
            AddHuntRequest(1016, NPCID.UndeadViking, InitialCount: 3, RewardValue: 150);
            //
            AddHuntRequest(1017, NPCID.Nymph, InitialCount: 1, ExtraFriendshipLevelCount: 0, RewardValue: 1000);
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
            AddHuntRequest(1033, NPCID.Demon, InitialCount: 7).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1034, NPCID.FireImp).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1035, NPCID.LavaSlime).CanGetRequest = AnyFirstBossKillRequirement;
            AddHuntRequest(1036, NPCID.Hellbat).CanGetRequest = AnyFirstBossKillRequirement;
            //Goblin Army Requests
            AddHuntRequest(1037, NPCID.GoblinPeon, InitialCount: 9).CanGetRequest = DefeatedGoblinsRequirement;
            AddHuntRequest(1038, NPCID.GoblinSorcerer).CanGetRequest = DefeatedGoblinsRequirement;
            AddHuntRequest(1039, NPCID.GoblinWarrior, InitialCount: 7).CanGetRequest = DefeatedGoblinsRequirement;
            AddHuntRequest(1040, NPCID.GoblinThief, InitialCount: 7).CanGetRequest = DefeatedGoblinsRequirement;
            AddHuntRequest(1041, NPCID.GoblinArcher).CanGetRequest = DefeatedGoblinsRequirement;

            //Item Requests
            AddItemRequest(2000, ItemID.Gel, ExtraFriendshipLevelCount: 1);
            AddItemRequest(2001, ItemID.Lens, InitialCount: 2);
            //Dungeon
            AddItemRequest(2002, ItemID.Bone, InitialCount: 50);
            AddItemRequest(2003, ItemID.GoldenKey, InitialCount: 5, RewardValue: 200);
            //Desert
            AddItemRequest(2004, ItemID.AntlionMandible, InitialCount: 7);
            //Corruption
            AddItemRequest(2005, ItemID.RottenChunk, InitialCount: 9).CanGetRequest = AnyFirstBossKillRequirement;
            //Crimson
            AddItemRequest(2006, ItemID.Vertebrae, InitialCount: 9).CanGetRequest = AnyFirstBossKillRequirement;
            //Jungle
            AddItemRequest(2007, ItemID.Stinger, InitialCount: 12).CanGetRequest = AnyFirstBossKillRequirement;
            AddItemRequest(2008, ItemID.Vine, InitialCount: 5).CanGetRequest = AnyFirstBossKillRequirement;
        }
    }
}

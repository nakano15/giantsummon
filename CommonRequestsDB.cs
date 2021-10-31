using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon
{
    public class CommonRequestsDB
    {
        public static void PopulateCommonRequestsDB() //Never change the order of the quests, always add by the end, or else you may end up breaking the quests for saves.
        {
            List<RequestBase> Requests = new List<RequestBase>();
            //PHM Surface
            RequestBase rb = new RequestBase("", 0, "", "", "", "", "(Slimes appears in the many places, but mostly in the forest.)");
            rb.AddHuntObjective(Terraria.ID.NPCID.BlueSlime, 3);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 90, "", "", "", "", "(Zombies roams the forest at night.)");
            rb.AddRequestRequirement(RequestBase.GetNightRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.Zombie, 5, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 110, "", "", "", "", "(Demon Eyes flies through the sky during the night.)");
            rb.AddRequestRequirement(RequestBase.GetNightRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.DemonEye, 4, 0.33f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 70, "", "", "", "", "(Gels comes from Slimes, they mostly appear in forest.)");
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.Gel, 3, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 50, "", "", "", "", "(Bunnies appears mostly in safe places at the Forest.)");
            rb.AddRequestRequirement(RequestBase.GetBugNetRequirement);
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.Bunny, 3, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 65, "", "", "", "", "(Can be caught in forest lakes.)");
            rb.AddRequestRequirement(RequestBase.GetFishingRodRequirement);
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.Bass, 3, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 60, "", "", "", "", "(Explore the world with the request giver.)");
            rb.AddExploreRequest(1000);
            Requests.Add(rb);
            // PHM Corruption
            rb = new RequestBase("", 130, "", "", "", "", "(Eater of Souls are found in the Corruption.)");
            rb.AddRequestRequirement(RequestBase.GetCorruptionRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.EaterofSouls, 7, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 120, "", "", "", "", "(Devourers are found in the Corruption.)");
            rb.AddRequestRequirement(RequestBase.GetCorruptionRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.DevourerHead, 1, 0.333f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 120, "", "", "", "", "(Rotten Chunk are acquired from Eater of Souls, found in the Corruption.)");
            rb.AddRequestRequirement(RequestBase.GetCorruptionRequestRequirement);
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.RottenChunk, 5);
            Requests.Add(rb);
            // PHM Crimson
            rb = new RequestBase("", 130, "", "", "", "", "(Crimeras are found in the Crimson.)");
            rb.AddRequestRequirement(RequestBase.GetCrimsonRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.Crimera, 7, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 130, "", "", "", "", "(Face Monsters are found in the Crimson.)");
            rb.AddRequestRequirement(RequestBase.GetCrimsonRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.FaceMonster, 7, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 130, "", "", "", "", "(Blood Crawlers are found in the Crimson.)");
            rb.AddRequestRequirement(RequestBase.GetCrimsonRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.BloodCrawler, 7, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 120, "", "", "", "", "(Vertebraes comes from several Crimson monsters.)");
            rb.AddRequestRequirement(RequestBase.GetCrimsonRequestRequirement);
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.Vertebrae, 5);
            Requests.Add(rb);
            //PHM Dungeon
            rb = new RequestBase("", 150, "", "", "", "", "(Angry Bones are found in the Dungeon.)");
            rb.AddRequestRequirement(RequestBase.GetDungeonRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.AngryBones, 10, 0.333f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 160, "", "", "", "", "(Dark Casters are found in the Dungeon.)");
            rb.AddRequestRequirement(RequestBase.GetDungeonRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.DarkCaster, 5, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 170, "", "", "", "", "(Cursed Skulls are found in the Dungeon.)");
            rb.AddRequestRequirement(RequestBase.GetDungeonRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.CursedSkull, 3, 0.1f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 180, "", "", "", "", "(Bones can be acquired from many Dungeon creatures.)");
            rb.AddRequestRequirement(RequestBase.GetDungeonRequestRequirement);
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.Bone, 20, 3);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 190, "", "", "", "", "(Golden Key can be acquired from some Dungeon creatures.)");
            rb.AddRequestRequirement(RequestBase.GetDungeonRequestRequirement);
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.GoldenKey, 3, 0.333f);
            Requests.Add(rb);
            // PHM Underworld
            rb = new RequestBase("", 210, "", "", "", "", "(Demons can be found in the deepest part of the world, the Underworld)");
            rb.AddRequestRequirement(RequestBase.GetUnderworldRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.Demon, 5);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 215, "", "", "", "", "(Fire Imps can be found in the deepest part of the world, the Underworld)");
            rb.AddRequestRequirement(RequestBase.GetUnderworldRequestRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.FireImp, 3, 0.2f);
            Requests.Add(rb);
            // HM Forest
            rb = new RequestBase("", 240, "", "", "", "", "(Wraiths appears in many places, but most notably in surface, at night.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.Wraith, 3, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 230, "", "", "", "", "(Possessed Armors roams the forest during the night.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.PossessedArmor, 7, 0.333f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 245, "", "", "", "", "(Wandering Eyes attacks during the night.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.WanderingEye, 5, 0.35f);
            Requests.Add(rb);
            // HM Corruption
            rb = new RequestBase("", 250, "", "", "", "", "(Corruptors can be found frequently in the Corruption.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeCorruptionRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.Corruptor, 10);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 265, "", "", "", "", "(World Feeders sometimes appears in the Corruption.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeCorruptionRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.SeekerHead, 5, 0.2f); //Seeker is how the World Feeder is known as in the game source.
            Requests.Add(rb);
            //
            rb = new RequestBase("", 275, "", "", "", "", "(Cursed Flames can be acquired from a number of Underground Corruption creatures.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeCorruptionRequirement);
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.CursedFlame, 12, 0.2f);
            Requests.Add(rb);
            // HM Crimson
            rb = new RequestBase("", 250, "", "", "", "", "(Herplings appears frequently in the Crimson.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeCrimsonRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.Herpling, 10);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 265, "", "", "", "", "(Crimslimes appears in the Crimson.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeCrimsonRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.Crimslime, 5, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 275, "", "", "", "", "(Ichor can drop from some monsters in the Underground Crimson.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeCrimsonRequirement);
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.Ichor, 12, 0.2f);
            Requests.Add(rb);
            // HM Jungle
            rb = new RequestBase("", 290, "", "", "", "", "(Moss Hornets appears in the Underground Jungle.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeJungleRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.MossHornet, 12);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 310, "", "", "", "", "(Giant Tortoises appears in the Underground Jungle.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeJungleRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.GiantTortoise, 7, 0.2f);
            Requests.Add(rb);
            // HM Temple
            rb = new RequestBase("", 325, "", "", "", "", "(Lihzahrds appears in the Jungle Temple.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeDungeonAndTempleRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.Lihzahrd, 15, 0.333f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 335, "", "", "", "", "(Flying Snakes appears in the Jungle Temple.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeDungeonAndTempleRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.FlyingSnake, 10, 0.2f);
            Requests.Add(rb);
            // HM Dungeon
            rb = new RequestBase("", 325, "", "", "", "", "(Dungeon Spirits can surge from creatures killed in the Dungeon.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeDungeonAndTempleRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.DungeonSpirit, 8, 0.333f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 310, "", "", "", "", "(Cursed Skulls appears in the Dungeon.)");
            rb.AddRequestRequirement(RequestBase.GetHardmodeDungeonAndTempleRequirement);
            rb.AddHuntObjective(Terraria.ID.NPCID.CursedSkull, 6, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 210, "", "", "", "", "(Requester can call the Eye of Cthulhu for you to fight against.)");
            rb.AddRequestRequirement(delegate(Player player)
            {
                return player.statDefense >= 10 && player.statLifeMax >= 200 && NPC.downedBoss1;
            });
            rb.AddKillBossRequest(Terraria.ID.NPCID.EyeofCthulhu);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 210, "", "", "", "", "(Requester can call the Eater of Worlds for you to fight against.)");
            rb.AddRequestRequirement(delegate(Player player)
            {
                return !WorldGen.crimson && player.statDefense >= 10 && player.statLifeMax >= 200 && NPC.downedBoss2;
            });
            rb.AddKillBossRequest(Terraria.ID.NPCID.EaterofWorldsHead);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 210, "", "", "", "", "(Requester can call the Brain of Cthulhu for you to fight against.)");
            rb.AddRequestRequirement(delegate(Player player)
            {
                return WorldGen.crimson && player.statDefense >= 10 && player.statLifeMax >= 200 && NPC.downedBoss2;
            });
            rb.AddKillBossRequest(Terraria.ID.NPCID.BrainofCthulhu);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 210, "", "", "", "", "(Requester can call the Skeletron for you to fight against.)");
            rb.AddRequestRequirement(delegate(Player player)
            {
                return player.statDefense >= 10 && player.statLifeMax >= 200 && NPC.downedBoss3;
            });
            rb.AddKillBossRequest(Terraria.ID.NPCID.SkeletronHead);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 210, "", "", "", "", "(Requester can call the King Slime for you to fight against.)");
            rb.AddRequestRequirement(delegate(Player player)
            {
                return player.statDefense >= 10 && player.statLifeMax >= 200 && NPC.downedSlimeKing;
            });
            rb.AddKillBossRequest(Terraria.ID.NPCID.KingSlime);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 210, "", "", "", "", "(Requester can call the Queen Bee for you to fight against.)");
            rb.AddRequestRequirement(delegate(Player player)
            {
                return player.statDefense >= 10 && player.statLifeMax >= 200 && NPC.downedQueenBee;
            });
            rb.AddKillBossRequest(Terraria.ID.NPCID.QueenBee);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 620, "", "", "", "", "(Requester can call the Twins for you to fight against.)");
            rb.AddRequestRequirement(delegate(Player player)
            {
                return Main.hardMode && NPC.downedMechBossAny;
            });
            rb.AddKillBossRequest(Terraria.ID.NPCID.Spazmatism);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 620, "", "", "", "", "(Requester can call The Destroyer for you to fight against.)");
            rb.AddRequestRequirement(delegate(Player player)
            {
                return Main.hardMode && NPC.downedMechBossAny;
            });
            rb.AddKillBossRequest(Terraria.ID.NPCID.TheDestroyer);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 620, "", "", "", "", "(Requester can call the Skeletron Prime for you to fight against.)");
            rb.AddRequestRequirement(delegate(Player player)
            {
                return Main.hardMode && NPC.downedMechBossAny;
            });
            rb.AddKillBossRequest(Terraria.ID.NPCID.SkeletronPrime);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 840, "", "", "", "", "(You can try spawning Plantera by breaking It's bulbs.)");
            rb.AddRequestRequirement(delegate(Player player)
            {
                return Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && NPC.downedPlantBoss;
            });
            rb.AddKillBossRequest(Terraria.ID.NPCID.Plantera);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 920, "", "", "", "", "(You can try spawning The Golem by using a Lihzahrd Powercell on a Lihzahrd Altar.)");
            rb.AddRequestRequirement(delegate(Player player)
            {
                return Main.hardMode && NPC.downedGolemBoss;
            });
            rb.AddKillBossRequest(Terraria.ID.NPCID.Golem);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 86, RequestInfoText: "(You can craft using wood on a workbench.)");
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.WoodenChair, 1);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 94, RequestInfoText: "(You can craft using wood on a workbench.)");
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.WoodenTable, 1, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 80, RequestInfoText: "(You can craft using wood on a workbench.)");
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.WoodenSword, 1, 0.1f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 92, RequestInfoText: "(You can craft using ebonwood on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return !WorldGen.crimson && NPC.downedBoss2;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.EbonwoodChair, 1);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 100, RequestInfoText: "(You can craft using ebonwood on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return !WorldGen.crimson && NPC.downedBoss2;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.EbonwoodTable, 1, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 90, RequestInfoText: "(You can craft using ebonwood on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return !WorldGen.crimson && NPC.downedBoss2;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.EbonwoodSword, 1);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 92, RequestInfoText: "(You can craft using shadewood on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return WorldGen.crimson && NPC.downedBoss2;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.ShadewoodChair, 1);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 100, RequestInfoText: "(You can craft using shadewood on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return WorldGen.crimson && NPC.downedBoss2;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.ShadewoodTable, 1, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 90, RequestInfoText: "(You can craft using shadewood on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return WorldGen.crimson && NPC.downedBoss2;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.ShadewoodSword, 1);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 102, RequestInfoText: "(You can craft using rich mahogany on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return NPC.downedQueenBee;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.RichMahoganyChair, 1);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 110, RequestInfoText: "(You can craft using rich mahogany on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return NPC.downedQueenBee;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.RichMahoganyTable, 1, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 100, RequestInfoText: "(You can craft using rich mahogany on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return NPC.downedQueenBee;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.RichMahoganySword, 1, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 122, RequestInfoText: "(You can craft using pearlwood on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return NPC.downedQueenBee;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.PearlwoodChair, 1);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 130, RequestInfoText: "(You can craft using pearlwood on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return NPC.downedQueenBee;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.PearlwoodTable, 1, 0.2f);
            Requests.Add(rb);
            //
            rb = new RequestBase("", 120, RequestInfoText: "(You can craft using pearlwood on a workbench.)");
            rb.AddRequestRequirement(delegate (Player player)
            {
                return NPC.downedQueenBee;
            });
            rb.AddItemCollectionRequest(Terraria.ID.ItemID.PearlwoodSword, 1, 0.2f);
            Requests.Add(rb);
            //
            RequestBase.CommonRequests = Requests.ToArray();
        }
    }
}

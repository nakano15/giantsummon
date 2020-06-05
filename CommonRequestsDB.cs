using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon
{
    public class CommonRequestsDB
    {
        public static void PopulateCommonRequestsDB()
        {
            List<RequestBase> Requests = new List<RequestBase>();
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
            //
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
            //
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
            //
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
            //
            RequestBase.CommonRequests = Requests.ToArray();
        }
    }
}

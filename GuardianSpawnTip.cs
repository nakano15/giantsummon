using System.Collections.Generic;
using Terraria;

namespace giantsummon
{
    class GuardianSpawnTip
    {
        public static bool HasMetGuardian(int Id, string ModID = "")
        {
            return NpcMod.HasMetGuardian(Id, ModID);
        }
        public static bool HasGuardianNPC(int Id, string ModID = "")
        {
            return NpcMod.HasGuardianNPC(Id, ModID);
        }

        public static string GetRandomTip()
        {
            Player player = Main.LocalPlayer;
            List<string> Messages = new List<string>();
            if (!HasMetGuardian(GuardianBase.Rococo))
            {
                Messages.Add("*I've met a friendly TerraGuardian looking for a place to live. He may end up appearing anytime during your adventure.*");
            }
            if (!HasMetGuardian(GuardianBase.Blue))
            {
                Messages.Add("*I've met once a TerraGuardian who liked camping. Maybe she'll stop by if there be a campfire.*");
            }
            if (!HasMetGuardian(GuardianBase.Sardine))
            {
                Messages.Add("*There was that cat... I don't remember his name... He said he was pursuing his highest bounty: The King Slime. I wonder If he were successfull.*");
            }
            if (!HasMetGuardian(GuardianBase.Zacks))
            {
                if (HasMetGuardian(GuardianBase.Blue))
                {
                    Messages.Add("*I can't forget one Blood Moon I survived. I was very far in the world, while being attacked by zombies from all the sides, until a big wolf zombie TerraGuardian appeared. Now that I'm thinking, that zombie looked a lot like " + NpcMod.GetGuardianNPCName(1) + ".*");
                }
                else
                {
                    Messages.Add("*I can't forget one Blood Moon I survived. I was very far in the world, while being attacked by zombies from all the sides, until a big wolf zombie TerraGuardian appeared. I managed to dispatch it, but It was scary.*");
                }
            }
            if (!HasMetGuardian(GuardianBase.Nemesis) && Main.hardMode)
            {
                Messages.Add("*The other day, I was barring my door, because there was a Possessed Armor repeatedly saying \"I'll be your shadow\". I don't know what It was talking about, but was really terrifying.*");
            }
            if (!HasMetGuardian(GuardianBase.Alex))
            {
                Messages.Add("*In the Ether Realm, there is a popular story about a Giant Dog and a Terrarian Woman. They lived happy, went into adventures and played together everyday, until one day she died. Legends says that the Giant Dog buried his owner, and still guards her tombstone since that day. I wonder if those legends are true.*");
            }
            if (!HasMetGuardian(GuardianBase.Brutus))
            {
                int TownNpcCount = (int)(NpcMod.GetCompanionNPCCount() * 0.5f);
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].active && Main.npc[n].townNPC)
                        TownNpcCount++;
                }
                if (TownNpcCount >= 5)
                {
                    Messages.Add("*I've been hearing stories of a Royal Guard from Ether Realm who lost his job, and is now roaming through worlds looking to work as a bodyguard. I think there's a chance that he may be appearing here.*");
                }
                else
                {
                    Messages.Add("*I've been hearing stories of a Royal Guard from Ether Realm who lost his job, and is now roaming through worlds looking to work as a bodyguard. Is said that he has higher chances of appearing in places with many people living.*");
                }
            }
            if (!HasMetGuardian(GuardianBase.Bree))
            {
                Messages.Add("*I've bumped into a white cat earlier, who said she was looking for her husband. She said that she was travelling world by world trying to look for him, and she looked a bit worn out the last time I saw her. I tried convincing her to stay for a while, but she didn't accepted. If you find her, can you convince her to stay for a while?*");
            }
            if (!HasMetGuardian(GuardianBase.Mabel) && Npcs.MabelNPC.CanSpawnMabel)
            {
                Messages.Add("*I've met a TerraGuardian who wanted to try flying like a reindeer. The problem, is that not only reindeers can't fly, but she's not a reindeer. Can you please find her before she gets hurt?*");
            }
            if (!HasMetGuardian(GuardianBase.Domino) && Npcs.DominoNPC.CanSpawnDomino(player))
            {
                if (HasGuardianNPC(GuardianBase.Brutus))
                {
                    Messages.Add("*There is a shady TerraGuardian roaming this world. He seems to be running away from something. Maybe you should bring " + NpcMod.GetGuardianNPCName(GuardianBase.Brutus) + " with you in case you bump with him.*");
                }
                else
                {
                    Messages.Add("*There is a shady TerraGuardian roaming this world. He seems to be running away from something. You can try talking to him, but I don't know if will result into anything fruitful.*");
                }
            }
            if (!HasMetGuardian(10) && Npcs.LeopoldNPC.CanSpawnLeopold)
            {
                Messages.Add("*Did you hear? Leopold is visiting this world! You don't know who he is? He's a famous sage from Ether Realm. I managed to bump into him the other day when I was picking up flowers. I think you may end up finding him any time during your travels.");
            }
            if (!HasMetGuardian(11) && Npcs.VladimirNPC.CanRecruitVladimir)
            {
                Messages.Add("*I heard a weird rumor from a Terrarian who said that found a \"giant bear\" when exploring the Underground Jungle. They said that the bear were saying that was hungry and that wanted them to give him a hug. I think that may be another TerraGuardian, and I recommend you to check that out, since that person seems to be in trouble, and please don't freak out like the other Terrarian.");
            }
            if (!HasMetGuardian(12) && Npcs.MalishaNPC.MalishaCanSpawn)
            {
                Messages.Add("*I heard that we should be careful, since a witch seems to be taking vacation on this world. Who told me that? Well... You wont believe me, but the warning was given by a Bunny.*");
            }
            if (!HasMetGuardian(GuardianBase.Wrath) && Npcs.WrathNPC.WrathCanSpawn)
            {
                Messages.Add("*A person was attacked last night in the forest. They were brought unconscious to the town, and when woke up, said that a \"" +
                    (player.GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.AngerPigGuardianID] ? 
                    "kind of cloud in form of a red pig" :
                    "angry red pig") + "\" attacked them. You need to check that out.*");
            }
            if (!HasMetGuardian(GuardianBase.Fluffles) && Npcs.GhostFoxGuardianNPC.CanGhostFoxSpawn(player))
            {
                Messages.Add("*Watch out, [nickname]. I've been hearing that there's a ghost chasing people in the dark. Better not let it catch you.*");
            }
            if (!HasMetGuardian(GuardianBase.Minerva))
            {
                Messages.Add("*I've met a friendly TerraGuardian who seems to be travelling this world. I don't think you may end up convincing her to stay at first, but she may visit often if she finds out there's a place she can visit.*");
            }
            if (!HasMetGuardian(GuardianBase.Alexander) && Npcs.AlexanderNPC.AlexanderConditionMet)
            {
                Messages.Add("*I heard that there's a TerraGuardian jumping and sleuthing people who tries exploring the dungeon. Based on the what people said, every time ends up with him saying it's not who they're looking for. I wonder who that TerraGuardian is looking for.*");
            }
            if (!HasMetGuardian(GuardianBase.Cinnamon))
            {
                Messages.Add("*There's a cute TerraGuardian sometimes follows Travelling Merchants on their travels. I think she may end up arriving here if that's true.*");
            }
            if (!HasMetGuardian(GuardianBase.Miguel) && Npcs.MiguelNPC.CanSpawnMe())
            {
                Messages.Add("*There's a really buff TerraGuardian exploring this world. He also likes to insult people who don't have \"proper body building\". I know because he did that to me...*");
            }
            if (!HasMetGuardian(GuardianBase.Quentin))
            {
                Messages.Add("*A person told me that they heard someone crying, when exploring the dungeon. Whoever that is, they definitelly seems to need help.*");
            }
            if (Messages.Count == 0)
                return "*I didn't heard about anything latelly.*";
            return Messages[Main.rand.Next(Messages.Count)];
        }
    }
}

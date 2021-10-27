using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon
{
    class GuardianSpawnTip
    {
        private static bool HasMetGuardian(int Id, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            return NpcMod.HasMetGuardian(Id, ModID);
        }
        private static bool HasGuardianNPC(int Id, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            return NpcMod.HasGuardianNPC(Id, ModID);
        }

        public static string GetRandomTip
        {
            get
            {
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
                if (!HasMetGuardian(9) && Npcs.DominoNPC.CanSpawnDomino(Main.player[Main.myPlayer])) //Continue adapting the dialogues from here
                {
                    if (HasGuardianNPC(GuardianBase.Brutus))
                    {
                        Messages.Add("There is a shady Guardian wandering around the world. It looks like he's running from something. I think It's a good idea to ask him what is he up to. Maybe you should bring " + NpcMod.GetGuardianNPCName(GuardianBase.Brutus) + " with you.");
                    }
                    else
                    {
                        Messages.Add("There is a shady Guardian wandering around the world. It looks like he's running from something. I think It's a good idea to ask him what is he up to.");
                    }
                }
                if (!HasMetGuardian(10) && Npcs.LeopoldNPC.CanSpawnLeopold)
                {
                    Messages.Add("Hey, did you hear? A sage from the Ether Realm is exploring the Terra Realm. You may bump into him anytime! If you're goign to look for him, can I go with you?! I'm kind of his fan.");
                }
                if (!HasMetGuardian(11) && Npcs.VladimirNPC.CanRecruitVladimir)
                {
                    Messages.Add("I heard a rumor about a Bear creature travelling around this world. The person who found It, said that the bear were saying that was hungry and that wanting to give a hug, that made the person imediatelly ran away from the Underground Jungle. What could that bear be doing in the Underground Jungle? Maybe It were looking for honey?");
                }
                if (!HasMetGuardian(12) && Npcs.MalishaNPC.MalishaCanSpawn)
                {
                    Messages.Add("I heard weird rumors about a TerraGuardian who's came here to take vacations. That wouldn't be weird, if wasn't for the fact that a Bunny told me that.");
                }
                if (!HasGuardianNPC(GuardianBase.Wrath) && Npcs.WrathNPC.WrathCanSpawn)
                {
                    Messages.Add("One of your citizens told me that was attacked by a " +
                        (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.AngerPigGuardianID] ? "kind of cloud in form of a red pig" :
                        "angry red Pig TerraGuardian") + " in the forest last night. The person were carried to the town unconscious. You should investigate that issue.");
                }
                if (!HasMetGuardian(GuardianBase.Fluffles) && Npcs.GhostFoxGuardianNPC.CanGhostFoxSpawn(Main.player[Main.myPlayer]))
                {
                    Messages.Add("Watch out, I've heard that there's a ghost TerraGuardian who chases people in the forest.");
                }
                if (!HasMetGuardian(GuardianBase.Minerva))
                {
                    Messages.Add("Have you heard? There seems to be a TerraGuardian travelling through the world. Try seeing if you can find her on your travels.");
                }
                if (!HasMetGuardian(GuardianBase.Alexander) && Npcs.AlexanderNPC.AlexanderConditionMet)
                {
                    Messages.Add("There seems to be a TerraGuardian jumping and sleuthing anyone It finds travelling the dungeon, and saying that is not who he's are looking for. I think you may want to check that out.");
                }
                if (!HasMetGuardian(GuardianBase.Cinnamon))
                {
                    Messages.Add("I heard from a Travelling Merchant that there's a Squirrel TerraGuardian that sometimes follows him. If that's about right, It may show up sometimes when one appears.");
                }
                if (!HasMetGuardian(GuardianBase.Miguel) && Npcs.MiguelNPC.CanSpawnMe())
                {
                    Messages.Add("There's a buffed TerraGuardian travelling around this world. He insulted me just because I'm not fit.");
                }
                if (!HasMetGuardian(GuardianBase.Quentin))
                {
                    Messages.Add("I heard from another person, that they heard someone crying for help on the dungeon. You should take a look.");
                }
                if (Messages.Count == 0)
                    return "*I didn't heard about anything latelly.*";
                return Messages[Main.rand.Next(Messages.Count)];
            }
        }
    }
}

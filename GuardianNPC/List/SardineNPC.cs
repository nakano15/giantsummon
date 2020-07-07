using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class CatGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Sardine_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "CatGuardian";
            return mod.Properties.Autoload;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            base.SetChatButtons(ref button, ref button2);
            if (!CheckingRequest)
            {
                if (!GuardianBountyQuest.SardineTalkedToAboutBountyQuests)
                {
                    button2 = "About Bounties";
                }
                else
                {
                    button2 = "Report bounty";
                }
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (CheckingRequest)
            {
                base.OnChatButtonClicked(firstButton, ref shop);
                return;
            }
            if (!firstButton)
            {
                if (!GuardianBountyQuest.SardineTalkedToAboutBountyQuests)
                {
                    Main.npcChatText = "Since It's so boring staying at home, I decided to do something in to make me busy. I will open a Bounty Hunting agency here, but first I need a Sign inside my house. If you end up placing one, I will place the latest bounty here.";
                    GuardianBountyQuest.SardineTalkedToAboutBountyQuests = true;
                }
                else
                {
                    if (GuardianBountyQuest.SignID == -1)
                    {
                        Main.npcChatText = "I can see that you are eager to get requests, but I need a sign in my house first.";
                    }
                    else if (GuardianBountyQuest.PlayerAlreadyRedeemedReward(Main.player[Main.myPlayer]))
                    {
                        Main.npcChatText = "I don't have anything else for you. Wait until another bounty shows up.";
                    }
                    else if (GuardianBountyQuest.PlayerRedeemReward(Main.player[Main.myPlayer]))
                    {
                        Main.npcChatText = "Nice job! If I were there with you, you'd take half the time facing it, but whatever.";
                    }
                    else
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.npcChatText = "The bounty target appears in the " + GuardianBountyQuest.spawnBiome.ToString() + ", cause a mayhem on it until It shows up.";
                        }
                        else
                        {
                            Main.npcChatText = "Beware when facing the target, It is not just a regular monster.";
                        }
                    }
                }
            }
            else
            {
                base.OnChatButtonClicked(true, ref shop);
            }
        }

        public CatGuardian()
            : base(2)
        {

        }
    }
}

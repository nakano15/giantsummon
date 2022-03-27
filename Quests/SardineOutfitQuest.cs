using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader.IO;

namespace giantsummon.Quests
{
    public class SardineOutfitQuest : QuestBase
    {
        private static byte KingSlimeSlot = 255;

        public override string Name => "Bash the King!";

        public override QuestData GetQuestData => new OutfitQuestData();

        public override string Description(QuestData data)
        {
            return "Sardine wants revenge for the shame the King Slime made him pass through.";
        }

        public override string GetQuestCurrentObjective(QuestData rawdata)
        {
            OutfitQuestData data = (OutfitQuestData)rawdata;
            switch (data.QuestStep)
            {
                case 2:
                case 3:
                case 4:
                    return "Speak with Sardine.";
            }
            return "Help Sardine get revenge on the King Slime.";
        }

        public override string QuestStory(QuestData rawdata)
        {
            OutfitQuestData data = (OutfitQuestData)rawdata;
            string Story = "";
            if(data.QuestStep == 0)
            {
                Story = "Sardine still didn't spoke to me about this.";
            }
            else
            {
                Story = "When I talked with Sardine, he told me that wanted to have some revenge on the King Slime, and told me to call him the next time I decide to try facing it.\n" +
                    "He gave me a Slime Crown he managed to create on his personal time. With this, I could try spawning the King Slime when I feel like we're ready to face it.";
                if (data.QuestStep == 2 || data.QuestStep == 5)
                {
                    Story += "\n\nWe were able to defeat the King Slime.";
                    if (data.QuestStep == 2)
                        Story += " I should try speaking with Sardine and hear what he has to say.";
                    else
                    {
                        Story += " After speaking with Sardine, he felt happy about finally managing to defeat the King Slime. He even felt the right of celebrating by wearing an outfit" +
                            " he bought from the Travelling Merchant. Now that he got his revenge, I believe he's no longer haunted by his defeat given by the King Slime.\n\nTHE END";
                    }
                }
                else
                {
                    switch (data.QuestStep)
                    {
                        case 3:
                            Story += "\n\nI went ahead and defeated the King Slime myself, but I don't think Sardine will be happy to hear that.";
                            break;
                        case 4:
                            Story += "\n\nWe managed to defeat the King Slime, but Sardine fell at some point during the fight. I should check him and see if he's alright.";
                            break;
                    }
                }
            }
            return Story;
        }

        public override bool IsQuestStarted(QuestData rawdata)
        {
            OutfitQuestData data = (OutfitQuestData)rawdata;
            return data.QuestStep >= 1;
        }

        public override bool IsQuestComplete(QuestData rawdata)
        {
            OutfitQuestData data = (OutfitQuestData)rawdata;
            return data.QuestStep == 5;
        }

        public override List<DialogueOption> AddDialogueOptions(bool IsTalkDialogue, int GuardianID, string GuardianModID)
        {
            List<DialogueOption> Options = base.AddDialogueOptions(IsTalkDialogue, GuardianID, GuardianModID);
            if (IsTalkDialogue)
            {
                OutfitQuestData data = (OutfitQuestData)Data;
                if (data.QuestStep > 0)
                {
                    Options.Add(new DialogueOption(data.QuestStep < 5 ? "Why do you want to slay the King Slime?" : "How are you feeling now that defeated King Slime?", TalkWithSardineAboutQuest));
                }
            }
            return Options;
        }

        private void TalkWithSardineAboutQuest()
        {
            OutfitQuestData data = (OutfitQuestData)Data;
            if (data.QuestStep < 5)
            {
                Dialogue.ShowDialogueWithContinue("That was a job I were given by another Terrarian. I had to defeat the King Slime, and I ended up stuck inside it.");
                Dialogue.ShowDialogueWithContinue("I already told the Terrarian that the job is done, even more since it wasn't very hard to prove that the king slime was slayed, since all around my body had proof.");
                Dialogue.ShowDialogueWithContinue("But I can't get over the fact that it has actually defeated me. I'm glad that you managed to help me get out of it, but it's also a very shameful situation.");
                Dialogue.ShowDialogueWithContinue("That's why I want to slay it, without being in need of rescue or anything.");
                Dialogue.ShowEndDialogueMessage("Just please, don't tell that to anyone, okay? Leave this as our little secret.", false);
            }
            else
            {
                Dialogue.ShowDialogueWithContinue("Amazing! Whenever I remember of the fight we had against it, I remember how awesome it was.");
                Dialogue.ShowDialogueWithContinue("We really did a really great job at kicking that slimey ass.");
                Dialogue.ShowEndDialogueMessage("");
            }
        }

        public override Action ImportantDialogueMessage(QuestData rawdata, TerraGuardian tg, int GuardianID, string GuardianModID)
        {
            OutfitQuestData data = (OutfitQuestData)rawdata;
            if(tg.ID == GuardianBase.Sardine && tg.ModID == MainMod.mod.Name)
            {
                if(tg.FriendshipLevel >= 3 && data.QuestStep == 0)
                {
                    return SardineTellsHisProblem;
                }
                if(data.QuestStep >= 2 && data.QuestStep <= 4)
                {
                    return SardineCommentsFightResult;
                }
            }
            return base.ImportantDialogueMessage(data, tg, GuardianID, GuardianModID);
        }

        private void SardineTellsHisProblem()
        {
            TerraGuardian Sardine = Dialogue.GetSpeaker,
                Bree = PlayerMod.GetPlayerSummonedGuardian(Main.LocalPlayer, GuardianBase.Bree),
                Glenn = PlayerMod.GetPlayerSummonedGuardian(Main.LocalPlayer, GuardianBase.Glenn);
            Dialogue.ShowDialogueWithContinue("Hey [nickname], I need to speak with you.");
            Dialogue.ShowDialogueWithContinue("I didn't liked the result of my last fight against the King Slime, and I think there's still some gel inside my ears.");
            if (Bree != null)
            {
                Dialogue.ShowDialogueWithContinue("You're still sad about that? I'm actually glad that [nickname] actually were able to rescue you from the inside of that creature.", Bree);
                Dialogue.ShowDialogueWithContinue("That still doesn't make me feel good, since I had to be rescued from it.", Sardine);
                Dialogue.ShowDialogueWithContinue("Don't feel like that, you know that you're still my hero, right?", Bree);
                Dialogue.ShowDialogueWithContinue("Yes, I know, but still...", Sardine);
            }
            OutfitQuestData data = (OutfitQuestData)Data;
            data.QuestStep = 1;
            QuestStartedNotification(data);
            if(Dialogue.ShowDialogueWithOptions("I think we should try doing that fight again, but this time doing it right. What do you say?", new string[] { "Let's bash the king!", "Not now." }) == 0)
            {
                Dialogue.ShowDialogueWithContinue("I like that! Let's go kick King Slime's jelly ass!");
                Item.NewItem(Dialogue.GetSpeaker.HitBox, Terraria.ID.ItemID.SlimeCrown, 1, true, noGrabDelay: true);
                Dialogue.ShowDialogueWithContinue("I have made one of its summoning item in my spare time, so take this.");
                Dialogue.ShowEndDialogueMessage("Do you want to talk about something else before we go?", false);
            }
            else
            {
                Dialogue.ShowDialogueWithContinue("It's not necessary that we do that now, but any time when possible, let's kick it's jelly ass.");
                Item.NewItem(Dialogue.GetSpeaker.HitBox, Terraria.ID.ItemID.SlimeCrown, 1, true, noGrabDelay: true);
                Dialogue.ShowDialogueWithContinue("I think you should have this too. In case you decide to give it a try on King Slime, so we could face it together.");
                Dialogue.ShowEndDialogueMessage("Anyway, is there anything else you want to speak about?", false);
            }
        }

        private void SardineCommentsFightResult()
        {
            TerraGuardian Sardine = Dialogue.GetSpeaker,
                Bree = PlayerMod.GetPlayerSummonedGuardian(Main.LocalPlayer, GuardianBase.Bree),
                Glenn = PlayerMod.GetPlayerSummonedGuardian(Main.LocalPlayer, GuardianBase.Glenn);
            OutfitQuestData data = (OutfitQuestData)Data;
            switch (data.QuestStep)
            {
                case 2: // KS Down, Sardine around.
                    Dialogue.ShowDialogueWithContinue("That was awesome!");
                    Dialogue.ShowDialogueWithContinue("The way the King Slime exploded into that rain of gel all around was really cool.");
                    Dialogue.ShowDialogueWithContinue("I have bought something from the Travelling Merchant the last time he visitted us.");
                    Dialogue.ShowDialogueWithContinue("He said something about being from a creature from a post apocalyptic Tokyo or something, I don't get what that means, but I actually like the outfit itself.");
                    Dialogue.ShowDialogueWithContinue("I think this is a good moment to use it, don't you think?");
                    Dialogue.GetSpeaker.OutfitID = Companions.SardineBase.CaitSithOutfitID;
                    if(Dialogue.ShowDialogueWithOptions("Here, what do you think?", new string[] { "It looks cool.", "It looks bad." }) == 0)
                    {
                        Dialogue.ShowDialogueWithContinue("You think so too? I also find this outfit cool. It even came with a sword.");
                    }
                    else
                    {
                        Dialogue.ShowDialogueWithContinue("Hey! But I liked it. Anyways, it also came with a sword.");
                    }
                    /*if(Glenn != null)
                    {
                        Dialogue.ShowDialogueWithContinue("You look cool in that outfit.", Glenn);
                        Dialogue.ShowDialogueWithContinue("Thanks Glenn, I'll see if I can get one for you too.", Sardine);
                        Dialogue.ShowDialogueWithContinue("Please, no! Matching outfits are lame.", Glenn);
                    }
                    if (Bree != null)
                    {
                        Dialogue.ShowDialogueWithContinue("I actually think it looks good on you.", Bree);
                        Dialogue.ShowDialogueWithContinue("Haha, I think I did a good investiment then.", Sardine);
                        Dialogue.ShowDialogueWithContinue("You didn't bought it with your shared funds, right?", Bree);
                        Dialogue.ShowDialogueWithContinue("Ah, eh... Let's talk about that later?", Sardine);
                    }*/
                    Dialogue.ShowDialogueWithContinue("I don't know if I should use it, but probably will make me look cooler.", Sardine);
                    QuestCompletedNotification(data);
                    data.QuestStep = 5;
                    Terraria.Item.NewItem(Dialogue.GetSpeaker.HitBox, Terraria.ModLoader.ModContent.ItemType<Items.Weapons.CaitSithScimitar>(), 1, true, noGrabDelay: true);
                    Dialogue.ShowEndDialogueMessage("Thank you very much for helping me with the King Slime.", false);
                    return;
                case 3: // KS Down, Sardine absent.
                    Dialogue.ShowDialogueWithContinue("Hello again, I heard that you managed to defeat the King Slime again.");
                    Dialogue.ShowDialogueWithContinue("You know that the deal was that we fight it together, right?");
                    Dialogue.ShowEndDialogueMessage("Next time you plan on facing the King Slime, be sure to call me too. Is there anything else you want to speak to me about?", false);
                    break;
                case 4: // KS Down, Sardine defeated.
                    Dialogue.ShowDialogueWithContinue("That was shameful! How could I be defeated again in that fight?!");
                    Dialogue.ShowDialogueWithContinue("Please, [nickname], I have to at least once be able to defeat that giant jelly.");
                    Dialogue.ShowEndDialogueMessage("Let's try doing better next time, maybe even check our equipments too. By the way, is there something else you want to speak to me about?", false);
                    break;
            }
            data.SardineFell = false;
            data.QuestStep = 1;
        }

        public override void UpdatePlayer(Player player, QuestData rawdata)
        {
            OutfitQuestData data = (OutfitQuestData)rawdata;
            if (data.QuestStep == 1)
            {
                if (KingSlimeSlot < 255)
                {
                    if (!Main.npc[KingSlimeSlot].active || Main.npc[KingSlimeSlot].type != Terraria.ID.NPCID.KingSlime)
                        KingSlimeSlot = 255;
                    else if(!data.SardineFell)
                    {
                        TerraGuardian sardine = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine);
                        if(sardine != null)
                        {
                            data.SardineFell = sardine.KnockedOut || sardine.Downed;
                        }
                    }
                }
                else
                {
                    for(byte i = 0; i < 200; i++)
                    {
                        if(Main.npc[i].active && Main.npc[i].type == Terraria.ID.NPCID.KingSlime)
                        {
                            KingSlimeSlot = i;
                            break;
                        }
                    }
                    if(KingSlimeSlot < 255 && PlayerMod.HasGuardianSummoned(player, GuardianBase.Sardine))
                    {
                        PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine).SaySomething("There it is! Let's slay it!");
                    }
                }
            }
        }

        public override void OnMobKill(NPC killedNpc)
        {
            if(killedNpc.type == Terraria.ID.NPCID.KingSlime)
            {
                OutfitQuestData data = (OutfitQuestData)Data;
                if (data.QuestStep == 1)
                {
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.LocalPlayer, GuardianBase.Sardine))
                    {
                        if (data.SardineFell)
                        {
                            data.QuestStep = 4;
                        }
                        else
                        {
                            data.QuestStep = 2;
                            PlayerMod.GetPlayerSummonedGuardian(Main.LocalPlayer, GuardianBase.Sardine).SaySomethingCanSchedule("We did it! Yes!!");
                        }
                    }
                    else
                    {
                        data.QuestStep = 3;
                    }
                }
            }
        }

        public class OutfitQuestData : QuestData
        {
            /// <summary>
            /// Steps:
            /// 0 = Pre Start
            /// 1 = Quest started
            /// 2 = King Slime Defeated with Sardine Around.
            /// 3 = King Slime Defeated with Sardine Absent.
            /// 4 = King Slime Defeated with Sardine Defeated.
            /// 5 = Quest Finished
            /// </summary>
            public byte QuestStep = 0;
            public bool SardineFell = false;
            private const byte QVer = 0;

            public override void CustomSaveQuest(string QuestKey, TagCompound Writer)
            {
                Writer.Add(QuestKey + "qver", QVer);
                Writer.Add(QuestKey + "qstep", QuestStep);
                Writer.Add(QuestKey + "sardinefell", SardineFell);
            }

            public override void CustomLoadQuest(string QuestKey, TagCompound Reader, int ModVersion)
            {
                byte questver = Reader.GetByte(QuestKey + "qver");
                QuestStep = Reader.GetByte(QuestKey + "qstep");
                SardineFell = Reader.GetBool(QuestKey + "sardinefell");
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Companions
{
    public class NemesisBase : GuardianBase
    {
        /// <summary>
        /// -Doesn't cares about anything.
        /// -Emotions stolen.
        /// -Mirrors Terrarian trying to get some personality from doing so.
        /// -Questions the outfit choices the player gives It.
        /// -Feels indiferent on relationship with town citizens.
        /// </summary>

        public NemesisBase()
        {
            Name = "Nemesis";
            Description = "It's cryptic to know who the Nemesis is, or was.\nNeither if is a \"he\" or a \"she\".";
            Age = 256;
            SetBirthday(SEASON_AUTUMN, 3);
            Male = true;
            InitialMHP = 100; //500
            LifeCrystalHPBonus = 20;
            LifeFruitHPBonus = 10;
            Accuracy = 0.32f;
            Mass = 0.3f;
            MaxSpeed = 3f;
            Acceleration = 0.08f;
            SlowDown = 0.2f;
            MaxJumpHeight = 15;
            JumpSpeed = 5.01f;
            DrinksBeverage = true;
            CanChangeGender = true;
            Effect = GuardianEffect.Wraith;
            IsNocturnal = true;
            SetTerrarian();
            HurtSound = new SoundData(Terraria.ID.SoundID.NPCHit54);
            DeadSound = new SoundData(Terraria.ID.SoundID.NPCDeath52);
            CallUnlockLevel = 0;

            TerrarianInfo.HairStyle = 15;

            PopularityContestsWon = 0;
            ContestSecondPlace = 1;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.WoodenSword, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "If you have something dangerous to do that can possibly end on demise, you can send me to do it instead. I don't care.";
            }
        }
        
        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "You and I are tied together, now.";
                case 1:
                    return "You seems strong enough, I will be your shadow.";
                case 2:
                    return "Who am I doesn't matter, what matters is that now I'm yours.";
            }
            return base.GreetMessage(player, guardian);
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "The only thing I want to do is follow you.";
            return "I don't need anything right now.";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "I want you to [objective] for me. Doesn't matter why.";
            return "I have got a little task for you, If you don't mind. I need you to [objective]. Do it and I'll give you something in exchange.";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "Good. What? Were expecting something else, I can't really express anything for what you did.";
            return "I can't cheer for you doing what I asked you to do.";
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (!Main.bloodMoon)
            {
                Mes.Add("I have no emotion, no memory, neither a physical body.");
            }
            else
            {
                Mes.Add("This night looks perfect for some random violence.");
                Mes.Add("Don't worry about me, I don't feel fear, or any other emotion.");
            }
            if (Main.eclipse)
            {
                Mes.Add("Those creatures seems to have spawned from a flat screen.");
            }
            Mes.Add("I can wear any outfit you give me, just leave it on the first slots of my inventory.");
            Mes.Add("I don't have any goal, so I set my own goal to be of helping you on your quest.");
            Mes.Add("I have become so numb.");
            Mes.Add("Should I worry that you could make me look ridiculous?");
            Mes.Add("I am now your shadow, whenever you need, I will go with you, even if it means my demise.");
            Mes.Add("I were in this world for too long, I have seen several things that happened here.");
            if (Main.dayTime && !Main.eclipse)
                Mes.Add("The clothings I have are like my body parts, since they are visible all time. But that doesn't seems to help making the other citizens feel less alarmed about my presence.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Guide))
            {
                Mes.Add("[nn:" + Terraria.ID.NPCID.Guide + "] doesn't appreciate the fact that I know more things than him.");
            }
            if (!Main.dayTime && !Main.bloodMoon)
            {
                Mes.Add("It's night, I don't feel sleepy.");
                Mes.Add("Doesn't matter if It's day or night, I still am partially invisible anytime.");
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("Everyone seems to be expressing themselves on this party. Me? I'll just stay drinking.");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Clothier))
                Mes.Add("You have emotions, right? What should I be feeling after hearing that [nn:" + Terraria.ID.NPCID.Clothier + "] is a lady killer?");
            if (NpcMod.HasGuardianNPC(1))
                Mes.Add("[gn:1] keeps forgetting to look where she sits.");
            if (NpcMod.HasGuardianNPC(2))
                Mes.Add("I told [gn:2] that I don't feel anything about drinking, after he asked me about going out for a drink sometime.");
            if (NpcMod.HasGuardianNPC(3))
                Mes.Add("Everyone seems uncomfortable about having [gn:3] and me around. I don't know where is the problem.");
            if (NpcMod.HasGuardianNPC(0))
                Mes.Add("[gn:0] always runs away when he sees me. Did I do something to him?");
            if (NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("Before you ask: No, I'm not " + AlexRecruitScripts.AlexOldPartner + ", but I once saw a cheerful woman playing with him during my night stalking a long time ago.");
                Mes.Add("I don't know what it feels by tossing a ball to make [gn:5] fetch it. I just do it because he askes me to.");
            }
            if (NpcMod.HasGuardianNPC(2) && NpcMod.HasGuardianNPC(7))
                Mes.Add("I don't know what is love, or what it is to feel love, but I think [gn:2] and [gn:7] have a very divergent and disturbed relationship.");
            if (NpcMod.HasGuardianNPC(8))
            {
                Mes.Add("I always win the stare contest, because [gn:8] ends up laughing after a few minutes staring my face. I don't know why.");
                Mes.Add("I think [gn:8] is super effective on the town, since she atracts attention of almost everyone in the town. Me? I don't care. \"Sips coffee\"");
            }
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("What? [gn:" + Fluffles + "] wasn't doing a stare contest?");
                Mes.Add("No. [gn:" + Fluffles + "] and I are different. Her soul wasn't devoured by a vile creature.");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("I don't mind sharing the room with you. There's enough space.");
                Mes.Add("If you get a bed for yourself, you are free to stay as long as you want.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("I would like to have some place which I could stay when I'm not following you.");
            Mes.Add("You have some place where I could stay, right?");
            if (!Main.dayTime)
            {
                Mes.Add("It will be hard for you to find me in a place like this. Give me some place to stay.");
                Mes.Add("I am nothing like those night creatures, let me stay in someplace.");
            }
            if (Main.raining)
                Mes.Add("This is not a good situation, give me some place dry.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Why does people here look at me like as if I would kill them in their sleep?");
            Mes.Add("I see all this colorful environment, but can't feel anything.");
            if (PlayerMod.HasGuardianSummoned(player, 4))
            {
                Mes.Add("Take me with you on your quest, sometime.");
            }
            if (Main.raining)
                Mes.Add("The rain passes through my body, but the armor still can take the drops.");
            Mes.Add("The dungeon in this world? It is a place where cultists sacrificed people to awaken some ancient god. A Terrarian has defeated that ancient god, but parts of it remains in this world.");
            if (PlayerMod.HasGuardianSummoned(player, 0))
            {
                Mes.Add("I don't know what it is to feel fun, [gn:0]. So stop doing jokes.");
                Mes.Add("I were wanting to talk to you, [gn:0]. Why do you take people trash with you?");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (!PlayerMod.HasGuardianBeenGifted(player, 4) && Main.rand.NextDouble() < 0.5)
                return "You want to give me something? Ok, I will open it then.";
            return "It doesn't matter how old I am, I nearly don't exist.";
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("I wont copy that, seems shameful.");
                Mes.Add("Are you supposed to stay lying down on the floor?");
                Mes.Add("Those wounds will be taken care of.");
            }
            else
            {
                Mes.Add("You will be back soon.");
                Mes.Add("I'll take care of this.");
                Mes.Add("You look exausted.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompanionRecruitedMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            Weight = 0.8f;
            return "...";
        }

        public override string CompanionJoinGroupMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {

                }
            }
            Weight = 1f;
            return "...Welcome.";
        }
        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "I'm your shadow, so there was no need for asking.";
                case MessageIDs.RescueMessage:
                    return "Here we are, now let's take care of those wounds.";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "What. I can't really sleep.";
                        case 1:
                            return "Yes, I'm awake.";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "Did you do what I asked?";
                        case 1:
                            return "What about my request?";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "Yes, sure.";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "No. Too many people.";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "Not now.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "This doesn't seems like a good way to leave me.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "I'll stay then.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "I know the way home.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "Alright.";
                case MessageIDs.RequestAccepted:
                    return "Good.";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "No. You have too many requests to do.";
                case MessageIDs.RequestRejected:
                    return "Fine. Then don't do It.";
                case MessageIDs.RequestPostpone:
                    return "It will be waiting here, then.";
                case MessageIDs.RequestFailed:
                    return "Failure was not an option.";
                case MessageIDs.RequestAsksIfCompleted:
                    return "You completed my request?";
                case MessageIDs.RequestRemindObjective:
                    return "[objective]. There, now do it.";
                case MessageIDs.RestAskForHowLong:
                    return "How long do you plan on resting?";
                case MessageIDs.RestNotPossible:
                    return "No, not a good time.";
                case MessageIDs.RestWhenGoingSleep:
                    return "...";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "[shop]'s shop has something useful for me.";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "Yes, this one.";
                case MessageIDs.GenericYes:
                    return "Yes.";
                case MessageIDs.GenericNo:
                    return "No.";
                case MessageIDs.GenericThankYou:
                    return "Thanks.";
                case MessageIDs.GenericNevermind:
                    return "Nevermind.";
                case MessageIDs.ChatAboutSomething:
                    return "Speak.";
                case MessageIDs.NevermindTheChatting:
                    return "Ok.";
                case MessageIDs.CancelRequestAskIfSure:
                    return "So, you don't want to do what I asked anymore?";
                case MessageIDs.CancelRequestYesAnswered:
                    return "Okay. You no longer need to do It.";
                case MessageIDs.CancelRequestNoAnswered:
                    return "Then It was just a mistake of what to say.";
                //
                case MessageIDs.ReviveByOthersHelp:
                    return "Brought back.";
                case MessageIDs.RevivedByRecovery:
                    return "I returned.";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "Life Crystal.";
                case MessageIDs.FoundPressurePlateTile:
                    return "A trap.";
                case MessageIDs.FoundMineTile:
                    return "Mine.";
                case MessageIDs.FoundDetonatorTile:
                    return "A detonator over there.";
                case MessageIDs.FoundPlanteraTile:
                    return "Our doom will approach if we break that.";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "";
                case MessageIDs.FoundTreasureTile:
                    return "A chest.";
                case MessageIDs.FoundGemTile:
                    return "Gems over there.";
                case MessageIDs.FoundRareOreTile:
                    return "Ores.";
                case MessageIDs.FoundVeryRareOreTile:
                    return "Rare ores.";
                case MessageIDs.FoundMinecartRailTile:
                    return "";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "";
                case MessageIDs.CompanionInvokesAMinion:
                    return "Minion, do my bidding.";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "Don't we have something more important to do?";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "I have another shadow named [player]. They also check up on me frequently.";
                case MessageIDs.FeatMentionBossDefeat:
                    return "[player] defeated [subject] recently.";
                case MessageIDs.FeatFoundSomethingGood:
                    return "[player] found [subject] in their travels.";
                case MessageIDs.FeatEventFinished:
                    return "[player] took care of a [subject] that happened in their world.";
                case MessageIDs.FeatMetSomeoneNew:
                    return "I heard that [player] met [subject].";
                case MessageIDs.FeatPlayerDied:
                    return "A terrarian has died recently. Their name was [player].";
                case MessageIDs.FeatOpenTemple:
                    return "A temple door was opened by [player] in [subject], recently.";
                case MessageIDs.FeatCoinPortal:
                    return "A coin portal isn't a myth. [player] proved that.";
                case MessageIDs.FeatPlayerMetMe:
                    return "I'm also [player]'s shadow.";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "[player] spends too much time helping the Angler Kid.";
                case MessageIDs.FeatKilledMoonLord:
                    return "Yes, I know that [player] killed the Moon Lord at [subject].";
                case MessageIDs.FeatStartedHardMode:
                    return "Looks like the tides shifted on [subject].";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "I heard about [player] picking [subject] as their buddy. I don't care.";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "Does being your buddy means that I'm officially your shadow?";
                case MessageIDs.DeliveryGiveItem:
                    return "Take that [item], [target].";
                case MessageIDs.DeliveryItemMissing:
                    return "Hm. The item isn't on me anymore.";
                case MessageIDs.DeliveryInventoryFull:
                    return "I can't give [target] an item with their inventory full.";
                case MessageIDs.CommandingLeadGroupSuccess:
                    return "I shall mimic you then.";
                case MessageIDs.CommandingLeadGroupFail:
                    return "No. I'd preffer to copy you.";
                case MessageIDs.CommandingDisbandGroup:
                    return "Everyone, begone.";
                case MessageIDs.CommandingChangeOrder:
                    return "I shall do so.";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}

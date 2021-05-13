using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Creatures
{
    public class WrathBase : PigGuardianFragmentBase
    {
        public WrathBase() //I'll need to think how I'll make the cloud form of them work, and toggle.
            : base(AngerPigGuardianID)
        {
            Name = "Wrath";
            PossibleNames = new string[] { "Wrath", "Rage", "Fury", "Irk" };
            Description = "One of the emotion pieces fragments\nof a TerraGuardian. Very volatile.";
            Width = 10 * 2;
            Height = 27 * 2;
            SpriteWidth = 64;
            SpriteHeight = 68;
            FramesInRows = 30;
            //DuckingHeight = 54;
            //Each pig should definitelly have the same size, birthday age and time, so I moved those infos.
            Male = true;
            InitialMHP = 110; //320
            LifeCrystalHPBonus = 15;
            LifeFruitHPBonus = 5;
            Accuracy = 0.67f;
            Mass = 0.40f;
            MaxSpeed = 3.62f;
            Acceleration = 0.12f;
            SlowDown = 0.35f;
            MaxJumpHeight = 12;
            JumpSpeed = 9.76f;
            CanDuck = false;
            ReverseMount = true;
            DontUseHeavyWeapons = true;
            SetTerraGuardian();

            MountUnlockLevel = 255;

            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.RedPhaseblade, 1));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.LesserHealingPotion, 10));

            //Animation Frames

            GetRequests();
        }

        public void GetRequests()
        {
            AddNewRequest("Sublimation", 350, 
                "*This form is revolting, I'm boiling out of rage due to this. There must be a way of making me solid again, maybe that nerdy guy can help me in this, let's talk to him.*",
                "*Great, or else I would give you a beating.*",
                "*Oh you...! (Insert several different insults here)*",
                "*What are you waiting for?! I'M SICK OF BEING A CLOUD YOU FOOL!*");
            AddRequestRequirement(delegate(Player player)
            {
                return PlayerMod.PlayerHasGuardian(player, Leopold) && !player.GetModPlayer<PlayerMod>().TalkedToLeopoldAboutThePigs;
            });
            AddTalkToGuardianRequest("*Hm... Actually, I do know about his condition. It seems like his body has vaporized at the moment his personality was split. I can try doing something to make his personality solid, but I can only merge his personalities if you find them.*", Leopold);
        }

        public override void Attributes(TerraGuardian g)
        {
            g.MeleeDamageMultiplier += 0.05f;
            g.Defense -= 4;
            if (GetIfIsCloudForm(g))
            {
                //if (!g.HasFlag(GuardianFlags.NoGravity))
                //    g.AddFlag(GuardianFlags.NoGravity);
                //if (!g.HasFlag(GuardianFlags.NoTileCollision))
                //    g.AddFlag(GuardianFlags.NoTileCollision);
            }
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm so furious, why I'm furious? I don't know! That's what makes me more furious!*");
            Mes.Add("*Grrr. GRRRRR!! Grrrrrrrrrrr!*");
            Mes.Add("*Who are you?! What?! Something's funny with my face?! Want to taste my punch?!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm so angry, actually I'm FURIOUS!*");
            Mes.Add("*Stay away! I'm not in the mood.*");
            Mes.Add("*Grrr! I'm so furious!*");
            Mes.Add("*Whaaaaaaaat?!*");
            Mes.Add("*I'm trying my best to be less angry, but I can't!*");
            bool CloudForm = player.GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.AngerPigGuardianID];
            if (CloudForm)
            {
                Mes.Add("*Don't you dare doing any joke about my current form. DON'T. YOU. DARE!*");
                Mes.Add("*I really hate being a cloud, and don't you dare breath near me.*");
            }
            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*Perfect! I can discount my rage on them!*");
                    Mes.Add("*Bring them on! I'll take care of them!*");
                }
                else
                {
                    Mes.Add("*That lush green grass and the bird chirping sounds is driving me nuts!*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*What?! You thought I would be more angry this night? Get lost!*");
                    Mes.Add("*Maybe beating them very hard will make me less angry!*");
                }
                else
                {
                    Mes.Add("*Urgh! All those \"Grahs\" during the night are infuriating me! I'm nearly going outside and shutting them up!*");
                }
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*Don't you have anything else to do? Go away!*");
                Mes.Add("*Want me to put your head in the toilet and then push the flush? Then GO AWAY!*");
            }
            if (Main.raining)
            {
                Mes.Add("*Great, It couldn't get worse, right?*");
                Mes.Add("*I tried! Even being in the rain wont make me less furious!*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Alex))
                Mes.Add("*GO AWAY! Oh, my bad. I thought It was [gn:"+GuardianBase.Alex+"] wanting to play.*");
            if (NpcMod.HasGuardianNPC(GuardianBase.Brutus))
                Mes.Add("My rage wont lower! I even asked [gn:"+GuardianBase.Brutus+"] if he would let me beat him to try lowering my rage, and It didn't work!");
            if (NpcMod.HasGuardianNPC(GuardianBase.Malisha))
            {
                Mes.Add("How many times I have to tell you, that I'M NOT GOING TO PARTICIPATE OF ANY... Oh.. My bad, I thought you were [gn:"+GuardianBase.Malisha+"].");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Leopold))
            {
                if (player.GetModPlayer<PlayerMod>().TalkedToLeopoldAboutThePigs)
                    Mes.Add("*It seems like the only way of lowering my rage, is if I get fused together with my other emotions. But where could they be?*");
                else
                    Mes.Add("*I wonder if the nerdy guy knows how I could get off this form.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Sardine))
            {
                Mes.Add("*[gn:" + Sardine + "] has the kind of thing that keeps me busy with, that's why I'm not punching random people here.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Domino))
            {
                Mes.Add("*If I ever see [gn:" + Domino + "] making another joke about me, I'll beat him so hard that he'll need plastic surgery!*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Vladimir))
            {
                Mes.Add("*The hugs [gn:"+GuardianBase.Vladimir+"] gives doesn't works either! I can't get less angry with them!*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Fluffles))
            {
                if(CloudForm)
                    Mes.Add("*I already said that I'm not a ghost. Don't compare me to [gn:"+Fluffles+"].*");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                if (!guardian.KnockedOutCold)
                {
                    Mes.Add("*Ngh... Just you wait... Until I stand up...* (He seems very furious to the one who have beaten him.)");
                    Mes.Add("*Ugh... My blood is boiling right now... Pain is increasing... The rage rises... Cough...* (He seems very furious to the one who have beaten him.)");
                }
                else
                {
                    Mes.Add("(His body is trembling while he's passed out.)");
                    Mes.Add("(You can hear loud breathing noises coming from his nose.)");
                }
            }
            else if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("(He growls while sleeping, like as if was going to bite someone.)");
                Mes.Add("(He doesn't seems to be having a very peaceful sleep, because of the constant movements he does.)");
                Mes.Add("(It looks like he's fighting against someone in his sleep.)");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Sometimes I temble out of rage. If you ever see me in that state, don't get close.*");
            Mes.Add("*I still don't remember anything from before I woke up. I wonder who was I before what ever made me unconscious.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm too furious to try doing this right now. Could you do It instead?*");
            Mes.Add("*Grrr!! There is something I should do that is making me furious, but I can't do that myself. Would you do It?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*NO!*");
            Mes.Add("*I don't! There isn't anything!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Good. I can't feel happy about this, so take this as a... Friendly rage.*");
            Mes.Add("*Okay, I wont hurt you for a few hours, is that a good reward?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*No, I'm not happy, I'm still angry, you know.*");
            Mes.Add("*All those people dancing around, meanwhile I'm here, this is making me so furious.*");
            if (!PlayerMod.HasGuardianBeenGifted(player, Wrath))
                Mes.Add("*I hope the gift is good.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("*Come on, wake up!*");
                Mes.Add("*I didn't agree to baby sit you.*");
                Mes.Add("*I hope you don't die on me, I will be very angry with you if you do so.*");
            }
            else
            {
                Mes.Add("*Hey, get up, now!*");
                Mes.Add("*I hope you aren't doing that on purpose.*");
                Mes.Add("*This is already getting me furious.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I need a house! Now!*");
            Mes.Add("*This delay is really getting me angry. How long until you give me a house?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "*When you wake up, tell me who did this to you.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*You got the nerve of waking me up in the middle of the night. Do you want to be punched?*";
                        case 1:
                            return "*I'm not happy at all about you waking me up. Actually, I'm FURIOUS!*";
                        case 2:
                            return "*Grrr... I had several troubles getting to sleep. Why you woke me up?*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Please, tell me you did my request, tell me, because I really want to hit you hard.*";
                        case 1:
                            return "*You better have completed my request, because I'm even more furious now.*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*Good, I could use discounting my rage on our opposition.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*There's too many people! I hate mobs! They make me furious!*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*Not now!*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*You're crazy?! You plan on leaving me here?!*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*Grr... Fine.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*Grrr.... Alright... Do It your way then...*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*I thought so.*";
                case MessageIDs.RequestAccepted:
                    return "*Try not to take too long.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*No more. Go deal with your other requests first!*";
                case MessageIDs.RequestRejected:
                    return "*Grr... Fine. I'll do It myself.*";
                case MessageIDs.RequestPostpone:
                    return "*What?! But I wanted It now!*";
                case MessageIDs.RequestFailed:
                    return "*WHAT? GRrrrrrrrr.... GRRRRRRRRRRRRR... *";
                case MessageIDs.RestAskForHowLong:
                    return "*Hmph, how long?!*";
                case MessageIDs.RestNotPossible:
                    return "*This is a horrible moment for that!*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*You'll be sorry If you make me fall from the bed.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Wait, [shop] has something I need.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Don't you dare leave me behind!*";
                case MessageIDs.GenericYes:
                    return "*YES!*";
                case MessageIDs.GenericNo:
                    return "*NO!*";
                case MessageIDs.GenericThankYou:
                    return "*THANKS!*";
                case MessageIDs.ChatAboutSomething:
                    return "*Grrr... Fine. Just don't annoy me.*";
                case MessageIDs.NevermindTheChatting:
                    return "*Finally.*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*You what? Tell me that you really didn't mean that!*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Urgh... Okay... I can do It myself them. No thanks to you, of course!*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Then you just wanted to anger me?!*";
                case MessageIDs.LeopoldMessage1:
                    return "*He's already starting to annoy me. Can I beat him up?*";
                case MessageIDs.LeopoldMessage2:
                    return "*What?! How can you think of attacking me? I'm trying to save you!*";
                case MessageIDs.LeopoldMessage3:
                    return "*Save me? I'm following that Terrarian because I want, and also because he asked me to.*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}

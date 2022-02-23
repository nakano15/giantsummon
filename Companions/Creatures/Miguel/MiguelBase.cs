using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Companions
{
    public class MiguelBase : GuardianBase
    {
        public MiguelBase()
        {
            Name = "Miguel";
            Description = "Your very own personal trainer, like it or not.";
            Size = GuardianSize.Large;
            Width = 22;
            Height = 94;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 120;
            Scale = 112 / 96;
            CompanionSlotWeight = 1.1f;
            DefaultTactic = CombatTactic.Charge;
            Age = 21;
            SetBirthday(SEASON_SUMMER, 4);
            Male = true;
            InitialMHP = 225; //1200
            LifeCrystalHPBonus = 45;
            LifeFruitHPBonus = 15;
            Accuracy = 0.36f;
            Mass = 0.45f;
            MaxSpeed += 2.2f;
            Acceleration = 0.26f;
            SlowDown = 0.38f;
            MaxJumpHeight = 12;
            JumpSpeed = 7.08f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = true;
            SetTerraGuardian();
            CallUnlockLevel = 0;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.BladeofGrass, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 5);

            //Animation Frames
            StandingFrame = 2;
            WalkingFrames = new int[] { 3, 4, 5, 6, 7, 8, 9, 10 };
            PlayerMountedArmAnimation = JumpFrame = 11;
            HeavySwingFrames = new int[] { 13, 14, 15 };
            ItemUseFrames = new int[] { 12, 13, 14, 15 };
            DuckingFrame = 18;
            DuckingSwingFrames = new int[] { 19, 20, 21 };
            SittingFrame = 17;
            ChairSittingFrame = 16;
            ThroneSittingFrame = 22;
            BedSleepingFrame = 23;
            SleepingOffset.X = 16;
            ReviveFrame = 24;  //Rework animation before enabling this.
            //ReviveFrame = 21;
            DownedFrame = 25;

            BackwardStanding = 1;
            BackwardRevive = 26; //Needs animation rework, just like frame 24

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(16, 0);
            BodyFrontFrameSwap.Add(17, 0);

            RightArmFrontFrameSwap.Add(24, 0);

            SittingPoint2x = new Microsoft.Xna.Framework.Point(22, 47);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(12, 12, 12);
            LeftHandPoints.AddFramePoint2x(13, 31, 20);
            LeftHandPoints.AddFramePoint2x(14, 35, 28);
            LeftHandPoints.AddFramePoint2x(15, 30, 37);

            LeftHandPoints.AddFramePoint2x(17, 28, 35);
            LeftHandPoints.AddFramePoint2x(26, 28, 35);

            LeftHandPoints.AddFramePoint2x(19, 14, 13);
            LeftHandPoints.AddFramePoint2x(20, 33, 23);
            LeftHandPoints.AddFramePoint2x(21, 32, 42);

            LeftHandPoints.AddFramePoint2x(24, 32, 46);

            LeftHandPoints.AddFramePoint2x(26, 32, 46);

            //Right Arm
            RightHandPoints.AddFramePoint2x(12, 24, 12);
            RightHandPoints.AddFramePoint2x(13, 34, 20);
            RightHandPoints.AddFramePoint2x(14, 38, 28);
            RightHandPoints.AddFramePoint2x(15, 34, 37);

            RightHandPoints.AddFramePoint2x(19, 17, 13);
            RightHandPoints.AddFramePoint2x(20, 36, 23);
            RightHandPoints.AddFramePoint2x(21, 35, 42);

            RightHandPoints.AddFramePoint2x(24, 32, 46);

            RightHandPoints.AddFramePoint2x(26, 32, 46);

            //Mount Position
            MountShoulderPoints.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(17, 23);
            MountShoulderPoints.AddFramePoint2x(18, 17, 27);
            MountShoulderPoints.AddFramePoint2x(23, 18, 29);
            MountShoulderPoints.AddFramePoint2x(24, 17, 29);
            MountShoulderPoints.AddFramePoint2x(26, 17, 29);

            //Helmet Position
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(25 - 2, 20 + 2);
            HeadVanityPosition.AddFramePoint2x(18, 25 - 2, 24 + 2);
            HeadVanityPosition.AddFramePoint2x(24, 25 - 2, 24 + 2);
            HeadVanityPosition.AddFramePoint2x(26, 25 - 2, 24 + 2);
        }

        public override void Attributes(TerraGuardian g)
        {
            g.MeleeDamageMultiplier += 0.1f;
            g.MeleeCriticalRate += 8;
            g.DefenseRate += 0.02f;
            if (g.BlockRate > 0)
                g.BlockRate += 3;
            g.CoverRate += 10;
        }

        #region Messages

        public override string CallUnlockMessage => "*Hey, you! I need to find out if you are doing the exercises correctly. If you need me to train you personally, all you need is just to call. I can help you on your adventure too, on the way.*";
        public override string MountUnlockMessage => "*You look in bad shape, [nickname]. I need some weight on my arm to keep it's muscle strong, so you may help me with that.*";
        public override string ControlUnlockMessage => "*I believe there are some things you may not be able to do yourself. I can do it for you, as long as you keep doing your exercises.*";

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte BodyPartID, ref int Frame)
        {
            if (Frame == StandingFrame && guardian.TargetID == -1)
                Frame = 0;
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "*I see that you need some help making your muscles show up. Gladly I can help you with that.*";
                case 1:
                    return "*It seems like you have been eating more than burning fat. I will prepare some exercises for you to do.*";
                case 2:
                    return "*How are you able to use that weapon with those thin arms? Time to make you grow some muscles in those arms.*";
            }
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*My objective regarding you is to make you grow some muscle, not to make you like me, unless you want.*");
            Mes.Add("*When doing exercises, you must feel that you are making effort during it. Do not mistake that by pain, because if you do, stop right away.*");
            Mes.Add("*If you're feeling pain during your exercises, stop right away what you're doing and seek me. You may end up injuring yourself that way.*");
            Mes.Add("*You're ready for the next exercise?*");
            Mes.Add("*Just because you're doing exercises doesn't means you must eat poorly or light food. You need energy to burn, so eat a moderate plate of food. If you don't have energy, you'll pass out. Got it?*");
            Mes.Add("*I wonder if my wife will visit this world some day.*");
            Mes.Add("*Only because you get stronger doesn't means you get stupidier. The same is valid for the inverse. Keep that on mind.*");

            if (player.HasBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.Fit>()))
            {
                Mes.Add("*I can already see some muscles on your body. Nice job.*");
                Mes.Add("*Aren't you feeling better now that you are fit, [nickname]?*");
                Mes.Add("*I see that you have been doing exercises frequently. That's really good.*");
            }
            else
            {
                Mes.Add("*You look a bit skinny right now. I can fix that with daily exercises specially for you.*");
                Mes.Add("*Why your belly has more volume than the rest of your body? Let's change that.*");
                Mes.Add("*Have you been eating many chips and junk food? Let's convert that fat into muscles.*");
            }

            if (Main.dayTime)
            {
                if (!Main.raining)
                {
                    Mes.Add("*The weather looks perfect.*");
                    Mes.Add("*How are you doing? Came to get some advices?*");
                    Mes.Add("*Wait. Take a deep breath. Nice. It's good, isn't?*");
                }
                else
                {
                    Mes.Add("*I love the peace the rain brings.*");
                    Mes.Add("*Fearing a few rain drops, [nickname]? Or are you fearing catching flu?*");
                    Mes.Add("*I really hope nothing ruins this weather. I really wanted to stay at home right now.*");
                }
            }
            else
            {
                if (!Main.raining)
                {
                    Mes.Add("*Preparing yourself for a rest, [nickname]? Don't forget to eat something before sleeping.*");
                    Mes.Add("*If you're feeling depleted, don't feel bad about falling on a bed and sleeping. Your muscles need time to recover from the exercises.*");
                    Mes.Add("*Yawn~ What? I'm not made of rock either, I also feel sleepy during night.*");
                }
                else
                {
                    Mes.Add("*I will love sleeping with the sound of rain drops and chill weather.*");
                    Mes.Add("*Can't sleep, [nickname]? Let the rain help you fall asleep.*");
                    Mes.Add("*Seeking an more training? I was wanting to enjoy the weather.*");
                }
            }

            if (NpcMod.HasGuardianNPC(Rococo))
            {
                Mes.Add("*Can you help me convince [gn:"+Rococo+"] to do some exercises?*");
            }
            if (NpcMod.HasGuardianNPC(Sardine))
            {
                Mes.Add("*I'm having to spend some time trying to convert [gn:"+Sardine+"]'s belly fat into muscles.*");
                Mes.Add("*[gn:"+Sardine+"] is already fast, even though he is quite fat. Wonder how faster he will get once he gets muscles.*");
            }
            if (NpcMod.HasGuardianNPC(Zacks))
            {
                Mes.Add("*[gn:"+Zacks+"] asked me to give him some exercises for strengthening himself, but his body is decaying. I doubt there will be any effect if he did.*");
                Mes.Add("*Why [gn:"+Zacks+"] sometimes stares at me, like as if was seeing a beef?*");
            }
            if (NpcMod.HasGuardianNPC(Alex))
            {
                Mes.Add("*[gn:"+Alex+"] said that wanted to get stronger to protect you. I gave him some exercises that may work on him.*");
            }
            if (NpcMod.HasGuardianNPC(Brutus))
            {
                Mes.Add("*Do you know why [gn:"+Brutus+"] left arm is stronger than his right arm?*");
                Mes.Add("*I need to find ways of controlling [gn:"+Brutus+"] stomach. He's creates more fat than being able to burn it.*");
                Mes.Add("*I don't think [gn:"+Brutus+"] needs much advices from me. I challenged him to a arm wrestling and lost. But I will try to invest into making him stronger.*");
            }
            if (NpcMod.HasGuardianNPC(Bree))
            {
                Mes.Add("*You wouldn't believe how strong [gn:"+Bree+"] is. I recommended her some weights to lift as exercise, and she only felt something when she got ones with double the weight.*");
                Mes.Add("*You think [gn:"+Bree+"] is what made her stronger? I don't think so. She seems to be strong herself. She must do something that strains her muscles.*");
            }
            if (NpcMod.HasGuardianNPC(Malisha))
            {
                Mes.Add("*Did you knew that [gn:"+Malisha+"] asked me to give her some exercises for her tail? I wonder what is that for.*");
                Mes.Add("*I really hate it when [gn:"+Malisha+"] calls me a walking beef.*");
            }
            if (NpcMod.HasGuardianNPC(Leopold))
            {
                Mes.Add("*[gn:"+Leopold+"] keeps refusing my proposal of training him. He keeps saying that would rather watch slimes procreate.*");
                Mes.Add("*It's funny how for someone who uses magical weapons, [gn:" + Leopold + "] charges full on melee when his mana runs out.*");
                Mes.Add("*If you see [gn:" + Leopold + "], can you try convincing him to do some exercises?*");
            }
            if (NpcMod.HasGuardianNPC(Mabel))
            {
                Mes.Add("*It's quite complicated to train [gn:" + Mabel + "]. She makes me sweat... A lot....*");
                Mes.Add("*Normally I would ask [gn:"+Mabel+"] out for a date, but I'm a married man, so I wont.*");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("*I don't even need to tell [gn:"+Vladimir+"] to carry some weight and grow muscle, since eventually he's carrying someone.*");
            }
            if (NpcMod.HasGuardianNPC(Alexander))
            {
                Mes.Add("*I asked [gn:"+Alexander+"] why his torax and legs are so fit, and he said that was because of running from many ghosts.*");
            }
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("*You really have some weird people living here. [gn:"+Fluffles+"] gives me some chills, beside she seems nice.*");
                Mes.Add("*The other day, [gn:"+Fluffles+"] was on my shoulder when I went to visit my wife. Things didn't ended very well.*");
            }
            if(NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("*I'm giving [gn:"+Minerva+"] some rigorous exercises to make her lose fat.*");
                Mes.Add("*I can wonder how much [gn:"+Minerva+"] likes to taste her own food, but that is very unhealthy for her.*");
                Mes.Add("*If you're hungry, go visit [gn:"+Minerva+"] so she gives you something for you to eat. That way you'll refill your energy.*");
            }
            if (NpcMod.HasGuardianNPC(Liebre))
            {
                Mes.Add("*I can't believe. [gn:" + Liebre + "] actually asked me for some exercises he cold do. I hope his plasma shell actually gets stronger.*");
                Mes.Add("*Seeing [gn:" + Liebre + "] watching around, makes me feel like someone is about to die soon. It's creepy.*");
            }
            if (NpcMod.HasGuardianNPC(Quentin))
            {
                Mes.Add("*[gn:" + Quentin + "] may have really strong willpower, but he need to have stronger arm and body too. Nobody wants to be knocked out by a punch in the face, right?*");
            }
            if (NpcMod.HasGuardianNPC(Wrath))
            {
                Mes.Add("*[gn:" + Wrath + "] is very hostile, I'm neglecting to ask them to train.*");
            }
            if (NpcMod.HasGuardianNPC(Fear))
            {
                Mes.Add("*I don't get why [gn:" + Fear + "] fears me so much? Is it my exercises or my appearance? Probably my exercises. I should try explaining them to them and see if they get less scared.*");
            }
            if (NpcMod.HasGuardianNPC(CaptainStench))
            {
                Mes.Add("*For some reason, [gn:" + CaptainStench + "] said she would do exercise if she gets some materialistic reward as compensation. Can you believe that? As if being fit wasn't a reward in itself.*");
            }
            if (NpcMod.HasGuardianNPC(Luna))
            {
                Mes.Add("*I really would love seeing fit [gn:" + Luna + "]... I mean... I... I mean... Err... Forget it.*");
            }
            if (NpcMod.HasGuardianNPC(Green))
            {
                Mes.Add("*Hey [nickname]. With the help of [gn:"+Green+"], I can give exercise to people, while he helps with the diet and nutrients. Doesn't that seem good?*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*I'm okay with sharing my room with you. I hope my morning exercises don't end up waking you.*");
                Mes.Add("*Sorry if I sound really beaten during the night, but I really get depleted when I go sleep, so I literally sleep like a log.*");
            }

            if (guardian.IsUsingToilet)
            {
                Mes.Clear();
                Mes.Add("*[nickname], this is not the place and time for that. If you want another training, or just to talk, you could wait until I'm done with my things.*");
                Mes.Add("*I think this toilet is going to overflow... Ah... Wh.. [nickname]! When did you appeared?*");
                Mes.Add("*Yes, I do my business like anyone else. You watching me is making it harder for me to finish this.*");
            }
            else if (guardian.IsSleeping)
            {
                Mes.Clear();
                Mes.Add("*(Sleep talking) Pull... Push... Pull... Push... Now do that 10 more times...*");
                Mes.Add("(He's snoring quite loud. It seems like he really blacked out.)");
                Mes.Add("*Stop! You're going strain a tendon... (He's dreaming about training someone)*");
            }
            else if (Main.eclipse)
            {
                Mes.Clear();
                Mes.Add("*Don't overdo yourself, [nickname]. Sometimes you must retreat.*");
                Mes.Add("*Those creatures looks challenging. Let's hope the exercises I gave you helps.*");
                Mes.Add("*You can defend yourself today, right?*");
            }
            else if (Main.bloodMoon)
            {
                Mes.Clear();
                Mes.Add("*Practicing your arm through the night, [nickname]? I hope you have enough energy to spend.*");
                Mes.Add("*You know, this moment is perfect for doing your thing: Killing hostile creatures.*");
                Mes.Add("*I wonder if the moon being red is because of all the killing you're doing.*");
            }
            if(FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*She's not gonna jump onto my shoulder, right?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*I don't have anything I need right now, but if you want some exercises, I can make up something for you.*";
            return "*No, I don't need anything done right now. Do you want to speak about something else?*";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (guardian.request.IsTravelRequest && Main.rand.NextDouble() < 0.5)
            {
                return "*I need to exercise my legs, and I think you can help me with that. Can you [objective]?*";
            }
            if (!guardian.request.IsTravelRequest && Main.rand.NextDouble() < 0.5)
                return "*This isn't an exercise, I really need this done. I'm busy with some other things, and can't [objective] right now. Can you do it for me, instead?*";
            return "*I can give you a break of training. There is a thing that I need done, which is [objective]. Can you fulfill that?*";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*Perfect. That is exactly what I wanted done. Nice job.*";
            return "*That's great. You can take some rest before returning to your things.*";
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if(!PlayerMod.HasGuardianBeenGifted(player, guardian) && Main.rand.NextDouble() < 0.5)
            {
                return "*I wonder if you have something special for me, [nickname].*";
            }
            if (Main.rand.NextDouble() < 0.5) return "*[nickname], I recommend you to enjoy the party. Dancing actually is a good exercise too, and I'll show how good I am at it.*";
            return "*As long as you don't exagerate on the eating, you may eat some of the party treats.*";
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*You overdid yourself. Let me take a look at that wound.*");
            Mes.Add("*It's just a flesh wound. You'll be walking soon.*");
            Mes.Add("*Rest for a while, I'll take care of those wounds.*");
            Mes.Add("*Gladly I know a bit of first aid.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*My wife is currently unsure about visiting this world. She doesn't think it's safe.*");
            Mes.Add("*I've been exercising my muscles since 15, and learned about methods to exercise them at 17. It has been a long road.*");
            Mes.Add("*People tend to avoid me because they think I will only speak to them about exercises, but I also speak about other things.*");
            Mes.Add("*Some day I'll introduce you to my wife. I'm sure you'll like her since the greeting.*");
            Mes.Add("*Why some of your citizens want to mount on my back? Do I look like a chariot?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.LeopoldMessage1:
                    return "*Murmuring... Murmuring... Murmuring...*";
                case MessageIDs.LeopoldMessage2:
                    return "*Hey! Don't mock me. Why are you following that Terrarian?*";
                case MessageIDs.LeopoldMessage3:
                    return "*Pft. I'm their personal trainer, and that Terrarian is not stupid, they heard everything you said.*";
                //
                case MessageIDs.BuddySelected:
                    return "*Don't think I will take lightly with you for that, but thanks, anyways.*";
                case MessageIDs.RescueMessage:
                    return "*You seems to have exagerated on your exploration, but whatever. Let me see how bad that is.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    return "*[nickname], I need sleep to regain energy for another wave of exercises. Can we speak another time?*";
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    return "*It's quite late right now, [nickname]. Did you do my request?*";
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*Yes. While you travel, I can oversee your exercises.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*There's a lot of people with you already. I can't seem to fit in that.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*No. I have my own things to do right now.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*I don't mind walking the way back to home, but the monsters are what worries me. Are you sure that you want to ditch me here?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*I will stay then. Try not to overdo yourself on your exercises, [nickname].*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*Well, time to see how good I am at running.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Then let's find a place with a friendly person before I leave.*";
                case MessageIDs.RequestAccepted:
                    return "*Thank you, [nickname]. Tell me when you got the request done.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*By overdoing yourself, I also talk about not stressing yourself with many tasks to do at once. Solve your other friends requests first before taking mine. Health is a serious thing.*";
                case MessageIDs.RequestRejected:
                    return "*I see... When I get a free time, I will see to it myself. You're relieved, [nickname].*";
                case MessageIDs.RequestPostpone:
                    return "*I guess it can wait.*";
                case MessageIDs.RequestFailed:
                    return "*I'm disappointed, [nickname], but I wont punish you with exercises for that.*";
                case MessageIDs.RequestAsksIfCompleted:
                    return "*Tell me, [nickname]. You did my request?*";
                case MessageIDs.RequestRemindObjective:
                    return "*Short fused memory? I asked you to [objective]. Maybe I can recommend you some vitamines for memory.*";
                case MessageIDs.RestAskForHowLong:
                    return "*Battery running low? Alright, let's rest then. For how long?*";
                case MessageIDs.RestNotPossible:
                    return "*This isn't the best moment for a break.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*Alright. As you rest, I'll try doing some exercises until I get really tired for sleep.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Hm, what [shop] is offering looks interesting. Let's take a closer look.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Just a minute, I'm checking this...*";
                case MessageIDs.GenericYes:
                    return "*Uh huh.*";
                case MessageIDs.GenericNo:
                    return "*Nah.*";
                case MessageIDs.GenericThankYou:
                    return "*Thank you.*";
                case MessageIDs.ChatAboutSomething:
                    return "*Want to know about something? Feel free to ask.*";
                case MessageIDs.NevermindTheChatting:
                    return "*Then, what else you want to talk about?*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*Do you really want to cancel my request?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*I'm disappointed at you, [nickname]. If you couldn't do it, why you accepted?*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Good. Return to me if you manage to fulfill it.*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    return "*Thank you for that, I really mean it.*";
                case MessageIDs.RevivedByRecovery:
                    return "*Ouch, ouch... I'm back... I'm back..*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*My body... It burns...*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Ahh!! Fire!! It's burning!!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*My eyes!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*My head feels... Strange...*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*I really want to defend myself, but can't.*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*My legs are a bit lethargic.*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*I don't feel 100%...*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Ack! My chest.*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*What IS that thing?!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*Vile creature...*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*S-someone has a c-coat?*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*I'm stuck!!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*You will regret biting me!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*Try hiting my chest!*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*Aaaaaaaahh.... Now I can do 100 push ups.*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*Even my arms muscle look a bit bigger.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*Come on! Race me!*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*I feel more alive now.*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*I have pity of who my attacks hit, now. Nah, just kidding.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*Just to ensure victory.*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*As long as it's moderate, it's fine.*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*Hmm.... This is the sweetest part of the adventure.*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*Look! A Life Crystal over there!*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Watch your step! Pressure Plate.*";
                case MessageIDs.FoundMineTile:
                    return "*Mine! Watch it!*";
                case MessageIDs.FoundDetonatorTile:
                    return "*[nickname], please don't.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*Breaking that seems like a bad idea.*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*Why do we even defend that crystal for?*";
                case MessageIDs.FoundTreasureTile:
                    return "*Loot! Sweet loot! Over there!*";
                case MessageIDs.FoundGemTile:
                    return "*Those gems would look nice on my wife.*";
                case MessageIDs.FoundRareOreTile:
                    return "*You may find this interesting.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*[nickname], here some rare ores.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*You think I don't like rollercoaster, [nickname]? Let's go.*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Enough of this.*";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "*Oh, hello.*";
                case MessageIDs.PlayerMeetsSomeoneNewMessage:
                    return "*You're getting more popular, [nickname].*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*I wish they could help me on my exercises.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*That just got weird.*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*[nickname]! Stay down a while, I'll help you.*";
                case MessageIDs.LeaderDiesMessage:
                    return "*[nickname]!! No!!*";
                case MessageIDs.AllyFallsMessage:
                    return "*Someone fell! Let's aid them!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*That looks rare.*";
                case MessageIDs.LeavingToSellLoot:
                    return "*I'll be right back, my bag is full.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*Watch yourself, [nickname]. Your face tells me that you aren't fine.*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*No... I wont... I'm still okay...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*I have few potions with me. Someone has extras?*";
                case MessageIDs.UsesLastPotion:
                    return "*I'm out of potions!*";
                case MessageIDs.SpottedABoss:
                    return "*We are going to fight that, right? Sigh... Fine...*";
                case MessageIDs.DefeatedABoss:
                    return "*Is everyone okay? Good to know.*";
                case MessageIDs.InvasionBegins:
                    return "*I thought this place was peaceful.*";
                case MessageIDs.RepelledInvasion:
                    return "*They're gone, but thinking about them returning is a bit depressing.*";
                case MessageIDs.EventBegins:
                    return "*That doesn't look good...*";
                case MessageIDs.EventEnds:
                    return "*We managed to survive that. I'm glad that it's over.*";
                case MessageIDs.RescueComingMessage:
                    return "*Oh no! I'm coming!*";
                case MessageIDs.RescueGotMessage:
                    return "*Alright, got you. Now I need to keep you safe from more harm.*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*[player] has been receiving some training from me, too.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*I heard that [player] defeated [subject] not too long ago.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*It seems like [player] found a [subject] during their travels.*";
                case MessageIDs.FeatEventFinished:
                    return "*I heard that [player] took care of a [subject] that happened on their world.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*[player] has met [subject] the other day.*";
                case MessageIDs.FeatPlayerDied:
                    return "*One of the people I trained has died recently. Their name was [player]. I'm gonna miss that person.*";
                case MessageIDs.FeatOpenTemple:
                    return "*It seems that [player] opened a temple door in [subject]. What kind of marvels could be inside? Or dangers?*";
                case MessageIDs.FeatCoinPortal:
                    return "*A coin portal appeared in front of [player] the other day.*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I've met a new person who also needs to have their muscles be awaken. Their name was [player]. If you see them, try helping them on their exercises.*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*Did you heard that [player] has catched many weird fish recently? That wouldn't be possible if he wasn't training his arms, beside fishing also does that somehow.*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*[player] has defeated some godly creature in [subject]. Of course, that wouldn't be possible if they didn't trained their muscles and got stronger. Continue doing your exercises and someday you may do such a feat too.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*It was so strange. I had a weird feeling when I was in [subject], after that, I heard that the creatures got scarier and tougher. I hope [player] knows what they're doing.*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*I heard about [player] picking [subject] as their buddy. I hope they help each other on their daily exercises.*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*Hey buddy, seeking your next exercise? Just because I'm your buddy now doesn't mean you're free to skip exercises. Self health care is also important.*";
            }
            return base.GetSpecialMessage(MessageID);
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3)) {
                default:
                    return "*I need a place to stay. You can wonder that the wilderness creatures probably find me succulent.*";
                case 1:
                    return "*It's hard to do exercises while fighting for my life.*";
                case 2:
                    return "*If you're going to have me around, could at least give me a house.*";
            }
        }

        #endregion

        public override List<DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian guardian)
        {
            List<DialogueOption> Dialogue = base.GetGuardianExtraDialogueActions(guardian);
            if (!PlayerHasExercise())
            {
                Dialogue.Add(new DialogueOption("Do you have any exercise I can do?", GiveExerciseButtonAction));
            }
            else
            {
                PlayerMod pm = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                string DialogueText = "What is my exercise progress?";
                if (pm.CurrentExercise == ExerciseTypes.WaitUntilNextDay)
                    DialogueText = "Do you have any other exercise for me?";
                else if (pm.ExerciseCounter <= 0)
                    DialogueText = "I have completed the exercise.";
                Dialogue.Add(new DialogueOption(DialogueText, CheckExerciseButtonAction));
            }
            return Dialogue;
        }

        private void GiveExerciseButtonAction()
        {
            TerraGuardian tg = Dialogue.GetSpeaker;
            PlayerMod pm = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            pm.CurrentExercise = (ExerciseTypes)Main.rand.Next(1, (int)ExerciseTypes.Count);
            switch (pm.CurrentExercise)
            {
                case ExerciseTypes.AttackTimes:
                    pm.ExerciseCounter = Main.rand.Next(2, 8) * 20;
                    Dialogue.ShowEndDialogueMessage("*Yes, I do. Today's exercise may interest you, since will use something you Terrarians loves doing. I want you to attack anything "+(int)pm.ExerciseCounter+" times. Once you do that, come back to me.*", false);
                    break;
                case ExerciseTypes.JumpTimes:
                    pm.ExerciseCounter = Main.rand.Next(2, 7) * 5;
                    Dialogue.ShowEndDialogueMessage("*This time I want to see you jumping like popcorn. Jump "+(int)pm.ExerciseCounter+" times and then come talk to me.*", false);
                    break;
                case ExerciseTypes.TravelDistance:
                    pm.ExerciseCounter = Main.rand.Next(2, 6) * 1500;
                    Dialogue.ShowEndDialogueMessage("*Time to exercise your legs. You need to walk "+(int)(pm.ExerciseCounter * 0.5f)+" feets and then talk to me.*", false);
                    break;
            }
        }

        private void CheckExerciseButtonAction()
        {
            TerraGuardian tg = Dialogue.GetSpeaker;
            PlayerMod pm = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            if(pm.CurrentExercise == ExerciseTypes.WaitUntilNextDay)
            {
                Dialogue.ShowEndDialogueMessage("*There is no other exercise for you today. For your muscles to recover from today's exercise, wait until tomorrow.*", false);
            }
            else if (pm.ExerciseCounter <= 0)
            {
                int FitBuffID = Terraria.ModLoader.ModContent.BuffType<Buffs.Fit>();
                int NewBuffTime = 30 * 60 * 60;
                if (pm.player.HasBuff(FitBuffID))
                {
                    for(int b = 0; b < pm.player.buffType.Length; b++)
                    {
                        if(pm.player.buffType[b] == FitBuffID)
                        {
                            NewBuffTime += pm.player.buffTime[b];
                            if (NewBuffTime > 60 * 60 * 60)
                                NewBuffTime = 60 * 60 * 60;
                            break;
                        }
                    }
                }
                pm.player.AddBuff(FitBuffID, NewBuffTime);
                pm.ExercisesDone++;
                if (pm.ExercisesDone >= 10)
                {
                    pm.ExercisesDone = 0;
                    tg.IncreaseFriendshipProgress(1);
                    Dialogue.ShowEndDialogueMessage("*Good job, [nickname]. You have really impressed me those days. Let your muscles take a rest until tomorrow and then I will give you another exercise.*");
                }
                else
                {
                    Dialogue.ShowEndDialogueMessage("*Good job, [nickname]. Now take a rest and return to me tomorrow for another exercise.*");
                }
                pm.CurrentExercise = ExerciseTypes.WaitUntilNextDay;
            }
            else
            {
                switch (pm.CurrentExercise)
                {
                    case ExerciseTypes.AttackTimes:
                        Dialogue.ShowEndDialogueMessage("*I tasked you into attacking anything a number of times. It seems like you still need to hit anything "+(int)pm.ExerciseCounter+" times. I'm sure you know how you will do that.*", false);
                        break;
                    case ExerciseTypes.JumpTimes:
                        Dialogue.ShowEndDialogueMessage("*I told you to jump a number of times. You still need to jump "+(int)pm.ExerciseCounter+" more times to complete this exercise.*", false);
                        break;
                    case ExerciseTypes.TravelDistance:
                        Dialogue.ShowEndDialogueMessage("*You need to travel "+(int)(pm.ExerciseCounter * 0.5f)+" feets more to complete this exercise.*", false);
                        break;
                }
            }
        }

        private bool PlayerHasExercise()
        {
            return Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().CurrentExercise != ExerciseTypes.None;
        }

        public static void RefreshExercisesOfAllPlayers()
        {
            for(int i = 0; i < 255; i++)
            {
                if(Main.player[i].active)
                {
                    PlayerMod pm = Main.player[i].GetModPlayer<PlayerMod>();
                    if (pm.CurrentExercise == ExerciseTypes.WaitUntilNextDay)
                        pm.CurrentExercise = ExerciseTypes.None;
                }
            }
        }

        public enum ExerciseTypes : byte
        {
            None,
            JumpTimes,
            TravelDistance,
            AttackTimes,
            Count,
            WaitUntilNextDay = 255 //So whenever a new exercise is added, It doesn't counts as Count.
        }
    }
}

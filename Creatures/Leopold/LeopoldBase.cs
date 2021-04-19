using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Creatures
{
    public class LeopoldBase : GuardianBase
    {
        /// <summary>
        /// -Coward
        /// -Knows of many things
        /// -Brags about knowing many things
        /// -Female Terrarians keep touching his tail.
        /// -Spends most of his day reading books.
        /// -Rarelly leaves house, unless for a research.
        /// -Fears Malisha.
        /// </summary>
        public LeopoldBase()
        {
            Name = "Leopold";
            Description = "A sage from the Ether Realm.";
            Size = GuardianSize.Medium;
            Width = 22;
            Height = 58;
            DuckingHeight = 52;
            SpriteWidth = 64;
            SpriteHeight = 64;
            Age = 23;
            SetBirthday(SEASON_SUMMER, 6);
            Male = true;
            InitialMHP = 160; //640
            LifeCrystalHPBonus = 12;
            LifeFruitHPBonus = 15;
            InitialMP = 40;
            ManaCrystalMPBonus = 30;
            Accuracy = 0.86f;
            Mass = 0.4f;
            MaxSpeed = 5.2f;
            Acceleration = 0.18f;
            SlowDown = 0.47f;
            MaxJumpHeight = 17;
            JumpSpeed = 7.66f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = true;
            SetTerraGuardian();
            MountUnlockLevel = 7;
            CallUnlockLevel = 5;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 1;

            AddInitialItem(Terraria.ID.ItemID.WaterBolt, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 5);
            AddInitialItem(Terraria.ID.ItemID.ManaPotion, 20);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            PlayerMountedArmAnimation = JumpFrame = 9;
            HeavySwingFrames = new int[] { 10, 22, 23 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 14;
            ChairSittingFrame = 24;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            ThroneSittingFrame = 15;
            BedSleepingFrame = 16;
            ReviveFrame = 20;
            DownedFrame = 21;

            SittingPoint = new Point(14 * 2, 23 * 2);

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);
            
            //Left Hand
            LeftHandPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(14 * 2, 20 * 2);
            LeftHandPoints.AddFramePoint2x(10, 15, 2);
            LeftHandPoints.AddFramePoint2x(11, 24, 7);
            LeftHandPoints.AddFramePoint2x(12, 24, 14);
            LeftHandPoints.AddFramePoint2x(13, 23, 21);

            LeftHandPoints.AddFramePoint2x(20, 19, 23);

            LeftHandPoints.AddFramePoint2x(22, 26, 13);
            LeftHandPoints.AddFramePoint2x(23, 23, 25);

            //Right Hand
            RightHandPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(21 * 2, 20 * 2);
            RightHandPoints.AddFramePoint2x(10, 18, 2);
            RightHandPoints.AddFramePoint2x(11, 28, 7);
            RightHandPoints.AddFramePoint2x(12, 28, 14);
            RightHandPoints.AddFramePoint2x(13, 26, 21);

            RightHandPoints.AddFramePoint2x(20, 26, 23);

            RightHandPoints.AddFramePoint2x(22, 28, 13);
            RightHandPoints.AddFramePoint2x(23, 25, 25);

            //Mount Position
            MountShoulderPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(12 * 2, 9 * 2);

            //Head Vanity Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(14, 9);

            HeadVanityPosition.AddFramePoint2x(20, 22, 18);
            HeadVanityPosition.AddFramePoint2x(22, 18, 12);
            HeadVanityPosition.AddFramePoint2x(23, 21, 17);

            //Wing Position
            WingPosition.DefaultCoordinate2x = new Point(16, 17);
        }

        public override string CallUnlockMessage
        {
            get
            {
                return "*I've read so many books about adventures. You are an adventurer, right? I'd like to see with my own eyes how is it.*";
            }
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "*Can you hop? You can move faster that way. Wait, you can't? Then sit on my shoulder and see how I do that.*";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "*I'd like to have my own adventure, but I don't really have the initiative to do so, what if you help me?*";
            }
        }

        public override void Attributes(TerraGuardian g)
        {
            g.MagicDamageMultiplier += 0.15f;
            g.MagicCriticalRate += 10;
            g.DefenseRate -= 0.07f;
            g.AddFlag(GuardianFlags.AllowHopping);
        }

        public override void GuardianAnimationScript(TerraGuardian guardian, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            if (guardian.Velocity.Y != 0 && guardian.WallSlideStyle == 0 && guardian.DashCooldown <= 0 && !guardian.SittingOnPlayerMount && !guardian.KnockedOut && !guardian.Downed && guardian.BodyAnimationFrame != HeavySwingFrames[1] && guardian.BodyAnimationFrame != HeavySwingFrames[2] && (!guardian.Wet || !guardian.HasFlag(GuardianFlags.SwimmingAbility)))
            {
                if (guardian.Velocity.Y < -1.5f)
                {
                    guardian.BodyAnimationFrame = 17;
                }
                else if (guardian.Velocity.Y < 1.5f)
                {
                    guardian.BodyAnimationFrame = 18;
                }
                else
                {
                    guardian.BodyAnimationFrame = 19;
                }
                if (!UsingLeftArm)
                    guardian.LeftArmAnimationFrame = guardian.BodyAnimationFrame;
                if (!UsingRightArm)
                    guardian.RightArmAnimationFrame = guardian.BodyAnimationFrame;
            }
        }

        public override void GuardianBehaviorModScript(TerraGuardian guardian)
        {
            if (guardian.DashCooldown <= 0 && (guardian.MoveLeft || guardian.MoveRight) && guardian.Velocity.X != 0 && guardian.Velocity.Y == 0 && !guardian.KnockedOut && !guardian.Downed)
            {
                guardian.Velocity.Y = -5.6f;
            }
        }

        public override void GuardianHorizontalMovementScript(TerraGuardian guardian, ref float MaxSpeed, ref float Acceleration, ref float SlowDown, ref float DashSpeed)
        {
            if (guardian.Velocity.Y != 0)
            {
                MaxSpeed *= 1.6f;
                Acceleration *= 1.6f;
                SlowDown *= 1.6f;
                DashSpeed *= 1.6f;
            }
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Huh? Ah!! Oh... You're friendly. Sorry, I came here because I noticed several TerraGuardians were coming here.*");
            Mes.Add("*Are you... A Terrarian...? I read about your kind on the books in my old Workshop. "+(player.GetModPlayer<PlayerMod>().GetAllGuardianFollowers.Any(x => x.Active) ? "Hey, those are TerraGuardians! What are you doing here?" : "Say, have you seen any TerraGuardians around?") +"*");
            Mes.Add("*Yikes!! You scared me! I've never seen a Terrarian before. Wait, you can hear me? Then It's true... Oh... I'm thinking loud again. Do you have some place I can stay?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (player.GetModPlayer<PlayerMod>().TalkedToLeopoldAboutThePigs)
            {
                Mes.Add("*I can try changing the forms of the pig emotions between Cloud and Solid, but I will need all emotions to merge them together.*");
                Mes.Add("*There is not exactly a penalty for them being in cloud form, but they may not like It.*");
            }
            Mes.Add("*So, this is where the TerraGuardians have been moving to? I can see why.*");
            Mes.Add("*How many times I have to say that my tail is not made of cotton! Stop trying to touch it!*");
            Mes.Add("*I spend most of my time reading books, so I know of many things.*");
            if (!player.Male)
                Mes.Add("*Hey! Get your hand off my tail! It's not made of cotton!*");
            if (!NPC.downedBoss3)
                Mes.Add("*The dungeon in your world contains a great amount of readable material I could make use of. But the Old Man wont allow me in.*");
            else
            {
                Mes.Add("*So much knowledge found inside the dungeon. From dark secrets to sci-fi literature.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Bree))
            {
                Mes.Add("*What did I had on my mind during the popularity contest, when I agreed with Bree when she was clearly wrong?*");
            }
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    Mes.Add("*Too bright! I think I stayed inside home for too long.*");
                }
                else
                {
                    Mes.Add("*Want some popcorn?*");
                    Mes.Add("*I think I read about them in a book. Had something to do with the creators.*");
                }
            }
            else
            {
                Mes.Add("*What? It was day a while ago!*");
                if (Main.bloodMoon)
                {
                    Mes.Add("*Interesting, so whenever the red moon rises, monsters gets aggressive and smarter? And what is wrong with the female citizens?*");
                    Mes.Add("*I'm not scared! And no, the screams didn't came from here!*");
                }
                else
                {
                    Mes.Add("*This seems like a good night for some reading.*");
                    Mes.Add("*I tried staring at the moon, but then I got bored. I preffer my books.*");
                }
            }
            if (NPC.TowerActiveNebula || NPC.TowerActiveSolar || NPC.TowerActiveStardust || NPC.TowerActiveVortex)
            {
                Mes.Add("*I feel... Like something bad is about to happen...*");
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("*This place is being too noisy, I can't concentrate on my books.*");
                Mes.Add("*Could you guys PLEASE SHUT UP!! Oh! I didn't see you there. Need something?*");
                Mes.Add("*Ugh... The headache comes... Couldn't they party somewhere else?*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Blue))
            {
                Mes.Add("*I keep avoiding crossing [gn:" + GuardianBase.Blue + "]'s path, because everytime she sees me, I pass the next 1~2 hours trying to get off her arms.*");
                Mes.Add("*What is [gn:" + GuardianBase.Blue + "]'s problem? Whenever she sees a bunny she wants to hug it.*");
                Mes.Add("*You wont believe me, but [gn:" + GuardianBase.Blue + "] has really strong arms. I have to struggle for hours to be free from them.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Zacks))
            {
                Mes.Add("*I try my best to avoid crossing [gn:" + GuardianBase.Zacks + "]'s path, for my own safety.*");
                Mes.Add("*Creepy! [gn:" + GuardianBase.Zacks + "]'s mouth starts salivating when he sees me!*");
                if (PlayerMod.PlayerHasGuardianSummoned(player, GuardianBase.Brutus))
                {
                    Mes.Add("*Would you mind If I borrow [gn:"+GuardianBase.Brutus+"] for a while? I'm still not sure If I'm safe with [gn:"+GuardianBase.Zacks+"] around.*");
                }
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, GuardianBase.Blue))
            {
                Mes.Add("*Uh!? No way! Don't hug me again!*");
                Mes.Add("*You again?! Keep your arms off me!*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, GuardianBase.Zacks))
            {
                Mes.Add("Great, you came, I wanted to talk to you about some reseaAAAAAAAAHHHHH Zombie!! \n....\nUh... Do you have some more leaves..?");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, GuardianBase.Mabel))
            {
                Mes.Add("*A...Aa.... Uh.... Could... You please... Go away... With.. Her...*");
                Mes.Add("*Ah... Uh... I.... Have to... Go... To the toilet... Yes. The Toilet...*");
            }
            else if (NpcMod.HasGuardianNPC(GuardianBase.Mabel))
            {
                Mes.Add("*I get reactionless when [gn:" + GuardianBase.Mabel + "] is nearby.*");
            }
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("*D-d-did y-you l-let a g-g-ghost l-live here? A-are you out of your mind?*");
                Mes.Add("*[nickname], one of the things that mostly scares me are ghosts. Why did you let one live here?*");
                Mes.Add("*I think [gn:" + Fluffles + "] knows I'm scared of her. She always catches me off guard to spook me out.*");
                Mes.Add("*I look distacted? I'm checking out if [gn:" + Fluffles + "] wont surge from somewhere to give me a scare.*");
                //Mes.Add("*You need to speak with [gn:" + Fluffles + "]. The other day she made me faint out of a scare, when I woke up she was over me. I've never been so scared in my life!*"); //That would give a bad impression of what happened.
            }
            if (!Main.dayTime && !Main.bloodMoon)
            {
                Mes.Add("*Looks like I'll have troubles sleeping this night...*");
            }
            if (NpcMod.HasGuardianNPC(Michelle))
            {
                Mes.Add("*I discovered a way of dealing with [gn:" + Michelle + "], I just need to talk about my researches.*");
                Mes.Add("*That girl [gn:"+Michelle+"] surelly is curious. I wonder, what's wrong with her?*");
            }
            if (NpcMod.HasGuardianNPC(Malisha))
            {
                Mes.Add("*Of everyone you could have let move into this world, you had to let [gn:"+Malisha+"] live here? She's a menace to us all!*");
                Mes.Add("*During my life of mentoring, I never regretted teaching someone, except for [gn:"+Malisha+"]. She's not careless, or cares about the wellbeing of the people she does experiements to.*");
                Mes.Add("*Sometimes I think [gn:" + Malisha + "] is trying to eat me. If I suddenly disappear, try looking inside her mouth.*");
                Mes.Add("*Huh? Sorry, I'm trying to make my house [gn:"+Malisha+"] proof.*");
                Mes.Add("*My greatest misfortunes in life begins when [gn:"+Malisha+"] says that has a new experiement to test.*");
                if (NpcMod.HasGuardianNPC(Zacks))
                {
                    Mes.Add("*I wouldn't be surprised if what happeend to [gn:"+Zacks+"] wasn't [gn:"+Malisha+"]s doing.*");
                }
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*What's with you Terrarians? Don't you know that this is a moment of privacy?*");
                if (!player.Male)
                    Mes.Add("*No! Don't try touching my tail now!*");
                Mes.Add("*You're interrupting my reflection moment! Go away!*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*Yes, I don't mind sharing my room with you, just place your bed somewhere.*");
                Mes.Add("*I hope you don't snore at night, because I need sleep to process my researches.*");
                Mes.Add("*You're not planning on throwing parties every night, I've had enough of those during the Magic University.*");
            }
            if (NpcMod.IsGuardianPlayerRoomMate(player, Malisha))
            {
                Mes.Add("*You must either be courageous, or very stupid, for sharing your room with [gn:"+Malisha+"]. Who knows what she does to you while you sleep?*");
                Mes.Add("*Stop sharing room with [gn:"+Malisha+"], you may wake up tied in a bed while she does wacky experiements on you.*");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                Mes.Add("(" + guardian.Name + " looks at you, wanting you to heal his wounds)");
                Mes.Add("(He seems to be under heavy pain.)");
                Mes.Add("(He's unnable to speak with you because of how much pain he's having.)");
            }
            else if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("(He's theorizing to himself, while sleeping.)");
                Mes.Add("(Is he saying elements of the periodic table?)");
                if (NpcMod.HasGuardianNPC(Malisha))
                {
                    Mes.Add("*No! Go away! Don't come closer! No! NOOOOOOOOOO!!!! (I think [gn:" + Malisha + "] appeared on his dream)*");
                    Mes.Add("*Wait! What is in that flask! No! I wont drink it! NO! NOOOO!! (I think [gn:" + Malisha + "] appeared on his dream)*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*This place doesn't have enough spoony bards.*");
            Mes.Add("*I see far away places blurry, I think I read too much.*");
            Mes.Add("*Why people keep blaming me when they see several bunnies around?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*What? I don't need anything.*");
            Mes.Add("*Oh no, I don't need your help right now.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm filled with things to research, so I barelly have the time to do this. Can you help me with It?*");
            Mes.Add("*There is something that needs to be done for one of my researchs. Would you please do it?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*That's great! You helped me a lot.*");
            Mes.Add("*I was really looking forward that. Here some material affection.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It is hard to do researches without some place to stay.*");
            Mes.Add("*Can we discuss once I have a place to stay?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*As the age increases, wisdom rises.*");
            Mes.Add("*I'm not really used to party, I'm more the quiet type, you know.*");
            if (!PlayerMod.HasGuardianBeenGifted(player, guardian.ID, guardian.ModID))
            {
                Mes.Add("*You have a gift for me? I wonder what is it. Is it a book?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I know some healing magic, this will help you.*");
            Mes.Add("*I've read several medicine books. Don't worry, I know what I'm doing.*");
            Mes.Add("*I hope I don't need to open you up to try fixing a problem.*");
            if (!IsPlayer && ReviveGuardian.ModID == Guardian.ModID)
            {
                if (ReviveGuardian.ID == GuardianBase.Blue)
                {
                    Mes.Add("*I should help her carefully away, I don't want to be stuck in her arms for hours again.*");
                }
                if (ReviveGuardian.ID == GuardianBase.Vladimir)
                {
                    Mes.Add("*There is still a lot you can help me with.*");
                }
                if (ReviveGuardian.ID == GuardianBase.Zacks)
                {
                    Mes.Add("*Come on "+Guardian.Name+", control your intestine. You need to help him, no time for... Leaf, please.*");
                }
                if (ReviveGuardian.ID == GuardianBase.Mabel)
                {
                    Mes.Add("*Oh no... My nose... Someone has a leaf I could use?*");
                }
                if (ReviveGuardian.ID == GuardianBase.Malisha)
                {
                    Mes.Add("*Ugh... I really don't want to... But... I'll help...*");
                    Mes.Add("*I hope she stops tormenting me after this.*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override List<GuardianMouseOverAndDialogueInterface.DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian guardian)
        {
            List<GuardianMouseOverAndDialogueInterface.DialogueOption> Actions = base.GetGuardianExtraDialogueActions(guardian);
            if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TalkedToLeopoldAboutThePigs)
            {
                if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Wrath))
                {
                    string Text = "";
                    if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.AngerPigGuardianID])
                        Text = "Solidify " + PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianBase.Wrath).Name + "'s Body";
                    else
                        Text = "Cloudify " + PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianBase.Wrath).Name + "'s Body";
                    GuardianMouseOverAndDialogueInterface.DialogueOption action = new GuardianMouseOverAndDialogueInterface.DialogueOption(Text, delegate(TerraGuardian tg)
                    {
                        Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.AngerPigGuardianID] =
                            !Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.AngerPigGuardianID];
                        if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.AngerPigGuardianID])
                        {
                            GuardianMouseOverAndDialogueInterface.SetDialogue("*I've managed to revert his form back to Cloud Form. I'm not sure if he's happy about that.*");
                            PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], Wrath).SaySomething("Grrr. I hate this! I hate It!");
                        }
                        else
                        {
                            GuardianMouseOverAndDialogueInterface.SetDialogue("*He's now in flesh and bones now. Beware not to infuriate him on that state, who knows what his rage could do to you.*");
                            PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], Wrath).SaySomething("Amazing, now I can really hurt things.");
                        }
                        GuardianMouseOverAndDialogueInterface.GetDefaultOptions(tg);
                    });
                    Actions.Add(action);
                }
            }
            return Actions;
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "*Perfect, I'll try some healing spells.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    if (Main.rand.Next(3) == 0 && MainMod.IsGuardianInTheWorld(Malisha))
                    {
                        return "*No! Please! Don't... Oh... You're not [gn:"+Malisha+"]. Whew..*";
                    }
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*I can't process what I learned from the books without sleeping, [nickname].*";
                        case 1:
                            return "*Huh? I was reading this book until I fell asleep. Well... I can try finishing reading It later.*";
                        case 2:
                            return "*Waah! You scared me, I was really having a not so pleasant dream. Need something from me?*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Tell me you completed my request, because I can't think of any other reason you would wake me up.*";
                        case 1:
                            return "*Uh? Oh, I fell asleep. By the way, did you do the task I gave you?*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*It seems like a great idea, wonder how many new things I may end up meeting during your trip.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*There's too many people around you.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*I still have some books to read.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*Are you crazy?! There are very dangerous creatures on the way back home!*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*I have to research on my books about the creatures we found on the way.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*You are kidding, right? You aren't?! Looks like I'll have to f-fight my way back home then...*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Whew... You got me worries for a while.*";
                case MessageIDs.RequestAccepted:
                    return "*Great. Now, what was I... Oh... Yeah.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*I'm used to multi tasking, I don't think you are too.*";
                case MessageIDs.RequestRejected:
                    return "*Maybe I need to do that myself, then.*";
                case MessageIDs.RequestPostpone:
                    return "*Huh? Oh.. Fine.*";
                case MessageIDs.RequestFailed:
                    return "*I'll... Try adding that to my research notes. Thanks for the cooperation.*";
                case MessageIDs.RestAskForHowLong:
                    return "*I like the idea of staying for a while, I can read some books while I don't get sleepy. How long are we going to stay out of adventures?*";
                case MessageIDs.RestNotPossible:
                    return "*Are you crazy? Not now!*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*Try not to snore, that will ruin my concentration.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Wait [nickname], let's check out [shop]'s shop.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Just a minute, let me try getting something...*";
                case MessageIDs.GenericYes:
                    return "*That looks good.*";
                case MessageIDs.GenericNo:
                    return "*I don't find that good.*";
                case MessageIDs.GenericThankYou:
                    return "*Thank you! That will help.*";
                case MessageIDs.ChatAboutSomething:
                    return "*We may chat about anything, but I'm not in the mood for a lecture, right now.*";
                case MessageIDs.NevermindTheChatting:
                    return "*So, want something else from me?*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*You can't drop my request, the future about the Terra Realms research is in your hands. Are you really going to cancel my request?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Sigh.. Okay. But be sure not to drop my request in the future.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*I knew I could count on you, [nickname].*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon.Npcs
{
    public class BreeNPC : GuardianActorNPC
    {
        public SceneIds SceneStep = SceneIds.NoScene;
        public int StepTime = 0, SceneRepeatTimes = 0;
        public bool InterruptedOnce = false;
        public bool DialogueResultSuccess { get { return dialogues.Points >= 4; } }
        public bool EndDialogDone = false;
        public DialogueChain dialogues = new DialogueChain();
        private bool JustSpawned = true;
        private bool PlayerHasMetSardine = false, PlayerHasMetGlenn = false;
        private bool HadSardineSpotted = false, HadGlennSpotted = false;

        public BreeNPC()
            : base(7, "")
        {
            dialogues.AddDialogue("Hello, have you seen a black cat around?", "Why are you looking for a black cat?", "No, I haven't.", true);
            dialogues.AddDialogue("The black cat is my husband, he's been missing for quite some time.", "I need to find that black cat, I looked everywhere and couldn't find him.", "I can help you find him.", "Have you looked on the other towns?", true);
            dialogues.AddDialogue("Thank you, but I'm not sure if he may be found in this world, but I really need all the help I can find to locate him.", "I didn't yet, this place is huge, and I've been travelling through several worlds. I'm kind of tired.", "Any tips on how I can find him?", "What about forgetting him?", true);
            dialogues.AddDialogue("He's an adventurer, so probably he's doing something dangerous or stupid. That also makes me worried, what If he's dead? No, wait, I shouldn't be pessimistic.", "What?! No! I can't do that! I really need to find him. If the worst didn't happened to him.", "Don't give up.", "Maybe the worst has happened.", true);
            dialogues.AddDialogue("You're right, I have to keep looking. But I'm getting worn out after so much travelling.", "You are awful! But maybe you are right? I don't know, all I know is that I'm tired.", "Take a rest before you search for him.", "Why don't you stay here?", true);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("White Cat");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = false;
            npc.rarity = 1;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
            npc.dontCountMe = true;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            if (false && SceneStep == SceneIds.BreePersuadesThePlayerALittleCloser)
            {
                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.SittingFrame;
            }
        }

        public override void AI()
        {
            if (Main.myPlayer == -1)
                return;
            Player player = Main.player[npc.target];
            if (JustSpawned)
            {
                //if (IsInPerceptionRange(player))
                int NearestPlayer = Main.myPlayer;
                float NearestDistance = float.MaxValue;
                Vector2 NpcCenter = npc.Center;
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active)
                    {
                        float Distance = Main.player[p].Distance(NpcCenter);
                        if (Distance < NearestDistance)
                        {
                            NearestDistance = Distance;
                            NearestPlayer = p;
                        }
                    }
                }
                if(NearestPlayer > -1)
                {
                    player = Main.player[NearestPlayer];
                    npc.target = NearestPlayer;
                    Main.NewText("A White Cat appeared to the " + GuardianBountyQuest.GetDirectionText(npc.Center - player.Center) + " of " + player.name + " position.");
                }
                JustSpawned = false;
            }
            if (player.talkNPC == npc.whoAmI)
                npc.direction = player.Center.X - npc.Center.X >= 0 ? 1 : -1;
            if (SceneStep == SceneIds.NoScene && Math.Abs(player.Center.X - npc.Center.X) <= 152 &&
                ((player.Center.X < npc.Center.X && npc.direction < 0) || (player.Center.X > npc.Center.X && npc.direction >= 0)))
            {
                //Found Sardine
                if (PlayerMod.PlayerHasGuardian(player, 7))
                {
                    ChangeScene(SceneIds.BreeSeesFriendlyFace);
                }
                else if (PlayerMod.PlayerHasGuardianSummoned(player, GuardianBase.Glenn) && PlayerMod.PlayerHasGuardianSummoned(player, GuardianBase.Sardine))
                {
                    ChangeScene(SceneIds.BreeSpotsSardineAndGlenn);
                    SayMessage("Are they... Glenn! Sardine!");
                    HadSardineSpotted = HadGlennSpotted = true;
                }
                else if (PlayerMod.PlayerHasGuardianSummoned(player, GuardianBase.Glenn))
                {
                    ChangeScene(SceneIds.GlennSpotted);
                    SayMessage("Gl... Glenn?!");
                    HadSardineSpotted = false;
                    HadGlennSpotted = true;
                }
                else if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
                {
                    ChangeScene(0);
                    SayMessage("Sardine! I finally found you!");
                    HadSardineSpotted = true;
                    HadGlennSpotted = false;
                }
            }
            npc.npcSlots = 1;
            if (SceneStep >= 0)
            {
                npc.npcSlots = 200;
                StepTime--;
                bool SceneChange = StepTime == 0;
                if (false && SceneStep == SceneIds.BreePersuadesThePlayerALittleCloser)
                {
                    Vector2 PositionOnPlayer = player.Center;
                    npc.direction = -player.direction;
                    PositionOnPlayer.X += 12 * player.direction;
                    PositionOnPlayer.Y += -Base.SpriteHeight + Base.SittingPoint.Y;
                    int XMod = Base.SittingPoint.X;
                    if (npc.direction < 0)
                        XMod = Base.SpriteWidth - XMod;
                    PositionOnPlayer.X += XMod - Base.SpriteWidth * 0.5f;
                    npc.position = PositionOnPlayer;
                }
                TerraGuardian guardian = PlayerMod.GetPlayerMainGuardian(player),
                    Sardine = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine),
                    Glenn = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Glenn);
                if (HadGlennSpotted && HadSardineSpotted)
                {
                    if(Sardine != null && Glenn != null)
                    {
                        bool GlennIsClosest = (Glenn.CenterPosition - player.Center).Length() < (Sardine.CenterPosition - player.Center).Length();
                        if (GlennIsClosest)
                        {
                            guardian = Glenn;
                            bool IsGlennToTheLeft = guardian.Position.X - npc.Center.X < 0;
                            if (Math.Abs(guardian.Position.X - npc.Center.X) >= 96)
                            {
                                if (!IsGlennToTheLeft)
                                {
                                    MoveRight = true;
                                }
                                else
                                {
                                    MoveLeft = true;
                                }
                            }
                            if (!guardian.IsAttackingSomething && guardian.Velocity.X == 0)
                            {
                                guardian.FaceDirection(!IsGlennToTheLeft);
                            }
                            if (npc.velocity.X == 0)
                            {
                                npc.direction = !IsGlennToTheLeft ? 1 : -1;
                            }
                        }
                        else
                        {
                            guardian = Sardine;
                            bool IsSardineToTheLeft = guardian.Position.X - npc.Center.X < 0;
                            if (Math.Abs(guardian.Position.X - npc.Center.X) >= 96)
                            {
                                if (!IsSardineToTheLeft)
                                {
                                    MoveRight = true;
                                }
                                else
                                {
                                    MoveLeft = true;
                                }
                            }
                            if (!guardian.IsAttackingSomething && guardian.Velocity.X == 0)
                            {
                                guardian.FaceDirection(!IsSardineToTheLeft);
                            }
                            if (npc.velocity.X == 0)
                            {
                                npc.direction = !IsSardineToTheLeft ? 1 : -1;
                            }
                        }
                        if (SceneStep >= SceneIds.BreePersuadesThePlayerToCallHimBack && SceneStep <= SceneIds.BreePersuadesThePlayerALittleCloser)
                        {
                            if (InterruptedOnce)
                            {
                                ChangeScene(SceneIds.SardineIsCalledBackByPlayerAfterInterruption);
                            }
                            else
                            {
                                ChangeScene(SceneIds.PlayerCalledSardineBackAfterBreeAsked);
                            }
                        }
                    }
                    else
                    {
                        bool IsPlayerToTheLeft = player.Center.X - npc.Center.X < 0;
                        if (Math.Abs(player.Center.X - npc.Center.X) >= 96)
                        {
                            if (!IsPlayerToTheLeft)
                            {
                                MoveRight = true;
                            }
                            else
                            {
                                MoveLeft = true;
                            }
                        }
                        if (npc.velocity.X == 0)
                        {
                            npc.direction = !IsPlayerToTheLeft ? 1 : -1;
                        }
                        if (SceneStep >= SceneIds.BothAnswers && SceneStep < SceneIds.BreeThenSaysThatTheyShouldStayInTheWorldForAWhileThen)
                        {
                            InterruptedOnce = true;
                            if(Sardine == null)
                                ChangeScene(SceneIds.PlayerUnsummonedSardine);
                            else
                                ChangeScene(SceneIds.PlayerUnsummonedGlenn);
                        }
                    }
                }
                else if (HadGlennSpotted)
                {
                    if (Glenn != null)
                    {
                        guardian = Glenn;
                        bool IsGlennToTheLeft = guardian.Position.X - npc.Center.X < 0;
                        if (Math.Abs(guardian.Position.X - npc.Center.X) >= 96)
                        {
                            if (!IsGlennToTheLeft)
                            {
                                MoveRight = true;
                            }
                            else
                            {
                                MoveLeft = true;
                            }
                        }
                        if (!guardian.IsAttackingSomething && guardian.Velocity.X == 0)
                        {
                            guardian.FaceDirection(!IsGlennToTheLeft);
                        }
                        if (npc.velocity.X == 0)
                        {
                            npc.direction = !IsGlennToTheLeft ? 1 : -1;
                        }
                        if (SceneStep >= SceneIds.BreePersuadesThePlayerToCallHimBack && SceneStep <= SceneIds.BreePersuadesThePlayerALittleCloser)
                        {
                            if (InterruptedOnce)
                            {
                                ChangeScene(SceneIds.SardineIsCalledBackByPlayerAfterInterruption);
                            }
                            else
                            {
                                ChangeScene(SceneIds.PlayerCalledSardineBackAfterBreeAsked);
                            }
                        }
                    }
                    else
                    {
                        bool IsPlayerToTheLeft = player.Center.X - npc.Center.X < 0;
                        if (Math.Abs(player.Center.X - npc.Center.X) >= 96)
                        {
                            if (!IsPlayerToTheLeft)
                            {
                                MoveRight = true;
                            }
                            else
                            {
                                MoveLeft = true;
                            }
                        }
                        if (npc.velocity.X == 0)
                        {
                            npc.direction = !IsPlayerToTheLeft ? 1 : -1;
                        }
                        if (SceneStep >= SceneIds.GlennAnswer && SceneStep < SceneIds.BreeJoinsToTakeCareOfGlenn)
                        {
                            InterruptedOnce = true;
                            ChangeScene(SceneIds.PlayerUnsummonedGlenn);
                        }
                    }
                }
                else if (HadSardineSpotted)
                {
                    if (Sardine != null)
                    {
                        guardian = Sardine;
                        bool IsSardineToTheLeft = guardian.Position.X - npc.Center.X < 0;
                        if (Math.Abs(guardian.Position.X - npc.Center.X) >= 96)
                        {
                            if (!IsSardineToTheLeft)
                            {
                                MoveRight = true;
                            }
                            else
                            {
                                MoveLeft = true;
                            }
                        }
                        if (!guardian.IsAttackingSomething)
                        {
                            guardian.FaceDirection(!IsSardineToTheLeft);
                        }
                        if (npc.velocity.X == 0)
                        {
                            npc.direction = !IsSardineToTheLeft ? 1 : -1;
                        }
                        if (SceneStep >= SceneIds.BreePersuadesThePlayerToCallHimBack && SceneStep <= SceneIds.BreePersuadesThePlayerALittleCloser)
                        {
                            if (InterruptedOnce)
                            {
                                ChangeScene(SceneIds.SardineIsCalledBackByPlayerAfterInterruption);
                            }
                            else
                            {
                                ChangeScene(SceneIds.PlayerCalledSardineBackAfterBreeAsked);
                            }
                        }
                    }
                    else
                    {
                        bool IsPlayerToTheLeft = player.Center.X - npc.Center.X < 0;
                        if (Math.Abs(player.Center.X - npc.Center.X) >= 96)
                        {
                            if (!IsPlayerToTheLeft)
                            {
                                MoveRight = true;
                            }
                            else
                            {
                                MoveLeft = true;
                            }
                        }
                        if (npc.velocity.X == 0)
                        {
                            npc.direction = !IsPlayerToTheLeft ? 1 : -1;
                        }
                        if (SceneStep >= SceneIds.SardineStaysAndTalksToBree && SceneStep < SceneIds.BreeTurnsToTownNpc)
                        {
                            InterruptedOnce = true;
                            ChangeScene(SceneIds.PlayerUnsummonedSardine);
                        }
                        if (SceneStep == SceneIds.SardineSpotted)
                        {
                            ChangeScene(SceneIds.SardineFlees);
                        }
                    }
                }
                if (SceneChange)
                {
                    switch (SceneStep)
                    {
                        case SceneIds.SardineSpotted:
                            if (guardian.FriendshipLevel < 5 && (!PlayerMod.HasBuddiesModeOn(player) || !PlayerMod.GetPlayerBuddy(player).IsSameID(guardian))) //Tries to flee
                            {
                                ChangeScene(SceneIds.SardineFlees);
                                guardian.SaySomething("Uh oh, I gotta go.");
                            }
                            else //Stays
                            {
                                ChangeScene(SceneIds.SardineStaysAndTalksToBree);
                                guardian.SaySomething("Uh oh.");
                            }
                            break;
                        case SceneIds.SardineFlees:
                            if (PlayerMod.GetPlayerMainGuardian(player).Active)
                            {
                                TerraGuardian SardineRef = null;
                                foreach(TerraGuardian tg in player.GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                                {
                                    if (tg.ID == 2 && tg.ModID == MainMod.mod.Name)
                                    {
                                        SardineRef = tg;
                                        break;
                                    }
                                }
                                player.GetModPlayer<PlayerMod>().DismissGuardian(2, "");
                                if(SardineRef != null && SardineRef.Active)
                                {
                                    SardineRef.Spawn();
                                }
                            }
                            SayMessage("What? Where did he go?");
                            ChangeScene(SceneIds.BreeAsksWhereSardineWent);
                            break;
                        case SceneIds.BreeAsksWhereSardineWent:
                            SayMessage("You can call him back, right? Do so.");
                            ChangeScene(SceneIds.BreePersuadesThePlayerToCallHimBack);
                            break;
                        case SceneIds.BreePersuadesThePlayerToCallHimBack:
                            SayMessage("Call him back. NOW!");
                            ChangeScene(SceneIds.BreePersuadesThePlayerToCallHimBackAgain);
                            break;
                        case SceneIds.BreePersuadesThePlayerToCallHimBackAgain:
                            ChangeScene(SceneIds.BreePersuadesThePlayerALittleCloser);
                            SayMessage("I said... NOW!!!!");
                            break;
                        case SceneIds.BreePersuadesThePlayerALittleCloser:
                            {
                                string Message = "";
                                switch (SceneRepeatTimes)
                                {
                                    case 0:
                                        Message = "Call him! JUST. CALL. HIM!";
                                        break;
                                    case 1:
                                        Message = "You are making me angry.... EVEN MORE ANGRY!!!";
                                        break;
                                    case 2:
                                        Message = "GRRRRRRR!!! RRRRRRRRRRRRRRRR!!! RRRRRRRRRRRRRRGHHHH!!";
                                        break;
                                    case 3:
                                        Message = "MY PATIENCE IS ALREADY GOING DOWN THE DRAIN! CALL HIM BACK, NOW!!";
                                        break;
                                    case 4:
                                        Message = "ENOUGH!! CALL HIM, NOW!";
                                        ChangeScene(SceneIds.BreeForcedPlayerToCallSardineBack);
                                        break;
                                }
                                if (SceneStep == SceneIds.BreePersuadesThePlayerALittleCloser && SceneRepeatTimes < 4)
                                    ChangeScene(SceneIds.BreePersuadesThePlayerALittleCloser);
                                SayMessage(Message);
                            }
                            break;
                        case SceneIds.BreeForcedPlayerToCallSardineBack:
                            {
                                if (HadSardineSpotted)
                                {
                                    player.GetModPlayer<PlayerMod>().CallGuardian(2, "");
                                }
                                if (HadGlennSpotted)
                                {
                                    player.GetModPlayer<PlayerMod>().CallGuardian(GuardianBase.Glenn, "");
                                }
                                SayMessage("Good.");
                                if (HadSardineSpotted && HadGlennSpotted)
                                {
                                    ChangeScene(SceneIds.BreeAsksWhereWasSardine);
                                }
                                else if (HadGlennSpotted)
                                {
                                    ChangeScene(SceneIds.GlennSpotted);
                                }
                                else
                                {
                                    ChangeScene(SceneIds.SardineStaysAndTalksToBree);
                                }
                            }
                            break;
                        case SceneIds.SardineStaysAndTalksToBree:
                            {
                                guardian.SaySomething("H-Hello dear, how have you been latelly?");
                                ChangeScene(SceneIds.BreeScoldsSardine);
                            }
                            break;
                        case SceneIds.BreeScoldsSardine:
                            {
                                SayMessage("Seriously?! That is all you have to say?");
                                ChangeScene(SceneIds.BreeContinues);
                            }
                            break;
                        case SceneIds.BreeContinues:
                            {
                                SayMessage("You abandon me and your son at home and that is what you have to say?");
                                ChangeScene(SceneIds.SardineTriesToApologise);
                            }
                            break;
                        case SceneIds.SardineTriesToApologise:
                            {
                                guardian.SaySomething("I'm sorry?");
                                ChangeScene(SceneIds.BreeDoesntAcceptTheApology);
                            }
                            break;
                        case SceneIds.BreeDoesntAcceptTheApology:
                            {
                                SayMessage("That is it? You numbskull, you've been missing for " + (guardian.Data.GetBirthdayAge - guardian.Base.Age + 2) + " WHOLE YEARS!!");
                                ChangeScene(SceneIds.BreeContinuesAfterNotAcceptingTheApology);
                            }
                            break;
                        case SceneIds.BreeContinuesAfterNotAcceptingTheApology:
                            {
                                SayMessage("Do you know how many worlds I travelled trying to find you! Even the planet of the tentacles I had to travel through!");
                                ChangeScene(SceneIds.SardineTriesToApologiseAgain);
                            }
                            break;
                        case SceneIds.SardineTriesToApologiseAgain:
                            {
                                guardian.SaySomething("I already said that I'm sorry. I... Kind of forgot what world we lived so... I couldn't return.");
                                guardian.DisplayEmotion(TerraGuardian.Emotions.Sweat);
                                ChangeScene(SceneIds.BreeTalksAboutTakingSardineBackWithHer);
                            }
                            break;
                        case SceneIds.BreeTalksAboutTakingSardineBackWithHer:
                            {
                                SayMessage("Then there is no problem, since YOU are going back home with ME!");
                                ChangeScene(SceneIds.SardineTriesTheButs);
                            }
                            break;
                        case SceneIds.SardineTriesTheButs:
                            {
                                guardian.SaySomething("But... But... I have a job here and...");
                                ChangeScene(SceneIds.BreeSaysNoToButs);
                            }
                            break;
                        case SceneIds.BreeSaysNoToButs:
                            {
                                SayMessage("No buts! We are going back now! I just need to remember which world we...");
                                ChangeScene(SceneIds.BreeForgotTheWorldTheyLived);
                            }
                            break;
                        case SceneIds.BreeForgotTheWorldTheyLived:
                            {
                                SayMessage("Oh no! I can't believe it! I forgot which world we lived!");
                                ChangeScene(SceneIds.SardineLaughsOfBree);
                            }
                            break;
                        case SceneIds.SardineLaughsOfBree:
                            {
                                guardian.SaySomething("Haha! Joke is on you now. Looks like you'll have to say here for a while, until you remember.");
                                guardian.DisplayEmotion(TerraGuardian.Emotions.Happy);
                                ChangeScene(SceneIds.BreeAgrees);
                            }
                            break;
                        case SceneIds.BreeAgrees:
                            {
                                SayMessage("Grrr... Alright! I'll stay for a while, but only until I remember the world we lived!");
                                ChangeScene(SceneIds.BreeIntroducesHerself);
                            }
                            break;
                        case SceneIds.BreeIntroducesHerself:
                            {
                                SayMessage("My name is " + Base.Name + ", don't expect me to stay for long.");
                                ChangeScene(SceneIds.BreeTurnsToTownNpc);
                            }
                            break;
                        case SceneIds.BreeTurnsToTownNpc:
                            {
                                TransformNPC();
                            }
                            break;

                        case SceneIds.SardineIsCalledBackByPlayer:
                            {
                                SayMessage("There he is.");
                                ChangeScene(SceneIds.SardineStaysAndTalksToBree);
                            }
                            break;

                        case SceneIds.SardineIsCalledBackByPlayerAfterInterruption:
                            {
                                SayMessage("Don't do that again!");
                                ChangeScene(SceneIds.BreeTalksAboutSardineGoingBackWithHer);
                            }
                            break;

                        case SceneIds.BreeTalksAboutSardineGoingBackWithHer:
                            {
                                SayMessage("We are returning to home right now!");
                                ChangeScene(SceneIds.SardineTriesTheButs);
                            }
                            break;

                        case SceneIds.PlayerUnsummonedSardine:
                            {
                                SayMessage("Hey! We were talking!");
                                InterruptedOnce = true;
                                ChangeScene(SceneIds.BreePersuadesThePlayerToCallHimBack);
                            }
                            break;

                        case SceneIds.PlayerCalledSardineBackAfterBreeAsked:
                            {
                                SayMessage("Thank you. Now where was I?");
                                ChangeScene(SceneIds.SardineStaysAndTalksToBree);
                            }
                            break;

                        case SceneIds.BreeSeesFriendlyFace:
                            {
                                SayMessage("Oh, It's you again.");
                                ChangeScene(SceneIds.BreeSaysHowSheAppearedThere);
                            }
                            break;

                        case SceneIds.BreeSaysHowSheAppearedThere:
                            {
                                SayMessage("I'm still looking for the world I lived, but It's funny to bump into you on the way.");
                                ChangeScene(SceneIds.BreeJoinsYou);
                            }
                            break;

                        case SceneIds.BreeJoinsYou:
                            {
                                SayMessage("Anyway, I'm here, If you need me.");
                                ChangeScene(SceneIds.BreeTurnsToTownNpc);
                            }
                            break;

                        case SceneIds.GlennSpotted:
                            {
                                SayMessage("Glenn! What are you doing here? You should be at home.");
                                ChangeScene(SceneIds.GlennAnswer);
                            }
                            break;

                        case SceneIds.GlennAnswer:
                            {
                                guardian.SaySomething("You and Dad were taking too long to come home, so I came looking for you two.");
                                ChangeScene(SceneIds.AsksGlennIfHesOkay);
                            }
                            break;

                        case SceneIds.AsksGlennIfHesOkay:
                            {
                                SayMessage("But It's dangerous out there! Are you hurt?");
                                ChangeScene(SceneIds.GlennSaysThatIsFine);
                            }
                            break;

                        case SceneIds.GlennSaysThatIsFine:
                            {
                                guardian.SaySomething("I'm okay, don't worry. This Terrarian let me stay here, and It's safe here.");
                                ChangeScene(SceneIds.BreeJoinsToTakeCareOfGlenn);
                            }
                            break;

                        case SceneIds.BreeJoinsToTakeCareOfGlenn:
                            {
                                SayMessage("I can't let you stay here alone, I shouldn't have let you stay alone at home either. I'll stay here to take care of you, and look for your father.");
                                ChangeScene(SceneIds.BreeIntroducesHerself);
                            }
                            break;

                        case SceneIds.BreeSpotsSardineAndGlenn:
                            {
                                SayMessage("Are you two alright?");
                                ChangeScene(SceneIds.BothAnswers);
                            }
                            break;

                        case SceneIds.BothAnswers:
                            {
                                Sardine.SaySomething("Yes, we're fine.");
                                Glenn.SaySomething("I'm okay.");
                                ChangeScene(SceneIds.BreeAsksWhereWasSardine);
                            }
                            break;

                        case SceneIds.BreeAsksWhereWasSardine:
                            {
                                SayMessage("I'm so glad you two are fine. Sardine, where did you go? Why didn't you returned home?");
                                ChangeScene(SceneIds.SardineAnswers);
                            }
                            break;

                        case SceneIds.SardineAnswers:
                            {
                                Sardine.SaySomething("I was trying to find treasures for you two... And then I was saved by that Terrarian from a bounty hunt that gone wrong.");
                                ChangeScene(SceneIds.BreeThenFeelsRelievedAndAsksGlennWhatIsHeDoingHere);
                            }
                            break;

                        case SceneIds.BreeThenFeelsRelievedAndAsksGlennWhatIsHeDoingHere:
                            {
                                SayMessage("I think I should think you then, Terrarian. Now Glenn, I told you to wait for us at home!");
                                ChangeScene(SceneIds.GlennAnswers);
                            }
                            break;

                        case SceneIds.GlennAnswers:
                            {
                                Glenn.SaySomething("I stayed at home, but I was worried that you two didn't returned yet, so I explored worlds trying to find you two.");
                                ChangeScene(SceneIds.BreeSuggestsToSpendSomeTimeTogetherBeforeReturningToTheWorld);
                            }
                            break;

                        case SceneIds.BreeSuggestsToSpendSomeTimeTogetherBeforeReturningToTheWorld:
                            {
                                SayMessage("That's really reckless and dangerous, but I'm glad you two are unharmed. Let's spend a little time here and then return home.");
                                ChangeScene(SceneIds.SardineSaysThatForgotWhereTheWorldIsAt);
                            }
                            break;

                        case SceneIds.SardineSaysThatForgotWhereTheWorldIsAt:
                            {
                                Sardine.SaySomething("I hope you know our way home, because I forgot.");
                                ChangeScene(SceneIds.GlennAlsoSaysThatForgot);
                            }
                            break;

                        case SceneIds.GlennAlsoSaysThatForgot:
                            {
                                Glenn.SaySomething("I also forgot my way back home, sorry mom.");
                                ChangeScene(SceneIds.BreeThenSaysThatTheyShouldStayInTheWorldForAWhileThen);
                            }
                            break;

                        case SceneIds.BreeThenSaysThatTheyShouldStayInTheWorldForAWhileThen:
                            {
                                SayMessage("I can't remember either.... Well, I hope you don't mind if we stay here for longer, Terrarian.");
                                ChangeScene(SceneIds.BreeIntroducesHerself);
                            }
                            break;

                        case SceneIds.PlayerUnsummonedGlenn:
                            {
                                SayMessage("Where did you sent my son? Call him back now!");
                                InterruptedOnce = true;
                                ChangeScene(SceneIds.BreePersuadesThePlayerToCallHimBack);
                            }
                            break;
                    }
                }
                if (StepTime <= -600)
                {
                    SayMessage("Uh... What was I saying? Oh... I remember now.");
                    ChangeScene(SceneStep);
                }
            }
            else
            {
                Idle = true;
            }
            base.AI();
            if (!Main.dayTime)
            {
                bool PlayerInRange = false;
                for (int p = 0; p < 255; p++)
                {
                    Player pl = Main.player[p];
                    if ((Math.Abs(pl.Center.X - npc.Center.X) >= NPC.sWidth * 0.5f + NPC.safeRangeX ||
                        Math.Abs(pl.Center.Y - npc.Center.Y) >= NPC.sHeight * 0.5f + NPC.safeRangeY))
                    {
                        PlayerInRange = true;
                    }
                }
                if (!PlayerInRange)
                {
                    Main.NewText("The White Cat resumed the search for her husband.");
                    npc.active = false;
                    npc.life = 0;
                }
            }
        }

        public void ChangeScene(int number)
        {
            ChangeScene((SceneIds)number);
        }

        public void ChangeScene(SceneIds scene)
        {
            if (scene == SceneStep)
            {
                SceneRepeatTimes++;
            }
            else
            {
                SceneRepeatTimes = 0;
            }
            SceneStep = scene;
            //Main.NewText("Scene: " + scene.ToString());
            if (MessageTime > 0)
                StepTime = MessageTime + 30;
            else
                StepTime = 300;
        }

        public override string GetChat()
        {
            if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID))
            {
                return "Oh, It's you again... Well... I'm here, If you need me.";
            }
            if (PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], 2, GuardianModID) || PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Glenn, GuardianModID))
            {
                if (SceneStep == SceneIds.NoScene)
                    ChangeScene(SceneIds.BreeSpotsSardineAndGlenn);
                return "Are they... Glenn! Sardine!";
            }
            else if (PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], 2, GuardianModID))
            {
                if (SceneStep == SceneIds.NoScene)
                    ChangeScene(0);
                return "Wait, is that... Sardine! I finally found you!";
            }
            else if (PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Glenn, GuardianModID))
            {
                if (SceneStep == SceneIds.NoScene)
                    ChangeScene(SceneIds.GlennSpotted);
                return "Gl... Glenn?!";
            }
            else if (!dialogues.Finished)
            {
                return dialogues.GetQuestion();
            }
            else
            {
                return GetEndDialogueMessage;
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID))
            {
                button = "Alright.";
            }
            else if (PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], 2, GuardianModID))
            {
                button = button2 = "";
            }
            else if (!dialogues.Finished)
            {
                dialogues.GetAnswers(out button, out button2);
            }
            else
            {
                if (DialogueResultSuccess)
                {
                    button = "Feel free to stay";
                    button2 = "";
                }
            }
        }

        private string GetEndDialogueMessage
        {
            get
            {
                if (!dialogues.LastAnswerWasLeft)
                {
                    if (DialogueResultSuccess)
                    {
                        return "You know, you're right. I'll stay for a while and see If he shows up. I can recover some of my strength meanwhile.\nBy the way, my name is " + Base.Name + ".";
                    }
                    else
                    {
                        return "No way, I have to keep looking for him. I'll just stay here for a while to recover some strength.";
                    }
                }
                else
                {
                    if (DialogueResultSuccess)
                    {
                        return "Actually, you're right, I'll stay here for a while. I'll try gathering information around, to see If I have clues of where he went. \nMy name is " + Base.Name + ".";
                    }
                    else
                    {
                        return "I will do, I still have some places to look for him.";
                    }
                }
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID))
            {
                NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                //npc.Transform(ModContent.NPCType<GuardianNPC.List.FemaleCatGuardian>());
                TransformNPC();
                return;
            }
            else if (PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], 2, GuardianModID))
            {
                return;
            }
            else if (!dialogues.Finished)
            {
                dialogues.MarkAnswer(firstButton);
                if (dialogues.Finished)
                {
                    Main.npcChatText = GetEndDialogueMessage;
                }
                else
                {
                    Main.npcChatText = dialogues.GetQuestion();
                }
            }
            else if (dialogues.Finished)
            {
                if (DialogueResultSuccess)
                {
                    TransformNPC();
                }
            }
        }

        private void TransformNPC()
        {
            //Transform into Bree guardian npc
            NpcMod.AddGuardianMet(7);
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().AddNewGuardian(7);
            if(PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], 7).FriendshipLevel == 0)
                PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], 7).IncreaseFriendshipProgress(1);
            WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianBase.Bree);
            //npc.Transform(ModContent.NPCType<GuardianNPC.List.FemaleCatGuardian>());
        }

        public override bool CanChat()
        {
            return SceneStep == SceneIds.NoScene;
        }

        public static bool BreeMaySpawn { get { return NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedSlimeKing; } }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.water && Main.dayTime && !NpcMod.HasGuardianNPC(7) && !NpcMod.HasMetGuardian(7) && BreeMaySpawn && Main.time > 27000 && Main.time < 48600 && !NPC.AnyNPCs(ModContent.NPCType<BreeNPC>()))
            {
                return (float)(Main.time - 27000) / 432000 * 0.333f;
            }
            return 0;
        }

        public override void ModifyDrawDatas(List<GuardianDrawData> dds, Vector2 Position, Rectangle BodyRect, Rectangle LArmRect, Rectangle RArmRect, Vector2 Origin, Color color, Microsoft.Xna.Framework.Graphics.SpriteEffects seffects)
        {
            Microsoft.Xna.Framework.Graphics.Texture2D BagTexture = Base.sprites.GetExtraTexture(Companions.BreeBase.BagTextureID);
            Rectangle backrect = BodyRect,
                frontrect = BodyRect;
            backrect.Y += backrect.Height;
            GuardianDrawData bagback = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, BagTexture, Position, backrect, color, npc.rotation, Origin, npc.scale, seffects),
                bagfront = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, BagTexture, Position, frontrect, color, npc.rotation, Origin, npc.scale, seffects);
            for (int i = dds.Count - 1; i >= 0; i--)
            {
                if (dds[i].textureType == GuardianDrawData.TextureType.TGBody)
                {
                    dds.Insert(i + 1, bagfront);
                    dds.Insert(i, bagback);
                }
            }
        }

        public enum SceneIds : short
        {
            NoScene = -1,
            SardineSpotted,
            SardineFlees,
            BreeAsksWhereSardineWent,
            BreePersuadesThePlayerToCallHimBack,
            BreePersuadesThePlayerToCallHimBackAgain,
            BreePersuadesThePlayerALittleCloser,
            BreeForcedPlayerToCallSardineBack,

            SardineStaysAndTalksToBree,
            BreeScoldsSardine,
            BreeContinues,
            SardineTriesToApologise,
            BreeDoesntAcceptTheApology,
            BreeContinuesAfterNotAcceptingTheApology,
            SardineTriesToApologiseAgain,
            BreeTalksAboutTakingSardineBackWithHer,
            SardineTriesTheButs,
            BreeSaysNoToButs,
            BreeForgotTheWorldTheyLived,
            SardineLaughsOfBree,
            BreeAgrees,
            BreeIntroducesHerself,
            BreeTurnsToTownNpc,

            SardineIsCalledBackByPlayer,
            SardineIsCalledBackByPlayerAfterInterruption,
            PlayerUnsummonedSardine,
            BreeTalksAboutSardineGoingBackWithHer,
            PlayerCalledSardineBackAfterBreeAsked,

            BreeSeesFriendlyFace,
            BreeSaysHowSheAppearedThere,
            BreeJoinsYou,

            GlennSpotted,
            GlennAnswer,
            AsksGlennIfHesOkay,
            GlennSaysThatIsFine,
            BreeJoinsToTakeCareOfGlenn,

            PlayerUnsummonedGlenn,

            BreeSpotsSardineAndGlenn,
            BothAnswers,
            BreeAsksWhereWasSardine,
            SardineAnswers,
            BreeThenFeelsRelievedAndAsksGlennWhatIsHeDoingHere,
            GlennAnswers,
            BreeSuggestsToSpendSomeTimeTogetherBeforeReturningToTheWorld,
            SardineSaysThatForgotWhereTheWorldIsAt,
            GlennAlsoSaysThatForgot,
            BreeThenSaysThatTheyShouldStayInTheWorldForAWhileThen
        }
    }
}

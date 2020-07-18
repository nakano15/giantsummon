using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon.Npcs
{
    //[AutoloadHead]
    public class BreeNPC : GuardianActorNPC
    {
        /*public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Bree_Head"; //Necessary
            }
        }*/

        public SceneIds SceneStep = SceneIds.NoScene;
        public int StepTime = 0, SceneRepeatTimes = 0;
        public bool InterruptedOnce = false;
        public bool DialogueResultSuccess { get { return dialogues.Points >= 4; } }
        public bool EndDialogDone = false;
        public DialogueChain dialogues = new DialogueChain();

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
            DisplayName.SetDefault("WhiteCat");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            //npc.townNPC = true;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
            npc.dontCountMe = true;
            if (npc.GetGlobalNPC<NpcMod>().mobType > MobTypes.Normal)
                npc.GetGlobalNPC<NpcMod>().mobType = MobTypes.Normal;
            NpcMod.LatestMobType = MobTypes.Normal;
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
            Player player = Main.player[Main.myPlayer];
            if (npc.ai[2] == 0)
            {
                if ((Math.Abs(player.Center.X - npc.Center.X) >= NPC.sWidth * 0.5f + NPC.safeRangeX ||
                    Math.Abs(player.Center.Y - npc.Center.Y) >= NPC.sHeight * 0.5f + NPC.safeRangeY))
                {
                    Main.NewText("A White Cat appeared to the " + GuardianBountyQuest.GetDirectionText(npc.Center - player.Center) + " of " + player.name + " position.");
                }
                npc.ai[2] = 1;
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
                else if (PlayerMod.PlayerHasGuardian(player, 2))
                {
                    ChangeScene(0);
                    SayMessage("Sardine! I finally found you!");
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
                TerraGuardian guardian = PlayerMod.GetPlayerMainGuardian(player);
                if (PlayerMod.HasGuardianSummoned(player, 2))
                {
                    guardian = PlayerMod.GetPlayerSummonedGuardian(player, 2);
                    bool IsSardineToTheLeft = guardian.Position.X - npc.Center.X < 0;
                    if (Math.Abs(guardian.CenterPosition.X - npc.Center.X) >= 96)
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
                if (SceneChange)
                {
                    switch (SceneStep)
                    {
                        case SceneIds.SardineSpotted:
                            if (guardian.FriendshipLevel < 5) //Tries to flee
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
                                player.GetModPlayer<PlayerMod>().DismissGuardian(2, "");
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
                                player.GetModPlayer<PlayerMod>().CallGuardian(2, "");
                                SayMessage("Good.");
                                ChangeScene(SceneIds.SardineStaysAndTalksToBree);
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
                                SayMessage("What was that for?!");
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
            StepTime = 180;
        }

        public override string GetChat()
        {
            if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID))
            {
                return "Oh, It's you again... Well... I'm here, If you need me.";
            }
            if (PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], 2, GuardianModID))
            {
                if(SceneStep == SceneIds.NoScene)
                    ChangeScene(0);
                return "Wait, is that... Sardine! I finally found you!";
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
                npc.Transform(ModContent.NPCType<GuardianNPC.List.FemaleCatGuardian>());
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
            npc.Transform(ModContent.NPCType<GuardianNPC.List.FemaleCatGuardian>());
        }

        public override bool CanChat()
        {
            return SceneStep == SceneIds.NoScene;
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
            BreeJoinsYou
        }

        public static bool BreeMaySpawn { get { return NPC.downedBoss2 || NPC.downedBoss3; } }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.dayTime && !NpcMod.HasGuardianNPC(7) && !NpcMod.HasMetGuardian(7) && BreeMaySpawn && Main.time > 27000 && Main.time < 48600 && !NPC.AnyNPCs(ModContent.NPCType<BreeNPC>()))
            {
                return (float)(Main.time - 27000) / 432000;
            }
            return 0;
        }
    }
}

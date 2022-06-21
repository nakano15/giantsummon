using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    public class LeopoldNPC : GuardianActorNPC
    {
        public SceneIDs SceneID = SceneIDs.NoScene;
        public int SceneTime = 0;
        public bool EncounterAct { get { return SceneID >= SceneIDs.LeopoldSpotsThePlayer && SceneID < SceneIDs.ThinksAboutTryingToTalk; } }
        public bool SocialAct { get { return SceneID >= SceneIDs.ThinksAboutTryingToTalk && SceneID < SceneIDs.ThinksThePlayerDidntUnderstandHim; } }
        public bool FearAct { get { return SceneID >= SceneIDs.GotScaredUponPlayerApproach && SceneID < SceneIDs.AttemptsToRunAway1; } }
        public bool ScareAct { get { return SceneID >= SceneIDs.FearsAboutPlayerAttackingHim && SceneID < SceneIDs.TellsThatIsgoingToFlee; } }
        public bool PlayerHasLeopold = false;

        public LeopoldNPC()
            : base(10, "")
        {

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bunny Guardian");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
            npc.dontCountMe = true;
            npc.townNPC = false;
            if (npc.GetGlobalNPC<NpcMod>().mobType > MobTypes.Normal)
                npc.GetGlobalNPC<NpcMod>().mobType = MobTypes.Normal;
            NpcMod.LatestMobType = MobTypes.Normal;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
        }

        public override void AI()
        {
            if (npc.direction == 0)
                npc.direction = Main.rand.NextDouble() < 0.5 ? -1 : 1;
            if (SceneID == SceneIDs.NoScene)
            {
                Idle = true;
                npc.TargetClosest(false);
                if (Main.player[npc.target].Distance(npc.Center) < 200)
                {
                    PlayerHasLeopold = false;
                    if (PlayerMod.PlayerHasGuardian(Main.player[npc.target], 10))
                    {
                        PlayerHasLeopold = true;
                        ChangeScene(SceneIDs.KnownPlayerSpottedByLeopold);
                        SayMessage("*Aaahhh!!! What is that?!**");
                    }
                    else
                    {
                        ChangeScene(SceneIDs.LeopoldSpotsThePlayer);
                        SayMessage("*Ack! W-what is that?!*");
                    }
                }
            }
            else
            {
                if (SceneID != SceneIDs.LeopoldFreeForRecruit && SceneID != SceneIDs.Flee)
                {
                    npc.npcSlots = 100;
                }
                Player player = Main.player[npc.target];
                if (SceneID < SceneIDs.ThinksAboutTryingToTalk)
                {
                    if (player.Distance(npc.Center) < 150)
                    {
                        SayMessage("*Ah!! It approached.*");
                        ChangeScene(SceneIDs.WondersIfPlayerWillAttackHim);
                        Jump = true;
                    }
                }
                if (SceneID >= SceneIDs.GotScaredUponPlayerApproach && SceneID < SceneIDs.PlayerDoesNothing)
                {
                    if (player.Distance(npc.Center) < 150)
                    {
                        if (player.Center.X - npc.Center.X < 0)
                        {
                            MoveRight = true;
                        }
                        else
                        {
                            MoveLeft = true;
                        }
                    }
                }
                if (SceneID >= SceneIDs.GotScaredUponPlayerApproach && SceneID < SceneIDs.AttemptsToRunAway1)
                {
                    if (player.Distance(npc.Center) < 100)
                    {
                        SayMessage("*Yaah!!! Please, Don't kill me...*");
                        ChangeScene(SceneIDs.Crying1);
                        Jump = true;
                    }
                }
                npc.FaceTarget();
                if (SceneTime <= 0)
                {
                    switch (SceneID)
                    {
                        case SceneIDs.LeopoldSpotsThePlayer:
                            SayMessage("*It's a funny kind of creature.*");
                            ChangeScene(SceneIDs.LeopoldSaysNeverSawAnythingLikeThat);
                            break;
                        case SceneIDs.LeopoldSaysNeverSawAnythingLikeThat:
                            SayMessage("*I've never seen something like that.*");
                            ChangeScene(SceneIDs.LeopoldQuestionsHimselfAboutCreature);
                            break;
                        case SceneIDs.LeopoldQuestionsHimselfAboutCreature:
                            SayMessage("*What kind of creature is it?*");
                            ChangeScene(SceneIDs.IsItTerrarian);
                            break;
                        case SceneIDs.IsItTerrarian:
                            SayMessage("*Could It be a Terrarian?*");
                            ChangeScene(SceneIDs.NoticesOutfit);
                            break;
                        case SceneIDs.NoticesOutfit:
                            SayMessage("*It surelly have unusual outfit...*");
                            ChangeScene(SceneIDs.QuestionsIfIsReallyATerrarian);
                            break;
                        case SceneIDs.QuestionsIfIsReallyATerrarian:
                            SayMessage("*Maybe, It is said that they inhabit this world.*");
                            if (player.GetModPlayer<PlayerMod>().Guardian.Active && player.GetModPlayer<PlayerMod>().Guardian.Base.IsTerraGuardian)
                                ChangeScene(SceneIDs.NoticesOtherGuardians);
                            else
                                ChangeScene(SceneIDs.NoticesPlayerLooking);
                            break;
                        case SceneIDs.NoticesPlayerLooking:
                            SayMessage("*It is looking at me.*");
                            ChangeScene(SceneIDs.WondersPlayerReaction);
                            break;
                        case SceneIDs.WondersPlayerReaction:
                            SayMessage("*What is it planning to do?*");
                            ChangeScene(SceneIDs.IsPreparingAttack);
                            break;
                        case SceneIDs.IsPreparingAttack:
                            SayMessage("*Is it preparing to attack?*");
                            ChangeScene(SceneIDs.ThreatenUseSpell);
                            break;
                        case SceneIDs.ThreatenUseSpell:
                            SayMessage("*If It tries to attack, I will blow It with my spells.*");
                            ChangeScene(SceneIDs.FindsWeirdTheNoReaction, true);
                            break;
                        case SceneIDs.FindsWeirdTheNoReaction:
                            SayMessage("*Weird, It isn't doing anything.*");
                            ChangeScene(SceneIDs.ThinksAboutTryingToTalk);
                            break;
                        case SceneIDs.ThinksAboutTryingToTalk:
                            SayMessage("*Maybe If I try talking...*");
                            ChangeScene(SceneIDs.WondersIftheySpeak);
                            break;
                        case SceneIDs.WondersIftheySpeak:
                            SayMessage("*No... Wait, I don't even know if they can speak.*");
                            ChangeScene(SceneIDs.MentionsABook);
                            break;
                        case SceneIDs.MentionsABook:
                            SayMessage("*There is a book that theorizes that but...*");
                            ChangeScene(SceneIDs.ThinksAboutTrying);
                            break;
                        case SceneIDs.ThinksAboutTrying:
                            SayMessage("*Maybe If I try...*");
                            ChangeScene(SceneIDs.TriesTalking);
                            break;
                        case SceneIDs.TriesTalking:
                            SayMessage("*H-hey... Can you hear me?*");
                            ChangeScene(SceneIDs.WondersIfIsScared, true);
                            break;
                        case SceneIDs.WondersIfIsScared:
                            SayMessage("*(Maybe It's scared...)*");
                            ChangeScene(SceneIDs.SaysWontHurt);
                            break;
                        case SceneIDs.SaysWontHurt:
                            SayMessage("*Come here.. I wont hurt you...*");
                            ChangeScene(SceneIDs.TriesHidingFear, true);
                            break;
                        case SceneIDs.TriesHidingFear:
                            SayMessage("*(..Don't shake... You don't want to scare it...)*");
                            ChangeScene(SceneIDs.NoticesDidntWorked);
                            break;
                        case SceneIDs.NoticesDidntWorked:
                            SayMessage("*(Huh? Didn't worked? Maybe If...)*");
                            ChangeScene(SceneIDs.TriesGivingFood);
                            break;
                        case SceneIDs.TriesGivingFood:
                            SayMessage("*Uh... I... Got some food.. Do you want it...?*");
                            ChangeScene(SceneIDs.WondersHowStupidHisActionWas);
                            break;
                        case SceneIDs.WondersHowStupidHisActionWas:
                            SayMessage("*(Got some food?! What am I thinking?!)*");
                            ChangeScene(SceneIDs.WaitsAFewMoments);
                            break;
                        case SceneIDs.WaitsAFewMoments:
                            SayMessage("*...*");
                            ChangeScene(SceneIDs.ThinksThePlayerDidntUnderstandHim);
                            break;
                        case SceneIDs.ThinksThePlayerDidntUnderstandHim:
                            SayMessage("*I guess It can't understand me at all.*");
                            ChangeScene(SceneIDs.TalksAboutWalkingAway);
                            break;
                        case SceneIDs.TalksAboutWalkingAway:
                            SayMessage("*Maybe If I just walk away...*");
                            ChangeScene(SceneIDs.Flee);
                            break;
                            ////////////////////
                        case SceneIDs.GotScaredUponPlayerApproach:
                            SayMessage("*Ah!! It approached.*");
                            ChangeScene(SceneIDs.WondersIfPlayerWillAttackHim);
                            break;
                        case SceneIDs.WondersIfPlayerWillAttackHim:
                            SayMessage("*Is it going to attack me?!*");
                            ChangeScene(SceneIDs.FearPlayerAttack1);
                            break;
                        case SceneIDs.FearPlayerAttack1:
                            SayMessage("*No!! I'm too young to die!!*");
                            ChangeScene(SceneIDs.FearPlayerAttack2);
                            break;
                        case SceneIDs.FearPlayerAttack2:
                            SayMessage("*I didn't even finished the mysteries of the Terra Realm.*");
                            ChangeScene(SceneIDs.FearPlayerAttack3);
                            break;
                        case SceneIDs.FearPlayerAttack3:
                            SayMessage("*This... This is how I'm going to die?!*");
                            ChangeScene(SceneIDs.FearPlayerAttack4);
                            break;
                        case SceneIDs.FearPlayerAttack4:
                            SayMessage("*The great Leopold... Devoured by a Terrarian...*");
                            ChangeScene(SceneIDs.FearPlayerAttack5);
                            break;
                        case SceneIDs.FearPlayerAttack5:
                            SayMessage("*Oh... What a cruel world...*");
                            ChangeScene(SceneIDs.PlayerDoesNothing, true);
                            break;
                        case SceneIDs.PlayerDoesNothing:
                            SayMessage("....");
                            ChangeScene(SceneIDs.PlayerDoesNothing2);
                            break;
                        case SceneIDs.PlayerDoesNothing2:
                            SayMessage("*Uh...*");
                            ChangeScene(SceneIDs.WondersIfPlayerWillAttack);
                            break;
                        case SceneIDs.WondersIfPlayerWillAttack:
                            SayMessage("*Is... It going to try attacking me?*");
                            ChangeScene(SceneIDs.WondersIfScaredPlayer);
                            break;
                        case SceneIDs.WondersIfScaredPlayer:
                            SayMessage("*Maybe... I scared it?*");
                            ChangeScene(SceneIDs.ThreatensPlayer);
                            break;
                        case SceneIDs.ThreatensPlayer:
                            SayMessage("*Yeah!! Don't you dare get near me, or I'll show you some thing!*");
                            ChangeScene(SceneIDs.RealizesHowStupidWhatHeSaidWas);
                            break;
                        case SceneIDs.RealizesHowStupidWhatHeSaidWas:
                            SayMessage("*(Show It something?! What were I thinking?)*");
                            ChangeScene(SceneIDs.SeesIfPlayerReacts, true);
                            break;
                        case SceneIDs.SeesIfPlayerReacts:
                            SayMessage("...");
                            ChangeScene(SceneIDs.WonderIfAngeredPlayer);
                            break;
                        case SceneIDs.WonderIfAngeredPlayer:
                            SayMessage("*(Did I anger it?)*");
                            ChangeScene(SceneIDs.SeesIfPlayerReactsAgain, true);
                            break;
                        case SceneIDs.SeesIfPlayerReactsAgain:
                            SayMessage("...");
                            ChangeScene(SceneIDs.WonderIfPlayerIsFrozenInFear);
                            break;
                        case SceneIDs.WonderIfPlayerIsFrozenInFear:
                            SayMessage("*(Maybe It's frozen in fear?)*");
                            ChangeScene(SceneIDs.SeesIfPlayerReactsAgainAgain, true);
                            break;
                        case SceneIDs.SeesIfPlayerReactsAgainAgain:
                            SayMessage("...");
                            ChangeScene(SceneIDs.AttemptsToRunAway1);
                            break;
                        case SceneIDs.AttemptsToRunAway1:
                            SayMessage("*I... Kind of have more things to do... so...*");
                            ChangeScene(SceneIDs.AttemptsToRunAway2);
                            break;
                        case SceneIDs.AttemptsToRunAway2:
                            SayMessage("*Well, I'm gone!*");
                            ChangeScene(SceneIDs.Flee);
                            break;
                            //////////////////////////////
                        case SceneIDs.FearsAboutPlayerAttackingHim:
                            SayMessage("*Yaah!!! Please, Don't kill me...*");
                            ChangeScene(SceneIDs.Crying1);
                            break;
                        case SceneIDs.Crying1:
                            SayMessage("*(Sniffle... Sob...)*");
                            ChangeScene(SceneIDs.Crying2);
                            break;
                        case SceneIDs.Crying2:
                            SayMessage("*(Snif...)*");
                            ChangeScene(SceneIDs.WaitingForReaction, true);
                            break;
                        case SceneIDs.WaitingForReaction:
                            SayMessage("...");
                            ChangeScene(SceneIDs.AsksIfPlayerIsGoingToAttackHim);
                            break;
                        case SceneIDs.AsksIfPlayerIsGoingToAttackHim:
                            SayMessage("*You... Aren't going to attack me... Right...?*");
                            ChangeScene(SceneIDs.AsksIfPlayerUnderstandWhatHeSays);
                            break;
                        case SceneIDs.AsksIfPlayerUnderstandWhatHeSays:
                            SayMessage("*Can... You even understand me...?*");
                            ChangeScene(SceneIDs.WaitingForReactionAgain, true);
                            break;
                        case SceneIDs.WaitingForReactionAgain:
                            SayMessage("*...*");
                            ChangeScene(SceneIDs.TellsThatIsgoingToFlee);
                            break;
                        case SceneIDs.TellsThatIsgoingToFlee:
                            SayMessage("*I'm.. Going to... Walk... Backaway... *");
                            ChangeScene(SceneIDs.RunsWhileScreaming);
                            break;
                        case SceneIDs.RunsWhileScreaming:
                            SayMessage("*Waaaaaaaaaahhh!!!*");
                            ChangeScene(SceneIDs.Flee);
                            break;
                        /////////////////////////////////
                        case SceneIDs.NoticesOtherGuardians:
                            SayMessage("*Wait, are those... TerraGuardians?*");
                            ChangeScene(SceneIDs.WondersWhyGuardiansFollowsPlayer);
                            break;
                        case SceneIDs.WondersWhyGuardiansFollowsPlayer:
                            SayMessage("*Why are those TerraGuardians with that Terrarian?*");
                            ChangeScene(SceneIDs.ThinksPlayerIsGuardiansPet);
                            break;
                        case SceneIDs.ThinksPlayerIsGuardiansPet:
                            SayMessage("*Maybe It's their little pet?*");
                            ChangeScene(SceneIDs.IgnoresTheAboveIdea);
                            break;
                        case SceneIDs.IgnoresTheAboveIdea:
                            SayMessage("*It... Doesn't look like it...*");
                            ChangeScene(SceneIDs.ThinksPlayerEnslavedGuardians);
                            break;
                        case SceneIDs.ThinksPlayerEnslavedGuardians:
                            SayMessage("*Maybe... Oh no... The Terrarian enslaved them!*");
                            ChangeScene(SceneIDs.YellsThatIsGoingToSaveGuardians);
                            break;
                        case SceneIDs.YellsThatIsGoingToSaveGuardians:
                            SayMessage("*Don't worry friends, I will save you all!*");
                            ChangeScene(SceneIDs.WondersHowToSaveGuardians);
                            break;
                        case SceneIDs.WondersHowToSaveGuardians:
                            SayMessage("*I should save them, but how...*");
                            ChangeScene(SceneIDs.PlayerMainGuardianTalksToLeopold);
                            break;
                        case SceneIDs.PlayerMainGuardianTalksToLeopold:
                            {
                                TerraGuardian Guardian = PlayerMod.GetPlayerMainGuardian(player);
                                string Message = "";
                                if (Guardian.ModID == MainMod.mod.Name)
                                {
                                    switch (Guardian.ID) //What about moving those dialogues to a separate method, so It's easier to find them.
                                    {
                                        default:
                                            Message = Guardian.GetMessage(GuardianBase.MessageIDs.LeopoldMessage1, "*"+Guardian.Name+" is saying that you're It's friend.*");
                                            break;
                                        case GuardianBase.Rococo:
                                            Message = "*"+Guardian.Name+" looks very confused.*";
                                            break;
                                        case GuardianBase.Blue:
                                            Message = "*"+Guardian.Name+" seems to be enjoying the show.*";
                                            break;
                                        case GuardianBase.Sardine:
                                            Message = "I think the way we met isn't that weird now, right pal?";
                                            break;
                                        case GuardianBase.Zacks:
                                            Message = "*Boss, can I eat that stupid bunny?*";
                                            break;
                                        case GuardianBase.Alex:
                                            Message = "Save me? Save me from what? Who's threatening me and my friend?!";
                                            break;
                                        case GuardianBase.Brutus:
                                            Message = "*Are you missing some rivets, long eared guy?*";
                                            break;
                                        case GuardianBase.Bree:
                                            Message = "Have you finished making yourself into a fool.";
                                            break;
                                        case GuardianBase.Mabel:
                                            Message = "*Hello, Teehee. Do you have a problem, bunny guy?*";
                                            break;
                                        case GuardianBase.Domino:
                                            Message = "*Don't look at me, I stopped selling the kind of merchandise that caused that long ago.*";
                                            break;
                                        case GuardianBase.Vladimir:
                                            Message = "*I think that guy needs a hug. Maybe It will end up fixing his head, I guess.*";
                                            break;
                                        case GuardianBase.Malisha:
                                            Message = "*And he still fears even his own shadow.*";
                                            break;
                                    }
                                }
                                Guardian.SaySomething(Message);
                            }
                            ChangeScene(SceneIDs.LeopoldAnswersTheGuardian);
                            break;
                        case SceneIDs.LeopoldAnswersTheGuardian:
                            {
                                TerraGuardian Guardian = PlayerMod.GetPlayerMainGuardian(player);
                                string Message = "";
                                if (Guardian.ModID == MainMod.mod.Name)
                                {
                                    switch (Guardian.ID)
                                    {
                                        default:
                                            Message = Guardian.GetMessage(GuardianBase.MessageIDs.LeopoldMessage2, "*You're friends of that Terrarian?*");
                                            break;
                                        case GuardianBase.Rococo:
                                            Message = "*Uh... What is it with the look in your face?*";
                                            break;
                                        case GuardianBase.Blue:
                                            Message = "*Wait, why are you laughing?*";
                                            break;
                                        case GuardianBase.Sardine:
                                            Message = "*Wait, \"pal\"? You're that Terrarian's friend?!*";
                                            break;
                                        case GuardianBase.Zacks:
                                            Message = "*Wait, you...Yaaaaaah!! It's a zombie!!!*";
                                            break;
                                        case GuardianBase.Alex:
                                            Message = "*F..Friend?!*";
                                            break;
                                        case GuardianBase.Brutus:
                                            Message = "*I...I'm not crazy?! What are you doing with that Terrarian?*";
                                            break;
                                        case GuardianBase.Bree:
                                            Message = "*Hey! I'm not a fool!*";
                                            break;
                                        case GuardianBase.Mabel:
                                            Message = "*Ah... Uh... No... Uh... Just... I'm... Fine...*";
                                            break;
                                        case GuardianBase.Domino:
                                            Message = "*H-hey! I would never use such thing!*";
                                            break;
                                        case GuardianBase.Vladimir:
                                            Message = "*How can you think of hugs at a moment like this?*";
                                            break;
                                        case GuardianBase.Malisha:
                                            Message = "*W-what the! What are you doing here? And who's that?*";
                                            break;
                                    }
                                }
                                SayMessage(Message);
                            }
                            ChangeScene(SceneIDs.MainGuardianSaysThatPlayerHasBeenHearingAllTheTime);
                            break;
                        case SceneIDs.MainGuardianSaysThatPlayerHasBeenHearingAllTheTime:
                            {
                                TerraGuardian Guardian = PlayerMod.GetPlayerMainGuardian(player);
                                string Message = "";
                                if (Guardian.ModID == MainMod.mod.Name)
                                {
                                    switch (Guardian.ID)
                                    {
                                        default:
                                            Message = Guardian.GetMessage(GuardianBase.MessageIDs.LeopoldMessage3, "*" + Guardian.Name + " also said that you heard everything he said.*");
                                            break;
                                        case GuardianBase.Rococo:
                                            Message = "*" + Guardian.Name + " is asking you what is his problem.*";
                                            break;
                                        case GuardianBase.Blue:
                                            Message = "*"+Guardian.Name+" explains that you're her friend, and that you can understand what they are talking about.*";
                                            break;
                                        case GuardianBase.Sardine:
                                            Message = "Yes, and It heard everything you said as clear as day.";
                                            break;
                                        case GuardianBase.Zacks:
                                            Message = "*That guy is making me sick, my boss isn't a troglodyte, do you hear?*";
                                            break;
                                        case GuardianBase.Alex:
                                            Message = "Yes, me and my friend here were watching you talking to self all that time.";
                                            break;
                                        case GuardianBase.Brutus:
                                            Message = "*I was hired by it to be his bodyguard, you fool.*";
                                            break;
                                        case GuardianBase.Bree:
                                            Message = "Of course you are, how can you think that the Terrarian is a fool?";
                                            break;
                                        case GuardianBase.Mabel:
                                            Message = "*Hey friend, I can't understand that guy, can you explain to me what is his problem?*";
                                            break;
                                        case GuardianBase.Domino:
                                            Message = "*Then why were you fooling yourself a while ago? Terrarians aren't stupid.*";
                                            break;
                                        case GuardianBase.Vladimir:
                                            Message = "*What moment like this, the Terrarian is my buddy. And can understand what we are talking.*";
                                            break;
                                        case GuardianBase.Malisha:
                                            Message = "*I moved to here for a vacation, then this Terrarian let me live here.*";
                                            break;
                                    }
                                }
                                Guardian.SaySomething(Message);
                            }
                            ChangeScene(SceneIDs.LeopoldGetsSurprisedThatPlayerHasBeenHearingAllTime);
                            break;
                        case SceneIDs.LeopoldGetsSurprisedThatPlayerHasBeenHearingAllTime:
                            SayMessage("*What?! That Terrarian can understand what we are saying?!*");
                            ChangeScene(SceneIDs.LeopoldTellsToForgetEverything);
                            break;
                        case SceneIDs.LeopoldTellsToForgetEverything:
                            SayMessage("*I nearly thought... Oh... Nevermind... Does It matter anyway?*");
                            ChangeScene(SceneIDs.LeopoldPresentsHimself);
                            break;
                        case SceneIDs.LeopoldPresentsHimself:
                            SayMessage("*I'm Leopold, the Sage. Please disconsider what I debated with myself earlier. If you can.*");
                            ChangeScene(SceneIDs.LeopoldFreeForRecruit);
                            break;

                        case SceneIDs.KnownPlayerSpottedByLeopold:
                            SayMessage("*Wait... I know that face...*");
                            ChangeScene(SceneIDs.LeopoldRecognizesTerrarian);
                            break;
                        case SceneIDs.LeopoldRecognizesTerrarian:
                            SayMessage("*Oh, It is that Terrarian again.*");
                            ChangeScene(SceneIDs.LeopoldGreetsPlayer);
                            break;
                        case SceneIDs.LeopoldGreetsPlayer:
                            SayMessage("*Hello, I didn't expected to see you here.*");
                            ChangeScene(SceneIDs.LeopoldTellsThatIsGoingToPlayerTown);
                            break;
                        case SceneIDs.LeopoldTellsThatIsGoingToPlayerTown:
                            SayMessage("*Since you're here, I think you have some town around this world, so... See you there.*");
                            ChangeScene(SceneIDs.LeopoldTurnsToTownNPC, true);
                            break;
                        case SceneIDs.LeopoldTurnsToTownNPC:
                            NpcMod.AddGuardianMet(10);
                            PlayerMod.AddPlayerGuardian(player, GuardianID, GuardianModID);
                            PlayerMod.GetPlayerGuardian(player, GuardianID, GuardianModID).IncreaseFriendshipProgress(1);
                            WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                            //npc.Transform(ModContent.NPCType<GuardianNPC.List.BunnyGuardian>());
                            break;

                            //////////////////////////////////
                        case SceneIDs.Flee:
                            {
                            }
                            break;
                    }
                }
                if (SceneID == SceneIDs.Flee)
                {
                    if (player.Center.X - npc.Center.X < 0)
                    {
                        MoveRight = true;
                    }
                    else
                    {
                        MoveLeft = true;
                    }
                    if (player.Distance(npc.Center) >= Main.screenWidth)
                    {
                        npc.active = false;
                        Main.NewText("The Bunny Guardian has escaped.", Microsoft.Xna.Framework.Color.Red);
                        return;
                    }
                }
                SceneTime--;
            }
            base.AI();
        }

        public override bool CheckActive()
        {
            return true;
        }

        public void ChangeScene(SceneIDs scene, bool Extended = false)
        {
            SceneID = scene;
            SceneTime = (Extended ? 600 : 180);
        }

        public override bool CanChat()
        {
            return EncounterAct || SocialAct || FearAct || ScareAct || SceneID == SceneIDs.LeopoldFreeForRecruit;
        }

        public override string GetChat()
        {
            NpcMod.AddGuardianMet(GuardianID, GuardianModID);
            bool PlayerHasLeopold = PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID);
            PlayerMod.AddPlayerGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID);
            if (!PlayerHasLeopold) PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID).IncreaseFriendshipProgress(1);
            string Mes = "";
            if (SceneID == SceneIDs.LeopoldFreeForRecruit)
            {
                Mes = "*Why did you make me act like a fool in front of your TerraGuardians? That will make it harder for people to remember me as a wise sage.*";
            }
            else if (SocialAct)
            {
                Mes = "*You can talk?! Wait, why didn't you talked to me sooner then? I nearly thought you were... No... Nevermind... I'm Leopold, the Sage.*";
            }
            else if (FearAct)
            {
                Mes = "*Huh? You're friendly? And can talk?! Oh... Thank... No... I'm fine.. It's just... Uh... Pleased to meet you, by the way... I'm Leopold, the Sage.*";
            }
            else if (ScareAct)
            {
                Mes = "*What?! You can talk!? Why did you made me pass through all that you idiot! Ugh... I'm Leopold, the Sage, by the way. Do you... Do you have some spare leaves with you...?*";
            }
            else
            {
                Mes = "*You can understand what I say?! Wow! The book was right!!*";
            }
            Main.npcChatText = Mes;
            WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
            //GuardianMouseOverAndDialogueInterface.SetDialogue(Mes);
            return Mes;
        }

        public static bool CanSpawnLeopold { get { return WorldMod.GuardiansMetCount >= 5 && !NpcMod.HasMetGuardian(10) && !NpcMod.HasGuardianNPC(10); } }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (!spawnInfo.water && CanSpawnLeopold && Main.dayTime && NpcMod.RecruitNpcSpawnConditionCheck(spawnInfo) && !Main.eclipse && Main.invasionSize <= 0 && !NPC.AnyNPCs(ModContent.NPCType<LeopoldNPC>()) ? 0.03125f : 0);
        }

        public enum SceneIDs
        {
            NoScene = -1,
            LeopoldSpotsThePlayer,
            LeopoldSaysNeverSawAnythingLikeThat,
            LeopoldQuestionsHimselfAboutCreature,
            IsItTerrarian,
            NoticesOutfit,
            QuestionsIfIsReallyATerrarian,
            NoticesPlayerLooking,
            WondersPlayerReaction,
            IsPreparingAttack,
            ThreatenUseSpell,
            FindsWeirdTheNoReaction,

            ThinksAboutTryingToTalk,
            WondersIftheySpeak,
            MentionsABook,
            ThinksAboutTrying,
            TriesTalking,
            WondersIfIsScared,
            SaysWontHurt,
            TriesHidingFear,
            NoticesDidntWorked,
            TriesGivingFood,
            WondersHowStupidHisActionWas,
            WaitsAFewMoments,
            ThinksThePlayerDidntUnderstandHim,
            TalksAboutWalkingAway,

            //Fear Branch
            GotScaredUponPlayerApproach,
            WondersIfPlayerWillAttackHim,
            FearPlayerAttack1,
            FearPlayerAttack2,
            FearPlayerAttack3,
            FearPlayerAttack4,
            FearPlayerAttack5,
            PlayerDoesNothing,
            PlayerDoesNothing2,
            WondersIfPlayerWillAttack,
            WondersIfScaredPlayer,
            ThreatensPlayer,
            RealizesHowStupidWhatHeSaidWas,
            SeesIfPlayerReacts,
            WonderIfAngeredPlayer,
            SeesIfPlayerReactsAgain,
            WonderIfPlayerIsFrozenInFear,
            SeesIfPlayerReactsAgainAgain,
            AttemptsToRunAway1,
            AttemptsToRunAway2,

            //Scare Branch
            FearsAboutPlayerAttackingHim,
            Crying1,
            Crying2,
            WaitingForReaction,
            AsksIfPlayerIsGoingToAttackHim,
            AsksIfPlayerUnderstandWhatHeSays,
            WaitingForReactionAgain,
            TellsThatIsgoingToFlee,
            RunsWhileScreaming,

            NoticesOtherGuardians,
            WondersWhyGuardiansFollowsPlayer,
            ThinksPlayerIsGuardiansPet,
            IgnoresTheAboveIdea,
            ThinksPlayerEnslavedGuardians,
            YellsThatIsGoingToSaveGuardians,
            WondersHowToSaveGuardians,
            PlayerMainGuardianTalksToLeopold,
            LeopoldAnswersTheGuardian,
            MainGuardianSaysThatPlayerHasBeenHearingAllTheTime,
            LeopoldGetsSurprisedThatPlayerHasBeenHearingAllTime,
            LeopoldTellsToForgetEverything,
            LeopoldPresentsHimself,
            LeopoldFreeForRecruit,

            KnownPlayerSpottedByLeopold,
            LeopoldRecognizesTerrarian,
            LeopoldGreetsPlayer,
            LeopoldTellsThatIsGoingToPlayerTown,
            LeopoldTurnsToTownNPC,

            Flee
        }
    }
}

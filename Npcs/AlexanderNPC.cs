using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    public class AlexanderNPC : GuardianActorNPC
    {
        private int NpcRecruitStep = IdleStep, DialogueDuration = 0;

        public AlexanderNPC() : base(GuardianBase.Alexander, "")
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = true;
            npc.friendly = true;
            npc.knockBackResist = 0;
            npc.dontCountMe = true;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
            npc.npcSlots = 10;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            switch (NpcRecruitStep)
            {
                case InvestigatingPlayerStep:
                    {
                        Player player = Main.player[npc.target];
                        if (player.GetModPlayer<PlayerMod>().KnockedOut)
                        {
                            BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.ReviveFrame;
                        }
                        else
                        {
                            int AnimationDuration = DialogueDuration % 180;
                            if (AnimationDuration >= 70)
                            {
                                if ((AnimationDuration >= 70 && AnimationDuration < 80) ||
                                    (AnimationDuration >= 90 && AnimationDuration < 100) ||
                                    (AnimationDuration >= 110 && AnimationDuration < 120) ||
                                    (AnimationDuration >= 130 && AnimationDuration < 140))
                                {
                                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 29;
                                }
                                else
                                {
                                    BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 28;
                                }
                            }
                            else
                            {
                                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 27;
                            }
                        }
                    }
                    break;
            }
        }

        public override void AI()
        {
            int NextStep = NpcRecruitStep;
            switch (NpcRecruitStep)
            {
                case IdleStep:
                    {
                        Idle = true;
                        Rectangle SightRange = new Rectangle((int)npc.Center.X, (int)npc.Center.Y, 300, 300);
                        SightRange.Y -= (int)(SightRange.Height * 0.5f);
                        if (npc.direction < 0)
                            SightRange.X -= SightRange.Width;
                        DialogueDuration++;
                        if(DialogueDuration >= 600)
                        {
                            DialogueDuration -= 600;
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    SayMessage("*Hm... It must be around here somewhere...*");
                                    break;
                                case 1:
                                    SayMessage("*Come on... Show up...*");
                                    break;
                                case 2:
                                    SayMessage("*...It's around here... I smell It.*");
                                    break;
                                case 3:
                                    SayMessage("*Terrarian, where are you...*");
                                    break;
                                case 4:
                                    SayMessage("*Once I catch that Terrarian.... Ugh...*");
                                    break;
                            }
                        }
                        for(int p = 0; p < 255; p++)
                        {
                            if(Main.player[p].active && !Main.player[p].dead)
                            {
                                if(Main.player[p].getRect().Intersects(SightRange) && Collision.CanHitLine(npc.position, npc.width, npc.height, Main.player[p].position, Main.player[p].width, Main.player[p].height))
                                {
                                    npc.target = p;
                                    NextStep = CallingOutPlayerStep;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case CallingOutPlayerStep:
                    {
                        Idle = false;
                        if(DialogueDuration == 0)
                        {
                            if(PlayerMod.PlayerHasGuardian(Main.player[npc.target], GuardianID, GuardianModID))
                            {
                                SayMessage("*Hey, Terrarian! Over here!*");
                            }
                            else if (Main.player[npc.target].GetModPlayer<PlayerMod>().KnockedOut)
                            {
                                SayMessage("*You there! Hang on!*");
                            }
                            else
                            {
                                SayMessage("*Hey! You there! Stop!*");
                            }
                        }
                        if(DialogueDuration >= 150)
                        {
                            NextStep = ChasingPlayerStep;
                        }
                        else
                        {
                            DialogueDuration++;
                        }
                        if (Main.player[npc.target].Center.X < npc.Center.X)
                            npc.direction = -1;
                        else
                            npc.direction = 1;
                        if (!Main.player[npc.target].active)
                        {
                            NextStep = IdleStep;
                            SayMessage("*Hm... " + (Main.player[npc.target].Male ? "He" : "She") + " disappeared...*");
                        }
                        else if (Main.player[npc.target].dead)
                        {
                            NextStep = IdleStep;
                            SayMessage("*Well... That's... Horrible...*");
                        }
                    }
                    break;
                case ChasingPlayerStep:
                    {
                        Player player = Main.player[npc.target];
                        if (player.Center.X < npc.Center.X)
                            MoveLeft = true;
                        else
                            MoveRight = true;
                        if (!player.GetModPlayer<PlayerMod>().KnockedOut && npc.velocity.Y == 0 && player.velocity.Y != 0)
                        {
                            Jump = true;
                        }
                        if (PlayerMod.PlayerHasGuardian(Main.player[npc.target], GuardianID, GuardianModID) && Math.Abs(player.Center.X - npc.Center.X) < 80)
                        {
                            NextStep = SpeakingToAlreadyKnownPlayer;
                        }
                        else if (player.getRect().Intersects(npc.getRect()))
                        {
                            if (PlayerMod.PlayerHasGuardian(Main.player[npc.target], GuardianID, GuardianModID))
                            {
                                NextStep = SpeakingToAlreadyKnownPlayer;
                            }
                            else
                            {
                                NextStep = InvestigatingPlayerStep;
                            }
                        }
                        if (!player.active)
                        {
                            NextStep = IdleStep;
                            SayMessage("*Hm... " + (Main.player[npc.target].Male ? "He" : "She") + " disappeared...*");
                        }
                        else if (player.dead)
                        {
                            NextStep = IdleStep;
                            SayMessage("*Damn, I'm too late...*");
                        }
                    }
                    break;
                case InvestigatingPlayerStep:
                    {
                        if (DialogueDuration == 0)
                            FindFrame(0);
                        Player player = Main.player[npc.target];
                        player.immuneTime = 90;
                        player.immuneNoBlink = true;
                        Vector2 PlayerPosition = new Vector2(npc.Center.X + 32 * npc.direction, npc.position.Y + npc.height);
                        player.fullRotationOrigin = new Vector2(player.width, player.height) * 0.5f;
                        player.aggro = -99999;
                        if (npc.direction > 0)
                        {
                            player.fullRotation = 1.570796f;
                            player.direction = -1;
                        }
                        else
                        {
                            player.fullRotation = -1.570796f;
                            //PlayerPosition.X *= -1;
                            player.direction = 1;
                        }
                        DrawInFrontOfPlayers.Add(npc.whoAmI);
                        player.velocity = Vector2.Zero;
                        player.Center = PlayerPosition;
                        player.statLife++;
                        if (player.mount.Active)
                            player.mount.Dismount(player);
                        player.gfxOffY = -2;
                        player.AddBuff(Terraria.ID.BuffID.Cursed, 5);
                        if(npc.velocity.X != 0 || npc.velocity.Y != 0)
                        {

                        }
                        else if (player.GetModPlayer<PlayerMod>().KnockedOut)
                        {
                            if (DialogueDuration == 0)
                            {
                                SayMessage("*Better I take care of those wounds first.*");
                                DialogueDuration++;
                            }
                            player.GetModPlayer<PlayerMod>().ReviveBoost++;
                        }
                        else
                        {
                            if (DialogueDuration % 180 == 0)
                            {
                                switch(DialogueDuration / 180)
                                {
                                    case 0:
                                        SayMessage("*Got you! Now don't move.*");
                                        break;
                                    case 1:
                                        SayMessage("*Hm...*");
                                        break;
                                    case 2:
                                        SayMessage("*Interesting...*");
                                        break;
                                    case 3:
                                        SayMessage("*Uh huh...*");
                                        break;
                                    case 4:
                                        SayMessage("*But you're not the one I'm looking for...*");
                                        break;
                                    case 5:
                                        NextStep = IntroductingToPlayerStep;
                                        player.fullRotation = 0;
                                        player.fullRotationOrigin = new Vector2(40, 56) * 0.5f;
                                        player.position = npc.Bottom;
                                        player.position.Y -= player.height;
                                        player.immuneNoBlink = false;
                                        break;
                                }
                            }
                            DialogueDuration++;
                        }
                        if (!player.active)
                        {
                            NextStep = IdleStep;
                            SayMessage("*" + (Main.player[npc.target].Male ? "He" : "She") + " disappeared!*");
                        }
                    }
                    break;

                case IntroductingToPlayerStep:
                    {
                        if(DialogueDuration == 0)
                        {
                            Main.player[npc.target].talkNPC = npc.whoAmI;
                            DialogueDuration = 1;
                            Main.npcChatText = GetChat();
                        }
                        if (!Main.player[npc.target].active)
                        {
                            npc.TargetClosest(false);
                            DialogueDuration = 1;
                        }
                        if(DialogueDuration > 1 && Main.player[npc.target].talkNPC != npc.whoAmI)
                        {
                            DialogueDuration = 1;
                        }
                    }
                    break;
                case SpeakingToAlreadyKnownPlayer:
                    {
                        if(DialogueDuration % 180 == 0)
                        {
                            switch(DialogueDuration / 180)
                            {
                                case 0:
                                    SayMessage("*I didn't expected to see you here.*");
                                    break;
                                case 1:
                                    SayMessage("*I think I may have caught that Terrarian's scent around here.*");
                                    break;
                                case 2:
                                    SayMessage("*Or maybe I accidentally caught your scent again.*");
                                    break;
                                case 3:
                                    SayMessage("*Anyway, If you need me, I'll be around.*");
                                    break;
                                case 4:
                                    {
                                        NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                                        WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                                    }
                                    return;
                            }
                        }
                        if (Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) >= 30)
                        {
                            Walk = true;
                            if (Main.player[npc.target].Center.X < npc.Center.X)
                                MoveLeft = true;
                            else
                                MoveRight = true;
                        }
                        else
                        {
                            if (Main.player[npc.target].Center.X < npc.Center.X)
                                npc.direction = -1;
                            else
                                npc.direction = 1;
                        }
                        if (!Main.player[npc.target].active)
                        {
                            NextStep = IdleStep;
                            SayMessage("*" + (Main.player[npc.target].Male ? "He" : "She") + " disappeared!*");
                        }
                        DialogueDuration++;
                    }
                    break;
            }
            if(NextStep != NpcRecruitStep)
            {
                NpcRecruitStep = NextStep;
                DialogueDuration = 0;
            }
            base.AI();
        }

        public override bool CanChat()
        {
            return NpcRecruitStep == IntroductingToPlayerStep;
        }

        public override string GetChat()
        {
            switch (DialogueDuration)
            {
                case DialogueFirstTalk:
                    {
                        bool HasMetTerraGuardians = false;
                        foreach (GuardianData gd in Main.player[npc.target].GetModPlayer<PlayerMod>().MyGuardians.Values)
                        {
                            if (gd.Base.IsTerraGuardian)
                            {
                                HasMetTerraGuardians = true;
                                break;
                            }
                        }
                        return "*Terrarian, Human, " + (Main.player[npc.target].Male ? "Male" : "Female") + ", " + (HasMetTerraGuardians ? "and you seems to have met some TerraGuardians.*" : "and It must be a shock for you to see me.*");
                    }
                case DialogueQuestionJump:
                    return "*I had to make sure if you were the person I am looking for, and you could simply have tried to run away.*";
                case DialogueTellThatCouldHaveAskedInstead:
                    return "*That's not what I wanted to know, It wouldn't be necessary to catch you to know part of that.*";
                case DialogueAlexanderTellsReason:
                    return "*There is a Terrarian that seems to be involved in the disappearance of my friends.*";
                case DialogueAskHowCouldBeATerrarian:
                    return "*After tracking so many creatures, I learned how to distinguish scent, but my sleuthing isn't very accurate, which may explains why I caught you.*";
                case DialogueAskHowCouldBeRelatedToFriendsDisappearance:
                    return "*I caught their scent at the last place I smelled the scent of my friends. Every creature has a particularly different scent, but I accidentally ended up finding you while tracking one.*";
                case DialogueAskWhyHeCaughtYourScent:
                    return "*I'm not sure, either I caught your scent because you're a Terrarian too, or that Terrarian has appeared in your world.*";
                case DialogueTellHimToLookAround:
                    return "*That's what I intend to do, but I have a request for you: Would you mind if I hang around your world for a while to look for clues?*";
                case DialogueTellHimToHaveGoodLuck:
                    return "*I thank you for that, but I have a request for you: Would you mind if I hang around your world for a while to look for clues?*";
                case DialogueAcceptHimMovingIn:
                    return "*Thank you, Terrarian. My name is " + Base.Name + ", If you find a suspicious Terrarian, bring him to me.*";
                case DialogueRejectHimMovingIn:
                    return "*I see... Anyway, my name is " + Base.Name + ". If you change your mind, you just need to call me. I'll visit your world regularly looking for clues. Until another time.*";
            }
            return "**";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            switch (DialogueDuration)
            {
                case DialogueFirstTalk:
                    button = "Why did you jumped on me?";
                    button2 = "You could have just asked instead.";
                    break;
                case DialogueQuestionJump:
                    button = "Who are you looking for?";
                    break;
                case DialogueTellThatCouldHaveAskedInstead:
                    button = "Then what was all that for?";
                    break;
                case DialogueAlexanderTellsReason:
                    button = "How are you sure It's a Terrarian?";
                    button2 = "Why would It be involved?";
                    break;
                case DialogueAskHowCouldBeATerrarian:
                case DialogueAskHowCouldBeRelatedToFriendsDisappearance:
                    button = "How could you possibly mistake me with that Terrarian?";
                    break;
                case DialogueAskWhyHeCaughtYourScent:
                    button = "Why don't you take a look around?";
                    button2 = "Good luck on your search";
                    break;
                case DialogueTellHimToLookAround:
                case DialogueTellHimToHaveGoodLuck:
                    button = "Yes, you may.";
                    button2 = "No, you may not.";
                    break;
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            switch (DialogueDuration)
            {
                case DialogueFirstTalk:
                    if (firstButton)
                        DialogueDuration = DialogueQuestionJump;
                    else
                        DialogueDuration = DialogueTellThatCouldHaveAskedInstead;
                    break;
                case DialogueQuestionJump:
                    DialogueDuration = DialogueAlexanderTellsReason;
                    break;
                case DialogueTellThatCouldHaveAskedInstead:
                        DialogueDuration = DialogueQuestionJump;
                    break;
                case DialogueAlexanderTellsReason:
                    if (firstButton)
                        DialogueDuration = DialogueAskHowCouldBeATerrarian;
                    else
                        DialogueDuration = DialogueAskHowCouldBeRelatedToFriendsDisappearance;
                    break;
                case DialogueAskWhyHeCaughtYourScent:
                    if (firstButton)
                        DialogueDuration = DialogueTellHimToLookAround;
                    else
                        DialogueDuration = DialogueTellHimToHaveGoodLuck;
                    break;
                case DialogueTellHimToLookAround:
                case DialogueTellHimToHaveGoodLuck:
                    if (firstButton)
                        DialogueDuration = DialogueAcceptHimMovingIn;
                    else
                        DialogueDuration = DialogueRejectHimMovingIn;
                    Main.npcChatText = GetChat();
                    TurnNpcInTownNpc(firstButton);
                    return;
                case DialogueAskHowCouldBeATerrarian:
                case DialogueAskHowCouldBeRelatedToFriendsDisappearance:
                    DialogueDuration = DialogueAskWhyHeCaughtYourScent;
                    break;
            }
            Main.npcChatText = GetChat();
        }

        public static bool AlexanderConditionMet
        {
            get { return NPC.downedBoss3; }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.water && !NpcMod.HasGuardianNPC(GuardianBase.Alexander) && !NpcMod.HasMetGuardian(GuardianBase.Alexander) && AlexanderConditionMet && 
                spawnInfo.player.ZoneDungeon && !NPC.AnyNPCs(ModContent.NPCType<AlexanderNPC>()))
            {
                return 1f / 500;
            }
            return 0;
        }

        public void TurnNpcInTownNpc(bool MoveIn)
        {
            PlayerMod.AddPlayerGuardian(Main.player[npc.target], GuardianID, GuardianModID);
            NpcMod.AddGuardianMet(GuardianID, GuardianModID, MoveIn);
            if (PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID).FriendshipLevel == 0)
                PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID).IncreaseFriendshipProgress(1);
            WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
        }

        private const byte IdleStep = 0,
            CallingOutPlayerStep = 1,
            ChasingPlayerStep = 2,
            InvestigatingPlayerStep = 3,
            IntroductingToPlayerStep = 4,
            SpeakingToAlreadyKnownPlayer = 5;
        private const byte DialogueFirstTalk = 1,
            DialogueQuestionJump = 2,
            DialogueTellThatCouldHaveAskedInstead = 3,
            DialogueAlexanderTellsReason = 4,
            DialogueAskHowCouldBeATerrarian = 5,
            DialogueAskHowCouldBeRelatedToFriendsDisappearance = 6,
            DialogueAskWhyHeConfusedYourCharacterScentToThatTerrariansScent = 7,
            DialogueAskWhyHeCaughtYourScent = 8,
            DialogueTellHimToLookAround = 9,
            DialogueTellHimToHaveGoodLuck = 10,
            DialogueAcceptHimMovingIn = 11,
            DialogueRejectHimMovingIn = 12;
    }
}

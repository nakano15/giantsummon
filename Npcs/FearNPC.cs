using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    class FearNPC : GuardianActorNPC
    {
        public byte RunTime = 0, ScareCooldown = 0;
        private static byte DialogueStep = 0;
        private bool MetPlayer = false;
        private static byte SpeakingToKnownPlayer = 0;

        public FearNPC() : base(GuardianBase.Fear)
        {

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scared TerraGuardian");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.friendly = true;
            npc.townNPC = false;
            npc.dontTakeDamageFromHostiles = false;
        }

        public override void AI()
        {
            bool FoundThreat = false, ThreatIsKnownPlayer = false;
            bool PlayerChattingToThem = false;
            float ThreatDistance = 370;
            {
                float MyCenterX = npc.Center.X;
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead)
                    {
                        if(Main.player[i].talkNPC == npc.whoAmI)
                        {
                            PlayerChattingToThem = true;
                            break;
                        }
                        float PositionX = Main.player[i].Center.X;
                        if(!MetPlayer && Math.Abs(PositionX - MyCenterX) < Math.Abs(ThreatDistance) && Collision.CanHitLine(npc.position, npc.width, npc.height, Main.player[i].position, Main.player[i].width, Main.player[i].height))
                        {
                            ThreatDistance = PositionX - MyCenterX;
                            FoundThreat = true;
                            ThreatIsKnownPlayer = PlayerMod.PlayerHasGuardian(Main.player[i], GuardianBase.Fear);
                            npc.direction = ThreatDistance <= 0 ? 1 : -1;
                        }
                    }
                    if(i < 200 && i != npc.whoAmI && Main.npc[i].active)
                    {
                        float PositionX = Main.npc[i].Center.X;
                        if (Math.Abs(PositionX - MyCenterX) < Math.Abs(ThreatDistance) && Collision.CanHitLine(npc.position, npc.width, npc.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
                        {
                            ThreatDistance = PositionX - MyCenterX;
                            FoundThreat = true;
                            ThreatIsKnownPlayer = true;
                            npc.direction = ThreatDistance <= 0 ? 1 : -1;
                        }
                    }
                }
            }
            if (PlayerChattingToThem)
            {
                RunTime = ScareCooldown = 0;
            }
            else
            {
                DialogueStep = 0;
                if (FoundThreat)
                {
                    if (RunTime == 0)
                    {
                        ScareCooldown = 8;
                        npc.velocity.Y = -2f;
                        if (ThreatIsKnownPlayer)
                        {
                            switch (Main.rand.Next(3))
                            {
                                default:
                                    SayMessage("*Yikes! W-Who's there?! Do I know you?*");
                                    break;
                                case 1:
                                    SayMessage("*Aahh!! It's... It's you, again!*");
                                    break;
                                case 2:
                                    SayMessage("*Wha... You.. Just don't come closer!*");
                                    break;
                            }
                        }
                        else
                        {
                            switch (Main.rand.Next(3))
                            {
                                default:
                                    SayMessage("*Aaahhh!! Something approaches!!*");
                                    break;
                                case 1:
                                    SayMessage("*Get away from me!!*");
                                    break;
                                case 2:
                                    SayMessage("*Don't come closer!!*");
                                    break;
                            }
                        }
                    }
                    RunTime = 30;
                }
                if (RunTime > 0)
                {
                    Idle = false;
                    if (ScareCooldown > 0)
                        ScareCooldown--;
                    else
                    {
                        if (FoundThreat)
                        {
                            if (ThreatDistance <= 0)
                                MoveRight = true;
                            else
                                MoveLeft = true;
                        }
                        RunTime--;
                        if (RunTime == 0)
                        {
                            switch (Main.rand.Next(3))
                            {
                                default:
                                    SayMessage("*I think I'm safe now... Phew..*");
                                    break;
                                case 1:
                                    SayMessage("*I lost them... Good..*");
                                    break;
                                case 2:
                                    SayMessage("*This place is so scary! How did I got here?*");
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    Idle = true;
                }
            }
            base.AI();
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            bool CloudForm = Main.LocalPlayer.GetModPlayer<PlayerMod>().PigGuardianCloudForm[Creatures.PigGuardianFragmentBase.FearPigGuardianID];
            if (CloudForm)
            {
                switch (BodyAnimationFrame)
                {
                    case 0:
                    case 9:
                        BodyAnimationFrame = 19;
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        if((npc.direction > 0 && npc.velocity.X > 0) || (npc.direction < 0 && npc.velocity.X < 0))
                            BodyAnimationFrame = 20;
                        else
                            BodyAnimationFrame = 21;
                        break;
                    //case 14:
                    //    BodyAnimationFrame = 22;
                    //    break;
                }
            }
        }

        public override bool CanChat()
        {
            return true;
        }

        public override string GetChat()
        {
            SpeakingToKnownPlayer = (byte)(PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Fear) ? 1 : 0);
            if (MetPlayer)
                return "*Yikes!! Stop that! Don't scare me! Go away!*";
            if (SpeakingToKnownPlayer == 1)
            {
                if (PlayerMod.GetPlayerGuardian(Main.LocalPlayer, GuardianBase.Fear).FriendshipLevel >= 5)
                {
                    SpeakingToKnownPlayer = 2;
                    return "*I'm happy to see you here, beside I don't fully trust you. Go away!*";
                }
                return "*Hey! Don't come too close! I still don't trust you!*";
            }
            return "*PLEASE! DON'T HURT ME!*";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (MetPlayer)
            {
                return;
            }
            if(SpeakingToKnownPlayer > 0)
            {
                button = "Ok, I will be leaving then.";
                button2 = "Ok, bye. And don't follow me.";
                return;
            }
            switch (DialogueStep)
            {
                case 0:
                    button = "I will not harm you.";
                    break;
                case 1:
                    button = "I'm talking to you instead of attacking.";
                    break;
                case 2:
                    button = "Whatever, I'm leaving now.";
                    button2 = "I have to go, and don't follow me.";
                    break;
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (MetPlayer)
                return;
            if(SpeakingToKnownPlayer > 0)
            {
                if (firstButton)
                {
                    if (SpeakingToKnownPlayer == 1)
                    {
                        Main.npcChatText = "*Huh.*";
                        SayMessage("(I'll follow them again by keeping some distance, maybe they know the way out.)");
                    }
                    else
                    {
                        Main.npcChatText = "*No, I actually lied. Please, let me go with you! Get me out of here!!*";
                    }
                    PlayerMod.AddPlayerGuardian(Main.LocalPlayer, GuardianBase.Fear);
                    NpcMod.AddGuardianMet(GuardianBase.Fear);
                    WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                }
                else
                {
                    Main.npcChatText = "*Uh.. Alright... Just- Just go away!*";
                    MetPlayer = true;
                }
            }
            switch (DialogueStep)
            {
                case 0:
                    DialogueStep++;
                    Main.npcChatText = "*I don't trust you! You will try killing me like you did to those scary monsters!*";
                    break;
                case 1:
                    DialogueStep++;
                    Main.npcChatText = "*Uh.. I guess you're right. BUT! Doesn't mean you can't still turn on me.*";
                    break;
                case 2:
                    DialogueStep++;
                    if (firstButton)
                    {
                        Main.npcChatText = "*I, ah... Okay.*";
                        SayMessage("(I'll trail behind them carefully for now, maybe they will help me get out of this horrible place.)");
                        PlayerMod.AddPlayerGuardian(Main.LocalPlayer, GuardianBase.Fear);
                        NpcMod.AddGuardianMet(GuardianBase.Fear);
                        WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                    }
                    else
                    {
                        Main.npcChatText = "*Uh.. Alright... Just- Just go away!*";
                        MetPlayer = true;
                    }
                    break;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(spawnInfo.player.ZoneDungeon && !spawnInfo.water && !NpcMod.HasGuardianNPC(GuardianID, GuardianModID) && !NpcMod.HasMetGuardian(GuardianID, GuardianModID))
            {
                return 1f / 200;
            }
            return 0;
        }
    }
}

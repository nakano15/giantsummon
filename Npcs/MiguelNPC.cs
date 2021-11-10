using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    class MiguelNPC : GuardianActorNPC
    {
        public byte DialogueStep = 0, JumpTimes = 0;
        public bool PlayerMovedAway = false;
        public bool PlayerHasMiguelRecruited = false;

        public MiguelNPC() : base(GuardianBase.Miguel)
        {

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Horse Guardian");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = false;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
        }

        public override void AI()
        {
            if (DialogueStep > 0)
            {
                if (!Main.player[npc.target].active)
                {
                    SayMessage("*And " + (Main.player[npc.target].Male ? "he's" : "she's") + " gone. Oh well, until we meet again.*");
                    DialogueStep = 0;
                }
                else if (Main.player[npc.target].dead)
                {
                    SayMessage("*That was really horrible!*");
                    if (DialogueStep >= 5)
                        DialogueStep = 5;
                    else
                        DialogueStep = 0;
                }
                else if (npc.velocity.X == 0 && npc.velocity.Y == 0)
                {
                    if (DialogueStep >= 5)
                    {
                        if (!PlayerMovedAway)
                        {
                            if(Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) >= 400 ||
                                Math.Abs(Main.player[npc.target].Center.Y - npc.Center.Y) >= 300)
                            {
                                PlayerMovedAway = true;
                                SayMessage("*And "+ (Main.player[npc.target].Male ? "he" : "she")+" went away.*");
                                DialogueStep = 5;
                            }
                        }
                        else
                        {
                            npc.TargetClosest(false);
                            if (Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) < 200 &&
                                Math.Abs(Main.player[npc.target].Center.Y - npc.Center.Y) < 100)
                            {
                                PlayerMovedAway = false;
                                SayMessage("*Returned? Now you can try jumping "+(25 - JumpTimes)+", right?*");
                                DialogueStep = 6;
                            }
                        }
                    }
                    if (Main.player[npc.target].Center.X < npc.Center.X)
                        npc.direction = -1;
                    else
                        npc.direction = 1;
                }
            }
            switch (DialogueStep)
            {
                case 0: //Looking for Player
                    npc.TargetClosest(false);
                    Idle = true;
                    Rectangle SeekingRectangle = new Rectangle((int)npc.Center.X, (int)npc.Center.Y, 400, 300);
                    if (npc.direction < 0)
                        SeekingRectangle.X -= SeekingRectangle.Width;
                    SeekingRectangle.Y -= (int)(SeekingRectangle.Height * 0.5f);
                    if (Main.player[npc.target].active && !Main.player[npc.target].dead && Main.player[npc.target].getRect().Intersects(SeekingRectangle))
                    {
                        Idle = false;
                        DialogueStep = 1;
                        SayMessage("*Hey, you!*", true);
                        PlayerHasMiguelRecruited = PlayerMod.PlayerHasGuardian(Main.player[npc.target], GuardianID);
                    }
                    break;
                case 1:
                    {
                        if (MessageTime <= 0)
                        {
                            if (PlayerHasMiguelRecruited)
                            {
                                SayMessage("*Don't you go thinking that just because you changed worlds, you'll slack on the exercises!*", true);
                            }
                            else
                            {
                                SayMessage("*Are you carrying many loots in that pouch of yours?*", true);
                            }
                            DialogueStep = 2;
                        }
                    }
                    break;
                case 2:
                    {
                        if (MessageTime <= 0)
                        {
                            if (PlayerHasMiguelRecruited)
                            {
                                SayMessage("*I will be here to give you a new batch of exercises.*", true);
                                NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                                WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                            }
                            else
                            {
                                SayMessage("*Or is that because you haven't been doing abdominal exercises?*", true);
                            }
                            DialogueStep = 3;
                        }
                    }
                    break;
                case 3:
                    {
                        if (MessageTime <= 0)
                        {
                            SayMessage("*Hahahaha.*");
                            DialogueStep = 4;
                        }
                    }
                    break;
                case 4:
                    {
                        if (MessageTime <= 0)
                        {
                            SayMessage("*Don't worry, though, I will help you take carry of that belly.*", true);
                            DialogueStep = 5;
                        }
                    }
                    break;
                case 5:
                    {
                        if (!PlayerMovedAway && MessageTime <= 0 && Main.player[npc.target].talkNPC != npc.whoAmI)
                        {
                            SayMessage("*Why don't you try jumping 25 times.*", true);
                            DialogueStep = 6;
                        }
                    }
                    break;
                case 6:
                    {
                        if (!PlayerMovedAway && Main.player[npc.target].justJumped)
                        {
                            JumpTimes++;
                            if (JumpTimes >= 25)
                            {
                                SayMessage("*Nice job. As you can see, you got a little more fit for jumping all those times.*");
                                Main.player[npc.target].AddBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.Fit>(), 5 * 60 * 60);
                                DialogueStep++;
                            }
                            else
                            {
                                if (JumpTimes % 5 == 0)
                                {
                                    if (JumpTimes > 10)
                                    {
                                        SayMessage("*" + JumpTimes + "! Good job!*");
                                    }
                                    else
                                    {
                                        SayMessage("*" + JumpTimes + "! Nice!*");
                                    }
                                }
                                else
                                    SayMessage("*" + JumpTimes + "!*");
                            }
                        }
                    }
                    break;
                case 7:
                    {
                        if (MessageTime <= 0)
                        {
                            SayMessage("*Don't worry, by the way. The great "+Base.Name+" will handle your training from now on.*");
                            DialogueStep++;
                        }
                    }
                    break;
                case 8:
                    {
                        if (MessageTime <= 0)
                        {
                            SayMessage("*And don't you dare to deny my training.*");
                            PlayerMod.AddPlayerGuardian(Main.player[npc.target], GuardianID, GuardianModID);
                            NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                            WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                            PlayerMod.GetPlayerGuardian(Main.player[npc.target], GuardianID, GuardianModID).IncreaseFriendshipProgress(1);
                            return;
                        }
                    }
                    break;
            }
            base.AI();
        }

        public override bool CanChat()
        {
            return DialogueStep == 5 || DialogueStep == 6;
        }

        public override string GetChat()
        {
            switch (DialogueStep)
            {
                case 5:
                    return "*Your parents didn't teached you manners? If one person is speaking, wait until they end before talking.*";
                case 6:
                    return "*You forgot what I told you? I told you to jump "+(25 - JumpTimes)+" times.*";
            }
            return "*What? I have no idea why you are able to talk to me right now. But, well... How's the weather?*";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            switch (DialogueStep)
            {
                case 5:
                    button = "Do you always insult new people you meet?";
                    button2 = "Hey! That wasn't nice.";
                    break;
                case 6:
                    button = "Why?";
                    button2 = "I do whatever I want.";
                    break;
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            switch (DialogueStep)
            {
                case 5:
                    if (firstButton)
                    {
                        Main.npcChatText = "*I only do that to people who have been eating more than exercising their body.*";
                    }
                    else
                    {
                        Main.npcChatText = "*So, that wasn't an extra pouch? Then you really need abdominal exercises.*";
                    }
                    break;
                case 6:
                    if (firstButton)
                    {
                        Main.npcChatText = "*Just do it. It's not complicated for you, right Terrarian?*";
                    }
                    else
                    {
                        Main.npcChatText = "*What about doing something I want for once? You'll understand why I told you to jump a few times.*";
                    }
                    break;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            if(npc.velocity.X == 0 && npc.velocity.Y == 0)
            {
                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = 0;
            }
            if(DialogueStep == 1)
            {
                LeftArmAnimationFrame = Base.ItemUseFrames[2];
            }
        }

        public static bool CanSpawnMe()
        {
            return NPC.downedGoblins;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(CanSpawnMe() && !NpcMod.HasGuardianNPC(GuardianID) && !NpcMod.HasMetGuardian(GuardianID) && Main.dayTime && Main.invasionSize == 0 && !Main.eclipse && 
                spawnInfo.player.ZoneOverworldHeight && !Main.slimeRain)
            {
                return 1f / 200;
            }
            return 0;
        }
    }
}

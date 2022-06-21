using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon.Npcs
{
    public class GlennNPC : GuardianActorNPC
    {
        public bool HasSeenHisParents = false, PlayerHasMetGlenn = false;
        public byte DialogueStep = 0;
        public int Delay = 0;

        public GlennNPC() : base(GuardianBase.Glenn, "")
        {

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black and White Cat");
        }

        public override void SetDefaults()
        {
            npc.townNPC = false;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
        }

        public override void AI()
        {
            Idle = true;
            if(DialogueStep == 0)
            {
                Rectangle VisionPosition = new Rectangle(0, 0, 400, 300);
                VisionPosition.X = (int)npc.Center.X;
                VisionPosition.Y = (int)(npc.Center.Y - VisionPosition.Height * 0.5f);
                if (npc.direction < 0)
                    VisionPosition.X -= VisionPosition.Width;
                for(int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && !Main.player[p].dead && Main.player[p].getRect().Intersects(VisionPosition))
                    {
                        npc.target = p;
                        DialogueStep = 1;
                        PlayerHasMetGlenn = PlayerMod.PlayerHasGuardian(Main.player[p], GuardianID);
                        StareAt(Main.player[p]);
                        break;
                    }
                }
            }
            else
            {
                if(npc.target < 0 || npc.target > 255)
                {
                    DialogueStep = 0;
                    SayMessage("...");
                }
                else if (!Main.player[npc.target].active)
                {
                    DialogueStep = 0;
                    SayMessage((Main.player[npc.target].Male ? "He's" : "She's") + " gone. How?");
                }
                else if (Main.player[npc.target].dead)
                {
                    DialogueStep = 0;
                    SayMessage("I really didn't wanted to witness that.");
                }
                else
                {
                    Player player = Main.player[npc.target];
                    Idle = false;
                    bool HasBree = PlayerMod.PlayerHasGuardianSummoned(player, GuardianBase.Bree),
                        HasSardine = PlayerMod.PlayerHasGuardianSummoned(player, GuardianBase.Sardine),
                        HasZacks = PlayerMod.PlayerHasGuardianSummoned(player, GuardianBase.Zacks);
                    if (HasSardine)
                        PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine).LookAt(npc.Center);
                    if (HasBree)
                        PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree).LookAt(npc.Center);
                    if (HasZacks && (!HasBree && !HasSardine))
                        PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Zacks).LookAt(npc.Center);
                    if (Delay > 0)
                        Delay--;
                    else
                    {
                        switch (DialogueStep)
                        {
                            case 1:
                                {
                                    if (PlayerHasMetGlenn)
                                    {
                                        StareAt(player);
                                        Delay = SayMessage("Hey Terrarian, over here!");
                                    }
                                    else if (HasBree && HasSardine)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                            StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree));
                                        else
                                            StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine));
                                        Delay = SayMessage("Mom! Dad! I'm glad you're alright.");
                                    }
                                    else if (HasBree)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree));
                                        Delay = SayMessage("Mom! I'm glad you're safe, but... Where's dad?");
                                    }
                                    else if (HasSardine)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine));
                                        Delay = SayMessage("Dad! I'm glad you're safe, but... Where's mom?");
                                    }
                                    else
                                    {
                                        StareAt(player);
                                        Delay = SayMessage("Hey, Terrarian. Can you help me?");
                                    }
                                    DialogueStep = 2;
                                }
                                break;
                            case 2:
                                {
                                    if (PlayerHasMetGlenn)
                                    {
                                        StareAt(player);
                                        Delay = SayMessage("Since you moved out of the world, I was trying to look for where my house is at.");
                                    }
                                    else if (HasBree && HasSardine)
                                    {
                                        PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree).SaySomething("Glenn!!");
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine).SaySomething("Glenn, what are you doing here?");
                                    }
                                    else if (HasBree)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree));
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree).SaySomething("Glenn!! I'm glad you're safe. You didn't found your dad, either?");
                                    }
                                    else if (HasSardine)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine));
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine).SaySomething("Glenn, what are you doing here? And what you're saying about your mom? Wasn't she with you?");
                                    }
                                    else
                                    {
                                        if (Math.Abs(player.Center.X - npc.Center.X) >= 600 ||
                                            Math.Abs(player.Center.Y - npc.Center.Y) >= 400)
                                        {
                                            Delay = SayMessage("And " + (player.Male ? "he" : "she") + "'s gone....");
                                            DialogueStep = 0;
                                            //npc.target = -1;
                                        }
                                        else
                                        {
                                            StareAt(player);
                                        }
                                        break;
                                    }
                                    DialogueStep = 3;
                                }
                                break;
                            case 3:
                                {
                                    if (PlayerHasMetGlenn)
                                    {
                                        StareAt(player);
                                        Delay = SayMessage("So far, I didn't had any luck with that...");
                                    }
                                    else if (HasBree && HasSardine)
                                    {
                                        Delay = SayMessage("I came here looking for you.");
                                    }
                                    else if (HasBree)
                                    {
                                        Delay = SayMessage("No, I didn't. I came here looking for both of you.");
                                    }
                                    else if (HasSardine)
                                    {
                                        Delay = SayMessage("She came looking for you, but It has been a long time. I'm glad that I found you.");
                                    }
                                    else if (HasZacks) //Deactivated for now
                                    {
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Zacks).SaySomething(
                                            "**");
                                    }
                                    else
                                    {
                                        Delay = SayMessage("Wait, where did they go?");
                                        DialogueStep = 2;
                                        break;
                                    }
                                    DialogueStep = 4;
                                }
                                break;
                            case 4:
                                {
                                    if (PlayerHasMetGlenn)
                                    {
                                        StareAt(player);
                                        Delay = SayMessage("Anyways, I'm here, if you need me.");
                                        NpcMod.AddGuardianMet(GuardianID);
                                        PlayerMod.AddPlayerGuardian(player, GuardianID);
                                        WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID);
                                        return;
                                    }
                                    else if (HasBree)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree));
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree).SaySomething("But, It's too dangerous for you to be travelling alone.");
                                    }
                                    else if (HasSardine)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine));
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine).SaySomething("You should have waited for us at home.");
                                    }
                                    else
                                    {
                                        Delay = SayMessage("Wait, where did they go?");
                                        DialogueStep = 2;
                                        break;
                                    }
                                    DialogueStep = 5;
                                }
                                break;
                            case 5:
                                {
                                    if (HasBree || HasSardine)
                                    {
                                        Delay = SayMessage("I know... But it has been a long time since I last saw you two.");
                                    }
                                    else
                                    {
                                        Delay = SayMessage("Wait, where did they go?");
                                        DialogueStep = 2;
                                        break;
                                    }
                                    DialogueStep = 6;
                                }
                                break;
                            case 6:
                                {
                                    if (HasSardine)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine));
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine).SaySomething("At least you managed to get here safe. So, do you remember which world we came from?");
                                    }
                                    else if (HasBree)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree));
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree).SaySomething("I feel a bit better for seeing you again... Do you know our way back home?");
                                    }
                                    else
                                    {
                                        Delay = SayMessage("Wait, where did they go?");
                                        DialogueStep = 2;
                                        break;
                                    }
                                    DialogueStep = 7;
                                }
                                break;
                            case 7:
                                {
                                    if (HasSardine || HasBree)
                                    {
                                        Delay = SayMessage("I... Forgot...");
                                    }
                                    else
                                    {
                                        Delay = SayMessage("Wait, where did they go?");
                                        DialogueStep = 2;
                                        break;
                                    }
                                    DialogueStep = 8;
                                }
                                break;
                            case 8:
                                {
                                    if (HasBree)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree));
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree).SaySomething("...");
                                    }
                                    else if (HasSardine)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine));
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine).SaySomething("Well... Looks like you'll have to stay here meanwhile.");
                                    }
                                    else
                                    {
                                        Delay = SayMessage("Wait, where did they go?");
                                        DialogueStep = 2;
                                        break;
                                    }
                                    DialogueStep = 9;
                                }
                                break;
                            case 9:
                                {
                                    if (HasBree)
                                    {
                                        Delay = SayMessage("What do we do now?");
                                    }
                                    else if (HasSardine)
                                    {
                                        Delay = SayMessage("Stay here? Do you think it will be fine?");
                                    }
                                    else
                                    {
                                        Delay = SayMessage("Wait, where did they go?");
                                        DialogueStep = 2;
                                        break;
                                    }
                                    DialogueStep = 10;
                                }
                                break;
                            case 10:
                                {
                                    if (HasBree)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree));
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Bree).SaySomething("It seems like we'll have to live here for longer, until we remember where our home is.");
                                    }
                                    else if (HasSardine)
                                    {
                                        StareAt(PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine));
                                        Delay = PlayerMod.GetPlayerSummonedGuardian(player, GuardianBase.Sardine).SaySomething("Don't worry about It, the people here are very nice.");
                                    }
                                    else
                                    {
                                        Delay = SayMessage("Wait, where did they go?");
                                        DialogueStep = 2;
                                        break;
                                    }
                                    DialogueStep = 11;
                                }
                                break;
                            case 11:
                                {
                                    StareAt(player);
                                    if (HasBree)
                                    {
                                        Delay = SayMessage("Alright. My name is " + Base.Name + ", I hope you don't mind if I stay with my parents.");
                                    }
                                    else if (HasSardine)
                                    {
                                        Delay = SayMessage("That's cool! My name is " + Base.Name + ", I hope you don't mind my stay here.");
                                    }
                                    else
                                    {
                                        Delay = SayMessage("Wait, where did they go?");
                                        DialogueStep = 2;
                                        break;
                                    }
                                    NpcMod.AddGuardianMet(GuardianID, "", true);
                                    PlayerMod.AddPlayerGuardian(player, GuardianID);
                                    WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID);
                                }
                                break;
                        }
                    }
                }
            }
            base.AI();
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);

        }

        public override bool CanChat()
        {
            return !PlayerHasMetGlenn && DialogueStep == 2;
        }

        public override string GetChat()
        {
            bool HasBree = PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Bree), HasSardine = PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Sardine);
            if (!HasSeenHisParents)
            {
                HasSeenHisParents = PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Sardine) || PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Bree);
            }
            return "Hello. Have you seen a Black Cat, or a White Cat that looks like me, but taller?";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (DialogueStep == 2)
            {
                if (HasSeenHisParents)
                    button = "Yes, I've seen them.";
                else
                    button = "No, I haven't.";
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                if (HasSeenHisParents)
                {
                    Main.npcChatText = "Can you please call them here? I don't feel right when visiting strange places...";
                    if (false && PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Zacks)) //Deactivated for now
                    {
                        DialogueStep = 3;
                        Delay = 120;
                    }
                }
                else
                {
                    Main.npcChatText = "If you see them, please tell me.";
                }
            }
        }

        public static bool GlennCanSpawn
        {
            get
            {
                return NPC.downedGoblins;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.water && Main.dayTime && spawnInfo.player.townNPCs == 0 && GlennCanSpawn && NpcMod.RecruitNpcSpawnConditionCheck(spawnInfo) && !NpcMod.HasMetGuardian(GuardianBase.Glenn) && !NpcMod.HasGuardianNPC(GuardianBase.Glenn) && !PlayerMod.PlayerHasGuardianSummoned(spawnInfo.player, GuardianBase.Glenn) && 
                !NPC.AnyNPCs(ModContent.NPCType<GlennNPC>()))
            {
                return 1f / 200; //250
            }
            return 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    public class DominoNPC : GuardianActorNPC
    {
        public static bool DominoDismissed = false;
        public int DialogueStep = -1;
        public int DialogueTime = 0;
        public const int DialogueDelay = 300;
        public const int BrutusID = GuardianBase.Brutus;
        public bool AcceptedIn = false;

        public DominoNPC()
            : base(9, "")
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = true;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
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
            bool PlayerHasBrutusSummoned = PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], BrutusID);
            Player player = Main.player[Main.myPlayer];
            bool PlayerHasDomino = PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianBase.Domino);
            string BrutusChat = "";
            if (PlayerHasBrutusSummoned && !DominoDismissed)
            {
                if (DialogueStep == -1 && Math.Abs(player.Center.X - npc.Center.X) < 128f)
                {
                    DialogueStep = 0;
                    DialogueTime = 0;
                    BrutusChat = "*Halt! You there!*";
                }
            }
            if (DialogueStep == -1 && player.talkNPC != npc.whoAmI)
            {
                if (DialogueTime == 0)
                {
                    if (player.Center.X - npc.Center.X < 0)
                    {
                        npc.direction = -1;
                    }
                    else
                    {
                        npc.direction = 1;
                    }
                }
                Walk = true;
                if (DialogueTime <= 1401)
                {
                    if (npc.direction < 0)
                        MoveLeft = true;
                    else
                        MoveRight = true;
                }
                if (DialogueTime >= 1401)
                    DialogueTime -= 1400;
                DialogueTime++;
            }
            npc.npcSlots = 1;
            if (DialogueStep >= 0)
            {
                npc.npcSlots = 200;
                DialogueTime++;
                bool Trigger = false;
                if (DialogueTime >= DialogueDelay)
                {
                    Trigger = true;
                    DialogueTime = 0;
                    DialogueStep++;
                }
                bool FacePlayer = false;
                if (PlayerHasDomino)
                {
                    FacePlayer = true;
                    switch (DialogueStep)
                    {
                        case 0:
                            if (DialogueTime >= 60)
                            {
                                SayMessage("*This again?!*");
                            }
                            if (DialogueTime >= 90)
                            {
                                DialogueTime = DialogueDelay;
                            }
                            break;
                        case 1:
                            if (Trigger)
                            {
                                BrutusChat = "*You have the audacity of showing up again.*";
                            }
                            break;
                        case 2:
                            if (Trigger)
                            {
                                SayMessage("*Yeah right, like as If I'm happy for seeing you again.*");
                            }
                            break;
                        case 3:
                            if (Trigger)
                            {
                                SayMessage("*Terrarian, you know the drill. Need me for anything, I will be here.*");
                            }
                            break;
                        case 4:
                            if (Trigger)
                            {
                                BrutusChat = "*Ugh...*";
                                NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                                npc.Transform(ModContent.NPCType<GuardianNPC.List.DogGuardian>());
                            }
                            break;
                    }
                }
                else
                {
                    switch (DialogueStep)
                    {
                        case 0:
                            if (DialogueTime >= 60)
                            {
                                SayMessage("*Uh oh.*");
                                FacePlayer = true;
                            }
                            if (DialogueTime >= 90)
                            {
                                DialogueTime = DialogueDelay;
                            }
                            break;
                        case 1:
                            {
                                if (Trigger)
                                {
                                    BrutusChat = "*After him! Don't let him escape!*";
                                }
                                if (player.Center.X - npc.Center.X >= 0)
                                {
                                    MoveLeft = true;
                                }
                                else
                                {
                                    MoveRight = true;
                                }
                                if (Math.Abs(player.Center.X - npc.Center.X) >= NPC.sWidth ||
                                    Math.Abs(player.Center.Y - npc.Center.Y) >= NPC.sHeight)
                                {
                                    npc.active = false;
                                    BrutusChat = "*Blast it! He got away! He'll return, I'm sure.*";
                                }

                                DialogueTime = 5;
                                if (player.getRect().Intersects(npc.getRect()))
                                {
                                    DialogueTime = DialogueDelay;
                                }
                            }
                            break;
                        case 2:
                            {
                                if (Trigger)
                                    BrutusChat = "*Got you now! You wont run away anymore!*";
                            }
                            break;
                        case 3:
                            {
                                FacePlayer = true;
                                if (Trigger)
                                {
                                    SayMessage("*Hey Brutus, didn't you lost your place as a Royal Guard on the Ether Realm?*");
                                }
                            }
                            break;
                        case 4:
                            {
                                FacePlayer = true;
                                if (Trigger)
                                {
                                    BrutusChat = "*If It wasn't for you, I wouldn't have lost my job. Beside, I was already sick of that.*";
                                }
                            }
                            break;
                        case 5:
                            {
                                FacePlayer = true;
                                if (Trigger)
                                {
                                    SayMessage("*You don't need to thank me for that.*");
                                }
                            }
                            break;
                        case 6:
                            {
                                FacePlayer = true;
                                if (Trigger)
                                {
                                    BrutusChat = "*I'm going to have you locked behind bars forever for smuggling.*";
                                }
                            }
                            break;
                        case 7:
                            {
                                FacePlayer = true;
                                if (Trigger)
                                {
                                    SayMessage("*You're no longer a guard or anything, so you can't arrest me.*");
                                }
                            }
                            break;
                        case 8:
                            {
                                FacePlayer = true;
                                if (Trigger)
                                {
                                    BrutusChat = "*Ugh... Uh...*";
                                    SayMessage("*See? You can't arrest me, and the laws of the Ether Realm aren't valid here.*");
                                }
                            }
                            break;
                        case 9:
                            {
                                FacePlayer = true;
                                if (Trigger)
                                {
                                    SayMessage("*Now, If you don't mind, I have something to ask to this Terrarian.*");
                                }
                            }
                            break;
                        case 10:
                            {
                                if (Trigger)
                                {
                                    SayMessage("*Terrarian, come talk to me.*");
                                }
                                FacePlayer = true;
                                DialogueTime = 5;
                            }
                            break;
                    }
                    if (AcceptedIn)
                    {
                        switch (DialogueStep)
                        {
                            case 11:
                                {
                                    FacePlayer = true;
                                    if (Trigger)
                                    {
                                        SayMessage("*Looks like I will be staying here.*");
                                    }
                                }
                                break;
                            case 12:
                                {
                                    FacePlayer = true;
                                    if (Trigger)
                                    {
                                        BrutusChat = "*Oh great! Now I will have to share the world with my most hated person.*";
                                    }
                                }
                                break;
                            case 13:
                                {
                                    FacePlayer = true;
                                    if (Trigger)
                                    {
                                        SayMessage("*Now If you don't mind, I have some things to move in to this world.*");
                                    }
                                }
                                break;
                            case 14:
                                {
                                    FacePlayer = true;
                                    if (Trigger)
                                    {
                                        BrutusChat = "*Mark my words, slide just a little bit, and I will put you down!*";
                                    }
                                }
                                break;
                            case 15:
                                {
                                    FacePlayer = true;
                                    if (Trigger)
                                    {
                                        SayMessage("*Yeah, right. I also love having you as a neighbor or something.*");
                                    }
                                }
                                break;
                            case 16:
                                {
                                    if (Trigger)
                                    {
                                        PlayerMod.AddPlayerGuardian(player, GuardianID, GuardianModID);
                                        NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                                        npc.Transform(ModContent.NPCType<GuardianNPC.List.DogGuardian>());
                                        return;
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (DialogueStep)
                        {
                            case 11:
                                {
                                    if (Trigger)
                                    {
                                        FacePlayer = true;
                                        BrutusChat = "*Hah! The Terrarian said it! Now go away!*";
                                    }
                                }
                                break;
                            case 12:
                                {
                                    if (Trigger)
                                    {
                                        FacePlayer = true;
                                        SayMessage("*Sigh... Well, I'm a man of word, anyway. Goodbye.*");
                                    }
                                }
                                break;
                            case 13:
                                {
                                    if (Trigger)
                                    {
                                        DominoDismissed = true;
                                        BrutusChat = "*Good riddance.*";
                                        for (int i = 0; i < 5; i++)
                                        {
                                            Gore.NewGore(npc.Center, new Microsoft.Xna.Framework.Vector2(Main.rand.Next(-40, 40) * 0.01f, Main.rand.Next(-40, 40) * 0.01f), Main.rand.Next(11, 14));
                                        }
                                        npc.active = false;
                                    }
                                }
                                break;
                        }
                    }
                }
                if (FacePlayer)
                {
                    if (player.Center.X - npc.Center.X >= 0)
                    {
                        npc.direction = 1;
                    }
                    else
                    {
                        npc.direction = -1;
                    }
                    if (Math.Abs(player.Center.X - npc.Center.X) >= 64)
                    {
                        if (player.Center.X - npc.Center.X >= 0)
                        {
                            MoveRight = true;
                        }
                        else
                        {
                            MoveLeft = true;
                        }
                    }
                }
            }
            if (Math.Abs(player.Center.X - npc.Center.X) >= NPC.sWidth + 64 ||
                Math.Abs(player.Center.Y - npc.Center.Y) >= NPC.sHeight + 64)
            {
                npc.active = false;
            }
            if (PlayerHasBrutusSummoned && BrutusChat != "")
            {
                PlayerMod.GetPlayerSummonedGuardian(player, BrutusID).SaySomething(BrutusChat);
            }
            base.AI();
        }

        public static bool CanSpawnDomino(Player player)
        {
            return (PlayerMod.PlayerHasGuardian(player, BrutusID) || PlayerMod.PlayerHasGuardian(player, GuardianBase.Domino)) && NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.playerInTown && !NpcMod.HasGuardianNPC(GuardianID) && CanSpawnDomino(spawnInfo.player) && !NPC.AnyNPCs(ModContent.NPCType<DominoNPC>()))
            {
                return 0.00390625f;
            }
            return 0;
        }

        public override string GetChat()
        {
            bool PlayerHasDomino = PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianBase.Domino);
            if (PlayerHasDomino)
            {
                return "*Hey there again. Mind If I extend my shop to this world too?*";
            }
            else if (DominoDismissed)
            {
                if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], BrutusID))
                {
                    return "*Don't worry, I'm going away soon. [Have him go away and we can talk, If you want.]*";
                }
                else
                {
                    return "*Say, would you like to review the deal?*";
                }
            }
            return (DialogueStep == 10 ? "*I have some goods I can sell, If you don't mind, I would like to open a shop here. What do you say?*" : "*I would like to sell you my goods, but right now I can't.*");
        }

        public override bool CanChat()
        {
            return DialogueStep == -1 || DialogueStep == 10;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            bool PlayerHasDomino = PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianBase.Domino);
            if (PlayerHasDomino)
            {
                button = "Sure.";
            }
            else if (DialogueStep == 10 || (DominoDismissed && !PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], BrutusID)))
            {
                button = "Feel free to.";
                button2 = "No way! Go away!";
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            bool PlayerHasDomino = PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianBase.Domino);
            if (PlayerHasDomino || (DominoDismissed && !PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], BrutusID)))
            {
                Main.npcChatText = "*Thanks mate.*";
                PlayerMod.AddPlayerGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID);
                NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                npc.Transform(ModContent.NPCType<GuardianNPC.List.DogGuardian>());
            }
            if (DialogueStep == 10)
            {
                AcceptedIn = firstButton;
                DialogueTime = DialogueDelay;
                if (firstButton)
                    Main.npcChatText = "*Wise choice, Terrarian.*";
                else
                    Main.npcChatText = "*Eh... Alright... Go Team Brutus... Happy now?*";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon.Npcs
{
    public class BlueNPC : GuardianActorNPC
    {
        public float BonfireX = 0, BonfireY = 0;
        public int AiStage { get { return (int)npc.ai[0]; } set { npc.ai[0] = value; } }
        public int AiValue { get { return (int)npc.ai[1]; } set { npc.ai[1] = value; } }
        public const int RefusalStep = 200;
        private static readonly KeyValuePair<string, string>[] DialoguesAndAnswers = new KeyValuePair<string, string>[]{ //This should be turned into a method, instead.
            new KeyValuePair<string, string>("*She asks if you're here for camping, too.*","Deny"),
            new KeyValuePair<string, string>("*After you denied, she asked if you're an adventurer.*","Yes"),
            new KeyValuePair<string, string>("*After you told her that, she looked very interessed.*","Continue"),
            new KeyValuePair<string, string>("*She tells you that on truth, didn't came to your world for camping.*","Continue"),
            new KeyValuePair<string, string>("*Said that she's looking for someone that looks like her. And asks If you saw someone like that.*","No, I didn't"),
            new KeyValuePair<string, string>("*After you said that you didn't, she looked a bit sad.*","Continue"),
            new KeyValuePair<string, string>("*She asked If you wouldn't mind if she followed you on your adventures.*","Follow me?"),
            new KeyValuePair<string, string>("*Says that while helping you on your adventure, she may find who she's looking for.*","Hm.."),
            new KeyValuePair<string, string>("*She asks you what you think.*","You can give me a hand:No way.")
        };
        private bool Rejected = false;

        public BlueNPC() //TODO - Change how the recruitment works, instead of a lengthy monologue from Blue, why not let the player speak with her instead? Do the same to Mabel aswell.
            : base(GuardianBase.Blue, "")
        {

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wolf TerraGuardian");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = false;
            npc.friendly = true;
            npc.knockBackResist = 0;
            npc.dontCountMe = true;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
            if (npc.GetGlobalNPC<NpcMod>().mobType > MobTypes.Normal)
                npc.GetGlobalNPC<NpcMod>().mobType = MobTypes.Normal;
            NpcMod.LatestMobType = MobTypes.Normal;
        }

        public override bool CanChat()
        {
            return true;//AiStage == 0 || AiStage == 10;
        }

        public override string GetChat()
        {
            if (AiStage == 0)
            {
                if (npc.Center.X - Main.player[Main.myPlayer].Center.X < 0)
                {
                    npc.direction = 1;
                    npc.velocity.Y = -7.5f;
                    npc.velocity.X = -3.5f;
                }
                else
                {
                    npc.direction = -1;
                    npc.velocity.Y = -7.5f;
                    npc.velocity.X = 3.5f;
                }
                npc.target = Main.myPlayer;
                if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianBase.Blue))
                {
                    ChangeAIStage(155);
                }
                else
                {
                    ChangeAIStage(100);
                }
                return "*You startled her.*";
            }
            /*else if (AiStage == 10)
            {
                return "*She asks if you would mind having her follow you on your adventures.*";
            }
            else if (AiStage == RefusalStep)
            {
                return "*She tells you that she'll finish her marshmellow before leaving.*";
            }
            return "*You startled her.*";*/
            return GetDialogueMessage(AiStage);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            /*if (AiStage == 10)
            {
                button = "Yes";
                button2 = "No";
            }*/
            GetDialogueMessage(AiStage, out button, out button2);
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            /*if (AiStage == 10)
            {
                if (firstButton)
                {
                    RecruitmentScripts();
                    Main.npcChatText = "*She got very happy. She told you that her name is Blue, and that she's good at almost anything you may need in your adventure.*";
                }
                else
                {
                    ChangeAIStage(RefusalStep);
                    Main.npcChatText = "*She looks disappointed, and said that she will pack up her things and leave soon.*";
                }
            }*/
            if (!firstButton && AiStage == 9)
                Rejected = true;
            AiStage++;
            Main.npcChatText = GetDialogueMessage(AiStage);
            if (AiStage == 105)
                ChangeAIStage(3);
        }

        private void RecruitmentScripts()
        {
            bool PlayerHadBlue = PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianBase.Blue);
            PlayerMod.AddPlayerGuardian(Main.player[Main.myPlayer], GuardianBase.Blue);
            if (!PlayerHadBlue)
                PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianBase.Blue).IncreaseFriendshipProgress(1);
            NpcMod.AddGuardianMet(GuardianBase.Blue, "", !Rejected);
            WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianBase.Blue);
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            if (AiStage == 0)
            {
                LeftArmAnimationFrame = RightArmAnimationFrame = Base.ItemUseFrames[2];
            }
        }

        public void ChangeAIStage(int StepNumber)
        {
            AiValue = 0;
            AiStage = StepNumber;
        }

        //The idea of the AI revamp, is to make Blue be more friendly and open, and less "Siren" on the first moment the player meets her.
        //In the old AI, she lured the player to get close to her, then she picked It up, and pretended trying to eat It to prank on the victim. It wouldn't make sense, since that's the kind of thing Zacks would do, not her.
        public override void AI()
        {
            //Need to add AI for when the player has already met her, but bump into her on the travel.
            npc.npcSlots = 0;
            if (AiStage == 0)
            {
                npc.npcSlots = 100;
                if (AiValue == 0)
                {
                    if (npc.Center.X < BonfireX * 16 + 8)
                    {
                        npc.direction = 1;
                    }
                    else
                    {
                        npc.direction = -1;
                    }
                    Main.NewText("There's something on the campfire.");
                    AiValue = 1;
                }
                float CampfireX = BonfireX * 16 + 8;
                if (Math.Abs(npc.Center.X - CampfireX) > 40)
                {
                    if (npc.Center.X < CampfireX)
                        MoveRight = true;
                    else
                        MoveLeft = true;
                }
                Rectangle FoV = new Rectangle(0, -150, 250, 300);
                if (npc.direction < 0)
                    FoV.X -= FoV.Width;
                FoV.X += (int)npc.Center.X;
                FoV.Y += (int)npc.Center.Y;
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && !Main.player[p].dead && Main.player[p].getRect().Intersects(FoV) && Collision.CanHitLine(npc.position, npc.width, npc.height, Main.player[p].position, Main.player[p].width, Main.player[p].height))
                    {
                        //Player found!
                        npc.target = p;
                        if (PlayerMod.PlayerHasGuardian(Main.player[p], GuardianBase.Blue))
                        {
                            ChangeAIStage(150);
                            AiValue = SayMessage("*You see " + PlayerMod.GetPlayerGuardian(Main.player[p], GuardianBase.Blue).Name + ", she waves and calls you.*");
                        }
                        else
                        {
                            ChangeAIStage(1);
                            AiValue = SayMessage("*You heard a voice on your head calling you. Maybe It came from that creature?*");
                        }
                    }
                }
            }
            else if (AiStage == 1)
            {
                if ((!Main.player[npc.target].active || Main.player[npc.target].dead) || Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) >= 300)
                {
                    ChangeAIStage(0);
                }
                else
                {
                    if (Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) < 96)
                    {
                        //ChangeAIStage(2);
                        AiValue = SayMessage("*She asks if you're here for camping, too.*");
                    }
                }
            }
            /*else if (AiStage == 10)
            {
                if (Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) >= 300)
                {
                    ChangeAIStage(RefusalStep);
                    AiValue = SayMessage("*She looks saddened after you refused without saying a thing.*");
                }
            }
            else if (AiStage == 150)
            {
                if ((!Main.player[npc.target].active || Main.player[npc.target].dead) || Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) >= 300)
                {
                    ChangeAIStage(0);
                }
                else
                {
                    if (Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) < 96)
                    {
                        ChangeAIStage(151);
                        AiValue = SayMessage("*She seems happy for seeing you again.*");
                    }
                }
            }
            else if (AiStage != RefusalStep)
            {
                const int DialogueDelay = 300;
                if (Main.player[npc.target].Center.X < npc.Center.X)
                    npc.direction = -1;
                else
                    npc.direction = 1;
                if (AiStage != 10 && AiValue-- <= 0)
                {
                    AiValue = 0;
                    bool ChangeStage = true;
                    switch (AiStage)
                    {
                        case 2:
                            AiValue = SayMessage("*After you denied, she asked if you're an adventurer.*");
                            break;
                        case 3:
                            AiValue = SayMessage("*After you told her that, she looked very interessed.*");
                            break;
                        case 4:
                            AiValue = SayMessage("*She tells you that on truth, didn't came to your world for camping.*");
                            break;
                        case 5:
                            AiValue = SayMessage("*She tells you that she's looking for someone that looks like her. And asks If you saw someone like that.*");
                            break;
                        case 6:
                            AiValue = SayMessage("*After you said that you didn't, she looked saddened.*");
                            break;
                        case 7:
                            AiValue = SayMessage("*She asked If you wouldn't mind if she followed you on your adventures.*");
                            break;
                        case 8:
                            AiValue = SayMessage("*Says that while helping you on your adventure, she may find who she's looking for.*");
                            break;
                        case 9:
                            AiValue = SayMessage("*She asks you what you think.*");
                            break;
                            //
                        case 100:
                            AiValue = SayMessage("*She tells you not to sneak upon other people from behind.*");
                            break;
                        case 101:
                            AiValue = SayMessage("*Tells you that nearly sliced you in half with her sword.*");
                            break;
                        case 102:
                            AiValue = SayMessage("*Trying to calm down.*");
                            break;
                        case 103:
                            AiValue = SayMessage("*She tells you that you really scared her. And apologized for earlier.*");
                            break;
                        case 104:
                            AiValue = SayMessage("*She asks what are you doing here, If you're here for camping too.*");
                            break;
                        case 105:
                            AiValue = SayMessage("*You told her that you're exploring the world.*");
                            ChangeAIStage(3); //I hope nobody complains about me recicling part of the dialogue.
                            ChangeStage = false;
                            break;
                            //
                        case 151:
                            AiValue = SayMessage("*She told you that If you need her, she will be here.*");
                            break;
                        case 152:
                            RecruitmentScripts();
                            return;
                        case 155:
                            AiValue = SayMessage("*She told you to stop doing that to her.*");
                            break;
                        case 156:
                            AiValue = SayMessage("*She says that you nearly made her heart jump out of her mouth.*");
                            break;
                        case 157:
                            AiValue = SayMessage("*She tells you that If you need her help, she will be here.*");
                            break;
                        case 158:
                            AiValue = SayMessage("*And asks you not to scare her again.*");
                            break;
                        case 159:
                            RecruitmentScripts();
                            return;
                    }
                    if(ChangeStage)
                        AiStage++;
                    if (AiValue == 0)
                        AiValue = DialogueDelay;
                }
            }*/
                if (AiStage > 0 && ((!Main.player[npc.target].active || Main.player[npc.target].dead) || Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) >= 300))
                {
                    ChangeAIStage(0);
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
                    Main.NewText("The campfire is now empty.");
                    npc.active = false;
                    npc.life = 0;
                }
            }
        }

        public string GetDialogueMessage(int DialogueID)
        {
            string b1, b2;
            return GetDialogueMessage(DialogueID, out b1, out b2);
        }

        public string GetDialogueMessage(int DialogueID, out string Button1, out string Button2)
        {
            Button1 = Button2 = "";
            string Message = "";
            switch (DialogueID)
            {
                case 1:
                    Message = "*She asks if you're here for camping, too.*";
                    Button1 = "Deny";
                    break;
                case 2:
                    Message = ("*After you denied, she asked if you're an adventurer.*");
                    Button1 = "Yes";
                    break;
                case 3:
                    Message = ("*After you told her that, she looked very interessed.*");
                    Button1 = "(Continue)";
                    break;
                case 4:
                    Message = ("*She tells you that on truth, didn't came to your world for camping.*");
                    Button1 = "Why didn't you came for, then?";
                    break;
                case 5:
                    Message = ("*She tells you that she's looking for someone that looks like her. And asks If you saw someone like that.*");
                    Button1 = "No, I didn't";
                    break;
                case 6:
                    Message = ("*After you said that you didn't, she looked saddened.*");
                    Button1 = "(Continue)";
                    break;
                case 7:
                    Message = ("*She asked If you wouldn't mind if she followed you on your adventures.*");
                    Button1 = "Why do you want to follow me?";
                    break;
                case 8:
                    Message = ("*Says that while helping you on your adventure, she may find who she's looking for.*");
                    Button1 = "Maybe";
                    break;
                case 9:
                    Message = ("*She asks you what you think.*");
                    Button1 = "Yes, I accept";
                    Button2 = "No, sorry.";
                    break;
                case 10:
                    if (!Rejected)
                    {
                        Message = "*She got very happy. She told you that her name is Blue, and that she's good at almost anything you may need in your adventure.*";
                    }
                    else
                    {
                        Message = "*She looks disappointed, but tells you that will give you her contact, in case you change your mind.*";
                    }
                    Message += " She then told you that her name is " + Base.Name + ".";
                    Main.npcChatText = Message;
                    RecruitmentScripts();
                    break;
                //
                case 100:
                    Message = ("*She tells you not to sneak upon other people from behind.*");
                    Button1 = "I'm sorry.";
                    break;
                case 101:
                    Message = ("*Tells you that nearly sliced you in half with her sword.*");
                    Button1 = "I said that I'm sorry.";
                    break;
                case 102:
                    Message = ("*Trying to calm down.*");
                    Button1 = "(Continue)";
                    break;
                case 103:
                    Message = ("*She tells you that you really scared her. And apologized for earlier.*");
                    Button1 = "O.. Okay.";
                    break;
                case 104:
                    Message = ("*She asks what are you doing here, If you're here for camping too.*");
                    Button1 = "No, I'm exploring the world";
                    break;
                case 105:
                    Message = ("*She got interessed after you said that.*");
                    Button1 = "(Continue)";
                    ChangeAIStage(3); //I hope nobody complains about me recicling part of the dialogue.
                    break;
                //
                case 150:
                    Message = "*She seems happy for seeing you again.*";
                    Button1 = "Equally";
                    break;
                case 151:
                    Message = "*She told you that If you need her, she will be here.*";
                    Button1 = "Okay";
                    Main.npcChatText = Message;
                    RecruitmentScripts();
                    break;
                case 155:
                    Message = ("*She told you to stop doing that to her.*");
                    Button1 = "I'm sorry.";
                    break;
                case 156:
                    Message = ("*She says that you nearly made her heart jump out of her mouth.*");
                    Button1 = "Again, I said I'm sorry.";
                    break;
                case 157:
                    Message = ("*She tells you that If you need her help, she will be here.*");
                    Button1 = "Okay";
                    break;
                case 158:
                    Message = ("*And asks you not to scare her again.*");
                    RecruitmentScripts();
                    break;
            }
            return Message;
        }

        public override GuardianDrawData DrawItem(Color drawColor)
        {
            if (AiStage != 0) return null;
            Vector2 Origin = new Vector2(3, 6);
            int StickPositionX, StickPositionY;
            Base.GetBetweenHandsPosition(Base.ItemUseFrames[2], out StickPositionX, out StickPositionY);
            if (npc.direction < 0)
            {
                StickPositionX = Base.SpriteWidth - StickPositionX;
                Origin.X = Main.itemTexture[Terraria.ID.ItemID.MarshmallowonaStick].Width - Origin.X;
            }
            StickPositionX -= (int)(Base.SpriteWidth * 0.5f);
            StickPositionY -= Base.SpriteHeight;
            GuardianDrawData dd = new GuardianDrawData(GuardianDrawData.TextureType.MainHandItem, Main.itemTexture[Terraria.ID.ItemID.MarshmallowonaStick], npc.Bottom + new Vector2(StickPositionX, StickPositionY) - Main.screenPosition, null, drawColor, 0f, Origin, 1f, (npc.direction < 0 ? Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally : Microsoft.Xna.Framework.Graphics.SpriteEffects.None));
            return dd;
        }
        
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool MaySpawn = !spawnInfo.water && !NpcMod.HasMetGuardian(1) && !NpcMod.HasGuardianNPC(1) && !NPC.AnyNPCs(npc.type);
            //return 0;
            return (MaySpawn ? 1 : 0f);
        }

        public override int SpawnNPC(int tileX, int tileY)
        {
            int BonfireX = -1, BonfireY = -1;
            bool Found = false;
            for (int x = -16; x < 16; x++)
            {
                for (int y = -8; y < 8; y++)
                {
                    Tile t = MainMod.GetTile(tileX + x, tileY + y);
                    if (t.active() && t.type == 215)
                    {
                        BonfireX = tileX + x;
                        BonfireY = tileY + y;
                        Found = true;
                        break;
                    }
                }
                if (Found)
                    break;
            }
            if (Found)
            {
                bool Done = false;
                int attempts = 5;
                while (!Done)
                {
                    Tile t = MainMod.GetTile(BonfireX, BonfireY);
                    if (t.frameX < 1)
                        BonfireX++;
                    if (t.frameX > 1)
                        BonfireX--;
                    if (t.frameY < 1)
                        BonfireY++;
                    if (attempts <= 0 || (t.frameX == 1 && t.frameY == 1))
                        Done = true;
                    attempts--;
                }
                bool FacingLeft = Main.rand.NextDouble() < 0.5;
                int NpcPos = NPC.NewNPC(BonfireX * 16 + 8, BonfireY * 16, ModContent.NPCType<BlueNPC>());
                if (NpcPos < 200)
                {
                    Main.npc[NpcPos].direction = FacingLeft ? -1 : 1;
                    Main.npc[NpcPos].position.X -= 32 * Main.npc[NpcPos].direction;
                    BlueNPC npc = (BlueNPC)Main.npc[NpcPos].modNPC;
                    npc.BonfireX = BonfireX;
                    npc.BonfireY = BonfireY;
                    //Main.npc[NpcPos].position.X -= Main.npc[NpcPos].width * 0.8f * Main.npc[NpcPos].direction;
                    //Main.NewText("There's something on the campfire.");
                }
                return NpcPos;
            }
            return 200;
        }
    }
}

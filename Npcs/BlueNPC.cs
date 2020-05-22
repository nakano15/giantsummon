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
        public DialogueChain dialogue = new DialogueChain();

        public BlueNPC()
            : base(GuardianBase.Blue, "")
        {
            //There is no right or wrong option in this dialogue.
            dialogue.AddDialogue("*You seems to have scared the wolf. She asks where did you came from, and what are you*", "From over there", "I'm a Terrarian.");
            dialogue.AddDialogue("*She apologized for questioning, and said that she never expected anyone to show up, even more someone who can talk.*", "*She's surprised for seeing a Terrarian, and being able to talk with one. She apologizes for the questions, as she didn't expected anyone to show up.*", "It's okay", "What are you doing here?");
            dialogue.AddDialogue("*She got impressed by your manner, then asked if you saw someone like her.*", "*She tells that was looking for someone that looked quite like her, and asked if you have seen.*", "I didn't.", "There is someone like you?");
            //The only real option that impacts recruitment.
            dialogue.AddDialogue("*She looked a bit sad after you answered, then said that It was nice talking to you, and asked if she can stay in your world for some time.*", "*She says that doesn't mean exactly like her but, but that looks quite like her. She seems to have liked talking to you, and wants to know If she could stay in your world.*", "Yes", "No", true);
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
            return AiStage < 3;
        }

        public override string GetChat()
        {
            if (AiStage == 0)
            {
                npc.velocity.Y -= Base.JumpSpeed;
                AiStage = 1;
                return dialogue.GetQuestion();
            }
            if (AiStage == 1)
            {
                return dialogue.GetQuestion();
            }
            if (AiStage == 2)
            {
                return "*She says that she'll leave soon. First she needs to eat a snack.*";
            }
            return "*I think this shouldn't happen.*";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (!dialogue.Finished)
            {
                dialogue.GetAnswers(out button, out button2);
            }
            else
            {
                if (dialogue.Points == 1)
                {
                    button = "Good Luck.";
                    button2 = "";
                }
                else
                {
                    button = button2 = "";
                }
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (!dialogue.Finished)
            {
                dialogue.MarkAnswer(firstButton);
                if (dialogue.Finished)
                {
                    //End Dialogue
                    AiStage = 2;
                    if (dialogue.Points == 1)
                    {
                        npc.velocity.Y -= Base.JumpSpeed;
                        Main.npcChatText = "*She jumped out of happiness when heard that. Then said that her name's Blue, and that It's a pleasure to meet you.*";
                        RecruitmentScripts();
                    }
                    else
                    {
                        Main.npcChatText = "*She looks at you disappointed. Then she said that will finish what she's doing before leaving.*";
                    }
                }
                else
                {
                    Main.npcChatText = dialogue.GetQuestion();
                }
            }
            else
            {

            }
        }

        private void RecruitmentScripts()
        {
            PlayerMod.AddPlayerGuardian(Main.player[Main.myPlayer], GuardianBase.Blue);
            NpcMod.AddGuardianMet(GuardianBase.Blue);
            npc.Transform(ModContent.NPCType<GuardianNPC.List.WolfGuardian>());
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
            if (AiStage >= 5 && AiStage < 5 + 8 && npc.ai[2] == 3)
            {
                LeftArmAnimationFrame = RightArmAnimationFrame = Base.ItemUseFrames[2];
            }
        }

        public override void AI()
        {
            if (AiStage == 0)
            {
                const float CheckWidth = 108f;
                float CheckStartX = npc.Center.X;
                if (npc.direction < 0)
                    CheckStartX -= CheckWidth;
                for (int p = 0; p < 255; p++)
                {
                    Player pl = Main.player[p];
                    if (pl.active && !pl.dead && pl.Center.X >= CheckStartX && pl.Center.Y < CheckStartX + CheckWidth && Math.Abs(pl.Center.Y - npc.Center.Y) < 32f && Collision.CanHitLine(pl.position, pl.width, pl.height, npc.position, npc.width, npc.height))
                    {
                        AiStage = 5;
                        Main.NewText("*You hear a voice calling you in your head.*");
                    }
                }
            }
            else if (AiStage > 5)
            {
                int DialogueStep = AiStage - 5;
                npc.TargetClosest();
                bool LastPlayerNear = npc.ai[2] == 1, LastPlayerAway = npc.ai[2] == 2;
                bool TerrarianHasBeenCaught = npc.ai[2] == 3;
                bool PlayerIsNear = Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) < 72,
                    PlayerIsAway = Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) >= 136;
                const int DialogueTime = 300;
                if (!TerrarianHasBeenCaught && (Main.player[npc.target].Center - npc.Center).Length() < Base.Width)
                {
                    npc.ai[2] = 3;
                    AiValue = 0;
                    AiStage = 5;
                    Main.NewText("The wolf captured you.");
                }
                AiValue++;
                if (TerrarianHasBeenCaught)
                {
                    if (AiValue >= DialogueTime)
                    {
                        AiValue -= DialogueTime;
                        AiStage++;
                        switch (DialogueStep)
                        {
                            case 1:
                                SayMessage("*The wolf says that there is no escape for you now.*");
                                break;
                            case 2:
                                SayMessage("*It's saying that has been a long time since It last had a meal.*");
                                break;
                            case 3:
                                SayMessage("*It says that It will enjoy having a crunchy Terrarian for lunch.*");
                                break;
                            case 4:
                                SayMessage("*It's telling you to say your prayers while you can.*");
                                break;
                            case 5:
                                //SayMessage("...");
                                Main.NewText("The wolf opens It's mouth.");
                                break;
                            case 6:
                                Main.NewText("The wolf starts laughing.");
                                break;
                            case 7:
                                SayMessage("*It is saying that you should have seen the look on your face.*");
                                break;
                            case 8:
                                SayMessage("*She apologized for the scare, then said that came to your world looking for someone.*");
                                break;
                            case 9:
                                SayMessage("*She says that liked talking with you, and asks if can move to your world.*");
                                break;
                            case 10:
                                SayMessage("*If you aren't recovering from the scare, yet.*");
                                dialogue.DialogueChainStep = 3;
                                npc.ai[2] = 0;
                                AiStage = 1;
                                AiValue = 0;
                                break;
                        }
                        if (DialogueStep < 8)
                        {
                            Vector2 Position = npc.Center;
                            Point point = Base.GetBetweenHandsPosition(Base.ItemUseFrames[2]);
                            if (npc.direction < 0)
                                point.X = Base.SpriteWidth - point.X;
                            point.X -= (int)(Base.SpriteWidth * 0.5f);
                            point.Y -= Base.SpriteHeight;
                            Position.X += point.X;
                            Position.Y += point.Y;
                            Main.player[Main.myPlayer].Center = Position;
                            Main.player[Main.myPlayer].direction = -npc.direction;
                        }
                    }
                }
                else if (PlayerIsNear)
                {
                    if (!LastPlayerNear)
                    {
                        AiStage = 5;
                        AiValue = 0;
                        npc.ai[2] = 1;
                        Main.NewText("*The wolf tells you to come closer.*");
                    }
                    if (AiValue >= DialogueTime)
                    {
                        AiValue -= DialogueTime;
                        AiStage++;
                        switch (DialogueStep)
                        {
                            case 1:
                                Main.NewText("*The wolf tells you to come a some more closer.*");
                                break;
                            case 2:
                                Main.NewText("*The wolf says to not stop.*");
                                break;
                        }
                    }
                }
                else if (PlayerIsAway)
                {
                    if (!LastPlayerAway)
                    {
                        AiStage = 5;
                        AiValue = 0;
                        npc.ai[2] = 2;
                        Main.NewText("*The wolf asks you where are you going.*");
                    }
                    if (AiValue >= DialogueTime)
                    {
                        AiValue -= DialogueTime;
                        AiStage++;
                        switch (DialogueStep)
                        {
                            case 1:
                                Main.NewText("*The wolf seems sad.*");
                                break;
                            case 2:
                                AiStage = 0;
                                AiValue = 0;
                                npc.ai[2] = 0;
                                break;
                        }
                    }
                }
                else
                {
                    if (LastPlayerNear || LastPlayerAway)
                    {
                        AiStage = 5;
                        AiValue = 0;
                        npc.ai[2] = 0;
                        if (LastPlayerAway)
                        {
                            SayMessage("*The wolf says that you're doing good, tells you to come closer.*");
                        }
                        if (LastPlayerNear)
                        {
                            SayMessage("*The wolf tells you to not go away. To come closer.*");
                        }
                    }
                    if (AiValue >= DialogueTime)
                    {
                        AiStage++;
                        AiValue -= DialogueTime;
                        switch (AiStage)
                        {
                            case 1:
                                SayMessage("*The wolf is calling you.*");
                                break;
                            case 2:
                                SayMessage("*The wolf asks what are you doing, and tells you to come nearer.*");
                                break;
                            case 3:
                                SayMessage("*The wolf asks if you are scared.*");
                                break;
                            case 4:
                                SayMessage("*The wolf says that there is no reason for you to be scared.*");
                                break;
                            case 5:
                                SayMessage("*The wolf seems to have given up.*");
                                break;
                        }
                    }
                }
            }
            base.AI();
            if (!Main.dayTime)
            {
                bool PlayerInRange = false;
                for (int p = 0; p < 255; p++)
                {
                    Player pl = Main.player[p];
                    if (Math.Abs(pl.Center.X - npc.Center.X) >= NPC.sWidth * 0.5f + NPC.safeRangeX ||
                        Math.Abs(pl.Center.Y - npc.Center.Y) >= NPC.sHeight * 0.5f + NPC.safeRangeY)
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

        public override Terraria.DataStructures.DrawData? DrawItem(Color drawColor)
        {
            if (AiStage != 0) return null;
            Vector2 Origin = new Vector2(3, 6);
            int StickPositionX, StickPositionY;
            Base.GetBetweenHandsPosition(Base.ItemUseFrames[2], out StickPositionX, out StickPositionY);
            if (npc.direction < 0)
                StickPositionX = Base.SpriteWidth - StickPositionX;
            StickPositionX -= (int)(Base.SpriteWidth * 0.5f);
            StickPositionY -= Base.SpriteHeight;
            Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(Main.itemTexture[Terraria.ID.ItemID.MarshmallowonaStick], npc.Bottom + new Vector2(StickPositionX, StickPositionY) - Main.screenPosition, null, drawColor, 0f, Origin, 1f, (npc.direction < 0 ? Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally : Microsoft.Xna.Framework.Graphics.SpriteEffects.None), 0);
            return dd;
        }
        
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool MaySpawn = !NpcMod.HasMetGuardian(1) && !NpcMod.HasGuardianNPC(1) && GuardianNPC.List.WolfGuardian.SpawnRequirement && !NPC.AnyNPCs(npc.type);
            return 0;
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
                    Tile t = Framing.GetTileSafely(tileX + x, tileY + y);
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
                    Tile t = Framing.GetTileSafely(BonfireX, BonfireY);
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
                    //Main.npc[NpcPos].position.X -= Main.npc[NpcPos].width * 0.8f * Main.npc[NpcPos].direction;
                    Main.NewText("There's something on the campfire.");
                }
                return NpcPos;
            }
            return 200;
        }
    }
}

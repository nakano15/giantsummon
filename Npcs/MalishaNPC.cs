using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon.Npcs
{
    public class MalishaNPC : GuardianActorNPC
    {
        public int AiStage { get { return (int)npc.ai[0]; } set { npc.ai[0] = value; } }
        public int DialogueTime { get { return (int)npc.ai[1]; } set { npc.ai[1] = value; } }

        public MalishaNPC()
            : base(GuardianBase.Malisha, "")
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void AI()
        {
            if (AiStage > 1)
            {
                if (npc.target == -1 || npc.target == 256)
                {
                    AiStage = 1;
                    SayMessage("*Funny, where did the Terrarian I was talking with to went?*");
                }
                else if (!Main.player[npc.target].active)
                {
                    AiStage = 1;
                    SayMessage("*Funny, where did the Terrarian I was talking with to went?*");
                }
                else if (Main.player[npc.target].dead)
                {
                    AiStage = 1;
                    SayMessage("*Well, that was an awful sight.*");
                }
            }
            if (AiStage == 0)
            {
                Idle = true;
                AiStage = 1;
                npc.direction = (Main.rand.NextDouble() < 0.5 ? -1 : 1);
                Main.NewText("Something appeared to the " + GuardianBountyQuest.GetDirectionText(npc.Center - Main.player[Main.myPlayer].Center) + " of " + Main.player[Main.myPlayer].name + ".");
            }
            else if (AiStage == 1)
            {
                Idle = true;
                Rectangle FoV = new Rectangle(0, -150, 250, 300);
                if (npc.direction < 0)
                    FoV.X = -FoV.Width;
                FoV.X += (int)npc.Center.X;
                FoV.Y += (int)npc.Center.Y;
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && !Main.player[p].dead && Main.player[p].getRect().Intersects(FoV) && Collision.CanHitLine(npc.position, npc.width, npc.height, Main.player[p].position, Main.player[p].width, Main.player[p].height))
                    {
                        //Player found!
                        npc.target = p;
                        if (PlayerMod.PlayerHasGuardian(Main.player[p], GuardianBase.Malisha))
                        {
                            DialogueTime = SayMessage("*Hey, It's you again.*"); //Add later the dialogue
                            AiStage = 150;
                        }
                        else
                        {
                            AiStage = 2;
                            DialogueTime = SayMessage("*A Terrarian! I must have really arrived.*");
                        }
                        if (npc.Center.X - Main.player[p].Center.X > 0)
                        {
                            npc.direction = -1;
                        }
                        else
                        {
                            npc.direction = 1;
                        }
                        break;
                    }
                }
            }
            else if (AiStage == 2)
            {
                Player player = Main.player[npc.target];
                if (npc.Center.X - player.Center.X > 0)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }
                if (DialogueTime > 0)
                    DialogueTime--;
                else
                {
                    if (PlayerUsingDryadSet(player))
                    {
                        DialogueTime = SayMessage("*Look at how It's dressed! This must be the right place.*");
                    }
                    else
                    {
                        DialogueTime = SayMessage("*Wait, why is It using clothes?*");
                    }
                    AiStage = 3;
                }
            }
            else if (AiStage == 3)
            {
                Player player = Main.player[npc.target];
                if (npc.Center.X - player.Center.X > 0)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }
                if (DialogueTime > 0)
                    DialogueTime--;
                else
                {
                    float Distance = player.Center.X - npc.Center.X;
                    if (Math.Abs(Distance) > 96)
                    {
                        if (Distance > 0)
                            MoveRight = true;
                        else
                            MoveLeft = true;
                    }
                    else
                    {

                    }
                }
            }
            else if (AiStage == 100)
            {
                npc.TargetClosest(false);
                Player player = Main.player[npc.target];
                Idle = true;
            }
            else if (AiStage == 150)
            {
                Player player = Main.player[npc.target];
                if (npc.Center.X - player.Center.X > 0)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }
                if (DialogueTime > 0)
                    DialogueTime--;
                else
                {
                    DialogueTime = SayMessage("*I think this isn't a naturalist colony, either, right?*");
                    AiStage = 151;
                }
            }
            else if (AiStage == 151)
            {
                Player player = Main.player[npc.target];
                if (npc.Center.X - player.Center.X > 0)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }
                if (DialogueTime > 0)
                    DialogueTime--;
                else
                {
                    DialogueTime = SayMessage("*Well, whatever. I'm here If you need my knowledge. Or feel like wanting to be a guinea pig. You pick.*");
                    AiStage = 152;
                }
            }
            else if (AiStage == 152)
            {
                Player player = Main.player[npc.target];
                if (npc.Center.X - player.Center.X > 0)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }
                if (DialogueTime > 0)
                    DialogueTime--;
                else
                {
                    PlayerMod.AddPlayerGuardian(Main.player[npc.target], GuardianBase.Malisha);
                    NpcMod.AddGuardianMet(GuardianBase.Malisha);
                    WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                    //npc.Transform(ModContent.NPCType<GuardianNPC.List.PantherGuardian>());
                }
            }
            else
            {
                Player player = Main.player[npc.target];
                //Dialogue Checks
                if (player.talkNPC != npc.whoAmI)
                {
                    DialogueTime = SayMessage("*How rude! Speak to me when possible then.*");
                    AiStage = 100;
                }
            }
            base.AI();
        }

        private bool PlayerUsingDryadSet(Player player)
        {
            return (player.head == Terraria.ID.ArmorIDs.Head.FamiliarWig && player.body == Terraria.ID.ArmorIDs.Body.DryadCoverings && player.legs == Terraria.ID.ArmorIDs.Legs.DryadLoincloth);
        }

        public override bool CanChat()
        {
            return AiStage == 1 || AiStage == 100 || AiStage == 3 || AiStage == 4;
        }

        public override string GetChat()
        {
            if (AiStage == 3)
            {
                AiStage = 4;
            }
            if (AiStage == 100)
                AiStage = 4;
            return GetStepMessage();
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = GetStepAnswer();
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                switch (AiStage)
                {
                    default:
                        AiStage++;
                        break;
                    case 1:
                        AiStage = 4;
                        break;
                    case 3:
                    case 4:
                        if (PlayerUsingDryadSet(Main.player[npc.target]))
                        {
                            AiStage = 6;
                        }
                        else
                        {
                            if (!PlayerMod.GetPlayerMainGuardian(Main.player[npc.target]).Active)
                            {
                                AiStage = 9;
                            }
                            else
                            {
                                AiStage = 5;
                            }
                        }
                        break;
                    case 5:
                        AiStage = 8;
                        break;
                    case 6:
                        if (!PlayerMod.GetPlayerMainGuardian(Main.player[npc.target]).Active)
                        {
                            AiStage = 9;
                        }
                        else
                        {
                            AiStage++;
                        }
                        break;
                    case 7:
                        AiStage = 9;
                        break;
                    case 11:
                        Main.npcChatText = "*I'm Malisha, by the way. I'll try enjoying my time here.*";
                        PlayerMod.AddPlayerGuardian(Main.player[npc.target], GuardianBase.Malisha);
                        PlayerMod.GetPlayerGuardian(Main.player[npc.target], GuardianBase.Malisha).IncreaseFriendshipProgress(1);
                        NpcMod.AddGuardianMet(GuardianBase.Malisha);
                        WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                        //npc.Transform(ModContent.NPCType<GuardianNPC.List.PantherGuardian>());
                        return;
                }
                Main.npcChatText = GetStepMessage();
            }
        }

        public string GetStepMessage()
        {
            switch (AiStage)
            {
                case 1:
                    return "*Oh, at Terrarian. This must be the place of the naturalist colony I've heard then.*";
                case 3:
                case 4: //Initial message.
                    return (PlayerUsingDryadSet(Main.player[npc.target]) ? "*I thought this was a naturalist world, why are you using leaves?*" : "*Funny, I thought this was a naturalist colony world, why are you using clothes?*");
                case 5: //When player doesn't has Dryad outfit and Familiar wig. Jump to 8 after that.
                    return "*Then why the TerraGuardians aren't using clothes?*";
                case 6: //When player is using dryad outfit and familiar wig.
                    return "*Then why you are using such outfit?*";
                case 7: //Jump to 9
                    return "*And the TerraGuardians aren't using clothes.*";
                case 8:
                    return "*Then you're the one who's not in the trend.*";
                case 9:
                    return "*That's quite sad, I was thinking of taking some vacations here. Oh... Well.*";
                case 10:
                    return "*I may? Alright, I'll move in to some empty house. Maybe I'll be able to do some experiements too.*";
                case 11:
                    return "*Don't worry, maybe an explosion or two may happen, or familiar critters running around but nothing too serious.*";
            }
            return "*?*";
        }

        public string GetStepAnswer()
        {
            switch (AiStage)
            {
                case 1:
                    return "A what?";
                case 4:
                    return "This isn't a naturalist colony.";
                case 5:
                    return "I don't know";
                case 6:
                    return "Hey, that's my style";
                case 7:
                    return "We aren't naturalists, At least I'm not";
                case 8:
                    return "There's no trend, this isn't a naturalist colony";
                case 9:
                    return "You may stay here";
                case 10:
                    return "Uh... Experiements?";
                case 11:
                    return "No, wait.";
            }
            return "Proceed";
        }

        public override void  ModifyDrawDatas(List<GuardianDrawData> dds, Vector2 Position, Rectangle BodyRect, Rectangle LArmRect, Rectangle RArmRect, Vector2 Origin, Color color, Microsoft.Xna.Framework.Graphics.SpriteEffects seffects)
        {
            GuardianDrawData dd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra,Base.sprites.GetExtraTexture("tails"), Position, BodyRect, color, 0f, Origin, 1f, seffects);
            dds.Insert(0, dd);
        }

        public static bool MalishaCanSpawn { get { return NPC.downedBoss3; } }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.playerInTown && !Main.dayTime && MalishaCanSpawn && !NpcMod.HasGuardianNPC(GuardianBase.Malisha) && !PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianBase.Malisha) && Main.time > 19800 && !NPC.AnyNPCs(ModContent.NPCType<MalishaNPC>()))
            {
                return (float)(Main.time - 19800) / 54000;
            }
            return 0;
        }
    }
}

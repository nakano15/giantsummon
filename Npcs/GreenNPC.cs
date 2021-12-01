using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    public class GreenNPC : GuardianActorNPC
    {
        public bool FirstFrame = true, Sleeping = true, PlayerMovedAway = false;
        public int Timer = 0;
        public byte Step = 0;

        public GreenNPC() : base(GuardianBase.Green)
        {

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snake Guardian");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = false;
            npc.rarity = 1;
        }

        public override void AI()
        {
            Idle = false;
            if (FirstFrame)
            {
                int tileX = (int)(npc.Center.X * (1f / 16)), tileY = (int)(npc.Center.Y * (1f / 16));
                for (int y = 0; y >= -4; y--)
                {
                    for (int x = -2; x < 3; x++)
                    {
                        int TileX = tileX + x, TileY = tileY + y;
                        if (Main.tile[TileX, TileY].active() && Main.tile[TileX, TileY].type == Terraria.ID.TileID.Trees)
                        {
                            int SizeCount = 0;
                            while (Main.tile[TileX, TileY].active() && Main.tile[TileX, TileY].type == Terraria.ID.TileID.Trees)
                            {
                                TileY--;
                                SizeCount++;
                            }
                            TileY += 3;
                            if (SizeCount >= 9 && Main.tile[TileX, TileY].active() && Main.tile[TileX, TileY].type == Terraria.ID.TileID.Trees)
                            {
                                npc.position.X = TileX * 16 + 8 - npc.width * 0.25f;
                                npc.position.Y = TileY * 16 - npc.height * 0.5f;
                                return;
                            }
                        }
                    }
                }
            }
            if (Sleeping)
            {
                npc.velocity = Vector2.Zero;
                npc.noGravity = true;
                int TileCenterX = (int)((npc.position.X + npc.width * 0.25f) * (1f / 16));
                int TileCenterY = (int)(npc.Center.Y * (1f / 16));
                bool SuspendedByTree = false;
                    Tile tile = Framing.GetTileSafely(TileCenterX, TileCenterY);
                if (tile != null && tile.active())
                {
                    SuspendedByTree = true;
                }
                if (!SuspendedByTree)
                {
                    Sleeping = false;
                    SayMessage("*W-what?!*", true);
                    npc.TargetClosest();
                }
                return;
            }
            npc.noGravity = false;
            bool PlayerKnowsGuardian = PlayerMod.PlayerHasGuardian(Main.player[npc.target], GuardianID);
            if (Step > 0)
            {
                if(Step < 5 && Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) >= 1200)
                {
                    SayMessage("*Hey! I'm talking to you!*");
                    Step = 1;
                    Timer = 0;
                    Idle = true;
                    PlayerMovedAway = true;
                }
                else
                {
                    if(PlayerMovedAway)
                    {
                        SayMessage("*You returned, now you have to listen to me.*");
                        Timer = -120;
                        PlayerMovedAway = false;
                    }
                    if (Main.player[npc.target].Center.X < npc.Center.X)
                        npc.direction = -1;
                    else
                        npc.direction = 1;
                }
            }
            switch (Step)
            {
                case 0:
                    if(npc.velocity.Y == 0)
                    {
                        Timer++;
                        if(Timer >= 150)
                        {
                            Timer = 0;
                            Step++;
                        }
                    }
                    break;
                case 1:
                    if(Timer == 50)
                    {
                        if (PlayerKnowsGuardian)
                        {
                            SayMessage("*Again you destroyed the tree I was sleeping on.*", true);
                        }
                        else
                        {
                            SayMessage("*What is your problem? Didn't see me there?*", true);
                        }
                    }
                    Timer++;
                    if(Timer >= 260)
                    {
                        Timer = 0;
                        Step++;
                    }
                    break;
                case 2:
                    if(Timer == 0)
                    {
                        if (PlayerKnowsGuardian)
                        {
                            SayMessage("*Do you have anything against me sleeping on trees or something.*", true);
                        }
                        else
                        {
                            SayMessage("*Next time you want to get some lumber, look up-!*", true);
                        }
                    }
                    Timer++;
                    if(Timer >= 210)
                    {
                        Timer = 0;
                        Step++;
                    }
                    break;
                case 3:
                    if (Timer == 0)
                    {
                        if (PlayerKnowsGuardian)
                        {
                            SayMessage("*Ugh... Whatever...*", true);
                        }
                        else
                        {
                            SayMessage("*Wait a minute, aren't you a Terrarian.*", true);
                        }
                    }
                    Timer++;
                    if (Timer >= 180)
                    {
                        Timer = 0;
                        Step++;
                    }
                    break;
                case 4:
                    if (Timer == 0)
                    {
                        if (PlayerKnowsGuardian)
                        {
                            SayMessage("*I'm here if you ever need a medic... Now I need to look for a safe place for a nap.*", true);
                            NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                            WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                            return;
                        }
                        else
                        {
                            SayMessage("*Hm... I wonder if...*", true);
                        }
                    }
                    Timer++;
                    if (Timer >= 180)
                    {
                        Timer = 0;
                        Step++;
                    }
                    break;
                case 5:
                    if (Timer == 0)
                    {
                        SayMessage("*If you can understand me, could you try speaking to me?*", true);
                        Timer++;
                    }
                    break;
                default:
                    if (Step >= 6)
                    {
                        bool AnyoneSpeakingToMe = false;
                        for (int i = 0; i < 255; i++)
                        {
                            if (Main.player[i].active && Main.player[i].talkNPC == npc.whoAmI)
                            {
                                AnyoneSpeakingToMe = true;
                                break;
                            }
                        }
                        if (!AnyoneSpeakingToMe)
                        {
                            Step = 5;
                            Timer = 1;
                            SayMessage("*I still would like if we could speak some more, Terrarian.*");
                        }
                    }
                    break;
            }
            base.AI();
        }

        public override bool CanChat()
        {
            return Step == 5;
        }

        public override string GetChat()
        {
            return "*I see, you Terrarians can also speak to use... Good.*";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            switch (Step)
            {
                case 5:
                    button = "Speak to us?";
                    break;
                case 6:
                    if (Timer != 1)
                        button = "Who are you?";
                    else
                        button = "";
                    if (Timer != 2)
                        button2 = "What are you doing here?";
                    else
                        button2 = "";
                    break;
                case 7:
                    button = "Medic? You?";
                    button2 = "Exploring this world?";
                    break;
                case 8:
                    button = "What you're going to do now?";
                    break;
                case 9:
                    button = "About me?";
                    break;
                case 10:
                    button = "Yes";
                    button2 = "No";
                    break;
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            switch (Step)
            {
                case 5:
                    Step = 6;
                    Timer = 0;
                    Main.npcChatText = "*Yes, we TerraGuardians speaks mostly from ways out of the conventional. Gladly seems like the bond let us understand each other.*";
                    break;
                case 6:
                    if (firstButton)
                    {
                        if(Timer > 0)
                        {
                            Step = 7;
                            Timer = 2;
                        }
                        else
                        {
                            Timer = 1;
                        }
                        Main.npcChatText = "*I'm a medic from the Ether Realm. I have treated many patients there, but right now I'm exploring this world.*";
                    }
                    else
                    {
                        if (Timer > 0)
                        {
                            Step = 7;
                            Timer = 1;
                        }
                        else
                        {
                            Timer = 2;
                        }
                        Main.npcChatText = "*I was exploring this world, until I was getting tired of wandering, so I went atop that tree to sleep, until you put it all down.*";
                    }
                    break;
                case 7:
                    if (firstButton)
                    {
                        Main.npcChatText = "*Believe it or not. Even though my face may look intimidating, I take my job very seriously.*";
                    }
                    else
                    {
                        Main.npcChatText = "*I lived on the Ether Realm since I was an egg, literally. I wanted to see how this \"new world\" looks.*";
                    }
                    Step = 8;
                    Timer = 0;
                    break;
                case 8:
                    Main.npcChatText = "*Since you demolished my slumber spot, there isn't much I can do. But meeting you has just made me curious about you...*";
                    Step = 9;
                    break;
                case 9:
                    Main.npcChatText = "*Not in the sense you are thinking... Hm... Say, would you mind if I stayed here, and studied your people? I may end up being helpful in case you ever feel injured or sick, once I discover how your bodies work.*";
                    Step = 10;
                    break;
                case 10:
                    PlayerMod.AddPlayerGuardian(Main.player[Main.myPlayer], GuardianID);
                    NpcMod.AddGuardianMet(GuardianID, GuardianModID, firstButton);
                    if (firstButton)
                    {
                        Main.npcChatText = "*Thank you, this is of great importance to me. My name is Jochen Green, but you can call me Dr. Green, instead.*";
                    }
                    else
                    {
                        Main.npcChatText = "*Well, that's sad. Anyways, you can keep contact of me in case you change your mind. My name is Jochen Green. You can call me Dr. Green, instead.*";
                    }
                    WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                    break;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            if (Sleeping)
            {
                BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.BedSleepingFrame;
            }
            else
            {
                if(npc.velocity.Y == 0)
                {
                    if(Step == 0)
                    {
                        BodyAnimationFrame = LeftArmAnimationFrame = RightArmAnimationFrame = Base.DownedFrame;
                    }
                }
            }
        }

        public static bool CanSpawnGreen()
        {
            return NPC.downedBoss3;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!NPC.AnyNPCs(npc.type) && (Main.dayTime && !Main.eclipse && Main.time < 9 * 3600 && CanSpawnGreen()))
            {
                bool HasTree = false;
                for (int y = 0; y >= -4; y--)
                {
                    for (int x = -2; x < 3; x++)
                    {
                        int TileX = spawnInfo.spawnTileX + x, TileY = spawnInfo.spawnTileY + y;
                        int TreeSize = 0;
                        while(Main.tile[TileX, TileY].active() && Main.tile[TileX, TileY].type == Terraria.ID.TileID.Trees)
                        {
                            TreeSize++;
                            TileY--;
                        }
                        if (TreeSize >= 9)
                            HasTree = true;
                    }
                }
                if (HasTree)
                {
                    return 1f / 35;
                }
            }
            return 0;
        }
    }
}

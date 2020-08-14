using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon.Npcs
{
    public class VladimirNPC : GuardianActorNPC
    {
        public bool RequestTaken = false, RequestComplete = false, HugPassed = false;
        public byte FishsTaken = 0, FishsToTake = 0;
        public int HuggingPlayer = -1;
        const int FishID = Terraria.ID.ItemID.Honeyfin;
        const string FishName = "Honeyfin";
        public byte ComplaintCooldown = 0;
        public byte LastHoneySenseValue = 0;

        public VladimirNPC()
            : base(GuardianBase.Vladimir, "")
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.townNPC = false;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
            if (npc.GetGlobalNPC<NpcMod>().mobType > MobTypes.Normal)
                npc.GetGlobalNPC<NpcMod>().mobType = MobTypes.Normal;
            NpcMod.LatestMobType = MobTypes.Normal;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            if(HuggingPlayer > -1)
                LeftArmAnimationFrame = RightArmAnimationFrame = BodyAnimationFrame = Base.PlayerMountedArmAnimation;
        }

        public override bool CanChat()
        {
            return true;
        }

        public override void AI()
        {
            if (npc.ai[2] == 0)
            {
                npc.ai[2] = 1;
                npc.ai[3] = -3;
                npc.TargetClosest(false);
                Main.NewText("A huge bear appeared " + GuardianBountyQuest.GetDirectionText(npc.Center - Main.player[npc.target].Center) + " near to " + Main.player[npc.target].name + ".");
            }
            if (npc.direction == 0)
                npc.direction = 1;
            if (!RequestTaken)
            {
                this.Idle = true;
                npc.ai[0]++;
                if (npc.ai[0] >= (300 + 60 * npc.ai[1]))
                {
                    npc.ai[1] = 3 + Main.rand.Next(4);
                    npc.ai[0] = 0;
                    string Message = "";
                    switch (Main.rand.Next(5))
                    {
                        case 0:
                            Message = "*Is there really a town around here?*";
                            break;
                        case 1:
                            Message = "*I'm so hungry... I've been walking for days...*";
                            break;
                        case 2:
                            Message = "*I need to take some rest...*";
                            break;
                        case 3:
                            Message = "*Why are all the creatures here so aggressive?*";
                            break;
                        case 4:
                            Message = "*I wonder if there is someone nearby...*";
                            break;
                    }
                    SayMessage(Message);
                }
                npc.TargetClosest(false);
                if (Main.player[npc.target].talkNPC == npc.whoAmI)
                {
                    Idle = false;
                    if (Main.player[npc.target].Center.X < npc.Center.X)
                    {
                        npc.direction = -1;
                    }
                    else
                    {
                        npc.direction = 1;
                    }
                }
            }
            else if (RequestComplete)
            {
                if (HuggingPlayer != -1)
                {
                    Player player = Main.player[Main.myPlayer];
                    player.immuneTime = 3;
                    player.immuneNoBlink = true;
                    PlayerMod pm = player.GetModPlayer<PlayerMod>();
                    bool ControllingGuardian = pm.ControllingGuardian;
                    bool ItemUsed = false, JumpUsed = false, MoveUsed = false;
                    if (ControllingGuardian)
                    {
                        pm.Guardian.ImmuneTime = 3;
                        pm.Guardian.ImmuneNoBlink = true;
                        ItemUsed = pm.Guardian.LastAction && pm.Guardian.ItemAnimationTime == 0;
                        JumpUsed = pm.Guardian.LastJump;
                        MoveUsed = (pm.Guardian.LastMoveLeft) ||
                            (pm.Guardian.LastMoveUp) ||
                            (pm.Guardian.LastMoveRight) ||
                            (pm.Guardian.LastMoveDown);
                    }
                    else
                    {
                        ItemUsed = player.controlUseItem;
                        JumpUsed = player.controlJump;
                        MoveUsed = (player.controlLeft) || (player.controlUp) || (player.controlRight) || (player.controlDown);
                    }
                    if (PlayerMod.PlayerMountedOnGuardian(player))
                    {
                        foreach (TerraGuardian guardian in pm.GetAllGuardianFollowers)
                        {
                            if (guardian.Active && guardian.PlayerMounted)
                                guardian.ToggleMount(true, false);
                        }
                    }
                    string Message = null;
                    if (HugPassed && HuggingPlayer > -1 && (ItemUsed || JumpUsed || MoveUsed))
                    {
                        if (ComplaintCooldown == 0)
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    Message = "*You can just talk to me if that's enough.*";
                                    break;
                                case 1:
                                    Message = "*Do you want me to stop? Talk to me again.*";
                                    break;
                                case 2:
                                    Message = "*Had enough of hugs? Talk to me so I can stop.*";
                                    break;
                            }
                        }
                        ComplaintCooldown = 30;
                    }
                    else if (ItemUsed)
                    {
                        if (ComplaintCooldown == 0)
                        {
                            npc.ai[0] -= Main.rand.Next(19, 41);
                            if (player.inventory[player.selectedItem].damage > 0)
                            {
                                switch (Main.rand.Next(5))
                                {
                                    case 0:
                                        Message = ("*Becareful! That can hurt someone!*");
                                        break;
                                    case 1:
                                        Message = ("*Hey! Are you trying to hurt me?*");
                                        break;
                                    case 2:
                                        Message = ("*You're doing the complete oposite of what a hug should do!*");
                                        break;
                                    case 3:
                                        Message = ("*You'll end up hurting someone with that.*");
                                        break;
                                    case 4:
                                        Message = ("*Keep that off my skin!*");
                                        break;
                                }
                            }
                            else
                            {
                                switch (Main.rand.Next(5))
                                {
                                    case 0:
                                        Message = ("*What are you doing?*");
                                        break;
                                    case 1:
                                        Message = ("*Can you stop doing that?*");
                                        break;
                                    case 2:
                                        Message = ("*Hey! Watch out!*");
                                        break;
                                    case 3:
                                        Message = ("*Do you easily get bored?*");
                                        break;
                                    case 4:
                                        Message = ("*Why are you doing that?*");
                                        break;
                                }
                            }
                        }
                        ComplaintCooldown = 30;
                    }
                    else if (JumpUsed)
                    {
                        if (ComplaintCooldown == 0)
                        {
                            npc.ai[0] -= Main.rand.Next(23, 54);
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    Message = ("*Oof! Hey! My stomach!*");
                                    break;
                                case 1:
                                    Message = ("*Ouch! Are you a cat or something?*");
                                    break;
                                case 2:
                                    Message = ("*Urgh. Don't hit my belly!*");
                                    break;
                                case 3:
                                    Message = ("*Hey! Why the aggression?*");
                                    break;
                                case 4:
                                    Message = ("*Urrrf... (Breathing for a moment) Gasp! You took out all my air with that kick! Why?! Tell me why you did that?!*");
                                    break;
                            }
                        }
                        ComplaintCooldown = 30;
                    }
                    else if (MoveUsed)
                    {
                        if (ComplaintCooldown == 0)
                        {
                            npc.ai[0] -= Main.rand.Next(14, 33);
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    Message = ("*What are you doing?*");
                                    break;
                                case 1:
                                    Message = ("*It's hard to hug with you like that.*");
                                    break;
                                case 2:
                                    Message = ("*If you want me to stop, just tell me.*");
                                    break;
                                case 3:
                                    Message = ("*Are you tense or something? Stop that.*");
                                    break;
                                case 4:
                                    Message = ("*You'll end up falling if you keep doing that.*");
                                    break;
                            }
                        }
                        ComplaintCooldown = 30;
                    }
                    else
                    {
                        if (ComplaintCooldown > 0)
                            ComplaintCooldown--;
                        npc.ai[0]++;
                        if (ControllingGuardian)
                        {
                            if (pm.Guardian.ItemAnimationTime == 0 && !pm.Guardian.MoveLeft && !pm.Guardian.MoveRight)
                            {
                                pm.Guardian.LookingLeft = npc.direction == 1;
                            }
                        }
                        else
                        {
                            if (player.itemAnimation == 0 && !player.controlLeft && !player.controlRight)
                            {
                                player.direction = -npc.direction;
                            }
                        }
                        if (HugPassed && npc.ai[0] % (60 * 10) == 0)
                        {
                            switch (Main.rand.Next(4))
                            {
                                case 0:
                                    Message = "*You want some more? I don't mind.*";
                                    break;
                                case 1:
                                    Message = "*(Humming)*";
                                    break;
                                case 2:
                                    Message = "*Talk to me If you had enough.*";
                                    break;
                                case 3:
                                    Message = "*Warm.*";
                                    break;
                            }
                        }
                        else if (npc.ai[0] >= (PlayerHasVladimir ? 0.3f : (Main.expertMode ? 1.2f : 0.5f)) * 60 * 60)
                        {
                            if (!HugPassed)
                            {
                                HugPassed = true;
                                Message = "*Thank you, I was really needing that. I think this world could make use of someone like me. Call me Vladimir, It's my name.*";
                                npc.ai[0] = 1;
                            }
                        }
                        else if (player.talkNPC == npc.whoAmI && npc.ai[0] % (60 * 10) == 0)
                        {
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    Message = ("*You don't know how much I missed doing that.*");
                                    break;
                                case 1:
                                    Message = ("*It's good to feel another warm body after so much travel.*");
                                    break;
                                case 2:
                                    Message = ("*Just some more.*");
                                    break;
                                case 3:
                                    Message = ("*Thanks for the fishs, by the way.*");
                                    break;
                                case 4:
                                    Message = ("*Are there more people like you in this world?*");
                                    break;
                            }
                        }
                    }
                    if (Message != null)
                    {
                        if (player.talkNPC == npc.whoAmI)
                        {
                            Main.npcChatText = Message;
                        }
                        else
                        {
                            SayMessage(Message);
                        }
                    }
                    Vector2 HugPosition = Base.MountShoulderPoints.GetPositionFromFrameVector(BodyAnimationFrame);
                    if (npc.direction < 0)
                        HugPosition.X = Base.SpriteWidth - HugPosition.X;
                    HugPosition.X -= Base.SpriteWidth * 0.5f;
                    HugPosition.Y -= Base.SpriteHeight;
                    HugPosition.X += npc.position.X + npc.width * 0.5f;
                    HugPosition.Y += npc.position.Y + npc.height;
                    if (npc.ai[0] < -200)
                    {
                        switch (Main.rand.Next(5))
                        {
                            case 0:
                                Message = "*Alright, there you go. Happy?*";
                                break;
                            case 1:
                                Message = "*It was just a hug, why were you moving around like crazy?*";
                                break;
                            case 2:
                                Message = "*Does hugs makes you uncomfortable or something?*";
                                break;
                            case 3:
                                Message = "*Need to use the toilet or something?*";
                                break;
                            case 4:
                                Message = "*Is It the environment or me?*";
                                break;
                        }
                        HugPosition = npc.Center;
                        HuggingPlayer = -1;
                    }
                    if (ControllingGuardian)
                    {
                        pm.Guardian.Position = HugPosition;
                        pm.Guardian.Position.Y += pm.Guardian.Height * 0.5f;
                        pm.Guardian.Velocity.Y = -pm.Guardian.Mass;
                        pm.Guardian.ImmuneTime = 3;
                        pm.Guardian.ImmuneNoBlink = true;
                    }
                    else
                    {
                        player.Center = HugPosition;
                        player.velocity.Y = - Player.defaultGravity;
                    }
                    if (!player.active)
                    {
                        SayMessage("*Where did that person go?*");
                        HuggingPlayer = -1;
                    }
                }
                else
                {
                    npc.TargetClosest();
                }
            }
            else if (RequestTaken)
            {
                npc.TargetClosest();
                float Distance = npc.Center.X - Main.player[npc.target].Center.X;
                if (Math.Abs(Distance) >= 68f)
                {
                    if (Distance < 0)
                        MoveRight = true;
                    else
                        MoveLeft = true;
                }
                if (npc.Distance(Main.player[npc.target].Center) >= 520f)
                {
                    npc.position = Main.player[npc.target].position;
                    npc.position.Y -= Base.SpriteHeight - Player.defaultHeight;
                }
            }
            if (HuggingPlayer > -1) DrawInFrontOfPlayers.Add(HuggingPlayer);
            base.AI();
        }

        public static bool PlayerHasVladimir { get { return PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianBase.Vladimir); } }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<VladimirNPC>()) && !NpcMod.HasGuardianNPC(GuardianID) && CanRecruitVladimir && 
                !PlayerMod.PlayerHasGuardianSummoned(spawnInfo.player, GuardianID) && spawnInfo.player.ZoneJungle && 
                Main.rand.Next(256 - (int)spawnInfo.player.position.Y / 1024) == 0)
            {
                //Tile t = Framing.GetTileSafely(spawnInfo.spawnTileX, spawnInfo.spawnTileY);
                //if (t.wall == Terraria.ID.WallID.HiveUnsafe || t.type == Terraria.ID.TileID.HoneyBlock)
                //{
                    return 1;
                //}
            }
            return 0;
        }

        public static bool CanRecruitVladimir { get { return !NpcMod.HasMetGuardian(GuardianBase.Vladimir); } }

        public override bool CheckActive()
        {
            return false;
        }

        public override string GetChat()
        {
            if (HuggingPlayer > -1)
            {
                if (HugPassed)
                {
                    return "*I'm feeling a lot better now, but I have a request for you. Can I move in to your world? Maybe there are some other people who need my help.*";
                }
                return "*I still need some more hug.*";
            }
            else if (HugPassed)
            {
                return "*Now that I put you on the floor, can I move in to your world? Maybe there are some people who need my help.*";
            }
            else if (PlayerHasVladimir)
            {
                RequestTaken = RequestComplete = true;
                return "*Hello Terrarian. It's me, the marsupial bear of the hugs. I were travelling around the world trying to find other places that could use my help, the only luck I had was bumping into you on the way. You know what you need to do to have me move into this world.*";
            }
            else if (RequestComplete)
            {
                return "*I'm all fed now, but I need to feel the warmth of another body. Can you give me a hug?*";
            }
            else if (RequestTaken)
            {
                return "*My belly is still complaining... Did you got some more " + FishName + "?*";
            }
            else
            {
                if (PlayerMod.ControlledIsTerraGuardian(Main.player[Main.myPlayer]))
                {
                    return "*You're a TerraGuardian! No, wait... You're a Terrarian controlling a TerraGuardian! That Guardian must really like you to allow you to do that. Could you help me? I'm hungry and I need some fish to eat...*";
                }
                return "*Woah! You're a Terrarian? Amazing! Can you help me? I'm hungry, but I can't seem to be able to get some fish...*";
            }

        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (RequestComplete)
            {
                if (HugPassed)
                {
                    button = "Welcome.";
                    if (HuggingPlayer == Main.myPlayer)
                    {
                        button2 = "May you put me on the floor?";
                    }
                }
                else if (HuggingPlayer == -1)
                {
                    button = "Be hugged";
                    button2 = "No way!";
                }
                else if (HuggingPlayer == Main.myPlayer)
                {
                    button = "Enough hug.";
                }
                else
                {
                    button = "";
                    button2 = "";
                }
            }
            else if (RequestTaken)
            {
                if (Main.player[Main.myPlayer].HasItem(FishID))
                {
                    button = "I've got some " + FishName;
                }
                else
                {
                    button = "";
                }
                button2 = "";
            }
            else
            {
                button = "I can try";
                button2 = "";
                //button2 = "Not right now";
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (RequestComplete)
            {
                if (firstButton)
                {
                    if (HugPassed)
                    {
                        int HuggedPlayer = HuggingPlayer;
                        WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                        //npc.Transform(ModContent.NPCType<GuardianNPC.List.BearNPC>());
                        bool PlayerHasVladimir = PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID);
                        PlayerMod.AddPlayerGuardian(Main.player[Main.myPlayer], GuardianID);
                        if (!PlayerHasVladimir) PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianID).IncreaseFriendshipProgress(1);
                        NpcMod.AddGuardianMet(GuardianID);
                        if (HuggedPlayer > -1)
                        {
                            Player player = Main.player[HuggedPlayer];
                            foreach (TerraGuardian tg in MainMod.ActiveGuardians.Values)
                            {
                                if (tg.ID == GuardianID && tg.ModID == GuardianModID)
                                {
                                    GuardianActions ga = tg.StartNewGuardianAction(0);
                                    if (ga != null)
                                    {
                                        ga.Players.Add(player);
                                        Main.npcChatText = "*Thank you! I will try finding me a empty house to move in, but first, I will wait until ask me to stop hugging you.*";
                                    }
                                    else
                                    {
                                        Main.npcChatText = "*Thank you! I could hug you more, but for some reason I can't. Just tell me whenever you need one. Now I will try looking for a house to live.*";
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Main.npcChatText = "*Thank you! I will try looking for a house for me to live. I see you another time.*";
                        }
                        return;
                    }
                    else if (HuggingPlayer == -1)
                    {
                        npc.ai[0] = 0;
                        HuggingPlayer = Main.myPlayer;
                        if (PlayerHasVladimir)
                        {
                            Main.npcChatText = "*Please be patient, this wont take long.*";
                        }
                        else
                        {
                            Main.npcChatText = "*It wont take too long.*";
                        }
                    }
                    else if (HuggingPlayer == Main.myPlayer)
                    {
                        Main.npcChatText = "*Aww... I wanted some more hug...*";
                        HuggingPlayer = -1;
                    }
                }
                else
                {
                    if (HugPassed)
                    {
                        Main.npcChatText = "*Oh! Alright, alright. Say... Can I move in to your world? Maybe there are people who need hugs here.*";
                        if (HuggingPlayer > -1)
                        {
                            Main.player[HuggingPlayer].Center = npc.Center;
                            HuggingPlayer = -1;
                        }
                    }
                    else
                    {
                        if (PlayerHasVladimir)
                        {
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    Main.npcChatText = "*We have done that before, give me a hug.*";
                                    break;
                                case 1:
                                    Main.npcChatText = "*Don't be like that, you know I wont hurt you.*";
                                    break;
                                case 2:
                                    Main.npcChatText = "*That gives me flashbacks of when we first met. Or was It another Terrarian?*";
                                    break;
                                case 3:
                                    Main.npcChatText = "*You will refuse my hug? I'm sad now.*";
                                    break;
                                case 4:
                                    Main.npcChatText = "*I've been walking for long, would be nice to hug someone friendly.*";
                                    break;
                            }
                        }
                        else
                        {
                            switch (Main.rand.Next(6))
                            {
                                case 0:
                                    Main.npcChatText = "*Come on, It's just a hug. Are you scared of me?*";
                                    break;
                                case 1:
                                    Main.npcChatText = "*I need that to make me feel better. It's been a long time since I last saw a person.*";
                                    break;
                                case 2:
                                    Main.npcChatText = "*I wont eat you or something. Come on, give me a hug.*";
                                    break;
                                case 3:
                                    Main.npcChatText = "*I'm not a bad person, I wont hurt you either, trust me.*";
                                    break;
                                case 4:
                                    Main.npcChatText = "*That saddens me, why don't you give me a hug?*";
                                    break;
                                case 5:
                                    Main.npcChatText = "*You wont end up like the fishs, I just want a hug.*";
                                    break;
                            }
                        }
                    }
                }
            }
            else if (RequestTaken)
            {
                if (firstButton)
                {
                    int Fish = Main.player[Main.myPlayer].CountItem(FishID);
                    for (int i = 0; i < 50; i++)
                    {
                        if (Main.player[Main.myPlayer].inventory[i].type == FishID)
                        {
                            Main.player[Main.myPlayer].inventory[i].SetDefaults(0);
                        }
                    }
                    if (FishsTaken + Fish > 255)
                        FishsTaken = 255;
                    else
                        FishsTaken += (byte)Fish;
                    if (FishsTaken >= FishsToTake)
                    {
                        Main.npcChatText = "*I'm stuffed. Thank you friend. You helped me, and I can help you if you give me a hug.*";
                        RequestComplete = true;
                    }
                    else
                    {
                        Main.npcChatText = "*Amazing! (He eats them) I'm still hungry... Could you get some more " + FishName + " for me?*";
                    }
                }
                else
                {

                }
            }
            else
            {
                if (firstButton)
                {
                    RequestTaken = true;
                    FishsToTake = (byte)Main.rand.Next(7, 13);
                    Main.npcChatText = "*Thank you! I need some Honeyfins. Please be fast, I'm so hungry...*";
                }
                else
                {
                    Main.npcChatText = "*Oh... I'll get back to trying to get some fish then... (Stomach growling) Be quiet, I know I'm hungry.*";
                }
            }
        }
    }
}

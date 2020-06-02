using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC
{
    public class GuardianNPCPrefab : ModNPC
    {
        public int GuardianID = 0;
        public string GuardianModID = "";
        public TerraGuardian Guardian;
        private bool TeleportFrame = true;
        private bool PlayerHasThisGuardianMetAndInvoked { get { return Main.netMode == 0 && PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianID, GuardianModID); } }
        //Later, add a script to make so the Guardian be hidden on singleplayer, if the player has it summoned.
        private bool DuplicateChecked = false;
        
        public GuardianNPCPrefab(int ID, string ModID = "")
        {
            GuardianID = ID;
            if (ModID == "")
                ModID = MainMod.mod.Name;
            GuardianModID = ModID;
            Guardian = new TerraGuardian(ID, ModID);
        }

        public void UnlockGuardian()
        {
            NpcMod.AddGuardianMet(GuardianID, GuardianModID);
        }

        public override bool Autoload(ref string name)
        {
            name = "TerraGuardian";
            return mod.Properties.Autoload;
        }

        public static int GetGuardianNpcPosition(int GuardianID, string ModID = "")
        {
            return NpcMod.GetGuardianNPC(GuardianID, ModID);
        }

        public override string TownNPCName()
        {
            return Guardian.Name;
        }

        public override string Texture
        {
            get
            {
                return "giantsummon/GuardianNPC/BlankNPC";
            }
        }

        public override bool CheckActive()
        {
            return true;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 1;
            NPCID.Sets.HatOffsetY[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.dontTakeDamageFromHostiles = npc.dontTakeDamage = true;
            npc.lifeMax = 1;
            npc.aiStyle = 7;
            animationType = NPCID.Guide;
        }

        public override bool CanChat()
        {
            return !PlayerHasThisGuardianMetAndInvoked;
        }

        public override bool CanGoToStatue(bool toKingStatue)
        {
            return Guardian.Base.Male == toKingStatue;
        }

        public override void OnGoToStatue(bool toKingStatue)
        {
            TeleportFrame = true;
        }

        public override bool UsesPartyHat()
        {
            bool acc;
            Guardian.ReturnEquippableHeadVanityEquip(out acc);
            return false || !PlayerHasThisGuardianMetAndInvoked && (acc || Guardian.ReturnEquippableHeadVanityEquip(out acc) == -1);
        }
        
        public override void AI()
        {
            npc.breath = 100;
            if (!DuplicateChecked)
            {
                for (int n = 0; n < npc.whoAmI; n++)
                {
                    if (Main.npc[n].type == npc.type)
                    {
                        npc.active = false;
                        return;
                    }
                }
                DuplicateChecked = true;
            }
            npc.dontTakeDamageFromHostiles = npc.dontTakeDamage = true;
            npc.life = npc.lifeMax;
            if (!NpcMod.HasGuardianNPC(GuardianID, GuardianModID))
            {
                npc.homeTileX = npc.homeTileY = -1;
                npc.homeless = true;
            }
            if (MainMod.CheckIfGuardianNpcsCanStayAtHomeCheck)
            {
                if (!WorldMod.CanSpawnGuardianNPC(GuardianID, GuardianModID))
                {
                    bool HasPlayerNearby = false;
                    for (int p = 0; p < 255; p++)
                    {
                        Player player = Main.player[p];
                        if (player.active && Math.Abs(player.Center.X - npc.Center.X) < NPC.sWidth && Math.Abs(player.Center.Y - npc.Center.Y) < NPC.sHeight)
                        {
                            HasPlayerNearby = true;
                            break;
                        }
                    }
                    if (!HasPlayerNearby)
                    {
                        if (PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], GuardianID, GuardianModID))
                        {
                            Main.NewText(Guardian.Name + Guardian.Base.LeavingWorldMessageGuardianSummoned);
                        }
                        else
                        {
                            Main.NewText(Guardian.Name + Guardian.Base.LeavingWorldMessage);
                        }
                        npc.active = false;
                    }
                }
            }
            bool PlayerHasThisGuardianMetAndInvoked = this.PlayerHasThisGuardianMetAndInvoked;
            if (PlayerHasThisGuardianMetAndInvoked)
            {
                if (npc.homeless)
                {
                    npc.position.X = 0;
                    npc.position.Y = 0;
                }
                else
                {
                    npc.position.X = npc.homeTileX * 16 - npc.width * 0.5f;
                    npc.position.Y = npc.homeTileY * 16 - npc.height;
                }
                TeleportFrame = true;
            }
            if (Guardian != null)
            {
                if (CheckingRequest && Main.player[Main.myPlayer].talkNPC != npc.whoAmI)
                {
                    CheckingRequest = false;
                }
                if (PlayerHasThisGuardianMetAndInvoked)
                {
                    if (!npc.homeless)
                    {
                        npc.position.X = npc.homeTileX * 16 - npc.width * 0.5f;
                        npc.position.Y = npc.homeTileY * 16 - npc.height;
                    }
                    Guardian.Position.X = npc.position.X + npc.width * 0.5f;
                    Guardian.Position.Y = npc.position.Y + npc.height - 1;
                    Guardian.SetFallStart();
                    //npc.width = npc.height = 1;
                    npc.hide = true;
                }
                else
                {
                    npc.hide = false;
                    if (npc.position.X < 16 || npc.position.Y < 16)
                    {
                        TeleportFrame = true;
                        npc.position.X = Main.spawnTileX * 16;
                        npc.position.Y = Main.spawnTileY * 16;
                    }
                    if (TeleportFrame)
                    {
                        Guardian.Position.X = npc.position.X + npc.width * 0.5f;
                        Guardian.Position.Y = npc.position.Y + npc.height - (Guardian.Height - npc.height) - 1;
                        Guardian.SetFallStart();
                        Guardian.AddCooldown(GuardianCooldownManager.CooldownType.WaitTime, 200 + Main.rand.Next(200));
                        TeleportFrame = false;
                    }
                    bool WasDefeatedBefore = Guardian.Downed;
                    Guardian.Active = npc.active;
                    Guardian.Update();
                    npc.position.X = Guardian.Position.X - npc.width * 0.5f;
                    npc.position.Y = Guardian.Position.Y - npc.height;
                    npc.velocity = Guardian.Velocity;
                    npc.width = Guardian.Width;
                    npc.height = Guardian.Height;
                    //npc.lifeMax = Guardian.MHP;
                    //npc.life = Guardian.HP;
                    npc.direction = Guardian.Direction;
                    //if (npc.life < 1) npc.life = 1;
                    if (!WasDefeatedBefore && Guardian.Downed)
                    {
                        Main.NewText(Guardian.Name + " was slain...", 255, 0, 0);
                    }
                    /*if (Guardian.Downed && Guardian.WakeupTime == 1)
                    {
                        npc.active = false;
                        Guardian = null;
                    }*/
                }
            }
            else
            {
                npc.active = false;
            }
        }

        public bool FirstEncounter
        {
            get
            {
                return !NpcMod.HasMetGuardian(GuardianID, GuardianModID) && !NpcMod.HasGuardianNPC(GuardianID, GuardianModID);
            }
        }

        public override void DrawEffects(ref Microsoft.Xna.Framework.Color drawColor)
        {
            if (!PlayerHasThisGuardianMetAndInvoked)
            {
                Guardian.Draw();
            }
            base.DrawEffects(ref drawColor);
        }

        public bool MayGiveBirthdayGift()
        {
            bool GiveGift = false;
            Player player = Main.player[Main.myPlayer];
            if (!Guardian.DoAction.InUse && PlayerMod.PlayerHasGuardian(player, GuardianID, GuardianModID) && PlayerMod.IsGuardianBirthday(player, GuardianID, GuardianModID) && !PlayerMod.HasGuardianBeenGifted(player, GuardianID, GuardianModID) &&
                player.HasItem(ModContent.ItemType<Items.Misc.BirthdayPresent>()))
            {
                GuardianData gd = PlayerMod.GetPlayerGuardian(player, GuardianID, GuardianModID);
                for (int i = 0; i < 50; i++)
                {
                    if (gd.Inventory[i].type == 0)
                    {
                        GiveGift = true;
                        break;
                    }
                }
            }
            return GiveGift;
        }

        public bool CheckingRequest = false;

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Check Request"; //Request button
            if (MayGiveBirthdayGift())
            {
                button = "I have a gift for you.";
            }
            else if (Guardian.request.requestState == RequestData.RequestState.HasRequestReady)
            {
                if (CheckingRequest)
                {
                    button = "Accept";
                    button2 = "Deny";
                }
                else
                {
                    if (Guardian.request.IsTalkQuest)
                    {
                        button = "You wanted to talk to me?";
                    }
                    else
                    {
                        button = "Need Something?";
                    }
                }
            }
            else if (Guardian.request.requestState == RequestData.RequestState.RequestActive)
            {
                if (Guardian.request.RequestCompleted)
                {
                    button = "Report Request";
                }
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (PlayerHasThisGuardianMetAndInvoked)
                return;
            Player player = Main.player[Main.myPlayer];
            PlayerMod modPlayer = player.GetModPlayer<PlayerMod>();
            GuardianData data = modPlayer.GetGuardian(GuardianID, GuardianModID);
            if (firstButton)
            {
                if (MayGiveBirthdayGift())
                {
                    int GiftSlot = -1, EmptyGuardianSlot = -1;
                    for (int i = 0; i < 50; i++)
                    {
                        if (Guardian.Inventory[i].type == 0)
                        {
                            EmptyGuardianSlot = i;
                        }
                        if (player.inventory[i].type == ModContent.ItemType<Items.Misc.BirthdayPresent>())
                        {
                            GiftSlot = i;
                        }
                    }
                    if (GiftSlot > -1 && EmptyGuardianSlot > -1)
                    {
                        Guardian.Inventory[EmptyGuardianSlot] = player.inventory[GiftSlot].Clone();
                        player.inventory[GiftSlot].SetDefaults(0);
                        GuardianActions.OpenBirthdayPresent(Guardian, EmptyGuardianSlot);
                        Main.npcChatText = "*You gave the gift.*";
                        return;
                    }
                    else
                    {
                        Main.npcChatText = "(Something went wrong... Either the guardian has no inventory space free, or you don't have a gift.)";
                    }
                }
                else
                {
                    switch (data.request.requestState)
                    {
                        case RequestData.RequestState.Cooldown:
                            Main.npcChatText = MessageParser(data.Base.NoRequestMessage(Main.player[Main.myPlayer], Guardian), Guardian);
                            break;
                        case RequestData.RequestState.HasRequestReady:
                            {
                                if (data.request.IsTalkQuest)
                                {
                                    data.request.CompleteRequest(Guardian, data, player.GetModPlayer<PlayerMod>());
                                }
                                else if (!CheckingRequest)
                                {
                                    CheckingRequest = true;
                                    Main.npcChatText = data.request.GetRequestBrief(data);
                                    if (Main.npcChatText == "")
                                        Main.npcChatText = data.Base.HasRequestMessage(player, Guardian);
                                    Main.npcChatText = MessageParser(Main.npcChatText, Guardian);

                                }
                                else
                                {
                                    CheckingRequest = false;
                                    if (PlayerMod.GetPlayerAcceptedRequestCount(player) >= RequestData.MaxRequestCount)
                                    {
                                        Main.npcChatText = "(You have too many requests active.)";
                                    }
                                    else
                                    {
                                        data.request.UponAccepting();
                                        string Mes = data.request.GetRequestAccept(data);
                                        if (Mes == "")
                                            Mes = "(You accepted the request.)";
                                        Main.npcChatText = MessageParser(Mes, Guardian);
                                    }
                                }
                            }
                            break;
                        case RequestData.RequestState.RequestActive:
                            {
                                string Mes = data.request.GetRequestInfo(data);
                                if (data.request.IsTalkQuest && data.request.CompleteRequest(Guardian, data, player.GetModPlayer<PlayerMod>()))
                                {

                                }
                                else if (data.request.RequestCompleted && data.request.CompleteRequest(Guardian, data, player.GetModPlayer<PlayerMod>()))
                                {
                                    Mes = data.request.GetRequestComplete(data);
                                    if (Mes == "")
                                        Mes = data.Base.CompletedRequestMessage(player, Guardian);
                                    Main.npcChatText = MessageParser(Mes, Guardian);
                                }
                                else
                                {
                                    if (Mes == "")
                                    {
                                        Mes = "(It gave you a list of things you need to do.)";
                                    }
                                    Mes += "\n---------------------";
                                    foreach (string s in data.request.GetRequestText(player, data))
                                    {
                                        Mes += "\n" + s;
                                    }
                                    Main.npcChatText = MessageParser(Mes, Guardian);
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                if (data.request.requestState == RequestData.RequestState.HasRequestReady && CheckingRequest)
                {
                    CheckingRequest = false;
                    data.request.UponRejecting();
                    Main.npcChatText = MessageParser(data.request.GetRequestDeny(data), Guardian);
                }
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            //return false;
            return (GuardianID == 0 && (GuardianModID == "" || GuardianModID == MainMod.mod.Name) && !NpcMod.HasMetGuardian(0, "")) || (NpcMod.HasMetGuardian(GuardianID, GuardianModID) && WorldMod.CanSpawnGuardianNPC(GuardianID, GuardianModID));
        }

        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            bool TallEnough = true;
            int ExpectedMinimumHeight = Guardian.Height / 16;
            if (bottom - top < ExpectedMinimumHeight)
            {
                TallEnough = false;
                //if(NPC.AnyNPCs(npc.type))
                //    Main.NewText("This house is not big enough for this Guardian. It need to be " + (ExpectedMinimumHeight - (bottom - top)) + " block taller for it to move in.");
            }
            return TallEnough;
        }

        public override string GetChat()
        {
            string Message = "*You talked to [name].*";
            Player player = Main.player[Main.myPlayer];
            PlayerMod modPlayer = player.GetModPlayer<PlayerMod>();
            if (!modPlayer.HasGuardian(GuardianID, GuardianModID))
            {
                modPlayer.AddNewGuardian(GuardianID, GuardianModID);
                if(npc.type == WorldMod.SpawnGuardian.Key)
                    modPlayer.GetGuardian(GuardianID, GuardianModID).IsStarter = true;
                Message = Guardian.Base.GreetMessage(player, Guardian);
            }
            else
            {
                if (PlayerMod.IsGuardianBirthday(player, GuardianID, GuardianModID) && Main.rand.Next(2) == 0)
                    Message = Guardian.Base.BirthdayMessage(player, Guardian);
                else
                {
                    string t;
                    if (Guardian.Data.CheckForImportantMessages(out t))
                    {
                        Message = t;
                    }
                    else if (npc.homeless)
                        Message = Guardian.Base.HomelessMessage(player, Guardian);
                    else
                        Message = Guardian.Base.NormalMessage(player, Guardian);
                }
            }
            if (!NpcMod.HasMetGuardian(GuardianID, GuardianModID))
            {
                Message = Guardian.Base.GreetMessage(player, Guardian);
                NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                if (WorldMod.HasEmptyGuardianNPCSlot())
                    WorldMod.AllowGuardianNPCToSpawn(GuardianID, GuardianModID);
                if (modPlayer.HasGuardian(GuardianID, GuardianModID))
                    modPlayer.GetGuardian(GuardianID, GuardianModID).IncreaseFriendshipProgress(1);
            }
            else if(PlayerHasThisGuardianMetAndInvoked)
            {
                Message = "Booo....";
            }
            GuardianData data = modPlayer.GetGuardian(GuardianID, GuardianModID);
            if(!Guardian.DoAction.InUse || Guardian.DoAction.NpcCanFacePlayer)Guardian.LookingLeft = player.Center.X < Guardian.CenterPosition.X;
            Guardian.LastFriendshipLevel = 255;
            Guardian.FriendshipLevel = data.FriendshipLevel;
            Guardian.FriendshipProgression = data.FriendshipProgression;
            return MessageParser(Message, Guardian);
        }

        public static string MessageParser(string Message, TerraGuardian guardian)
        {
            Message = Message.Replace("[name]", guardian.Name);
            //Message += "[gn:1:""]";
            string FinalMessage = "";
            string CommandType = "";
            string CommandValue = "", CommandValue2 = "";
            bool ParsingCommand = false, GettingValue = false, GettingValue2 = false;
            for (int s = 0; s < Message.Length; s++)
            {
                Char c = Message[s];
                if (c == '[')
                {
                    ParsingCommand = true;
                    GettingValue = false;
                    GettingValue2 = false;
                    CommandType = "";
                    CommandValue = "";
                    CommandValue2 = "";
                }
                else if (c == ']')
                {
                    ParsingCommand = false;
                    if (CommandType == "gn")
                    {
                        PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                        int gi = int.Parse(CommandValue);
                        string mi = CommandValue2;
                        if (player.HasGuardian(gi, mi))
                        {
                            FinalMessage += player.GetGuardian(gi).Name;
                        }
                        else
                        {
                            FinalMessage += GuardianBase.GetGuardianBase(gi).Name;
                        }
                    }
                    else if (CommandType == "nn")
                    {
                        int ni = int.Parse(CommandValue);
                        if (NPC.AnyNPCs(ni))
                        {
                            FinalMessage += NPC.firstNPCName(ni);
                        }
                        else
                        {
                            FinalMessage += "???";
                        }
                    }
                    else
                    {
                        FinalMessage += "[" + CommandType +"]";
                        CommandValue = CommandValue2 = "";
                        GettingValue = GettingValue2 = false;
                    }
                }
                else if (ParsingCommand)
                {
                    if (c == ':')
                    {
                        if (!GettingValue)
                        {
                            GettingValue = true;
                            CommandValue = "";
                        }
                        else if (!GettingValue2)
                        {
                            GettingValue2 = true;
                            CommandValue2 = "";
                        }
                    }
                    else if (!GettingValue)
                        CommandType += c;
                    else if (GettingValue2)
                    {
                        CommandValue2 += c;
                    }
                    else
                    {
                        CommandValue += c;
                    }
                }
                else
                {
                    FinalMessage += c;
                }
            }
            return FinalMessage;
        }
    }
}

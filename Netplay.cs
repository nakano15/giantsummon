using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class Netplay
    {
        private static bool MultiplayerActive = false;

        private static ModPacket StartNewMessage(MessageIDs mID)
        {
            ModPacket packet = MainMod.GetPacket;
            packet.Write((byte)mID);
            return packet;
        }

        public static void GetMessage(BinaryReader reader, int whoAmI)
        {
            MessageIDs mID = (MessageIDs)reader.ReadByte();
            if (Main.netMode == 2)
                Console.WriteLine("Received message: " + mID.ToString());
            else
                Main.NewText("Received message: " + mID.ToString());
            switch (mID)
            {
                case MessageIDs.SpawnCompanionOnPlayer:
                    GetSpawnCompanionOnPlayer(reader, whoAmI);
                    return;
                case MessageIDs.DespawnCompanionOnPlayer:
                    GetDespawnCompanionOnPlayer(reader, whoAmI);
                    return;
                case MessageIDs.GuardianInventoryItem:
                    GetGuardianInventoryItem(reader, whoAmI);
                    return;
                case MessageIDs.GuardianEquippedItem:
                    GetGuardianEquippedItem(reader, whoAmI);
                    return;
                case MessageIDs.GuardianWhoAmIDUpdate:
                    GetGuardianWhoAmIDUpdate(reader, whoAmI);
                    return;
                case MessageIDs.GuardianMovementUpdate:
                    GetGuardianMovementUpdate(reader, whoAmI);
                    return;
                case MessageIDs.GuardianItemUseUpdate:
                    GetGuardianItemUseUpdate(reader, whoAmI);
                    return;
                case MessageIDs.GuardianHurt:
                    GetGuardianHurt(reader, whoAmI);
                    return;
                case MessageIDs.GuardianBuffUpdate:
                    GetGuardianBuffUpdate(reader, whoAmI);
                    return;
            }
        }

        public static void SendSpawnCompanionOnPlayer(PlayerMod pm, int CompanionPosID, byte AssistSlot, int WhoAmID, int ToWho = -1, int FromWho = -1)
        {
            if (!MultiplayerActive || Main.netMode == 0 || !pm.GetGuardianFromSlot(AssistSlot).Active)
                return;
            ModPacket packet = StartNewMessage(MessageIDs.SpawnCompanionOnPlayer);
            packet.Write((byte)pm.player.whoAmI);
            packet.Write(CompanionPosID);
            packet.Write(AssistSlot);
            packet.Write(WhoAmID);
            packet.Write(pm.MyGuardians[CompanionPosID].ID);
            packet.Write(pm.MyGuardians[CompanionPosID].ModID);
            packet.Write(pm.MyGuardians[CompanionPosID].IsStarter);
            packet.Send(ToWho, (FromWho == -1 ? pm.player.whoAmI : FromWho));
        }

        public static void GetSpawnCompanionOnPlayer(BinaryReader reader, int WhoAmI) //On server, need not only to get the server WhoAmID of the character, but also send that to the player who created it, and send the updated change to other players.
        {
            byte PlayerID = reader.ReadByte();
            int MyGuardianPosID = reader.ReadInt32();
            byte AssistSlot = reader.ReadByte();
            int ReceivedWhoAmID = reader.ReadInt32();
            int CompanionID = reader.ReadInt32();
            string CompanionModID = reader.ReadString();
            bool Starter = reader.ReadBoolean();
            Player player = Main.player[PlayerID];
            if (PlayerID == Main.myPlayer)
                return;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            if (!pm.MyGuardians.ContainsKey(MyGuardianPosID))
            {
                if (!pm.AddNewGuardian(CompanionID, CompanionModID, MyGuardianPosID, Starter))
                {
                    return;
                }
            }
            else
            {
                if (!pm.MyGuardians[MyGuardianPosID].MyID.IsSameID(CompanionID, CompanionModID))
                {
                    pm.MyGuardians.Remove(MyGuardianPosID);
                    if (!pm.AddNewGuardian(CompanionID, CompanionModID, MyGuardianPosID, Starter))
                        return;
                }
            }
            if(pm.GetGuardianFromSlot(AssistSlot).Active)
                pm.DismissGuardian(AssistSlot);
            pm.CallGuardian(MyGuardianPosID, AssistSlot);
            if(Main.netMode == 2)
            {
                int NewWhoAmID = pm.GetGuardianFromSlot(AssistSlot).WhoAmID;
                SendGuardianWhoAmIDUpdate(ReceivedWhoAmID, NewWhoAmID, WhoAmI);
                SendSpawnCompanionOnPlayer(pm, MyGuardianPosID, AssistSlot, NewWhoAmID, -1, WhoAmI);
            }
            else
            {
                pm.GetGuardianFromSlot(AssistSlot).WhoAmID = ReceivedWhoAmID;
                if (MainMod.ActiveGuardians.ContainsKey(ReceivedWhoAmID))
                {
                    do
                    {
                        MainMod.ActiveGuardians[ReceivedWhoAmID].WhoAmID = TerraGuardian.IDStack++;
                    }
                    while (MainMod.ActiveGuardians.ContainsKey(MainMod.ActiveGuardians[ReceivedWhoAmID].WhoAmID));
                }
            }
        }

        public static void SendDespawnCompanionOnPlayer(PlayerMod pm, byte AssistSlot, int ToWho = -1, int FromWho = -1)
        {
            if (!MultiplayerActive || Main.netMode == 0)
                return;
            ModPacket packet = StartNewMessage(MessageIDs.DespawnCompanionOnPlayer);
            packet.Write((byte)pm.player.whoAmI);
            packet.Write(AssistSlot);
            packet.Send(ToWho, (FromWho == -1 ? pm.player.whoAmI : FromWho));
        }

        public static void GetDespawnCompanionOnPlayer(BinaryReader reader, int WhoAmI)
        {
            byte PlayerID = reader.ReadByte();
            byte AssistSlot = reader.ReadByte();
            Player player = Main.player[PlayerID];
            if (PlayerID == Main.myPlayer)
                return;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            if (pm.GetGuardianFromSlot(AssistSlot).Active)
                pm.DismissGuardian(AssistSlot);
            if(Main.netMode == 2)
            {
                SendDespawnCompanionOnPlayer(pm, AssistSlot, -1, WhoAmI);
            }
        }

        public static void SendGuardianInventoryItem(PlayerMod pm, int MyGuardianPosID, int Slot, int ToWho = -1, int FromWho = -1)
        {
            if (!MultiplayerActive || Main.netMode == 0 || !pm.MyGuardians.ContainsKey(MyGuardianPosID))
                return;
            ModPacket packet = StartNewMessage(MessageIDs.GuardianInventoryItem);
            packet.Write((byte)pm.player.whoAmI);
            packet.Write(MyGuardianPosID);
            packet.Write(Slot);
            Item i = pm.MyGuardians[MyGuardianPosID].Inventory[Slot];
            packet.Write(i.netID);
            if (i.netID != 0)
            {
                packet.Write(i.prefix);
                packet.Write(i.stack);
            }
            ItemIO.SendModData(i, packet);
            packet.Send(ToWho, (FromWho == -1 ? pm.player.whoAmI : FromWho));
        }

        public static void GetGuardianInventoryItem(BinaryReader reader, int WhoAmI)
        {
            byte PlayerID = reader.ReadByte();
            int MyGuardianPos = reader.ReadInt32();
            int Slot = reader.ReadInt32();
            int NetID = reader.ReadInt32();
            byte Prefix = (byte)(NetID == 0 ? 0 : reader.ReadByte());
            int Stack = (NetID == 0 ? 0 : reader.ReadInt32());
            Item i = new Item();
            i.netDefaults(NetID);
            i.Prefix(Prefix);
            i.stack = Stack;
            ItemIO.ReceiveModData(i, reader);
            Player player = Main.player[PlayerID];
            if (PlayerID == Main.myPlayer)
                return;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            if (!pm.MyGuardians.ContainsKey(MyGuardianPos))
                return;
            GuardianData tg = pm.MyGuardians[MyGuardianPos];
            tg.Inventory[Slot] = i;
            if(Main.netMode == 2)
            {
                SendGuardianInventoryItem(pm, MyGuardianPos, Slot, -1, WhoAmI);
            }
        }

        public static void SendGuardianEquippedItem(PlayerMod pm, int MyGuardianPosID, int Slot, int ToWho = -1, int FromWho = -1)
        {
            if (!MultiplayerActive || Main.netMode == 0 || !pm.MyGuardians.ContainsKey(MyGuardianPosID))
                return;
            ModPacket packet = StartNewMessage(MessageIDs.GuardianEquippedItem);
            packet.Write((byte)pm.player.whoAmI);
            packet.Write(MyGuardianPosID);
            packet.Write(Slot);
            Item i = pm.MyGuardians[MyGuardianPosID].Equipments[Slot];
            packet.Write(i.netID);
            if (i.netID != 0)
            {
                packet.Write(i.prefix);
                packet.Write(i.stack);
            }
            ItemIO.SendModData(i, packet);
            packet.Send(ToWho, (FromWho == -1 ? pm.player.whoAmI : FromWho));
        }

        public static void GetGuardianEquippedItem(BinaryReader reader, int WhoAmI)
        {
            byte PlayerID = reader.ReadByte();
            int MyGuardianPosID = reader.ReadInt32();
            int Slot = reader.ReadInt32();
            int NetId = reader.ReadInt32();
            byte Prefix = (byte)(NetId == 0 ? 0 : reader.ReadByte());
            int Stack = (NetId == 0 ? 0 : reader.ReadInt32());
            Item i = new Item();
            i.netDefaults(NetId);
            i.Prefix(Prefix);
            i.stack = Stack;
            ItemIO.ReceiveModData(i, reader);
            Player player = Main.player[PlayerID];
            if (PlayerID == Main.myPlayer)
                return;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            if (!pm.MyGuardians.ContainsKey(MyGuardianPosID))
                return;
            pm.MyGuardians[MyGuardianPosID].Equipments[Slot] = i;
            if(Main.netMode == 2)
            {
                SendGuardianEquippedItem(pm, MyGuardianPosID, Slot, -1, WhoAmI);
            }
        }

        public static void SendGuardianWhoAmIDUpdate(int OldWhoAmID, int NewWhoAmID, int ToWho =-1, int FromWho = -1)
        {
            if (!MultiplayerActive || Main.netMode == 0)
                return;
            ModPacket packet = StartNewMessage(MessageIDs.GuardianWhoAmIDUpdate);
            packet.Write(OldWhoAmID);
            packet.Write(NewWhoAmID);
            packet.Send(ToWho, FromWho);
        }

        public static void GetGuardianWhoAmIDUpdate(BinaryReader reader, int WhoAmI)
        {
            int OldID = reader.ReadInt32();
            int NewID = reader.ReadInt32();
            if (MainMod.ActiveGuardians.ContainsKey(OldID))
            {
                MainMod.ActiveGuardians[OldID].WhoAmID = NewID;
                if (MainMod.ActiveGuardians.ContainsKey(NewID))
                {
                    do
                    {
                        MainMod.ActiveGuardians[NewID].WhoAmID = TerraGuardian.IDStack++;
                    }
                    while (MainMod.ActiveGuardians.ContainsKey(MainMod.ActiveGuardians[NewID].WhoAmID));
                }
            }
        }

        public static void SendGuardianMovementUpdate(int GuardianWhoAmID, int ToWho = -1, int FromWho = -1)
        {
            if (!MultiplayerActive || Main.netMode == 0 || !MainMod.ActiveGuardians.ContainsKey(GuardianWhoAmID))
                return;
            ModPacket packet = StartNewMessage(MessageIDs.GuardianMovementUpdate);
            TerraGuardian tg = MainMod.ActiveGuardians[GuardianWhoAmID];
            BitsByte firstControls = new BitsByte(tg.MoveUp, tg.MoveDown, tg.MoveLeft, tg.MoveRight, tg.Jump);
            packet.Write(GuardianWhoAmID);
            packet.WriteVector2(tg.Position);
            packet.WriteVector2(tg.Velocity);
            packet.Write(firstControls);
            packet.Send(ToWho, FromWho);
        }

        public static void GetGuardianMovementUpdate(BinaryReader reader, int SenderWhoAmI)
        {
            int WhoAmID = reader.ReadInt32();
            Vector2 Position = reader.ReadVector2();
            Vector2 Velocity = reader.ReadVector2();
            BitsByte controls = reader.ReadByte();
            if (!MainMod.ActiveGuardians.ContainsKey(WhoAmID))
                return;
            TerraGuardian tg = MainMod.ActiveGuardians[WhoAmID];
            tg.Position = Position;
            tg.Velocity = Velocity;
            tg.MoveUp = controls[0];
            tg.MoveDown = controls[1];
            tg.MoveLeft = controls[2];
            tg.MoveRight = controls[3];
            tg.Jump = controls[4];
            if (Main.netMode == 2)
            {
                SendGuardianMovementUpdate(WhoAmID, -1, SenderWhoAmI);
            }
        }

        public static void SendGuardianItemUseUpdate(int GuardianWhoAmID, int ToWho = -1, int FromWho = -1)
        {
            if (!MultiplayerActive || Main.netMode == 0 || !MainMod.ActiveGuardians.ContainsKey(GuardianWhoAmID))
                return;
            ModPacket packet = StartNewMessage(MessageIDs.GuardianItemUseUpdate);
            TerraGuardian tg = MainMod.ActiveGuardians[GuardianWhoAmID];
            BitsByte ActionPress = new BitsByte(tg.Action, tg.OffHandAction);
            BitsByte LastActionPress = new BitsByte(tg.LastAction, tg.LastOffHandAction);
            Vector2 MousePosition = tg.AimPosition;
            packet.Write(GuardianWhoAmID);
            packet.Write(ActionPress);
            packet.Write(LastActionPress);
            packet.WriteVector2(MousePosition);
            packet.Write(tg.SelectedItem);
            packet.Write(tg.SelectedOffhand);
            packet.Send(ToWho, FromWho);
        }

        public static void GetGuardianItemUseUpdate(BinaryReader reader, int SenderWhoAmI)
        {
            int WhoAmID = reader.ReadInt32();
            BitsByte ActionPress = reader.ReadByte();
            BitsByte LastActionPress = reader.ReadByte();
            Vector2 AimPosition = reader.ReadVector2();
            int SelectedItem = reader.ReadInt32();
            int SelectedOffhandItem = reader.ReadInt32();
            if (!MainMod.ActiveGuardians.ContainsKey(WhoAmID))
                return;
            TerraGuardian tg = MainMod.ActiveGuardians[WhoAmID];
            tg.Action = ActionPress[0];
            tg.OffHandAction = ActionPress[1];
            tg.LastAction = LastActionPress[0];
            tg.LastOffHandAction = LastActionPress[1];
            tg.AimDirection = AimPosition - tg.CenterPosition;
            tg.SelectedItem = SelectedItem;
            tg.SelectedOffhand = SelectedOffhandItem;
            if(Main.netMode == 2)
            {
                SendGuardianItemUseUpdate(WhoAmID, -1, SenderWhoAmI);
            }
        }

        public static void SendGuardianHurt(int WhoAmID, int Damage, int Direction, bool Critical, string DeathMessage, int ToWho = -1, int FromWho = -1)
        {
            if (!MultiplayerActive || Main.netMode == 0 || !MainMod.ActiveGuardians.ContainsKey(WhoAmID))
                return;
            ModPacket packet = StartNewMessage(MessageIDs.GuardianHurt);
            TerraGuardian tg = MainMod.ActiveGuardians[WhoAmID];
            packet.Write(WhoAmID);
            packet.WriteVector2(tg.Position);
            packet.Write(Damage);
            packet.Write((sbyte)Direction);
            packet.Write(Critical);
            packet.Write(DeathMessage);
            byte DownState = 0;
            if (tg.Downed)
                DownState = 3;
            else if (tg.KnockedOutCold)
                DownState = 2;
            else if (tg.KnockedOut)
                DownState = 1;
            packet.Write(DownState);
            packet.Write(tg.HP);
            packet.Send(ToWho, FromWho);
        }

        public static void GetGuardianHurt(BinaryReader reader, int SenderWhoAmI)
        {
            int WhoAmID = reader.ReadInt32();
            Vector2 Position = reader.ReadVector2();
            int Damage = reader.ReadInt32();
            sbyte Direction = reader.ReadSByte();
            bool Critical = reader.ReadBoolean();
            string DeathMessage = reader.ReadString();
            byte DownState = reader.ReadByte();
            int HP = reader.ReadInt32();
            if (!MainMod.ActiveGuardians.ContainsKey(WhoAmID))
                return;
            TerraGuardian tg = MainMod.ActiveGuardians[WhoAmID];
            tg.Position = Position;
            tg.Hurt(Damage, (int)Direction, Critical, true);
            switch (DownState)
            {
                case 3:
                    tg.Knockout(DeathMessage);
                    break;
                case 2:
                    if(!tg.KnockedOut)
                        tg.EnterDownedState();
                    tg.KnockedOutCold = true;
                    break;
                case 1:
                    if (!tg.KnockedOut)
                        tg.EnterDownedState();
                    tg.KnockedOutCold = false;
                    break;
            }
            tg.HP = HP;
            if(Main.netMode == 2)
            {
                SendGuardianHurt(WhoAmID, Damage, (int)Direction, Critical, DeathMessage, -1, SenderWhoAmI);
            }
        }

        public static void SendGuardianBuffUpdate(int WhoAmID, int BuffID, int BuffTime, int ToWho = -1, int FromWho = -1)
        {
            if (!MultiplayerActive || Main.netMode == 0 || !MainMod.ActiveGuardians.ContainsKey(WhoAmID))
                return;
            ModPacket packet = StartNewMessage(MessageIDs.GuardianBuffUpdate);
            packet.Write(WhoAmID);
            packet.Write(BuffID);
            packet.Write(BuffTime);
            packet.Send(ToWho, FromWho);
        }

        public static void GetGuardianBuffUpdate(BinaryReader reader, int SenderWhoAmI)
        {
            int WhoAmID = reader.ReadInt32();
            int BuffID = reader.ReadInt32();
            int BuffTime = reader.ReadInt32();
            if (!MainMod.ActiveGuardians.ContainsKey(WhoAmID))
                return;
            TerraGuardian tg = MainMod.ActiveGuardians[WhoAmID];
            tg.AddBuff(BuffID, BuffTime, true);
            if(Main.netMode == 2)
            {
                SendGuardianBuffUpdate(WhoAmID, BuffID, BuffTime, -1, SenderWhoAmI);
            }
        }

        public enum MessageIDs
        {
            SpawnCompanionOnPlayer,
            DespawnCompanionOnPlayer,
            GuardianInventoryItem,
            GuardianEquippedItem,
            GuardianWhoAmIDUpdate,
            GuardianMovementUpdate,
            GuardianItemUseUpdate,
            GuardianHurt,
            GuardianBuffUpdate
        }
    }
}

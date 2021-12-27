using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria;

namespace giantsummon
{
    public class Netplay
    {
        private static ModPacket StartNewMessage(MessageIDs mID)
        {
            ModPacket packet = MainMod.GetPacket;
            packet.Write((byte)mID);
            return packet;
        }

        public static void GetMessage(BinaryReader reader, int whoAmI)
        {
            MessageIDs mID = (MessageIDs)reader.ReadByte();
            /*if (Main.netMode == 2)
                Console.WriteLine("Received message: " + mID.ToString());
            else
                Main.NewText("Received message: " + mID.ToString());*/
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
            }
        }

        public static void SendSpawnCompanionOnPlayer(PlayerMod pm, int CompanionPosID, byte AssistSlot, int WhoAmID, int ToWho = -1, int FromWho = -1)
        {
            if (Main.netMode == 0 || !pm.GetGuardianFromSlot(AssistSlot).Active)
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
            if (!player.active || PlayerID == Main.myPlayer)
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
            if (Main.netMode == 0)
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
            if (!player.active || PlayerID == Main.myPlayer)
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
            if (Main.netMode == 0 || !pm.MyGuardians.ContainsKey(MyGuardianPosID))
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
            if (!player.active || PlayerID == Main.myPlayer)
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
            if (Main.netMode == 0 || !pm.MyGuardians.ContainsKey(MyGuardianPosID))
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
            if (!player.active || PlayerID == Main.myPlayer)
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
            if (Main.netMode == 0)
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

        public enum MessageIDs
        {
            SpawnCompanionOnPlayer,
            DespawnCompanionOnPlayer,
            GuardianInventoryItem,
            GuardianEquippedItem,
            GuardianWhoAmIDUpdate
        }
    }
}

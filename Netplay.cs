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
        private static ModPacket StartNewMessage
        {
            get
            {
                return MainMod.GetPacket;
            }
        }

        public static void GetMessage(BinaryReader reader, int whoAmI)
        {
            MessageIDs mID = (MessageIDs)reader.ReadByte();
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
            }
        }

        public static void SendSpawnCompanionOnPlayer(PlayerMod pm, int CompanionPosID, byte AssistSlot)
        {
            if (Main.netMode == 0 || !pm.GetGuardianFromSlot(AssistSlot).Active)
                return;
            ModPacket packet = StartNewMessage;
            packet.Write((byte)pm.player.whoAmI);
            packet.Write(CompanionPosID);
            packet.Write(AssistSlot);
            packet.Write(pm.MyGuardians[CompanionPosID].ID);
            packet.Write(pm.MyGuardians[CompanionPosID].ModID);
            packet.Send(-1, pm.player.whoAmI);
        }

        public static void GetSpawnCompanionOnPlayer(BinaryReader reader, int WhoAmI)
        {
            byte PlayerID = reader.ReadByte();
            int MyGuardianPosID = reader.ReadInt32();
            byte AssistSlot = reader.ReadByte();
            int CompanionID = reader.ReadInt32();
            string CompanionModID = reader.ReadString();
            Player player = Main.player[PlayerID];
            if (!player.active)
                return;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            if (!pm.MyGuardians.ContainsKey(MyGuardianPosID))
            {
                if (!pm.AddNewGuardian(CompanionID, CompanionModID, MyGuardianPosID, false))
                    return;
            }
            else
            {
                if (!pm.MyGuardians[MyGuardianPosID].MyID.IsSameID(CompanionID, CompanionModID))
                {
                    pm.MyGuardians.Remove(MyGuardianPosID);
                    if (!pm.AddNewGuardian(CompanionID, CompanionModID, MyGuardianPosID, false))
                        return;
                }
            }
            if(pm.GetGuardianFromSlot(AssistSlot).Active)
                pm.DismissGuardian(AssistSlot);
            pm.CallGuardian(MyGuardianPosID, AssistSlot);
        }

        public static void SendDespawnCompanionOnPlayer(PlayerMod pm, byte AssistSlot)
        {
            if (Main.netMode == 0)
                return;
            ModPacket packet = StartNewMessage;
            packet.Write((byte)pm.player.whoAmI);
            packet.Write(AssistSlot);
            packet.Send(-1, pm.player.whoAmI);
        }

        public static void GetDespawnCompanionOnPlayer(BinaryReader reader, int WhoAmI)
        {
            byte PlayerID = reader.ReadByte();
            byte AssistSlot = reader.ReadByte();
            Player player = Main.player[PlayerID];
            if (!player.active)
                return;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            if (pm.GetGuardianFromSlot(AssistSlot).Active)
                pm.DismissGuardian(AssistSlot);
        }

        public static void SendGuardianInventoryItem(PlayerMod pm, int MyGuardianPosID, int Slot)
        {
            if (Main.netMode == 0 || !pm.MyGuardians.ContainsKey(MyGuardianPosID))
                return;
            ModPacket packet = StartNewMessage;
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
            packet.Send(-1, pm.player.whoAmI);
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
            if (!player.active)
                return;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            if (!pm.MyGuardians.ContainsKey(MyGuardianPos))
                return;
            GuardianData tg = pm.MyGuardians[MyGuardianPos];
            tg.Inventory[Slot] = i;
        }

        public static void SendGuardianEquippedItem(PlayerMod pm, int MyGuardianPosID, int Slot)
        {
            if (Main.netMode == 0 || !pm.MyGuardians.ContainsKey(MyGuardianPosID))
                return;
            ModPacket packet = StartNewMessage;
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
            packet.Send(-1, pm.player.whoAmI);
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
            if (!player.active)
                return;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            if (!pm.MyGuardians.ContainsKey(MyGuardianPosID))
                return;
            pm.MyGuardians[MyGuardianPosID].Equipments[Slot] = i;
        }

        public enum MessageIDs
        {
            SpawnCompanionOnPlayer,
            DespawnCompanionOnPlayer,
            GuardianInventoryItem,
            GuardianEquippedItem
        }
    }
}

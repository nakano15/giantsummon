using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class NetMod
    {
        public static ModPacket writer { get { return MainMod.mod.GetPacket(); } }

        public static void SendGuardianData(Player player, int DataPos, int To = -1, int From = -1)
        {
            PlayerMod pmod = player.GetModPlayer<PlayerMod>();
            if (!pmod.MyGuardians.ContainsKey(DataPos))
                return;
            writer.Write((byte)MessageTypes.SendGuardianBasicData);
            writer.Write((byte)player.whoAmI);
            writer.Write(DataPos);
            GuardianData gd = pmod.MyGuardians[DataPos];
            writer.Write(gd.ID);
            writer.Write(gd.ModID);
            writer.Write(gd.Name);
            writer.Write((byte)gd.LifeCrystalHealth);
            writer.Write((byte)gd.LifeFruitHealth);
            writer.Write((byte)gd.ManaCrystals);
            writer.Send(To, From);
        }

        public static void SendGuardianInventoryItem(Player player, int DataPos, int Slot, int To = -1, int From = -1)
        {
            PlayerMod pmod = player.GetModPlayer<PlayerMod>();
            if (!pmod.MyGuardians.ContainsKey(DataPos))
                return;
            writer.Write((byte)MessageTypes.SendGuardianInventory);
            writer.Write((byte)player.whoAmI);
            writer.Write(DataPos);
            GuardianData gd = pmod.MyGuardians[DataPos];
            Item i = gd.Inventory[Slot];
            writer.Write((byte)Slot);
            writer.Write(i.type);
            writer.Write(i.stack);
            writer.Write(i.prefix);
            writer.Send(To, From);
        }

        public static void SendGuardianEquippedItem(Player player, int DataPos, int Slot, int To = -1, int From = -1)
        {
            PlayerMod pmod = player.GetModPlayer<PlayerMod>();
            if (!pmod.MyGuardians.ContainsKey(DataPos))
                return;
            writer.Write((byte)MessageTypes.SendGuardianEquipment);
            writer.Write((byte)player.whoAmI);
            writer.Write(DataPos);
            GuardianData gd = pmod.MyGuardians[DataPos];
            Item eq = gd.Equipments[Slot];
            writer.Write((byte)Slot);
            writer.Write(eq.type);
            writer.Write(eq.prefix);
            writer.Send(To, From);
        }

        public static void SendGuardianBehaviorFlags(Player player, int DataPos, int To = -1, int From = -1)
        {
            PlayerMod pmod = player.GetModPlayer<PlayerMod>();
            if (!pmod.MyGuardians.ContainsKey(DataPos))
                return;
            writer.Write((byte)MessageTypes.SendGuardianBehaviorFlags);
            writer.Write((byte)player.whoAmI);
            writer.Write(DataPos);
            GuardianData gd = pmod.MyGuardians[DataPos];
            writer.Write((byte)gd.tactic);
            writer.Write(gd.Tanker);
            //writer.Write(gd.OverrideQuickMountToMountGuardianInstead);
            writer.Write(gd.UseHeavyMeleeAttackWhenMounted);
            writer.Write(gd.AvoidCombat);
            writer.Write(gd.ChargeAhead);
            writer.Write(gd.Passive);
            writer.Write(gd.AttackMyTarget);
            writer.Write(gd.SitOnTheMount);
            writer.Write(gd.MayLootItems);
            writer.Write(gd.SetToPlayerSize);
            //writer.Write(gd.GetItemsISendtoTrash);
            writer.Write(gd.UseWeaponsByInventoryOrder);
            writer.Send(To, From);
        }

        public static void SendGuardianKeyChange(Player player, int To = -1, int From = -1)
        {
            PlayerMod pmod = player.GetModPlayer<PlayerMod>();
            writer.Write((byte)MessageTypes.SendGuardianControlChanges);
            writer.Write((byte)player.whoAmI);
            TerraGuardian guardian = pmod.Guardian;
            writer.Write(guardian.Position.X);
            writer.Write(guardian.Position.Y);
            writer.Write(guardian.Velocity.X);
            writer.Write(guardian.Velocity.Y);
            writer.Write(guardian.MoveUp);
            writer.Write(guardian.MoveRight);
            writer.Write(guardian.MoveDown);
            writer.Write(guardian.MoveLeft);
            writer.Write(guardian.Jump);
            writer.Send(To, From);
        }

        public static void SendGuardianSummonState(Player player, int GuardianPos, int To = -1, int From = -1)
        {
            PlayerMod pmod = player.GetModPlayer<PlayerMod>();
            writer.Write((byte)MessageTypes.GuardianSummon);
            writer.Write((byte)player.whoAmI);
            writer.Write(pmod.Guardian.Active);
            writer.Write(GuardianPos);
            writer.Send(To, From);
        }

        public static void SendGuardianItemUse(Player player, int To = -1, int From = -1)
        {
            PlayerMod pmod = player.GetModPlayer<PlayerMod>();
            writer.Write((byte)MessageTypes.SendGuardianItemUsage);
            writer.Write((byte)player.whoAmI);
            writer.Write(pmod.Guardian.SelectedItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="ToggleType">0 = Mount, 1 = Control, 2 = Sitting on Mount</param>
        public static void SendGuardianStateCommand(Player player, byte ToggleType, int From = -1, int To = -1)
        {
            PlayerMod pmod = player.GetModPlayer<PlayerMod>();
            writer.Write((byte)MessageTypes.SendGuardianToggleStatus);
            writer.Write((byte)player.whoAmI);
            writer.Write(ToggleType);
            if (ToggleType == 0) //Mount
            {
                writer.Write(pmod.Guardian.PlayerMounted);
            }
            else if (ToggleType == 1) //Control
            {
                writer.Write(pmod.Guardian.PlayerControl);
            }
            else if (ToggleType == 2) //Mount Sit
            {
                writer.Write(pmod.Guardian.SittingOnPlayerMount);
            }
        }

        public static void SendGuardianSkillProgress(Player player, int GuardianSlot, int SkillID, int To = -1, int From = -1)
        {
            if (!player.GetModPlayer<PlayerMod>().MyGuardians.ContainsKey(GuardianSlot))
                return;
            GuardianData gd = player.GetModPlayer<PlayerMod>().MyGuardians[GuardianSlot];
            writer.Write((byte)MessageTypes.SendGuardianSkillStatus);
            writer.Write((byte)player.whoAmI);
            writer.Write(GuardianSlot);
            writer.Write(SkillID);
            GuardianSkills skill = gd.SkillList[SkillID];
            writer.Write(skill.Level);
            writer.Send(To, From);
        }

        public static void ReceiveMessage(BinaryReader reader, int whoAmI)
        {
            MessageTypes mt = (MessageTypes)reader.ReadByte();
            switch (mt)
            {
                case MessageTypes.SendGuardianBasicData:
                    {
                        byte Player = reader.ReadByte();
                        PlayerMod player = Main.player[Player].GetModPlayer<PlayerMod>();
                        int GuardianPos = reader.ReadInt32();
                        int GuardianID = reader.ReadInt32();
                        string GuardianModID = reader.ReadString();
                        if (!player.MyGuardians.ContainsKey(GuardianPos))
                        {
                            player.MyGuardians.Add(GuardianPos, GuardianBase.GetGuardianBase(GuardianID, GuardianModID).GetGuardianData(GuardianID, GuardianModID));
                        }
                        else
                        {
                            player.MyGuardians[GuardianPos] = GuardianBase.GetGuardianBase(GuardianID, GuardianModID).GetGuardianData(GuardianID, GuardianModID);
                        }
                        string GuardianName = reader.ReadString();
                        byte GuardianLifeCrystalHealth = reader.ReadByte(),
                            GuardianLifeFruitHealth = reader.ReadByte(),
                            GuardianManaCrystal = reader.ReadByte();
                        GuardianData gd = player.MyGuardians[GuardianPos];
                        gd.Name = GuardianName;
                        gd.LifeCrystalHealth = GuardianLifeCrystalHealth;
                        gd.LifeFruitHealth = GuardianLifeFruitHealth;
                        gd.ManaCrystals = GuardianManaCrystal;
                    }
                    break;

                case MessageTypes.SendGuardianInventory:
                    {
                        byte PlayerPos = reader.ReadByte();
                        PlayerMod player = Main.player[PlayerPos].GetModPlayer<PlayerMod>();
                        int DataPos = reader.ReadInt32();
                        int InvSlotPos = reader.ReadByte();
                        int ItemType = reader.ReadInt32();
                        int Stack = reader.ReadInt32();
                        byte Prefix = reader.ReadByte();
                        if (player.MyGuardians.ContainsKey(DataPos))
                        {
                            GuardianData gd = player.MyGuardians[DataPos];
                            gd.Inventory[InvSlotPos] = new Item();
                            gd.Inventory[InvSlotPos].SetDefaults(ItemType);
                            gd.Inventory[InvSlotPos].stack = Stack;
                            gd.Inventory[InvSlotPos].Prefix(Prefix);
                        }
                    }
                    break;

                case MessageTypes.SendGuardianEquipment:
                    {
                        byte PlayerPos = reader.ReadByte();
                        PlayerMod player = Main.player[PlayerPos].GetModPlayer<PlayerMod>();
                        int DataPos = reader.ReadInt32();
                        int EquipSlot = reader.ReadByte();
                        int EquipType = reader.ReadInt32();
                        byte Prefix = reader.ReadByte();
                        if (player.MyGuardians.ContainsKey(DataPos))
                        {
                            GuardianData gd = player.MyGuardians[DataPos];
                            gd.Equipments[EquipSlot] = new Item();
                            gd.Equipments[EquipSlot].SetDefaults(EquipType);
                            gd.Equipments[EquipSlot].Prefix(Prefix);
                        }
                    }
                    break;

                case MessageTypes.SendGuardianBehaviorFlags:
                    {
                        byte PlayerPos = reader.ReadByte();
                        PlayerMod player = Main.player[PlayerPos].GetModPlayer<PlayerMod>();
                        int DataPos = reader.ReadInt32();
                        CombatTactic ct = (CombatTactic)reader.ReadByte();
                        bool Tanker = reader.ReadBoolean(),
                            //OverrideQuickMount = reader.ReadBoolean(),
                            HeavyMeleeWhenMounted = reader.ReadBoolean(),
                            AvoidCombat = reader.ReadBoolean(),
                            ChargeAhead = reader.ReadBoolean(),
                            Passive = reader.ReadBoolean(),
                            AttackMyTarget = reader.ReadBoolean(),
                            SitOnTheMount = reader.ReadBoolean(),
                            MayLootItems = reader.ReadBoolean(),
                            SetToPlayerSize = reader.ReadBoolean(),
                            //GetItemsSentToTrash = reader.ReadBoolean(),
                            UseWeaponsByInventoryOrder = reader.ReadBoolean();
                        if (player.MyGuardians.ContainsKey(DataPos))
                        {
                            GuardianData gd = player.MyGuardians[DataPos];
                            gd.Tanker = Tanker;
                            gd.UseHeavyMeleeAttackWhenMounted = HeavyMeleeWhenMounted;
                            gd.AvoidCombat = AvoidCombat;
                            gd.ChargeAhead = ChargeAhead;
                            gd.Passive = Passive;
                            gd.AttackMyTarget = AttackMyTarget;
                            gd.SitOnTheMount = SitOnTheMount;
                            gd.MayLootItems = MayLootItems;
                            gd.SetToPlayerSize = SetToPlayerSize;
                            gd.UseWeaponsByInventoryOrder = UseWeaponsByInventoryOrder;
                        }
                    }
                    break;

                case MessageTypes.SendGuardianControlChanges:
                    {
                        byte PlayerPos = reader.ReadByte();
                        PlayerMod pmod = Main.player[PlayerPos].GetModPlayer<PlayerMod>();
                        float PosX = reader.ReadSingle(), PosY = reader.ReadSingle(),
                            VelX = reader.ReadSingle(), VelY = reader.ReadSingle();
                        bool MoveUp = reader.ReadBoolean(),
                            MoveRight = reader.ReadBoolean(),
                            MoveDown = reader.ReadBoolean(),
                            MoveLeft = reader.ReadBoolean(),
                            Jump = reader.ReadBoolean();
                        TerraGuardian tg = pmod.Guardian;
                        tg.Position.X = PosX;
                        tg.Position.Y = PosY;
                        tg.Velocity.X = VelX;
                        tg.Velocity.Y = VelY;
                        tg.MoveUp = MoveUp;
                        tg.MoveRight = MoveRight;
                        tg.MoveDown = MoveDown;
                        tg.MoveLeft = MoveLeft;
                        tg.Jump = Jump;
                    }
                    break;

                case MessageTypes.GuardianSummon:
                    {
                        byte PlayerPos = reader.ReadByte();
                        bool Active = reader.ReadBoolean();
                        int GuardianPos = reader.ReadInt32();
                        PlayerMod pmod = Main.player[PlayerPos].GetModPlayer<PlayerMod>();
                        if (pmod.MyGuardians.ContainsKey(GuardianPos))
                        {
                            if (Active)
                                pmod.CallGuardian(GuardianPos);
                            else
                                pmod.DismissGuardian();
                        }
                    }
                    break;

                case MessageTypes.SendGuardianItemUsage:
                    {
                        byte PlayerPos = reader.ReadByte();
                        int ItemPos = reader.ReadInt32();
                        PlayerMod pmod = Main.player[PlayerPos].GetModPlayer<PlayerMod>();
                        if (pmod.Guardian.Active)
                        {
                            pmod.Guardian.SelectedItem = ItemPos;
                            pmod.Guardian.Action = true;
                        }
                    }
                    break;

                case MessageTypes.SendGuardianToggleStatus:
                    {
                        byte PlayerPos = reader.ReadByte();
                        byte ToggleType = reader.ReadByte();
                        bool ToggleValue = reader.ReadBoolean();
                        TerraGuardian g = Main.player[PlayerPos].GetModPlayer<PlayerMod>().Guardian;
                        if (g.Active)
                        {
                            switch (ToggleType)
                            {
                                case 0: //Mount
                                    {
                                        g.PlayerMounted = !ToggleValue;
                                        g.ToggleMount(true, false);
                                    }
                                    break;
                                case 1: //Control
                                    {
                                        g.PlayerControl = !ToggleValue;
                                        g.TogglePlayerControl(true);
                                    }
                                    break;
                                case 2: //Mount Sit
                                    {
                                        g.SittingOnPlayerMount = ToggleValue;
                                    }
                                    break;
                            }
                        }
                    }
                    break;

                case MessageTypes.SendGuardianSkillStatus:
                    {
                        byte PlayerPos = reader.ReadByte();
                        int GuardianSlot = reader.ReadInt32();
                        int SkillPos = reader.ReadInt32();
                        int Level = reader.ReadInt32();
                        PlayerMod pmod = Main.player[PlayerPos].GetModPlayer<PlayerMod>();
                        if (pmod.MyGuardians.ContainsKey(GuardianSlot))
                        {
                            pmod.MyGuardians[GuardianSlot].SkillList[SkillPos].Level = Level;
                        }
                    }
                    break;
            }
        }

        public enum MessageTypes
        {
            SendGuardianBasicData,
            GuardianSummon,
            SendGuardianInventory,
            SendGuardianEquipment,
            SendGuardianBehaviorFlags,
            SendGuardianControlChanges,
            SendGuardianItemUsage,
            SendGuardianToggleStatus,
            SendGuardianSkillStatus
        }
    }
}

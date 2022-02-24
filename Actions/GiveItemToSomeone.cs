using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Actions
{
    public class GiveItemToSomeone : GuardianActions
    {
        public Player player = null;
        public TerraGuardian tg = null;
        public int ItemSlot = 0;
        public int ItemStack = 1;
        public bool Formally = true;

        public GiveItemToSomeone(Player player, int Slot, int Stack = 1, bool Formally = true)
        {
            ID = (int)ActionIDs.GiveItemToSomeone;
            this.player = player;
            ItemSlot = Slot;
            ItemStack = Stack;
            this.Formally = Formally;
            BlockOffHandUsage = true;
            BlockTwoHandedAttack = true;
        }

        public GiveItemToSomeone(TerraGuardian tg, int Slot, int Stack = 1, bool Formally = true)
        {
            ID = (int)ActionIDs.GiveItemToSomeone;
            this.tg = tg;
            ItemSlot = Slot;
            ItemStack = Stack;
            this.Formally = Formally;
            BlockOffHandUsage = true;
            BlockTwoHandedAttack = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            switch (Step)
            {
                case 0:
                    {
                        if(guardian.Inventory[ItemSlot].type == 0 || guardian.Inventory[ItemSlot].stack < ItemStack)
                        {
                            InUse = false;
                            return;
                        }
                        if (guardian.UsingFurniture)
                        {
                            guardian.LeaveFurniture(true);
                        }
                        if (Formally)
                        {
                            ChangeStep();
                        }
                        else
                        {
                            ChangeStep(2);
                        }
                    }
                    break;

                case 1:
                    {
                        Vector2 TargetPosition = Vector2.Zero;
                        if(player != null)
                        {
                            TargetPosition = player.Bottom;
                        }
                        else
                        {
                            TargetPosition = tg.Position;
                        }
                        if(guardian.Position.X > TargetPosition.X)
                        {
                            guardian.MoveLeft = true;
                            guardian.MoveRight = false;
                        }
                        else
                        {
                            guardian.MoveRight = true;
                            guardian.MoveLeft = false;
                        }
                        if(Time >= 10 * 60)
                        {
                            guardian.Position = TargetPosition;
                            guardian.SetFallStart();
                        }
                        if(TargetPosition.X >= guardian.Position.X - guardian.Width * 0.5f && TargetPosition.X < guardian.Position.X + guardian.Width * 0.5f && 
                            TargetPosition.Y >= guardian.Position.Y -guardian.Height && TargetPosition.Y - 24 < guardian.Position.Y)
                        {
                            ChangeStep();
                        }
                    }
                    break;

                case 2:
                    {
                        if(guardian.Inventory[ItemSlot].type == 0)
                        {
                            InUse = false; //Where's the item I was going to give?
                            guardian.SaySomething(guardian.GetMessage(GuardianBase.MessageIDs.DeliveryItemMissing, 
                                "*They seems to have wanted to deliver something, but can't find it in their inventory.*"));
                            return;
                        }
                        if (StepStart)
                        {
                            if (player != null)
                            {
                                byte EmptySlot = 255;
                                byte MaxSlots = 50;
                                if (guardian.Inventory[ItemSlot].ammo > 0)
                                    MaxSlots = 54;
                                for (byte i = 0; i < MaxSlots; i++)
                                {
                                    if (player.inventory[i].type == 0)
                                    {
                                        EmptySlot = i;
                                    }
                                }
                                if (EmptySlot == 255)
                                {
                                    string Message = guardian.GetMessage(GuardianBase.MessageIDs.DeliveryInventoryFull,
                                        "*It seems like you have no room for what they wanted to give [target].*").Replace("[target]", (player.whoAmI == Main.myPlayer ? "you" : player.name));
                                    guardian.SaySomething(Message);
                                    InUse = false;
                                }
                            }
                            else
                            {
                                int EmptySlot = -1;
                                for (int i = 0; i < guardian.Inventory.Length; i++)
                                {
                                    if (guardian.Inventory[i].type == 0)
                                        EmptySlot = i;
                                }
                                if (EmptySlot == 255)
                                {
                                    string Message = guardian.GetMessage(GuardianBase.MessageIDs.DeliveryInventoryFull,
                                        "*It seems like you have no room for what they wanted to give [target].*").Replace("[target]", tg.Name);
                                    guardian.SaySomething(Message);
                                    InUse = false;
                                }
                            }
                        }
                        else if (Time >= 60)
                        {
                            if (player != null)
                            {
                                byte EmptySlot = 255;
                                byte MaxSlots = 50;
                                if (guardian.Inventory[ItemSlot].ammo > 0)
                                    MaxSlots = 54;
                                for (byte i = 0; i < MaxSlots; i++)
                                {
                                    if (player.inventory[i].type == 0)
                                    {
                                        EmptySlot = i;
                                    }
                                }
                                if (EmptySlot == 255)
                                {
                                    string Message = guardian.GetMessage(GuardianBase.MessageIDs.DeliveryInventoryFull,
                                        "*It seems like you have no room for what they wanted to give you.*").Replace("[target]", (player.whoAmI == Main.myPlayer ? "you" : player.name));
                                    guardian.SaySomething(Message);
                                }
                                else
                                {
                                    Item item = guardian.Inventory[ItemSlot].DeepClone();
                                    player.GetItem(player.whoAmI, item);
                                    guardian.Inventory[ItemSlot].SetDefaults(0, true);
                                    string Message = guardian.GetMessage(GuardianBase.MessageIDs.DeliveryGiveItem,
                                        "*"+guardian.Name+" gave [target] some [item].*").Replace("[target]", (player.whoAmI == Main.myPlayer ? "you" : player.name)).
                                        Replace("[item]", item.Name);
                                    guardian.SaySomething(Message);
                                }
                            }
                            else
                            {
                                int EmptySlot = -1;
                                for (int i = 0; i < guardian.Inventory.Length; i++)
                                {
                                    if (guardian.Inventory[i].type == 0)
                                        EmptySlot = i;
                                }
                                if (EmptySlot == 255)
                                {
                                    string Message = guardian.GetMessage(GuardianBase.MessageIDs.DeliveryInventoryFull,
                                        "*It seems like you have no room for what they wanted to give [target].*").Replace("[target]", tg.Name);
                                    guardian.SaySomething(Message);
                                }
                                else
                                {
                                    Item item = guardian.Inventory[ItemSlot].DeepClone();
                                    tg.Inventory[EmptySlot] = item;
                                    guardian.Inventory[EmptySlot].SetDefaults(0, true);
                                    string Message = guardian.GetMessage(GuardianBase.MessageIDs.DeliveryGiveItem,
                                        "*" + guardian.Name + " gave [target] some [item].*").Replace("[target]", tg.Name).
                                        Replace("[item]", item.Name);
                                    guardian.SaySomething(Message);
                                }
                            }
                            InUse = false;
                        }
                    }
                    break;
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if (Step == 2 && !UsingRightArmAnimation)
            {
                if (Formally)
                {
                    guardian.RightArmAnimationFrame = guardian.Base.ItemUseFrames[2];
                }
                else
                {
                    float AnimationPercentage = Time * (1f / 60);
                    guardian.RightArmAnimationFrame = guardian.Base.ItemUseFrames[(int)(AnimationPercentage >= 1 ? 3 : 4 * AnimationPercentage)];
                }
            }
        }

        public override void Draw(TerraGuardian guardian)
        {
            if(Step == 2 && guardian.Inventory[ItemSlot].type > 0)
            {
                Vector2 ItemSpawnPosition = guardian.GetRightHandPosition(guardian.Base.ItemUseFrames[0]);
                if (Formally)
                {
                    GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, Main.itemTexture[guardian.Inventory[ItemSlot].type], ItemSpawnPosition - Main.screenPosition, null, Color.White);
                    TerraGuardian.DrawBehind.Add(gdd);
                }
                else
                {
                    float AnimationPercentage = Time * (1f / 60);
                    Vector2 Destination = Vector2.Zero;
                    if (player != null)
                    {
                        Destination = player.Center;
                    }
                    else
                    {
                        Destination = tg.CenterPosition;
                    }
                    ItemSpawnPosition = ItemSpawnPosition + (Destination - ItemSpawnPosition) * AnimationPercentage - Main.screenPosition;
                    GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, Main.itemTexture[guardian.Inventory[ItemSlot].type], ItemSpawnPosition, null, Color.White);
                    TerraGuardian.DrawBehind.Add(gdd);
                }
            }
        }
    }
}

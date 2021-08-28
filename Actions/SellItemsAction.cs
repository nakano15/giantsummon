using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Actions
{
    public class SellItemsAction : GuardianActions
    {
        public byte TeleportMethod = 0;
        public bool LastWasPlayerMounted = false;
        public int TeleportTime = 0;

        public SellItemsAction()
        {
            ID = (int)ActionIDs.SellItems;
            InUse = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            EffectOnlyMirror = true;
            CantUseInventory = true;
            bool TeleportedEffects = false;
            switch (Step)
            {
                case 0:
                    {
                        if (StepStart)
                        {
                            string Message = guardian.GetMessage(GuardianBase.MessageIDs.LeavingToSellLoot);
                            if (Message != "") guardian.SaySomething(Message);
                            else ChangeStep();
                        }
                        if(guardian.MessageTime <= 0)
                        {
                            ChangeStep();
                        }
                    }
                    break;
                case 1: //Check distance to town, calculate time to sell items.
                    {
                        if (StepStart)
                        {
                            if (guardian.furniturex > -1)
                                guardian.LeaveFurniture();
                            if (!guardian.HasMagicMirror)
                            {
                                TeleportMethod = 1;
                                LastWasPlayerMounted = guardian.PlayerMounted;
                                if (guardian.PlayerMounted)
                                    guardian.ToggleMount(true);
                                if (guardian.SittingOnPlayerMount)
                                    guardian.DoSitOnPlayerMount(false);
                                //InUse = false;
                                //return;
                            }
                            else
                            {
                                Vector2 GuardianPos = guardian.CenterPosition;
                                Vector2 ResultPoint = new Vector2(Main.spawnTileX * 16, Main.spawnTileY * 16);
                                int Time = 60 * 10;
                                //Time += (int)Math.Abs(ResultPoint.X - GuardianPos.X) / 128;
                                //Time += (int)Math.Abs(ResultPoint.Y - GuardianPos.Y) / 128;
                                TeleportTime = Time;
                                TeleportMethod = 0;
                            }
                        }
                        if (TeleportMethod == 1)
                        {
                            //make guardian walk away from the player. If It gets away from screen distance, teleport.
                            //If passes 5 seconds and It didn't teleport, make it disappear and calculate the time until nearest vendor.
                            bool DoMove = false;
                            if (this.Time >= 60 * 5)
                                DoMove = true;
                            else if (Math.Abs(guardian.Position.X - Main.player[guardian.OwnerPos].Center.X) < NPC.sWidth * 16)
                            {
                                IgnoreCombat = true;
                                CantUseInventory = true;
                                Immune = true;
                                guardian.StuckTimer = 0;
                                guardian.MoveLeft = guardian.MoveRight = false;
                                if (Main.player[guardian.OwnerPos].Center.X - guardian.Position.X < 0)
                                {
                                    guardian.MoveRight = true;
                                }
                                else
                                {
                                    guardian.MoveLeft = true;
                                }
                            }
                            else
                            {
                                DoMove = true;
                            }
                            if (DoMove)
                            {
                                int NearestTownNPC = -1;
                                float NearestDist = -1;
                                for (int n = 0; n < 200; n++)
                                {
                                    if (Main.npc[n].active && Main.npc[n].townNPC && MainMod.VendorNpcs.Contains(Main.npc[n].type))
                                    {
                                        float Distance = (guardian.CenterPosition - Main.npc[n].Center).Length();
                                        if (NearestDist == -1 || Distance < NearestDist)
                                        {
                                            NearestTownNPC = n;
                                            NearestDist = Distance;
                                        }
                                    }
                                }
                                Vector2 ResultPosition = Vector2.Zero;
                                if (NearestTownNPC > -1)
                                {
                                    ResultPosition = Main.npc[NearestTownNPC].Center;
                                }
                                else
                                {
                                    ResultPosition.X = Main.spawnTileX * 16;
                                    ResultPosition.Y = Main.spawnTileY * 16;
                                }
                                float WalkTime = 16f / guardian.MoveSpeed;
                                int Time = (int)((Math.Abs(ResultPosition.X - guardian.Position.X) + Math.Abs(ResultPosition.Y - guardian.CenterY)) * WalkTime) / (16 * 16) + 60 * 7;
                                TeleportTime = Time;
                                //Main.NewText("Teleport time: " + Math.Round((float)Time / 60, 1) + "s.");
                                LastWasPlayerMounted = guardian.PlayerMounted;
                                if (guardian.PlayerMounted)
                                    guardian.ToggleMount(true);
                                ChangeStep();
                                TeleportedEffects = true;
                            }
                        }
                        else
                        {
                            if (guardian.ItemAnimationTime <= 0)
                            {
                                guardian.UseMagicMirror();
                            }
                            if (guardian.MagicMirrorTrigger)
                            {
                                LastWasPlayerMounted = guardian.PlayerMounted;
                                if (guardian.PlayerMounted)
                                    guardian.ToggleMount(true);
                                ChangeStep();
                                TeleportedEffects = true;
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        TeleportedEffects = true;
                        if (Time >= TeleportTime)
                        {
                            bool SendToPiggyBank = guardian.FriendshipGrade >= 3;
                            bool SoldItems = false, SentItemsToPiggyBank = false;
                            int p = 0, g = 0, s = 0, c = 0;
                            int token = 0;
                            int copperstack = 0;
                            for (int i = 10; i < 50; i++)
                            {
                                if (i != guardian.SelectedItem && guardian.Inventory[i].type != 0 && !guardian.Inventory[i].favorited)
                                {
                                    if ((guardian.Inventory[i].type < Terraria.ID.ItemID.CopperCoin || guardian.Inventory[i].type > Terraria.ID.ItemID.PlatinumCoin) &&
                                    guardian.Inventory[i].type != Terraria.ID.ItemID.DefenderMedal)
                                    {
                                        c += guardian.Inventory[i].value * guardian.Inventory[i].stack;
                                        guardian.Inventory[i].SetDefaults(0);
                                        SoldItems = true;
                                    }
                                    else if (SendToPiggyBank)
                                    {
                                        SentItemsToPiggyBank = true;
                                        switch (guardian.Inventory[i].type)
                                        {
                                            case Terraria.ID.ItemID.CopperCoin:
                                                copperstack += guardian.Inventory[i].stack;
                                                guardian.Inventory[i].SetDefaults(0);
                                                break;
                                            case Terraria.ID.ItemID.SilverCoin:
                                                s += guardian.Inventory[i].stack;
                                                guardian.Inventory[i].SetDefaults(0);
                                                break;
                                            case Terraria.ID.ItemID.GoldCoin:
                                                g += guardian.Inventory[i].stack;
                                                guardian.Inventory[i].SetDefaults(0);
                                                break;
                                            case Terraria.ID.ItemID.PlatinumCoin:
                                                p += guardian.Inventory[i].stack;
                                                guardian.Inventory[i].SetDefaults(0);
                                                break;
                                            case Terraria.ID.ItemID.DefenderMedal:
                                                token += guardian.Inventory[i].stack;
                                                guardian.Inventory[i].SetDefaults(0);
                                                break;
                                        }
                                    }
                                }
                            }
                            c = c / 5 + copperstack;
                            if (c >= 100)
                            {
                                s += c / 100;
                                c -= s * 100;
                            }
                            if (s >= 100)
                            {
                                g += s / 100;
                                s -= g * 100;
                            }
                            if (g >= 100)
                            {
                                p += g / 100;
                                g -= p * 100;
                            }
                            string ResultText = "";
                            bool First = true;
                            if (p > 0)
                            {
                                First = false;
                                ResultText += p + " Platinum";
                            }
                            if (g > 0)
                            {
                                if (!First)
                                    ResultText += ", ";
                                First = false;
                                ResultText += g + " Gold";
                            }
                            if (s > 0)
                            {
                                if (!First)
                                    ResultText += ", ";
                                First = false;
                                ResultText += s + " Silver";
                            }
                            if (c > 0)
                            {
                                if (!First)
                                    ResultText += ", ";
                                First = false;
                                ResultText += c + " Copper";
                            }
                            if (SentItemsToPiggyBank && !SoldItems)
                            {
                                ResultText = guardian.Name + " stored " + ResultText + " on your Piggy Bank";
                            }
                            else if (c == 0 && s == 0 && g == 0 && p == 0)
                            {
                                ResultText = guardian.Name + " gained nothing from selling the items";
                            }
                            else
                            {
                                ResultText = guardian.Name + " got " + ResultText + " Coins from item sale";
                                if (SendToPiggyBank)
                                    ResultText += ", and they were sent to your Piggy Bank";
                            }
                            if (token > 0)
                            {
                                ResultText += ", and stored " + token + " defender medals";
                            }
                            Main.NewText(ResultText + ".", Color.Yellow);
                            if (SendToPiggyBank && guardian.OwnerPos != -1) //Store on player piggy bank
                            {
                                Chest bank = Main.player[guardian.OwnerPos].bank;
                                for (byte Coin = 0; Coin < 5; Coin++)
                                {
                                    int EmptySlot = -1;
                                    int CoinID = Coin + Terraria.ID.ItemID.CopperCoin;
                                    if (Coin == 4)
                                        CoinID = Terraria.ID.ItemID.DefenderMedal;
                                    int CoinsToDiscount = 0;
                                    switch (Coin)
                                    {
                                        case 0:
                                            CoinsToDiscount = c;
                                            c = 0;
                                            break;
                                        case 1:
                                            CoinsToDiscount = s;
                                            s = 0;
                                            break;
                                        case 2:
                                            CoinsToDiscount = g;
                                            g = 0;
                                            break;
                                        case 3:
                                            CoinsToDiscount = p;
                                            p = 0;
                                            break;
                                        case 4:
                                            CoinsToDiscount = token;
                                            token = 0;
                                            break;
                                    }
                                    if (CoinsToDiscount == 0)
                                        continue;
                                    for (int i = 0; i < bank.item.Length; i++)
                                    {
                                        if (bank.item[i].type == 0)
                                            EmptySlot = i;
                                        if (CoinsToDiscount > 0 && bank.item[i].type == CoinID)
                                        {
                                            bank.item[i].stack += CoinsToDiscount;
                                            CoinsToDiscount = 0;
                                            if (bank.item[i].stack >= 100 && CoinID != Terraria.ID.ItemID.PlatinumCoin && CoinID != Terraria.ID.ItemID.DefenderMedal)
                                            {
                                                int NextSum = bank.item[i].stack / 100;
                                                bank.item[i].stack -= NextSum * 100;
                                                switch (Coin)
                                                {
                                                    case 0:
                                                        s += NextSum;
                                                        break;
                                                    case 1:
                                                        g += NextSum;
                                                        break;
                                                    case 2:
                                                        p += NextSum;
                                                        break;
                                                }
                                            }
                                            if (CoinID == Terraria.ID.ItemID.PlatinumCoin && bank.item[i].stack > 1000)
                                            {
                                                CoinsToDiscount = bank.item[i].stack - 1000;
                                                bank.item[i].stack = 1000;
                                            }
                                            if (CoinID == Terraria.ID.ItemID.DefenderMedal && bank.item[i].stack > 999)
                                            {
                                                CoinsToDiscount = bank.item[i].stack - 999;
                                                bank.item[i].stack = 999;
                                            }
                                            if (bank.item[i].stack == 0)
                                            {
                                                bank.item[i].SetDefaults(0);
                                            }
                                        }
                                    }
                                    while (CoinsToDiscount > 0)
                                    {
                                        if (EmptySlot > -1)
                                        {
                                            bank.item[EmptySlot].SetDefaults(CoinID);
                                            if (CoinsToDiscount > 1000)
                                            {
                                                bank.item[EmptySlot].stack = 1000;
                                                CoinsToDiscount -= 1000;
                                                EmptySlot = -1;
                                                for (int i = 0; i < bank.item.Length; i++)
                                                {
                                                    if (bank.item[i].type == 0)
                                                        EmptySlot = i;
                                                }
                                            }
                                            else
                                            {
                                                bank.item[EmptySlot].stack = CoinsToDiscount;
                                                CoinsToDiscount = 0;
                                            }
                                        }
                                        else
                                        {
                                            switch (Coin)
                                            {
                                                case 0:
                                                    c = CoinsToDiscount;
                                                    break;
                                                case 1:
                                                    s = CoinsToDiscount;
                                                    break;
                                                case 2:
                                                    g = CoinsToDiscount;
                                                    break;
                                                case 3:
                                                    p = CoinsToDiscount;
                                                    break;
                                                case 4:
                                                    token = CoinsToDiscount;
                                                    break;
                                            }
                                            CoinsToDiscount = 0;
                                        }
                                    }
                                }
                            }
                            for (int i = 0; i < 50; i++)
                            {
                                switch (guardian.Inventory[i].type)
                                {
                                    case Terraria.ID.ItemID.CopperCoin:
                                        {
                                            c += guardian.Inventory[i].stack;
                                            guardian.Inventory[i].SetDefaults(0);
                                        }
                                        break;
                                    case Terraria.ID.ItemID.SilverCoin:
                                        {
                                            s += guardian.Inventory[i].stack;
                                            guardian.Inventory[i].SetDefaults(0);
                                        }
                                        break;
                                    case Terraria.ID.ItemID.GoldCoin:
                                        {
                                            g += guardian.Inventory[i].stack;
                                            guardian.Inventory[i].SetDefaults(0);
                                        }
                                        break;
                                    case Terraria.ID.ItemID.PlatinumCoin:
                                        {
                                            p += guardian.Inventory[i].stack;
                                            guardian.Inventory[i].SetDefaults(0);
                                        }
                                        break;
                                    case Terraria.ID.ItemID.DefenderMedal:
                                        {
                                            token += guardian.Inventory[i].stack;
                                            guardian.Inventory[i].SetDefaults(0);
                                        }
                                        break;
                                }
                            }
                            if (c >= 100)
                            {
                                s += c / 100;
                                c -= s * 100;
                            }
                            if (s >= 100)
                            {
                                g += s / 100;
                                s -= g * 100;
                            }
                            if (g >= 100)
                            {
                                p += g / 100;
                                g -= p * 100;
                            }
                            for (int i = 0; i < 50; i++)
                            {
                                if (guardian.Inventory[i].type == 0)
                                {
                                    if (token > 0)
                                    {
                                        guardian.Inventory[i].SetDefaults(Terraria.ID.ItemID.DefenderMedal);
                                        guardian.Inventory[i].stack = token;
                                        token = 0;
                                    }
                                    else if (p > 0)
                                    {
                                        guardian.Inventory[i].SetDefaults(Terraria.ID.ItemID.PlatinumCoin);
                                        guardian.Inventory[i].stack = p;
                                        p = 0;
                                    }
                                    else if (g > 0)
                                    {
                                        guardian.Inventory[i].SetDefaults(Terraria.ID.ItemID.GoldCoin);
                                        guardian.Inventory[i].stack = g;
                                        g = 0;
                                    }
                                    else if (s > 0)
                                    {
                                        guardian.Inventory[i].SetDefaults(Terraria.ID.ItemID.SilverCoin);
                                        guardian.Inventory[i].stack = s;
                                        s = 0;
                                    }
                                    else if (c > 0)
                                    {
                                        guardian.Inventory[i].SetDefaults(Terraria.ID.ItemID.CopperCoin);
                                        guardian.Inventory[i].stack = c;
                                        c = 0;
                                    }
                                }
                            }
                            for (byte Coin = 0; Coin < 5; Coin++)
                            {
                                int CoinID = Coin + Terraria.ID.ItemID.CopperCoin;
                                if (Coin == 4)
                                    CoinID = Terraria.ID.ItemID.DefenderMedal;
                                int Stack = 0;
                                switch (Coin)
                                {
                                    case 0:
                                        Stack = c;
                                        break;
                                    case 1:
                                        Stack = s;
                                        break;
                                    case 2:
                                        Stack = g;
                                        break;
                                    case 3:
                                        Stack = p;
                                        if (Stack > 1000)
                                        {
                                            p -= 1000;
                                            Stack = 1000;
                                            Coin--;
                                        }
                                        break;
                                    case 4:
                                        Stack = token;
                                        if (Stack > 999)
                                        {
                                            token -= 999;
                                            Stack = 999;
                                            Coin--;
                                        }
                                        break;
                                }
                                if (Stack > 0)
                                    Item.NewItem(guardian.HitBox, CoinID, Stack);
                            }
                            ChangeStep();
                        }
                    }
                    break;
                case 3:
                    {
                        if (TeleportMethod == 0)
                        {
                            if (guardian.ItemAnimationTime <= 0)
                            {
                                guardian.UseMagicMirror();
                            }
                            if (guardian.MagicMirrorTrigger)
                            {
                                InUse = false;
                                if (LastWasPlayerMounted)
                                    guardian.ToggleMount(true);
                                else if (guardian.OwnerPos > -1)
                                    guardian.Velocity = Main.player[guardian.OwnerPos].velocity;
                            }
                            else
                            {
                                TeleportedEffects = true;
                            }
                        }
                        else
                        {
                            if (StepStart)
                            {
                                TeleportTime = 60 * 5;
                            }
                            if (Time < TeleportTime)
                            {
                                TeleportedEffects = true;
                            }
                            else
                            {
                                Main.NewText(guardian.Name + " has returned.");
                                guardian.StuckTimer = 0;
                                InUse = false;
                                if (LastWasPlayerMounted)
                                    guardian.ToggleMount();
                                else if (guardian.OwnerPos > -1)
                                    guardian.Velocity = Main.player[guardian.OwnerPos].velocity;

                            }
                        }
                    }
                    break;
            }
            IgnoreCombat = TeleportedEffects;
            //AvoidItemUsage = TeleportedEffects;
            Invisibility = TeleportedEffects;
            Immune = TeleportedEffects;
            NoAggro = TeleportedEffects;
            Inactivity = TeleportedEffects;
            if (TeleportedEffects)
            {
                if (guardian.GuardingPosition.HasValue)
                {
                    guardian.Position.X = guardian.GuardingPosition.Value.X * 16;
                    guardian.Position.Y = guardian.GuardingPosition.Value.Y * 16;
                }
                else
                {
                    guardian.Position.X = Main.player[guardian.OwnerPos].Center.X;
                    guardian.Position.Y = Main.player[guardian.OwnerPos].position.Y + Main.player[guardian.OwnerPos].height - 1;
                }
                guardian.Velocity.X = guardian.Velocity.Y = 0;
            }
        }
    }
}

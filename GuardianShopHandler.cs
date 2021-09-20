using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon
{
    public class GuardianShopHandler
    {
        private static List<GuardianShop> Shops = new List<GuardianShop>();

        public static void UnloadShops()
        {
            foreach (GuardianShop shop in Shops)
            {
                shop.Dispose();
            }
            Shops.Clear();
            Shops = null;
        }

        public static GuardianShop CreateShop(int OwnerID, string OwnerModID)
        {
            GuardianShop shop = new GuardianShop();
            shop.OwnerID = OwnerID;
            shop.OwnerModID = OwnerModID;
            Shops.Add(shop);
            return shop;
        }

        public static void SaveShops(Terraria.ModLoader.IO.TagCompound tag)
        {
            tag.Add("Shop_Count", Shops.Count);
            for (int s = 0; s < Shops.Count; s++)
            {
                string ShopTag = "Shop_s"+s+">";
                GuardianShop shop = Shops[s];
                tag.Add(ShopTag + "OwnerID", shop.OwnerID);
                tag.Add(ShopTag + "OwnerModID", shop.OwnerModID);
                tag.Add(ShopTag + "ItemCount", shop.Items.Count);
                for (int i = 0; i < shop.Items.Count; i++)
                {
                    string ItemTag = ShopTag + "i" + i + ">";
                    GuardianShopItem item = shop.Items[i];
                    bool HasItem = item.ItemID != 0;
                    tag.Add(ItemTag + "hasitem", HasItem);
                    if (HasItem)
                    {
                        Item dummyItem = new Item();
                        dummyItem.SetDefaults(item.ItemID, true);
                        bool IsModItem = dummyItem.modItem != null;
                        tag.Add(ItemTag + "ismoditem", IsModItem);
                        if (IsModItem)
                        {
                            tag.Add(ItemTag + "itemname", dummyItem.modItem.Name);
                            tag.Add(ItemTag + "itemmodname", dummyItem.modItem.mod.Name);
                        }
                        else
                        {
                            tag.Add(ItemTag + "itemid", item.ItemID);
                        }
                        tag.Add(ItemTag + "saletime", item.SaleTime);
                        tag.Add(ItemTag + "timedsaletime", item.TimedSaleTime);
                        tag.Add(ItemTag + "stack", item.Stack);
                    }
                }
            }
        }

        public static void LoadShops(Terraria.ModLoader.IO.TagCompound tag, int ModVersion)
        {
            int ShopCount = tag.GetInt("Shop_Count");
            for (int s = 0; s < ShopCount; s++)
            {
                string ShopTag = "Shop_s" + s + ">";
                int OwnerID = tag.GetInt(ShopTag + "OwnerID");
                string OwnerModID = tag.GetString(ShopTag + "OwnerModID");
                GuardianBase gb = GuardianBase.GetGuardianBase(OwnerID, OwnerModID);
                GuardianShop shop = GetShop(OwnerID, OwnerModID);
                if (shop == null)
                {
                    gb.SetupShop(OwnerID, OwnerModID);
                    shop = GetShop(OwnerID, OwnerModID);
                }
                if (shop != null)
                {
                    int ItemCount = tag.GetInt(ShopTag + "ItemCount");
                    for (int i = 0; i < ItemCount; i++)
                    {
                        string ItemTag = ShopTag + "i" + i + ">";
                        if (tag.GetBool(ItemTag + "hasitem"))
                        {
                            bool IsModItem = tag.GetBool(ItemTag + "ismoditem");
                            string ItemInternalName = null, ItemModInternalName = null;
                            int ItemID = 0;
                            if (IsModItem)
                            {
                                ItemInternalName = tag.GetString(ItemTag + "itemname");
                                ItemModInternalName = tag.GetString(ItemTag + "itemmodname");
                            }
                            else
                            {
                                ItemID = tag.GetInt(ItemTag + "itemid");
                            }
                            foreach (GuardianShopItem item in shop.Items)
                            {
                                if (item.ItemID == 0)
                                    continue;
                                if (ItemInternalName != null)
                                {
                                    Item anotheritem = new Item();
                                    anotheritem.SetDefaults(item.ItemID, true);
                                    if (anotheritem.modItem == null || anotheritem.modItem.Name != ItemInternalName || anotheritem.modItem.mod.Name != ItemModInternalName)
                                        continue;
                                }
                                else if (ItemID != item.ItemID)
                                {
                                    continue;
                                }
                                item.SaleTime = tag.GetInt(ItemTag + "saletime");
                                item.TimedSaleTime = tag.GetInt(ItemTag + "timedsaletime");
                                item.Stack = tag.GetInt(ItemTag + "stack");
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static GuardianShop GetShop(int OwnerID, string OwnerModID)
        {
            return GetShop(new GuardianID(OwnerID, OwnerModID));
        }

        public static GuardianShop GetShop(GuardianID ID)
        {
            foreach (GuardianShop shop in Shops)
            {
                if (ID.ID == shop.OwnerID && ID.ModID == shop.OwnerModID)
                    return shop;
            }
            return null;
        }

        public static bool HasShop(GuardianID ID)
        {
            foreach (GuardianShop shop in Shops)
            {
                if (ID.ID == shop.OwnerID && ID.ModID == shop.OwnerModID)
                    return true;
            }
            return false;
        }

        public static void UpdateShops()
        {
            bool PassedAnHour = (int)Main.time % 3600 == 0;
            foreach (GuardianShop shop in Shops)
            {
                foreach (GuardianShopItem item in shop.Items)
                {
                    if (item.disponibility == GuardianShopItem.DisponibilityTime.Timed)
                    {
                        if (item.TimedSaleTime > 0)
                        {
                            item.TimedSaleTime--;
                        }
                        else
                        {
                            if (PassedAnHour && Main.rand.NextDouble() < item.TimedSaleChance)
                            {
                                item.TimedSaleTime = Main.rand.Next(5, 9) * 3600 + Main.rand.Next(4, 8) * 60;
                            }
                        }
                    }
                    if (item.SaleTime > 0)
                        item.SaleTime--;
                    else if (PassedAnHour && item.IsItemDisponible() && item.SaleTime == 0 && Main.rand.NextDouble() < 0.01f)
                    {
                        item.TimedSaleTime = Main.rand.Next(3, 6) * 3600 + Main.rand.Next(10, 26) * 60;
                        int SaleStack = 1;
                        if (item.Stack >= 25)
                        {
                            SaleStack++;
                        }
                        if (item.Stack >= 50)
                        {
                            SaleStack++;
                        }
                        if (item.Price > 3000)
                        {
                            SaleStack++;
                        }
                        if (item.Price > 9000)
                        {
                            SaleStack++;
                        }
                        if (item.Price > 20000)
                        {
                            SaleStack++;
                        }
                        if (item.Price > 500000)
                        {
                            SaleStack++;
                        }
                        item.SaleFactor = Main.rand.Next(1, SaleStack) * 5 * 0.01f;
                    }
                    if (PassedAnHour && item.Stack > -1 && (!item.UniqueItem || item.Stack == 0))
                    {
                        int Difficulty = 1;
                        if (item.UniqueItem)
                            Difficulty += 4;
                        if (item.Price >= 500)
                            Difficulty++;
                        if (item.Price >= 5000)
                            Difficulty++;
                        if (item.Price >= 50000)
                            Difficulty++;
                        if (item.Price >= 500000)
                            Difficulty++;
                        if (item.Stack >= 25)
                            Difficulty++;
                        if (item.Stack >= 50)
                            Difficulty++;
                        if (item.Stack >= 100)
                            Difficulty++;
                        if (item.Stack >= 250)
                            Difficulty++;
                        if (item.Stack >= 500)
                            Difficulty++;
                        if (Main.rand.Next(Difficulty) == 0)
                            item.Stack++;
                    }
                }
            }
        }

        public class GuardianShop : IDisposable
        {
            public int OwnerID = 0;
            public string OwnerModID = "";
            public List<GuardianShopItem> Items = new List<GuardianShopItem>();

            public GuardianShopItem AddNewItem(int ItemID, int Price = -1, string Name = "", int FixedSellStack = 1)
            {
                Main.NewText("Tried to add ID zero item to shop.", 200, 128, 0);
                GuardianShopItem item = new GuardianShopItem();
                item.SetItemForSale(ItemID, Price, Name, FixedSellStack);
                if(ItemID > 0)
                    Items.Add(item);
                return item;
            }

            public void Dispose()
            {
                foreach (GuardianShopItem gsi in Items)
                    gsi.Dispose();
                Items.Clear();
                Items = null;
                OwnerModID = null;
            }
        }

        public class GuardianShopItem : IDisposable
        {
            public string ItemName = "";
            public int ItemID = 0;
            public int Price = 0, Stack = -1, SellStack = 1;
            public DisponibilityTime disponibility = DisponibilityTime.Anytime;
            public float SaleFactor = 0, TimedSaleChance = 0;
            public int SaleTime = 0, TimedSaleTime = 0;
            public bool ItemOnDisplay = false;
            public bool UniqueItem = false;
            public delegate bool DisponibilityDel(Player player);
            /// <summary>
            /// Use this to set the extra disponibility function.
            /// </summary>
            public DisponibilityDel GetIfItemIsDisponible = delegate(Player player) { return true; };

            public bool CanItemBeVisibleAtTheStore(Player player)
            {
                return GetIfItemIsDisponible(player) && IsItemDisponible();
            }

            public bool IsItemDisponible()
            {
                if (disponibility != DisponibilityTime.Anytime)
                {
                    switch (disponibility)
                    {
                        case DisponibilityTime.Day:
                            return Main.dayTime;
                        case DisponibilityTime.Bloodmoon:
                            return Main.bloodMoon;
                        case DisponibilityTime.Eclipse:
                            return Main.eclipse;
                        case DisponibilityTime.EarlyNight:
                            return !Main.dayTime && Main.time < 16200;
                        case DisponibilityTime.LateNight:
                            return !Main.dayTime && Main.time >= 16200;
                        case DisponibilityTime.EarlyDay:
                            return Main.dayTime && Main.time < 27000;
                        case DisponibilityTime.Afternoon:
                            return Main.dayTime && Main.time >= 27000;
                        case DisponibilityTime.LunarApocalypse:
                            return NPC.LunarApocalypseIsUp;
                        case DisponibilityTime.Night:
                            return !Main.dayTime;
                        case DisponibilityTime.Timed:
                            return TimedSaleTime > 0;
                    }
                }
                return true;
            }

            public void SetLimitedDisponibility(DisponibilityTime disponibility)
            {
                this.disponibility = disponibility;
            }

            public void SetToBeRandomlySold(float TimedSaleChance)
            {
                disponibility = DisponibilityTime.Timed;
                this.TimedSaleChance = TimedSaleChance;
            }

            public void SetItemForSale(int ItemID, int Price = -1, string Name = "", int SellFixedStack = 1)
            {
                this.ItemID = ItemID;
                Item SoldItem = new Item();
                SoldItem.SetDefaults(ItemID, true);
                UniqueItem = SoldItem.maxStack == 1;
                if (Price == -1)
                {
                    this.Price = SoldItem.value;
                }
                else
                {
                    this.Price = Price;
                }
                if (Name != "")
                    ItemName = Name;
                else
                    ItemName = SoldItem.Name;
                SellStack = SellFixedStack;
            }

            public void Dispose()
            {
                ItemName = null;
            }

            public enum DisponibilityTime : byte
            {
                Anytime,
                Timed,
                Day,
                EarlyDay,
                Afternoon,
                Night,
                EarlyNight,
                LateNight,
                Bloodmoon,
                Eclipse,
                LunarApocalypse,
                Count
            }
        }
    }
}

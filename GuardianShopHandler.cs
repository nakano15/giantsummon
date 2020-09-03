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

        public static GuardianShop CreateShop(int OwnerID, string OwnerModID)
        {
            GuardianShop shop = new GuardianShop();
            shop.OwnerID = OwnerID;
            shop.OwnerModID = OwnerModID;
            Shops.Add(shop);
            return shop;
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
                    else if (PassedAnHour && item.IsItemDisponible && item.SaleTime == 0 && Main.rand.NextDouble() < 0.01f)
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

        public class GuardianShop
        {
            public int OwnerID = 0;
            public string OwnerModID = "";
            public List<GuardianShopItem> Items = new List<GuardianShopItem>();

            public GuardianShopItem AddNewItem(int ItemID, int Price = -1, string Name = "")
            {
                GuardianShopItem item = new GuardianShopItem();
                item.SetItemForSale(ItemID, Price,Name);
                Items.Add(item);
                return item;
            }
        }

        public class GuardianShopItem
        {
            public string ItemName = "";
            public int ItemID = 0, Price = 0, Stack = -1;
            public DisponibilityTime disponibility = DisponibilityTime.Anytime;
            public float SaleFactor = 0, TimedSaleChance = 0;
            public int SaleTime = 0, TimedSaleTime = 0;
            public bool ItemOnDisplay = false;
            public bool UniqueItem = false;

            public void SetLimitedDisponibility(DisponibilityTime disponibility)
            {
                this.disponibility = disponibility;
            }

            public void SetToBeRandomlySold(float TimedSaleChance)
            {
                this.disponibility = DisponibilityTime.Timed;
                this.TimedSaleChance = TimedSaleChance;
            }

            public void SetItemForSale(int ItemID, int Price = -1, string Name = "")
            {
                this.ItemID = ItemID;
                Item i = new Item();
                i.SetDefaults(ItemID);
                UniqueItem = i.maxStack == 1;
                if (Price == -1)
                {
                    this.Price = i.value;
                }
                else
                {
                    this.Price = Price;
                }
                if (Name != "")
                    this.ItemName = Name;
                else
                    this.ItemName = i.Name;
            }

            public bool IsItemDisponible
            {
                get
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
                            case DisponibilityTime.LateNight:
                                return !Main.dayTime && Main.time >= 16200;
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
            }

            public enum DisponibilityTime : byte
            {
                Anytime,
                Timed,
                Day,
                Afternoon,
                Night,
                LateNight,
                Bloodmoon,
                Eclipse,
                LunarApocalypse,
                Count
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class DogGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Domino_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "DogGuardian";
            return mod.Properties.Autoload;
        }

        public DogGuardian()
            : base(9)
        {

        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (!firstButton)
                shop = true;
            base.OnChatButtonClicked(firstButton, ref shop);
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            base.SetChatButtons(ref button, ref button2);
            button2 = "Shop";
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            byte FriendshipLevel = 0;
            if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID))
            {
                FriendshipLevel = PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianID, GuardianModID).FriendshipGrade;
            }
            shop.item[nextSlot++].SetDefaults(ItemID.MusketBall);
            shop.item[nextSlot++].SetDefaults(ItemID.WoodenArrow);
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Cannon.CannonShell>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Cannon.IronCannon>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Consumable.FirstAidKit>());
            if (FriendshipLevel >= 2)
            {
            }
        }
    }
}

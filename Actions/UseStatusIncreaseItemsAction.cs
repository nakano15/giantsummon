using Terraria.ModLoader;

namespace giantsummon.Actions
{
    public class UseStatusIncreaseItemsAction : GuardianActions
    {
        public UseStatusIncreaseItemsAction()
        {
            ID = (int)ActionIDs.UseStatusIncreaseItems;
            InUse = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (guardian.ItemAnimationTime <= 0 && !guardian.IsAttackingSomething)
            {
                bool AllItemsUsed = true;
                for (int i = 0; i < 50; i++)
                {
                    if ((guardian.Inventory[i].type == Terraria.ID.ItemID.LifeCrystal || guardian.Inventory[i].type == ModContent.ItemType<Items.Consumable.EtherHeart>()) && guardian.LifeCrystalHealth < TerraGuardian.MaxLifeCrystals)
                    {
                        guardian.SelectedItem = i;
                        guardian.Action = true;
                        AllItemsUsed = false;
                        break;
                    }
                    if ((guardian.Inventory[i].type == Terraria.ID.ItemID.LifeFruit || guardian.Inventory[i].type == ModContent.ItemType<Items.Consumable.EtherFruit>()) && guardian.LifeCrystalHealth == TerraGuardian.MaxLifeCrystals && guardian.LifeFruitHealth < TerraGuardian.MaxLifeFruit)
                    {
                        guardian.SelectedItem = i;
                        guardian.Action = true;
                        AllItemsUsed = false;
                        break;
                    }
                    if (guardian.Inventory[i].type == Terraria.ID.ItemID.ManaCrystal && guardian.ManaCrystals < GuardianData.MaxManaCrystals)
                    {
                        guardian.SelectedItem = i;
                        guardian.Action = true;
                        AllItemsUsed = false;
                        break;
                    }
                }
                if (AllItemsUsed)
                    InUse = false;
            }
        }
    }
}

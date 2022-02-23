
using Terraria.ModLoader;

namespace giantsummon.Actions
{
    public class UseSkillResetItemAction : GuardianActions
    {
        public UseSkillResetItemAction()
        {
            ID = (int)ActionIDs.UseSkillResetPotion;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (!guardian.LastAction && guardian.ItemAnimationTime <= 0)
            {
                int ItemID = ModContent.ItemType<Items.Consumable.SkillResetPotion>();
                InUse = false;
                for (int i = 0; i < 50; i++)
                {
                    if (guardian.Inventory[i].type == ItemID)
                    {
                        guardian.SelectedItem = i;
                        guardian.Action = true;
                        return;
                    }
                }
            }
        }
    }
}

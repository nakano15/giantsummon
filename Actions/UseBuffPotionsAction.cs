using Terraria;

namespace giantsummon.Actions
{
    public class UseBuffPotionsAction : GuardianActions
    {
        public UseBuffPotionsAction()
        {
            ID = (int)ActionIDs.UseBuffPotions;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (guardian.ItemAnimationTime <= 0 && !guardian.IsAttackingSomething)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (guardian.Inventory[i].buffType > 0 && !Main.vanityPet[guardian.Inventory[i].buffType] && !Main.lightPet[guardian.Inventory[i].buffType] && !Main.projPet[guardian.Inventory[i].buffType] && !guardian.Inventory[i].summon && !guardian.Inventory[i].DD2Summon && !guardian.Inventory[i].sentry && !guardian.HasBuff(guardian.Inventory[i].buffType))
                    {
                        guardian.SelectedItem = i;
                        guardian.Action = true;
                        return;
                    }
                }
                InUse = false;
            }
        }
    }
}

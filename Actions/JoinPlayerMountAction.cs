using Terraria;

namespace giantsummon.Actions
{
    public class JoinPlayerMountAction : GuardianActions
    {
        public Player MountUser;

        public JoinPlayerMountAction(Player Target)
        {
            ID = (int)ActionIDs.JoinPlayerMount;
            InUse = true;
            MountUser = Target;
        }

        public override void Update(TerraGuardian guardian)
        {
            IgnoreCombat = true;
            if (!MountUser.mount.Active)
            {
                InUse = false;
                return;
            }
            if (TryReachingPlayer(guardian, MountUser))//guardian.HitBox.Intersects(Target.getRect()))
            {
                guardian.DoSitOnPlayerMount(true);
                InUse = false;
            }
            else
            {
                if (guardian.furniturex > -1)
                    guardian.LeaveFurniture();
            }
        }
    }
}

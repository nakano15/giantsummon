using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using giantsummon.Actions;

namespace giantsummon
{
    public class GuardianActions
    {
        public bool InUse = false;
        private int _ID = 0;
        public int ID { get { return _ID; } set { _ID = value; } }
        public bool IsGuardianSpecificAction = false;
        public int Time = 0, Step = 0;
        public bool IgnoreCombat = false, Cancellable = false, AvoidItemUsage = false, FocusCameraOnGuardian = false, EffectOnlyMirror = false, Invisibility = false, Immune = false, NoAggro = false, Inactivity = false, CantUseInventory = false, NpcCanFacePlayer = true, ProceedIdleAIDuringDialogue = false;
        public bool StepStart { get { return Time == 0; } }
        public bool StepChanged = false;

        public void ChangeStep(int Number = -1)
        {
            if (Number == -1) Step++;
            else Step = Number;
            Time = 0;
            StepChanged = true;
        }

        public static bool TryRevivingPlayer(TerraGuardian Guardian, Player target)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new ReviveSomeoneAction(target);
            return true;
        }

        public static bool TryRevivingGuardian(TerraGuardian Guardian, TerraGuardian target)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new ReviveSomeoneAction(target);
            return true;
        }

        public static void UseThrowPotion(TerraGuardian Guardian, Player Target)
        {
            int HighestHealingDifference = -1, Potion = -1;
            for (int i = 0; i < 50; i++)
            {
                if (Guardian.Inventory[i].type != 0 && Guardian.Inventory[i].potion && !MainMod.IsGuardianItem(Guardian.Inventory[i]))
                {
                    int PosHealingValue = Math.Abs((Target.statLifeMax2 - Target.statLife) - (int)(Guardian.Inventory[i].healLife * Guardian.HealthHealMult));
                    if (HighestHealingDifference == -1 || PosHealingValue < HighestHealingDifference)
                    {
                        Potion = i;
                        HighestHealingDifference = PosHealingValue;
                    }
                }
            }
            if (Potion < 0)
                return;
            Guardian.DoAction = new ThrowPotionAction(Target, Potion);
        }

        public static bool UseLaunchPlayer(TerraGuardian Guardian, Player Target)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new LaunchPlayerAction(Target);
            return true;
        }

        public static bool UseLiftPlayer(TerraGuardian Guardian, Player Target)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new LiftPlayerAction(Target);
            return true;
        }

        public static bool UseResurrectOnPlayer(TerraGuardian Guardian, Player Target)
        {
            if (!Target.dead || Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new ResurrectPlayerAction(Target);
            return true;
        }

        public static bool UseBuffPotions(TerraGuardian Guardian)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new UseBuffPotionsAction();
            return true;
        }

        public static bool UseStatusIncreaseItems(TerraGuardian Guardian)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new UseStatusIncreaseItemsAction();
            return true;
        }

        public static void OpenBirthdayPresent(TerraGuardian Guardian, int BoxPosition)
        {
            Guardian.DoAction = new OpenGiftBoxAction(BoxPosition);
        }

        public static void SellLootCommand(TerraGuardian Guardian)
        {
            Guardian.DoAction = new SellItemsAction();
        }

        public static void GuardianJoinPlayerMountCommand(TerraGuardian Guardian, Player TargetPlayer)
        {
            Guardian.DoAction = new JoinPlayerMountAction(TargetPlayer);
        }

        public static bool GuardianPutPlayerOnShoulderCommand(TerraGuardian Guardian)
        {
            if (Guardian.OwnerPos == -1 || Guardian.DoAction.InUse)
                return false;
            Guardian.DoAction = new PickupPlayerMountAction(Main.player[Guardian.OwnerPos]);
            return true;
        }

        public static void GuardianPutPlayerOnTheFloorCommand(TerraGuardian Guardian)
        {
            if (Guardian.OwnerPos == -1 || Guardian.DoAction.InUse)
                return;
            Guardian.DoAction = new MountPutPlayerDownAction(Main.player[Guardian.OwnerPos]);
        }

        public static bool TeleportWithPlayerCommand(TerraGuardian Guardian, Player Target)
        {
            if (Guardian.OwnerPos == -1 || Guardian.DoAction.InUse || !Guardian.HasMagicMirror) return false;
            Guardian.DoAction = new TeleportWithPlayerToTownAction(Target);
            return true;
        }

        public static void UseSkillResetPotionCommand(TerraGuardian Guardian)
        {
            if (Guardian.OwnerPos == -1) return;
            Guardian.DoAction = new UseSkillResetItemAction();
        }

        public static void RestCommand(TerraGuardian Guardian, byte RestTime)
        {
            if (Guardian.OwnerPos == -1) return;
            Guardian.DoAction = new RestAction(RestTime);
        }

        public static void BuyItemFromShopCommand(TerraGuardian Guardian, int NpcPosition, int ItemID, int BuyStack, int BuyPrice)
        {
            //if (Guardian.OwnerPos == -1) return;
            Guardian.DoAction = new BuySomethingFromNpcShopAction(NpcPosition, ItemID, BuyStack, BuyPrice);
        }

        public void UpdateAction(TerraGuardian guardian)
        {
            Update(guardian);
            if (StepChanged)
                StepChanged = false;
            else
                Time++;
        }
        
        public virtual void Update(TerraGuardian guardian)
        {

        }

        public virtual void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {

        }

        public void DrawAction(TerraGuardian guardian)
        {
            if (!InUse)
                return;
            Draw(guardian);
        }

        public virtual void Draw(TerraGuardian guardian)
        {

        }

        public Vector2 GetHandPosition(TerraGuardian guardian, int AnimationFrame, HeldHand hand)
        {
            Vector2 HandPosition = guardian.Position;
            if ((hand == HeldHand.Right || hand == HeldHand.Both) && guardian.Base.DontUseRightHand)
                hand = HeldHand.Left;
            switch (hand)
            {
                case HeldHand.Left:
                    HandPosition += guardian.GetLeftHandPosition(AnimationFrame);
                    break;
                case HeldHand.Right:
                    HandPosition += guardian.GetRightHandPosition(AnimationFrame);
                    break;
                case HeldHand.Both:
                    HandPosition += guardian.GetBetweenHandsPosition(AnimationFrame);
                    break;
            }
            return HandPosition;
        }

        public void SetPlayerOnHandPosition(TerraGuardian guardian, Player player, int AnimationFrame, HeldHand hand)
        {
            Vector2 HandPosition = GetHandPosition(guardian, AnimationFrame, hand);
            player.position.X = HandPosition.X - player.width * 0.5f;
            player.position.Y = HandPosition.Y - player.height * 0.5f;
            player.velocity.X = 0;
            player.velocity.Y = 0;
            player.fallStart = (int)player.position.Y / 16;
        }

        public void SetPlayerPositionOnGuardianCenter(TerraGuardian guardian, Player player)
        {
            player.position.X = guardian.Position.X - player.width * 0.5f;
            player.position.Y = guardian.Position.Y - player.height - 1;
            player.fallStart = (int)player.position.Y / 16;
        }

        public bool TryReachingPlayer(TerraGuardian guardian, Player player)
        {
            if (guardian.OwnerPos == player.whoAmI && guardian.PlayerMounted)
                return true;
            if (guardian.HitBox.Intersects(player.getRect()) && !guardian.BeingPulledByPlayer && guardian.ItemAnimationTime == 0)
            {
                return true;
            }
            else
            {
                guardian.MoveLeft = guardian.MoveRight = false;
                if (player.Center.X < guardian.Position.X)
                    guardian.MoveLeft = true;
                else
                    guardian.MoveRight = true;
            }
            return false;
        }

        public enum ActionIDs : byte
        {
            ThrowPotion,
            LaunchPlayer,
            LiftPlayer,
            ResurrectPlayer,
            UseBuffPotions,
            UseStatusIncreaseItems,
            OpenGiftBox,
            SellItems,
            JoinPlayerMount,
            PickupPlayerMount,
            MountPutPlayerDown,
            TeleportWithPlayerToTown,
            UseSkillResetPotion,
            ReviveSomeone,
            GoSleep,
            BuySomethingFromNpcShop
        }
    }
}

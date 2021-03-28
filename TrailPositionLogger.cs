using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon
{
    public class TrailPositionLogger
    {
        public Vector2 Position = new Vector2();
        public int LeftArmFrame = 0, RightArmFrame = 0, BodyFrame = 0;
        public bool FacingLeft = false;
        public int ItemPositionX = 0, ItemPositionY = 0;
        public float ItemRotation = 0f, ItemScale = 0f;
        public int SelectedItem = 0;

        private static Vector2 BackupPosition = Vector2.Zero;
        private static int BackupLeftArmFrame = 0, BackupRightArmFrame = 0, BackupBodyFrame = 0;
        private static bool BackupFacingLeft = false;
        private static int BackupItemPositionX = 0, BackupItemPositionY = 0;
        private static float BackupItemRotation = 0f, BackupItemScale = 0f;
        private static int BackupSelectedItem = 0;

        public void MaskGuardianInfosToTrail(TerraGuardian tg)
        {
            BackupPosition = tg.Position;
            BackupLeftArmFrame = tg.LeftArmAnimationFrame;
            BackupRightArmFrame = tg.RightArmAnimationFrame;
            BackupBodyFrame = tg.BodyAnimationFrame;
            BackupFacingLeft = tg.LookingLeft;
            //
            BackupItemPositionX = tg.ItemPositionX;
            BackupItemPositionY = tg.ItemPositionY;
            BackupItemRotation = tg.ItemRotation;
            BackupItemScale = tg.ItemScale;
            BackupSelectedItem = tg.SelectedItem;
            ///
            tg.Position = Position;
            tg.LeftArmAnimationFrame = LeftArmFrame;
            tg.RightArmAnimationFrame = RightArmFrame;
            tg.BodyAnimationFrame = BodyFrame;
            tg.LookingLeft = FacingLeft;
            //
            tg.ItemPositionX = ItemPositionX;
            tg.ItemPositionY = ItemPositionY;
            tg.ItemRotation = ItemRotation;
            tg.ItemScale = ItemScale;
            tg.SelectedItem = SelectedItem;
        }

        public void RestoreGuardianInfos(TerraGuardian tg)
        {
            tg.Position = BackupPosition;
            tg.LeftArmAnimationFrame = BackupLeftArmFrame;
            tg.RightArmAnimationFrame = BackupRightArmFrame;
            tg.BodyAnimationFrame = BackupBodyFrame;
            tg.LookingLeft = BackupFacingLeft;
            //
            tg.ItemPositionX = BackupItemPositionX;
            tg.ItemPositionY = BackupItemPositionY;
            tg.ItemRotation = BackupItemRotation;
            tg.ItemScale = BackupItemScale;
            tg.SelectedItem = BackupSelectedItem;
        }
    }
}

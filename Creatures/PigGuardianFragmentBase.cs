using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Creatures
{
    public class PigGuardianFragmentBase : GuardianBase
    {
        public int CloudFormStanding = 19,
            CloudFormMoveForward = 20,
            CloudFormMoveBackward = 21,
            CloudFormChairSit = 22,
            CloudFormThroneSit = 23,
            CloudFormKO = 24,
            CloudFormRevive = 25,
            CloudFormBedSleep = 26;
        public byte PigID = 0;
        public const byte AngerPigGuardianID = 0, SadnessPigGuardianID = 1, HappinessPigGuardianID = 2, FearPigGuardianID = 3, BlandPigGuardianID = 4;

        public PigGuardianFragmentBase(byte PigID)
        {
            this.PigID = PigID;
        }
        
        public bool GetIfIsCloudForm(TerraGuardian guardian)
        {
            return true; //Remove if you don't want to test It.
            if (guardian.OwnerPos > -1)
            {
                return Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().PigGuardianCloudForm[PigID];
            }
            else
            {
                return Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().PigGuardianCloudForm[PigID];
            }
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte AnimationID, ref int Frame)
        {
            bool CloudForm = GetIfIsCloudForm(guardian);
            if (CloudForm)
            {
                if ((AnimationID == 1 && TerraGuardian.UsingLeftArmAnimation) || (AnimationID == 2 && TerraGuardian.UsingRightArmAnimation))
                {
                    return;
                }
                if (guardian.SittingOnPlayerMount)
                {
                    Frame = CloudFormChairSit;
                }
                else if (Frame == DownedFrame)
                {
                    Frame = CloudFormKO;
                }
                else if (Frame == ReviveFrame)
                {
                    Frame = CloudFormRevive;
                }
                else if (guardian.UsingFurniture)
                {
                    if (Frame == ChairSittingFrame || Frame == SittingFrame)
                    {
                        Frame = CloudFormChairSit;
                    }
                    else if (Frame == ThroneSittingFrame)
                    {
                        Frame = CloudFormThroneSit;
                    }
                    else if (Frame == BedSleepingFrame)
                    {
                        Frame = CloudFormBedSleep;
                    }
                }
                else if (guardian.Velocity.X != 0 && guardian.Velocity.Y == 0)
                {
                    if ((guardian.LookingLeft && guardian.Velocity.X < 0) || (!guardian.LookingLeft && guardian.Velocity.X > 0))
                    {
                        Frame = 20;
                    }
                    else
                    {
                        Frame = 21;
                    }
                }
                else
                {
                    Frame = 19;
                }
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Microsoft.Xna.Framework.Vector2 DrawPosition, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Color armorColor, float Rotation, Microsoft.Xna.Framework.Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            const float Opacity = 0.8f;
            if (GetIfIsCloudForm(guardian))
            {
                foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
                {
                    if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                        gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                    {
                        gdd.color *= Opacity;
                    }
                }
                foreach (GuardianDrawData gdd in TerraGuardian.DrawFront)
                {
                    if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                        gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                    {
                        gdd.color *= Opacity;
                    }
                }
            }
        }

        public override void GuardianBehaviorModScript(TerraGuardian guardian)
        {
            if (GetIfIsCloudForm(guardian))
            {
                /*if (guardian.Jump)
                {
                    if (guardian.Velocity.Y < -3f)
                    {
                        guardian.Velocity.Y = -3f;
                    }
                    else
                    {
                        guardian.Velocity.Y += -0.05f;
                    }
                }
                else*/
                {
                    if (guardian.Velocity.Y >= 2.3f)
                    {
                        guardian.Velocity.Y = 2.3f;
                        guardian.SetFallStart();
                    }
                    else
                    {
                        //guardian.Velocity.Y += 0.05f;
                    }
                }
                if(!guardian.KnockedOut && !guardian.MountedOnPlayer && !guardian.UsingFurniture && guardian.Velocity.Y == 0 && guardian.BodyAnimationFrame != CloudFormRevive)
                    guardian.OffsetY -= ((float)Math.Sin(Main.GlobalTime * 2)) * 5;
            }
        }
    }
}

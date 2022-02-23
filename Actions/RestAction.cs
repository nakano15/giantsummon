using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Actions
{
    public class RestAction : GuardianActions
    {
        public byte RestTime;

        public RestAction(byte Time)
        {
            ID = (int)ActionIDs.GoSleep;
            RestTime = Time;
        }

        public override void Update(TerraGuardian guardian)
        {
            IgnoreCombat = true;
            Cancellable = false;
            if (guardian.Downed || guardian.KnockedOut)
            {
                MainMod.ScreenColorAlpha = 0;
                InUse = false;
                return;
            }
            bool SetPlayerToGuardianBed = false;
            const bool UseBedSharing = false;
            switch (Step)
            {
                case 0:
                    {
                        if (UseBedSharing && guardian.Base.Size >= GuardianBase.GuardianSize.Large)
                        {
                            if (guardian.AttemptToGrabPlayer())
                            {
                                ChangeStep();
                            }
                            guardian.WalkMode = true;
                        }
                        else
                        {
                            ChangeStep();
                        }
                    }
                    break;

                case 1:
                    {
                        if (Time == 0)
                        {
                            if (guardian.furniturex > -1)
                            {
                                guardian.LeaveFurniture(false);
                            }
                            guardian.TryFindingNearbyBed(false);
                        }
                        if (guardian.furniturex == -1 || guardian.UsingFurniture)
                            ChangeStep();
                        if (guardian.furniturex > -1)
                            guardian.WalkMode = true;
                    }
                    break;

                case 2:
                    {
                        MainMod.ScreenColor.R = MainMod.ScreenColor.G = MainMod.ScreenColor.B = 0;
                        MainMod.ScreenColorAlpha = (float)Time / (5 * 60);
                        if (MainMod.ScreenColorAlpha > 1)
                            MainMod.ScreenColorAlpha = 1f;
                        if (UseBedSharing && guardian.IsSleeping && guardian.GrabbingPlayer)
                        {
                            SetPlayerToGuardianBed = true;
                        }
                        if (Time >= 5 * 60)
                        {
                            //Do time change
                            if (guardian.OwnerPos > -1)
                            {
                                Main.player[guardian.OwnerPos].position.X = guardian.Position.X - Main.player[guardian.OwnerPos].width * 0.5f;
                                Main.player[guardian.OwnerPos].position.Y = guardian.Position.Y - Main.player[guardian.OwnerPos].height;
                                Main.player[guardian.OwnerPos].velocity = Vector2.Zero;
                                int RestValue = 4;
                                switch (RestTime)
                                {
                                    case 0:
                                        RestValue = 4;
                                        break;
                                    case 1:
                                        RestValue = 8;
                                        break;
                                    case 2: //UntilDawn
                                        RestValue = 0;
                                        if (Main.dayTime)
                                        {
                                            RestValue += (int)(24 * 3600 - Main.time) / 3600 + 1;
                                        }
                                        else
                                        {
                                            RestValue += (int)(9 * 3600 - Main.time) / 3600 + 1;
                                        }
                                        break;
                                    case 3: //UntilNight
                                        RestValue = 0;
                                        if (Main.dayTime)
                                        {
                                            RestValue += (int)(15 * 3600 - Main.time) / 3600 + 1;
                                        }
                                        else
                                        {
                                            RestValue += (int)(24 * 3600 - Main.time) / 3600 + 1;
                                        }
                                        break;
                                }
                                WorldMod.SkipTime(RestValue);
                                foreach (TerraGuardian g in Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                                {
                                    float RestBonus = 1.5f;
                                    if (g.Base.IsNocturnal)
                                    {
                                        if (!Main.dayTime)
                                            RestBonus = 2f;
                                    }
                                    else
                                    {
                                        if (Main.dayTime)
                                            RestBonus = 2f;
                                    }
                                    RestBonus *= RestValue;
                                    if ((int)g.Data.Fatigue - RestBonus < 0)
                                        g.Data.Fatigue = 0;
                                    else
                                        g.Data.Fatigue -= (sbyte)RestBonus;
                                    float InjuryValue = RestBonus * 0.5f;
                                    if ((int)g.Data.Injury - RestBonus < 0)
                                        g.Data.Injury = 0;
                                    else
                                        g.Data.Injury -= (sbyte)InjuryValue;
                                    if (g.request.Active)
                                        g.request.RequestTimeLeft -= RestValue;
                                }
                            }
                            guardian.GrabbingPlayer = false;
                            ChangeStep();
                        }
                    }
                    break;

                case 3:
                    {
                        Player player = Main.player[guardian.OwnerPos];
                        if (Time >= 5 * 60)
                        {
                            InUse = false;
                            MainMod.ScreenColorAlpha = 0;
                            if (guardian.IsSleeping)
                            {
                                player.fullRotation = 0;
                                guardian.LeaveFurniture();
                            }
                        }
                        else
                        {
                            MainMod.ScreenColor.R = MainMod.ScreenColor.G = MainMod.ScreenColor.B = 0;
                            MainMod.ScreenColorAlpha = 1f - (float)Time / (5 * 60);
                            if (guardian.IsSleeping)
                            {
                                SetPlayerToGuardianBed = true;
                            }
                        }
                    }
                    break;
            }
            if (UseBedSharing && SetPlayerToGuardianBed)
            {
                Player player = Main.player[guardian.OwnerPos];
                player.direction = guardian.Direction;
                player.fullRotation = -1.570796326794897f * player.direction;
                player.position.X = guardian.Position.X - player.width * 0.5f - guardian.Base.SleepingOffset.X;
                player.position.Y = guardian.Position.Y - player.height - guardian.Base.SleepingOffset.Y;
                player.velocity = Vector2.Zero;
                player.fullRotationOrigin.X = player.width * 0.5f;
                player.fullRotationOrigin.Y = player.height * 0.5f;
                guardian.GrabbingPlayer = false;
            }
        }
    }
}

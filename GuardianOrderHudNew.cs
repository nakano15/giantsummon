using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class GuardianOrderHudNew
    {
        public static bool Active = false;

        public static List<OptionHolder> OptionMenus = new List<OptionHolder>();
        public static byte SelectedGuardian = 255;

        public static void Open()
        {
            if (!Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian.Active)
                return;
            SelectedGuardian = 255;
            OptionMenus.Clear();
            Active = true;
            AddOptionHolder();
            AddOption(Option.OptionType.PullGuardians);
            AddOption(Option.OptionType.Order);
            AddOption(Option.OptionType.Interaction);
            AddOption(Option.OptionType.Action);
            AddOption(Option.OptionType.Item);
        }

        public static void AddOptionHolder()
        {
            OptionHolder oh = new OptionHolder();
            OptionMenus.Add(oh);
        }

        public static Option AddOption(Option.OptionType otype, bool Enabled = true, int InternalValue = 0)
        {
            Option o = new Option();
            o.oType = otype;
            o.Enabled = Enabled;
            o.InternalValue = InternalValue;
            OptionMenus[OptionMenus.Count - 1].Options.Add(o);
            return o;
        }

        public static void Close()
        {
            Active = false;
            OptionMenus.Clear();
            SelectedGuardian = 255;
        }

        public static void Draw()
        {
            try
            {
                if (!Active)
                {
                    return;
                }
                PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                Vector2 OrderPositionStart = new Vector2(0, Main.screenHeight - 86);
                foreach (OptionHolder oh in OptionMenus)
                {
                    Vector2 OptionsPosition = OrderPositionStart;
                    float WidthStack = 0;
                    OptionsPosition.Y -= 24 * oh.Options.Count;
                    for (int i = 0; i < oh.Options.Count; i++)
                    {
                        Color c = Color.White;
                        if (!oh.Options[i].Enabled)
                            c = Color.Gray;
                        if (i == oh.Selected)
                        {
                            c = Color.Yellow;
                        }
                        string Text = (i + 1) + ". " + TranslateOption(oh.Options[i], player);
                        float Width = Utils.DrawBorderString(Main.spriteBatch, Text, OptionsPosition, c).X;
                        if (Width > WidthStack)
                        {
                            WidthStack = Width;
                        }
                        OptionsPosition.Y += 24;
                    }
                    OrderPositionStart.X += WidthStack + 4;
                }
            }
            catch
            {
                Close();
            }
        }

        public static void Update(PlayerMod player)
        {
            if (!Active) return;
            List<Option> PickedOptions = new List<Option>();
            foreach (OptionHolder oh in OptionMenus)
            {
                if (oh.Selected > -1)
                {
                    //WidthStack = 64;
                    PickedOptions.Add(oh.Options[oh.Selected]);
                }
            }
            bool Cancel = MainMod.orderCallButton.JustPressed;
            if (Cancel)
            {
                if (OptionMenus.Count == 1)
                {
                    Close();
                }
                else
                {
                    OptionMenus.RemoveAt(OptionMenus.Count - 1);
                }
            }
            else
            {
                OptionHolder oh = OptionMenus[OptionMenus.Count - 1];
                for (int o = 0; o < oh.Options.Count; o++)
                {
                    bool ButtonPress = MainMod.DButtonPress == o;
                    if (ButtonPress && oh.Options[o].Enabled)
                    {
                        oh.Selected = o;
                        //PickedOptions.Add(oh.Options[o]);
                        UponPickingOption(oh.Options[o], player, PickedOptions);
                    }
                }
                MainMod.DButtonPress = 255;
            }
        }

        public static void UponPickingOption(Option option, PlayerMod player, List<Option> LastPickedOptions)
        {
            TerraGuardian[] Guardians = player.GetAllGuardianFollowers;
            switch (option.oType)
            {
                case Option.OptionType.Order:
                case Option.OptionType.Action:
                case Option.OptionType.Item:
                case Option.OptionType.Interaction:
                case Option.OptionType.PullGuardians:
                    {
                        AddOptionHolder();
                        AddOption(Option.OptionType.PickGuardian, true, 255);
                        for (int i = 0; i < Guardians.Length; i++)
                        {
                            if (!Guardians[i].Active)
                                continue;
                            AddOption(Option.OptionType.PickGuardian, true, i);
                        }
                    }
                    break;
                case Option.OptionType.PickGuardian:
                    {
                        SelectedGuardian = (byte)option.InternalValue;
                        switch (LastPickedOptions[0].oType)
                        {
                            case Option.OptionType.Order:
                                {
                                    AddOptionHolder();
                                    AddOption(Option.OptionType.Follow);
                                    AddOption(Option.OptionType.GuardHere);
                                    AddOption(Option.OptionType.GoAheadBehind);
                                    AddOption(Option.OptionType.AvoidCombat);
                                    AddOption(Option.OptionType.FreeControl, 
                                        (SelectedGuardian == 255 && Guardians.Any(x => x.Active && x.PlayerMounted && x.HasFlag(GuardianFlags.StopMindingAfk))) ||
                                        (SelectedGuardian != 255 && Guardians[SelectedGuardian].PlayerMounted && Guardians[SelectedGuardian].HasFlag(GuardianFlags.StopMindingAfk)));
                                }
                                break;
                            case Option.OptionType.Action:
                                {
                                    AddOptionHolder();
                                    AddOption(Option.OptionType.GoSellLoot, ((SelectedGuardian == 255 && Guardians.Any(x => x.Active && x.HasFlag(GuardianFlags.MayGoSellLoot))) ||
                                        (SelectedGuardian < 255 && Guardians[SelectedGuardian].HasFlag(GuardianFlags.MayGoSellLoot))));
                                    AddOption(Option.OptionType.UseFurniture);
                                    AddOption(Option.OptionType.LiftMe);
                                    AddOption(Option.OptionType.LaunchMe, player.player.wings > 0);
                                }
                                break;
                            case Option.OptionType.Item:
                                {
                                    AddOptionHolder();
                                    AddOption(Option.OptionType.HealSelf);
                                    AddOption(Option.OptionType.UseBuffPotions);
                                    AddOption(Option.OptionType.UseSkillResetPotion);
                                    AddOption(Option.OptionType.UseStatusIncreaseItem);
                                }
                                break;
                            case Option.OptionType.Interaction:
                                {
                                    AddOptionHolder();
                                    AddOption(Option.OptionType.MountGuardian, (SelectedGuardian == 255 && Guardians.Any(x => x.Active && (x.HasFlag(GuardianFlags.AllowMount) || x.PlayerMounted))) ||
                                        (SelectedGuardian != 255 && (Guardians[SelectedGuardian].HasFlag(GuardianFlags.AllowMount) || Guardians[SelectedGuardian].PlayerMounted)));
                                    AddOption(Option.OptionType.ShareMount, player.player.mount.Active && (SelectedGuardian == 255 || !player.GetAllGuardianFollowers[SelectedGuardian].PlayerControl));
                                    AddOption(Option.OptionType.TeleportWithPlayer,
                                        ((SelectedGuardian == 255 && Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetAllGuardianFollowers.Any(x => x.Active && x.HasMagicMirror)) ||
                                        (SelectedGuardian != 255 && Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetAllGuardianFollowers[SelectedGuardian].HasMagicMirror)));
                                    AddOption(Option.OptionType.PlayerControl,
                                        (SelectedGuardian == 255 && Guardians.Any(x => x.Active && x.HasFlag(GuardianFlags.PlayerControl))) ||
                                        (SelectedGuardian < 255 && Guardians[SelectedGuardian].Active && Guardians[SelectedGuardian].HasFlag(GuardianFlags.PlayerControl)));
                                    AddOption(Option.OptionType.SetLeader, SelectedGuardian != 0 && SelectedGuardian != 255);
                                }
                                break;
                            case Option.OptionType.PullGuardians:
                                {
                                    TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                                    for (int g = 0; g < guardians.Length; g++)
                                    {
                                        if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active)
                                        {
                                            guardians[g].BePulledByPlayer();
                                        }
                                    }
                                    Close();
                                }
                                break;
                        }
                    }
                    break;

                case Option.OptionType.Follow:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active)
                            {
                                guardians[g].GuardingPosition = null;
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.GuardHere:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        Point GuardPosition = new Point((int)player.player.Center.X / 16, (int)(player.player.position.Y + player.player.height - 1) / 16);
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active && !guardians[g].PlayerMounted)
                            {
                                guardians[g].GuardingPosition = GuardPosition;
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.GoAheadBehind:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active)
                            {
                                guardians[g].ChargeAhead = !guardians[g].ChargeAhead;
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.AvoidCombat:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active)
                            {
                                guardians[g].AvoidCombat = !guardians[g].AvoidCombat;
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.FreeControl:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        byte HighestPriority = 0;
                        TerraGuardian MountedPriority = null;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active && guardians[g].PlayerMounted && guardians[g].HasFlag(GuardianFlags.StopMindingAfk))
                            {
                                byte Priority = 0;
                                if (guardians[g].Base.ReverseMount)
                                {
                                    Priority = 1;
                                }
                                else
                                {
                                    Priority = 2;
                                }
                                if (Priority > HighestPriority)
                                {
                                    MountedPriority = guardians[g];
                                    HighestPriority = Priority;
                                }
                            }
                        }
                        if (MountedPriority != null)
                        {
                            MountedPriority.ToggleGuardianFullControl(!MountedPriority.GuardianHasControlWhenMounted);
                        }
                        Close();
                    }
                    break;

                case Option.OptionType.GoSellLoot:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active && guardians[g].HasFlag(GuardianFlags.MayGoSellLoot) && !guardians[g].PlayerControl)
                            {
                                GuardianActions.SellLootCommand(guardians[g]);
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.TeleportWithPlayer:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active && !guardians[g].PlayerControl)
                            {
                                if (GuardianActions.TeleportWithPlayerCommand(guardians[g]))
                                    break;
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.UseFurniture:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active)
                            {
                                if (guardians[g].furniturex != -1)
                                {
                                    guardians[g].LeaveFurniture();
                                }
                                else
                                {
                                    guardians[g].UseNearbyFurniture();
                                }
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.LiftMe:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active && !guardians[g].PlayerControl)
                            {
                                if (GuardianActions.UseLiftPlayer(guardians[g], Main.player[guardians[g].OwnerPos]))
                                    break;
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.LaunchMe:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active && !guardians[g].PlayerControl)
                            {
                                GuardianActions.UseLaunchPlayer(guardians[g], Main.player[guardians[g].OwnerPos]);
                            }
                        }
                        Close();
                    }
                    break;

                case Option.OptionType.HealSelf:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active && !guardians[g].PlayerControl)
                            {
                                guardians[g].AttemptToUseHealingPotion();
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.UseBuffPotions:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active && !guardians[g].PlayerControl)
                            {
                                GuardianActions.UseBuffPotions(guardians[g]);
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.UseSkillResetPotion:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active)
                            {
                                GuardianActions.UseSkillResetPotionCommand(guardians[g]);
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.UseStatusIncreaseItem:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active && !guardians[g].PlayerControl)
                            {
                                GuardianActions.UseStatusIncreaseItems(guardians[g]);
                            }
                        }
                        Close();
                    }
                    break;

                case Option.OptionType.MountGuardian:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        bool Dismount = player.MountedOnGuardian || player.GuardianMountingOnPlayer;
                        bool SomeoneMounted = player.GuardianMountingOnPlayer, MountedOnGuardian = player.MountedOnGuardian;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active && guardians[g].HasFlag(GuardianFlags.AllowMount) && !guardians[g].PlayerControl)
                            {
                                if (SelectedGuardian == 255)
                                {
                                    if (guardians[g].PlayerMounted)
                                    {
                                        if (Dismount)
                                        {
                                            GuardianActions.GuardianPutPlayerOnTheFloorCommand(guardians[g]);
                                        }
                                    }
                                    else
                                    {
                                        if (!Dismount)
                                        {
                                            if (GuardianActions.GuardianPutPlayerOnShoulderCommand(guardians[g]))
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (guardians[g].PlayerMounted)
                                    {
                                        GuardianActions.GuardianPutPlayerOnTheFloorCommand(guardians[g]);
                                    }
                                    else
                                    {
                                        if (!guardians[g].Base.ReverseMount && !MountedOnGuardian)
                                        {
                                            if (GuardianActions.GuardianPutPlayerOnShoulderCommand(guardians[g]))
                                                break;
                                        }
                                        if (guardians[g].Base.ReverseMount && !SomeoneMounted)
                                        {
                                            if (GuardianActions.GuardianPutPlayerOnShoulderCommand(guardians[g]))
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.ShareMount:
                    {
                        TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                        for (int g = 0; g < guardians.Length; g++)
                        {
                            if ((SelectedGuardian == 255 || g == SelectedGuardian) && guardians[g].Active && !guardians[g].PlayerControl)
                            {
                                if (guardians[g].SittingOnPlayerMount)
                                    guardians[g].DoSitOnPlayerMount(false);
                                else
                                    GuardianActions.GuardianJoinPlayerMountCommand(guardians[g], Main.player[guardians[g].OwnerPos]);
                            }
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.PlayerControl:
                    {
                        if (player.Guardian.Active && player.Guardian.HasFlag(GuardianFlags.PlayerControl))
                        {
                            player.Guardian.TogglePlayerControl();
                        }
                        Close();
                    }
                    break;
                case Option.OptionType.SetLeader:
                    {
                        player.ChangeLeaderGuardian(SelectedGuardian);
                        Close();
                    }
                    break;
            }
        }

        public static string TranslateOption(Option option, PlayerMod player)
        {
            switch (option.oType)
            {
                default:
                    return option.oType.ToString();

                case Option.OptionType.PullGuardians:
                    return "Pull to Me";

                case Option.OptionType.PickGuardian:
                    if (option.InternalValue == 255)
                        return "Everyone";
                    return player.GetAllGuardianFollowers[option.InternalValue].Name;

                case Option.OptionType.Follow:
                    return "Follow Me";
                case Option.OptionType.GuardHere:
                    return "Wait Here";
                case Option.OptionType.GoAheadBehind:
                    if (SelectedGuardian != 255)
                    {
                        if (player.GetAllGuardianFollowers[SelectedGuardian].ChargeAhead)
                        {
                            return "Follow Behind";
                        }
                        else
                        {
                            return "Follow Ahead";
                        }
                    }
                    return "Swap Row";
                case Option.OptionType.AvoidCombat:
                    if (SelectedGuardian != 255)
                    {
                        if (player.GetAllGuardianFollowers[SelectedGuardian].AvoidCombat)
                        {
                            return "Allow Combat";
                        }
                        else
                        {
                            return "Avoid Combat";
                        }
                    }
                    return "Avoid Combat/Allow combat";
                case Option.OptionType.FreeControl:
                    if (SelectedGuardian != 255)
                    {
                        if (player.GetAllGuardianFollowers[SelectedGuardian].GuardianHasControlWhenMounted)
                        {
                            return "Let me Control";
                        }
                        else
                        {
                            return "Give Control";
                        }
                    }
                    return "Give Control/Take Control";

                case Option.OptionType.GoSellLoot:
                    return "Sell your Loot";
                case Option.OptionType.TeleportWithPlayer:
                    return "Teleport Us Home";
                case Option.OptionType.UseFurniture:
                    if (SelectedGuardian != 255)
                    {
                        if (player.GetAllGuardianFollowers[SelectedGuardian].furniturex > -1)
                            return "Leave Furniture";
                        return "Use Nearby Furniture";
                    }
                    return "Use/Leave Furniture";
                case Option.OptionType.LiftMe:
                    return "Lift me Up";
                case Option.OptionType.LaunchMe:
                    return "Launch me";

                case Option.OptionType.HealSelf:
                    return "Use Healing Potion";
                case Option.OptionType.UseBuffPotions:
                    return "Buff Yourself";
                case Option.OptionType.UseSkillResetPotion:
                    return "Use Skill Reset Potion";
                case Option.OptionType.UseStatusIncreaseItem:
                    return "Use HP/MP Increase Items";

                case Option.OptionType.MountGuardian:
                    if (SelectedGuardian != 255)
                    {
                        if (player.GetAllGuardianFollowers[SelectedGuardian].PlayerMounted)
                        {
                            return "Dismount";
                        }
                        else
                        {
                            return "Mount";
                        }
                    }
                    return "Mount/Dismount";
                case Option.OptionType.ShareMount:
                    if (SelectedGuardian != 255)
                    {
                        if (player.GetAllGuardianFollowers[SelectedGuardian].SittingOnPlayerMount)
                        {
                            return "Leave my Mount";
                        }
                        else
                        {
                            return "Ride in my Mount";
                        }
                    }
                    return "Sit/Leave my Mount";
                case Option.OptionType.PlayerControl:
                    if (player.GetAllGuardianFollowers[0].PlayerControl)
                    {
                        return "Release Control";
                    }
                    else
                    {
                        return "Control Companion";
                    }
                case Option.OptionType.SetLeader:
                    return "Set as Leader";
            }
        }


        public class OptionHolder
        {
            public List<Option> Options = new List<Option>();
            public int Selected = -1;
        }

        public class Option
        {
            public OptionType oType = OptionType.PickGuardian;
            public int InternalValue = 0;
            public bool Enabled = true;

            public enum OptionType
            {
                PickGuardian,

                Order,
                Action,
                Item,
                Interaction,
                PullGuardians,

                //Order
                Follow,
                GuardHere,
                GoAheadBehind,
                AvoidCombat,
                FreeControl,

                //Action
                GoSellLoot,
                TeleportWithPlayer,
                UseFurniture,
                LiftMe,
                LaunchMe,

                //Item
                HealSelf,
                UseBuffPotions,
                UseSkillResetPotion,
                UseStatusIncreaseItem,

                //Interaction
                MountGuardian,
                ShareMount,
                PlayerControl,
                SetLeader
            }
        }
    }
}

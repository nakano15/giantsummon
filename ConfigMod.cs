using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;

namespace giantsummon
{
    //Client side Config
    [Label("Client Settings")]
    public class ConfigMod : ModConfig
    {
        public override ConfigScope Mode
        {
            get { return ConfigScope.ClientSide; }
        }

        [Label("Second player control port.")]
        [Tooltip("Change in case the second player is unable to control the guardian.")]
        public PlayerIndex Control2P { get { return MainMod.controlPort; } set { MainMod.controlPort = value; } }

        [Label("Allow guardians to idle easier when near town npcs?")]
        [Tooltip("Guardians will take less longer to start their Idle AI when you are stopped in a place with town npcs nearby.")]
        [DefaultValue(true)]
        public bool GuardiansIdleEasierOnTowns { get { return MainMod.GuardiansIdleEasierOnTowns; } set { MainMod.GuardiansIdleEasierOnTowns = value; } }

        [Label("Use Guardian Necessities System.")]
        [Tooltip("To allow rotativity, you can turn this on. Guardians can get injured, tired or more as they travel with you. When they get a bad status, you need to send them home to recover from that bad status.")]
        [DefaultValue(true)]
        public bool NecessitiesSystem { get { return MainMod.UsingGuardianNecessitiesSystem; } set { MainMod.UsingGuardianNecessitiesSystem = value; } }

        [Label("Warn about sellable inventory slots filled.")]
        [Tooltip("Turning this on, will only tell the number of inventory slots left, based on the second row and ahead of the guardian inventory.")]
        public bool WarnSaleableInventorySlotsInstad { get { return MainMod.WarnAboutSaleableInventorySlotsLeft; } set { MainMod.WarnAboutSaleableInventorySlotsLeft = value; } }
         
        [Label("Use new order system.")]
        [Tooltip("Turns on the new order system. You just have to press the order button to call It, and navigate by pressing the dpad number keys. When off, uses the old system, where you hold down the key, then moves to the option.")]
        [DefaultValue(true)]
        public bool UseNewOrderSystem { get { return MainMod.TestNewOrderHud; } set { MainMod.TestNewOrderHud = value; } }

        [Label("Show Companions Backward Animations?")]
        [Tooltip("Useful in case you don't feel okay with them.")]
        [DefaultValue(true)]
        public bool ShowBackwardAnimations { get { return MainMod.ShowBackwardAnimations = true; } set { MainMod.ShowBackwardAnimations = value; } }
    }
    [Label("Server Settings")]
    public class ServerConfigMod : ModConfig
    {
        public override ConfigScope Mode
        {
            get { return ConfigScope.ServerSide; }
        }

        [Label("Test multiplayer sync?")]
        [Tooltip("There is no guarantee that it will work, but at least can debug it.")]
        public bool TestMpSync { get { return MainMod.NetplaySync; } set { MainMod.NetplaySync = value; } }

        [Label("Disable Ether Items?")]
        [Tooltip("Requests no longer give Ether Heart and Ether Fruit when turned on.")]
        public bool ClassicMode { get { return MainMod.NoEtherItems; } set { MainMod.NoEtherItems = value; } }

        [Label("Scale Companions to their intended size?")]
        [Tooltip("Each companion may have a set scale change value. That scale value changes their in-game size. That also affects the height required for their houses.")]
        [DefaultValue(true)]
        public bool ScaleCompanions { get { return MainMod.UseCompanionsDefinedScaleChange; } set { MainMod.UseCompanionsDefinedScaleChange = value; } }

        [Label("Disable damage reduction by companion count?")]
        [Tooltip("To make the mod still be fun to play, there is a minor damage reduction applied to everyone when having multiple companions. Disable this if the mod gets unbearable or way too harder for you to play.")]
        public bool DisableDamageReduction { get { return MainMod.DisableDamageReductionByNumberOfCompanions; } set { MainMod.DisableDamageReductionByNumberOfCompanions = value; } }

        [Label("Increase all monsters health based on number of guardians following?")]
        [Tooltip("Only available on singleplayer. Having more than 1 guardian following you, increases monsters health by 5% per guardian.")]
        public bool MobHealthBoost { get { return MainMod.MobHealthBoost; } set { MainMod.MobHealthBoost = value; } }

        /*[Label("Test new guardians combat AI?")]
        [Tooltip("Changes the combat behavior AI to the new AI.")]
        public bool TestNewCombatAI { get { return MainMod.TestNewCombatAI; } set { MainMod.TestNewCombatAI = value; } }*/

        [Label("Add stronger version of monsters based on guardians and their health?")]
        [Tooltip("Makes so monsters can spawn with buffed status, depending on the number of guardians you have summoned, and also their max health.\nDisabling this makes so +3 max monster spawn is added for each guardian.")]
        [DefaultValue(true)]
        public bool UseRaidMonsterBuffing { get { return MainMod.UseNewMonsterModifiersSystem; } set { MainMod.UseNewMonsterModifiersSystem = value; } }

        [Label("Shared Maximum Life and Mana?")]
        [Tooltip("If turned on, your guardians will get the life and mana bonus value based on your characters. Nothing stops you from increasing their personal status cap.")]
        public bool SharedStatus { get { return MainMod.SharedCrystalValues; } set { MainMod.SharedCrystalValues = value; } }

        [Label("Use Skills System?")]
        [Tooltip("If turned on, guardians skills will increase during their travels with you, depending on what they do during that. Those skills gives boost to the guardian status based on the level.")]
        [DefaultValue(true)]
        public bool UseSkillsSystem { get { return MainMod.UseSkillsSystem; } set { MainMod.UseSkillsSystem = value; } }

        [Label("Guardians Health and Mana status like the player.")]
        [Tooltip("All companions will have their health and mana values base and incresed values like the player, regardless of size or anything else.")]
        public bool HealthAndManaPlayerStandards { get { return MainMod.SetGuardiansHealthAndManaToPlayerStandards; } set { MainMod.SetGuardiansHealthAndManaToPlayerStandards = value; } }

        [Label("Companions can visit your world sometimes?")]
        [Tooltip("When turned on, companions you didn't allowed to move in your world, can end up coming to visit you.")]
        [DefaultValue(true)]
        public bool CompanionsCanVisitWorld { get { return MainMod.CompanionsCanVisitWorld; } set { MainMod.CompanionsCanVisitWorld = value; } }

        [Label("Dualwield able weapons.")]
        [Tooltip("Here contains the list of items your companion will be able to dual wield, If It is possible for them to.")]
        public List<ItemDefinition> DualwieldableItems = MainMod.GetDefaultDualwieldableItems();// { get { return MainMod.DualwieldWhitelist; } set { MainMod.DualwieldWhitelist = value; } }

        [Label("Item Attack Range.")]
        [Tooltip("Here contains a list of maximum range to use items. Add a row to items that have limited flight range, and set a pixel distance to It.\n" +
            "If the item is not here, then the mod will use default AI setting for range.\n" +
            "Do not setup the attack ranges here, instead, add the items you want to change attack range, save and then close the game.\n" +
            "Then go to \"My Documents/My Games/Terraria/ModLoader/Mod Configs\" and open \"giantsummon_ServerConfigMod.json\".\nFind the items you added on \"ItemAttackRange\", and set the range there, instead.")]
        [Range(0, 99999)]
        public Dictionary<Terraria.ModLoader.Config.ItemDefinition, int> ItemAttackRange = MainMod.GetDefaultItemRanges();// { get { return MainMod.ItemAttackRange; } set { MainMod.ItemAttackRange = value; } }

        //[Label("Tile Collision is the same as Hit Collision")]
        //[Tooltip("When turned on, the game will no longer try using a player like collision detection on your companions. Will instead use their internal hitbox collision dimension.")]
        //[DefaultValue(false)]
        //public bool UseHitCollisionAsTileCollision { get { return MainMod.TileCollisionIsSameAsHitCollision; } set { MainMod.TileCollisionIsSameAsHitCollision = value; } }

        private void SetDefaultDualwieldableItems()
        {
            MainMod.DualwieldWhitelist = DualwieldableItems; //MainMod.GetDefaultDualwieldableItems();
            MainMod.ItemAttackRange = ItemAttackRange; //MainMod.GetDefaultItemRanges();
        }

        public override void OnLoaded()
        {
            SetDefaultDualwieldableItems();
        }

        public override bool Autoload(ref string name)
        {
            //if (DualwieldableItems.Count == 0)
            //    SetDefaultDualwieldableItems();
            return base.Autoload(ref name);
        }

        public override void OnChanged()
        {
            SetDefaultDualwieldableItems();
            MainMod.ForceUpdateGuardiansStatus = true;
        }
    }
    [Label("Knockout System Settings")]
    public class ReviveSystemConfigMod : ModConfig
    {
        public override ConfigScope Mode
        {
            get { return ConfigScope.ServerSide; }
        }

        [Label("Player can get Knocked Out upon defeat?")]
        [Tooltip("Makes your character enter a dying state when hp drops to 0. You wont be able to do anything in this state, and will be hurt by foes and debuffs, but can be revived by nearby allies.")]
        public bool PlayerKO { get { return MainMod.PlayersGetKnockedOutUponDefeat; } set { MainMod.PlayersGetKnockedOutUponDefeat = value; } }

        [Label("Player can't die when health drops to 0 on Knocked Out state?")]
        [Tooltip("Controls if the player will either die when health reaches 0 on Knocked Out state, or will enter a Knocked Out Cold state. In Knocked Out Cold state, your character health wont restore naturally without the help of others. You can force your death by pressing Grappling Hook key, in case there is no way of reviving. Some kinds of defeats causes death anyway, like dying to lava.")]
        public bool PlayerCanDie { get { return MainMod.PlayersDontDiesAfterDownedDefeat; } set { MainMod.PlayersDontDiesAfterDownedDefeat = value; } }

        [Label("Companions can get Knocked Out upon defeat?")]
        [Tooltip("Makes so companions enter a dying state when hp drops to 0. They wont be able to do anything in this state, and will be hurt by foes and debuffs, but can be revived by nearby allies, includding you.")]
        public bool GuardianKO { get { return MainMod.GuardiansGetKnockedOutUponDefeat; } set { MainMod.GuardiansGetKnockedOutUponDefeat = value; } }

        [Label("Companions can't die when health drops to 0 on Knocked Out state?")]
        [Tooltip("Controls if the companions will either die when health reaches 0 on Knocked Out state, or will enter a Knocked Out Cold state. In Knocked Out Cold state, the companions health wont be restored naturally without the help of others. There is no other way of reviving them, without the help of others. Some kinds of defeats causes death anyway, like dying to lava.")]
        public bool GuardianCanDie { get { return MainMod.GuardiansDontDiesAfterDownedDefeat; } set { MainMod.GuardiansDontDiesAfterDownedDefeat = value; } }

        [Label("Should the character be rescued by awaken guardians when knocked out cold?")]
        [Tooltip("If this option is on, your character will always be rescued from wherever It is when knocked out cold by a living town companion. If no living companion is around, your character will be set to the spawn in Knocked Out state, with low health.")]
        [DefaultValue(true)]
        public bool UseRescue { get { return !MainMod.DoNotUseRescue; } set { MainMod.DoNotUseRescue = !value; } }

        [Label("Start rescue countdown when your character is knocked out cold?")]
        [Tooltip("Regardless of wether or not there are companions alive near you, the rescue countdown will run while your character isn't revived by someone.")]
        [DefaultValue(false)]
        public bool StartReviveCountdownWhenKnockedOutCold { get { return MainMod.StartRescueCountdownWhenKnockedOutCold; } set { MainMod.StartRescueCountdownWhenKnockedOutCold = value; } }

        [Label("Companions speaks while reviving someone?")]
        [Tooltip("Makes the companion speak to the ones they are trying to revive.")]
        [DefaultValue(true)]
        public bool GuardianCanReviveChatter { get { return MainMod.CompanionsSpeaksWhileReviving; } set { MainMod.CompanionsSpeaksWhileReviving = value; } }
    }
}

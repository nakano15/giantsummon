# Introduction
Confusing, isn't? This is the source of TerraGuardians mod.

If you don't know what TerraGuardians mod is, It is a companion mod for Terraria. It allows you to have custom companion, unlike Terrarian characters, aswell as Terrarian looking like companions. Since the mod is made by me (Nakano15), that means not only that is in the mod.

The TerraGuardians themselves are human-animal mixed creatures with different appearances and status. Due to the fact that they come in varied sizes, colors and styles, that means I'm not bound into making them Terrarian like.

# Source Map

Buffs = This is where the custom mod buffs are sent.

Compatibility = Scripts for other mod compatibilities are sent here.

Creatures = Guardian definitions their and sprites goes here. This is unrelated to their npc.

Extra = Only has Sardine's sprite for when inside the King Slime.

GuardianNPC = This is where the npc definition of the guardians are placed. Their head sprite is necessary to see them in the world map.

Ideas = A folder containing ideas for the mods, be either from me, or fans.

Interface = Interface element sprites.

Items = Various mod items are here, from guardian items to items Terrarians can use.

Npcs = Here contains some of the pre-recruit guardian npcs.

PlayerItems = Only has Staff of Renew inside.

Projectiles = Custom projectiles are placed here. Only Zacks recruitment related projectiles are inside.

TerraGuardian Sprites = TerraGuardian sprites are located here. Almost their files are Aseprite files.

AlexRecruitScripts.cs = Scripts related to Alex's recruitment. Also contains a constant with his old owner's name.

BuffData.cs = For TerraGuardians buffs.

ChangeLog.txt = Gotta keep track of what changed.

ConfigMod.cs = The script of the mod configurations are located here.

DialogueChain.cs = Handles a quite poor quiz system to the mod. Used by Bree's recruitment.

ExternalCompatibilityScripts.cs = My attempt of allowing other modders to add their mod compatibility to my mod.

GuardianActions.cs = Handles the guardian action system. Actions makes so the guardians executes certain functions until It is ended. You can count it as a kind of behavior. Many systems makes use of it.

GuardianAnimationPoints.cs = Used to get certain parts positions on guardian animation frames. DefaultCoordinate is returned if there is no entry for a specific frame.

GuardianArmorAndSetEffects.cs = Here is placed the scripts for what each equipment and set will offer to the companion.

GuardianBase.cs = This is the common definition of the guardian, containing data all of that companion will use. With a guardian inheriting this, can setup your own custom guardian, but the companion definition must be inside Creatures folder.

GuardianBaseContainer.cs = Used to help finding out what guardian the mod needs, based on the mod It is from.

GuardianBountyQuest.cs = Scripts handling the bounty quest system.

GuardianCooldownManager.cs = Handles the cooldowns and counters used by the companions. 

GuardianData.cs = This is the specific definition of the guardian, containing data specific for that guardian, different from others of the same type. The players for example makes use of this.

GuardianFlags.cs = Holds the flags companions may use. Mostly tied to effects and friendship level bonuses.

GuardianManagement.cs = Opens the guardian inventory and equipment, when clicking on the Inventory button at the Guardian Selection Interface.

GuardianOrderHud.cs = The script of the old order hud.

GuardianOrderHudNew.cs = The script of the new order hud.

GuardianPlayerAccessoryEffects.cs = Holds the effects the accessories causes on the companions

GuardianRequests.cs = The request system script is here.

GuardianSchedule.cs = This is a system I paused developing, which would add a kind of "schedule" to guardian town npcs.

GuardianSelectionInterface.cs = Script of the Guardian Selection Interface is here.

GuardianSkills.cs = Script of the guardian skills and their effects.

GuardianSprites.cs = It's an object that kind of manages and carries the guardian sprites in it. It will call the guardian sprites when necessary, to save memory.

ItemMod.cs = Item effect scripts are here.

MainMod.cs = Main mod file.

ModGuardianData.cs = This was my attempt of adding custom mod data loading, saving and holding for other mods. No idea how to proceed right now, though.

ModTargetting.cs = Aids when trying to either get a companion, or a player.

NetMod.cs = The netplay sync scripts.

Notes.txt = //Why is this here?

NpcMod.cs = Npc scripts.

PathFinder.cs = My attempt at adding path finding. Somehow doesn't works correctly.

PlayerDataBackup.cs = This is used when masking projectiles players with a terraguardian. Disposing of it causes the player data to be restored.

PlayerMod.cs = Player scripts.

ProjMod.cs = Projectile scripts.

RenameGuardianCommand.cs = Rename Guardian command. I don't recommend using it on your own, though. Use the pencil on the GSI.

SavePlayerDefsCommand.cs = This is very useful If you want to create a companion that is a Terrarian, It will tell you the colors and skin variant used for the character.

SoundData.cs = Used to store mod sound info for when the companion is hurt or defeated.

TerraGuardian.cs = The main script of the companions. Everything related to their behavior and AI is here.

TriggerHandler.cs = I'm still developing this system. Triggers will affect guardians nearby a certain range (or just everyone), telling that something happened. This will be useful for specific guardian behaviors when something happens.

UtilityMethods.cs = Only contains a couple of Beziers scripts.

WorldMod.cs = World scripts.

todo.txt = I nearly never read this, so you may kind of ignore it.

#Finishing up
Well, that's about it. By the way, take it easy, It's the first time I'm using github to backup my codes. The method I were using until today, was zipping my mod source and sending to my Mega account.

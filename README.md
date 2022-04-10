# Introduction
Confusing, isn't? This is the source of TerraGuardians mod.

If you don't know what TerraGuardians mod is, It is a companion mod for Terraria. It allows you to have custom companion, unlike Terrarian characters, aswell as Terrarian looking like companions. Since the mod is made by me (Nakano15), that means not only that is in the mod.

The TerraGuardians themselves are human-animal mixed creatures with different appearances and status. Due to the fact that they come in varied sizes, colors and styles, that means I'm not bound into making them Terrarian like.

# Source Map

Actions = Contains the scripts of actions companions can take in-game. The actions overrides their AI while they're active, so they can execute whatever action they need to do.

Buffs = This is where the custom mod buffs are sent.

Compatibility = Scripts for other mod compatibilities are sent here.

Companions = Companions base definitions and their sprites are contained here. This is unrelated to their npc. If you want to avoid spoilers, better not check this. And if you want to avoid major spoilers, avoid their scripts. Creatures folder is for TerraGuardians companions, while Terrarians is for Human companions.

Extra = Only has Sardine's sprite for when his recruit npc spawns inside the King Slime.

GuardianNPC = This is where the npc definition of the guardians are placed. Their head sprite is necessary to see them in the world map.

Ideas = A folder containing ideas for the mods, be either from me, or fans.

Interface = Interface element sprites.

Items = Various mod items are here, from guardian items to items Terrarians can use.

Npcs = Here contains some of the pre-recruit guardian npcs.

PlayerItems = Only has Staff of Renew inside.

Projectiles = Custom projectiles are placed here.

Quests = The quests that you can find in the game are contained here. Well, at least most of them, since some of them may be work in progress.

Requests = In this folder contains the array of requests companions can give you. Each request can spawn at random if their requirement is met.

TerraGuardian Sprites = TerraGuardian sprites are located here. Almost their files are Aseprite files.

Workbench = Contains files used by mod contributors to aid the mod development with content. Some of them may contain spoilers.

//Main files

AlexRecruitScripts.cs = Scripts related to Alex's recruitment. Also contains a constant with his old owner's name.

BuffData.cs = For TerraGuardians buffs.

ChangeLog.txt = Gotta keep track of what changed.

CommonRequestsDB.cs = Contains the list of common requests any companion can give. Those requests are quite generic and vague.

ConfigMod.cs = The script of the mod configurations are located here.

Dialogue.cs = An attempt of easing dialogue handling, without having to mess with GuardianMouseOverAndDialogueInterface.

DialogueChain.cs = Handles a quite poor quiz system to the mod. Used by Bree's recruitment.

FeatMentioning.cs = A system which allows companions to not only carry, but mention feat other player characters did on their adventures.

GuardianActions.cs = Handles the guardian action system. Actions makes so the guardians executes certain functions until It is ended. You can count it as a kind of behavior. Many systems makes use of it.

GuardianAnimationPoints.cs = Used to get certain parts positions on guardian animation frames. DefaultCoordinate is returned if there is no entry for a specific frame.

GuardianArmorAndSetEffects.cs = Here is placed the scripts for what each equipment and set will offer to the companion.

GuardianBase.cs = This is the common definition of the guardian, containing data all of that companion will use. With a guardian inheriting this, can setup your own custom guardian, but the companion definition must be inside Creatures folder.

GuardianBaseContainer.cs = Used to help finding out what guardian the mod needs, based on the mod It is from.

GuardianBountyQuest.cs = Scripts handling the bounty quest system.

GuardianCommonStatus.cs = Holds the status companions will carry across save files. This includes Life Crystal, Mana Crystal, Life Fruit and Skills progression.

GuardianCooldownManager.cs = Handles the cooldowns and counters used by the companions. 

GuardianData.cs = This is the specific definition of the guardian, containing data specific for that guardian, different from others of the same type. The players for example makes use of this.

GuardianDrawData.cs = When adding draw data to companions, this class should be used instead. It allows telling which kind of data is being stored here, in case modifying drawing of a character is necessary. (For example, form change, skin or outfit change).

GuardianFlags.cs = Holds the flags companions may use. Mostly tied to effects and friendship level bonuses.

GuardianGlobalInfos.cs = Holds global information for the mod, that should persist regardless of which character you're playing with. For example, the feats and ether realm time.

GuardianID.cs = Holds the ID of the companion, and lets you check if the companion id you input is the same.

GuardianManagement.cs = Opens the guardian inventory and equipment, when clicking on the Inventory button at the Guardian Selection Interface.

GuardianMouseOverAndDialogueInterface.cs = This is a mistake. I mean, handles mouse over, and also dialogues of the companions. The issue with this is if you need the help of this script file to make dialogues happen. I created Dialogue class to try easing the need of this class to handle it.

GuardianOrderHud.cs = The script of the old order hud.

GuardianOrderHudNew.cs = The script of the new order hud.

GuardianPlayerAccessoryEffects.cs = Holds the effects the accessories causes on the companions

GuardianSchedule.cs = This is a system I paused developing, which would add a kind of "schedule" to guardian town npcs. Would be cool if they had their own daily lives, right?

GuardianSelectionInterface.cs = Script of the Guardian Selection Interface is here.

GuardianShopHandler.cs = Script that takes care of the logic of the companions shop. By the time I have written this, only Domino has a store.

GuardianShopInterface.cs = Script for the guardian shop interface. It surelly needs rework.

GuardianSkills.cs = Script of the guardian skills and their effects.

GuardianSpawnTip.cs = Holds spawn tips for companions. Luna allows you to read those tips, which comes at random.

GuardianSpawningScript.cs = Handles Cille and Michelle spawning.

GuardianSpecialAttack.cs = Holds the base script necessary for making special attacks for companions. Companions like Sally Stench uses this.

GuardianSprites.cs = It's an object that kind of manages and carries the guardian sprites in it. It will call the guardian sprites when necessary, to save memory.

ItemMod.cs = Item effect scripts are here.

MainMod.cs = Main mod file.

ModGuardianData.cs = This was my attempt of adding custom mod data loading, saving and holding for other mods. No idea how to proceed right now, though.

ModTargetting.cs = Aids when trying to either get a companion, or a player.

NetMod.cs = The netplay sync scripts.

NpcMod.cs = Npc scripts.

Node.cs = Used for the path finding system of the mod.

PathFinder.cs = My attempt at adding path finding. Somehow doesn't works correctly.

PlayerDataBackup.cs = This is used when masking projectiles players with a terraguardian. Disposing of it causes the player data to be restored.

PlayerMod.cs = Player scripts.

ProjMod.cs = Projectile scripts.

QuestBase.cs = Contains the base info necessary for making quests. Check Quests folder for how to use.

QuestContainer.cs = Container that not only holds the quests, but also allows enumerating them. Also has support to other mods quests.

QuestData.cs = Allows storing important infos from quests, like your progress on it.

RenameGuardianCommand.cs = Rename Guardian command. I don't recommend using it on your own, though. Use the pencil on the GSI.

RequestBase.cs = Is the base information used by requests, like request name, and objectives. Also holds classes that are useful for quick starting requests types, like HuntRequest and ItemRequests. Extending this allows you to create custom request types.

RequestData.cs = The request progress and information that is stored on the companions. The request data depends on the class that extends RequestBase.

RequestReward.cs = Used for storing possible request rewards the mod will give to the player, upon completting a request. This will randomly pick an item from an array of possible rewards, taking in account that the higher acquisition rate, increases the chance of the item being picked.

SavePlayerDefsCommand.cs = This is very useful If you want to create a companion that is a Terrarian, It will tell you the colors and skin variant used for the character.

Seasons.cs = An enumerator containing the seasons of the mod.

SoundData.cs = Used to store mod sound info for when the companion is hurt or defeated.

TerraGuardian.cs = The main script of the companions. Mostly everything related to them is here.

TriggerHandler.cs = I'm still developing this system. Triggers will affect guardians nearby a certain range (or just everyone), telling that something happened. This will be useful for specific guardian behaviors when something happens.

TrustLevels.cs = Contains infos about the companions trust level system.

UtilityMethods.cs = Only contains a couple of Beziers scripts.

WorldMod.cs = World scripts.

todo.txt = I nearly never read this, so you may kind of ignore it.

#Finishing up
Well, that's about it. By the way, take it easy, It's the first time I'm using github to backup my codes. The method I were using until today, was zipping my mod source and sending to my Mega account.

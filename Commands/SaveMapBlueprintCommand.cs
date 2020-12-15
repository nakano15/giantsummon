using Terraria.ModLoader;
using Terraria;

namespace giantsummon.Commands
{
    public class SaveMapBlueprintCommand : ModCommand
    {
        public override string Command { get { return "savemapblueprint"; } }

        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (!Compatibility.SubWorlds.IsModInstalled)
            {
                Main.NewText("This command can only be used with Subworlds Mod installed, and while inside a Subworld created for this mod.");
                return;
            }
            if (!SubworldLibrary.SLWorld.subworld)
            {
                Main.NewText("You are not in a Subworld.");
                return;
            }
            if (SubworldLibrary.SLWorld.currentSubworld is SubWorlds.SubworldBase swb)
            {
                SubWorlds.SubworldBlueprint.SaveSubworldBlueprint();
            }
            else
            {
                Main.NewText("This isn't a subworld made for this TerraGuardians mod.");
            }
        }
    }
}

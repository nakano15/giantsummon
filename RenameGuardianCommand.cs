using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class RenameGuardianCommand : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Description
        {
            get
            {
                return "Allows you to give a nickname to a TerraGuardian. Don't try using this by yourself, click the pencil near the Guardian name, on the Guardian Selection Window.";
            }
        }

        public override string Command
        {
            get { return "renameguardian"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 3)
            {
                caller.Reply("Command parameters are invalid.", Color.Red);
            }
            else
            {
                int GuardianID;
                string ModID;
                string NewName = "";
                if (!int.TryParse(args[0], out GuardianID))
                {
                    caller.Reply("Invalid guardian id. ", Color.Red);
                }
                else
                {
                    ModID = args[1];
                    //NewName = args[2];
                    for (int i = 2; i < args.Length; i++)
                    {
                        if (i > 2)
                            NewName += " ";
                        NewName += args[i];
                    }
                    if (NewName.Length < 2 || NewName.Length > 16)
                    {
                        caller.Reply("Guardian nickname length must be between 2 and 16 characters.", Color.Red);
                    }
                    else
                    {
                        if (!caller.Player.GetModPlayer<PlayerMod>().HasGuardian(GuardianID, ModID))
                        {
                            caller.Reply("Invalid guardian id.", Color.Red);
                        }
                        else
                        {
                            GuardianData gd = caller.Player.GetModPlayer<PlayerMod>().GetGuardian(GuardianID, ModID);
                            if (gd.CanChangeName)
                            {
                                caller.Reply(gd.Base.Name + "'s nickname changed to " + NewName + ".");
                                gd.Name = NewName;
                            }
                            else
                            {
                                caller.Reply("You can't change " +gd.Name + "'s nickname again, buy a Rename Card from the Dryad and use to allow changing it.");
                            }
                        }
                    }
                }
            }
        }
    }
}

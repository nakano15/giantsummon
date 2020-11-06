using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class ChangeNicknameCommand : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Description
        {
            get
            {
                return "Changes how the companions will call you.";
            }
        }

        public override string Command
        {
            get { return "changenickname"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length > 0)
            {
                caller.Player.GetModPlayer<PlayerMod>().GetNickname = args[0];
                Main.NewText("Your companions will now begin calling you '" + args[0] + "'.");
            }
        }
    }
}

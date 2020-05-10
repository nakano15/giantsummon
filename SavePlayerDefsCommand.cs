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
    public class SavePlayerDefsCommand : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Description
        {
            get
            {
                return "Saves some infos about your character on your desktop. Those infos will be helpful when creating a custom companion.";
            }
        }

        public override string Command
        {
            get { return "saveplayerdefs"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (Main.netMode == 2)
                return;
            string Infos = "Name: " + caller.Player.name+ Environment.NewLine +
                "Hair Style: " + caller.Player.hair + Environment.NewLine +
                "Is Male? " + caller.Player.Male + Environment.NewLine +
                "Skin Variant: " + caller.Player.skinVariant + Environment.NewLine +
                "Hair Color: " + caller.Player.hairColor + Environment.NewLine +
                "Eyes Color: " + caller.Player.eyeColor + Environment.NewLine +
                "Skin Color: " + caller.Player.skinColor + Environment.NewLine +
                "Shirt Color: " + caller.Player.shirtColor + Environment.NewLine +
                "Undershirt Color: " + caller.Player.underShirtColor + Environment.NewLine +
                "Pants Color: " + caller.Player.pantsColor + Environment.NewLine +
                "Shoes Color: " + caller.Player.shoeColor;
            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/PlayerSkinInfo.txt";
            if (File.Exists(FilePath))
                File.Delete(FilePath);
            using (FileStream stream = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(Infos);
                }
            }
            Main.NewText("Your character infos are now in your desktop.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Compatibility
{
    public class HubWorlds
    {
        public static bool IsModInstalled { get { return MainMod.SubworldLibrary != null; } }

        public static void EnterWorld(string WorldID)
        {
            SubworldLibrary.Subworld.Enter(WorldID);
        }

        public static void LeaveWorld(string WorldID)
        {
            SubworldLibrary.Subworld.Exit();
        }
    }
}

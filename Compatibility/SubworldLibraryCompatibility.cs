using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria;

namespace giantsummon.Compatibility
{
    public class SubworldLibraryCompatibility
    {
        //https://github.com/jjohnsnaill/SubworldLibrary/wiki/Mod.Call
        public static bool IsModActive { get { return MainMod.SubworldLibrary != null; } }

        public class SubworldInfo
        {
            public bool Success = false;
            private string ID = "";
            public string GetID { get { return ID; } }
            public Mod mod;
            public string Name = "";
            public int MapWidth = 400, MapHeight = 400;
            public ModWorld modWorld = null;
            public bool SaveSubworld = false, SaveModData = false, NoWorldUpdate = false, DisablePlayerSaving = false;
            public List<Terraria.World.Generation.GenPass> Tasks = new List<Terraria.World.Generation.GenPass>();
            public Terraria.UI.UserInterface LoadingUI = null;
            public Func<Terraria.UI.UIState> loadingUIState = null, votingUI = null;
            public Action OnLoad = null, OnUnload = null, OnVoteFor = null;
            public int VotingDuration = 1800;

            public SubworldInfo(string Name, Mod mod, int Width, int Height, List<Terraria.World.Generation.GenPass> Tasks)
            {
                this.mod = mod;
                this.Name = Name;
                this.MapWidth = Width;
                this.MapHeight = Height;
                this.Tasks = Tasks;
            }

            public void Register()
            {
                if (!SubworldLibraryCompatibility.IsModActive)
                {
                    Main.NewText("Subworld Library isn't installed.");
                    return;
                }
                object Value = mod.Call(new object[] { "Register", mod, Name, MapWidth, MapHeight, Tasks, OnLoad, OnUnload, modWorld, SaveSubworld,  DisablePlayerSaving, SaveModData,
                NoWorldUpdate, LoadingUI, loadingUIState, votingUI, VotingDuration, OnVoteFor });
                if (Value == null)
                    Success = false;
                else
                {
                    ID = (string)Value;
                    Success = true;
                }
            }

            public void Enter()
            {
                if (Success)
                {
                    mod.Call(new object[] { "Enter", ID });
                }
                else
                {
                    Main.NewText("This world is not ready.");
                }
            }

            public void Exit()
            {
                mod.Call(new object[] { "Exit" });
            }

            public string Current()
            {
                object o = mod.Call(new object[] { "Current" });
                if (o == null) return null;
                return (string)o;
            }

            public bool IsActive()
            {
                return (bool)mod.Call(new object[] { "IsActive", ID });
            }

            public bool AnyActive()
            {
                return (bool)mod.Call(new object[] { "IsActive", mod });
            }
        }
    }
}

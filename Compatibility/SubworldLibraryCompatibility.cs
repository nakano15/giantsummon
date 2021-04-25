using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

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
            public int Width = 400, Height = 400;
            public ModWorld modWorld = null;
            public bool SaveSubworld = false, SaveModData = false, NoWorldUpdate = false, DisablePlayerSaving = false;
            public Terraria.UI.UserInterface LoadingUI = null;
            public Func<Terraria.UI.UIState> loadingUIState = null, votingUI = null;
            public Action OnLoad = null, OnUnload = null, OnVoteFor = null;
            public ushort VotingDuration = 1800;

            public SubworldInfo(string Name, Mod mod, int Width, int Height)
            {
                this.mod = mod;
                this.Name = Name;
                this.Width = Width;
                this.Height = Height;
            }

            public bool Register()
            {
                if (!SubworldLibraryCompatibility.IsModActive)
                {
                    Main.NewText("Subworld Library isn't installed.");
                    return false;
                }
                object Value = MainMod.SubworldLibrary.Call("Register", mod, Name, Width, Height, Tasks(), OnLoad, OnUnload, 
                    modWorld, SaveSubworld,  DisablePlayerSaving, SaveModData, NoWorldUpdate, LoadingUI, loadingUIState, 
                    votingUI, VotingDuration, OnVoteFor);
                if (Value == null)
                {
                    Success = false;
                    Main.NewText("World creation failed.");
                }
                else
                {
                    ID = (string)Value;
                    Success = true;
                    Main.NewText("World created sucessfully.");
                }
                return Success;
            }

            public void Enter()
            {
                if (Success)
                {
                    MainMod.SubworldLibrary.Call(new object[] { "Enter", ID });
                }
                else
                {
                    Main.NewText("This world is not ready.");
                }
            }

            public void Exit()
            {
                MainMod.SubworldLibrary.Call(new object[] { "Exit" });
            }

            public string Current()
            {
                object o = MainMod.SubworldLibrary.Call(new object[] { "Current" });
                if (o == null) return null;
                return (string)o;
            }

            public bool IsActive()
            {
                return (bool)MainMod.SubworldLibrary.Call(new object[] { "IsActive", ID });
            }

            public bool AnyActive()
            {
                return (bool)MainMod.SubworldLibrary.Call(new object[] { "IsActive", mod });
            }

            public virtual List<Terraria.World.Generation.GenPass> Tasks()
            {
                List<Terraria.World.Generation.GenPass> DefaultPass = new List<Terraria.World.Generation.GenPass>();
                DefaultPass.Add(new PassLegacy("ground gen", delegate (GenerationProgress progress)
                {
                    progress.Message = "Creating Ground";
                    int StartHeight = (int)(Height * 0.6f);
                    for (int x = 0; x < Width; x++)
                    {
                        progress.Value = (float)x / Width;
                        for (int y = 0; y < Height; y++)
                        {
                            Main.tile[x, y].active(true);
                            Main.tile[x, y].type = Terraria.ID.TileID.Dirt;
                        }
                    }
                }, 0.1f));
                return DefaultPass;
            }

            public virtual void ModifySpawns(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
            {

            }

            public virtual void UpdatePlayerStatus(Player player)
            {

            }
        }
    }
}

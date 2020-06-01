using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class BearNPC : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Vladimir_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "BearGuardian";
            return mod.Properties.Autoload;
        }

        public BearNPC()
            : base(GuardianBase.Vladimir)
        {

        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            base.SetChatButtons(ref button, ref button2);
            if (!CheckingRequest)
            {
                if (Guardian.DoAction.InUse && Guardian.DoAction.ID == 0 && Guardian.DoAction.IsGuardianSpecificAction)
                {
                    button2 = "Enough";
                }
                else
                {
                    button2 = "Be Hugged";
                }
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                base.OnChatButtonClicked(firstButton, ref shop);
            }
            else
            {
                if (CheckingRequest)
                {
                    base.OnChatButtonClicked(firstButton, ref shop);
                }
                else if (Guardian.DoAction.InUse && Guardian.DoAction.ID == 0 && Guardian.DoAction.IsGuardianSpecificAction)
                {
                    Guardian.DoAction.InUse = false;

                    Main.npcChatText = (((Creatures.VladimirBase)Guardian.Base).GetEndHugMessage(Guardian));
                    Guardian.DoAction.Players[0].Bottom = Guardian.Position;
                }
                else if (!Guardian.DoAction.InUse)
                {
                    PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                    if (player.MountedOnGuardian || player.GuardianMountingOnPlayer)
                    {
                        Main.npcChatText = "*Get off your guardian first.*";
                    }
                    else
                    {
                        GuardianActions ga = Guardian.StartNewGuardianAction(0);
                        if (ga != null)
                        {
                            ga.Players.Add(Main.player[Main.myPlayer]);
                            Main.npcChatText = "*Press Jump button If you want me to stop.*";
                        }
                    }
                }
                else
                {
                    Main.npcChatText = "*I can't right now.*";
                }
            }
        }
    }
}

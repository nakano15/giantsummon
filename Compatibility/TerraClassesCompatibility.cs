using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TerraClasses;

namespace giantsummon.Compatibility
{
    public class TerraClassesCompatibility
    {
        /*public static bool IsModActive { get { return MainMod.TerraClassesMod != null; } }

        public static List<TerraGuardianTarget> GetTargets(Terraria.Player Caster, bool Allies)
        {
            List<TerraGuardianTarget> Targets = new List<TerraGuardianTarget>();
            foreach(TerraGuardian tg in MainMod.ActiveGuardians.Values)
            {
                if(tg.IsPlayerHostile(Caster) != Allies)
                {
                    Targets.Add(new TerraGuardianTarget(tg));
                }
            }
            return Targets;
        }

        public class TerraGuardianTarget : TerraClasses.TargetTranslator.Translator
        {
            public TerraGuardianTarget(TerraGuardian tg)
            {
                this.tg = tg;
            }

            public TerraGuardian tg;
            public override object Target => tg;
            public override string Name => tg.Name;
            public override string CharacterIdentifier => "tg"+tg.WhoAmID;

            public override bool Male { get => tg.Male; set { if (tg.Base.CanChangeGender) tg.Male = value; } }
            public override int Health { get => tg.HP; set => tg.HP = value; }
            public override int MaxHealth { get => tg.MHP; set => tg.MHP = value; }
            public override int Mana { get => tg.MP; set => tg.MP = value; }
            public override int MaxMana { get => tg.MMP; set => tg.MMP = value; }

        }*/
    }
}

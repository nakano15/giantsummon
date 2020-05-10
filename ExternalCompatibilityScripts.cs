using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class ExternalCompatibilityScripts
    {
        public delegate void UpdateGuardianDel(TerraGuardian guardian);
        private static List<UpdateGuardianDel> ResetStatusScript = new List<UpdateGuardianDel>(),
            UpdateStatusScript = new List<UpdateGuardianDel>(),
            UpdateBehaviorScript = new List<UpdateGuardianDel>();
        public delegate ModGuardianData ModGuardianDataCreationScript(GuardianData guardian);
        private static List<ModGuardianDataCreationScript> GuardianDataCreationScripts = new List<ModGuardianDataCreationScript>();

        public static void AddResetStatusScript(UpdateGuardianDel script)
        {
            ResetStatusScript.Add(script);
        }

        public static void AddUpdateStatusScript(UpdateGuardianDel script)
        {
            UpdateStatusScript.Add(script);
        }

        public static void AddUpdateBehaviorScript(UpdateGuardianDel script)
        {
            UpdateBehaviorScript.Add(script);
        }

        public static void AddGuardianData(UpdateGuardianDel script)
        {
            UpdateBehaviorScript.Add(script);
        }

        public static void RunGuardianDataCreationScripts(GuardianData guardian)
        {
            foreach (ModGuardianDataCreationScript del in GuardianDataCreationScripts)
            {
                del(guardian);
            }
        }

        public static void RunResetScript(TerraGuardian guardian)
        {
            foreach (UpdateGuardianDel del in ResetStatusScript)
            {
                del(guardian);
            }
        }

        public static void RunUpdateStatusScript(TerraGuardian guardian)
        {
            foreach (UpdateGuardianDel del in UpdateStatusScript)
            {
                del(guardian);
            }
        }

        public static void RunBehaviorStatusScript(TerraGuardian guardian)
        {
            foreach (UpdateGuardianDel del in UpdateBehaviorScript)
            {
                del(guardian);
            }
        }
    }
}

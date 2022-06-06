using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace KK_AgeProgression
{
    internal static class AgeProMasturbationHook
    {
        internal static void ApplyHooks(Harmony instance)
        {
            instance.PatchAll(typeof(AgeProMasturbationHook));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(HMasturbation),"Proc")]
        public static void AgeProFinishGaugeDownPost(ChaControl ___female, ref HFlag ___flags)
        {
            if (GrowthConfig.IsDisabled.Value == false)
            {   
                if (___female.getAnimatorStateInfo(0).IsName("Orgasm_B"))
                {
                    OrgasmCounter.ChangeMasOrgasmState("Orgasm_B");
                }
                if (!(___female.getAnimatorStateInfo(0).IsName(OrgasmCounter.GetMasOrgasmState())) && OrgasmCounter.GetMasOrgasmState() == "Orgasm_B")
                {
                    OrgasmCounter.ChangeMasOrgasmState("None");
                    ___flags.count.aibuOrg++; //might make the plugin incompatible with other plugins
                }
            }
            
        }
    }
}

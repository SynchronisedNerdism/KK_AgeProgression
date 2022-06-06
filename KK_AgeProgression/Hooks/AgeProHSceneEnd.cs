using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace KK_AgeProgression
{
    internal static class AgeProHSceneEnd
    {
        internal static void ApplyHooks(Harmony instance)
        {
            instance.PatchAll(typeof(AgeProHSceneEnd));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(HSprite), "OnClickHSceneEnd")]
        public static void AgeProHSceneEndPost()
        {
            //reset
            AgeProHSceneStart.FirstTime = true;
            AgeProHSceneStart.IsSolo = true;
            OrgasmCounter.ResetCounter();
            OrgasmCounter.ChangeMasOrgasmState("None");
            for(int i = 0; i < GrowthConfig.TotalGrowths; i++)
            {
                GrowthConfig.OriginalSizes[i] = -1;
                GrowthConfig.OriginalSizes2[i] = -1;


            }
        }
    }
}

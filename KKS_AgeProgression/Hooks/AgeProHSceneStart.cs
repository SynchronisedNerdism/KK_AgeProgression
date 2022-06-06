using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace KK_AgeProgression
{
    internal static class AgeProHSceneStart
    {
        internal static bool FirstTime = true;
        internal static bool IsSolo = true;

        internal static void ApplyHooks(Harmony instance)
        {
            instance.PatchAll(typeof(AgeProHSceneStart));
        }

        [HarmonyPrefix, HarmonyPatch(typeof(HSprite), "InitHeroine")]
        public static void AgeProHSceneStartPost(List<ChaControl> ___females, HFlag ___flags)
        {
            if(FirstTime)
            {
                //Initialise values
                //AgePro.Logger.LogMessage(___females[0].GetShapeBodyValue(4) + ___females[1].GetShapeBodyValue(4));
                for(int i=0; i < GrowthConfig.TotalGrowths; i++)
                {
                    GrowthConfig.OriginalSizes[i] = ___females[0].GetShapeBodyValue(GrowthConfig.GrowthIndexes[i]-1);
                }

                if (___flags.mode == HFlag.EMode.lesbian || ___flags.mode == HFlag.EMode.sonyu3P || ___flags.mode == HFlag.EMode.houshi3P)
                {
                    IsSolo = false;
                    for (int i = 0; i < GrowthConfig.TotalGrowths; i++)
                    {
                        GrowthConfig.OriginalSizes2[i] = ___females[1].GetShapeBodyValue(GrowthConfig.GrowthIndexes[i]-1);
                    }
                }
                FirstTime = false;
            }
            
        }
    }
}

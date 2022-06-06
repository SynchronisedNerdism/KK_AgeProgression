using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace KK_AgeProgression
{
    internal static class AgeProFemaleGaugeHook
    {
        //fem = UnityEngine.Object.FindObjectOfType(CharFemale)

        internal static void ApplyHooks(Harmony instance)
        {
            instance.PatchAll(typeof(AgeProFemaleGaugeHook));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(HFlag), "FemaleGaugeUp")]
        public static void AgeProFemaleGaugeUpPost(HFlag __instance)
        {
            if (GrowthConfig.IsInstant.Value == true && GrowthConfig.AttachedMale.Value == false && GrowthConfig.IsDisabled.Value == false)
            {
                if (OrgasmCounter.CheckCounter(__instance.GetOrgCount())==true)
                {
                    UpdateAgePro(__instance);
                }
            }
            else if (GrowthConfig.AttachedMale.Value == false && GrowthConfig.IsDisabled.Value == false)
            {
                UpdateAgePro(__instance);
                
            }
        }
        
        public static SaveData.Heroine AgeProGetTargetHeroine(HFlag __instance, int index)
        {
            return __instance.lstHeroine[index];
        }

        public static void UpdateAgePro(HFlag __instance)
        {
            if (GrowthConfig.IsOriginalSize.Value == true)
            {
                for (int i = 0; i < GrowthConfig.TotalGrowths; i++)
                {
                    if (GrowthConfig.IsGrowthEnabled[i].Value)
                    {
                        AgeProGetTargetHeroine(__instance,0).chaCtrl.SetShapeBodyValue(GrowthConfig.GrowthIndexes[i] - 1, AgeProOriginalInterpolateSize(__instance, true, i));
                    }
                }
                UpdateParameters(__instance, 0);

                if (!AgeProHSceneStart.IsSolo)
                {
 
                    for (int i = 0; i < GrowthConfig.TotalGrowths; i++)
                    {
                        if (GrowthConfig.IsGrowthEnabled[i].Value)
                        {
                            AgeProGetTargetHeroine(__instance, 1).chaCtrl.SetShapeBodyValue(GrowthConfig.GrowthIndexes[i] - 1, AgeProOriginalInterpolateSize(__instance, false, i));
                        }
                        
                    }
                    UpdateParameters(__instance, 1);
                }
            }
            else
            {
                for (int i = 0; i < GrowthConfig.TotalGrowths; i++)
                {
                    if (GrowthConfig.IsGrowthEnabled[i].Value)
                    {
                        AgeProGetTargetHeroine(__instance,0).chaCtrl.SetShapeBodyValue(GrowthConfig.GrowthIndexes[i] - 1, AgeProInterpolateSize(__instance, i));
                    }
                    
                }

                if (!AgeProHSceneStart.IsSolo)
                {
                    for (int i = 0; i < GrowthConfig.TotalGrowths; i++)
                    {
                        if (GrowthConfig.IsGrowthEnabled[i].Value)
                        {
                            AgeProGetTargetHeroine(__instance, 1).chaCtrl.SetShapeBodyValue(GrowthConfig.GrowthIndexes[i] - 1, AgeProInterpolateSize(__instance, i));
                        }
                    }
                    UpdateParameters(__instance,1);
                }
            }
        }
        
        public static void UpdateParameters(HFlag __instance, int index)
        {
            AgeProGetTargetHeroine(__instance,index).chaCtrl.updateShapeBody = true;
            AgeProGetTargetHeroine(__instance,index).chaCtrl.UpdateBustSoftnessAndGravity();
            AgeProGetTargetHeroine(__instance, index).chaCtrl.updateBustSize = true;
            AgeProGetTargetHeroine(__instance, index).chaCtrl.reSetupDynamicBoneBust = true;
        }

        public static float AgeProGetCurrentFemaleGauge(HFlag __instance)
        {
            return __instance.gaugeFemale;
        }
        public static AnimatorStateInfo AgeProGetAnimatorStateInfo(HFlag __instance, int index)
        {
            return AgeProGetTargetHeroine(__instance, index).chaCtrl.getAnimatorStateInfo(0);
        }

        public static float AgeProInterpolateSize(HFlag __instance, int index)
        {
            if (GrowthConfig.StayatMax.Value == true && (__instance.GetOrgCount() >= GrowthConfig.NumberofOrgasms.Value))
            {
                return GrowthConfig.EndSizes[index].Value;
            }
            return GrowthConfig.StartSizes[index].Value + AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentFemaleGauge(__instance) / 100) * (GrowthConfig.EndSizes[index].Value-GrowthConfig.StartSizes[index].Value);
        }

        public static float AgeProOriginalInterpolateSize(HFlag __instance, bool isFirst, int index)
        {
            if (GrowthConfig.StayatMax.Value == true && (__instance.GetOrgCount() >= GrowthConfig.NumberofOrgasms.Value) && !GrowthConfig.IsOriginalGrowToSetSize.Value)
            {
                if (isFirst)
                {
                    return GrowthConfig.OriginalSizes[index] + GrowthConfig.ChangeSizes[index].Value;
                }
                else
                {
                    return GrowthConfig.OriginalSizes2[index] + GrowthConfig.ChangeSizes[index].Value;
                }

            }
            else if (GrowthConfig.IsOriginalGrowToSetSize.Value)
            {
                if (GrowthConfig.StayatMax.Value == true && (__instance.GetOrgCount() >= GrowthConfig.NumberofOrgasms.Value))
                {
                    if (isFirst)
                    {
                        if (GrowthConfig.OriginalSizes[index] > GrowthConfig.EndSizes[index].Value)
                        {
                            return GrowthConfig.OriginalSizes[index];
                        }
                        return GrowthConfig.EndSizes[index].Value;
                    }
                    else
                    {
                        if (GrowthConfig.OriginalSizes2[index] > GrowthConfig.EndSizes[index].Value)
                        {
                            return GrowthConfig.OriginalSizes2[index];
                        }
                        return GrowthConfig.EndSizes[index].Value;

                    }
                }
                if (isFirst)
                {
                    if((GrowthConfig.OriginalSizes[index]+ AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentFemaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value)) >= GrowthConfig.EndSizes[index].Value)
                    {
                        if (GrowthConfig.OriginalSizes[index] > GrowthConfig.EndSizes[index].Value)
                        {
                            return GrowthConfig.OriginalSizes[index];
                        }
                        return GrowthConfig.EndSizes[index].Value;
                    }
                    return GrowthConfig.OriginalSizes[index] + AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentFemaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value);
                }
                else
                {
                    if ((GrowthConfig.OriginalSizes2[index] + AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentFemaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value)) > GrowthConfig.EndSizes[index].Value)
                    {
                        if(GrowthConfig.OriginalSizes2[index]> GrowthConfig.EndSizes[index].Value)
                        {
                            return GrowthConfig.OriginalSizes2[index];  
                        }
                        return GrowthConfig.EndSizes[index].Value;
                    }
                    return GrowthConfig.OriginalSizes2[index] + AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentFemaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value);
                }
            }
            else
            {
                if (isFirst)
                {
                    return GrowthConfig.OriginalSizes[index] + AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentFemaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value);
                }
                else
                {
                    return GrowthConfig.OriginalSizes2[index] + AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentFemaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value);
                }
            }
        }
        private static int AgeProGetOrgasmCount(HFlag __instance)
        {
            //AgePro.Logger.LogMessage("HFlag Count: " + __instance.count);
            return __instance.GetOrgCount()% GrowthConfig.NumberofOrgasms.Value;
        }

    }
}

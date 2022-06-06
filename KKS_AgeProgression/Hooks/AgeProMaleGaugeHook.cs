using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace KK_AgeProgression
{
    internal static class AgeProMaleGaugeHook
    {
        internal static void ApplyHooks(Harmony instance)
        {
            instance.PatchAll(typeof(AgeProMaleGaugeHook));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(HFlag), "MaleGaugeUp")]
        public static void AgeProMaleGaugeUpPost(HFlag __instance)
        {
            if (GrowthConfig.IsInstant.Value == true && GrowthConfig.AttachedMale.Value == true && GrowthConfig.IsDisabled.Value == false)
            {
                if (OrgasmCounter.CheckCounter(AgeProGetMaleOrgasmCount(__instance)) ==true)
                {
                    UpdateAgePro(__instance);  
                }
            }
            else if (GrowthConfig.AttachedMale.Value == true && GrowthConfig.IsDisabled.Value == false)
            {
                UpdateAgePro(__instance);
            }
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
                UpdateParameters(__instance, 0);

                if (!AgeProHSceneStart.IsSolo)
                {
                    for (int i = 0; i < GrowthConfig.TotalGrowths; i++)
                    {
                        if (GrowthConfig.IsGrowthEnabled[i].Value)
                        {
                            AgeProGetTargetHeroine(__instance, 1).chaCtrl.SetShapeBodyValue(GrowthConfig.GrowthIndexes[i] - 1, AgeProInterpolateSize(__instance, i));
                        }
                        
                    }
                    UpdateParameters(__instance, 1);
                }
            }
        }

        public static void UpdateParameters(HFlag __instance, int index)
        {
            AgeProGetTargetHeroine(__instance, index).chaCtrl.updateShapeBody = true;
            AgeProGetTargetHeroine(__instance, index).chaCtrl.UpdateBustSoftnessAndGravity();
            AgeProGetTargetHeroine(__instance, index).chaCtrl.updateBustSize = true;
            AgeProGetTargetHeroine(__instance, index).chaCtrl.reSetupDynamicBoneBust = true;
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
                    if ((GrowthConfig.OriginalSizes[index]+ AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentMaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value)) > GrowthConfig.EndSizes[index].Value)
                    {
                        if (GrowthConfig.OriginalSizes[index] > GrowthConfig.EndSizes[index].Value)
                        {
                            return GrowthConfig.OriginalSizes[index];
                        }
                        return GrowthConfig.EndSizes[index].Value;
                    }
                    return GrowthConfig.OriginalSizes[index] + AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentMaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value);
                }
                else
                {
                    if ((GrowthConfig.OriginalSizes2[index]+ AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentMaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value)) > GrowthConfig.EndSizes[index].Value)
                    {
                        if (GrowthConfig.OriginalSizes2[index] > GrowthConfig.EndSizes[index].Value)
                        {
                            return GrowthConfig.OriginalSizes2[index];
                        }
                        return GrowthConfig.EndSizes[index].Value;
                    }
                    return GrowthConfig.OriginalSizes2[index] + AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentMaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value);
                }
            }
            else
            {
                if (isFirst)
                {

                    return GrowthConfig.OriginalSizes[index] + AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentMaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value);
                }
                else
                {
                    return GrowthConfig.OriginalSizes2[index] + AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentMaleGauge(__instance) / 100) * (GrowthConfig.ChangeSizes[index].Value);
                }
            }
        }
        public static SaveData.Heroine AgeProGetTargetHeroine(HFlag __instance, int index)
        {
            return __instance.lstHeroine[index];
        }

        public static SaveData.Player AgeProGetPlayer(HFlag __instance)
        {
            return __instance.player;
        }

        public static float AgeProGetCurrentMaleGauge(HFlag __instance)
        {
            return __instance.gaugeMale;
        }

        public static float AgeProInterpolateSize(HFlag __instance, int index)
        {
            if (GrowthConfig.StayatMax.Value == true && AgeProGetMaleOrgasmCount(__instance) >= GrowthConfig.NumberofOrgasms.Value)
            {
                return GrowthConfig.EndSizes[index].Value;
            }
            return GrowthConfig.StartSizes[index].Value + AgeProCalculator.CalculateAgePro(AgeProGetOrgasmCount(__instance), AgeProGetCurrentMaleGauge(__instance) / 100) * (GrowthConfig.EndSizes[index].Value- GrowthConfig.StartSizes[index].Value);
        }

        
        private static int AgeProGetOrgasmCount(HFlag __instance)
        {
            //AgePro.Logger.LogMessage("Splash: " + __instance.count.splash);
            //AgePro.Logger.LogMessage("Kuwae: " + __instance.count.kuwaeFinish);
            //AgePro.Logger.LogMessage("Name: " + __instance.count.nameFinish);
            // AgePro.Logger.LogMessage("houshiInside: " + __instance.count.houshiInside);
            // AgePro.Logger.LogMessage("sonyuInside: " + __instance.count.sonyuInside);
            // AgePro.Logger.LogMessage("sonyuAnalInside: " + __instance.count.sonyuAnalInside);
            //  AgePro.Logger.LogMessage("finish: " + __instance.finish);
            //  AgePro.Logger.LogMessage("houshiinside: " + __instance.count.houshiInside);
            //  AgePro.Logger.LogMessage("houshi outisde: " + __instance.count.houshiOutside);
            //   AgePro.Logger.LogMessage("handfinish: " + __instance.count.handFinish);
            //AgePro.Logger.LogMessage(__instance.count.houshiInside + __instance.count.houshiOutside + __instance.count.sonyuInside + __instance.count.sonyuOutside + __instance.count.sonyuAnalInside + __instance.count.sonyuAnalOutside + __instance.count.sonyuAnalCondomInside + __instance.count.sonyuCondomInside);
            return AgeProGetMaleOrgasmCount(__instance) % GrowthConfig.NumberofOrgasms.Value;
        }

        private static int AgeProGetMaleOrgasmCount(HFlag __instance)
        {
            return __instance.count.houshiInside + __instance.count.houshiOutside + __instance.count.sonyuInside + __instance.count.sonyuOutside + __instance.count.sonyuAnalInside + __instance.count.sonyuAnalOutside + __instance.count.sonyuAnalCondomInside + __instance.count.sonyuCondomInside;
        }

    }
}

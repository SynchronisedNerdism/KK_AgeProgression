using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace KK_AgeProgression
{
    [BepInPlugin(GUID, "Age Progression", Version)]
    [BepInProcess(GameProcessName)]
    [BepInProcess(GameProcessNameSteam)]
    [BepInProcess(GameProcessNameVR)]
    [BepInProcess(GameProcessNameVRSteam)]

    public class AgePro : BaseUnityPlugin   
    {
        private const string GUID = "KK_AgeProgression";
        private const string Version = "1.0";
        private const string GameProcessName = "Koikatu";
        private const string GameProcessNameSteam = "Koikatsu Party";
        private const string GameProcessNameVR = "KoikatuKoikatuVR";
        private const string GameProcessNameVRSteam = "KoikatuKoikatuVRSteam";

        internal static AgePro Instance;
        internal static new ManualLogSource Logger;


        private void Awake()
        {
            try
            {
                var i = new Harmony(GUID);
                Instance = this;
                Logger = base.Logger;
                AgeProFemaleGaugeHook.ApplyHooks(i);
                AgeProMaleGaugeHook.ApplyHooks(i);
                AgeProMasturbationHook.ApplyHooks(i);
                AgeProHSceneEnd.ApplyHooks(i);
                AgeProHSceneStart.ApplyHooks(i);
                GrowthConfig.BindConfig(Config);
                    
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                Logger.LogError("AgePro Mod has trouble hooking onto Koikatsu");
            }
            
        }        
}

}

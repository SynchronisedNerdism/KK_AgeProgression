using System;
using BepInEx.Configuration;

namespace KK_AgeProgression
{
    internal static class GrowthConfig
    {
        internal const int TotalGrowths = 22;

        //structure of arrays
        internal static int[] GrowthIndexes = new int[TotalGrowths]{1,5,12,13,14,23,25,26,27,29,30,31,32,33,34,35,39,40,41,42,43,44};
        internal static float[] DefEndSizes = new float[TotalGrowths] {1f,0.7f,1.0f,1f,0.7f,0.5f,0.5f,0.5f,0.8f,0.7f,0.5f,0.5f,0.4f,0.5f,0.6f,0.4f,0.5f,0.5f,0.6f,0.5f,0.5f,0.5f};
        internal static string[] GrowthNames = new string[TotalGrowths] {"Height","Breast Size","Areola Bulge","Nipple Thickness","Nipple Length", "Upper Waist Width","Lower Waist Width","Lower Waist Thickness","Butt","Top Thigh Width","Top Thigh Thickness","Lower Thigh Width","Lower Thigh Thickness","Lower Knee Width","Lower Knee Thickness","Calf","Shoulder Thickness","Upper Arm Width","Upper Arm Thickness","Elbow Width","Elboww Thickness","Forearm"};
        internal static ConfigEntry<float>[] StartSizes = new ConfigEntry<float>[TotalGrowths];
        internal static ConfigEntry<float>[] EndSizes = new ConfigEntry<float>[TotalGrowths];
        internal static ConfigEntry<float>[] ChangeSizes = new ConfigEntry<float>[TotalGrowths]; //SetShapeFaceValue
        internal static ConfigEntry<bool>[] IsGrowthEnabled = new ConfigEntry<bool>[TotalGrowths];
        internal static float[] OriginalSizes = new float[TotalGrowths] { -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f };
        internal static float[] OriginalSizes2 = new float[TotalGrowths] { -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f };

        internal static ConfigEntry<bool> AttachedMale;
        internal static ConfigEntry<InterpolationType> SetInterpolationType;
        internal static ConfigEntry<int> NumberofOrgasms;
        internal static ConfigEntry<bool> StayatMax;
        internal static ConfigEntry<bool> IsInstant;
        internal static ConfigEntry<bool> IsDisabled;
        internal static ConfigEntry<bool> IsOriginalSize;
        internal static ConfigEntry<bool> IsOriginalGrowToSetSize;


        /*
         for(int i = 0; i < GrowthConfig.TotalGrowths; i++)
            {

            }
        */
        static readonly String[] headerText = new String[] { "General Settings", "Set Your Own Size", "Set Original Size" };
           
        internal static void BindConfig(BepInEx.Configuration.ConfigFile Config)
        {
            for(int i = 0; i < TotalGrowths; i++)
            {
                IsGrowthEnabled[i] = Config.Bind(headerText[0], "is Growth of " + GrowthNames[i] +  " Enabled", true, "Adjust Start Size of " + GrowthNames[i] + "(0 is normal starting value)");
                StartSizes[i] = Config.Bind(headerText[2], "Start Size of "+GrowthNames[i], 0.0f, "Adjust Start Size of "+GrowthNames[i] + "(0 is normal starting value)");
                EndSizes[i] = Config.Bind(headerText[2], "End Size of " + GrowthNames[i], DefEndSizes[i], "Adjust End Size of AgePro" + GrowthNames[i]);
                ChangeSizes[i] = Config.Bind(headerText[1], "Change Size of " + GrowthNames[i], DefEndSizes[i], "Adjust End Size of " + GrowthNames[i] + "(default values assume values start from 0)");
            }
            NumberofOrgasms = Config.Bind(headerText[0], "Orgasms to max size", 1, "Number of orgasms till bust reaches max size");
            AttachedMale = Config.Bind(headerText[0], "Is Attached to Male Gauge", false, "Bust size will be based on male gauge rather than female gauge");
            StayatMax = Config.Bind(headerText[0], "Stay at max size", false, "Whether heroine will stay at max bust size once reached");
            IsInstant = Config.Bind(headerText[0], "AgePro is instant", false, "Whether AgePro will be instant when orgasming");
            IsDisabled = Config.Bind(headerText[0], "Disable AgePro", false, "Disable the AgePro mod");
            IsOriginalSize = Config.Bind(headerText[1], "Is at Original Size", false, "Whether AgePro will start at original size(set Endsize to be larger than this size)");
            SetInterpolationType = Config.Bind(headerText[0], "Scaling Type", InterpolationType.Linear, "Choose the type of bust scaling\nLinear will\nBezier will\nLogarithmic will\nExponenial Will");
            IsOriginalGrowToSetSize = Config.Bind(headerText[2], "Grow to a Set Size", false, "Grow Character to a Set Size from Original Size");
        }

        public enum InterpolationType
        {
            Linear,
            Quadratic,
            Logarithmic,
            Exponential
        }
    }
}

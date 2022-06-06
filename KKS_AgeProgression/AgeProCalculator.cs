using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KK_AgeProgression
{
    internal static class AgeProCalculator
    {
        private static float curPercentage;
        /// <summary>
        /// Turns orgasm count and gauge percentage into one percentage value and interpolates it
        /// </summary>
        /// <param name="orgCount"></param>
        /// <param name="gaugePercentage"></param>
        /// <returns>interpolated AgePro percentage change </returns>
        internal static float CalculateAgePro(int orgCount,float  gaugePercentage)
        {
            if ((orgCount + gaugePercentage)> GrowthConfig.NumberofOrgasms.Value)
            {
                curPercentage = 1f;
            }
            else
            {
                curPercentage = (orgCount + gaugePercentage) / GrowthConfig.NumberofOrgasms.Value;
            }
            return (float) Interpolation.AgeProInterpolation(curPercentage);
        }
    }
}

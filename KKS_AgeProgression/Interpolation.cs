using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KK_AgeProgression
{
    internal static class Interpolation
    {
        /// <summary>
        /// Varies the rate of change of AgePro
        /// </summary>
        /// <param name="completion">Percentage completion of AgePro from 0 to 1</param>
        /// <returns>Modified AgePro completion percentage from 0 to 1</returns>
        internal static double AgeProInterpolation(float completion)
        {
            switch (GrowthConfig.SetInterpolationType.Value) {
                case GrowthConfig.InterpolationType.Linear:
                    return MathUtils.LinearInterpolation(completion);
                case GrowthConfig.InterpolationType.Logarithmic:
                    return MathUtils.LogInterpolation(completion);
                case GrowthConfig.InterpolationType.Quadratic:
                    return MathUtils.QuadraticInterpolation(completion);
                case GrowthConfig.InterpolationType.Exponential:
                    return MathUtils.ExponentialInterpolation(completion);
                default:
                    return MathUtils.LinearInterpolation(completion);
            }

        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace FeatureFlagsSample
{
    [FilterAlias("AnYunCustom")]
    public class CustomFeatureFilter : IFeatureFilter
    {
        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var isEnabled = false;
            var para1 = context.Parameters.GetValue<string>("Para1");

            if (para1 == "AnYun")
                isEnabled = true;

            return Task.FromResult(isEnabled);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureFlagsSample.Controllers
{
    [FeatureGate(MyFeatureFlags.NewFeature)]
    public class NewFeatureController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureFlagsSample.Controllers
{
    [FeatureGate(MyFeatureFlags.CustomFeature)]
    public class CustomFeatureController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

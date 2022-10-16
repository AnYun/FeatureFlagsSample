using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureFlagsSample.Controllers
{
    [FeatureGate(MyFeatureFlags.VIP)]
    public class VIPController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

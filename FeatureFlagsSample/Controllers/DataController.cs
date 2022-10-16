using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlagsSample.Controllers
{
    public class DataController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataController(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            AddRole("User");
            AddRole("Vip");

            var user = AddUser("user@sample.com");
            _ = _userManager.AddToRoleAsync(user, "User").Result;

            var vipUser = AddUser("vip@sample.com");
            _ = _userManager.AddToRoleAsync(vipUser, "User").Result;
            _ = _userManager.AddToRoleAsync(vipUser, "Vip").Result;

            return Ok();
        }
        /// <summary>
        /// Add Role
        /// </summary>
        /// <param name="name"></param>
        private void AddRole(string name)
        {
            var defaultrole = _roleManager.FindByNameAsync(name).Result;

            if (defaultrole == null)
            {
                _ = _roleManager.CreateAsync(new IdentityRole(name)).Result;
            }
        }
        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private IdentityUser AddUser(string name)
        {
            var user = _userManager.FindByNameAsync(name).Result;


            if (user == null)
            {
                user = new IdentityUser(name);
                _=_userManager.CreateAsync(user, "1qaz@WSX").Result;
            }

            return user;
        }
    }
}

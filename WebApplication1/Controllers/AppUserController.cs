using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Areas.Identity.Data;

namespace WebApplication1.Controllers
{
    public class AppUserController : Controller
    {
        UserManager<ApplicationUser> _userManager;

        public AppUserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
    }
}

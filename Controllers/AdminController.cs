using EpicBookstoreSprint.Models;
using EpicBookstoreSprint.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EpicBookstoreSprint.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<DefaultUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<DefaultUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult ListAllRoles()
        {
            var roles = _roleManager.Roles.ToList(); // Retrieve all roles

            return View(roles);
        }
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new()
                {
                    Name = model.RoleName
                };
                var result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction(actionName: "ListAllRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(key: "", error.Description);

                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"No Role with Id {id} Was found";
                return View("Error");

            }

            EditRoleViewModel model = new()
            {
                Id = id,
                RoleName = role.Name,

            };
            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"No Role with Id {model.Id} Was found";
                return View("Error");

            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListAllRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

        }


        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);

            ViewData["roleId"] = id;
            ViewData["roleName"] = role.Name;

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"No Role with Id {id} Was found";
                return View("Error");
            }

            var model = new List<UserRoleViewModel>();
            foreach (var user in _userManager.Users)
            {
                UserRoleViewModel userRoleVm = new UserRoleViewModel
                {
                    Id = user.Id,
                    Name = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleVm.IsSelected = true;
                }
                else
                {
                    userRoleVm.IsSelected = false;
                }

                model.Add(userRoleVm);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model,string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"No Role with Id {id} Was found";
                return View("Error");
            }


            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].Id);
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    await _userManager.AddToRoleAsync(user,role.Name);

                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user,role.Name))
                {
                    await _userManager.RemoveFromRoleAsync(user,role.Name);
                }
                else
                {
                    continue;
                }
                
            }
            return RedirectToAction(actionName: "EditRole", new { Id = id });
        }
    }
}

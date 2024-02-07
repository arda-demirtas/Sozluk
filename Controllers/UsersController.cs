using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sozluk.Data.Abstract.User;
using Sozluk.Models;
using System.Security.Claims;

namespace Sozluk.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;

        public UsersController(IUserWriteRepository userWriteRepository, IUserReadRepository userReadRepository)
        {
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userReadRepository.GetSingleAsync(x => x.Email == model.Email || x.NickName == model.NickName);
                if (user == null)
                {
                    await _userWriteRepository.AddAsync(
                        new Entities.User
                        {
                            NickName = model.NickName,
                            Email = model.Email,
                            Password = model.Password,
                            CreationTime = DateTime.Now,
                        }
                    );
                    await _userWriteRepository.SaveAsync();
                }
                return RedirectToAction("Login");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userReadRepository.GetSingleAsync(x=>x.Email == model.Email);
                if(user == null)
                {
                    return RedirectToAction("Login");
                }
                if(user.Password != model.Password)
                {
                    return RedirectToAction("Login");
                }
                var userClaims = new List<Claim>();
                userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                userClaims.Add(new Claim(ClaimTypes.Name, user.NickName ?? ""));
                if(user.Email == "arda.demirtas2002@gmail.com")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return Redirect("/Titles/index/1");
            }
            else
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Profile(string nickname)
        {
            var user = await _userReadRepository.GetAll().Include(x => x.Titles).Include(x => x.Entries).ThenInclude(x => x.Title).FirstOrDefaultAsync(x=>x.NickName == nickname);
            user!.Entries = user.Entries.OrderByDescending(x => x.CreationTime).ToList();
            user.Titles = user.Titles.OrderByDescending(x=>x.CreationTime).ToList();
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userReadRepository.GetByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            if(User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                _userWriteRepository.Delete(user);
                await _userWriteRepository.SaveAsync();
            }
            return Redirect("/titles/index/1");
        }
    }
}

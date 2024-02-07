using Microsoft.AspNetCore.Mvc;
using Sozluk.Data.Abstract.User;
using Sozluk.Entities;

namespace Sozluk.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUserReadRepository _userReadRepository;
        public HomeController(IUserWriteRepository userWriteRepository, IUserReadRepository userReadRepository)
        {
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
        }
        public IActionResult Index()
        {
            return Redirect("/titles/index/1");
        }
    }
}

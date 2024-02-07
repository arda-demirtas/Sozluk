using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sozluk.Data.Abstract.Entry;
using Sozluk.Data.Abstract.Title;
using Sozluk.Data.Concrete.TitleF;
using Sozluk.Entities;
using Sozluk.Models;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;

namespace Sozluk.Controllers
{
    public class TitlesController : Controller
    {
        private readonly ITitleReadRepository _titleReadRepository;
        private readonly ITitleWriteRepository _titleWriteRepository;
        private readonly IEntryReadRepository _entryReadRepository;
        private readonly IEntryWriteRepository _entryWriteRepository;

        private const int TitlePerPage = 15;
        private const int EntryPerPage = 15;
        public TitlesController(ITitleReadRepository titleReadRepository, ITitleWriteRepository titleWriteRepository, IEntryWriteRepository entryWriteRepository, IEntryReadRepository entryReadRepository)
        {
            _titleReadRepository = titleReadRepository;
            _titleWriteRepository = titleWriteRepository;
            _entryWriteRepository = entryWriteRepository;
            _entryReadRepository = entryReadRepository;
        }
        public IActionResult Index(string page)
        {
            ViewBag.title = "Titles";
            int Page = 1;
            if(page != null)
            {
                Page = int.Parse(page);
            }
            var models = _titleReadRepository.GetAll().OrderByDescending(x=>x.CreationTime).ToList();
            ViewBag.MaxPage = models.Count() / TitlePerPage + 1;
            ViewBag.Page = Page;
            models = models.Skip((Page - 1) * TitlePerPage).Take(TitlePerPage).ToList();
            return View(models);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.title = "Create a title";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TitleCreateViewModel model)
        {
            ViewBag.title = "Create a title";
            if (ModelState.IsValid) 
            {
                var title = await _titleReadRepository.GetSingleAsync(x => x.Text == model.Text);
                if(title != null)
                {
                    title.CreationTime = DateTime.Now;
                    _titleWriteRepository.Update(title);
                }
                else
                {
                    await _titleWriteRepository.AddAsync
                    (
                        new Title
                        {
                            Text = model.Text,
                            Slug = model.Text!.Replace(" ", "-"),
                            CreationTime = DateTime.Now,
                            UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                            Like = 0,
                            Dislike = 0
                        }
                    );
                }
                await _titleWriteRepository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(); 
        }
        [HttpGet]
        public async Task<IActionResult> Title(string slug, int page) 
        {
            ViewBag.title = slug;
            if(page < 1)
            {
                page = 1;
            }
            var title = await _titleReadRepository.GetAll().Include(x=>x.User).Include(x=>x.Entries).ThenInclude(x=>x.User).FirstOrDefaultAsync(x=>x.Slug == slug);
       
            if (title == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.EntryCount = title.Entries.Count();
                ViewBag.MaxPage = title.Entries.Count() / EntryPerPage + 1;
                title.Entries = title.Entries.Skip((page - 1) * EntryPerPage).Take(EntryPerPage).ToList();
                ViewBag.Page = page;
                return View(title);
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> AddEntry(int TitleId, string Text, string Slug)
        {
            var entry = new Entry { UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!), TitleId = TitleId, Text = Text, Like = 0, Dislike = 0, CreationTime = DateTime.Now };
            await _entryWriteRepository.AddAsync(entry);
            await _entryWriteRepository.SaveAsync();
            return Redirect("/Title/" + Slug + "/" + 1);
        }
        [HttpGet]
        public IActionResult Search(string search)
        {
            search = search.Replace(" ", "-").ToLower();
            var title = _titleReadRepository.GetWhere(x=> EF.Functions.Like(x.Slug!.ToLower(), search)).ToList();
            if(title == null)
            {
                return NotFound();
            }
            else
            {
                return View("Index", title);
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveEntry(int id)
        {
            var entry = await _entryReadRepository.GetAll().Include(x=>x.Title).FirstOrDefaultAsync(x=>x.Id == id);
            if(entry == null)
            {
                return NotFound();
            }
            if(entry.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!) || User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                _entryWriteRepository.Delete(entry);
                await _entryWriteRepository.SaveAsync();
                return Redirect("/title/" + entry.Title.Slug + "/1");
            }
            return NotFound();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveTitle(int id)
        {
            var title = await _titleReadRepository.GetByIdAsync(id);
            if(title == null)
            {
                return NotFound();
            }
            if(title.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!) || User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                _titleWriteRepository.Delete(title);
                await _titleWriteRepository.SaveAsync();
                return Redirect("/titles/index/1");
            }
            return NotFound();
        }
    }
}

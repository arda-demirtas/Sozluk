using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sozluk.Data.Abstract.Title;
using Sozluk.Data.Concrete.TitleF;

namespace Sozluk.ViewComponents
{
    public class NewTitles : ViewComponent
    {
        private readonly ITitleReadRepository _titleReadRepository;
        public NewTitles(ITitleReadRepository titleReadRepository)
        {
            _titleReadRepository = titleReadRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var latestTitles = await _titleReadRepository.GetAll().OrderByDescending(x=>x.CreationTime).Take(5).ToListAsync();
            return View(latestTitles);
        }
    }
}

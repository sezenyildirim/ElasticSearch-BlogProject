using ElastichSearch_BlogProject.Models;
using ElastichSearch_BlogProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElastichSearch_BlogProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;
        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var blogList = await _blogService.ListAsync();
                return View(blogList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            try
            {
                var blogList = await _blogService.SearchAsync(searchText);
                ViewBag.searchText = searchText;
                return View(blogList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Categories(string category)
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            try
            {
                var result = await _blogService.GetByIdAsync(id);
                return View(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchModal(string searchText)
        {
            try
            {
                var results = await _blogService.SearchAsync(searchText);
                return Json(results);
            }
            catch (Exception)
            {
                return Json(new List<Flowers>());
            }
        }
    }
}

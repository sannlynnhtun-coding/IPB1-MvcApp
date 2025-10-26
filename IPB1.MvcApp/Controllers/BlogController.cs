using IPB1.MvcApp.Database.AppDbContextModels;
using IPB1.MvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IPB1.MvcApp.Controllers
{
    // mvc
    // controller > view + model
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;

        public BlogController()
        {
            _db = new AppDbContext();
        }

        public async Task<IActionResult> IndexAsync()
        {
            var lst = await _db.TblBlogs.ToListAsync();
            return View("Index", lst);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateRequestModel request)
        {
            await _db.TblBlogs.AddAsync(new TblBlog
            {
                BlogAuthor = request.BlogAuthor,
                BlogContent = request.BlogContent,
                BlogTitle = request.BlogTitle,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
                IsDelete = false
            });
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
            //return Redirect("/Blog/Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == Convert.ToInt32(id));
            if (item is null)
            {
                return RedirectToAction("Index");
            }

            BlogEditRequestModel request = new BlogEditRequestModel
            {
                BlogId = item.BlogId,
                BlogAuthor = item.BlogAuthor,
                BlogContent = item.BlogContent,
                BlogTitle = item.BlogTitle
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, BlogEditRequestModel request)
        {
            var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == Convert.ToInt32(id));
            if (item is null)
            {
                return RedirectToAction("Index");
            }

            item.BlogTitle = request.BlogTitle;
            item.BlogContent = request.BlogContent;
            item.BlogAuthor = request.BlogAuthor;
            item.ModifiedBy = "System";
            item.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == Convert.ToInt32(id));
            if (item is null)
            {
                return RedirectToAction("Index");
            }

            item.IsDelete = true;

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

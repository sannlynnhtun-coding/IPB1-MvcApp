using IPB1.MvcApp.Database.AppDbContextModels;
using IPB1.MvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPB1.MvcApp.Controllers
{
    public class BlogAjaxController : Controller
    {
        private readonly AppDbContext _db;

        public BlogAjaxController()
        {
            _db = new AppDbContext();
        }

        public async Task<IActionResult> Index()
        {
            var lst = await _db.TblBlogs
                .Where(x => x.IsDelete == false)
                .ToListAsync();
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
            var result = await _db.SaveChangesAsync();

            return Ok(new BlogCreateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Blog saved successfully!" : "Error saving blog."
            });
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
        public async Task<IActionResult> Update(BlogEditRequestModel request)
        {
            var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == request.BlogId);
            if (item is null)
            {
                return Ok(new BlogCreateResponseModel
                {
                    IsSuccess = false,
                    Message = "Blog not found."
                });
            }

            item.BlogTitle = request.BlogTitle;
            item.BlogContent = request.BlogContent;
            item.BlogAuthor = request.BlogAuthor;
            item.ModifiedBy = "System";
            item.ModifiedDate = DateTime.Now;

            var result = await _db.SaveChangesAsync();

            return Ok(new BlogCreateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Blog updated successfully!" : "Error updating blog."
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BlogDeleteRequestModel request)
        {
            var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == request.BlogId);
            if (item is null)
            {
                return Ok(new BlogDeleteResponseModel
                {
                    IsSuccess = false,
                    Message = "Blog not found."
                });
            }

            item.IsDelete = true;

            var result = await _db.SaveChangesAsync();
            return Ok(new BlogDeleteResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Blog deleted successfully!" : "Error deleting blog."
            });
        }
    }
}

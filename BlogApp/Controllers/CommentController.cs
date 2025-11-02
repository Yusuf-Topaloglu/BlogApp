using BlogApp.DataContext;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class CommentController : Controller
    {
        BlogContext _blogContext;
        public CommentController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }
        public async Task<IActionResult> Index(int postId)
        {
            var comment = await _blogContext.Comments.
            Where(p => p.PostId == postId).ToListAsync();

            ViewBag.PostId = postId;
            return View(comment);
        }

        [HttpGet]
        public IActionResult Create(int postId)
        {
            var comment =new Comment { PostId = postId };
            return View(comment); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                _blogContext.Comments.Add(comment);
                await _blogContext.SaveChangesAsync();
                return RedirectToAction("Details","Post",new {id=comment.PostId});
            }
            return View(comment);
        }
    }
}




using BlogApp.Data;
using BlogApp.DataContext;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly IRepository<Comment> _commentRepository;  
        private readonly BlogContext _blogContext;

        public CommentController(IRepository<Comment> repository, BlogContext blogContext)
        {
            _blogContext = blogContext;
            _commentRepository = repository;
        }

        public async Task<IActionResult> Index(int postId)
        {
            
            var comments = (await _commentRepository.GetAllAsync())
                .Where(c => c.PostId == postId)
                .ToList();

            ViewBag.PostId = postId;
            return View(comments);
        }

        [HttpGet]
        public IActionResult Create(int postId)
        {
            var comment = new Comment { PostId = postId };
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                
                await _commentRepository.AddAsync(comment);
                await _commentRepository.SaveAsync();
                return RedirectToAction("Details", "Post", new { id = comment.PostId });
            }
            return View(comment);
        }
    }
}
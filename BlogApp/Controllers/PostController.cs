using BlogApp.Data;
using BlogApp.DataContext;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IRepository<Post> _postRepository;  
        private readonly BlogContext _blogContext;           

        public PostController(IRepository<Post> postRepository, BlogContext blogContext)
        {
            _postRepository = postRepository;
            _blogContext = blogContext;  
        }

        public async Task<IActionResult> Index()
        {
            
            var posts = await _blogContext.Posts
                .Include(p => p.Comments)
                .ToListAsync();

            return View(posts);
        }

        public async Task<IActionResult> Details(int id)
        {
            
            var post = await _blogContext.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                post.CreateDate = DateTime.Now;

                
                await _postRepository.AddAsync(post);
                await _postRepository.SaveAsync();  // ⬅️ Repository üzerinden save

                return RedirectToAction("Index");
            }
            return View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {
            
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                
                var existingPost = await _postRepository.GetByIdAsync(id);
                if (existingPost == null)
                    return NotFound();

                existingPost.Title = post.Title;
                existingPost.Content = post.Content;

                _postRepository.Update(existingPost);  
                await _postRepository.SaveAsync();

                return RedirectToAction("Index");
            }
            return View(post);
        }

        public async Task<IActionResult> Delete(int id)
        {
            
            var post = await _blogContext.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null) return NotFound();

            _postRepository.Delete(post);  
            await _postRepository.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
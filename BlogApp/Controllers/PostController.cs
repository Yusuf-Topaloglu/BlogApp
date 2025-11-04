using BlogApp.DataContext;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        BlogContext _blogContext;

        public PostController(BlogContext blogContext)
        {
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
                await _blogContext.Posts.AddAsync(post);
                await _blogContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post =await _blogContext.Posts.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,CreateDate")] Post post)
        
        {
            if (id != post.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var existingPost = await _blogContext.Posts.FindAsync(id);
                if (existingPost == null)
                    return NotFound();

                existingPost.Title = post.Title;
                existingPost.Content = post.Content;
                await _blogContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Delete (View gösterir)
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _blogContext.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();
            return View(post);
        }

        // POST: DeleteConfirmed (İşlem yapar)
        [HttpPost, ActionName("Delete")]  // ← BU KRİTİK!
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _blogContext.Posts.FindAsync(id);
            if (post == null) return NotFound();

            _blogContext.Posts.Remove(post);
            await _blogContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

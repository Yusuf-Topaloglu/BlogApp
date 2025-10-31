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
            var post = await _blogContext.Posts.ToListAsync();
            return View(post);
        }


        public async Task<IActionResult> Details(int id)
        {
            var post = await _blogContext.Posts.FindAsync(id);
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
            await _blogContext.Posts.AddAsync(post);
            await _blogContext.SaveChangesAsync();
            return RedirectToAction("Index");

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
        public async Task<IActionResult> Edit(int id,Post post)
        {
            if (id!=post.Id) 
            return BadRequest();

            if(ModelState.IsValid)
            {
                var existingPost = await _blogContext.Posts.FindAsync(id);
                if(existingPost==null)
                    return NotFound();
                existingPost.Title= post.Title;
                existingPost.Content= post.Content;

                await _blogContext.SaveChangesAsync();
                return RedirectToAction("Index", "Post");

            }
            return View(post);
            
        }

        public async Task<IActionResult> Delete (int id)
        {
            var post = await _blogContext.Posts.FindAsync(id);
            if(id<=0)
                return NotFound();
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _blogContext.Posts.FindAsync(id);
            _blogContext.Posts.Remove(post);
            await _blogContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }
    }
}

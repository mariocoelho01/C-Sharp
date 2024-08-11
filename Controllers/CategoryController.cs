using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices]IMemoryCache cache,
            [FromServices] BlogDataContext context)
        {
            User.IsInRole("admin");
            try
            {
                var categories = cache.GetOrCreate("CategoriesCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategories(context);
                });

                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("05X08 - Internal server failure"));
            }
        }

        private List<Category> GetCategories(BlogDataContext context)
        {
            return context.Categories.ToList();
        }
        
        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            try
            {
                var category = await context
                    .Categories
                    .FirstOrDefaultAsync(x=>x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Content not found"));
            
                return Ok(new ResultViewModel<Category>(category));
            }
            catch (Exception e)
            {
                return StatusCode(500,new ResultViewModel<Category>( "05X09 - Internal server failure"));
            }
        }
        
        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug.ToLower(),
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
          
            {
                return StatusCode(500,   new ResultViewModel<Category>("05XD1 - it was not possible to include the category"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("05X10 - Internal server failure"));
            }
        }
        
        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Content not found"));

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XD2 - it was not possible to changed the category"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05X11 - Internal server failure"));
            }
        }
        
        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            try
            {
                var category = context.Categories.FirstOrDefault(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Content not found"));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XD3 - it was not possible to deleted the category"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResultViewModel<Category>("05X12 - Internal server failure"));
            }
        }
      
    }
}
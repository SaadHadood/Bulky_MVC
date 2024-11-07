using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories;

[BindProperties]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public CategoryModel Category { get; set; }

    public DeleteModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public IActionResult OnGet(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Category = _db.Categories.FirstOrDefault(c => c.Id == id)!;

        if (Category == null)
        {
            return NotFound();
        }

        return Page();
    }

    public IActionResult OnPost(int? id)
    {
        CategoryModel? obj = _db.Categories.FirstOrDefault(c => c.Id == id);
        if (obj == null) { return NotFound(); }

        _db.Categories.Remove(obj);
        _db.SaveChanges();
        TempData["success"] = "Category Deleted successfully";

        return RedirectToPage("Index");
    }
}

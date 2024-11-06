using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories;

[BindProperties]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public CategoryModel Category { get; set; }
    public EditModel(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult OnGet(int? id)
    {
        if (id == null || id == 0) { return NotFound(); }
        Category = _db.Categories.FirstOrDefault(c => c.Id == id)!;

        if (Category == null) { return NotFound(); }

        return Page();
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            _db.Categories.Update(Category);
            _db.SaveChanges();
            TempData["success"] = "Category Edit successfully";
            return RedirectToPage("Index");
        }
        return RedirectToPage("Index");
    }


}

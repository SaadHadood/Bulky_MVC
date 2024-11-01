using BulkyWeb.Context;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
namespace BulkyWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _db;
    public CategoryController(ApplicationDbContext db)
    {
        _db = db;
    }
    public IActionResult Index()
    {
        List<CategoryModel> objCategoryList = _db.Categories.ToList();
        return View(objCategoryList);
    }

    public IActionResult CreateCategory()
    {
        return View();
    }
    [HttpPost]
    public IActionResult CreateCategory(CategoryModel obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the name");
        }

        if (ModelState.IsValid) 
        {
            _db.Categories.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View();

    }
    [HttpDelete]
    public IActionResult DeleteCategory(int id)
    {

        var category = _db.Categories.FirstOrDefault(c => c.Id == id);
        if (category != null) 
        {
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View();
    }

}

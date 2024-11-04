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

    //Create
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
            TempData["success"] = "Category Created successfully";
            return RedirectToAction("Index");
        }
        return View();

    }

    //Edit
    public IActionResult EditCategory(int? id) 
    {
        if (id == null || id == 0) { return NotFound(); }
        CategoryModel? CategoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);

        if (CategoryFromDb == null) { return NotFound(); }

        return View(CategoryFromDb);
    }

    [HttpPost]
    public IActionResult EditCategory(CategoryModel obj) 
    { 
        if (ModelState.IsValid)
        {
            _db.Categories.Update(obj);
            _db.SaveChanges();
            TempData["success"] = "Category Edit successfully";
            return RedirectToAction("Index");
        }
        return View();
    }


    //Delete
    public IActionResult DeleteCategory(int? id)
    {
        if (id == null || id == 0) { return NotFound(); }
        CategoryModel? CategoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);

        if (CategoryFromDb == null) { return NotFound(); }

        return View(CategoryFromDb);
    }
    [HttpPost, ActionName ("DeleteCategory")]
    public IActionResult DeleteCategoryPost(int? id)
    {
        CategoryModel? obj = _db.Categories.FirstOrDefault(c => c.Id == id);

        if (obj == null) { return NotFound(); }

        _db.Categories.Remove(obj);
        _db.SaveChanges();
        TempData["success"] = "Category Deleted successfully";
        return RedirectToAction("Index");
    }

}

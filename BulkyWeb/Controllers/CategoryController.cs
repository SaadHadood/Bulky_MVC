using Bulky.DataAccess.Context;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
namespace BulkyWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepo;
    public CategoryController(ICategoryRepository db)
    {
        _categoryRepo = db;
    }
    public IActionResult Index()
    {
        List<CategoryModel> objCategoryList = _categoryRepo.GetAll().ToList();
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
            _categoryRepo.Add(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category Created successfully";
            return RedirectToAction("Index");
        }
        return View();

    }

    //Edit
    public IActionResult EditCategory(int? id) 
    {
        if (id == null || id == 0) { return NotFound(); }
        CategoryModel? CategoryFromDb = _categoryRepo.Get(c => c.Id == id);

        if (CategoryFromDb == null) { return NotFound(); }

        return View(CategoryFromDb);
    }

    [HttpPost]
    public IActionResult EditCategory(CategoryModel obj) 
    { 
        if (ModelState.IsValid)
        {
            _categoryRepo.Update(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category Edit successfully";
            return RedirectToAction("Index");
        }
        return View();
    }


    //Delete
    public IActionResult DeleteCategory(int? id)
    {
        if (id == null || id == 0) { return NotFound(); }
        CategoryModel? CategoryFromDb = _categoryRepo.Get(c => c.Id == id);

        if (CategoryFromDb == null) { return NotFound(); }

        return View(CategoryFromDb);
    }
    [HttpPost, ActionName ("DeleteCategory")]
    public IActionResult DeleteCategoryPost(int? id)
    {
        CategoryModel? obj = _categoryRepo.Get(c => c.Id == id);

        if (obj == null) { return NotFound(); }

        _categoryRepo.Remove(obj);
        _categoryRepo.Save();
        TempData["success"] = "Category Deleted successfully";
        return RedirectToAction("Index");
    }

}

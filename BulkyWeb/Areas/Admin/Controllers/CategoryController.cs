using Bulky.DataAccess.Context;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        List<CategoryModel> objCategoryList = _unitOfWork.Category.GetAll().ToList();
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
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category Created successfully";
            return RedirectToAction("Index");
        }
        return View();

    }

    //Edit
    public IActionResult EditCategory(int? id)
    {
        if (id == null || id == 0) { return NotFound(); }
        CategoryModel? CategoryFromDb = _unitOfWork.Category.Get(c => c.Id == id);

        if (CategoryFromDb == null) { return NotFound(); }

        return View(CategoryFromDb);
    }

    [HttpPost]
    public IActionResult EditCategory(CategoryModel obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category Edit successfully";
            return RedirectToAction("Index");
        }
        return View();
    }


    //Delete
    public IActionResult DeleteCategory(int? id)
    {
        if (id == null || id == 0) { return NotFound(); }
        CategoryModel? CategoryFromDb = _unitOfWork.Category.Get(c => c.Id == id);

        if (CategoryFromDb == null) { return NotFound(); }

        return View(CategoryFromDb);
    }
    [HttpPost, ActionName("DeleteCategory")]
    public IActionResult DeleteCategoryPost(int? id)
    {
        CategoryModel? obj = _unitOfWork.Category.Get(c => c.Id == id);

        if (obj == null) { return NotFound(); }

        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Category Deleted successfully";
        return RedirectToAction("Index");
    }

}

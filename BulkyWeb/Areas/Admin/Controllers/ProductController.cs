using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        List<ProductModel> products = _unitOfWork.Product.GetAll().ToList();

        return View(products);
    }

    //Create

    public IActionResult CreateProduct()
    {
        //CategoryList Dropdown
        IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString()
        });

        ViewBag.CategoryList = CategoryList;

        return View();
    }

    [HttpPost]
    public IActionResult CreateProduct(ProductModel obj)
    {
        var existingProduct = _unitOfWork.Product.Get(p => p.Title == obj.Title && p.ISBN == obj.ISBN);
        if (existingProduct != null)
        {
            ModelState.AddModelError("CustomError", "A product with the same Title and ISBN already exists.");
        }

        if (ModelState.IsValid) 
        {
            _unitOfWork.Product.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product Created successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    //Edit
    public IActionResult EditProduct(int? id)
    {
        if(id == null || id == 0) { return NotFound(); }
        ProductModel ProductFromDb = _unitOfWork.Product.Get(x => x.Id == id);
        if (ProductFromDb == null) { return NotFound(); }
        return View(ProductFromDb);
    }

    [HttpPost]
    public IActionResult EditProduct(ProductModel obj) 
    {
        if (ModelState.IsValid) 
        {
            _unitOfWork.Product.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product Edit successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    //Delete
    public IActionResult DeleteProduct(int? id) 
    {
        if (id == null || id == 0) { return NotFound(); }
        ProductModel productFromDb = _unitOfWork.Product.Get(x => x.Id == id);
        if (productFromDb == null) { return NotFound(); }
        
        return View(productFromDb);
    }

    [HttpPost, ActionName("DeleteProduct")]
    public IActionResult DeleteProductPost(int? id)
    {
        ProductModel obj = _unitOfWork.Product.Get(x => x.Id == id);
        if(obj == null) { return NotFound(); }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Product Deleted successfully";
        return RedirectToAction("Index");
    }

}

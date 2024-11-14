using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
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
        ProductVM productVM = new()
        {
            CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            Product = new ProductModel()
        };

        return View(productVM);
    }

    [HttpPost]
    public IActionResult CreateProduct(ProductVM productVM)
    {
        var existingProduct = _unitOfWork.Product.Get(p => p.Title == productVM.Product.Title && p.ISBN == productVM.Product.ISBN);
        if (existingProduct != null)
        {
            ModelState.AddModelError("CustomError", "A product with the same Title and ISBN already exists.");
        }

        if (ModelState.IsValid) 
        {
            _unitOfWork.Product.Add(productVM.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product Created successfully";
            return RedirectToAction("Index");
        }

        else
        {

            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(productVM);
        }
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

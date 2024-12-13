using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]

public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        List<ProductModel> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

        return View(products);
    }

    //Create

    public IActionResult UpsertProduct(int? id)
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

        if (id == null || id == 0)
        {
            //Create
            return View(productVM);
        }
        else 
        {
            //update
            productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
            return View(productVM);
        }

    }

    [HttpPost]
    public IActionResult UpsertProduct(ProductVM productVM, IFormFile? file)
    {

        if (ModelState.IsValid) 
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath; //Var filen ska sparars.
            if (file != null) 
            { 
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); //skapa id och hämta filändelsen.
                string productPath = Path.Combine(wwwRootPath, @"images\product"); //sparas i images/product.

                if (!string.IsNullOrEmpty(productVM.Product.ImageUrl)) 
                {
                    //delete the old image
                    var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath)) 
                    { 
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStram = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStram);
                }

                productVM.Product.ImageUrl = @"\images\product\" + fileName;
            
            }

            if (productVM.Product.Id == 0)
            {
                var existingProduct = _unitOfWork.Product.Get(p => p.Title == productVM.Product.Title && p.ISBN == productVM.Product.ISBN);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("CustomError", "A product with the same Title and ISBN already exists.");
                }
                _unitOfWork.Product.Add(productVM.Product);

            }
            else
            {
                _unitOfWork.Product.Update(productVM.Product);

            }

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



    //Behövs för datatables
    #region API CALLS

    [HttpGet]
    public IActionResult GetAll() 
    {
        List<ProductModel> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
        return Json(new { data = products });
    }

    [HttpDelete]
    public IActionResult DeleteProduct(int? id)
    {
        var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
        if (productToBeDeleted == null) return Json(new{ success = false, message = "Error while deleting" });

        //Delete Image
        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
            productToBeDeleted.ImageUrl!.TrimStart('\\'));

        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }


        _unitOfWork.Product.Remove(productToBeDeleted);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Delete Successful" });
    }


    #endregion

}

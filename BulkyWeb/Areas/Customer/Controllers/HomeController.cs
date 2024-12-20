using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<ProductModel> productList = _unitOfWork.Product.GetAll(includeProperties :"Category");
        return View(productList);
    }

    public IActionResult Details(int productId) 
    {
        ShoppingCartModel cart = new()
        {
            Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
            Count = 1,
            ProductId = productId
        };

        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCartModel shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        shoppingCart.ApplicationUserId = userId;

        _unitOfWork.ShoppingCart.Add(shoppingCart);
        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }
}

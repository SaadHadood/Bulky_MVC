﻿using Bulky.DataAccess.Repository.IRepository;
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

public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        List<CompanyModel> Companys = _unitOfWork.Company.GetAll().ToList();

        return View(Companys);
    }

    //Create

    public IActionResult UpsertCompany(int? id)
    {
        if (id == null || id == 0)
        {
            //Create
            return View(new CompanyModel());
        }
        else
        {
            //update
            CompanyModel companyObj = _unitOfWork.Company.Get(u => u.Id == id);
            return View(companyObj);
        }

    }

    [HttpPost]
    public IActionResult UpsertCompany(CompanyModel CompanyObj)
    {

        if (ModelState.IsValid)
        {
            if (CompanyObj.Id == 0)
            {
                _unitOfWork.Company.Add(CompanyObj);

            }
            else
            {
                _unitOfWork.Company.Update(CompanyObj);

            }

            _unitOfWork.Save();
            TempData["success"] = "Company Created successfully";
            return RedirectToAction("Index");
        }

        else
        {
            return View(CompanyObj);
        }
    }



    //Behövs för datatables
    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        List<CompanyModel> Companys = _unitOfWork.Company.GetAll().ToList();
        return Json(new { data = Companys });
    }

    [HttpDelete]
    public IActionResult DeleteCompany(int? id)
    {
        var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
        if (CompanyToBeDeleted == null) return Json(new { success = false, message = "Error while deleting" });

        _unitOfWork.Company.Remove(CompanyToBeDeleted);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Delete Successful" });
    }


    #endregion

}
using Microsoft.AspNetCore.Mvc;
using MyCompany.Domain;
using MyCompany.Domain.Entities;
using System;

namespace MyCompany.Controllers
{
    public class ServicesController : Controller
    {
        private readonly DataManager _dataManager;



        public ServicesController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }


        public IActionResult Index(Guid id)
        {
            if (id != default)
                return View("Show", _dataManager.ServiceItems.GetById(id));

            // else - show all
            ViewBag.TextField = _dataManager.TextFields.GetByCodeWord("PageServices");
            return View(_dataManager.ServiceItems.GetAll());
        }
    }
}

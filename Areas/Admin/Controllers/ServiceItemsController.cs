using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCompany.Domain;
using MyCompany.Domain.Entities;
using MyCompany.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceItemsController : Controller
    {
        private readonly DataManager _dataManager;
        private readonly IWebHostEnvironment _hostingEnvironment;



        public ServiceItemsController(DataManager dataManager, IWebHostEnvironment hostingEnvironment)
        {
            _dataManager = dataManager;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var entity = (id == default)
                ? new ServiceItem()
                : _dataManager.ServiceItems.GetById(id);

            return View(entity);
        }

        [HttpPost]
        public IActionResult Edit(ServiceItem model, IFormFile titleImageFile)
        {
            if (ModelState.IsValid)
            {
                // Зберігаємо файл на сервері
                if (titleImageFile != null)
                {
                    model.TitleImagePath = $"{Guid.NewGuid()}_{titleImageFile.FileName}";    // Додаємо Guid щоб при додаванні однакових файлів назва файлів на сервері була різна

                    // Зберігаємо файл 
                    using (var destinationStream = new FileStream(Path.Combine(_hostingEnvironment.WebRootPath, Config.PathServiceItemsImages, model.TitleImagePath), FileMode.Create))
                    {
                        titleImageFile.CopyTo(destinationStream);
                    }
                }

                _dataManager.ServiceItems.Save(model);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            _dataManager.ServiceItems.Delete(id);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MyCompany.Domain;
using MyCompany.Domain.Entities;
using MyCompany.Service;


namespace MyCompany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TextFieldsController : Controller
    {
        private readonly DataManager _dataManager;



        public TextFieldsController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }


        [HttpGet]
        public IActionResult Edit(string codeWord)
        {
            var entity = _dataManager.TextFields.GetByCodeWord(codeWord);
            return View(entity);
        }

        [HttpPost]
        public IActionResult Edit(TextField model)
        {
            if (ModelState.IsValid)
            {
                _dataManager.TextFields.Save(model);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
            return View(model);
        }
    }
}

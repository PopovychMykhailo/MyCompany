using Microsoft.AspNetCore.Mvc;
using MyCompany.Domain;
using MyCompany.Domain.Entities;
using System.Threading.Tasks;

namespace MyCompany.Models.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly DataManager _dataManager;



        public SidebarViewComponent(DataManager dataManager)
        {
            _dataManager = dataManager;
        }


        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult((IViewComponentResult) View("Default", _dataManager.ServiceItems.GetAll()));
        }
    }
}

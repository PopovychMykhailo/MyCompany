using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.Service
{
    public class AdminAreaAuthorization : IControllerModelConvention
    {
        private readonly string _area;      // Для якої області створена модель (класи з namespace Areas)
        private readonly string _policy;    // Політика (правило) для ціє області



        public AdminAreaAuthorization(string area, string policy)
        {
            _area = area;
            _policy = policy;
        }


        public void Apply(ControllerModel controller)
        {
            
            if (// Якщо атрибут контролера має область ...
                controller.Attributes.Any(a => 
                        a is AreaAttribute                                                                  // Тип є AreaAttribute
                    && (a as AreaAttribute).RouteValue.Equals(_area, StringComparison.OrdinalIgnoreCase))   // Має значення _area в шляху
                ||
                // Або якщо маршрут контролера має область...
                controller.RouteValues.Any(r => 
                       r.Key.Equals("area", StringComparison.OrdinalIgnoreCase)         // Має слово area в ключі шляху
                    && r.Value.Equals(_area, StringComparison.OrdinalIgnoreCase)))      // Має значення _area в шляху
            {
                // Додаємо фільтр до контролера (відправляємо користувача на авторизацію)
                controller.Filters.Add(new AuthorizeFilter(_policy));
            }
        }
    }
}

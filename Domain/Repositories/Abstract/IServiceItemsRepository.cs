using MyCompany.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.Domain.Repositories.Abstract
{
    // Інтерфейс для роботи з послугами
    public interface IServiceItemsRepository
    {
        public IQueryable<ServiceItem> GetAll();
        public ServiceItem GetById (Guid id);
        public void Save(ServiceItem serviceItem);
        public void Delete(Guid id);
    }
}

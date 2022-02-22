using Microsoft.EntityFrameworkCore;
using MyCompany.Domain.Entities;
using MyCompany.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.Domain.Repositories.EntityFramework
{
    public class EFServiceItemsRepository : IServiceItemsRepository
    {
        private readonly AppDbContext _dbContext;


        public EFServiceItemsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IQueryable<ServiceItem> GetAll()
        {
            return _dbContext.ServiceItems;
        }

        public ServiceItem GetById(Guid id)
        {
            return _dbContext.ServiceItems.FirstOrDefault(i => i.Id == id);
        }

        public void Save(ServiceItem entity)
        {
            if (entity.Id == default)
                _dbContext.Entry(entity).State = EntityState.Added;
            else
                _dbContext.Entry(entity).State = EntityState.Modified;

            _dbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            _dbContext.Remove(new ServiceItem() { Id = id });
            _dbContext.SaveChanges();
        }
    }
}

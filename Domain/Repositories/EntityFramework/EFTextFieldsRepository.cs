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
    public class EFTextFieldsRepository : ITextFieldsRepository
    {
        private readonly AppDbContext _dbContext;


        public EFTextFieldsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IQueryable<TextField> GetAll()
        {
            return _dbContext.TextFields;
        }

        public TextField GetById(Guid id)
        {
            return _dbContext.TextFields.FirstOrDefault(i => i.Id == id);
        }

        public TextField GetByCodeWord(string codeWord)
        {
            return _dbContext.TextFields.FirstOrDefault(i => i.CodeWord == codeWord);
        }

        public void Save(TextField entity)
        {
            if (entity.Id == default)
                _dbContext.Entry(entity).State = EntityState.Added;
            else
                _dbContext.Entry(entity).State = EntityState.Modified;

            _dbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            _dbContext.TextFields.Remove(new TextField() { Id = id });
            _dbContext.SaveChanges();
        }
    }
}

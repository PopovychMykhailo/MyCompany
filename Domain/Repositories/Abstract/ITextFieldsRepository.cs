using MyCompany.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.Domain.Repositories.Abstract
{
    // Інтерфейс для роботи з текстовими полями
    public interface ITextFieldsRepository
    {
        public IQueryable<TextField> GetAll();
        public TextField GetById(Guid id);
        public TextField GetByCodeWord(string codeWord);
        public void Save(TextField textField);
        public void Delete(Guid id);
    }
}

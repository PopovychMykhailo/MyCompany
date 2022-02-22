using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.Domain.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase() => DateAdded = DateTime.UtcNow;



        [Required]
        public Guid Id { get; set; }

        [Display(Name = "Назва (заголовку)")]
        public virtual string Title { get; set; }

        [Display(Name = "Короткий опис")]
        public virtual string Subtitle { get; set; }

        [Display(Name = "Повний опис")]
        public virtual string Description { get; set; }

        [Display(Name = "Титульна картинка")]
        public virtual string TitleImagePath { get; set; }

        [Display(Name = "SEO метатег Title")]
        public virtual string MetaTitle { get; set; }

        [Display(Name = "SEO метатег Description")]
        public virtual string MetaDescription { get; set; }

        [Display(Name = "SEO метатег Keywords")]
        public virtual string MetaKeywords { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; }
    }
}

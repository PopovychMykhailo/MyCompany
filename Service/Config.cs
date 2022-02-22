using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.Service
{
    public class Config
    {
        public static string ConnectionString { get; set; }
        
        public static string CompanyName { get; set; }
        public static string CompanyPhone { get; set; }
        public static string CompanyPhoneShort { get; set; }
        public static string CompanyEmail { get; set; }

        public static string PathImages { get; set; }               // Коренева директорія для image файлів
        public static string PathServiceItemsImages { get; set; }   // Директорія для image файлів, послуг компанії
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCompany.Service;
using MyCompany.Domain.Repositories.Abstract;
using MyCompany.Domain.Repositories.EntityFramework;
using MyCompany.Domain.Repositories;
using MyCompany.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MyCompany
{
    public class Startup
    {
        public IConfiguration Configuration { get; }



        public Startup(IConfiguration configuration)  =>  Configuration = configuration;
        

        public void ConfigureServices(IServiceCollection services)
        {
            // Отримуємо налаштування з файлу appsettings.json
            Configuration.Bind("Project", new Config());

            // Підключаємо необіхідний функціонал як сервіси
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
            services.AddTransient<DataManager>();

            // Підключаємо БД
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Config.ConnectionString));

            // Налаштування identity систему (юзерів)
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // Налаштування authentication cookie
            services.ConfigureApplicationCookie(option =>
            {
                option.Cookie.Name = "myCompanyAuth";               // Ім'я cookie файлу
                option.Cookie.HttpOnly = true;                      // ?
                option.LoginPath = "/account/login";                // Куди перенаправити юзера для авторизації
                option.AccessDeniedPath = "/account/accsessdenied"; // Шлях при відмові доступу
                option.SlidingExpiration = true;                    // Давати новий cookie коли він закінчується
            });

            // Створення політик (правил) авторизації
            services.AddAuthorization(options =>
            {
                // Назва правила: AdminArea;
                // Правило:       Необхідні права юзера admin
                options.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
            });

            // Додаємо підтримку контролерів і представлень (MVC)
            services.AddControllersWithViews(conf =>
            {
                // Назначаємо що: контролери в області Admin, мають політику (правила доступу) AdminArea  (тобто контролери з атрибутом [Area("Admin")] )
                conf.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea")); 
            })
                // Налаштовуємо сумісність з ASP.NET Core 3.0
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Під час розробки додатку:    бачити сторінку з детальним описом помилок
            if (env.IsDevelopment())        app.UseDeveloperExceptionPage();

            app.UseRouting();

            // Підключаємо аутентифікацію та ауторизацію
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();


            // Підтримка статичних файлів (css, js, img, т.д.)
            app.UseStaticFiles();

            // Реєстрація маршрутів (ендпоінтів)
            app.UseEndpoints(endpoints =>
            {
                // Тип шляху для адміна:    область доступу -> контролер -> дія -> id чогось (для кого дія) 
                endpoints.MapControllerRoute("admin",   "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // Дефолтний тип шляху: контролер -> дія -> id чогось (для кого дія) 
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

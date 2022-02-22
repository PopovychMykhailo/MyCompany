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
            // �������� ������������ � ����� appsettings.json
            Configuration.Bind("Project", new Config());

            // ϳ�������� ���������� ���������� �� ������
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
            services.AddTransient<DataManager>();

            // ϳ�������� ��
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Config.ConnectionString));

            // ������������ identity ������� (�����)
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

            // ������������ authentication cookie
            services.ConfigureApplicationCookie(option =>
            {
                option.Cookie.Name = "myCompanyAuth";               // ��'� cookie �����
                option.Cookie.HttpOnly = true;                      // ?
                option.LoginPath = "/account/login";                // ���� ������������� ����� ��� �����������
                option.AccessDeniedPath = "/account/accsessdenied"; // ���� ��� ����� �������
                option.SlidingExpiration = true;                    // ������ ����� cookie ���� �� ����������
            });

            // ��������� ������ (������) �����������
            services.AddAuthorization(options =>
            {
                // ����� �������: AdminArea;
                // �������:       �������� ����� ����� admin
                options.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
            });

            // ������ �������� ���������� � ������������ (MVC)
            services.AddControllersWithViews(conf =>
            {
                // ��������� ��: ���������� � ������ Admin, ����� ������� (������� �������) AdminArea  (����� ���������� � ��������� [Area("Admin")] )
                conf.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea")); 
            })
                // ����������� �������� � ASP.NET Core 3.0
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ϳ� ��� �������� �������:    ������ ������� � ��������� ������ �������
            if (env.IsDevelopment())        app.UseDeveloperExceptionPage();

            app.UseRouting();

            // ϳ�������� �������������� �� �����������
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();


            // ϳ������� ��������� ����� (css, js, img, �.�.)
            app.UseStaticFiles();

            // ��������� �������� (��������)
            app.UseEndpoints(endpoints =>
            {
                // ��� ����� ��� �����:    ������� ������� -> ��������� -> �� -> id ������ (��� ���� ��) 
                endpoints.MapControllerRoute("admin",   "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // ��������� ��� �����: ��������� -> �� -> id ������ (��� ���� ��) 
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

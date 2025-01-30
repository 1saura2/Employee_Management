using EmployeeManagement.RepoServices;
using EmployeeManagement.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IDbConnection>(db =>
                new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
            );


            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositoryService>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepositoryService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); 
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); 

            app.UseRouting(); 

            app.UseAuthorization(); 

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

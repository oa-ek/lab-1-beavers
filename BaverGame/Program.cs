using Core;
using Infrastructure.Data.Contexts;
using Infrastructure.Repository;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BaverGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IRepository<Developer>>(provider =>
                new GenericRepository<Developer>(provider.GetService<ApplicationContext>()!));
            
            builder.Services.AddScoped<IRepository<Publisher>>(provider =>
                new GenericRepository<Publisher>(provider.GetService<ApplicationContext>()!));
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
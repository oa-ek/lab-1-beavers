using Core;
using Infrastructure.Data.Contexts;
using Infrastructure.Repository;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaverGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentity<User, UserRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            AddRepositories(builder);

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

        private static void AddRepositories(WebApplicationBuilder builder)
        {
            AddRepository<Developer>(builder);
            AddRepository<Publisher>(builder);
            AddRepository<Tag>(builder);
            AddRepository<UserRole>(builder);
            AddRepository<User>(builder);
            AddRepository<Store>(builder);
            AddRepository<Screenshot>(builder);
            AddRepository<Price>(builder);
            AddRepository<UserGameOwnership>(builder);
            AddRepository<GameTag>(builder);
            AddRepository<Game>(builder);
            AddRepository<Comment>(builder);
            AddRepository<Vote>(builder);
        }

        private static void AddRepository<T>(WebApplicationBuilder builder) 
            where T : class =>
            builder.Services.AddScoped<IRepository<T>>(provider =>
                new GenericRepository<T>(provider.GetService<ApplicationContext>()!));
    }
}
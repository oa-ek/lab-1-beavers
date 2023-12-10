using System.Text.Json.Serialization;
using BaverGame.Domain.Contracts;
using BaverGame.Domain.Entities;
using BaverGame.Persistence.Data.Contexts;
using BaverGame.Persistence.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaverGame;

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

        builder.Services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.Services.AddCors(option =>
        {
            option.AddPolicy("ApplicationCorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
        });
        
        AddRepositories(builder);

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors("ApplicationCorsPolicy");
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
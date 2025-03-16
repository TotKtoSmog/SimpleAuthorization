using Microsoft.EntityFrameworkCore;
using SimpleAuthorization.Models;

namespace SimpleAuthorization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")));

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

            // Настройка маршрутов
            app.MapControllerRoute(
                name: "userRegistration",
                pattern: "User/Registration",
                defaults: new { controller = "User", action = "Registration" });

            app.MapControllerRoute(
                name: "userAuthorization",
                pattern: "User/Authorization",
                defaults: new { controller = "User", action = "Authorization" });

            app.MapControllerRoute(
                name: "userAllUsers",
                pattern: "User/AllUsers",
                defaults: new { controller = "User", action = "AllUsers" });

            app.MapControllerRoute(
                name: "userProfile",
                pattern: "User/{id}",
                defaults: new { controller = "User", action = "Index" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

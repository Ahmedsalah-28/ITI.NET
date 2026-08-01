using Lab15_StudentPortalWeb.Models;
using Lab15_StudentPortalWeb.Services;
using Microsoft.EntityFrameworkCore;

namespace Lab15_StudentPortalWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<StudentPortalContext>(options =>
            {
                options.UseSqlServer("Data Source=AHMEDSALAH\\SQLEXPRESS;Initial Catalog=ITI_StudentPortal;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            });

            // Lab ID = 7 -> 7 % 3 = 1 -> Scoped lifetime
            builder.Services.AddScoped<IAhmedStampService, AhmedStampService>();
            var app = builder.Build();
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"[START] {context.Request.Path}");
                if (context.Request.Path.StartsWithSegments("/audit-07"))
                {
                    Console.WriteLine($"[AUDIT] Ahmed Salah saw a request for {context.Request.Path}");
                }

                await next();

                Console.WriteLine($"[END] {context.Request.Path}");
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}

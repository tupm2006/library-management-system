using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MQTTnet;
using MQTTnet.Client;
// using MQTTnet.Client.Options;
using SmartLibraryManagementSystem.Data;
using SmartLibraryManagementSystem.Data.Entities;
using SmartLibraryManagementSystem.Services;

namespace SmartLibraryManagementSystem.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // THÊM ĐÚNG DÒNG NÀY VÀO ĐÂY ĐỂ ĐĂNG KÝ NGƯỜI ĐƯA THƯ (HTTP CLIENT)
            builder.Services.AddHttpClient();

            // Configure Cookie Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            // Register MqttService
            builder.Services.AddSingleton<IMqttService, MqttService>();

            // Configure DbContext
            builder.Services.AddDbContext<SmartLibraryDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            // ==========================================
            // THÊM ĐOẠN ĐĂNG KÝ (DEPENDENCY INJECTION) NÀY VÀO ĐÂY:
            // Đăng ký Repositories
            builder.Services.AddScoped<SmartLibraryManagementSystem.Data.Repositories.IBookRepository, SmartLibraryManagementSystem.Data.Repositories.BookRepository>();
            builder.Services.AddScoped<SmartLibraryManagementSystem.Data.Repositories.IUserRepository, SmartLibraryManagementSystem.Data.Repositories.UserRepository>();
            builder.Services.AddScoped<SmartLibraryManagementSystem.Data.Repositories.IBorrowRecordRepository, SmartLibraryManagementSystem.Data.Repositories.BorrowRecordRepository>();
            builder.Services.AddScoped<SmartLibraryManagementSystem.Data.Repositories.IReservationRepository, SmartLibraryManagementSystem.Data.Repositories.ReservationRepository>();
            builder.Services.AddScoped<SmartLibraryManagementSystem.Data.Repositories.IReviewRepository, SmartLibraryManagementSystem.Data.Repositories.ReviewRepository>();

            // Đăng ký Services
            builder.Services.AddScoped<SmartLibraryManagementSystem.Business.Interfaces.IBookService, SmartLibraryManagementSystem.Business.Services.BookService>();
            builder.Services.AddScoped<SmartLibraryManagementSystem.Business.Interfaces.IUserService, SmartLibraryManagementSystem.Business.Services.UserService>();
            builder.Services.AddScoped<SmartLibraryManagementSystem.Business.Interfaces.IBorrowService, SmartLibraryManagementSystem.Business.Services.BorrowService>();
            // builder.Services.AddScoped<SmartLibraryManagementSystem.Business.Interfaces.IReservationService, SmartLibraryManagementSystem.Business.Services.ReservationService>();
            // builder.Services.AddScoped<SmartLibraryManagementSystem.Business.Interfaces.IReviewService, SmartLibraryManagementSystem.Business.Services.ReviewService>();
            // ==========================================
            var app = builder.Build();

            // Seed the database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<SmartLibraryDbContext>();
                DbInitializer.Initialize(context);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
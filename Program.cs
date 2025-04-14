using Visitors_Management.IRepository;
using Visitors_Management.Repository;

namespace Visitors_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add CORS services to the container
            //Enable CORS
            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            //builder.WebHost.ConfigureKestrel(options =>
            //{
            //    options.Listen(System.Net.IPAddress.Loopback, 5051); // Port 5051
            //});
            // Add services to the container.
            builder.Services.AddScoped<IUser_Login, User_Login>();
            builder.Services.AddScoped<IUser_Master, User_Master>();
            builder.Services.AddScoped<IEmployee_Master, Employee_Master>();
            builder.Services.AddScoped<IVisitor_Master, Visitor_Master>();
            builder.Services.AddScoped<IDepartment_Master, Department_Master>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            //Enable CORS
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Login}/{id?}");
            app.Run();
        }
    }
}



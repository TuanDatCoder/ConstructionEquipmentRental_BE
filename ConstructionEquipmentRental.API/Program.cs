using Data;
using Microsoft.EntityFrameworkCore;
using Repositories.ProductRepos;
using Repositories.RefreshTokenRepos;
using Services.Helper.MapperProfiles;
using Services.JWTServices;
using Services.ProductServices;

namespace ConstructionEquipmentRental.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //--------------------------------------AUTOMAPPER-----------------------------------------
            builder.Services.AddAutoMapper(typeof(MapperProfiles).Assembly);

            //------------------------------------REPOSITORIES-----------------------------------------
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>(); 
            //------------------------------------SERVICES-----------------------------------------
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IJWTService, JWTService>();

            //-----------------------------------------DB-----------------------------------------
            builder.Services.AddDbContext<ConstructionEquipmentRentalDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}

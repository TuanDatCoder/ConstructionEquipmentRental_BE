using ConstructionEquipmentRental.API.Middlewares;
using Data;
using Microsoft.EntityFrameworkCore;
using Repositories.OrderItemRepos;
using Repositories.OrderRepos;
using Repositories.BrandRepos;
using Repositories.ProductRepos;
using Repositories.RefreshTokenRepos;
using Services.BrandServices;
using Services.Helper.MapperProfiles;
using Services.JWTServices;
using Services.OrderItemServices;
using Services.OrderServices;
using Services.ProductServices;
using Repositories.FeedbackRepos;
using Services.FeedbackServices;
using Repositories.ProductImageRepos;
using Repositories.CategoryRepos;
using Repositories.AccountRepos;
using Services.CategoryServices;
using Services.ProductImageServices;
using Services.AccountServices;
using Services.OrderReportServices;
using Repositories.OrderReportRepos;
using Services.TransactionServices;
using Repositories.TransactionRepos;

namespace ConstructionEquipmentRental.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //------------------------------------AUTOMAPPER-------------------------------------
            builder.Services.AddAutoMapper(typeof(MapperProfiles).Assembly);

            //----------------------------------REPOSITORIES-------------------------------------
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IOrderReportRepository, OrderReportRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

            //------------------------------------SERVICES-----------------------------------------
            builder.Services.AddScoped<IJWTService, JWTService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductImageService, ProductImageService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IOrderReportService, OrderReportService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();

            //-----------------------------------------DB----------------------------------------
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
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}

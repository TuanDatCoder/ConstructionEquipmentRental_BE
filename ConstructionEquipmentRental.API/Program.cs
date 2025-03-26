using ConstructionEquipmentRental.API.Middlewares;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories.AccountRepos;
using Repositories.BrandRepos;
using Repositories.CartItemRepos;
using Repositories.CartRepos;
using Repositories.CategoryRepos;
using Repositories.FeedbackRepos;
using Repositories.OrderItemRepos;
using Repositories.OrderReportRepos;
using Repositories.OrderRepos;
using Repositories.ProductImageRepos;
using Repositories.ProductRepos;
using Repositories.RefreshTokenRepos;
using Repositories.StoreRepos;
using Repositories.TransactionRepos;
using Repositories.WalletLogRepos;
using Repositories.WalletRepos;
using Services.AccountServices;
using Services.AdminServices;
using Services.AuthenticationServices;
using Services.BrandServices;
using Services.CartItemServices;
using Services.CartServices;
using Services.CategoryServices;
using Services.CloudinaryStorageServices;
using Services.EmailServices;
using Services.FeedbackServices;
using Services.FirebaseStorageServices;
using Services.Helper.DecodeTokenHandler;
using Services.Helper.MapperProfiles;
using Services.Helper.VerifyCode;
using Services.JWTServices;
using Services.LessorServices;
using Services.OrderItemServices;
using Services.OrderReportServices;
using Services.OrderServices;
using Services.PayOSServices;
using Services.ProductImageServices;
using Services.ProductServices;
using Services.StoreServices;
using Services.TransactionServices;
using Services.VnPayServices;
using Services.WalletLogServices;
using Services.WalletServices;
using System.Text;

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
            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<IWalletLogRepository, WalletLogRepository>();
            builder.Services.AddScoped<IStoreRepository, StoreRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();



            //------------------------------------SERVICES-----------------------------------------
            builder.Services.AddScoped<IJWTService, JWTService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductImageService, ProductImageService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IOrderReportService, OrderReportService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IDecodeTokenHandler, DecodeTokenHandler>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IStoreService, StoreService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IWalletService, WalletService>();
            builder.Services.AddScoped<IWalletLogService, WalletLogService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICartItemService, CartItemService>();
            builder.Services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
            builder.Services.AddScoped<IVnPayService, VnPayService>();
            builder.Services.AddScoped<IPayOSService, PayOSService>();
            builder.Services.AddScoped<ILessorService, LessorService>();

            builder.Services.AddScoped<ICloudinaryStorageService, CloudinaryStorageService>();

            //-----------------------------------------DB----------------------------------------
            builder.Services.AddDbContext<ConstructionEquipmentRentalDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




            //-----------------------------------------CORS-----------------------------------------

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowAll",
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            builder.Services.AddRouting(options => options.LowercaseUrls = true);



            //-----------------------------------------AUTHENTICATION-----------------------------------------


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:JwtKey"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };
    });


            //-----------------------------------------AUTHORIZATION-----------------------------------------
            builder.Services.AddAuthorization();


            //----------------------------------------------------------------------------------


            builder.Services.AddScoped<VerificationCodeCache>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                ////JWT Config
                option.DescribeAllParametersInCamelCase();
                option.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });

            //++++++++++++ appsettings
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);





            var app = builder.Build();
            /////////////////////////////////////////////////
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                c.RoutePrefix = "swagger";
            });


            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}

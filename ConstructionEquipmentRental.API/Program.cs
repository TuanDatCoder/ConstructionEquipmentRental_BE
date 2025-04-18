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
using Repositories.WalletRepos;
using Services.WalletServices;
using Repositories.WalletLogRepos;
using Services.WalletLogServices;

using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Services.Helper.DecodeTokenHandler;
using Services.EmailServices;
using Services.AuthenticationServices;
using Services.Helper.VerifyCode;
using Repositories.StoreRepos;
using Services.StoreServices;
using Services.AdminServices;
using Repositories.CartRepos;
using Repositories.CartItemRepos;
using Services.CartServices;
using Services.CartItemServices;
using Services.FirebaseStorageServices;
using Services.VnPayServices;
using Services.PayOSServices;
using Services.LessorServices;

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
         //  builder.Services.AddSwaggerGen();


            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
            new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
            new string[] {}
         }
     });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
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

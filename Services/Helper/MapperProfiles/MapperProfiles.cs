using AutoMapper;
using Data.DTOs.Product;
using Data.DTOs.Order;
using Data.DTOs.OrderItem;
using Data.DTOs.Brand;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DTOs.Feedback;
using Data.DTOs.Category;
using Data.DTOs.ProductImage;
using Data.DTOs.OrderReport;
using Data.DTOs.Transaction;
using Data.DTOs.Wallet;
using Data.DTOs.WalletLog;
using Data.DTOs.Account;
using Data.DTOs.Store;

namespace Services.Helper.MapperProfiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {

            // Product
            CreateMap<Product, ProductResponseDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store != null ? src.Store.Name : null));


            CreateMap<ProductRequestDTO, Product>();

            CreateMap<ProductUpdateRequestDTO, Product>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

  

            //Order
            CreateMap<OrderRequestDTO, Order>()
            .ForMember(dest => dest.Status, opt => opt.Ignore()) 
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); 

            CreateMap<Order, OrderResponseDTO>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Username));

            //Order Item
            CreateMap<OrderItem, OrderItemResponseDTO>()
               .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<OrderItemRequestDTO, OrderItem>();

            // Brand
            CreateMap<Brand, BrandResponseDTO>();
            CreateMap<BrandRequestDTO, Brand>();

            //FeedBack
            
            CreateMap<Feedback, FeedbackResponseDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.Username : null));
            CreateMap<FeedbackRequestDTO, Feedback>();

            CreateMap<FeedbackUpdateRequestDTO, Feedback>()
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            //Category
            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<CategoryRequestDTO, Category>();

            //ProductImage
            CreateMap<ProductImage, ProductImageResponseDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null));

            CreateMap<ProductImageRequestDTO, ProductImage>();

            CreateMap<ProductImageUpdateRequestDTO, ProductImage>()
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            //Order Report
            CreateMap<OrderReport, OrderReportResponseDTO>();
            CreateMap<OrderReportRequestDTO, OrderReport>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            //Transaction
            CreateMap<Transaction, TransactionResponseDTO>();
            CreateMap<TransactionRequestDTO, Transaction>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            //Wallet
            CreateMap<Wallet, WalletResponseDTO>();
            CreateMap<WalletRequestDTO, Wallet>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            //WalletLog
            CreateMap<WalletLog, WalletLogResponseDTO>();
            CreateMap<WalletLogRequestDTO, WalletLog>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            //Store

            CreateMap<Store, StoreResponseDTO>()
               .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.Username : null));

            CreateMap<StoreRequestDTO, Store>();

            CreateMap<StoreUpdateRequestDTO, Store>()
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));


            //Account 
            CreateMap<AccountRequestDTO, Account>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.StoreId, opt => opt.Ignore())
           .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
           .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
           .ForMember(dest => dest.Status, opt => opt.Ignore())
           .ForMember(dest => dest.Points, opt => opt.Ignore())
           .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<Account, AccountResponseDTO>()
          .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store != null ? src.Store.Name : null));
      

        }
    }
}

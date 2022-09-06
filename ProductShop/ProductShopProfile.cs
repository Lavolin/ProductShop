using System.Linq;

namespace ProductShop
{
    using DTOs.User;
    using DTOs.Product;
    using DTOs.Category;
    using DTOs.CategoryProduct;
    using Models;

    using AutoMapper;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUserDto, User>();
            this.CreateMap<ImportProductDto, Product>();
            this.CreateMap<ImportCategoryDto, Category>();
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();

            this.CreateMap<Product, ExportProductRangeDto>()
                .ForMember(d => d.SellerFullName,
                    mo 
                        => mo.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            //Inner DTO
            this.CreateMap<Product, ExportUsersSoldProductsDto>()
                .ForMember(d => d.BuyerFirstName, mo
                    => mo.MapFrom(s => s.Buyer.FirstName))
                .ForMember(d => d.BuyerLastName, mo
                    => mo.MapFrom(s => s.Buyer.LastName));

            //Outer DTO
            this.CreateMap<User, ExportUsersWithSoldProductsDto>()
                .ForMember(d => d.SoldProducts, mo
                    => mo.MapFrom(s => s.ProductsSold
                        .Where(p => p.BuyerId.HasValue)));

            this.CreateMap<Product, ExportProductsShortInfoDto>();
            this.CreateMap<User, ExportProductsSoldFullInfoDto>()
               .ForMember(d => d.SoldProducts, mo 
                   => mo.MapFrom(s => s.ProductsSold
                        .Where(p => p.BuyerId.HasValue)));

            this.CreateMap<User, ExportUsersFullProductInfoDto>()
                .ForMember(d => d.SoldProductsInfo, mo 
                    => mo.MapFrom(s => s)); 
        }
    }
}

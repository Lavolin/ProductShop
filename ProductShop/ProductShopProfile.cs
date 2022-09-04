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
        }
    }
}

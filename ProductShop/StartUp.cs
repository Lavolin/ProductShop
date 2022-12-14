using System.Linq;
using AutoMapper.QueryableExtensions;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ProductShop.DTOs.Category;
using ProductShop.DTOs.CategoryProduct;
using ProductShop.DTOs.Product;

namespace ProductShop
{
    using System;
    using System.Collections.Generic;
    using Models;
    using System.IO;

    using Newtonsoft.Json;
    using Data;
    using DTOs.User;

    using AutoMapper;
    using System.ComponentModel.DataAnnotations;

    public class StartUp
    {
        private static IMapper mapper;

        private static string filePath;
        public static void Main(string[] args)
        {
           //configuration of AutoMapper
            mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));

            

            ProductShopContext dbContext = new ProductShopContext();

            InitializeOutputFilePath("users-and-products.json");

            //InitializeDataSetFilePath("categories-products.json");


            //string inputJson = File.ReadAllText(filePath);

            //string inputJson = File.ReadAllText("../../../Datasets/users.json");


            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //Console.WriteLine("Database was successfully created");

           // string output = ImportCategoryProducts(dbContext, inputJson);
            //Console.WriteLine(output);

            string json = GetUsersWithProducts(dbContext);
            File.WriteAllText(filePath, json);
        }

        //Ex 1.	Import Data
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            ICollection<User> validUsers = new List<User>();
            foreach (ImportUserDto uDto in userDtos)
            {
                if (!IsValid(uDto))
                {
                   continue; 
                }

                User user = mapper.Map<User>(uDto);
                validUsers.Add(user);
            }

            context.AddRange(validUsers);
            context.SaveChanges();
            return $"Successfully imported {validUsers.Count}";
        }

        //Ex 2. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ImportProductDto[] productDtos = JsonConvert
                .DeserializeObject<ImportProductDto[]>(inputJson);

            ICollection<Product> validProducts = new List<Product>();
            foreach (ImportProductDto pDto in productDtos)
            {
                if (!IsValid(pDto))
                {
                    continue;
                }
                Product product = mapper.Map<Product>(pDto);
                validProducts.Add(product);
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }

        //Ex 3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            ImportCategoryDto[] categoryDtos =
                JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);

            ICollection<Category> valiCategories = new List<Category>();
            foreach (ImportCategoryDto cDto in categoryDtos)
            {
                if (!IsValid(cDto))
                {
                    continue;
                }
                Category category = mapper.Map<Category>(cDto);
                valiCategories.Add(category);
            }

            context.Categories.AddRange(valiCategories);
            context.SaveChanges();

            return $"Successfully imported {valiCategories.Count}";
        }

        //Ex 4. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            ImportCategoryProductDto[] categoryProductDtos =
                JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson);

            ICollection<CategoryProduct> validCategoryProducts = new List<CategoryProduct>();
            foreach (ImportCategoryProductDto cpDto in categoryProductDtos)
            {
                if (!IsValid(cpDto))
                {
                    continue;
                }

                CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(cpDto);
                validCategoryProducts.Add(categoryProduct);
            }
            context.CategoryProducts.AddRange(validCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validCategoryProducts.Count}";
        }

        //Ex 5. Export Products in Range

        public static string GetProductsInRange(ProductShopContext context)
        {
            ExportProductRangeDto[] products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ProjectTo<ExportProductRangeDto>(mapper.ConfigurationProvider)
                .ToArray();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);
            return json;
        }

        //Ex 6. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            ExportUsersWithSoldProductsDto[] users = context
                .Users
                .Where(u => u.ProductsSold.Any(b => b.BuyerId.HasValue))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<ExportUsersWithSoldProductsDto>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }

        //Ex 8. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            ExportUsersFullProductInfoDto[] users = context
                .Users
                .Where(u => u.ProductsSold.Any(b => b.BuyerId.HasValue))
                .OrderByDescending(u => u.ProductsSold.Count(p => p.BuyerId.HasValue))
                .ProjectTo<ExportUsersFullProductInfoDto>(mapper.ConfigurationProvider)
                .ToArray();

            ExportUsersInfoDto serDto = new ExportUsersInfoDto()
            {
                UsersCount = users.Length,
                Users = users
            };
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(serDto, Formatting.Indented, 
                jsonSerializerSettings);
            return json;

            //Manual Mapping
            //ExportUsersInfoDto usersInfoDtoDto = new ExportUsersInfoDto()
            //{
            //    Users = context
            //        .Users
            //        .Where(u => u.ProductsSold.Any(b => b.BuyerId.HasValue)
            //            .OrderByDescending(u => u.ProductsSold.Count(p => p.BuyerId.HasValue))
            //            .Select(u => new ExportUsersFullProductInfoDto()
            //            {
            //                FirstName = u.FirstName,
            //                LastName = u.LastName,
            //                Age = u.Age,
            //                SoldProductsInfo = new ExportProductsShortInfoDto()
            //                {
            //                    SoldProducts = u.ProductsSold,
            //                    .Where(p => p.BuyerId.HasValue),
            //                    .Select(p => new ExportSoldProductsShortInfoDto()
            //                    {
            //                        Name = p.Name,
            //                        Price = ProductShopContext.Price
            //                    })
            //                    .ToArray()
            //                }
            //            }))
            //                    .ToArray()


            //};

        }

        private static void InitializeDataSetFilePath(string fileName)
        {
            filePath =
                Path.Combine(Directory.GetCurrentDirectory(), "../../../Datasets/", fileName);
        }

        private static void InitializeOutputFilePath(string fileName)
        {
            filePath =
                Path.Combine(Directory.GetCurrentDirectory(), "../../../Results/", fileName);
        }


        /// <summary>
        /// Executes all validation attributes in a class and return true or false
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}
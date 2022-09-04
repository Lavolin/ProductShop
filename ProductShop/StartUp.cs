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

            InitializeFilePath("users.json");
            string inputJson = File.ReadAllText(filePath);
            //string inputJson = File.ReadAllText("../../../Datasets/users.json");


            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //Console.WriteLine("Database was successfully created");

            string output = ImportUsers(dbContext, inputJson);
            Console.WriteLine(output);
        }

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

        private static void InitializeFilePath(string fileName)
        {
            filePath =
                Path.Combine(Directory.GetCurrentDirectory(), "../../../Datasets/", fileName);
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
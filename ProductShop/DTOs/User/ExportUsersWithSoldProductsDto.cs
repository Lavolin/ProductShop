using Newtonsoft.Json;
using ProductShop.Common;
using System.ComponentModel.DataAnnotations;
using ProductShop.DTOs.Product;

namespace ProductShop.DTOs.User
{
    [JsonObject]
    public class ExportUsersWithSoldProductsDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("soldProducts")]
        public ExportUsersSoldProductsDto[] SoldProducts { get; set; }
    }
}

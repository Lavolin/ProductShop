using Newtonsoft.Json;
using ProductShop.DTOs.Product;

namespace ProductShop.DTOs.User
{
    [JsonObject]
    public class ExportUsersFullProductInfoDto
    {
        [JsonProperty("fistName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }

        [JsonProperty("soldProducts")]
        public ExportProductsSoldFullInfoDto[] SoldProductsInfo { get; set; }
    }
}

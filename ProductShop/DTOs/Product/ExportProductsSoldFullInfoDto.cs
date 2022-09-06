using System.Linq;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Product
{
    [JsonObject]
    public class ExportProductsSoldFullInfoDto
    {
        [JsonProperty("count")]
        public int ProductsCount
            => SoldProducts.Any() ? SoldProducts.Length : 0;

        [JsonProperty("products")]
        public ExportProductsShortInfoDto[] SoldProducts { get; set; }
    }
}

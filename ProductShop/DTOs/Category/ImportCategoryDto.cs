using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ProductShop.Common;

namespace ProductShop.DTOs.Category
{
    [JsonObject]
    public class ImportCategoryDto
    {
        [JsonProperty("name")]
        [Required]
        [MinLength(GlobalConstants.categoryNameMinLength)]
        [MaxLength(GlobalConstants.categoryNameMaxLength)]
        public string Name { get; set; }

    }
}

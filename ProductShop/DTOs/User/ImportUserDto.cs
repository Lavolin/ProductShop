namespace ProductShop.DTOs.User
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    using Common;
    public class ImportUserDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        [Required]
        [MinLength(GlobalConstants.userLastNameMinLength)]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }
    }
}

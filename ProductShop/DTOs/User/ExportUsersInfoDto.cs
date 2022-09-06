using Newtonsoft.Json;

namespace ProductShop.DTOs.User
{
    [JsonObject]
    public class ExportUsersInfoDto
    {
        [JsonProperty("usersCount")] 
        public int UsersCount { get; set; }

        [JsonProperty("users")]
        public ExportUsersFullProductInfoDto[] Users { get; set; }
    }
}

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LearnSmartCoding.CosmosDb.Linq.API.Core
{
    public class UserDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("settings")]
        public UserSettings Settings { get; set; }
    }

    public class UserSettings
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("notificationEnabled")]
        public bool NotificationEnabled { get; set; }
    }
}

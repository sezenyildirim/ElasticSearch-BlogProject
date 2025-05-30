using System.Text.Json.Serialization;

namespace ElastichSearch_BlogProject.Models
{
    public class Flowers
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = null!;


        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("latinName")]
        public string LatinName { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("description")]
        public string Details { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }
    }
}

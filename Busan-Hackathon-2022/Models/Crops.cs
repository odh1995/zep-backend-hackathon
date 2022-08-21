using Newtonsoft.Json ;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busan_Hackathon_2022
{
    public class Crops
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; internal set; }
        [JsonProperty(PropertyName = "startTime")]
        public long StartTime { get; set; }
        [JsonProperty(PropertyName = "lastWater")]
        public long LastWater { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "growth")]
        public int Growth { get; set; } = 0;
        [JsonProperty(PropertyName = "x_coordinate")]
        public int XCoordinate { get; set; }
        [JsonProperty(PropertyName = "y_coordinate")]
        public int YCoordinate { get; set; }
    }
}
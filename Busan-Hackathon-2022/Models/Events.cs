using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Busan_Hackathon_2022.Models
{
    public class Event
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "startTime")]
        public long StartTime { get; set; }
        [JsonProperty(PropertyName = "endTime")]
        public long EndTime { get; set; }
    }
}

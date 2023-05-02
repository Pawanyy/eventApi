using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eventApi.Models
{
    public class EventUser
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public int EventId { get; set; }
        public User User { get; set; }
        public Event Event { get; set; }
    }
}
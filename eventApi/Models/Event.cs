using System.Text.Json.Serialization;

namespace eventApi.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public string Name { get; set; }
        public string Tagline { get; set; }
        public DateTime Schedule { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int ModeratorId { get; set; }
        [JsonIgnore]
        public User Moderator { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public int RigorRank { get; set; }
        public ICollection<EventUser> Attendees { get; set; }
    }

}
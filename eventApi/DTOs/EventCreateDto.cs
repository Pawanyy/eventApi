using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using eventApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace eventApi.DTOs
{
    public class EventAttendee
    {

    }

    public class EventCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Tagline { get; set; }
        [Required]
        public DateTime Schedule { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        [FromForm(Name = "image")]
        public IFormFile ImageFile { get; set; }
        [Required]
        public int ModeratorId { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string SubCategory { get; set; }
        [Required]
        public int RigorRank { get; set; }
    }
}
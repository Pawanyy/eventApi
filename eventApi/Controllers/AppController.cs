using System.Security.Claims;
using eventApi.Data;
using eventApi.DTOs;
using eventApi.Interfaces;
using eventApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eventApi.Controllers
{
    public class AppController : BaseV3ApiController
    {
        private readonly DataContext db;
        public readonly ITokenService _tokenService;
        public AppController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            db = context;
        }

        [Authorize]
        [HttpGet("events")]
        public async Task<IActionResult> GetAllEvents(string type = "latest", int page = 1, int limit = 10) 
        {

            IQueryable<Event> eventsQuery = db.Events;

            if (!string.IsNullOrEmpty(type))
            {
                if (type.ToLower() == "latest")
                {
                    eventsQuery = eventsQuery.OrderByDescending(e => e.Schedule);
                }
                else if (type.ToLower() == "oldest")
                {
                    eventsQuery = eventsQuery.OrderBy(e => e.Schedule);
                }
                else
                {
                    return BadRequest("Invalid event type.");
                }
            }

            var eventsToSkip = (page - 1) * limit;

            var foundEvents = await eventsQuery
                                .Skip(eventsToSkip)
                                .Take(limit)
                                .ToListAsync();

            return Ok(foundEvents);
        }

        [Authorize]
        [HttpGet("events/id={id:int}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var foundEvent = await db.Events.FirstOrDefaultAsync(e => e.Id == id);
            //            .Include(e => e.Moderator)
            //              .Include(e => e.Attendees)
            //              .ThenInclude(a => a.User)
            //              .Include(e => e.User)

            if(foundEvent == null) return NotFound();

            return foundEvent;
        }

        [Authorize]
        [HttpDelete("events/{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var MyEvent = await db.Events.FindAsync(id);
            if (MyEvent == null)
            {
                return NotFound();
            }

            db.Events.Remove(MyEvent);
            await db.SaveChangesAsync();

            return Ok(new {message = "Event Deleted Successfully"});
        }

        [Authorize]
        [HttpPost("events")]
        public async Task<ActionResult<Event>> CreateEvent([FromForm] EventCreateDto eventCreateDto) {

            int UserId = await GetUserIdAsync();

            var user = await db.Users.FindAsync(UserId);
            var morderator = await db.Users.FindAsync(eventCreateDto.ModeratorId);

            if(user == null) return BadRequest(new {error = "User not exist!"});
            
            if(morderator == null) return BadRequest(new {error = "Moderator not exist!"});

            var newEvent = new Event
            {
                UserId = UserId,
                Name = eventCreateDto.Name,
                Tagline = eventCreateDto.Tagline,
                Schedule = eventCreateDto.Schedule,
                Description = eventCreateDto.Description,
                ModeratorId = eventCreateDto.ModeratorId,
                Category = eventCreateDto.Category,
                SubCategory = eventCreateDto.SubCategory,
                RigorRank = eventCreateDto.RigorRank,
            };

            if (eventCreateDto.ImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await eventCreateDto.ImageFile.CopyToAsync(memoryStream);
                    newEvent.Image = memoryStream.ToArray();
                }
            }

            db.Add(newEvent);
            await db.SaveChangesAsync();

            return newEvent;
        }

        private async Task<int> GetUserIdAsync()
        {
            string username = this.GetCurrentUsername();

            var user = await db.Users.SingleOrDefaultAsync(x => x.Username == username);

            if(user == null) return 0;

            return user.Id;
        }

        private string GetCurrentUsername()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string Username = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                return Username;
            }

            return null;
        }
    }
}
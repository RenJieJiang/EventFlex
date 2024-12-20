using Microsoft.AspNetCore.Mvc;
using EventTypeManagement.API.Data;
using EventTypeManagement.API.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace EventTypeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventTypesController : ControllerBase
    {
        private readonly MongoDbContext _context;

        public EventTypesController(MongoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventType>>> Get()
        {
            var eventTypes = await _context.EventTypes.Find(_ => true).ToListAsync();
            return Ok(eventTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventType>> Get(string id)
        {
            var eventType = await _context.EventTypes.Find(et => et.Id == id).FirstOrDefaultAsync();
            if (eventType == null)
            {
                return NotFound();
            }
            return Ok(eventType);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EventType eventType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            eventType.Id = ObjectId.GenerateNewId().ToString();

            await _context.EventTypes.InsertOneAsync(eventType);
            return CreatedAtAction(nameof(Get), new { id = eventType.Id }, eventType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] EventType eventType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _context.EventTypes.ReplaceOneAsync(et => et.Id == id, eventType);
            if (result.MatchedCount == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _context.EventTypes.DeleteOneAsync(et => et.Id == id);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
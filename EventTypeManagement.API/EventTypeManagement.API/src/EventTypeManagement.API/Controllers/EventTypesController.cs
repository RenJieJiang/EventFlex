using Microsoft.AspNetCore.Mvc;
using EventTypeManagement.API.Data;
using EventTypeManagement.API.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using EventTypeManagement.API.Messages;

namespace EventTypeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventTypesController : ControllerBase
    {
        private readonly IMongoDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public EventTypesController(IMongoDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
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

            // Create the message model
            var message = new EventTypeCreatedMessage
            {
                Id = eventType.Id,
                Name = eventType.Name,
                Description = eventType.Description,
                CreatedAt = DateTime.UtcNow
            };

            // Call the MessagingService
            var response = await SendMessageAsync("message/event-type-created", message);

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error appropriately
                return StatusCode((int)response.StatusCode, "Failed to notify the messaging service.");
            }

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

        private async Task<HttpResponseMessage> SendMessageAsync(string endpoint, object message)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var messagingServiceDomain = _configuration["MessagingService:Domain"] ?? "messaging-service";
            var messagingServicePort = _configuration["MessagingService:Port"] ?? "3002";
            var url = $"http://{messagingServiceDomain}:{messagingServicePort}/{endpoint}";
            var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(url, content);
        }
    }
}
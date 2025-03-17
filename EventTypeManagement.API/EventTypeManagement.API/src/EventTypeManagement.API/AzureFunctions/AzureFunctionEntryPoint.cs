using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using EventTypeManagement.API.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using EventTypeManagement.API.Models;
using System;
using MongoDB.Driver;
using MongoDB.Bson;
using EventTypeManagement.API.Messages;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.Http;
using EventTypeManagement.API.AzureFunctions;

[assembly: FunctionsStartup(typeof(AzureFunctionStartup))]

namespace EventTypeManagement.API
{
    public class AzureFunctionStartup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<MongoDbContext>();
            builder.Services.AddHttpClient();
        }
    }

    public class EventTypeFunctions
    {
        private readonly MongoDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public EventTypeFunctions(MongoDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [FunctionName("GetEventTypes")]
        public async Task<IActionResult> GetEventTypes(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "eventtypes")] HttpRequest req)
        {
            log.LogInformation("GetEventTypes function triggered");
            try {
                // Log connection string (sanitized)
                var connStr = _context.ConnectionString ?? "null";
                var sanitizedConnStr = connStr.Contains("@") 
                    ? connStr.Substring(0, connStr.IndexOf("@")) + "@[REDACTED]" 
                    : "[CONNECTION STRING NOT FOUND]";
                Console.WriteLine($"Attempting MongoDB connection with: {sanitizedConnStr}");
                    
                // Try to get collection stats first as a quick test
                var stats = await _context.Database.RunCommandAsync<BsonDocument>(new BsonDocument("dbstats", 1));
                Console.WriteLine($"Connected to MongoDB. Database stats: {stats.ToJson()}");
                
                var eventTypes = await _context.EventTypes.Find(_ => true).ToListAsync();
                Console.WriteLine($"Found {eventTypes.Count} event types");
                return new OkObjectResult(eventTypes);
            }
            catch (Exception ex) {
                Console.WriteLine($"Error in GetEventTypes: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("GetEventTypeById")]
        public async Task<IActionResult> GetEventTypeById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "eventtypes/{id}")] HttpRequest req,
            string id)
        {
            var eventType = await _context.EventTypes.Find(et => et.Id == id).FirstOrDefaultAsync();
            if (eventType == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(eventType);
        }

        [FunctionName("CreateEventType")]
        public async Task<IActionResult> CreateEventType(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "eventtypes")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var eventType = JsonSerializer.Deserialize<EventType>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (eventType == null)
            {
                return new BadRequestResult();
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
            await SendMessageAsync("message/event-type-created", message);

            return new CreatedResult($"/api/eventtypes/{eventType.Id}", eventType);
        }

        [FunctionName("UpdateEventType")]
        public async Task<IActionResult> UpdateEventType(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "eventtypes/{id}")] HttpRequest req,
            string id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var eventType = JsonSerializer.Deserialize<EventType>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (eventType == null)
            {
                return new BadRequestResult();
            }

            var result = await _context.EventTypes.ReplaceOneAsync(et => et.Id == id, eventType);
            if (result.MatchedCount == 0)
            {
                return new NotFoundResult();
            }
            return new NoContentResult();
        }

        [FunctionName("DeleteEventType")]
        public async Task<IActionResult> DeleteEventType(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eventtypes/{id}")] HttpRequest req,
            string id)
        {
            var result = await _context.EventTypes.DeleteOneAsync(et => et.Id == id);
            if (result.DeletedCount == 0)
            {
                return new NotFoundResult();
            }
            return new NoContentResult();
        }

        private async Task<HttpResponseMessage> SendMessageAsync(string endpoint, object message)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var messagingServiceDomain = _configuration["MessagingService:Domain"] ?? "";
            var url = $"{messagingServiceDomain}/{endpoint}";
            var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(url, content);
        }
    }
}


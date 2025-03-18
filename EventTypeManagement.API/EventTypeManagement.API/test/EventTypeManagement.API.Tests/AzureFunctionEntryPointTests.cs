using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventTypeManagement.API.Data;
using EventTypeManagement.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace EventTypeManagement.API.Tests
{
    public class EventTypeFunctionsTests
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly Mock<ILogger<EventTypeFunctions>> _mockLogger;
        private readonly Mock<IMongoCollection<EventType>> _mockCollection;
        private readonly EventTypeFunctions _functions;

        public EventTypeFunctionsTests()
        {
            // Create a real configuration from appsettings.json
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Set up mocks
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockLogger = new Mock<ILogger<EventTypeFunctions>>();
            _mockCollection = new Mock<IMongoCollection<EventType>>();

            // Create a mock database
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<EventType>(It.IsAny<string>(), null))
                .Returns(_mockCollection.Object);

            // Create the real context WITH A CONSTRUCTOR THAT DOESN'T DO MUCH
            var mockContext = new MongoDbContext(_configuration);

            // Use reflection to replace the internal database field
            typeof(MongoDbContext)
                .GetField("_database", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(mockContext, mockDatabase.Object);

            // Create the function instance
            _functions = new EventTypeFunctions(
                mockContext,
            _mockHttpClientFactory.Object,
                _configuration,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task GetEventTypes_ReturnsOkObjectResult_WithEventTypesList()
        {
            // Arrange
            var eventTypes = new List<EventType>
            {
                new EventType { Id = "1", Name = "Conference", Description = "A conference event" },
                new EventType { Id = "2", Name = "Workshop", Description = "A workshop event" }
            };

            var mockCursor = new Mock<IAsyncCursor<EventType>>();
            mockCursor.Setup(m => m.Current).Returns(eventTypes);
            mockCursor
                .SetupSequence(m => m.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<EventType>>(),
                    It.IsAny<FindOptions<EventType, EventType>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(mockCursor.Object);

            // Ensure we have stats mocked - with proper null checking
            var mockStats = new BsonDocument { { "dbStats", 1 }, { "collections", 2 } };

            // Get the database field with null checking
            var databaseField = typeof(MongoDbContext)
                .GetField("_database", BindingFlags.NonPublic | BindingFlags.Instance);

            var contextField = _functions.GetType()
                .GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance);

            if (databaseField == null || contextField == null)
            {
                Assert.Fail("Could not find necessary fields via reflection. Test setup is incorrect.");
            }

            var context = contextField.GetValue(_functions);
            if (context == null)
            {
                Assert.Fail("The _context field is null.");
            }

            var database = databaseField.GetValue(context);
            if (database == null)
            {
                Assert.Fail("The _database field is null.");
            }

            var mockDatabase = Mock.Get((IMongoDatabase)database);

            // Fix the nullability constraint warning
            mockDatabase.Setup(d => d.RunCommandAsync(
                It.IsAny<Command<BsonDocument>>(),
                It.IsAny<ReadPreference>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockStats);

            // Create a mock HttpRequest
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;

            // Act
            var result = await _functions.GetEventTypes(request);

            // Assert - with better diagnostics
            if (result is StatusCodeResult statusResult)
            {
                Assert.Fail($"Expected OkObjectResult but got StatusCodeResult with status code: {statusResult.StatusCode}");
            }

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEventTypes = Assert.IsAssignableFrom<List<EventType>>(okResult.Value);
            Assert.Equal(2, returnedEventTypes.Count);
            Assert.Equal("Conference", returnedEventTypes[0].Name);
            Assert.Equal("Workshop", returnedEventTypes[1].Name);
        }

        [Fact]
        public async Task GetEventTypeById_ReturnsOkObjectResult_WhenEventTypeExists()
        {
            // Arrange
            var eventType = new EventType { Id = "1", Name = "Conference", Description = "A conference event" };

            var mockCursor = new Mock<IAsyncCursor<EventType>>();
            mockCursor.Setup(m => m.Current).Returns(new List<EventType> { eventType });
            mockCursor
                .SetupSequence(m => m.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<EventType>>(),
                    It.IsAny<FindOptions<EventType, EventType>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(mockCursor.Object);

            // Create a mock HttpRequest
            var context = new DefaultHttpContext();
            var request = context.Request;

            // Act
            var result = await _functions.GetEventTypeById(request, "1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEventType = Assert.IsAssignableFrom<EventType>(okResult.Value);
            Assert.Equal("Conference", returnedEventType.Name);
        }

        [Fact]
        public async Task GetEventTypeById_ReturnsNotFoundResult_WhenEventTypeDoesNotExist()
        {
            // Arrange
            var mockCursor = new Mock<IAsyncCursor<EventType>>();
            mockCursor.Setup(m => m.Current).Returns(new List<EventType>());
            mockCursor
                .SetupSequence(m => m.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<EventType>>(),
                    It.IsAny<FindOptions<EventType, EventType>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(mockCursor.Object);

            // Create a mock HttpRequest
            var context = new DefaultHttpContext();
            var request = context.Request;

            // Act
            var result = await _functions.GetEventTypeById(request, "999");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateEventType_ReturnsCreatedResult_WithEventType()
        {
            // Arrange
            var eventType = new EventType { Name = "Conference", Description = "A conference event" };
            var eventTypeJson = JsonConvert.SerializeObject(eventType);

            // Create a mock HttpRequest with a body
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes(eventTypeJson));

            // Mock the HttpClientFactory
            _mockHttpClientFactory
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());

            // Mock the InsertOneAsync method
            _mockCollection
                .Setup(c => c.InsertOneAsync(
                    It.IsAny<EventType>(),
                    It.IsAny<InsertOneOptions>(),
                    It.IsAny<CancellationToken>()
                ))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _functions.CreateEventType(request);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            var createdEventType = Assert.IsAssignableFrom<EventType>(createdResult.Value);
            Assert.Equal("Conference", createdEventType.Name);
            Assert.NotNull(createdEventType.Id);
        }

        [Fact]
        public async Task UpdateEventType_ReturnsNoContent_WhenEventTypeExists()
        {
            // Arrange
            var eventType = new EventType { Id = "1", Name = "Conference Updated", Description = "An updated conference event" };
            var eventTypeJson = JsonConvert.SerializeObject(eventType);

            // Create a mock HttpRequest with a body
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes(eventTypeJson));

            // Mock the ReplaceOneAsync method
            var replaceResult = new ReplaceOneResult.Acknowledged(1, 1, null);
            _mockCollection
                .Setup(c => c.ReplaceOneAsync(
                    It.IsAny<FilterDefinition<EventType>>(),
                    It.IsAny<EventType>(),
                    It.IsAny<ReplaceOptions>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(replaceResult);

            // Act
            var result = await _functions.UpdateEventType(request, "1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateEventType_ReturnsNotFound_WhenEventTypeDoesNotExist()
        {
            // Arrange
            var eventType = new EventType { Id = "999", Name = "Conference Updated", Description = "An updated conference event" };
            var eventTypeJson = JsonConvert.SerializeObject(eventType);

            // Create a mock HttpRequest with a body
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes(eventTypeJson));

            // Mock the ReplaceOneAsync method
            var replaceResult = new ReplaceOneResult.Acknowledged(0, 0, null);
            _mockCollection
                .Setup(c => c.ReplaceOneAsync(
                    It.IsAny<FilterDefinition<EventType>>(),
                    It.IsAny<EventType>(),
                    It.IsAny<ReplaceOptions>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(replaceResult);

            // Act
            var result = await _functions.UpdateEventType(request, "999");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEventType_ReturnsNoContent_WhenEventTypeExists()
        {
            // Arrange
            // Create a mock HttpRequest
            var context = new DefaultHttpContext();
            var request = context.Request;

            // Mock the DeleteOneAsync method
            var deleteResult = new DeleteResult.Acknowledged(1);
            _mockCollection
                .Setup(c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<EventType>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _functions.DeleteEventType(request, "1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEventType_ReturnsNotFound_WhenEventTypeDoesNotExist()
        {
            // Arrange
            // Create a mock HttpRequest
            var context = new DefaultHttpContext();
            var request = context.Request;

            // Mock the DeleteOneAsync method
            var deleteResult = new DeleteResult.Acknowledged(0);
            _mockCollection
                .Setup(c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<EventType>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _functions.DeleteEventType(request, "999");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetEventTypes_ReturnsServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<EventType>>(),
                    It.IsAny<FindOptions<EventType, EventType>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ThrowsAsync(new Exception("Test exception"));

            // Create a mock HttpRequest
            var context = new DefaultHttpContext();
            var request = context.Request;

            // Act
            var result = await _functions.GetEventTypes(request);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }
    }
}
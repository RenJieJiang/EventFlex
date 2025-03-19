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
        // 声明模拟对象和配置对象
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory; // 模拟的 HttpClient 工厂
        private readonly IConfiguration _configuration; // 配置对象
        private readonly Mock<ILogger<EventTypeFunctions>> _mockLogger; // 模拟的日志记录器
        private readonly Mock<IMongoCollection<EventType>> _mockCollection; // 模拟的 MongoDB 集合
        private readonly Mock<IMongoDatabase> _mockDatabase; // 模拟的 MongoDB 数据库
        private readonly EventTypeFunctions _functions; // 要测试的函数实例

        public EventTypeFunctionsTests()
        {
            // 创建一个真实的配置对象，从 appsettings.json 文件加载配置
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // 设置配置文件的基础路径为当前目录
                .AddJsonFile("appsettings.json") // 从 appsettings.json 文件中加载配置
                .Build();

            // 设置模拟对象
            _mockHttpClientFactory = new Mock<IHttpClientFactory>(); // 创建模拟的 HttpClient 工厂
            _mockLogger = new Mock<ILogger<EventTypeFunctions>>(); // 创建模拟的日志记录器
            _mockCollection = new Mock<IMongoCollection<EventType>>(); // 创建模拟的 MongoDB 集合
            _mockDatabase = new Mock<IMongoDatabase>(); // 创建模拟的 MongoDB 数据库

            // 配置数据库模拟对象
            _mockDatabase.Setup(db => db.GetCollection<EventType>(It.IsAny<string>(), null))
                .Returns(_mockCollection.Object); // 当调用数据库的 GetCollection 方法时，返回模拟的集合

            // 创建一个真实的 MongoDbContext 实例，使用配置对象
            var mockContext = new MongoDbContext(_configuration);

            // 使用反射来替换 MongoDbContext 内部的 _database 字段
            typeof(MongoDbContext)
                .GetField("_database", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(mockContext, _mockDatabase.Object); // 将模拟的数据库对象赋值给 MongoDbContext 的 _database 字段

            // 创建要测试的函数实例
            _functions = new EventTypeFunctions(
                mockContext, // 传入模拟的数据库上下文
                _mockHttpClientFactory.Object, // 传入模拟的 HttpClient 工厂
                _configuration, // 传入配置对象
                _mockLogger.Object // 传入模拟的日志记录器
            );
        }

        // 测试 GetEventTypes 方法，当成功获取事件类型列表时，应返回 OkObjectResult
        [Fact]
        public async Task GetEventTypes_ReturnsOkObjectResult_WithEventTypesList()
        {
            // Arrange
            var eventTypes = new List<EventType>
            {
                new EventType { Id = "1", Name = "Conference", Description = "A conference event" },
                new EventType { Id = "2", Name = "Workshop", Description = "A workshop event" }
            };

            // 创建模拟的异步游标
            var mockCursor = new Mock<IAsyncCursor<EventType>>();
            mockCursor.Setup(m => m.Current).Returns(eventTypes); // 设置游标当前返回的事件类型列表
            mockCursor
                .SetupSequence(m => m.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false); // 设置游标移动的顺序

            // 配置模拟的集合，当调用 FindAsync 方法时，返回模拟的游标
            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<EventType>>(),
                    It.IsAny<FindOptions<EventType, EventType>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(mockCursor.Object);

            // 设置数据库统计信息 Setup the database stats using the stored _mockDatabase reference 
            var mockStats = new BsonDocument { { "dbStats", 1 }, { "collections", 2 } };

            _mockDatabase.Setup(d => d.RunCommandAsync(
                It.IsAny<Command<BsonDocument>>(),
                It.IsAny<ReadPreference>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockStats);

            // Create a mock HttpRequest
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;

            // Act
            var result = await _functions.GetEventTypes(request);

            // Assert
            if (result is StatusCodeResult statusResult)
            {
                Assert.Fail($"Expected OkObjectResult but got StatusCodeResult with status code: {statusResult.StatusCode}");
            }

            var okResult = Assert.IsType<OkObjectResult>(result); // 断言结果是 OkObjectResult
            var returnedEventTypes = Assert.IsAssignableFrom<List<EventType>>(okResult.Value); // 断言返回的值是EventType列表
            Assert.Equal(2, returnedEventTypes.Count); // 断言返回的事件类型数量为 2
            Assert.Equal("Conference", returnedEventTypes[0].Name); // 断言第一个事件类型的名称为 "Conference"
            Assert.Equal("Workshop", returnedEventTypes[1].Name); // 断言第二个事件类型的名称为 "Workshop"
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
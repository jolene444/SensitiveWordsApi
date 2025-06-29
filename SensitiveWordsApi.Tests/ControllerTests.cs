using Xunit;
using Moq;
using SensitiveWordsApi.Controllers;
using SensitiveWordsApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SensitiveWordsApi.Tests
{
    public class ControllerTests
    {
        // Test that adding a word returns OK with the expected ID
        [Fact]
        public async Task Add_ReturnsOkResult_WhenWordIsAdded()
        {
            // Arrange: set up a mock repository
            var mockRepo = new Mock<ISensitiveWordsRepository>();
            mockRepo.Setup(r => r.AddAsync("secret")).ReturnsAsync(1);

            // Inject mock repo into controller
            var controller = new SensitiveWordsController(mockRepo.Object);

            // Act: call the action
            var result = await controller.Add("secret");

            // Assert: check the result is OK and value matches
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, okResult.Value);
        }
    }
}

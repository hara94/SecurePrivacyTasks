using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecurePrivacyTask1.Models;
using SecurePrivacyTask1.Repository;

namespace SecurePrivacyTask1.Tests
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IMongoRepository<User>> _repositoryMock;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            // Initialize the Mock Repository
            _repositoryMock = new Mock<IMongoRepository<User>>();
            // Initialize the UsersController with the mocked repository
            _controller = new UsersController(_repositoryMock.Object);
        }

        [Test]
        public async Task GetUsers_ShouldReturnOkResult_WithListOfUsers()
        {
            // Arrange
            var mockUsers = new List<User>
            {
                new User("john_doe", "password123", "john@example.com", "123456789", "123 Main St", "New York"),
                new User("jane_smith", "password456", "jane@example.com", "987654321", "456 Oak St", "San Francisco")
            };

            _repositoryMock.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(mockUsers);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(mockUsers, okResult.Value);
        }

        [Test]
        public async Task GetUserById_UserExists_ShouldReturnOkResult_WithUser()
        {
            // Arrange
            var mockUser = new User("john_doe", "password123", "john@example.com", "123456789", "123 Main St", "New York")
            {
                Id = "507f1f77bcf86cd799439011"
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                           .ReturnsAsync((User?)mockUser); // Cast to nullable User to handle nullability

            // Act
            var result = await _controller.GetUserById(mockUser.Id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(mockUser, okResult.Value);
        }

        [Test]
        public async Task GetUserById_UserDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                           .ReturnsAsync((User?)null); // Return null

            // Act
            var result = await _controller.GetUserById("non-existent-id");

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task CreateUser_ValidUser_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var newUser = new User("new_user", "password123", "new@example.com", "123456789", "789 Elm St", "Chicago");

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateUser(newUser);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.AreEqual(nameof(_controller.GetUserById), createdAtActionResult.ActionName);
            Assert.AreEqual(newUser.Id, createdAtActionResult.RouteValues["id"]);
        }

        [Test]
        public async Task CreateUser_NullUser_ShouldReturnBadRequest()
        {
            // Act
            var result = await _controller.CreateUser(null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateUser_UserExists_ShouldReturnNoContent()
        {
            // Arrange
            var existingUser = new User("john_doe", "password123", "john@example.com", "123456789", "123 Main St", "New York")
            {
                Id = "507f1f77bcf86cd799439011"
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                           .ReturnsAsync((User?)existingUser); // Return existing user

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<User>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateUser(existingUser.Id, existingUser);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task UpdateUser_UserDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                           .ReturnsAsync((User?)null); // Return null for non-existent user

            // Act
            var result = await _controller.UpdateUser("non-existent-id", new User("john_doe", "password123", "john@example.com", "123456789", "123 Main St", "New York"));

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteUser_UserExists_ShouldReturnNoContent()
        {
            // Arrange
            var existingUser = new User("john_doe", "password123", "john@example.com", "123456789", "123 Main St", "New York")
            {
                Id = "507f1f77bcf86cd799439011"
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                           .ReturnsAsync((User?)existingUser); // Return existing user

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(existingUser.Id);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteUser_UserDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                           .ReturnsAsync((User?)null); // Return null for non-existent user

            // Act
            var result = await _controller.DeleteUser("non-existent-id");

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}

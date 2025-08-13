using Microsoft.EntityFrameworkCore;
using Moq;
using PAWScrum.Business.Managers;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Models.DTOs;
using PAWScrum.Repositories.Implementations;

namespace PAWScrum.Tests.Business
{
    [TestClass]
    public class UserBusinessTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private PAWScrumDbContext _dbContext;
        private UserBusiness _userBusiness;

        [TestInitialize]
        public void Initialize()
        {
            // Configurar DbContext en memoria
            var options = new DbContextOptionsBuilder<PAWScrumDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new PAWScrumDbContext(options);
            _mockUserRepository = new Mock<IUserRepository>();

            _userBusiness = new UserBusiness(_mockUserRepository.Object, _dbContext);
        }

        [TestMethod]
        public async Task CreateUserAsync_NewUser_ReturnsTrueAndCreatesUser()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Username = "testuser",
                FirstName = "Test",
                LastName = "User",
                Role = "Developer",
                Password = "Password123!"
            };

            // Configurar mock para simular que no existe el usuario
            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(request.Email))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userBusiness.CreateUserAsync(request);

            // Assert
            Assert.IsTrue(result);

            // Verificar que el usuario se creó en la base de datos
            var createdUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            Assert.IsNotNull(createdUser);
            Assert.AreEqual(request.Username, createdUser.Username);
            Assert.AreEqual(request.FirstName, createdUser.FirstName);
            Assert.AreEqual(request.LastName, createdUser.LastName);
            Assert.AreEqual(request.Role, createdUser.Role);
            Assert.IsNotNull(createdUser.PasswordHash);
        }

        [TestMethod]
        public async Task CreateUserAsync_ExistingEmail_ReturnsFalse()
        {
            // Arrange
            var existingUser = new User
            {
                Email = "existing@example.com",
                Username = "existinguser",
                PasswordHash = "hashedpassword"
            };

            var request = new RegisterRequest
            {
                Email = "existing@example.com",
                Username = "newuser",
                Password = "NewPassword123!"
            };

            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(request.Email))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userBusiness.CreateUserAsync(request);

            // Assert
            Assert.IsFalse(result);

            // Verificar que NO se creó un nuevo usuario
            var newUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == "newuser");
            Assert.IsNull(newUser);
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingUser_ReturnsCorrectUser()
        {
            // Arrange (Preparación)
            var testId = 1;
            var expectedUser = new User
            {
                UserId = testId,
                Email = "testuser@example.com",
                Username = "testuser",
                FirstName = "Test",
                LastName = "User",
                Role = "Developer"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(testId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userBusiness.GetByIdAsync(testId);

            // Assert
            Assert.IsNotNull(result, "Debería retornar un usuario existente");
            Assert.AreEqual(testId, result.UserId, "El ID del usuario debería coincidir");
            Assert.AreEqual(expectedUser.Email, result.Email, "El email debería coincidir");
            Assert.AreEqual(expectedUser.Role, result.Role, "El rol debería coincidir");

            _mockUserRepository.Verify(
                repo => repo.GetByIdAsync(testId),
                Times.Once,
                "El repositorio debería ser llamado exactamente una vez"
            );
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingUser_ReturnsNull()
        {
            // Arrange
            var nonExistingId = 999;

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(nonExistingId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userBusiness.GetByIdAsync(nonExistingId);

            // Assert
            Assert.IsNull(result);
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(nonExistingId), Times.Once);
        }

    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Models.DTOs;
using PAWScrum.Services.Service;
using System.Threading.Tasks;

namespace PAWScrum.Tests.Services
{
    [TestClass]
    public class AuthServiceTests
    {
        private readonly Mock<IUserBusiness> _mockUserBusiness;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserBusiness = new Mock<IUserBusiness>();
            _mockConfiguration = new Mock<IConfiguration>();
            _authService = new AuthService(_mockUserBusiness.Object, _mockConfiguration.Object);
        }

        [TestMethod]
        public async Task LoginAsync_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var validRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "ValidPassword123"
            };

            var expectedUser = new User
            {
                UserId = 1,
                Email = validRequest.Email,
                Username = "testuser"
            };

            _mockUserBusiness.Setup(b => b.ValidateUserCredentialsAsync(validRequest.Email, validRequest.Password))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _authService.LoginAsync(validRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.UserId, result.UserId);
            Assert.AreEqual(expectedUser.Email, result.Email);
        }

        [TestMethod]
        public async Task LoginAsync_EmptyCredentials_ReturnsNull()
        {
            // Arrange
            var invalidRequests = new[]
            {
            // Email vacío
            new LoginRequest { Email = "", Password = "password" },
        
            // Password vacío
            new LoginRequest { Email = "test@example.com", Password = "" },
        
            // Ambos vacíos
            new LoginRequest { Email = "", Password = "" },
        
            // Email nulo
            new LoginRequest { Email = null, Password = "password" },
        
            // Password nulo
            new LoginRequest { Email = "test@example.com", Password = null }
            };

            foreach (var request in invalidRequests)
            {
                // Act
                var result = await _authService.LoginAsync(request);

                // Assert
                Assert.IsNull(result, $"Debería retornar null para: Email='{request.Email}', Password='{request.Password}'");

                // Verificar que NO se llamó al método de validación
                _mockUserBusiness.Verify(
                    b => b.ValidateUserCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Times.Never,
                    $"No debería llamar a ValidateUserCredentials para: Email='{request.Email}', Password='{request.Password}'"
                );
            }
        }

        [TestMethod]
        public async Task RegisterAsync_NewUser_ReturnsTrue()
        {
            // Arrange
            var validRequest = new RegisterRequest
            {
                Email = "newuser@example.com",
                Username = "newuser",
                FirstName = "New",
                LastName = "User",
                Role = "Developer",
                Password = "ValidPassword123"
            };

            // Configurar mock
            _mockUserBusiness.Setup(b => b.CreateUserAsync(validRequest))
                .ReturnsAsync(true);

            // Act
            var result = await _authService.RegisterAsync(validRequest);

            // Assert
            Assert.IsTrue(result, "Debería retornar true para un nuevo usuario");

            // Verificar que se llamó al método de creación
            _mockUserBusiness.Verify(
                b => b.CreateUserAsync(validRequest),
                Times.Once, "Debería llamar a CreateUserAsync con la solicitud correcta"
            );
        }

        [TestMethod]
        public async Task RegisterAsync_ExistingUser_ReturnsFalse()
        {
            // Arrange
            var existingUserRequest = new RegisterRequest
            {
                Email = "existing@example.com",
                Username = "existinguser",
                FirstName = "Existing",
                LastName = "User",
                Role = "Tester",
                Password = "Password123"
            };

            // Configurar el mock para simular usuario existente
            _mockUserBusiness.Setup(b => b.CreateUserAsync(existingUserRequest))
                .ReturnsAsync(false);

            // Act
            var result = await _authService.RegisterAsync(existingUserRequest);

            // Assert
            Assert.IsFalse(result, "Debería retornar false para un usuario existente");

            // Verificar que se llamó al método de creación
            _mockUserBusiness.Verify(
                b => b.CreateUserAsync(existingUserRequest),
                Times.Once,
                "Debería intentar crear el usuario aunque ya exista"
            );
        }
    }
}
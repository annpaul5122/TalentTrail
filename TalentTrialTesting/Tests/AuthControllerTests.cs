using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using TalentTrail.Controllers;
using TalentTrail.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using TalentTrail.Enum;

namespace TalentTrail.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private AuthController _authController;
        private Mock<IConfiguration> _mockConfiguration;
        private TalentTrailDbContext _dbContext;

        [SetUp]
        public void SetUp()
        {
            // Mock configuration setup
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("http://localhost");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("http://localhost");
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("SuperSecretKey123");

            // In-memory database context setup
            var options = new DbContextOptionsBuilder<TalentTrailDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new TalentTrailDbContext(options);

            // Seed data
            _dbContext.Users.AddRange(new List<Users>
            {
                new Users { UserId = 1, Email = "test@test.com", Password = "password123", FirstName = "John", LastName = "Doe", Role = Roles.Admin, CreatedAt = DateTime.Now },
                new Users { UserId = 2, Email = "jane@test.com", Password = "password456", FirstName = "Jane", LastName = "Doe", Role = Roles.JobSeeker, CreatedAt = DateTime.Now }
            });
            _dbContext.SaveChanges();

            // Initialize the AuthController with the in-memory database
            _authController = new AuthController(_mockConfiguration.Object, _dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of the in-memory database context
            _dbContext?.Database.EnsureDeleted();
            _dbContext?.Dispose();
        }

        [Test]
        public void Validate_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var email = "test@test.com";
            var password = "password123";

            // Act
            var result = _authController.Validate(email, password);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
            Assert.AreEqual("John", result.FirstName);
        }

        [Test]
        public void Validate_InvalidPassword_ThrowsArgumentException()
        {
            // Arrange
            var email = "test@test.com";
            var password = "wrongpassword";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _authController.Validate(email, password));
            Assert.AreEqual("Invalid password.", ex.Message);
        }

        [Test]
        public void Validate_UserNotFound_ThrowsArgumentException()
        {
            // Arrange
            var email = "nonexistent@test.com";
            var password = "password123";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _authController.Validate(email, password));
            Assert.AreEqual("User not found with the provided email.", ex.Message);
        }


    
    }
}

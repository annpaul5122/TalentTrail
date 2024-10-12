using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using TalentTrail.Controllers;
using TalentTrail.Services;
using TalentTrail.Dto;
using TalentTrail.Models;
using System.Threading.Tasks;
using TalentTrail.Enum;

namespace TalentTrail.Tests.Controllers
{
    [TestFixture]
    public class EmployersControllerTests
    {
        private Mock<IEmployerProfileService> _mockProfileService;
        private EmployersController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockProfileService = new Mock<IEmployerProfileService>();
            _controller = new EmployersController(_mockProfileService.Object);
        }

        [Test]
        public async Task CreateProfile_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var createDto = new EmployerCreateDto
            {
                UserId = 1,
                JobPosition = JobPosition.HiringManager,
                IsThirdParty = false,
                CompanyName = "TechCorp",
                CompanyWebUrl = "http://techcorp.com",
                CompanyDescription = "A technology company",
                CompanyLogo = "logo.png",
                CompanyAddress = "123 Tech St",
                Industry = "IT"
            };

            var employer = new Employer
            {
                EmployerId = 1,
                UserId = 1
            };

            _mockProfileService
                .Setup(s => s.CreateProfile(It.IsAny<Employer>(), It.IsAny<CompanyDetails>()))
                .ReturnsAsync(employer);

            // Act
            var result = await _controller.CreateProfile(createDto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            // Instead of dynamic, cast to a strongly typed object or directly assert
            var response = result.Value;

            // If you want to check message and values, assert directly using the object type
            Assert.AreEqual("Employer registered successfully.", response.GetType().GetProperty("message")?.GetValue(response));
            Assert.AreEqual(1, response.GetType().GetProperty("employerId")?.GetValue(response));
            Assert.AreEqual(1, response.GetType().GetProperty("userId")?.GetValue(response));
        }

        [Test]
        public async Task CreateProfile_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("UserId", "UserId is required.");

            var createDto = new EmployerCreateDto(); // Invalid input

            // Act
            var result = await _controller.CreateProfile(createDto) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task UpdateProfile_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var employerDto = new EmployerUpdateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                JobPosition = JobPosition.Recruiter,
                IsThirdParty = false,
                CompanyId = 1
            };

            var employer = new Employer
            {
                EmployerId = 1,
                UserId = 1
            };

            _mockProfileService
                .Setup(s => s.UpdateProfile(1, It.IsAny<Employer>()))
                .ReturnsAsync(employer);

            // Act
            var result = await _controller.UpdateProfile(1, employerDto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task UpdateProfile_EmployerNotFound_ReturnsNotFound()
        {
            // Arrange
            var employerDto = new EmployerUpdateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                JobPosition = JobPosition.Recruiter,
                IsThirdParty = false,
                CompanyId = 1
            };

            _mockProfileService
                .Setup(s => s.UpdateProfile(1, It.IsAny<Employer>()))
                .ReturnsAsync((Employer)null); // No employer found

            // Act
            var result = await _controller.UpdateProfile(1, employerDto) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("Employer not found.", result.Value);
        }

        [Test]
        public async Task DeleteProfile_ValidEmployer_ReturnsOkResult()
        {
            // Arrange
            _mockProfileService
                .Setup(s => s.DeleteProfile(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteProfile(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var response = result.Value;
            Assert.AreEqual("Account deleted successfully", response.GetType().GetProperty("message")?.GetValue(response));
        }

        [Test]
        public async Task CheckProfile_ValidUserId_ReturnsProfileExists()
        {
            // Arrange
            var employer = new Employer
            {
                EmployerId = 1,
                UserId = 1
            };

            _mockProfileService
                .Setup(s => s.GetEmployerProfileByUserId(1))
                .ReturnsAsync(employer);

            // Act
            var result = await _controller.CheckProfile(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var response = result.Value;
            Assert.AreEqual(true, response.GetType().GetProperty("exists")?.GetValue(response));
            Assert.AreEqual(1, response.GetType().GetProperty("employerId")?.GetValue(response));
        }

        [Test]
        public async Task CheckProfile_UserProfileNotFound_ReturnsProfileDoesNotExist()
        {
            // Arrange
            _mockProfileService
                .Setup(s => s.GetEmployerProfileByUserId(1))
                .ReturnsAsync((Employer)null); // No profile found

            // Act
            var result = await _controller.CheckProfile(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var response = result.Value;
            Assert.AreEqual(false, response.GetType().GetProperty("exists")?.GetValue(response));
        }
    }
}

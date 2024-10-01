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

            var response = result.Value as dynamic;
            Assert.AreEqual("Employer registered successfully.", response.message);
            Assert.AreEqual(1, response.employerId);
            Assert.AreEqual(1, response.userId);
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

        //[Test]
        //public async Task ViewProfile_ValidEmployer_ReturnsOkResult()
        //{
        //    // Arrange
        //    var employer = new Employer
        //    {
        //        EmployerId = 1,
        //        UserId = 1
        //    };

        //    _mockProfileService
        //        .Setup(s => s.ViewProfile(1))
        //        .Returns(employer);

        //    // Act
        //    var result = await _controller.ViewProfile(1) as OkObjectResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(200, result.StatusCode);
        //}

        //[Test]
        //public async Task ViewProfile_EmployerNotFound_ReturnsNotFound()
        //{
        //    // Arrange
        //    _mockProfileService
        //        .Setup(s => s.ViewProfile(1))
        //        .ReturnsAsync((Employer)null); // No employer found

        //    // Act
        //    var result = await _controller.ViewProfile(1) as NotFoundObjectResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(404, result.StatusCode);
        //    Assert.AreEqual("Employer profile not found", result.Value);
        //}

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
            Assert.AreEqual("Account deleted successfully", ((dynamic)result.Value).message);
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
            Assert.AreEqual(true, ((dynamic)result.Value).exists);
            Assert.AreEqual(1, ((dynamic)result.Value).employerId);
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
            Assert.AreEqual(false, ((dynamic)result.Value).exists);
        }
    }
}

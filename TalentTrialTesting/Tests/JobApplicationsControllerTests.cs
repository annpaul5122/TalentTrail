using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalentTrail.Controllers;
using TalentTrail.Dto;
using TalentTrail.Services;

namespace TalentTrail.Tests.Controllers
{
    [TestFixture]
    public class JobApplicationsControllerTests
    {
        private Mock<IJobApplicationService> _mockJobApplicationService;
        private JobApplicationsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockJobApplicationService = new Mock<IJobApplicationService>();
            _controller = new JobApplicationsController(_mockJobApplicationService.Object);
        }

        //[Test]
        //public async Task CreateJobApplication_ReturnsOkResult_WhenApplicationIsCreated()
        //{
        //    // Arrange
        //    var applyJobDto = new ApplyJobDto { /* Initialize with valid data */ };
        //    var jobApplication = new JobApplicationDto { ApplicationId = 1 };

        //    _mockJobApplicationService
        //        .Setup(s => s.CreateJobApplication(It.IsAny<ApplyJobDto>()))
        //        .ReturnsAsync(jobApplication);

        //    // Act
        //    var result = await _controller.CreateJobApplication(applyJobDto);

        //    // Assert
        //    var okResult = result as OkObjectResult;
        //    Assert.IsNotNull(okResult);
        //    Assert.AreEqual(200, okResult.StatusCode);

        //    var response = okResult.Value;
        //    Assert.AreEqual("Job Application created successfully.", response.GetType().GetProperty("message")?.GetValue(response));
        //    Assert.AreEqual(1, response.GetType().GetProperty("applicationId")?.GetValue(response));
        //}


        [Test]
        public async Task CreateJobApplication_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _controller.CreateJobApplication(new ApplyJobDto());

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task CreateJobApplication_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var applyJobDto = new ApplyJobDto { /* Initialize with valid data */ };
            _mockJobApplicationService.Setup(s => s.CreateJobApplication(applyJobDto))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.CreateJobApplication(applyJobDto);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var response = badRequestResult.Value;
            Assert.AreEqual("Service error", response.GetType().GetProperty("message")?.GetValue(response));
        }

        [Test]
        public async Task GetJobApplicationById_ReturnsOkResult_WhenApplicationExists()
        {
            // Arrange
            var applicationId = 1;
            var jobApplicationDto = new JobApplicationDto { ApplicationId = applicationId };

            _mockJobApplicationService.Setup(s => s.GetJobApplicationById(applicationId))
                .ReturnsAsync(jobApplicationDto);

            // Act
            var result = await _controller.GetJobApplicationById(applicationId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(jobApplicationDto, okResult.Value);
        }

        [Test]
        public async Task GetJobApplicationById_ReturnsNotFound_WhenApplicationDoesNotExist()
        {
            // Arrange
            var applicationId = 1;
            _mockJobApplicationService.Setup(s => s.GetJobApplicationById(applicationId))
                .ThrowsAsync(new Exception("Application not found"));

            // Act
            var result = await _controller.GetJobApplicationById(applicationId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            var response = notFoundResult.Value;
            Assert.AreEqual("Application not found", response.GetType().GetProperty("message")?.GetValue(response));
        }

        [Test]
        public async Task GetAllJobApplicationsByJobSeeker_ReturnsOkResult_WhenApplicationsExist()
        {
            // Arrange
            var seekerId = 1;
            var jobApplicationDtos = new List<JobApplicationDto>
            {
                new JobApplicationDto { ApplicationId = 1 },
                new JobApplicationDto { ApplicationId = 2 }
            };

            _mockJobApplicationService.Setup(s => s.GetAllJobApplicationsByJobSeeker(seekerId))
                .ReturnsAsync(jobApplicationDtos);

            // Act
            var result = await _controller.GetAllJobApplicationsByJobSeeker(seekerId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as List<JobApplicationDto>;
            Assert.AreEqual(2, response.Count);
        }

        [Test]
        public async Task GetAllJobApplicationsByJobSeeker_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var seekerId = 1;
            _mockJobApplicationService.Setup(s => s.GetAllJobApplicationsByJobSeeker(seekerId))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.GetAllJobApplicationsByJobSeeker(seekerId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var response = badRequestResult.Value;
            Assert.AreEqual("Service error", response.GetType().GetProperty("message")?.GetValue(response));
        }

        [Test]
        public async Task DeleteJobApplication_ReturnsNoContent_WhenApplicationIsDeleted()
        {
            // Arrange
            var applicationId = 1;

            _mockJobApplicationService.Setup(s => s.DeleteJobApplication(applicationId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteJobApplication(applicationId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteJobApplication_ReturnsNotFound_WhenApplicationDoesNotExist()
        {
            // Arrange
            var applicationId = 1;
            _mockJobApplicationService.Setup(s => s.DeleteJobApplication(applicationId))
                .ThrowsAsync(new ArgumentException("Application not found"));

            // Act
            var result = await _controller.DeleteJobApplication(applicationId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            var response = notFoundResult.Value;
            Assert.AreEqual("Application not found", response.GetType().GetProperty("message")?.GetValue(response));
        }

        [Test]
        public async Task UpdateJobApplication_ReturnsNoContent_WhenApplicationIsUpdated()
        {
            // Arrange
            var applyJobDto = new ApplyJobDto { /* Initialize with valid data */ };

            _mockJobApplicationService.Setup(s => s.UpdateJobApplication(applyJobDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateJobApplication(applyJobDto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task UpdateJobApplication_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _controller.UpdateJobApplication(new ApplyJobDto());

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task UpdateJobApplication_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var applyJobDto = new ApplyJobDto { /* Initialize with valid data */ };
            _mockJobApplicationService.Setup(s => s.UpdateJobApplication(applyJobDto))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.UpdateJobApplication(applyJobDto);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var response = badRequestResult.Value;
            Assert.AreEqual("Service error", response.GetType().GetProperty("message")?.GetValue(response));
        }
    }
}

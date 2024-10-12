//using Moq;
//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using TalentTrail.Controllers;
//using TalentTrail.Dto;
//using TalentTrail.Models;
//using TalentTrail.Services;
//using Microsoft.AspNetCore.Mvc;
//using System;

//[TestFixture]
//public class JobSeekersControllerTests
//{
//    private Mock<IJobSeekerService> _jobSeekerServiceMock;
//    private JobSeekersController _jobSeekersController;

//    [SetUp]
//    public void SetUp()
//    {
//        _jobSeekerServiceMock = new Mock<IJobSeekerService>();
//        _jobSeekersController = new JobSeekersController(_jobSeekerServiceMock.Object);
//    }

//    [Test]
//    public async Task CreateProfile_ReturnsOk_WhenProfileIsCreated()
//    {
//        // Arrange: Create a profile DTO with CertificationDto
//        var profileDto = new CreateJobSeekerProfileDto
//        {
//            JobSeeker = new JobSeeker { PhoneNumber = "1234567890" },
//            ResumePaths = new List<string> { "resume1.pdf" },
//            Educations = new List<Education>(),
//            Certifications = new List<CertificationDto>
//        {
//            new CertificationDto
//            {
//                CertificationName = "Certification A",
//                CertificatePicturePath = "path/to/certA.jpg",
//                DateIssued = DateTime.Now
//            }
//        }
//        };

//        // Manual mapping from DTO to domain model
//        var certifications = profileDto.Certifications.Select(dto => new Certification
//        {
//            CertificationName = dto.CertificationName,
//            CertificatePicturePath = dto.CertificatePicturePath,
//            DateIssued = dto.DateIssued
//        }).ToList();

//        var createdProfile = new JobSeeker { SeekerId = 1 };

//        // Mock the service to accept JobSeeker and mapped Certifications
//        _jobSeekerServiceMock
//            .Setup(s => s.CreateProfile(
//                It.IsAny<JobSeeker>(),
//                It.IsAny<IEnumerable<string>>(),
//                It.IsAny<IEnumerable<Education>>(),
//                It.IsAny<IEnumerable<Certification>>() // Now mock expects the correct model type
//            ))
//            .ReturnsAsync(createdProfile);

//        // Act: Call the controller method
//        var result = await _jobSeekersController.CreateProfile(profileDto);

//        // Assert: Verify the response
//        Assert.IsInstanceOf<OkObjectResult>(result);
//        var okResult = result as OkObjectResult;
//        Assert.IsNotNull(okResult);
//        Assert.AreEqual(200, okResult.StatusCode);
//    }



//    [Test]
//    public async Task UpdateProfile_ReturnsOk_WhenProfileIsUpdated()
//    {
//        // Arrange
//        int seekerId = 1;
//        var updateDto = new JobSeekerUpdateDto
//        {
//            FirstName = "John",
//            LastName = "Doe",
//            Email = "john.doe@example.com",
//            PhoneNumber = "9876543210",
//            Educations = new List<Education>(),
//            Certifications = new List<Certification>()
//        };

//        var updatedProfile = new JobSeeker { SeekerId = seekerId };

//        _jobSeekerServiceMock
//            .Setup(s => s.UpdateProfile(
//                It.IsAny<int>(),
//                It.IsAny<JobSeeker>(),
//                It.IsAny<IEnumerable<Education>>(),
//                It.IsAny<IEnumerable<Certification>>()))
//            .ReturnsAsync(updatedProfile);

//        // Act
//        var result = await _jobSeekersController.UpdateProfile(seekerId, updateDto);

//        // Assert
//        Assert.IsInstanceOf<OkObjectResult>(result);
//    }

//    [Test]
//    public async Task DeleteProfile_ReturnsOk_WhenProfileIsDeleted()
//    {
//        // Arrange
//        int seekerId = 1;

//        _jobSeekerServiceMock.Setup(s => s.DeleteProfile(seekerId)).Returns(Task.CompletedTask);

//        // Act
//        var result = await _jobSeekersController.DeleteProfile(seekerId);

//        // Assert
//        Assert.IsInstanceOf<OkObjectResult>(result);
//    }

//    [Test]
//    public async Task ViewProfile_ReturnsOk_WhenProfileExists()
//    {
//        // Arrange
//        int seekerId = 1;
//        var profile = new JobSeeker { SeekerId = seekerId };

//        _jobSeekerServiceMock.Setup(s => s.ViewProfile(seekerId)).ReturnsAsync(profile);

//        // Act
//        var result = await _jobSeekersController.ViewProfile(seekerId);

//        // Assert
//        Assert.IsInstanceOf<OkObjectResult>(result);
//    }

//    [Test]
//    public async Task SearchJobPosts_ReturnsOk_WhenJobPostsFound()
//    {
//        // Arrange
//        var jobPosts = new List<JobPostDto> { new JobPostDto { JobTitle = "Software Engineer" } };

//        _jobSeekerServiceMock.Setup(s => s.SearchJobPosts(It.IsAny<string>())).ReturnsAsync(jobPosts);

//        // Act
//        var result = await _jobSeekersController.SearchJobPosts("Software Engineer");

//        // Assert
//        Assert.IsInstanceOf<OkObjectResult>(result);
//    }

//    [Test]
//    public async Task JobPostFilter_ReturnsOk_WhenJobPostsFound()
//    {
//        // Arrange
//        var jobPosts = new List<JobPostDto> { new JobPostDto { JobTitle = "Software Engineer" } };

//        _jobSeekerServiceMock.Setup(s => s.JobPostFilter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<EmploymentType>()))
//            .ReturnsAsync(jobPosts);

//        // Act
//        var result = await _jobSeekersController.JobPostFilter("Software Engineer", "IT", "C#", "Remote", EmploymentType.FullTime);

//        // Assert
//        Assert.IsInstanceOf<OkObjectResult>(result);
//    }

//    [Test]
//    public async Task CheckProfile_ReturnsExistsTrue_WhenProfileExists()
//    {
//        // Arrange
//        int userId = 1;
//        var jobSeeker = new JobSeeker { SeekerId = 1 };

//        _jobSeekerServiceMock.Setup(s => s.GetSeekerProfileByUserId(userId)).ReturnsAsync(jobSeeker);

//        // Act
//        var result = await _jobSeekersController.CheckProfile(userId);

//        // Assert
//        var okResult = result as OkObjectResult;
//        Assert.IsNotNull(okResult);
//        Assert.IsTrue(((dynamic)okResult.Value).exists);
//    }

//    [Test]
//    public async Task GetAppliedJobs_ReturnsOk_WhenJobsFound()
//    {
//        // Arrange
//        int seekerId = 1;
//        var appliedJobs = new List<int> { 101, 102 };

//        _jobSeekerServiceMock.Setup(s => s.GetAppliedJobsAsync(seekerId)).ReturnsAsync(appliedJobs);

//        // Act
//        var result = await _jobSeekersController.GetAppliedJobs(seekerId);

//        // Assert
//        Assert.IsInstanceOf<OkObjectResult>(result);
//    }

//    [Test]
//    public async Task GetAppliedJobs_ReturnsBadRequest_WhenSeekerIdInvalid()
//    {
//        // Arrange
//        int invalidSeekerId = 0;

//        // Act
//        var result = await _jobSeekersController.GetAppliedJobs(invalidSeekerId);

//        // Assert
//        Assert.IsInstanceOf<BadRequestObjectResult>(result);
//    }
//}

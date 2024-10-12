using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TalentTrail.Controllers;
using TalentTrail.Services;
using TalentTrail.Models;
using Microsoft.AspNetCore.Mvc;

[TestFixture]
public class ResumesControllerTests
{
    private Mock<IResumeService> _resumeServiceMock;
    private ResumesController _resumesController;

    [SetUp]
    public void SetUp()
    {
        _resumeServiceMock = new Mock<IResumeService>();
        _resumesController = new ResumesController(_resumeServiceMock.Object);
    }

    [Test]
    public async Task CreateResume_ReturnsOk_WhenResumeIsCreated()
    {
        // Arrange
        var resume = new Resume { ResumeId = 1 };
        _resumeServiceMock.Setup(s => s.CreateResume(It.IsAny<Resume>())).ReturnsAsync(resume);

        // Act
        var result = await _resumesController.CreateResume(resume);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.AreEqual("Resume uploaded successfully.", ((OkObjectResult)result).Value.GetType().GetProperty("message")?.GetValue(((OkObjectResult)result).Value));
    }

    [Test]
    public async Task GetAllResumes_ReturnsOk_WhenResumesAreFound()
    {
        // Arrange
        int seekerId = 1;
        var resumes = new List<Resume> { new Resume { ResumeId = 1 } };
        _resumeServiceMock.Setup(s => s.GetAllResumesOfJobSeeker(seekerId)).ReturnsAsync(resumes);

        // Act
        var result = await _resumesController.GetAllResumes(seekerId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
}

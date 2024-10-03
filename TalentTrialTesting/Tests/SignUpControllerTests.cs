using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TalentTrail.Controllers;
using TalentTrail.Services;
using TalentTrail.Models;
using Microsoft.AspNetCore.Mvc;
using TalentTrail.Dto;
using TalentTrail.Enum;

[TestFixture]
public class SignUpControllerTests
{
    private Mock<ISignUpService> _signUpServiceMock;
    private SignUpController _signUpController;

    [SetUp]
    public void SetUp()
    {
        _signUpServiceMock = new Mock<ISignUpService>();
        _signUpController = new SignUpController(_signUpServiceMock.Object);
    }

    [Test]
    public async Task SignUpEmployer_ReturnsOk_WhenEmployerIsRegistered()
    {
        // Arrange
        var userDto = new UsersDto { FirstName = "Test", LastName = "User", Email = "test@test.com", Password = "pass123" };
        var user = new Users { UserId = 1, FirstName = "Test", Role = Roles.Employer };
        _signUpServiceMock.Setup(s => s.SignUpUserAsync(It.IsAny<Users>())).ReturnsAsync(user);

        // Act
        var result = await _signUpController.SignUpEmployer(userDto);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var response = (OkObjectResult)result;
        Assert.AreEqual("Employer registered successfully.", response.Value.GetType().GetProperty("message")?.GetValue(response.Value));
    }

    [Test]
    public async Task SignUpJobSeeker_ReturnsOk_WhenJobSeekerIsRegistered()
    {
        // Arrange
        var userDto = new UsersDto { FirstName = "Test", LastName = "Seeker", Email = "seeker@test.com", Password = "pass123" };
        var user = new Users { UserId = 1, FirstName = "Test", Role = Roles.JobSeeker };
        _signUpServiceMock.Setup(s => s.SignUpUserAsync(It.IsAny<Users>())).ReturnsAsync(user);

        // Act
        var result = await _signUpController.SignUpJobSeeker(userDto);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
}

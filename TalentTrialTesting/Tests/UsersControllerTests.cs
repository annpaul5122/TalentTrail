using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TalentTrail.Controllers;
using TalentTrail.Services;
using TalentTrail.Models;
using Microsoft.AspNetCore.Mvc;
using TalentTrail.Dto;

[TestFixture]
public class UsersControllerTests
{
    private Mock<IUserService> _userServiceMock;
    private UsersController _usersController;

    [SetUp]
    public void SetUp()
    {
        _userServiceMock = new Mock<IUserService>();
        _usersController = new UsersController(_userServiceMock.Object, null);
    }

    [Test]
    public async Task RequestPasswordReset_ReturnsOk_WhenCalled()
    {
        // Arrange
        var emailRequest = new EmailRequest
        {
            Email = "someemail@gmail.com"
        };

        // Act
        var result = await _usersController.RequestPasswordReset(emailRequest);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.AreEqual("Password reset email sent.", ((OkObjectResult)result).Value);
    }

    [Test]
    public async Task ResetPassword_ReturnsOk_WhenCalled()
    {
        // Arrange
        var resetPasswordRequest = new ResetPassword
        {
            token = "some-token",
            newPassword = "new-password",
            confirmPassword = "new-password"
        };

        // Act
        var result = await _usersController.ResetPassword(resetPasswordRequest);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.AreEqual("Password has been reset.", ((OkObjectResult)result).Value);
    }

    [Test]
    public async Task DeleteUser_ReturnsOk_WhenUserIsDeleted()
    {
        // Arrange
        int userId = 1;
        _userServiceMock.Setup(s => s.DeleteUser(It.IsAny<int>())).Returns(Task.CompletedTask);

        // Act
        var result = await _usersController.DeleteUser(userId) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("User deleted successfully.", result.Value?.GetType().GetProperty("message")?.GetValue(result.Value));
    }

    [Test]
    public async Task UpdateUserDetails_ReturnsOk_WhenUserIsUpdated()
    {
        // Arrange
        var user = new Users { UserId = 1, FirstName = "Test" };
        _userServiceMock.Setup(s => s.UpdateUserDetails(user)).ReturnsAsync(user);

        // Act
        var result = await _usersController.UpdateUserDetails(1, user) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(user, result.Value);
    }
}

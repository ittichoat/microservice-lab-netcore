using AutoMapper;
using BL.Entities;
using BL.Model;
using BL.Services.User;
using DAL.Repository.Interface;
using FluentAssertions;
using Moq;

namespace UnitTest;

public class UserServiceTest
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UserService _userService;

    public UserServiceTest()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _userService = new UserService(_repositoryMock.Object, _mapperMock.Object);
    }

    #region RegisterAsync
    [Fact]
    public async Task RegisterAsync_Should_Fail_When_Username_Exists()
    {
        // Arrange
        var request = new RegisterRequest { Username = "testuser" };
        _repositoryMock.Setup(r => r.GetByUsernameAsync(request.Username))
                       .ReturnsAsync(new User());

        // Act
        var result = await _userService.RegisterAsync(request);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Username already exists.");
    }

    [Fact]
    public async Task RegisterAsync_Should_CreateUser_When_UsernameIsUnique()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "uniqueuser",
            Password = "123456"
        };

        _repositoryMock.Setup(r => r.GetByUsernameAsync(request.Username))
                       .ReturnsAsync((User?)null);

        _mapperMock.Setup(m => m.Map<User>(request))
                   .Returns(new User { Username = request.Username });

        _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                   .Returns(new UserDto { Username = request.Username });

        // Act
        var result = await _userService.RegisterAsync(request);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Username.Should().Be(request.Username);

        _repositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Once);
    }

    #endregion

    #region AuthenticateAsync
    [Fact]
    public async Task AuthenticateAsync_Should_Fail_When_User_Not_Found()
    {
        // Arrange
        var request = new LoginRequest { Username = "unknown", Password = "any" };
        _repositoryMock.Setup(r => r.GetByUsernameAsync(request.Username))
                       .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.AuthenticateAsync(request);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid credentials");
    }

    [Fact]
    public async Task AuthenticateAsync_Should_Fail_When_Password_Incorrect()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpass")
        };

        var request = new LoginRequest { Username = "testuser", Password = "wrongpass" };

        _repositoryMock.Setup(r => r.GetByUsernameAsync(request.Username))
                       .ReturnsAsync(user);

        // Act
        var result = await _userService.AuthenticateAsync(request);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Invalid credentials");
    }

    [Fact]
    public async Task AuthenticateAsync_Should_Succeed_When_Credentials_Are_Correct()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
        };

        var request = new LoginRequest { Username = "testuser", Password = "123456" };

        _repositoryMock.Setup(r => r.GetByUsernameAsync(request.Username))
                       .ReturnsAsync(user);

        _mapperMock.Setup(m => m.Map<UserDto>(user))
                   .Returns(new UserDto { Username = user.Username });

        // Act
        var result = await _userService.AuthenticateAsync(request);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Username.Should().Be("testuser");
    }

    #endregion AuthenticateAsync
}

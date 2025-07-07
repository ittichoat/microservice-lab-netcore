using AutoMapper;
using BL.Entities;
using BL.Model;
using BL.Services;
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

    #region GetAllAsync
    [Fact]
    public async Task GetAllAsync_Should_Return_UserDto_List()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Username = "A" },
            new User { Id = 2, Username = "B" }
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        _mapperMock.Setup(m => m.Map<List<UserDto>>(users))
                   .Returns(new List<UserDto>
                   {
                       new UserDto { Id = 1, Username = "A" },
                       new UserDto { Id = 2, Username = "B" }
                   });

        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0].Username.Should().Be("A");
    }

    #endregion GetAllAsync

    #region GetByIdAsync
    [Fact]
    public async Task GetByIdAsync_Should_Return_UserDto_When_User_Exists()
    {
        // Arrange
        var user = new User { Id = 1, Username = "A" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

        _mapperMock.Setup(m => m.Map<UserDto>(user))
                   .Returns(new UserDto { Id = 1, Username = "A" });

        // Act
        var result = await _userService.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be("A");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    #endregion GetByIdAsync

    #region UpdateAsync
    [Fact]
    public async Task UpdateAsync_Should_Return_Success_When_User_Exists()
    {
        // Arrange
        var user = new User { Id = 1, Username = "A", Email = "old@email.com", PasswordHash = "1das23d1as54f6as41f5as1d85wq964r65as1f" };
        var updated = new UserDto { Id = 1, Username = "A", Email = "new@email.com" };

        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        _repositoryMock.Setup(r => r.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync(true);

        _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(updated);

        var request = new UpdateUserRequest { Email = "new@email.com" };

        // Act
        var result = await _userService.UpdateAsync(1, request);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Email.Should().Be("new@email.com");
    }

    [Fact]
    public async Task UpdateAsync_Should_Fail_When_User_Not_Found()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        var request = new UpdateUserRequest { Email = "new@email.com" };

        // Act
        var result = await _userService.UpdateAsync(99, request);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("User not found");
    }

    #endregion UpdateAsync

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_Should_Return_True_When_User_Deleted()
    {
        // Arrange
        var user = new User { Id = 1, Username = "A" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        _repositoryMock.Setup(r => r.DeleteUserAsync(It.IsAny<User>())).ReturnsAsync(true);

        // Act
        var result = await _userService.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_Return_False_When_User_Not_Found()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.DeleteAsync(99);

        // Assert
        result.Should().BeFalse();
    }

    #endregion DeleteAsync
}

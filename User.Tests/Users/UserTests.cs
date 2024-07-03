using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using User.Application.Users;
using User.Core.Users;
using User.Domain.Users;
using UserServices.Controllers;
using UserServices.Users;

namespace User.Tests.Users
{
    public class UserTests
    {
        IUserService userService;
        UserEmailServiceFake fakeUserEmailService;
        UserPhoneServiceFake fakeUserPhoneService;

        public UserTests()
        {
            fakeUserEmailService = new UserEmailServiceFake();
            fakeUserPhoneService = new UserPhoneServiceFake();
            userService = new UserService(new UserRepository(), new UserHashingService(), 
                new UserMessageService(fakeUserEmailService, fakeUserPhoneService));
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedResult()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "test@example.com",
                PhoneNumber = "+1234567890",
                Password = "12345!Aa"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotEqual(Guid.Empty, createdResult.Value);
        }

        [Fact]
        public async Task Create_InvalidEmail_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "invalid-email",
                PhoneNumber = "+1234567890",
                Password = "password"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid email address.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_EmailExceeds256Characters_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = new string('a', 247) + "@gmail.com",
                PhoneNumber = "+1234567890",
                Password = "password"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid email address.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_PhoneNumberIsNotNineDigits_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "12345678",
                Password = "password"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid phone number.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_PasswordLengthLessThan8Characters_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "+1234567890",
                Password = "Short1!"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Password must be at least 8 characters long.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_PasswordDoesNotContainLowercaseCharacter_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "+1234567890",
                Password = "UPPERCASE123!"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Password must contain at least one lowercase character.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_PasswordDoesNotContainUppercaseCharacter_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "+1234567890",
                Password = "lowercase123!"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Password must contain at least one uppercase character.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_PasswordDoesNotContainTwoDigits_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "+1234567890",
                Password = "Uppercase1!"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Password must contain at least two digits.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_PasswordDoesNotContainSymbol_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "+1234567890",
                Password = "Uppercase12"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Password must contain at least one symbol.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_PhoneNumberWithoutCountryCode_DefaultsToCountryCode46()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "123456789",
                Password = "Uppercase12!"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var returnedResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotEqual(Guid.Empty, returnedResult.Value);

            var user = await userService.GetUserByEmailOrPhoneNumber(request.Email);
            Assert.Equal("+46123456789", user.PhoneNumber.ToString());
        }

        [Fact]
        public async Task Create_PhoneNumberWithJustPlus_DefaultsToCountryCode46()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "+123456789",
                Password = "Uppercase12!"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var returnedResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotEqual(Guid.Empty, returnedResult.Value);

            var user = await userService.GetUserByEmailOrPhoneNumber(request.Email);
            Assert.Equal("+46123456789", user.PhoneNumber.ToString());
        }

        [Fact]
        public async Task Create_PhoneNumberWith00_ReturnsWithPlusPhoneNumber()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "001123456789",
                Password = "Uppercase12!"
            };

            // Act
            var result = await controller.Create(request);

            // Assert
            var returnedResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotEqual(Guid.Empty, returnedResult.Value);

            var user = await userService.GetUserByEmailOrPhoneNumber(request.Email);
            Assert.Equal("+1123456789", user.PhoneNumber.ToString());
        }

        [Fact]
        public async Task GetUserByEmailOrPhoneNumber_ByEmail_ReturnsUser()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "001123456789",
                Password = "Uppercase12!"
            };

            var expected = await controller.Create(request);

            // Act
            var user = await userService.GetUserByEmailOrPhoneNumber(request.Email);

            // Assert
            Assert.Equal(((OkObjectResult)expected).Value, user.Id);
        }

        [Fact]
        public async Task GetUserByEmailOrPhoneNumber_ByPhoneNumber_ReturnsUser()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "001123456789",
                Password = "Uppercase12!"
            };

            var expected = await controller.Create(request);

            // Act
            var user = await userService.GetUserByEmailOrPhoneNumber("+1123456789");

            // Assert
            Assert.Equal(((OkObjectResult)expected).Value, user.Id);
        }

        [Fact]
        public async Task Create_IfUserHasEmail_SendsEmail()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = "valid@example.com",
                PhoneNumber = "001123456789",
                Password = "Uppercase12!"
            };

            // Act
            var actual = await controller.Create(request);

            // Assert
            Assert.True(!String.IsNullOrEmpty(fakeUserEmailService.SendReturn.email.Address));
        }

        [Fact]
        public async Task Create_IfUserHasNotEmail_SendsPhoneNumber()
        {
            // Arrange
            var controller = new UserController(null, userService);

            var request = new CreateUserRequest
            {
                Email = null,
                PhoneNumber = "001123456789",
                Password = "Uppercase12!"
            };

            // Act
            var actual = await controller.Create(request);

            // Assert
            Assert.True(!String.IsNullOrEmpty(fakeUserPhoneService.SendReturn.phoneNumber.ToString()));
        }
    }
}
using System;
using System.Linq;
using CoreNG.Domain.Accounts.Requests;
using Xunit;

namespace TestCoreNG.Ui
{
    public class CreateUserRequestShould: BaseTest
    {
        [Fact]
        public void HaveAKnownDefaultRequestState()
        {
            var request = new CreateUserRequest(_authContext, _userManager);
            Assert.NotNull(request);
            Assert.NotNull(request.Model);
            Assert.NotNull(request.ErrorMessages);
            Assert.Empty(request.ErrorMessages);
            
            Assert.True(string.IsNullOrEmpty(request.Model.Username));
            Assert.True(string.IsNullOrEmpty(request.Model.Password));
            Assert.True(string.IsNullOrEmpty(request.Model.ConfirmPassword));
            Assert.False(request.IsValid);
            Assert.False(request.Validate());
        }

        [Fact]
        public void HaveAKnownDefaultResponseState()
        {
            var response = new CreateUserResponse();
            Assert.NotNull(response);
            Assert.NotNull(response.Model);
            Assert.NotNull(response.ErrorMessages);
            Assert.Empty(response.ErrorMessages);
            
            Assert.True(string.IsNullOrEmpty(response.Model.Username));
            Assert.True(string.IsNullOrEmpty(response.Model.Password));
            Assert.True(string.IsNullOrEmpty(response.Model.ConfirmPassword));
            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public void FailWhenUsernameNotProvided()
        {
            Assert.False(_authContext.Users.Any());
            
            var request = new CreateUserRequest(_authContext, _userManager);
            Assert.NotNull(request);
            Assert.NotNull(request.Model);

            request.Model.Username = null;
            request.Model.Password = "Password123";
            request.Model.ConfirmPassword = request.Model.Password;

            var response = request.Send();
            Assert.NotNull(response);
            Assert.False(response.IsSuccessful);
            Assert.NotNull(response.Model);
            Assert.NotNull(response.ErrorMessages);
            Assert.NotEmpty(response.ErrorMessages);
            Assert.Equal(1, response.ErrorMessages.Count);
            Assert.Equal("Property Username cannot be null or empty", response.ErrorMessages[0]);
        }
        
        [Fact]
        public void FailWhenPasswordNotProvided()
        {
            Assert.False(_authContext.Users.Any());
            
            var request = new CreateUserRequest(_authContext, _userManager);
            Assert.NotNull(request);
            Assert.NotNull(request.Model);

            request.Model.Username = "test";
            request.Model.Password = null;
            request.Model.ConfirmPassword = "password123";

            var response = request.Send();
            Assert.NotNull(response);
            Assert.False(response.IsSuccessful);
            Assert.NotNull(response.Model);
            Assert.NotNull(response.ErrorMessages);
            Assert.NotEmpty(response.ErrorMessages);
            Assert.Equal(1, response.ErrorMessages.Count);
            Assert.Equal("Property Password cannot be null or empty", response.ErrorMessages[0]);
        }
        
        [Fact]
        public void FailWhenConfirmPasswordNotProvided()
        {
            Assert.False(_authContext.Users.Any());
            
            var request = new CreateUserRequest(_authContext, _userManager);
            Assert.NotNull(request);
            Assert.NotNull(request.Model);

            request.Model.Username = "test";
            request.Model.Password = "password123";
            request.Model.ConfirmPassword = null;

            var response = request.Send();
            Assert.NotNull(response);
            Assert.False(response.IsSuccessful);
            Assert.NotNull(response.Model);
            Assert.NotNull(response.ErrorMessages);
            Assert.NotEmpty(response.ErrorMessages);
            Assert.Equal(1, response.ErrorMessages.Count);
            Assert.Equal("Property ConfirmPassword cannot be null or empty", response.ErrorMessages[0]);
        }
        
        [Fact]
        public void FailWhenPasswordsNotMatch()
        {
            Assert.False(_authContext.Users.Any());
            
            var request = new CreateUserRequest(_authContext, _userManager);
            Assert.NotNull(request);
            Assert.NotNull(request.Model);

            request.Model.Username = "test";
            request.Model.Password = "123Password";
            request.Model.ConfirmPassword = "password123";

            var response = request.Send();
            Assert.NotNull(response);
            Assert.False(response.IsSuccessful);
            Assert.NotNull(response.Model);
            Assert.NotNull(response.ErrorMessages);
            Assert.NotEmpty(response.ErrorMessages);
            Assert.Equal(1, response.ErrorMessages.Count);
            Assert.Equal("Password and ConfirmPassword do not match", response.ErrorMessages[0]);
        }
        
        [Fact]
        public void FailWhenPasswordNotComplexEnough()
        {
            Assert.False(_authContext.Users.Any());
            
            var request = new CreateUserRequest(_authContext, _userManager);
            Assert.NotNull(request);
            Assert.NotNull(request.Model);

            request.Model.Username = "test";
            request.Model.Password = "1";
            request.Model.ConfirmPassword = request.Model.Password;

            var response = request.Send();
            Assert.NotNull(response);
            Assert.False(response.IsSuccessful);
            Assert.NotNull(response.Model);
            Assert.NotNull(response.ErrorMessages);
            Assert.NotEmpty(response.ErrorMessages);
            
        }
        
        [Fact]
        public void FailWhenUsernameIsDuplicated()
        {
            Assert.False(_authContext.Users.Any());
            
            var request = new CreateUserRequest(_authContext, _userManager);
            Assert.NotNull(request);
            Assert.NotNull(request.Model);

            request.Model.Username = "test";
            request.Model.Password = "password123";
            request.Model.ConfirmPassword = request.Model.Password;

            var response = request.Send();
            Assert.NotNull(response);
            Assert.True(response.IsSuccessful);
            Assert.NotNull(response.Model);
            Assert.NotNull(response.ErrorMessages);
            Assert.Empty(response.ErrorMessages);
            
            response = request.Send();
            Assert.NotNull(response);
            Assert.False(response.IsSuccessful);
            Assert.NotNull(response.Model);
            Assert.NotNull(response.ErrorMessages);
            Assert.NotEmpty(response.ErrorMessages);
            Assert.Equal(1, response.ErrorMessages.Count);
            Assert.Equal("Record already exists", response.ErrorMessages[0]);

        }
        
        [Fact]
        public void SuccessfullyCreateAUser()
        {
            Assert.False(_authContext.Users.Any());
            
            var request = new CreateUserRequest(_authContext, _userManager);
            Assert.NotNull(request);
            Assert.NotNull(request.Model);

            request.Model.Username = "test";
            request.Model.Password = "Password123";
            request.Model.ConfirmPassword = request.Model.Password;

            var response = request.Send();
            Assert.NotNull(response);
            Assert.True(response.IsSuccessful);
            Assert.NotNull(response.Model);
            Assert.NotNull(response.ErrorMessages);
            Assert.Empty(response.ErrorMessages);
            
        }
    }
}
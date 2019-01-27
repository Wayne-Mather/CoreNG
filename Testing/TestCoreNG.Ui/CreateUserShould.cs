using System;
using System.Linq;
using CoreNG.Domain.Accounts.Requests;
using Xunit;

namespace TestCoreNG.Ui
{
    public class CreateUserRequestShould: BaseTest
    {
        [Fact]
        public void HaveAKnownDefaultState()
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
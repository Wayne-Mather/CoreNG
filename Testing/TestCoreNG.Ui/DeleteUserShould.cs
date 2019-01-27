using System;
using System.Linq;
using CoreNG.Domain.Accounts.Requests;
using Xunit;

namespace TestCoreNG.Ui
{
    public class DeleteUserRequestShould: BaseTest
    {
        [Fact]
        public void HaveAKnownDefaultState()
        {
            var request = new DeleteUserRequest(_authContext, _userManager);
            Assert.NotNull(request);
            Assert.NotNull(request.Model);
            Assert.NotNull(request.ErrorMessages);
            Assert.Empty(request.ErrorMessages);
            
            Assert.True(string.IsNullOrEmpty(request.Model.Username));

            Assert.False(request.IsValid);
            Assert.False(request.Validate());
        }

        [Fact]
        public void HaveAKnownDefaultResponseState()
        {
            var response = new DeleteUserResponse();
            Assert.NotNull(response);
            Assert.NotNull(response.Model);
            Assert.NotNull(response.ErrorMessages);
            Assert.Empty(response.ErrorMessages);
            
            Assert.True(string.IsNullOrEmpty(response.Model.Username));
            Assert.True(response.IsSuccessful);
        }
        
        [Fact]
        public void SuccessfullyDeleteAUser()
        {
            Assert.False(_authContext.Users.Any());

            var createRequest = new CreateUserRequest(_authContext, _userManager);
            createRequest.Model.Username = "test";
            createRequest.Model.Password = "password123";
            createRequest.Model.ConfirmPassword = createRequest.Model.Password;
            var createResponse = createRequest.Send();
            Assert.NotNull(createResponse);
            Assert.True(createResponse.IsSuccessful);

            Assert.True(_authContext.Users.Any());
            
            var request = new DeleteUserRequest(_authContext, _userManager);
            Assert.NotNull(request);
            Assert.NotNull(request.Model);

            request.Model.Username = "test";

            var response = request.Send();
            Assert.NotNull(response);
            Assert.True(response.IsSuccessful);
            Assert.NotNull(response.Model);
            Assert.NotNull(response.ErrorMessages);
            Assert.Empty(response.ErrorMessages);

            Assert.False(_authContext.Users.Any());
        }
    }
}
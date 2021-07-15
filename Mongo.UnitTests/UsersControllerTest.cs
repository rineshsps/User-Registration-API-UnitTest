using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mongo.API.Attribute;
using Mongo.API.Controllers;
using Mongo.Database.Interfaces;
using Mongo.Database.Models;
using Mongo.DTOs;
using Mongo.Services;
using Mongo.Services.Interfaces;
using Mongo.Settings;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace Mongo.UnitTests
{

    public class GenerateDefaultTestData : AutoDataAttribute
    {
        public GenerateDefaultTestData() : base(GetDefaultFixture)
        {

        }
        public static IFixture GetDefaultFixture()
        {
            //var autoMoqCustomization = new AutoMoqCustomization() { ConfigureMembers=true};
            var autoMoqCustomization = new AutoMoqCustomization();

            return new Fixture().Customize(autoMoqCustomization);
        }
    }

    public class UsersControllerTest
    {
        [Theory, GenerateDefaultTestData]
        public void CreateUser_Pass_Test(UserCreateDTO user, IUsersContext db, IEmailServices email, IUserServices _userServices, ILogger<UsersController> logger, IMapper mapper, IJWTServices jWTService)
        {
            var usersController = new UsersController(_userServices, logger, mapper, jWTService);
            var userDTO = new UserCreateDTO
            {
                FullName = "Full Name",
                Password = "password123",
                UserName = "user@mail.com"
            };

            var result = usersController.PostUsers(userDTO);
            var okObject = result as StatusCodeResult;
            Assert.True(okObject.StatusCode == (int)HttpStatusCode.Created);
        }

        [Theory, GenerateDefaultTestData]
        public void CreateUser_Failed_Vaidation_Test(UserCreateDTO user, IUsersContext db, IEmailServices email, IUserServices _userServices, ILogger<UsersController> logger, IMapper mapper, IJWTServices jWTService)
        {
            var usersController = new UsersController(_userServices, logger, mapper, jWTService);
            usersController.ModelState.AddModelError("FullName", "Name is requied filed, Model error");
            var result = usersController.PostUsers(user);
            // Assert

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            Assert.True(badRequestResult.StatusCode == (int)HttpStatusCode.BadRequest);

        }


        [Theory, GenerateDefaultTestData]
        public void Authenticate_Pass_Test(IUsersContext db, IEmailServices email, IUserServices _userServices, ILogger<UsersController> logger, IMapper mapper, IJWTServices jWTService)
        {
            var usersController = new UsersController(_userServices, logger, mapper, jWTService);

            var userDTO = new UserCreateDTO
            {
                FullName = "Full Name",
                Password = "password123",
                UserName = "user@mail.com"
            };

            var authenticate = new AuthenticateDTO
            {
                Password = "password123",
                UserName = "user@mail.com"
            };

            var createUser = usersController.PostUsers(userDTO);
            var result = usersController.Authenticate(authenticate);
            var okObject = result as OkObjectResult;
            var respdata = okObject.Value;
            Assert.NotNull(respdata);
        }


        [Theory, GenerateDefaultTestData]
        public void GetUser_Vaidation_Test(UserCreateDTO user, ClaimsPrincipal claims1, IUsersContext db, IEmailServices email, IUserServices _userServices, ILogger<UsersController> logger, IMapper mapper, IJWTServices jWTService, HttpContext httpContext, IServiceCollection services, IApplicationBuilder app, IWebHostEnvironment env, AuthenticationFilter authentication, AuthorizeAttribute authorize, IPrincipal principal, IClaimsTransformation claimsTransformation)
        {

            if (httpContext.User != null /*&& httpContext.User.Identity.IsAuthenticated*/)
            {
                var claims = new[]
             {
                new Claim("Id", "123"),
                new Claim("UserName", "user.UserName.ToString()"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             };

                var appIdentity = new ClaimsIdentity(claims);
                httpContext.User.AddIdentity(appIdentity);
            }

            var usersController = new UsersController(_userServices, logger, mapper, jWTService);
            usersController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = usersController.GetUser();
            // Assert

            var okObject = result as StatusCodeResult;
            Assert.True(okObject.StatusCode == (int)HttpStatusCode.OK);

        }
    }
}

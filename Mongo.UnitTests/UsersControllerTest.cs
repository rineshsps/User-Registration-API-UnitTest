using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mongo.API.Controllers;
using Mongo.Database.Interfaces;
using Mongo.Database.Models;
using Mongo.DTOs;
using Mongo.Services;
using Mongo.Services.Interfaces;
using Mongo.Settings;
using MongoDB.Driver;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            var autoMoqCustomization = new AutoMoqCustomization() { ConfigureMembers=true};

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
        public void CreateUser_Failed_Vaidation_Test(User user, IUsersContext db, IEmailServices email, IUserServices _userServices, ILogger<UsersController> logger, IMapper mapper, IJWTServices jWTService)
        {
            var usersController = new UsersController(_userServices, logger, mapper, jWTService);

            var userDTO = new UserCreateDTO
            {
                Password = "password123",
                UserName = "user@mail.com"
            };

            usersController.ModelState.AddModelError("FullName", "Name is requied filed, Model error");
            var result = usersController.PostUsers(userDTO);
            var okObject = result as StatusCodeResult;
            Assert.Null(okObject);
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
    }
}

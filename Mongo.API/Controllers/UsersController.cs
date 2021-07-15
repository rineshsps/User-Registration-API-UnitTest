using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Mongo.API.Attribute;
using Mongo.Database.Models;
using Mongo.DTOs;
using Mongo.Services.Interfaces;
using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Mongo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly IJWTServices _jWTService;

        public UsersController(IUserServices userServices, ILogger<UsersController> logger, IMapper mapper, IJWTServices jWTService)
        {
            _userServices = userServices;
            _logger = logger;
            _mapper = mapper;
            this._jWTService = jWTService;
        }

        [Authorize]
        [HttpGet("GetUser")]
        public IActionResult GetUser()
        {
            //try
           // {
                
                var userid = User.FindFirst("UserName").Value;
                var user = _userServices.GetUser(userid);

                var model = _mapper.Map<UserDisplayDTO>(user);

                if (model == null)
                {
                    _logger.LogError($"User with id: {userid}, hasn't been found in database.");
                    return NotFound();
                }
                return Ok(model);
           // }
            //catch (Exception ex)
            //{

            //    _logger.LogError(ex, $"Exception get books api/GetUser");
            //   // await HandleExceptionAsync(httpContext, avEx);


            //    return StatusCode(500);
            //}
        }

        [HttpPost]
        public IActionResult PostUsers(UserCreateDTO user)
        {
           // try
            //{
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<User>(user);
                    var result = _userServices.Create(model);
                    return StatusCode(201);
                }
                else
                {
                    return BadRequest(ModelState.Values);
                }
           // }
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, $"Add User api/User");
            //    return StatusCode(500);
            //}
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate(AuthenticateDTO authenticate)
        {
            try
            {
                var userInfo = _userServices.Authenticate(authenticate);

                if (userInfo != null)
                {
                    var model = _mapper.Map<UserDisplayDTO>(userInfo);
                    var token = _jWTService.GenerateToken(model);

                    var data = new { token = token };
                    return Ok(data);
                }
                else
                {
                    _logger.LogWarning($"Username or password is incorrect");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exeption Authenticate book api/Authenticate");
                return StatusCode(501);
            }
        }
    }
}

using System.Collections.Generic;
using System;
using Pctel.Controllers;
using Xunit;
using Microsoft.Extensions.Logging;
using Entities.DbContexts;
using Entities.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Services;
using Repositories;
using Contracts.Repositories;
using Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using CustomExceptionMiddleware;
using Entities.Models;

namespace UserUnitTest
{
    public class UnitTest1
    {
        public readonly UserController _userController;
        //       private readonly ILogger _logger;
        private readonly UserContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public UnitTest1()
        {
            _configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

            using ServiceProvider services = new ServiceCollection()
                            .AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(_configuration)
                            // -> add your DI needs here
                            .BuildServiceProvider();

            _context = InMemoryContext.InMemorydbContext.userContext();

            IHostBuilder hostBuilder = Host.CreateDefaultBuilder().
           ConfigureLogging((builderContext, loggingBuilder) =>
           {
               loggingBuilder.AddConsole((options) =>
               {
                   options.IncludeScopes = true;
               });
           });
            IHost host = hostBuilder.Build();
            //            _logger = host.Services.GetRequiredService<ILogger<UserController>>();
            _userRepository = new UserRepository(_context);
            _userService = new UserService(_configuration, _userRepository);
            _userController = new UserController(_userService);
        }

        /// <summary>
        ///   To test create user  
        /// </summary>
        [Fact]
        public void CreateUser_OkResult()
        {
            RegisterDto user = new RegisterDto()
            {
                EmailAddress = "ajay2@gmail.com",
                Password = "asdfDD234@#$",
            };

            ActionResult response = _userController.CreateUser(user) as ActionResult;
            Assert.IsType<OkObjectResult>(response);
        }

        /// <summary>
        ///   To test create user with invalid email 
        /// </summary>
        [Fact]
        public void CreateUser_InvalidEmail()
        {
            RegisterDto user = new RegisterDto()
            {
                EmailAddress = "ajaygmail.com",//no @ symbol
                Password = "asdDDf234@#$",
            };

            Action response = () => _userController.CreateUser(user);
            ExceptionModel exception = Record.Exception(response) as ExceptionModel;
            Assert.Equal(400, exception.error.ErrorCode); //password wrong
        }

        /// <summary>
        ///   To test create user with invalid password 
        /// </summary>
        [Fact]
        public void CreateUser_InvalidPassword()
        {
            RegisterDto user = new RegisterDto()
            {
                EmailAddress = "ajay@gmail.com",
                Password = "asdf234@##",//no caps
            };

            Action response = () => _userController.CreateUser(user);
            ExceptionModel exception = Record.Exception(response) as ExceptionModel;
            Assert.Equal(400, exception.error.ErrorCode); //password wrong
        }

        /// <summary>
        ///   To test create user with existing email
        /// </summary>
        [Fact]
        public void CreateUser_EmailExist()
        {
            RegisterDto user = new RegisterDto()
            {
                EmailAddress = "ajay@ajay.live",//existing email
                Password = "asdfDD234@##",
            };

            Action response = () => _userController.CreateUser(user);
            ExceptionModel exception = Record.Exception(response) as ExceptionModel;
            Assert.Equal(409, exception.error.ErrorCode); //password wrong
        }

        /// <summary>
        ///   To test login user with invalid email 
        /// </summary>
        [Fact]
        public void LoginUser_InvalidEmail()
        {
            LoginDto user = new LoginDto()
            {
                EmailAddress = "ajayajay.live",//removed @ symbol
                Password = "asdDDf234@##",
            };

            Action response = () => _userController.LoginUser(user);
            ExceptionModel exception = Record.Exception(response) as ExceptionModel;
            Assert.Equal(400, exception.error.ErrorCode); //password wrong
        }

        /// <summary>
        ///   To test login user with non existing email 
        /// </summary>
        [Fact]
        public void LoginUser_EmailNotFound()
        {
            LoginDto user = new LoginDto()
            {
                EmailAddress = "ajay33@ajay.live",//non existing email
                Password = "asdDDf234@##",
            };

            Action response = () => _userController.LoginUser(user);
            ExceptionModel exception = Record.Exception(response) as ExceptionModel;
            Assert.Equal(404, exception.error.ErrorCode); //password wrong
        }

        /// <summary>
        ///   To test login user with incorrect password
        /// </summary>
        [Fact]
        public void LoginUser_BadPassword()
        {
            LoginDto user = new LoginDto()
            {
                EmailAddress = "ajay@ajay.live",
                Password = "asdeeef234@##",//incorret password
            };

            Action response = () => _userController.LoginUser(user);
            ExceptionModel exception = Record.Exception(response) as ExceptionModel;
            Assert.Equal(403, exception.error.ErrorCode);
        }

        /// <summary>
        ///   To test login user 
        /// </summary>
        [Fact]
        public void LoginUser_OkResult()
        {
            LoginDto user = new LoginDto()
            {
                EmailAddress = "ajay@ajay.live",
                Password = "asdf23@#$SDF",
            };
            ActionResult response = _userController.LoginUser(user) as ActionResult;
            Assert.IsType<OkObjectResult>(response);
        }

        /// <summary>
        ///   To test get all user
        /// </summary>
        [Fact]
        public void GetAllUser()
        {
            ActionResult response = _userController.GetUser() as ActionResult;
            Assert.IsType<OkObjectResult>(response);
            OkObjectResult item = response as OkObjectResult;
            Assert.IsType<List<User>>(item.Value);
        }
    }
}

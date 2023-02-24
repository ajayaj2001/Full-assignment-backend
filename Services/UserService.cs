using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Contracts.Services;
using Contracts.Repositories;
using Entities.Dtos;
using Entities.Models;
using CustomExceptionMiddleware;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private string EmailRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        private string PasswordRegex = @"(?=[A-Za-z0-9@#$%^&+!=]+$)^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%^&+!=])(?=.{8,}).*$";


        public UserService(IConfiguration config, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _config = config;
        }


        ///<summary>
        ///create session token
        ///</summary>
        ///<param name="userData"></param>
        private string GenerateJWTToken(User userData)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSecret:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Claim[] claims = new[] {
         new Claim(JwtRegisteredClaimNames.Sub, userData.Id.ToString()),
         new Claim(JwtRegisteredClaimNames.Sub, userData.EmailAddress),
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            JwtSecurityToken token = new JwtSecurityToken(_config["JwtSecret:Issuer"],
                _config["JwtSecret:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(290),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        ///<summary>
        ///compare password
        ///</summary>
        ///<param name="dbPass"></param>
        ///<param name="userPass"></param>
        private bool ValidatePassword(string userPass, string dbPass)
        {
            string hashedUserPass = CalculateSHA256(userPass);
            return hashedUserPass == dbPass;
        }

        ///<summary>
        ///validate user input for register 
        ///</summary>
        ///<param name="userInput"></param>
        public string ValidateUserInputRegister(RegisterDto userInput)
        {
            if (!Regex.IsMatch(userInput.EmailAddress, EmailRegex, RegexOptions.IgnoreCase))
            {
                throw new ExceptionModel("Enter valid email", "Enter valid email to create an user", 400);
            }

            if (!Regex.IsMatch(userInput.Password, PasswordRegex))
            {
                throw new ExceptionModel("Enter valid password", "password must contain 1 uppercase 1 lowercase 1 symbol min 8 char", 400);
            }

            bool IsEmailExist = _userRepository.IsEmailExist(userInput.EmailAddress);
            if (IsEmailExist)
            {
                throw new ExceptionModel("Email already exist", "Email already used by another person try another email ", 409);
            }

            return CreateUser(userInput);
        }

        ///<summary>
        ///validate user input for login 
        ///</summary>
        ///<param name="userInput"></param>
        public TokenDto ValidateUserInputLogin(LoginDto userInput)
        {
            if (!Regex.IsMatch(userInput.EmailAddress, EmailRegex, RegexOptions.IgnoreCase))
            {
                throw new ExceptionModel("Enter valid email", "Enter valid email to login", 400);
            }

            User userFromRepo = _userRepository.GetUserByEmail(userInput.EmailAddress);
            if (userFromRepo == null)
            {
                throw new ExceptionModel("Email not exist", "User with this email not exist", 404);
            }

            if (!ValidatePassword(userInput.Password, userFromRepo.Password))
            {
                throw new ExceptionModel("Incorrent password", "you have entered wrong password", 403);
            }

            return new TokenDto() { TokenType = "Bearer", AccessToken = GenerateJWTToken(userFromRepo) };
        }

        ///<summary>
        ///hash user password
        ///</summary>
        ///<param name="password"></param>
        private string CalculateSHA256(string password)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(password));

            return Convert.ToBase64String(hashValue);
        }

        ///<summary>
        ///create new user
        ///</summary>
        ///<param name="userInput"></param>
        private string CreateUser(RegisterDto userInput)
        {
            string hashedPassword = CalculateSHA256(userInput.Password);

            Guid userId = Guid.NewGuid();

            User user = new User()
            {
                Id = userId,
                EmailAddress = userInput.EmailAddress,
                Password = hashedPassword,
                IsActive = true
            };

            _userRepository.CreateUser(user);
            return userId.ToString();
        }

        ///<summary>
        ///Get all users
        ///</summary>
        public List<User> GetUser()
        {
            return _userRepository.GetAllUser();
        }

    }
}

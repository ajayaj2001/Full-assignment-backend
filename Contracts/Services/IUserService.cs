using System.Collections.Generic;
using Entities.Models;
using Entities.Dtos;


namespace Contracts.Services
{
    public interface IUserService
    {
        ///<summary>
        ///validate user input for register 
        ///</summary>
        ///<param name="userInput"></param>
        string ValidateUserInputRegister(RegisterDto userInput);

        ///<summary>
        ///validate user input for login 
        ///</summary>
        ///<param name="userInput"></param>
        TokenDto ValidateUserInputLogin(LoginDto userInput);

        ///<summary>
        ///create new user
        ///</summary>
        ///<param name=""></param>
        List<User> GetUser();
    }
}

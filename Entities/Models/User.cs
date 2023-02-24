using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class User : BaseModel
    {
         ///<summary>
        ///email address of user
        ///</summary>
        public string EmailAddress { get; set; }

        ///<summary>
        ///password user
        ///</summary>
        public string Password { get; set; }

    }
}

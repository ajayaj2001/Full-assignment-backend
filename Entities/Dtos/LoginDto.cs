using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Entities.Dtos
{
    [DataContract]
    public class LoginDto
    {
        ///<summary>
        ///email address of user
        ///</summary>
        [Required]
        [DataMember(Name = "email_address")]
        public string EmailAddress { get; set; }

        ///<summary>
        ///password user
        ///</summary>
        [Required]
        [DataMember(Name = "password")]
        public string Password { get; set; }

    }
}

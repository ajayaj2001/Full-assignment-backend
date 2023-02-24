using System.Runtime.Serialization;

namespace Entities.Dtos
{
    [DataContract]
    public class TokenDto
    {
        ///<summary>
        /// token type
        ///</summary>
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        ///<summary>
        /// session token 
        ///</summary>
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

    }
}
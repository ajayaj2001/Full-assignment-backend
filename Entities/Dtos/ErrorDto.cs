using System.Runtime.Serialization;


namespace Entities.Dtos
{
    [DataContract]
    public class ErrorDto
    {
        ///<summary>
        /// detailed error message 
        ///</summary>
        [DataMember(Name = "error_message")]
        public string ErrorMessage { get; set; }

        ///<summary>
        /// detailed error code 
        ///</summary>
        [DataMember(Name = "error_code")]
        public int ErrorCode { get; set; }

        ///<summary>
        /// detailed error type 
        ///</summary>
        [DataMember(Name = "error_description")]
        public string ErrorDescription { get; set; }
    }
}
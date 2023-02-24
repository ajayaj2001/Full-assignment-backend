using System;

namespace Entities.Models
{
    public class BaseModel
    {
        ///<summary>
        ///email address of user
        ///</summary>
        public Guid Id { get; set; }

        ///<summary>
        ///created by user id
        ///</summary>
        public Guid CreatedBy { get; set; }

        ///<summary>
        ///updated by user id
        ///</summary>
        public Guid UpdatedBy { get; set; }

        ///<summary>
        ///created on date
        ///</summary>
        public DateTime CreatedOn { get; set; }

        ///<summary>
        ///updated on date
        ///</summary>
        public DateTime UpdatedOn { get; set; }

        ///<summary>
        ///Is user active
        ///</summary>
        public bool IsActive { get; set; }

    }
}

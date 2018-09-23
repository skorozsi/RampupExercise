using System;
namespace Users.Api.Models
{
    /// <summary>
    /// Represents a User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier of the user
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Full name of the the user
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Date of birth
        /// </summary>
        public DateTime DoB { get; set; }
    }
}
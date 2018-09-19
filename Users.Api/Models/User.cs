using System;
namespace Users.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DoB { get; set; }
    }
}
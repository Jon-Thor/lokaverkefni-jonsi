using System.ComponentModel;
using static System.Net.WebRequestMethods;

namespace ReactProject.Models
{
    public class User
    {
        public int UserId { get; set; } 

        public string UserName { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime CreatedOnDate { get; set; } = DateTime.Now;

        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<TwitterPost> TwitterPosts { get; set;} = new List<TwitterPost>();

        public ICollection<User> Followers { get; set; } = new List<User>();
        public ICollection<User> Following { get; set; } = new List<User>();

    }
}

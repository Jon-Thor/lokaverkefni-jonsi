namespace ReactProject.Models
{
    public class UserDTO
    {
        public UserDTO() { }
        public string UserName { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime CreatedOnDate { get; set; } = DateTime.Now;

        public ICollection<TwitterPost> TwitterPosts { get; set; } = new List<TwitterPost>();

        public List<int> FollowersIDs { get; set; } = new List<int>();
        public List<int> FollowingIDs { get; set; } = new List<int>();
    }
}

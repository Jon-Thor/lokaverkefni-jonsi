using Microsoft.Extensions.Hosting;

namespace ReactProject.Models
{
    public class Like
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int TwitterPostId { get; set; }
        public TwitterPost TwitterPost { get; set; }
    }
}

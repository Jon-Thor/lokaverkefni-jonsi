namespace ReactProject.Models
{
    public class TwitterPost
    {
        public int TwitterPostId { get; set; }

        public string? Text { get; set; }

        public string? ImageURl { get; set; }

        public ICollection<Like> Likes { get; set; } = new List<Like>();

        public int? Shares { get; set; } = 0;

        public DateTime? Date { get; set; }
        public string FormattedCreationDate
        {
            get
            {
                return string.Format("{0:HH:mm : MMMM dd/yyyy}", Date);
            }
        }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}

namespace ReactProject.Models
{
    public class TwitterPostDTO
    {
        public int TwitterPostId { get; set; }

        public string? Text { get; set; }

        public string? ImageURl { get; set; }

        public string UserName { get; set; }

        public string UserImage { get; set; }

        public List<int> LikesNumber { get; set; } = new List<int>();

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

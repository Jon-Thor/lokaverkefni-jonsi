using ReactProject.Models;

namespace ReactProject.Data
{
    public class MockRepository
    {


        List<User> users = new List<User>() {
            new User { UserId = 1, UserName = "Majin", Password = "Test1", Email = "Test1@test.com", ImageUrl = "https://localhost:7167/images/cat.png", CreatedOnDate = DateTime.UtcNow.Date },
            new User { UserId = 2, UserName = "Testy01", Password = "Test2", Email = "Test2@test.com",  CreatedOnDate = DateTime.UtcNow.Date },
            new User { UserId = 3, UserName = "Testy02", Password = "Test3", Email = "Test3@test.com",  CreatedOnDate = DateTime.UtcNow.Date },
            new User { UserId = 4, UserName = "Testy03", Password = "Test4", Email = "Test4@test.com",  CreatedOnDate = DateTime.UtcNow.Date },
        };

        List<TwitterPost> posts = new List<TwitterPost>()
        {
            new TwitterPost {TwitterPostId= 1, Text = "Sample1", UserId = 1},
            new TwitterPost {TwitterPostId= 2, Text = "Sample2", UserId = 1},
            new TwitterPost {TwitterPostId= 3, Text = "Sample1", UserId = 2},
        };

        public List<User> GetAllUsers()
        {
            return users;
        }

        public List<TwitterPost> GetAllPosts() 
        {
            return posts; 
        }


        public User? GetUserById(int id)
        {
            foreach (User user in users) {
            if(user.UserId == id)
                {
                    return user;
                }    
            }

            return null;
        }



    }
}

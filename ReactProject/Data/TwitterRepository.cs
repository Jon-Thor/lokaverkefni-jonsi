
using Microsoft.EntityFrameworkCore;
using ReactProject.Models;

namespace ReactProject.Data
{
    public class TwitterRepository
    {
        private TwitterDbContext _dbContext;
        public TwitterRepository() 
        {
        _dbContext= new TwitterDbContext();
        }

        public List<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public User? GetUserById(int id)
        {
            var user = _dbContext.Users.Where(t => t.UserId == id)
                .Include(x => x.Followers)
                .Include(x => x.Following).FirstOrDefault();
            return user;
        }

        public User? GetUserNamePassword (string username, string password) 
        {
            return _dbContext.Users.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();
        }


        //-----------For Follow User ↓------------------/
        public UserDTO? GetUserDtoById(int id)
        {
            User? user = GetUserById(id);

            if(user == null)
            {
                return null;
            }

            UserDTO userdto = new UserDTO()
            {
                UserName = user.UserName,
                Password = user.Password,
                ImageUrl = user.ImageUrl,
                Email = user.Email,
                FollowersIDs = user.Followers.Select(f => f.UserId).ToList(),
                FollowingIDs = user.Following.Select(f => f.UserId).ToList()
            };


            return userdto;
        }


        public UserDTO ToggleFollowUser(int followerId, int followingId) 
        {
            var follower = GetUserById(followerId);
            var following = GetUserById(followingId);


            if (follower == null || following == null)
            {
                throw new ArgumentException("Follower or following user not found.");
            }
            if(follower.Following.Contains(following) && following.Followers.Contains(follower))
            {
                follower.Following.Remove(following);
                following.Followers.Remove(follower);
            }
            else
            {
                follower.Following.Add(following);
                following.Followers.Add(follower);
            }



            _dbContext.SaveChanges();

            return GetUserDtoById(followerId) ?? throw new InvalidOperationException("An error occurred while converting the User to UserDTO.");

        }

        //-------------------Follow User Function ↑--------------------------//

        public void AddUser(User user) 
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public bool DoesUserExist(string username)
        {
            return _dbContext.Users.Any(u => u.UserName == username);
        }

        public User? UpdateUser(int id, User userFromBody) 
        {
            User? userFromDb = GetUserById(id);

            if (userFromDb == null)
            {
                return null;
            }

            if (userFromBody.UserName != null && userFromBody.UserName != "")
            {
                userFromDb.UserName = userFromBody.UserName;
            }

            if (userFromBody.Password != null && userFromBody.Password != "")
            {
                userFromDb.Password = userFromBody.Password;
            }

            if (userFromBody.Email != null && userFromBody.Email != "")
            {
                userFromDb.Email = userFromBody.Email;
            }

            if (userFromBody.ImageUrl != null)
            {
                userFromDb.ImageUrl = userFromBody.ImageUrl;
            }

            _dbContext.SaveChanges();

            return userFromDb;
        }

        public bool DeleteUser(User user)
        {
            try
            {
                var likesToDelete = _dbContext.Likes.Where(l => l.UserId == user.UserId);

                _dbContext.Likes.RemoveRange(likesToDelete);

                foreach (var followingUser in user.Following.ToList())
                {
                    user.Following.Remove(followingUser);
                }


                foreach (var followerUser in user.Followers.ToList())
                {
                    user.Followers.Remove(followerUser);
                }

                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        public bool DeletePost(TwitterPost post)
        {
            try
            {
                _dbContext.TwitterPosts.Remove(post);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //----Ask Teach About-----//
        public List<TwitterPostDTO> GetAllPosts() {

         return _dbContext.TwitterPosts.Include(l => l.Likes).Select(p => new TwitterPostDTO
         {
             TwitterPostId= p.TwitterPostId,
             Text= p.Text,
             ImageURl= p.ImageURl,
             LikesNumber = p.Likes.Select(p => p.UserId).ToList(),
             Shares = p.Shares,
             Date = p.Date,
             UserId = p.UserId,     
             UserName = p.User.UserName,
             UserImage = p.User.ImageUrl,
         }).ToList();
        }

        //--------Ask About-------//    

        public TwitterPost? GetPostById(int id)
        {
            var post = _dbContext.TwitterPosts.Where(t => t.TwitterPostId == id).FirstOrDefault();
            return post;
        }

        public void CreateTwitterPost(TwitterPost twitterPost) 
        {
            _dbContext.TwitterPosts.Add(twitterPost);
            _dbContext.SaveChanges();
        }

        public bool LikePost(int userId, int twitterPostId)
        {
            try
            {
               
                var existingLike = _dbContext.Likes
                    .FirstOrDefault(l => l.UserId == userId && l.TwitterPostId == twitterPostId);

                if (existingLike != null)
                {
                    
                    _dbContext.Likes.Remove(existingLike);
                }
                else
                {
                   
                    var like = new Like { UserId = userId, TwitterPostId = twitterPostId };
                    _dbContext.Likes.Add(like);
                }

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

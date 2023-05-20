using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactProject.Models;
using ReactProject.Data;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Hosting;

namespace ReactProject.Controllers
{        
    
     
    [ApiController]
    [Route("[controller]")]
    public class TwitterController : ControllerBase
    {

        private TwitterRepository _repo;
        private readonly IWebHostEnvironment _env;

        public TwitterController(IWebHostEnvironment env) 
        { 
           _repo = new TwitterRepository();
            _env = env;
        }  


        [HttpGet]
        [Route("users")]
        public List<User> GetallUsers() 
        {
            return _repo.GetAllUsers();
        }

        [HttpPost]
        [Route("users")]

        public ActionResult<User> Add(User user)
        {
            try
            {
                if (_repo.DoesUserExist(user.UserName))
                {
                    return Conflict("Username already exists");
                }

                _repo.AddUser(user);

                return CreatedAtAction(nameof(GetUserbyID), new { id = user.UserId }, user);
            }
            catch (Exception) {
                return StatusCode(500);
            }

        }

        [HttpPost]
        [Route("posts")]
        public ActionResult<TwitterPost> CreatePost([FromForm] TwitterPost post, IFormFile? image)
        {
            try
            {
                if(image != null)
                {
                    var filename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filepath = Path.Combine(_env.WebRootPath, "images", filename);
                    using (var stream = new FileStream(filepath, FileMode.Create))


                    {
                        image.CopyTo(stream);
                    }

                    post.ImageURl = $"https://localhost:7167/images/{filename}";
                }

                _repo.CreateTwitterPost(post);

                return CreatedAtAction(nameof(GetPostById), new { id = post.TwitterPostId }, post);

            }

            catch (Exception)
            {
                return StatusCode(500);
            }

        }

        [HttpGet]
        [Route("users/{id}")]
        public ActionResult<UserDTO> GetUserbyID (int id)
        {
            User? user = _repo.GetUserById(id);

            if(user == null)
            {
                return NotFound();
            }

            UserDTO userdto = new UserDTO() {
                UserName = user.UserName,
                Password = user.Password,
                ImageUrl = user.ImageUrl,
                Email = user.Email,
                FollowersIDs = user.Followers.Select(f => f.UserId).ToList(),
                FollowingIDs = user.Following.Select(f => f.UserId).ToList() };

            return userdto;
        }

        [HttpGet]
        [Route("posts/{id}")]
        public ActionResult<TwitterPost> GetPostById (int id)
        {
            TwitterPost? post = _repo.GetPostById(id); 
            if(post == null) 
            {
            return NotFound();
            }

            return post;

        }

        //-------------------Follow User Function ↓--------------------------//

        [HttpPost]
        [Route("users/{followerId}/follow/{followingId}")]
        public ActionResult<UserDTO> FollowUser(int followerId, int followingId)
        {
            try
            {
                var follower = _repo.ToggleFollowUser(followerId, followingId);
                return Ok(follower);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message); // Log error message
                return NotFound("Follower or following user not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message); // Log any other error messages
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        //-------------------Follow User Function ↑--------------------------//

        [HttpPatch]
        [Route("users/{id}")]
        public IActionResult UpdateUser(int id, [FromForm] UserUpdateModel userUpdateModel)
        {
            if (id != userUpdateModel.UserId)
            {
                return BadRequest();
            }
            try
            {

                User userFromBody = new User
                {
                    UserId = userUpdateModel.UserId ?? 0,
                    UserName = userUpdateModel.UserName,
                    Password = userUpdateModel.Password,
                    Email = userUpdateModel.Email,
                    ImageUrl = null,
                };

                if (userUpdateModel.Image != null)
                {
                    var filename = Guid.NewGuid().ToString() + Path.GetExtension(userUpdateModel.Image.FileName);
                    var filepath = Path.Combine(_env.WebRootPath, "images", filename);
                    using (var stream = new FileStream(filepath, FileMode.Create))


                    {
                        userUpdateModel.Image.CopyTo(stream);
                    }

                        userFromBody.ImageUrl = $"https://localhost:7167/images/{filename}";
                }

                User? updated = _repo.UpdateUser(id, userFromBody);

                if(updated == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("users/{id}")]
        public ActionResult DeleteUserId(int id) {

            try {
                
                User? user = _repo.GetUserById(id);
            
            if(user == null)
                {
                    return NotFound();
                }

            bool success = _repo.DeleteUser(user);

                if (!success)
                {
                    return StatusCode(500);
                }

                return Ok();
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpDelete]
        [Route("posts/{id}")]

        public ActionResult DeletePost(int id) {
        
            try
            {
                TwitterPost? post = _repo.GetPostById(id);

                if(post == null)
                {
                    return NotFound();
                }

                bool success = _repo.DeletePost(post);

                if (!success)
                {
                    return StatusCode(500);
                }

                return Ok();

            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("posts")]

        public List<TwitterPostDTO> Getposts()
        {
            return _repo.GetAllPosts();
        }

        [HttpPost]
        [Route("{userid}/like-post/{twitterPostId}")]
        public IActionResult LikePost(int userId, int twitterPostId)
        {
            bool success = _repo.LikePost(userId, twitterPostId);

            if (success)
            {
                return Ok(new { Message = "Post liked successfully" });
            }
            else
            {
                return BadRequest(new { Message = "Unable to like the post" });
            }
        }

        [HttpPost]
        [Route("users/login")]
public IActionResult Login([FromBody] LoginData data) 
{
    try { 
        var user = _repo.GetUserNamePassword(data.Username, data.Password);

        if(user == null)
        {
            return Unauthorized();
        }

        return Ok(user);
    }
    catch(Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
        

    }
}

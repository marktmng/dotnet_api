using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public PostController(IConfiguration config) //  constructor injection with IConfiguration || dependency injection
        {
            _dapper = new DataContextDapper(config);
        }

        // [HttpGet("Posts/{postId}/{userId}/{searchParam}")]
        // public IEnumerable<Post> GetPosts(int postId = 0, int userId = 0, string searchParam = "None")
        // {
        //     string sql = @"EXEC TutorialAppSchema.spPosts_Get";
        //     string parameters = ""; // parameters to pass to sql query

        //     if (postId != 0)
        //     {
        //         parameters += ", @PostId = " + postId.ToString();
        //     }
        //     if (userId != 0)
        //     {
        //         parameters += ", @UserId = " + userId.ToString();
        //     }
        //     if (searchParam != "None")
        //     {
        //         parameters += ", @SearchValue = '" + searchParam + "'";
        //     }

        //     if (parameters.Length > 0)
        //     {
        //         sql += parameters.Substring(1); // remove first character from parameters
        //     }

        //     return _dapper.LoadData<Post>(sql);
        // }

        [HttpGet("Posts/{postId:int}/{userId:int}/{searchParam}")] // refactored GetPosts
        public IEnumerable<Post> GetPosts(int postId = 0, int userId = 0, string searchParam = "None")
        {
            // Create a new list of strings to hold the SQL parameters based on the provided conditions.
            var parameters = new List<string>
            {
                postId != 0 ? $"@PostId = {postId}" : null,  // Include PostId if it's not 0
                userId != 0 ? $"@UserId = {userId}" : null,  // Include UserId if it's not 0
                searchParam.ToLower() != "none" ? $"@SearchValue = '{searchParam}'" : null // Include SearchValue if it's not "None"
            }.Where(p => p != null); // Remove any nulls from the list
            // .ToList(); // Convert to a list

            string sql = "EXEC TutorialAppSchema.spPosts_Get";
            // If there are any valid parameters, join them into a single string separated by commas
            // and append them to the SQL EXEC command.
            if (parameters.Any())
            {
                sql += " " + string.Join(", ", parameters);
            }

            return _dapper.LoadData<Post>(sql);
        }

        // [HttpGet("PostSingle/{postId}")]
        // public Post GetPostSingle(int postId)
        // {
        //     string sql = @"SELECT [UserId],
        //         [UserId],
        //         [PostTitle],
        //         [PostContent],
        //         [PostCreated],
        //     [PostUpdated] FROM TutorialAppSchema.Posts
        //         WHERE PostId =" + postId.ToString();
        //     return _dapper.LoadDataSingle<Post>(sql);
        // }

        // [HttpGet("PostsByUser/{userId}")]
        // public IEnumerable<Post> GetPostsByUser(int userId)
        // {
        //     string sql = @"SELECT [UserId],
        //         [UserId],
        //         [PostTitle],
        //         [PostContent],
        //         [PostCreated],
        //     [PostUpdated] FROM TutorialAppSchema.Posts
        //         WHERE UserId =" + userId.ToString();
        //     return _dapper.LoadData<Post>(sql);
        // }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get @UserId = " +
            this.User.FindFirst("userId")?.Value;

            return _dapper.LoadData<Post>(sql);
        }

        // search posts
        // [HttpGet("PostsBySearch/{searchParam}")]
        // public IEnumerable<Post> PostsBySearch(string searchParam)
        // {
        //     string sql = @"SELECT [UserId],
        //         [UserId],
        //         [PostTitle],
        //         [PostContent],
        //         [PostCreated],
        //     [PostUpdated] FROM TutorialAppSchema.Posts
        //         WHERE PostTitle LIKE '%" + searchParam + "%'" +
        //             " OR PostContent LIKE '%" + searchParam + "%'";

        //     return _dapper.LoadData<Post>(sql);
        // }

        [HttpPut("UpsertPost")]
        public IActionResult UpsertPost(Post postToUpsert)
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Upsert
                @UserId =" + this.User.FindFirst("userId")?.Value +
                @", @PostTitle ='" + postToUpsert.PostTitle +
                @"', @PostContent ='" + postToUpsert.PostContent + "'";

            if (postToUpsert.PostId != 0) // if postId is not 0, then add it to the sql query
            {
                sql += ", @PostId = " + postToUpsert.PostId;
            }

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to create new post!");

        }
        // [AllowAnonymous]
        // [HttpPut("Post")]
        // public IActionResult EditPost(PostToEditDto postToEdit)
        // {
        //     string sql = @"
        //     UPDATE TutorialAppSchema.Posts 
        //         SET PostContent = '" + postToEdit.PostContent +
        //         "', PostTitle = '" + postToEdit.PostTitle +
        //             @"', PostUpdated = GETDATE()
        //             WHERE PostId = " + postToEdit.PostId.ToString() +
        //             "AND  UserId = " + this.User.FindFirst("userId")?.Value; // to check if the same user id is updating

        //     if (_dapper.ExecuteSql(sql))
        //     {
        //         return Ok();
        //     }
        //     throw new Exception("Failed to edit new post!");

        // }

        [HttpDelete("Post/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string sql = @"EXEC TutorialAppSchema.spPost_Delete @PostId = " +
            postId.ToString() +
            ",  @UserId = " + this.User.FindFirst("userId")?.Value;


            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to delete post!");
        }
    }
}
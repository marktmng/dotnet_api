using DotnetAPI;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_API.Controllers;

[ApiController] // api controller that gives access to api
[Route("[controller]")] // route

// class created UserController
public class UserController : ControllerBase // created endpoint user before controller
{
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
        // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
    }

    [HttpGet("TestConnection")] // get method
    public DateTime TestConnection() // created method
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers")] // endpoint to get all users
    public IEnumerable<User> GetUsers() // arguement   || Users[]

    { // copied from sql query to pass it to dapper
        string sql = @"
        SELECT [UserId],
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active]
        FROM TutorialAppSchema.Users";

        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
        // return new string[] { "user1", "user2", "user3" };
        // string[] responseArray = new string[] {
        // "test1",
        // "test2",
        // "test3",
        // testValue
        // };
        // return responseArray;
    }

    [HttpGet("GetSingleUser/{UserId}")] // endpoint for getting single user
    public User GetSingleUser(int UserId) // arguement

    {
        string sql = @"
        SELECT [UserId],
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active]
        FROM TutorialAppSchema.Users
            WHERE UserId = " + UserId.ToString(); // exaple; "1"

        User user = _dapper.LoadDataSingle<User>(sql);
        return user;
    }
}

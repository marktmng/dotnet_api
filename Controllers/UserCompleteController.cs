using DotnetAPI;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController] // api controller that gives access to api
[Route("[controller]")] // route

// class created UserController
public class UserCompleteController : ControllerBase // created endpoint user before controller
{
    DataContextDapper _dapper;

    public UserCompleteController(IConfiguration config)
    {
        // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetUsers/{UserId}/{isActive}")] // endpoint to get all users
    public IEnumerable<UserComplete> GetUsers(int UserId, bool isActive) // arguement   || Users[]

    { // copied from sql query to pass it to dapper
        string sql = @"EXEC TutorialAppSchema.spUsers_Get";
        string parameters = ""; // parameters to pass to sql query
        if (UserId != 0) // if userId is not 0, then add it to the sql query
        {
            parameters += ", @UserId = " + UserId.ToString();
        }
        if (UserId != 0) // if userId is not 0, then add it to the sql query
        {
            parameters += ", @Active = " + isActive.ToString();
        }

        sql += parameters.Substring(1); // remove first character from parameters

        // Console.WriteLine(sql); //, parameters.Length  // print sql to check error

        IEnumerable<UserComplete> users = _dapper.LoadData<UserComplete>(sql);
        return users;
    }

    // [HttpGet("GetSingleUser/{UserId}")] // endpoint for getting single user
    // public User GetSingleUser(int UserId) // arguement

    // {
    //     string sql = @"
    //     SELECT [UserId],
    //         [FirstName],
    //         [LastName],
    //         [Email],
    //         [Gender],
    //         [Active]
    //     FROM TutorialAppSchema.Users
    //         WHERE UserId = " + UserId.ToString();

    //     User user = _dapper.LoadDataSingle<User>(sql);
    //     return user;
    // }

    [HttpPut("UpsertUser")] // endpoint to edit user
    public IActionResult UpsertEditUser(UserComplete user)
    {

        string sql = @"EXEC TutorialAppSchema.spUser_Upsert
            @FirstName = '" + user.FirstName +
            "', @LastName = '" + user.LastName +
            "', @Email = '" + user.Email +
            "', @Gender = '" + user.Gender +
            "', @Active = '" + user.Active +
            "', @JobTitle = '" + user.Active +
            "', @Department = '" + user.Active +
            "', @Salary = '" + user.Active +
            "', @UserId = " + user.UserId; // put some space befor WHERE

        // Console.WriteLine(sql); // print sql to check error


        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update user"); // exception
    }

    // [HttpPost("AddUser")] // endpoint to add user
    // public IActionResult AddUser(UserToAddDto user)
    // {
    //     string sql = @"INSERT INTO TutorialAppSchema.Users(
    //             [FirstName],
    //             [LastName],
    //             [Email],
    //             [Gender],
    //             [Active] 
    //         ) VALUES (
    //         '" + user.FirstName +
    //         "', '" + user.LastName +
    //         "', '" + user.Email +
    //         "', '" + user.Gender +
    //         "', '" + user.Active +
    //         "')";

    //     Console.WriteLine(sql); // print sql to check error

    //     if (_dapper.ExecuteSql(sql))
    //     {
    //         return Ok();
    //     }

    //     throw new Exception("Failed to add user"); // exception
    // }

    [HttpDelete("DeleteUser/{UserId}")] // endpoint to delete user
    public IActionResult DeleteUser(int UserId)
    {
        string sql = @"EXEC TutorialAppSchema.spUser_Delete 
        @UserId = " + UserId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete user"); // exception

    }
}

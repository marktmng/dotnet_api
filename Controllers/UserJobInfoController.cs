using DotnetAPI;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController] // api controller that gives access to api
[Route("[controller]")] // route

// class created UserController
public class UserJobInfoController : ControllerBase // created endpoint user before controller
{
    DataContextDapper _dapper;

    public UserJobInfoController(IConfiguration config)
    {
        // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
        _dapper = new DataContextDapper(config);
    }

    // [HttpGet("GetUserJobInfo/{UserId}")] // endpoint for getting single user
    // public UserJobInfo GetUserJobInfo(int UserId) // arguement

    // {
    //     string sql = @"
    //     SELECT [UserId],
    //     [JobTitle],
    //     [Department] FROM TutorialAppSchema.UserJobInfo
    //         WHERE UserId = " + UserId.ToString(); // exaple; "1"

    //     UserJobInfo userjobinfo = _dapper.LoadDataSingle<UserJobInfo>(sql);
    //     return userjobinfo;
    // }

    // [HttpPost("AddUserJobInfo")] // endpoint to add user
    // public IActionResult AddUserJobInfo(UserJobInfo user)
    // {
    //     string sql = @"INSERT INTO TutorialAppSchema.UserJobInfo(
    //             [JobTitle],
    //             [Department] 
    //         ) VALUES (
    //         '" + user.JobTitle +
    //         "', '" + user.Department +
    //         "')";

    //     Console.WriteLine(sql); // print sql to check error

    //     if (_dapper.ExecuteSql(sql))
    //     {
    //         return Ok();
    //     }

    //     throw new Exception("Failed to add user Job Information"); // exception
    // }

    // [HttpPut("PutUserJobInfo")] // endpoint to edit user
    // public IActionResult PutUserJobInfo(UserJobInfo userjobinfo)
    // {
    //     string sql = @"
    //     UPDATE TutorialAppSchema.UserJobInfo
    //         SET [JobTitle] = '" + userjobinfo.JobTitle +
    //         "', [Department] = '" + userjobinfo.Department + // add single ''
    //         "' WHERE UserId = " + userjobinfo.UserId; // put some space befor WHERE

    //     Console.WriteLine(sql); // print sql to check error


    //     if (_dapper.ExecuteSql(sql))
    //     {
    //         return Ok();
    //     }

    //     throw new Exception("Failed to update user Job Information"); // exception
    // }

    // [HttpDelete("DeleteUserJobInfo/{UserId}")] // endpoint to delete user
    // public IActionResult DeleteUserJobInfo(int UserId)
    // {
    //     string sql = @"
    //     DELETE FROM TutorialAppSchema.UserJobInfo WHERE UserId = " + UserId.ToString();

    //     Console.WriteLine(sql); // print sql to check error

    //     if (_dapper.ExecuteSql(sql))
    //     {
    //         return Ok();
    //     }

    //     throw new Exception("Failed to delete user Job Information"); // exception

    // }
}

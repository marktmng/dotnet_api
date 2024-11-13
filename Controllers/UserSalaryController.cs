using DotnetAPI;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController] // api controller that gives access to api
[Route("[controller]")] // route

// class created UserController
public class UserSalaryController : ControllerBase // created endpoint user before controller
{
    DataContextDapper _dapper;

    public UserSalaryController(IConfiguration config)
    {
        // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetUserSalary")] // endpoint to get all users
    public IEnumerable<UserSalary> GetUserSalary() // arguement   || Users[]

    { // copied from sql query to pass it to dapper
        string sql = @"
        SELECT [UserId],
        [Salary],
        [AvgSalary] FROM TutorialAppSchema.UserSalary";

        IEnumerable<UserSalary> usersalaries = _dapper.LoadData<UserSalary>(sql);
        return usersalaries;
    }

    [HttpGet("GetSingleUser/{UserId}")] // endpoint for getting single user
    public UserSalary GetSingleUser(int UserId) // arguement

    {
        string sql = @"
        SELECT [UserId],
        [Salary],
        [AvgSalary] FROM TutorialAppSchema.UserSalary
            WHERE UserId = " + UserId.ToString(); // exaple; "1"

        UserSalary usersalary = _dapper.LoadDataSingle<UserSalary>(sql);
        return usersalary;
    }

    [HttpPut("EditUser")] // endpoint to edit user
    public IActionResult EditUser(UserSalary usersalary)
    {
        string sql = @"
        UPDATE TutorialAppSchema.UserSalary
            SET [Salary] = '" + usersalary.Salary +
            "', [AvgSalary] = '" + usersalary.AvgSalary + // add single ''
            "' WHERE UserId = " + usersalary.UserId; // put some space befor WHERE

        Console.WriteLine(sql); // print sql to check error


        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update user"); // exception
    }

    [HttpPost("AddUser")] // endpoint to add user
    public IActionResult AddUser(UserSalary user)
    {
        string sql = @"INSERT INTO TutorialAppSchema.UserSalary(
                [Salary],
                [AvgSalary] 
            ) VALUES (
            '" + user.Salary +
            "', '" + user.AvgSalary +
            "')";

        Console.WriteLine(sql); // print sql to check error

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to add user"); // exception
    }

    [HttpDelete("DeleteUser/{UserId}")] // endpoint to delete user
    public IActionResult DeleteUser(int UserId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.UserSalary WHERE UserId = " + UserId.ToString();

        Console.WriteLine(sql); // print sql to check error

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete user"); // exception

    }
}

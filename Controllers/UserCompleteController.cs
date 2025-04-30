using System.Data;
using Dapper;
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
        string stringParameters = ""; // parameters to pass to sql query
        DynamicParameters sqlParameters = new DynamicParameters(); // create dynamic parameters

        if (UserId != 0) // if userId is not 0, then add it to the sql query
        {
            stringParameters += ", @UserId=@UserIdParameter ";
            sqlParameters.Add("@UserIdParameter", UserId, DbType.Int32); // add userId to the sql parameters
        }
        if (isActive) // if is Active is true, then add it to the sql query
        {
            stringParameters += ", @Active=@ActiveParameter";
            sqlParameters.Add("@ActiveParameter", UserId, DbType.Boolean);
        }
        if (!string.IsNullOrEmpty(stringParameters)) // if string parameters is not empty, then add it to the sql query
        {
            sql += stringParameters.Substring(1); // add string parameters to the sql query
        }

        IEnumerable<UserComplete> users = _dapper.LoadDataWithParameters<UserComplete>(sql, sqlParameters);
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
            @FirstName = @FirstNameParameter,
            @LastName = @LastNameParameter, 
            @Email = @EmailParameter,
            @Gender = @GenderParameter,
            @Active = @ActiveParameter,
            @JobTitle = @JobTitleParameter,
            @Department = @DepartmentParameter,
            @Salary = @SalaryParameter,
            @UserId = @UserIdParameter";

        DynamicParameters sqlParameters = new DynamicParameters();

        sqlParameters.Add("@FirstNameParameter", user.FirstName, DbType.String);
        sqlParameters.Add("@LastNameParameter", user.LastName, DbType.String);
        sqlParameters.Add("@EmailParameter", user.Email, DbType.String);
        sqlParameters.Add("@GenderParameter", user.Gender, DbType.String);
        sqlParameters.Add("@ActiveParameter", user.Active, DbType.Boolean);
        sqlParameters.Add("@JobTitleParameter", user.JobTitle, DbType.String);
        sqlParameters.Add("@DepartmentParameter", user.Department, DbType.String);
        sqlParameters.Add("@SalaryParameter", user.Salary, DbType.Decimal);
        sqlParameters.Add("@UserIdParameter", user.UserId, DbType.Int32);


        if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
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
        @UserId = @UserIdParameter";

        DynamicParameters sqlParameters = new DynamicParameters(); // create dynamic parameters
        sqlParameters.Add("@UserIdParameter", UserId, DbType.Int32);
        if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters)) // execute sql with parameters
        {
            return Ok();
        }

        throw new Exception("Failed to delete user"); // exception

    }
}

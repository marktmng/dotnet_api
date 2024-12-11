using AutoMapper;
using DotnetAPI;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController] // api controller that gives access to api
[Route("[controller]")] // route

// class created UserController
public class UserSalaryEFController : ControllerBase // created endpoint user before controller
{
    DataContextEF _entityFramework;
    IMapper _mapper; // created automaper to map dto to model

    public UserSalaryEFController(IConfiguration config)
    {
        // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
        _entityFramework = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserSalaryDto, UserSalary>()
        ));
    }

    [HttpGet("GetUserSalary/{userId}")] // endpoint for getting single user  || for EF userId instead of UserId
    public UserSalary GetSingleUser(int userId) // arguement

    {
        UserSalary? userSalary = _entityFramework.UserSalaries // Query the database using Entity Framework to find the UserSalary record
            .Where(u => u.UserId == userId) // where the UserId matches the provided userId.
            .FirstOrDefault<UserSalary>(); // The result is either the matching UserSalary record or null (because of "?")

        if (userSalary != null)
        {
            return userSalary;
        }
        throw new Exception("User Salary not found");
    }

    // add user
    [HttpPost("PostUserSalary")]
    public IActionResult PostUserSalary(UserSalaryDto userSalary) // to map with dto
    {
        UserSalary userSalarydb = _mapper
        .Map<UserSalary>(userSalary); // use automapper to map dto to model
        _entityFramework.Add(userSalarydb); // calling entity framework to add user salary

        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Falied to add user salary");
    }

    // update user salary
    [HttpPut("UpdateUserSalary")]
    public IActionResult UpdateUserSalary(UserSalary userSalary)
    {
        UserSalary? userSalarydb = _entityFramework.UserSalaries
            .Where(u => u.UserId == userSalary.UserId)
            .FirstOrDefault<UserSalary>();

        if (userSalarydb != null)
        {
            userSalarydb.Salary = userSalary.Salary;
            userSalarydb.AvgSalary = userSalary.AvgSalary;

            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
        }
        throw new Exception("Failed to update user salary");
    }

    // delete user salary
    [HttpDelete("DeleteUserSalary/{UserId}")] // endpoint to delete user
    public IActionResult DeleteUserSalary(int UserId)
    {
        // try to find the user with the given UserId in the database.
        UserSalary? userSalarydb = _entityFramework.UserSalaries
            .Where(u => u.UserId == UserId)
            .FirstOrDefault<UserSalary>();

        if (userSalarydb != null) // check if the user exists
        {
            _entityFramework.Remove(userSalarydb); // if the user is found remove it
            if (_entityFramework.SaveChanges() > 0)// Check if the deletion was successful.
            {
                return Ok();
            }
        }
        throw new Exception("Failed to delete user salary");
    }
}

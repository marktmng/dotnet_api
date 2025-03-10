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
    // DataContextEF _entityFramework;

    IUserRepository _userRepository;
    IMapper _mapper; // created automaper to map dto to model

    public UserSalaryEFController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
        // _entityFramework = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserSalaryDto, UserSalary>()
        ));
    }

    [HttpGet("GetUserSalary/{userId}")] // endpoint for getting single user  || for EF userId instead of UserId
    public UserSalary GetSingleUserSalary(int userId) // arguement
    {
        return _userRepository.GetUserSalary(userId);
    }

    // add user
    [HttpPost("PostUserSalary")]
    public IActionResult PostUserSalary(UserSalaryDto userSalary) // to map with dto
    {
        UserSalary userSalarydb = _mapper
        .Map<UserSalary>(userSalary); // use automapper to map dto to model

        _userRepository.AddEntity<UserSalary>(userSalarydb); // calling entity framework to add user salary
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Falied to add user salary");
    }

    // update user salary
    [HttpPut("UpdateUserSalary")]
    public IActionResult UpdateUserSalary(UserSalary userSalaryUpdate)
    {
        UserSalary? userSalarydb = _userRepository.GetUserSalary(userSalaryUpdate.UserId);

        if (userSalarydb != null)
        {
            userSalarydb.Salary = userSalaryUpdate.Salary;
            userSalarydb.AvgSalary = userSalaryUpdate.AvgSalary;

            if (_userRepository.SaveChanges())
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
        UserSalary? userSalarydb = _userRepository.GetUserSalary(UserId);

        if (userSalarydb != null) // check if the user exists
        {
            _userRepository.RemoveEntity<UserSalary>(userSalarydb); // if the user is found remove it
            if (_userRepository.SaveChanges())// Check if the deletion was successful.
            {
                return Ok();
            }
        }
        throw new Exception("Failed to delete user salary");
    }
}

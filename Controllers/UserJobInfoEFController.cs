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
public class UserJobInfoEFController : ControllerBase // created endpoint user before controller
{
    // DataContextEF _entityFramework;
    IUserRepository _userRepository;
    IMapper _mapper; // created automaper to map dto to model

    public UserJobInfoEFController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
        // _entityFramework = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserJobInfoDto, UserJobInfo>()
        ));
    }

    [HttpGet("GetUserJobInfo/{userId}")] // endpoint for getting single user  || for EF userId instead of UserId
    public UserJobInfo GetUserJobInfo(int userId) // arguement
    {
        UserJobInfo? userJobInfo = _userRepository.GetUserJobInfo(userId);

        if (userJobInfo != null)
        {
            return userJobInfo;
        }
        throw new Exception("User Job Information not found");
    }

    [HttpPost("AddUserJobInfo")] // endpoint to add user
    public IActionResult AddUserJobInfo(UserJobInfoDto userJobInfo) // to map with dto
    {
        UserJobInfo userJobInfodb = _mapper
        .Map<UserJobInfo>(userJobInfo);

        _userRepository.AddEntity<UserJobInfo>(userJobInfodb); // calling entity framework to add user job info
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception();
    }

    // put user job info
    // [HttpPut("UpdateUserJobInfo")]
    [HttpPut("UpdateUserJobInfo")]
    public IActionResult UpdateUserJobInfo(UserJobInfo userJobInfoUpdate)
    {
        UserJobInfo? userJobInfodb = _userRepository.GetUserJobInfo(userJobInfoUpdate.UserId);

        if (userJobInfodb != null)
        {
            userJobInfodb.JobTitle = userJobInfoUpdate.JobTitle;
            userJobInfodb.Department = userJobInfoUpdate.Department;

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
        }
        throw new Exception("Failed to update user salary");
    }

    // delete user Job Info
    [HttpDelete("DeleteUserJobInfo/{UserId}")] // endpoint to delete user
    public IActionResult DeleteUserJobInfo(int UserId)
    {
        // try to find the user with the given UserId in the database.
        UserJobInfo? userJobInfodb = _userRepository.GetUserJobInfo(UserId);

        if (userJobInfodb != null) // check if the user exists
        {
            _userRepository.RemoveEntity<UserJobInfo>(userJobInfodb); // if the user is found remove it
            if (_userRepository.SaveChanges())// Check if the deletion was successful.
            {
                return Ok();
            }
        }
        throw new Exception("Failed to delete user salary");
    }

}

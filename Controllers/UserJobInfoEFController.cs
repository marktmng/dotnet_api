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
    DataContextEF _entityFramework;
    IMapper _mapper; // created automaper to map dto to model

    public UserJobInfoEFController(IConfiguration config)
    {
        // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
        _entityFramework = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserJobInfoDto, UserJobInfo>()
        ));
    }

    [HttpGet("GetUserJobInfo/{userId}")] // endpoint for getting single user  || for EF userId instead of UserId
    public UserJobInfo GetUserJobInfo(int userId) // arguement
    {
        UserJobInfo? userJobInfo = _entityFramework.UserJobInfos // query the database using entity framework to find the UserJobInfo record
            .Where(x => x.UserId == userId) // where the UserId matches the provided userId
            .FirstOrDefault<UserJobInfo>();

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
        _entityFramework.Add(userJobInfodb); // calling entity framework to add user job info

        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception();
    }

    // put user job info
    // [HttpPut("UpdateUserJobInfo")]
    [HttpPut("UpdateUserJobInfo")]
    public IActionResult UpdateUserJobInfo(UserJobInfo userJobInfo)
    {
        UserJobInfo? userJobInfodb = _entityFramework.UserJobInfos
            .Where(u => u.UserId == userJobInfo.UserId)
            .FirstOrDefault<UserJobInfo>();

        if (userJobInfodb != null)
        {
            userJobInfodb.JobTitle = userJobInfo.JobTitle;
            userJobInfodb.Department = userJobInfo.Department;

            if (_entityFramework.SaveChanges() > 0)
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
        UserJobInfo? userJobInfodb = _entityFramework.UserJobInfos
            .Where(u => u.UserId == UserId)
            .FirstOrDefault<UserJobInfo>();

        if (userJobInfodb != null) // check if the user exists
        {
            _entityFramework.Remove(userJobInfodb); // if the user is found remove it
            if (_entityFramework.SaveChanges() > 0)// Check if the deletion was successful.
            {
                return Ok();
            }
        }
        throw new Exception("Failed to delete user salary");
    }

}

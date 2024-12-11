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
}

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
public class UserEFController : ControllerBase // created endpoint user before controller
{
    DataContextEF _entityFramework;
    IMapper _mapper; // created automaper to map dto to model

    public UserEFController(IConfiguration config)
    {
        // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
        _entityFramework = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserToAddDto, User>()
        ));
    }

    [HttpGet("GetUsers")] // endpoint to get all users
    public IEnumerable<User> GetUsers() // arguement   || Users[]

    {
        IEnumerable<User> users = _entityFramework.Users.ToList<User>(); // use entity framework to get List by using DataContextEF
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

    [HttpGet("GetSingleUser/{userId}")] // endpoint for getting single user  || for EF userId instead of UserId
    public User GetSingleUser(int userId) // arguement

    {
        User? user = _entityFramework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<User>();

        if (user != null)
        {
            return user;
        }
        throw new Exception("User not found");
    }

    [HttpPut("EditUser")] // endpoint to edit user
    public IActionResult EditUser(User user)
    {
        User? userDb = _entityFramework.Users
            .Where(u => u.UserId == user.UserId)
            .FirstOrDefault<User>();

        if (userDb != null)
        {
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            userDb.Active = user.Active;
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to update user"); // exception
        }
        throw new Exception("User not found");
    }

    [HttpPost("AddUser")] // endpoint to add user
    public IActionResult AddUser(UserToAddDto user)
    {
        // User userDb = new User();
        User userdb = _mapper.Map<User>(user); // use automapper to map dto to model

        // userDb.FirstName = user.FirstName;
        // userDb.LastName = user.LastName;
        // userDb.Email = user.Email;
        // userDb.Gender = user.Gender;
        // userDb.Active = user.Active;

        // _entityFramework.Add(userDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to add user"); // exception
    }

    [HttpDelete("DeleteUser/{UserId}")] // endpoint to delete user
    public IActionResult DeleteUser(int UserId)
    {
        User? userDb = _entityFramework.Users
            .Where(u => u.UserId == UserId)
            .FirstOrDefault<User>();

        if (userDb != null)
        {
            _entityFramework.Remove(userDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to delete user"); // exception
        }
        throw new Exception("User not found");

    }
}

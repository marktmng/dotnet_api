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
    // DataContextEF _entityFramework; // decalre entity framework

    IUserRepository _userRepository;
    IMapper _mapper; // created automaper to map dto to model

    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {

        _userRepository = userRepository;
        // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
        // _entityFramework = new DataContextEF(config); // set the value of entity framework
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserToAddDto, User>()
        ));
    }

    // [HttpGet("GetUsers")] // endpoint to get all users
    // public IEnumerable<User> GetUsers() // arguement   || Users[]

    // {
    //     IEnumerable<User> users = _entityFramework.Users.ToList<User>(); // use entity framework to get List by using DataContextEF
    //     return users;
    //     // return new string[] { "user1", "user2", "user3" };
    //     // string[] responseArray = new string[] {
    //     // "test1",
    //     // "test2",
    //     // "test3",
    //     // testValue
    //     // };
    //     // return responseArray;
    // }

    [HttpGet("GetUsers")] // endpoint to get all users
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _userRepository.GetUsers(); // use entity framework to get List by using DataContextEF
        return users;
    }
    [HttpGet("GetSingleUser/{userId}")] // endpoint for getting single user  || for EF userId instead of UserId
    public User GetSingleUser(int userId) // arguement

    {
        // User? user = _entityFramework.Users
        //     .Where(u => u.UserId == userId)
        //     .FirstOrDefault<User>();

        // if (user != null)
        // {
        //     return user;
        // }
        // throw new Exception("User not found");
        return _userRepository.GetSingleUser(userId);
    }

    [HttpPut("EditUser")] // endpoint to edit user
    public IActionResult EditUser(User user)
    {
        // User? userDb = _entityFramework.Users
        //     .Where(u => u.UserId == user.UserId)
        //     .FirstOrDefault<User>();
        User? userDb = _userRepository.GetSingleUser(user.UserId); // get single user

        if (userDb != null)
        {
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            userDb.Active = user.Active;
            if (_userRepository.SaveChanges()) // changing "_entityFramework.SaveChanges() > 0" to repository
            {
                return Ok();
            }

            // throw new Exception("Failed to update user"); // exception
        }
        throw new Exception("Failed to update user"); // exception
    }

    [HttpPost("AddUser")] // endpoint to add user
    public IActionResult AddUser(UserToAddDto user) // to map with dto
    {
        // User userDb = new User();
        User userdb = _mapper
        .Map<User>(user); // use automapper to map dto to model

        // userDb.FirstName = user.FirstName;
        // userDb.LastName = user.LastName;
        // userDb.Email = user.Email;
        // userDb.Gender = user.Gender;
        // userDb.Active = user.Active;

        _userRepository.AddEntity<User>(userdb); // to add user
        if (_userRepository.SaveChanges()) // changing "_entityFramework.SaveChanges() > 0" to repository
        {
            return Ok();
        }

        throw new Exception("Failed to add user"); // exception
    }

    [HttpDelete("DeleteUser/{UserId}")] // endpoint to delete user
    public IActionResult DeleteUser(int UserId)
    {
        // Try to find a user with the given UserId in the database.
        // The "?" after "User" means that the userDb variable can be null if no user is found.
        User? userDb = _userRepository.GetSingleUser(UserId);

        if (userDb != null) // check if the user exists
        {
            _userRepository.RemoveEntity<User>(userDb); // if the user is found remove it
            if (_userRepository.SaveChanges())// Check if the deletion was successful.
            {
                return Ok();
            }

            // throw new Exception("Failed to delete user"); // exception
        }

        throw new Exception("Failed to delete user"); // exception

    }
}

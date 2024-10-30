using Microsoft.AspNetCore.Mvc;

namespace Dotnet_API.Controllers;

[ApiController] // api controller that gives access to api
[Route("[controller]")] // route

// class created UserController
public class UserController : ControllerBase // created endpoint user before controller
{
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
        // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
    }

    [HttpGet("Test Connection")] // get method
    public DateTime TestConnection() // created method
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }


    [HttpGet("test/{testValue}")] // set this up as an explicit "test/testValue" instead of implicit "test"
    public string[] Test(string testValue) // arguement

    {
        string[] responseArray = new string[] {
        "test1",
        "test2",
        "test3",
        testValue
        };
        return responseArray;
    }
}

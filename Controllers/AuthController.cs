using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers
{
    [Authorize] // authorize attribute to make sure user is authorized
    [ApiController]
    [Route("[controller]")] // route to controller
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;

        public AuthController(IConfiguration config) //  constructor injection with IConfiguration || dependency injection
        {
            // _config = config;
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
        }

        [AllowAnonymous] // allow anonymous attribute to allow user to register
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUserExists = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '" +
                    userForRegistration.Email + "'";

                IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);
                if (existingUsers.Count() == 0)
                {
                    UserForLoginDto userForSetPassword = new UserForLoginDto()
                    {
                        Email = userForRegistration.Email,
                        Password = userForRegistration.Password
                    };
                    if (_authHelper.SetPassword(userForSetPassword))
                    {
                        string sqlAddUser = @"EXEC TutorialAppSchema.spUser_Upsert
                            @FirstName = '" + userForRegistration.FirstName +
                            "', @LastName = '" + userForRegistration.LastName +
                            "', @Email = '" + userForRegistration.Email +
                            "', @Gender = '" + userForRegistration.Gender +
                            "', @Active = 1" +
                            ", @JobTitle = '" + userForRegistration.JobTitle +
                            "', @Department = '" + userForRegistration.Department +
                            "', @Salary = '" + userForRegistration.Salary + "'";
                        // string sqlAddUser = @"INSERT INTO TutorialAppSchema.Users(
                        //         [FirstName],
                        //         [LastName],
                        //         [Email],
                        //         [Gender],
                        //         [Active] 
                        //     ) VALUES (
                        //     '" + userForRegistration.FirstName +
                        //     "', '" + userForRegistration.LastName +
                        //     "', '" + userForRegistration.Email +
                        //     "', '" + userForRegistration.Gender +
                        //     "', 1)";
                        if (_dapper.ExecuteSql(sqlAddUser))
                        {
                            return Ok();
                        }
                        throw new Exception("Failed to register user to database!");
                    }
                    throw new Exception("Failed to register user to database!");
                }
                throw new Exception("User already exists with this email!");
            }
            throw new Exception("Passwords do not match!");

        }

        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(UserForLoginDto userForSetPassword)
        {

            if (_authHelper.SetPassword(userForSetPassword))
            {
                return Ok();
            }
            throw new Exception("Failed to update password!");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            string sqlForHashAndSalt = @"EXEC TutorialAppSchema.spLoginConfirmation_Get 
            @Email = @EmailParam";

            DynamicParameters sqlParameters = new DynamicParameters();

            // SqlParameter emailParam = new SqlParameter("@EmailParam", SqlDbType.VarChar);
            // emailParam.Value = userForLogin.Email; // set the value of the parameter
            // sqlParameters.Add(emailParam);

            sqlParameters.Add("EmailParam", userForLogin.Email, DbType.String); // add the parameter to the dynamic parameters

            UserForLoginConfirmationDto userForLoginConfirmation = _dapper
                .LoadDataSingleWithParameters<UserForLoginConfirmationDto>(sqlForHashAndSalt, sqlParameters);

            byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForLoginConfirmation.PasswordSalt);

            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userForLoginConfirmation.PasswordHash[index])
                {
                    return StatusCode(401, "Invalid password!"); // instead of throwing an exception used StatusCode
                }
            }

            //                                  or

            // →→→→→
            // Compare the password hash with the stored hash using SequenceEqual.
            // SequenceEqual checks if both byte arrays are identical element by element.
            // If they do not match, return a 401 Unauthorized status with an "Invalid password!" message.
            // if (!passwordHash.SequenceEqual(userForLoginConfirmation.PasswordHash))
            // {
            //     return StatusCode(401, "Invalid password!");
            // }


            string userIdSql = @"
        SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" +
                userForLogin.Email + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string>{
                {"token", _authHelper.CreateToken(userId)}
            });
        }

        // refresh token method
        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            // string userId = User.FindFirst("userId")?.Value + "";

            string userIdSql = "SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = " +
            User.FindFirst("userId")?.Value + "";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return _authHelper.CreateToken(userId);
        }
    }
}



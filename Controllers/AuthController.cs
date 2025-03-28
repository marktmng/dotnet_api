using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
        // private readonly IConfiguration _config;
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
                    byte[] passwordSalt = new byte[128 / 8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);

                    string sqlAddAuth = @"INSERT INTO TutorialAppSchema.Auth ([Email],
                        [PasswordHash],
                        [PasswordSalt]) VALUES ('" + userForRegistration.Email +
                        "', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParam = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParam.Value = passwordSalt;

                    SqlParameter PasswordHashParam = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    PasswordHashParam.Value = passwordHash;

                    sqlParameters.Add(passwordSaltParam);
                    sqlParameters.Add(PasswordHashParam);

                    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        string sqlAddUser = @"INSERT INTO TutorialAppSchema.Users(
                                [FirstName],
                                [LastName],
                                [Email],
                                [Gender],
                                [Active] 
                            ) VALUES (
                            '" + userForRegistration.FirstName +
                            "', '" + userForRegistration.LastName +
                            "', '" + userForRegistration.Email +
                            "', '" + userForRegistration.Gender +
                            "', 1)";
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

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            string sqlForHashAndSalt = @"SELECT 
        [PasswordHash],
        [PasswordSalt]
        FROM TutorialAppSchema.Auth WHERE Email = '" +
                userForLogin.Email + "'";
            UserForLoginConfirmationDto userForLoginConfirmation = _dapper
                .LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);

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



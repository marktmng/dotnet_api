using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
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
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
            _dapper = new DataContextDapper(config);
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

                    byte[] passwordHash = GetPasswordHash(userForRegistration.Password, passwordSalt);

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

            byte[] passwordHash = GetPasswordHash(userForLogin.Password, userForLoginConfirmation.PasswordSalt);

            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userForLoginConfirmation.PasswordHash[index])
                {
                    return StatusCode(401, "Invalid password!"); // instead of throwing an exception used StatusCode
                }
            }

            string userIdSql = @"
        SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" +
                userForLogin.Email + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string>{
                {"token", CreateToken(userId)}
            });
        }

        // refresh token method
        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string userId = User.FindFirst("userId")?.Value + "";

            string userIdSql = "SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = " +
            userId;

            int userIdFromDb = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string>{
                {"token", CreateToken(userIdFromDb)}
            });
        }
        private byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            string passwordSaltString = _config.GetSection("Appsettings:PasswordKey").Value +
                        Convert.ToBase64String(passwordSalt);

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            );
        }

        private string CreateToken(int userId) // helper method to create token
        {
            Claim[] claims = new[]
            {
                new Claim("userId", userId.ToString()) // converting userId to string
            };

            string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;

            // break down to look at each part
            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey( // symmetric key and passing to new symmetric key 
                Encoding.UTF8.GetBytes( // and passing to new symmetric key byte array
                    tokenKeyString != null ? tokenKeyString : ""
                    )
                ); // out of our token key from appsettings

            // now we need to create signing credentials
            SigningCredentials credentials = new SigningCredentials( // signing credentials is uusing our token key
                tokenKey,
                SecurityAlgorithms.HmacSha256Signature
                );

            // now we need to create token descriptor
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            // now we need to create a token handler
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler(); // we need import using Microsoft.IdentityModel.Tokens;

            SecurityToken token = tokenHandler.CreateToken(descriptor); // creating a token and this is our actual token

            return tokenHandler.WriteToken(token); // writing token to make universal
        }

    }
}



using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _config; // private readonly field
        public AuthHelper(IConfiguration config) // constructor
        {
            _config = config; // setting config to _config
        }
        public byte[] GetPasswordHash(string password, byte[] passwordSalt)
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

        public string CreateToken(int userId) // helper method to create token
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
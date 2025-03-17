namespace DotnetAPI.Dtos
{
    public class UserForLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserForLoginDto()
        {
            // constructor
            Email = Email ?? ""; // if email is null then set it to empty string
            Password = Password ?? ""; // if password is null then set it to empty string
        }
    }
}
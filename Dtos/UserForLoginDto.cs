namespace DotnetAPI.Dtos
{
    partial class UserForLoginDto
    {
        string Email { get; set; }
        string Password { get; set; }

        public UserForLoginDto()
        {
            // constructor
            Email = Email ?? ""; // if email is null then set it to empty string
            Password = Password ?? ""; // if password is null then set it to empty string
        }
    }
}
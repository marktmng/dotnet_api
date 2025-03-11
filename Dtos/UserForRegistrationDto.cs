namespace DotnetAPI.Dtos
{
    partial class UserForRegistrationDto
    {
        string Email { get; set; }
        string Password { get; set; }
        string PasswordConfirm { get; set; }

        public UserForRegistrationDto()
        {
            // constructor
            Email = Email ?? ""; // if email is null then set it to empty string
            Password = Password ?? ""; // if password is null then set it to empty string
            PasswordConfirm = PasswordConfirm ?? ""; // if password confirm is null then set it to empty string
        }
    }
}
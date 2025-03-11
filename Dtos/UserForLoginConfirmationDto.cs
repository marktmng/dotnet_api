namespace DotnetAPI.Dtos
{
    public class UserForLoginConfirmationDto
    {
        byte[] PasswordHash { get; set; }
        byte[] PasswordSalt { get; set; }

        UserForLoginConfirmationDto()
        {
            PasswordHash = PasswordHash ?? new byte[0];
            PasswordSalt = PasswordSalt ?? new byte[0];
        }
    }
}
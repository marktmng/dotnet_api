namespace DotnetAPI.Dtos
{
    public class UserForLoginConfirmationDto
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        UserForLoginConfirmationDto()
        {
            PasswordHash = PasswordHash ?? new byte[0];
            PasswordSalt = PasswordSalt ?? new byte[0];
        }
    }
}
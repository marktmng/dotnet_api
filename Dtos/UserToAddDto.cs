namespace DotnetAPI.Dtos
{
    public partial class UserToAddDto // transfered from User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool Active { get; set; }

        // construc
        public UserToAddDto()
        {
            // if first name is null, set it to an empty string.
            if (FirstName == null)
            {
                FirstName = "";
            }
            // if last name is null, set it to an empty string.
            if (LastName == null)
            {
                LastName = "";
            }
            // if email is null, set it to an empty string.
            if (Email == null)
            {
                Email = "";
            }
            // if gender is null, set it to an empty string.
            if (Gender == null)
            {
                Gender = "";
            }
        }
    }
}
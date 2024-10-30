namespace DotnetAPI
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool Active { get; set; }

        // construc
        public Users()
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
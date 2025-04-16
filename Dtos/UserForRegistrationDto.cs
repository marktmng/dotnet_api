namespace DotnetAPI.Dtos
{
    public class UserForRegistrationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }


        public UserForRegistrationDto()
        {
            // constructor
            FirstName = FirstName ?? ""; // if first name is null then set it to empty string
            LastName = LastName ?? ""; // if last name is null then set it to empty string
            Gender = Gender ?? ""; // if gender is null then set it to empty string
            Email = Email ?? ""; // if email is null then set it to empty string
            Password = Password ?? ""; // if password is null then set it to empty string
            PasswordConfirm = PasswordConfirm ?? ""; // if password confirm is null then set it to empty string
            JobTitle = JobTitle ?? ""; // if job title is null then set it to empty string
            Department = Department ?? ""; // if department is null then set it to empty string
        }
    }
}


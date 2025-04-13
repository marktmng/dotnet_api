namespace DotnetAPI.Models
{
    public partial class UserComplete
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool Active { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public decimal AvgSalary { get; set; }


        // constructor
        public UserComplete()
        {
            FirstName = FirstName ?? ""; // if first name is null, set it to an empty string.
            LastName = LastName ?? "";// if last name is null, set it to an empty string.
            Email = Email ?? ""; // if email is null, set it to an empty string.
            Gender = Gender ?? ""; // if gender is null, set it to an empty string.
            JobTitle = JobTitle ?? ""; // if job title is null, set it to an empty string.
            Department = Department ?? ""; // if department is null, set it to an empty string.
        }
    }
}
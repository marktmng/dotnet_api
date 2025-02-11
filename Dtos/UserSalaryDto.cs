namespace DotnetAPI.Dtos
{
    public partial class UserSalaryDto
    {
        public int UserId { get; set;}
        public decimal Salary { get; set; }  // Use decimal or the correct type
        public decimal AvgSalary { get; set; }  // Use decimal or the correct type

        // Constructor
        public UserSalaryDto()
        {
            // If Salary is null, set it to 0.0m (appropriate default for decimal)
            if (Salary == null)
            {
                Salary = 0.0m;
            }

            // If AvgSalary is null, set it to 0.0m
            if (AvgSalary == null)
            {
                AvgSalary = 0.0m;
            }
        }
    }
}

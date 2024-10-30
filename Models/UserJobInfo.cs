namespace DotnetAPI
{
    public partial class UserJobInfo
    {
        public int UserId { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }

        // construc
        public UserJobInfo()
        {
            // if job title is null, set it to an empty string.
            if (JobTitle == null)
            {
                JobTitle = "";
            }
            // if Department is null, set it to an empty string.
            if (Department == null)
            {
                Department = "";
            }
        }
    }
}
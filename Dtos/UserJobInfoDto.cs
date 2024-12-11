namespace DotnetAPI.Dtos
{

    public partial class UserJobInfoDto
    {
        public string JobTitle { get; set; }
        public string Department { get; set; }

        public UserJobInfoDto()
        {
            if (JobTitle == null) // if job title is empty set it empty string
            {
                JobTitle = string.Empty;
            }

            if (Department == null) // if department is empty set it empty string
            {
                Department = string.Empty;
            }
        }
    }
}
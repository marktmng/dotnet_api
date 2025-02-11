namespace DotnetAPI.Dtos
{

    public partial class UserJobInfoDto
    {
        public int UserId { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }

        public UserJobInfoDto()
        {
            JobTitle ??= ""; // if job title is null, set it to an empty string
            Department ??= ""; // if department is null, set it to an empty string

            // JobTitle ??= string.Empty;
            // Department ??= string.Empty;
        }
    }
}
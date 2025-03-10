using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public class UserRepository : IUserRepository // connecting to IuserRepository
    {
        DataContextEF _entityFramework; // constructor to inject the dapper


        public UserRepository(IConfiguration config)
        {
            // Console.WriteLine(configuration.GetConnectionString("DefaultConnection")); // get connection string
            _entityFramework = new DataContextEF(config);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        // add entity
        // public bool AddEntity<T>(T entityToAdd)
        public void AddEntity<T>(T entityToAdd) // used void because we are not returning anything
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd); // run our EF add on our entity
                // return true; // if the entity is not null it will return true
            }
            // return false; // and if doesn't return true it will return false

        }
        // remove entity
        public void RemoveEntity<T>(T entityToAdd) // used void because we are not returning anything
        {
            if (entityToAdd != null)
            {
                _entityFramework.Remove(entityToAdd); // run our EF add on our entity
                // return true; // if the entity is not null it will return true
            }
            // return false; // and if doesn't return true it will return false
        }

        // get all users
        public IEnumerable<User> GetUsers() // hook this up to the interface
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>(); // use entity framework to get List by using DataContextEF
            return users;
        }

        // get single user
        public User GetSingleUser(int userId) // hook this up to the interface
        {
            User? user = _entityFramework.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefault<User>();

            if (user != null)
            {
                return user;
            }
            throw new Exception("User not found");
        }
        public UserSalary GetUserSalary(int userId) // get user salary
        {
            UserSalary? userSalary = _entityFramework.UserSalaries // Query the database using Entity Framework to find the UserSalary record
                .Where(u => u.UserId == userId) // where the UserId matches the provided userId.
                .FirstOrDefault<UserSalary>(); // The result is either the matching UserSalary record or null (because of "?")

            if (userSalary != null)
            {
                return userSalary;
            }
            throw new Exception("User Salary not found");
        }

        public UserJobInfo GetUserJobInfo(int userId) // get user job info
        {
            UserJobInfo? userJobInfo = _entityFramework.UserJobInfos // query the database using entity framework to find the UserJobInfo record
                .Where(x => x.UserId == userId) // where the UserId matches the provided userId
                .FirstOrDefault<UserJobInfo>();

            if (userJobInfo != null)
            {
                return userJobInfo;
            }
            throw new Exception("User Job Information not found");
        }
    }
}
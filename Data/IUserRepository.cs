using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public interface IUserRepository // interface for repository pattern
    {
        public bool SaveChanges(); // post or put
        public void AddEntity<T>(T entityToAdd); // insert
        public void RemoveEntity<T>(T entityToAdd); // delete
        public IEnumerable<User> GetUsers(); // get all users
        public User GetSingleUser(int userId); // get single user
        public UserSalary GetUserSalary(int userId); // get user salary
        public UserJobInfo GetUserJobInfo(int userId); // get user job info
    }
}
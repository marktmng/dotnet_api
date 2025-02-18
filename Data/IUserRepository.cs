namespace DotnetAPI.Data
{
    public interface IUserRepository // interface for repository pattern
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToAdd);
    }
}
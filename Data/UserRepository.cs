namespace DotnetAPI.Data
{
    public class UserRepository
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
        // public bool AddEntity<T>(T entityToAdd)
        public void RemoveEntity<T>(T entityToAdd) // used void because we are not returning anything
        {
            if (entityToAdd != null)
            {
                _entityFramework.Remove(entityToAdd); // run our EF add on our entity
                // return true; // if the entity is not null it will return true
            }
            // return false; // and if doesn't return true it will return false

        }
    }
}
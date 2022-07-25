using MongoDB.Driver;
using UserService.Data.Entity;

namespace UserService.Data.Mongo;

public static class  SeedUserData
{

    public static void SeedData(IMongoCollection<User> users)
    {
        var userList = users.Find(u => true).Limit(1).ToList();
        if (!userList.Any())
        {
            users.InsertMany(GetInitialUsers());
        }
        
    }
    private static IEnumerable<User>  GetInitialUsers()
    {
        return new[]
        {
            new User()
            {
                Id = "12d91541a2a411f44df899ce",
                Name = "Serkan",
                Surname = "Kutlu",
                Email = "kutluserkan1@gmail.com",
                Password = "12345"
            }
        };
    }
    
}
using GenericMongo.Bases;
using GenericMongo.Interfaces;

namespace GenericMongoClient;

public class StudentRepository : RepositoryBase<Student>, IStudentRepository
{
    public StudentRepository(IMongoService<Student> mongoService) : base(mongoService)
    {
        
    }

    public override async Task<Student> GetByIdAsync(string id)
    {
        return new Student
        {
            Name = "el üretimi"
        };
    }
}
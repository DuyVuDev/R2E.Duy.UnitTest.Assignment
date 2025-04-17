using R2EDuy.AspNetMVC.Assignment.Models;

namespace R2EDuy.AspNetMVC.Assignment.Repository
{
    public class PersonRepositoryImplement : IPersonRepository
    {
        private List<Person> People;

        public PersonRepositoryImplement()
        {
            // Fake database (temporary list)
            People = new List<Person>
                {
                    new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Gender = "Male", DateOfBirth = new DateTime(1990, 5, 20), PhoneNumber = "123456789", BirthPlace = "New York", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Gender = "Female", DateOfBirth = new DateTime(1995, 10, 15), PhoneNumber = "987654321", BirthPlace = "Los Angeles", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Johnson", Gender = "Female", DateOfBirth = new DateTime(1988, 3, 8), PhoneNumber = "555123456", BirthPlace = "Chicago", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Michael", LastName = "Brown", Gender = "Male", DateOfBirth = new DateTime(1992, 7, 22), PhoneNumber = "444987654", BirthPlace = "Houston", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "David", LastName = "Wilson", Gender = "Male", DateOfBirth = new DateTime(2000, 1, 15), PhoneNumber = "666789123", BirthPlace = "San Francisco", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Emily", LastName = "Davis", Gender = "Female", DateOfBirth = new DateTime(1997, 9, 10), PhoneNumber = "777321456", BirthPlace = "Boston", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "Daniel", LastName = "Garcia", Gender = "Male", DateOfBirth = new DateTime(1985, 4, 5), PhoneNumber = "888456789", BirthPlace = "Seattle", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Sophia", LastName = "Martinez", Gender = "Female", DateOfBirth = new DateTime(1993, 11, 25), PhoneNumber = "999654321", BirthPlace = "Miami", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "Chris", LastName = "Evans", Gender = "Male", DateOfBirth = new DateTime(1987, 6, 13), PhoneNumber = "111222333", BirthPlace = "Dallas", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Olivia", LastName = "Taylor", Gender = "Female", DateOfBirth = new DateTime(1991, 12, 5), PhoneNumber = "444555666", BirthPlace = "Denver", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Gender = "Male", DateOfBirth = new DateTime(1990, 5, 20), PhoneNumber = "123456789", BirthPlace = "New York", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Gender = "Female", DateOfBirth = new DateTime(1995, 10, 15), PhoneNumber = "987654321", BirthPlace = "Los Angeles", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Johnson", Gender = "Female", DateOfBirth = new DateTime(1988, 3, 8), PhoneNumber = "555123456", BirthPlace = "Chicago", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Michael", LastName = "Brown", Gender = "Male", DateOfBirth = new DateTime(1992, 7, 22), PhoneNumber = "444987654", BirthPlace = "Houston", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "David", LastName = "Wilson", Gender = "Male", DateOfBirth = new DateTime(2000, 1, 15), PhoneNumber = "666789123", BirthPlace = "San Francisco", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Emily", LastName = "Davis", Gender = "Female", DateOfBirth = new DateTime(1997, 9, 10), PhoneNumber = "777321456", BirthPlace = "Boston", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "Daniel", LastName = "Garcia", Gender = "Male", DateOfBirth = new DateTime(1985, 4, 5), PhoneNumber = "888456789", BirthPlace = "Seattle", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Sophia", LastName = "Martinez", Gender = "Female", DateOfBirth = new DateTime(1993, 11, 25), PhoneNumber = "999654321", BirthPlace = "Miami", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "Chris", LastName = "Evans", Gender = "Male", DateOfBirth = new DateTime(1987, 6, 13), PhoneNumber = "111222333", BirthPlace = "Dallas", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Olivia", LastName = "Taylor", Gender = "Female", DateOfBirth = new DateTime(1991, 12, 5), PhoneNumber = "444555666", BirthPlace = "Denver", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Gender = "Male", DateOfBirth = new DateTime(1990, 5, 20), PhoneNumber = "123456789", BirthPlace = "New York", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Gender = "Female", DateOfBirth = new DateTime(1995, 10, 15), PhoneNumber = "987654321", BirthPlace = "Los Angeles", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Johnson", Gender = "Female", DateOfBirth = new DateTime(1988, 3, 8), PhoneNumber = "555123456", BirthPlace = "Chicago", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Michael", LastName = "Brown", Gender = "Male", DateOfBirth = new DateTime(1992, 7, 22), PhoneNumber = "444987654", BirthPlace = "Houston", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "David", LastName = "Wilson", Gender = "Male", DateOfBirth = new DateTime(2000, 1, 15), PhoneNumber = "666789123", BirthPlace = "San Francisco", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Emily", LastName = "Davis", Gender = "Female", DateOfBirth = new DateTime(1997, 9, 10), PhoneNumber = "777321456", BirthPlace = "Boston", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "Daniel", LastName = "Garcia", Gender = "Male", DateOfBirth = new DateTime(1985, 4, 5), PhoneNumber = "888456789", BirthPlace = "Seattle", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Sophia", LastName = "Martinez", Gender = "Female", DateOfBirth = new DateTime(1993, 11, 25), PhoneNumber = "999654321", BirthPlace = "Miami", IsGraduated = false },
                    new Person { Id = Guid.NewGuid(), FirstName = "Chris", LastName = "Evans", Gender = "Male", DateOfBirth = new DateTime(1987, 6, 13), PhoneNumber = "111222333", BirthPlace = "Dallas", IsGraduated = true },
                    new Person { Id = Guid.NewGuid(), FirstName = "Olivia", LastName = "Taylor", Gender = "Female", DateOfBirth = new DateTime(1991, 12, 5), PhoneNumber = "444555666", BirthPlace = "Denver", IsGraduated = false }
                };
        }

        public Person? GetById(Guid id)
        {
            return People.FirstOrDefault(u => u.Id == id);
        }

        public List<Person> GetAll()
        {
            return People.ToList();
        }

        public void Add(Person user)
        {
            People.Add(user);
        }

        public void Update(Person user)
        {
            var person = People.FirstOrDefault(p => p.Id == user.Id);
            if (person != null)
            {
                person.FirstName = user.FirstName;
                person.LastName = user.LastName;
                person.Gender = user.Gender;
                person.DateOfBirth = user.DateOfBirth;
                person.PhoneNumber = user.PhoneNumber;
                person.BirthPlace = user.BirthPlace;
                person.IsGraduated = user.IsGraduated;
            }
        }

        public void Delete(Guid id)
        {
            var user = People.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                People.Remove(user);
            }
        }
    }
}

using R2EDuy.AspNetMVC.Assignment.Models;

namespace TestProject.Commons
{
    public static class PeopleDataSamples
    {
        public static List<Person> GetSamplePeople()
        {
            return new List<Person>
                {
                    new() { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Gender = "Male", DateOfBirth = new DateTime(1990, 1, 1), PhoneNumber = "0123456789", BirthPlace = "Hanoi", IsGraduated = true },
                    new() { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Gender = "Female", DateOfBirth = new DateTime(1985, 5, 15), PhoneNumber = "0987654321", BirthPlace = "Saigon", IsGraduated = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Brown", Gender = "Female", DateOfBirth = new DateTime(2000, 12, 12), PhoneNumber = "0912345678", BirthPlace = "Danang", IsGraduated = true },
                    new() { Id = Guid.NewGuid(), FirstName = "Bob", LastName = "White", Gender = "Male", DateOfBirth = new DateTime(1995, 3, 3), PhoneNumber = "0934567890", BirthPlace = "Hue", IsGraduated = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Charlie", LastName = "Green", Gender = "Other", DateOfBirth = new DateTime(1992, 6, 6), PhoneNumber = "0955555555", BirthPlace = "Hanoi", IsGraduated = true },
                    new() { Id = Guid.NewGuid(), FirstName = "David", LastName = "Black", Gender = "Male", DateOfBirth = new DateTime(1980, 11, 25), PhoneNumber = "0909090909", BirthPlace = "Saigon", IsGraduated = true },
                    new() { Id = Guid.NewGuid(), FirstName = "Emily", LastName = "Taylor", Gender = "Female", DateOfBirth = new DateTime(2001, 7, 7), PhoneNumber = "0988888888", BirthPlace = "Can Tho", IsGraduated = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Frank", LastName = "Lee", Gender = "Male", DateOfBirth = new DateTime(1998, 9, 9), PhoneNumber = "0922222222", BirthPlace = "Haiphong", IsGraduated = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Grace", LastName = "Wilson", Gender = "Female", DateOfBirth = new DateTime(1993, 4, 4), PhoneNumber = "0966666666", BirthPlace = "Nha Trang", IsGraduated = true },
                    new() { Id = Guid.NewGuid(), FirstName = "Henry", LastName = "King", Gender = "Male", DateOfBirth = new DateTime(1985, 5, 15), PhoneNumber = "0977777777", BirthPlace = "Bien Hoa", IsGraduated = true }
                };
        }
    }
}

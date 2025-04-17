using R2EDuy.AspNetMVC.Assignment.Models;

namespace TestProject.Commons
{
    public class PersonServiceData
    {
        public static IEnumerable<object[]> PersonTestData =>
            new List<object[]>
            {
                new object[]
                {
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Gender = "Male",
                        DateOfBirth = new DateTime(2000, 6, 15),
                        PhoneNumber = "0123456789",
                        BirthPlace = "New York",
                        IsGraduated = false
                    },
                    
                },
            };
    }
}

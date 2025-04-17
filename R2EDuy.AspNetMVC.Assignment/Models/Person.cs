namespace R2EDuy.AspNetMVC.Assignment.Models
{
    public class Person
    {

        public Guid Id { get; set; }

        public required string FirstName { get; set; }


        public required string LastName { get; set; }


        public required string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public required string PhoneNumber { get; set; }


        public required string BirthPlace { get; set; }

        public bool IsGraduated { get; set; }

        public int Age => DateTime.Now.Year - DateOfBirth.Year;

        public string FullName => $"{FirstName} {LastName}";
    }
}

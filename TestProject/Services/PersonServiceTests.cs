using ClosedXML.Excel;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using R2EDuy.AspNetMVC.Assignment.Models;
using R2EDuy.AspNetMVC.Assignment.Models.Validation;
using TestProject.Commons;

namespace TestProject.Services
{
    public class PersonServiceTests
    {
        private Mock<IPersonRepository> _personRepositoryMock;
        private PersonValidator validator = new();

        [Fact]
        public void GetAllPeople_WhenDataExists_ShouldReturnAllPeople()
        {
            // Arrange
            SetUpDependencies();
            var expectedPeople = GetListPerson();
            _personRepositoryMock.Setup(repo => repo.GetAll()).Returns(expectedPeople);
            var service = GetPersonServices();

            // Act
            var result = service.GetAllPeople();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedPeople, options =>
                options.Excluding(p => p.Age)
                        .Excluding(p => p.FullName));
        }


        [Fact]
        public void GetAllPeople_WhenNoData_ShouldReturnEmptyList()
        {
            // Arrange
            SetUpDependencies();
            _personRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Person>());
            var service = GetPersonServices();

            // Act
            var result = service.GetAllPeople();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetAllPeople_WhenRepositoryReturnsNull_ShouldReturnEmptyList()
        {
            // Arrange
            SetUpDependencies();
            _personRepositoryMock.Setup(repo => repo.GetAll()).Returns((List<Person>)null);
            var service = GetPersonServices();

            // Act
            var result = service.GetAllPeople();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetPerson_WhenPersonExists_ShouldReturnPerson()
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();
            var expectedPerson = new Person
            {
                Id = id,
                FirstName = "Alice",
                LastName = "Nguyen",
                Gender = "Female",
                DateOfBirth = new DateTime(1995, 3, 10),
                PhoneNumber = "0123456789",
                BirthPlace = "Da Nang"
            };

            _personRepositoryMock.Setup(repo => repo.GetById(id)).Returns(expectedPerson);
            var service = GetPersonServices();

            // Act
            var result = service.GetPerson(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedPerson);
            _personRepositoryMock.Verify(repo => repo.GetById(id), Times.Once);
        }

        [Fact]
        public void GetPerson_WhenPersonDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();
            _personRepositoryMock.Setup(repo => repo.GetById(id)).Returns((Person?)null);
            var service = GetPersonServices();

            // Act
            var result = service.GetPerson(id);

            // Assert
            result.Should().BeNull();
            _personRepositoryMock.Verify(repo => repo.GetById(id), Times.Once);
        }

        [Theory]
        [MemberData(nameof(PersonServiceData.PersonTestData), MemberType = typeof(PersonServiceData))]
        public void CreatePerson_ValidPerson_ShouldCallAddAndReturnTrue(Person person)
        {
            // Arrange
            SetUpDependencies();
            var service = GetPersonServices();

            // Act
            var result = service.CreatePerson(person);

            // Assert
            result.Should().BeTrue();
            _personRepositoryMock.Verify(repo => repo.Add(person), Times.Once);
        }

        [Fact]
        public void CreatePerson_InvalidPerson_ShouldHaveError()
        {
            // Arrange
            SetUpDependencies();
            var newPerson = new Person
            {
                FirstName = "", // Invalid
                LastName = "",
                Gender = "Alien",
                DateOfBirth = DateTime.Today.AddDays(1), // Future
                PhoneNumber = "123",
                BirthPlace = ""
            }; ;
            var service = GetPersonServices();
            // Act

            var result = validator.TestValidate(newPerson);
            // Assert
            result.ShouldHaveAnyValidationError();
            _personRepositoryMock.Verify(repo => repo.Add(It.IsAny<Person>()), Times.Never);
        }

        [Fact]
        public void CreatePerson_NullPerson_ShouldThrowArgumentNullException()
        {
            // Arrange
            SetUpDependencies();
            var service = GetPersonServices();

            // Act
            Action act = () => service.CreatePerson(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(PersonServiceData.PersonTestData), MemberType = typeof(PersonServiceData))]
        public void CreatePerson_RepositoryThrowsException_ShouldPropagate(Person person)
        {
            // Arrange
            SetUpDependencies();
            _personRepositoryMock.Setup(r => r.Add(It.IsAny<Person>())).Throws<Exception>();
            var service = GetPersonServices();

            // Act
            Action act = () => service.CreatePerson(person);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Theory]
        [MemberData(nameof(PersonServiceData.PersonTestData), MemberType = typeof(PersonServiceData))]

        public void UpdatePerson_ValidPerson_ShouldCallUpdateAndReturnTrue(Person person)
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();
            var existingPerson = new Person
            {
                Id = id,
                FirstName = "New",
                LastName = "Name",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 1, 1),
                PhoneNumber = "0123456789",
                BirthPlace = "City",
                IsGraduated = true
            };
            var newPerson = person;


            _personRepositoryMock.Setup(r => r.GetById(id)).Returns(existingPerson);
            var service = GetPersonServices();

            // Act
            var result = service.UpdatePerson(id, newPerson);

            // Assert
            result.Should().BeTrue();
            newPerson.Id.Should().Be(id);
            _personRepositoryMock.Verify(r => r.Update(newPerson), Times.Once);
        }

        [Fact]
        public void UpdatePerson_InvalidPerson_ShouldHaveError()
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();

            var newPerson = new Person
            {
                FirstName = "", // Invalid
                LastName = "",
                Gender = "Alien",
                DateOfBirth = DateTime.Today.AddDays(1), // Future
                PhoneNumber = "123",
                BirthPlace = ""
            };
            var service = GetPersonServices();
            // Act

            var result = validator.TestValidate(newPerson);
            // Assert
            result.ShouldHaveAnyValidationError();
            _personRepositoryMock.Verify(repo => repo.Update(It.IsAny<Person>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(PersonServiceData.PersonTestData), MemberType = typeof(PersonServiceData))]

        public void UpdatePerson_InvalidId_ShouldReturnFalse(Person person)
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();
            var newPerson = person;

            _personRepositoryMock.Setup(r => r.GetById(id)).Returns((Person)null!);
            var service = GetPersonServices();

            // Act
            var result = service.UpdatePerson(id, newPerson);

            // Assert
            result.Should().BeFalse();
            _personRepositoryMock.Verify(r => r.Update(It.IsAny<Person>()), Times.Never);
        }

        [Fact]
        public void UpdatePerson_NullPerson_ShouldThrowArgumentNullException()
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();

            var service = GetPersonServices();

            // Act
            Action act = () => service.UpdatePerson(id, null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(PersonServiceData.PersonTestData), MemberType = typeof(PersonServiceData))]
        public void UpdatePerson_ShouldSetCorrectIdBeforeUpdate(Person person)
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();
            var existing = new Person
            {
                Id = id,
                FirstName = "New",
                LastName = "Name",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 1, 1),
                PhoneNumber = "0123456789",
                BirthPlace = "City",
                IsGraduated = true
            };
            var newPerson = person;
            _personRepositoryMock.Setup(r => r.GetById(id)).Returns(existing);
            var service = GetPersonServices();

            // Act
            service.UpdatePerson(id, newPerson);

            // Assert
            person.Id.Should().Be(id);
            _personRepositoryMock.Verify(r => r.Update(It.Is<Person>(p => p.Id == id)), Times.Once);
        }// ID must be set correctly

        [Theory]
        [MemberData(nameof(PersonServiceData.PersonTestData), MemberType = typeof(PersonServiceData))]
        public void UpdatePerson_RepositoryThrows_ShouldPropagate(Person person)
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();
            var existing = new Person
            {
                Id = id,
                FirstName = "New",
                LastName = "Name",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 1, 1),
                PhoneNumber = "0123456789",
                BirthPlace = "City",
                IsGraduated = true
            };
            var newPerson = person;
            _personRepositoryMock.Setup(r => r.GetById(id)).Returns(existing);
            _personRepositoryMock.Setup(r => r.Update(It.IsAny<Person>())).Throws<Exception>();
            var service = GetPersonServices();

            // Act
            Action act = () => service.UpdatePerson(id, person);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void DeletePerson_WhenPersonExists_ShouldReturnTrue()
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();
            var person = new Person
            {
                Id = id,
                FirstName = "Test",
                LastName = "User",
                Gender = "Male",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "0123456789",
                BirthPlace = "City"
            };

            _personRepositoryMock.Setup(r => r.GetById(id)).Returns(person);
            var service = GetPersonServices();

            // Act
            var result = service.DeletePerson(id);

            // Assert
            result.Should().BeTrue();
            _personRepositoryMock.Verify(r => r.Delete(id), Times.Once);
        }

        [Fact]
        public void DeletePerson_WhenPersonDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();
            _personRepositoryMock.Setup(r => r.GetById(id)).Returns((Person)null!);
            var service = GetPersonServices();

            // Act
            var result = service.DeletePerson(id);

            // Assert
            result.Should().BeFalse();
            _personRepositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void GetMales_WhenMalesExist_ShouldReturnOnlyMales()
        {
            // Arrange
            SetUpDependencies();
            var people = new List<Person>
    {
        new Person { FirstName = "John", LastName = "Doe", Gender = "Male", DateOfBirth = DateTime.Now.AddYears(-30), PhoneNumber = "0123456789", BirthPlace = "City" },
        new Person { FirstName = "Jane", LastName = "Smith", Gender = "Female", DateOfBirth = DateTime.Now.AddYears(-25), PhoneNumber = "0987654321", BirthPlace = "City" },
        new Person { FirstName = "Alex", LastName = "Brown", Gender = "Male", DateOfBirth = DateTime.Now.AddYears(-20), PhoneNumber = "0111222333", BirthPlace = "City" }
    };

            _personRepositoryMock.Setup(r => r.GetAll()).Returns(people);
            var service = GetPersonServices();

            // Act
            var result = service.GetMales();

            // Assert
            result.Should().HaveCount(2);
            result.Should().OnlyContain(p => p.Gender == "Male");
        }

        [Fact]
        public void GetMales_WhenNoMalesExist_ShouldReturnEmptyList()
        {
            // Arrange
            SetUpDependencies();
            var people = new List<Person>
    {
        new Person { FirstName = "Jane", LastName = "Smith", Gender = "Female", DateOfBirth = DateTime.Now.AddYears(-25), PhoneNumber = "0987654321", BirthPlace = "City" },
        new Person { FirstName = "Taylor", LastName = "Lee", Gender = "Other", DateOfBirth = DateTime.Now.AddYears(-22), PhoneNumber = "0111222333", BirthPlace = "City" }
    };

            _personRepositoryMock.Setup(r => r.GetAll()).Returns(people);
            var service = GetPersonServices();

            // Act
            var result = service.GetMales();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetMales_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyList()
        {
            // Arrange
            SetUpDependencies();
            _personRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Person>());
            var service = GetPersonServices();

            // Act
            var result = service.GetMales();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetOldestPerson_WhenPeopleExist_ShouldReturnOldest()
        {
            // Arrange
            SetUpDependencies();
            var person1 = new Person { FirstName = "John", LastName = "Doe", Gender = "Male", DateOfBirth = new DateTime(1980, 1, 1), PhoneNumber = "0123456789", BirthPlace = "City" };
            var person2 = new Person { FirstName = "Jane", LastName = "Smith", Gender = "Female", DateOfBirth = new DateTime(1975, 5, 15), PhoneNumber = "0987654321", BirthPlace = "City" };
            var person3 = new Person { FirstName = "Alex", LastName = "Lee", Gender = "Other", DateOfBirth = new DateTime(1990, 3, 10), PhoneNumber = "0111222333", BirthPlace = "City" };

            var people = new List<Person> { person1, person2, person3 };
            _personRepositoryMock.Setup(r => r.GetAll()).Returns(people);
            var service = GetPersonServices();

            // Act
            var result = service.GetOldestPerson();

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(person2);
        }

        [Fact]
        public void GetOldestPerson_WhenNoPeopleExist_ShouldReturnNull()
        {
            // Arrange
            SetUpDependencies();
            _personRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Person>());
            var service = GetPersonServices();

            // Act
            var result = service.GetOldestPerson();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetFullName_WhenPersonExists_ShouldReturnFullName()
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();
            var person = new Person
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "0123456789",
                BirthPlace = "Hanoi",
                IsGraduated = true
            };

            _personRepositoryMock.Setup(r => r.GetById(id)).Returns(person);
            var service = GetPersonServices();

            // Act
            var fullName = service.GetFullName(id);

            // Assert
            fullName.Should().Be("John Doe");
        }

        [Fact]
        public void GetFullName_WhenPersonDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();
            _personRepositoryMock.Setup(r => r.GetById(id)).Returns((Person?)null);
            var service = GetPersonServices();

            // Act
            var fullName = service.GetFullName(id);

            // Assert
            fullName.Should().BeNull();
        }

        [Fact]
        public void GetFullNames_WhenPersonsExist_ShouldReturnFullNames()
        {
            // Arrange
            SetUpDependencies();
            var people = new List<Person>
                {
                    new Person { FirstName = "John", LastName = "Doe", Gender = "Male", DateOfBirth = DateTime.Now.AddYears(-30), PhoneNumber = "0123456789", BirthPlace = "Hanoi" },
                    new Person { FirstName = "Jane", LastName = "Smith", Gender = "Female", DateOfBirth = DateTime.Now.AddYears(-25), PhoneNumber = "0987654321", BirthPlace = "HCMC" }
                };

            _personRepositoryMock.Setup(repo => repo.GetAll()).Returns(people);
            var service = GetPersonServices();

            // Act
            var result = service.GetFullNames();

            // Assert
            result.Should().ContainInOrder("John Doe", "Jane Smith");
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetFullNames_WhenNoPersonsExist_ShouldReturnEmptyList()
        {
            // Arrange
            SetUpDependencies();
            _personRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Person>());
            var service = GetPersonServices();

            // Act
            var result = service.GetFullNames();

            // Assert
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(1990, "equal", 1)]
        [InlineData(1980, "greater", 9)]
        [InlineData(2000, "less", 8)]
        [InlineData(2005, "equal", 0)]
        [InlineData(1985, "less", 1)]
        [InlineData(1985, "invalid", 10)]
        public void FilterByBirthYear_WithVariousChoices_ReturnsExpectedCount(int year, string choice, int expectedCount)
        {
            // Arrange
            SetUpDependencies();
            _personRepositoryMock.Setup(r => r.GetAll()).Returns(PeopleDataSamples.GetSamplePeople());
            var service = GetPersonServices();

            // Act
            var result = service.FilterByBirthYear(year, choice);

            // Assert
            result.Should().HaveCount(expectedCount);
        }

        [Fact]
        public void FilterByBirthYear_EqualChoice_ShouldReturnMatchingYear()
        {
            // Arrange
            SetUpDependencies();
            var year = 1990;
            _personRepositoryMock.Setup(r => r.GetAll()).Returns(PeopleDataSamples.GetSamplePeople);
            var service = GetPersonServices();
            // Act
            var result = service.FilterByBirthYear(year, "equal");

            // Assert
            result.Should().Contain(p => p.DateOfBirth.Year == 1990);
        }

        [Fact]
        public void FilterByBirthYear_GreaterChoice_ShouldReturnMatchingYear()
        {
            // Arrange
            SetUpDependencies();
            var year = 1990;
            _personRepositoryMock.Setup(r => r.GetAll()).Returns(PeopleDataSamples.GetSamplePeople);
            var service = GetPersonServices();
            // Act
            var result = service.FilterByBirthYear(year, "greater");

            // Assert
            result.Should().Contain(p => p.DateOfBirth.Year > 1990);
        }

        [Fact]
        public void FilterByBirthYear_LessChoice_ShouldReturnMatchingYear()
        {
            // Arrange
            SetUpDependencies();
            var year = 1990;
            _personRepositoryMock.Setup(r => r.GetAll()).Returns(PeopleDataSamples.GetSamplePeople);
            var service = GetPersonServices();
            // Act
            var result = service.FilterByBirthYear(year, "less");

            // Assert
            result.Should().Contain(p => p.DateOfBirth.Year < 1990);
        }

        [Fact]
        public void FilterByBirthYear_DafaultChoice_ShouldReturnAll()
        {
            // Arrange
            SetUpDependencies();
            var year = 1990;
            _personRepositoryMock.Setup(r => r.GetAll()).Returns(PeopleDataSamples.GetSamplePeople);
            var service = GetPersonServices();
            // Act
            var result = service.FilterByBirthYear(year, "invalid");
            // Assert
            result.Should().HaveCount(10);
        }

        [Fact]
        public void FilterByBirthYear_NullChoice_ShouldReturnAll()
        {
            // Arrange
            SetUpDependencies();
            var year = 1990;
            _personRepositoryMock.Setup(r => r.GetAll()).Returns(PeopleDataSamples.GetSamplePeople);
            var service = GetPersonServices();
            // Act
            var result = service.FilterByBirthYear(year, null);
            // Assert
            result.Should().HaveCount(10);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3000)]
        public void FilterByBirthYear_InvalidYear_ShouldThrowArgumentOutOfRangeException(int year)
        {
            // Arrange
            SetUpDependencies();
            _personRepositoryMock.Setup(r => r.GetAll()).Returns(PeopleDataSamples.GetSamplePeople);
            var service = GetPersonServices();
            // Act
            Action act = () => service.FilterByBirthYear(year, "equal");
            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Year must be between 0 and the current year.*")
                .And.ParamName.Should().Be("year");
        }

        [Fact]
        public void ExportToExcel_ShouldReturnValidExcelFile()
        {
            // Arrange
            SetUpDependencies();
            var people = new List<Person>
                {
                    new() { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Gender = "Male", DateOfBirth = new DateTime(1990, 1, 1), PhoneNumber = "0123456789", BirthPlace = "Hanoi", IsGraduated = true },
                    new() { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Gender = "Female", DateOfBirth = new DateTime(1985, 5, 15), PhoneNumber = "0987654321", BirthPlace = "Saigon", IsGraduated = false }
                };

            _personRepositoryMock.Setup(r => r.GetAll()).Returns(people);
            var service = GetPersonServices();

            // Act
            var result = service.ExportToExcel();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<FileContentResult>();
            result.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            result.FileDownloadName.Should().Be("PeopleData.xlsx");
            result.FileContents.Should().NotBeEmpty();

            // Đọc nội dung Excel để xác nhận dữ liệu đúng
            using var stream = new MemoryStream(result.FileContents);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet("People");

            worksheet.Cell(1, 1).Value.ToString().Should().Be("ID");
            worksheet.Cell(2, 2).Value.ToString().Should().Be("John");
            worksheet.Cell(3, 2).Value.ToString().Should().Be("Jane");

            worksheet.Cell(2, 8).Value.ToString().Should().Be("Yes");
            worksheet.Cell(3, 8).Value.ToString().Should().Be("No");
        }

        [Fact]
        public void GetPagedPersons_ShouldReturnCorrectPage1()
        {
            // Arrange
            SetUpDependencies();
            var samplePeople = PeopleDataSamples.GetSamplePeople();
            _personRepositoryMock.Setup(repo => repo.GetAll()).Returns(samplePeople);

            int pageNumber = 1;
            int pageSize = 3;
            var service = GetPersonServices();
            // Act
            var result = service.GetPagedPersons(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(pageSize);
            result.Items.Should().BeEquivalentTo(samplePeople.Take(3));
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.TotalRecords.Should().Be(samplePeople.Count);
        }

        [Fact]
        public void GetPagedPersons_ShouldReturnCorrectPage2()
        {
            // Arrange
            SetUpDependencies();
            var samplePeople = PeopleDataSamples.GetSamplePeople();
            _personRepositoryMock.Setup(repo => repo.GetAll()).Returns(samplePeople);

            int pageNumber = 2;
            int pageSize = 4;
            var service = GetPersonServices();
            // Act
            var result = service.GetPagedPersons(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(pageSize);
            result.Items.Should().BeEquivalentTo(samplePeople.Skip(4).Take(4));
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.TotalRecords.Should().Be(samplePeople.Count);
        }

        [Fact]
        public void GetPagedPersons_PageOutOfRange_ShouldReturnEmptyList()
        {
            // Arrange
            SetUpDependencies();
            var samplePeople = PeopleDataSamples.GetSamplePeople();
            _personRepositoryMock.Setup(repo => repo.GetAll()).Returns(samplePeople);

            int pageNumber = 3;
            int pageSize = 5;
            var service = GetPersonServices();

            // Act
            var result = service.GetPagedPersons(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.TotalRecords.Should().Be(samplePeople.Count);
        }

        private List<Person> GetListPerson()
        {
            return new();
        }
        private void SetUpDependencies()
        {
            _personRepositoryMock = new();
        }

        private PersonServiceImplement GetPersonServices()
        {
            return new PersonServiceImplement(_personRepositoryMock.Object);
        }
    }
}

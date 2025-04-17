using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using R2EDuy.AspNetMVC.Assignment.Models;
using R2EDuy.AspNetMVC.Assignment.Services;
using TestProject.Commons;

namespace TestProject.Controller
{
    public class PersonControllerTest
    {
        private Mock<IPersonService> _personServiceMock;

        private void SetUpDependencies()
        {
            _personServiceMock = new Mock<IPersonService>();
        }

        private PersonController CreateController()
        {
            return new PersonController(_personServiceMock.Object);
        }

        [Fact]
        public void Index_ReturnsViewWithPagedResult()
        {
            // Arrange
            SetUpDependencies();
            int pageNumber = 2;
            int pageSize = 5;
            var expectedPagedResult = new PagedResult<Person>
            {
                Items = PeopleDataSamples.GetSamplePeople(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = 10
            };

            _personServiceMock
                .Setup(s => s.GetPagedPersons(pageNumber, pageSize))
                .Returns(expectedPagedResult);
            var _controller = CreateController();

            // Act
            var result = _controller.Index(pageNumber, pageSize);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(expectedPagedResult);

            _personServiceMock.Verify(s => s.GetPagedPersons(pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public void Index_UsesDefaultPageValues_WhenNoParametersProvided()
        {
            // Arrange
            SetUpDependencies();
            int defaultPageNumber = 1;
            int defaultPageSize = 8;

            var allPeople = PeopleDataSamples.GetSamplePeople();
            var pagedPeople = allPeople.Take(defaultPageSize).ToList();

            var expectedResult = new PagedResult<Person>
            {
                Items = pagedPeople,
                PageNumber = defaultPageNumber,
                PageSize = defaultPageSize,
                TotalRecords = allPeople.Count
            };

            _personServiceMock
                .Setup(s => s.GetPagedPersons(defaultPageNumber, defaultPageSize))
                .Returns(expectedResult);

            var _controller = CreateController();

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(expectedResult);

            _personServiceMock.Verify(s => s.GetPagedPersons(defaultPageNumber, defaultPageSize), Times.Once);
        }

        [Fact]
        public void Males_ReturnsViewWithMalePersons()
        {
            // Arrange
            SetUpDependencies();

            var expectedMales = PeopleDataSamples.GetSamplePeople()
                .Where(p => p.Gender == "Male")
                .ToList();

            _personServiceMock
                .Setup(s => s.GetMales())
                .Returns(expectedMales);

            var _controller = CreateController();

            // Act
            var result = _controller.Males();

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(expectedMales);

            _personServiceMock.Verify(s => s.GetMales(), Times.Once);
        }

        [Fact]
        public void Males_WhenNoMalePersons_ReturnsEmptyList()
        {
            // Arrange
            SetUpDependencies();

            var emptyList = new List<Person>();

            _personServiceMock
                .Setup(s => s.GetMales())
                .Returns(emptyList);

            var _controller = CreateController();

            // Act
            var result = _controller.Males();

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<List<Person>>().Subject;
            model.Should().BeEmpty();

            _personServiceMock.Verify(s => s.GetMales(), Times.Once);
        }

        [Fact]
        public void Oldest_ReturnsViewWithOldestPerson()
        {
            // Arrange
            SetUpDependencies();

            var allPeople = PeopleDataSamples.GetSamplePeople();
            var expectedOldest = allPeople.OrderBy(p => p.DateOfBirth).First();

            _personServiceMock
                .Setup(s => s.GetOldestPerson())
                .Returns(expectedOldest);

            var _controller = CreateController();

            // Act
            var result = _controller.Oldest();

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(expectedOldest);

            _personServiceMock.Verify(s => s.GetOldestPerson(), Times.Once);
        }

        [Fact]
        public void Oldest_WhenNoPersonExists_ReturnsNullModel()
        {
            // Arrange
            SetUpDependencies();

            _personServiceMock
                .Setup(s => s.GetOldestPerson())
                .Returns((Person)null);

            var _controller = CreateController();

            // Act
            var result = _controller.Oldest();

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeNull();

            _personServiceMock.Verify(s => s.GetOldestPerson(), Times.Once);
        }

        [Fact]
        public void FullNames_ReturnsViewWithListOfFullNames()
        {
            // Arrange
            SetUpDependencies();

            var people = PeopleDataSamples.GetSamplePeople();
            var expectedFullNames = people.Select(p => $"{p.FirstName} {p.LastName}").ToList();

            _personServiceMock
                .Setup(s => s.GetFullNames())
                .Returns(expectedFullNames);

            var _controller = CreateController();

            // Act
            var result = _controller.FullNames();

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(expectedFullNames);

            _personServiceMock.Verify(s => s.GetFullNames(), Times.Once);
        }

        [Fact]
        public void FullNames_WhenNoPersons_ReturnsEmptyList()
        {
            // Arrange
            SetUpDependencies();

            var expectedFullNames = new List<string>();

            _personServiceMock
                .Setup(s => s.GetFullNames())
                .Returns(expectedFullNames);

            var _controller = CreateController();

            // Act
            var result = _controller.FullNames();

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<List<string>>().Subject;
            model.Should().BeEmpty();

            _personServiceMock.Verify(s => s.GetFullNames(), Times.Once);
        }


        [Fact]
        public void FilterByBirthYear_WhenNoPersonMatches_ReturnsEmptyList()
        {
            // Arrange
            SetUpDependencies();

            int year = 2005;  // A year not present in the sample data
            string choice = "equals";

            var expectedResults = new List<Person>();  // No one born in 2005

            _personServiceMock
                .Setup(s => s.FilterByBirthYear(year, choice))
                .Returns(expectedResults);

            var _controller = CreateController();

            // Act
            var result = _controller.FilterByBirthYear(year, choice);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<List<Person>>().Subject;
            model.Should().BeEmpty();

            // Check ViewBag
            ((int)_controller.ViewBag.Year).Should().Be(year); // Fixed dynamic typing issue by casting ViewBag.Year to int
            ((string)_controller.ViewBag.Choice).Should().Be(choice); // Fixed dynamic typing issue by casting ViewBag.Choice to string

            _personServiceMock.Verify(s => s.FilterByBirthYear(year, choice), Times.Once);
        }


        [Fact]
        public void FilterByBirthYear_WhenChoiceIsInvalid_ReturnsEmptyList()
        {
            // Arrange
            SetUpDependencies();

            int year = 1995;
            string choice = "invalidChoice";  // Invalid choice, not handled by service

            var expectedResults = new List<Person>();  // Service should return empty list for invalid choice

            _personServiceMock
                .Setup(s => s.FilterByBirthYear(year, choice))
                .Returns(expectedResults);

            var _controller = CreateController();

            // Act
            var result = _controller.FilterByBirthYear(year, choice);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<List<Person>>().Subject;
            model.Should().BeEmpty();

            // Check ViewBag
            ((int)_controller.ViewBag.Year).Should().Be(year); // Fixed dynamic typing issue by casting ViewBag.Year to int
            ((string)_controller.ViewBag.Choice).Should().Be(choice); // Fixed dynamic typing issue by casting ViewBag.Choice to string

            _personServiceMock.Verify(s => s.FilterByBirthYear(year, choice), Times.Once);
        }

        [Fact]
        public void FilterByBirthYear_WhenChoiceIsBefore_ReturnsPersonsBornBeforeYear()
        {
            // Arrange
            SetUpDependencies();

            int year = 1995;
            string choice = "less";

            var expectedResults = PeopleDataSamples.GetSamplePeople()
                .Where(p => p.DateOfBirth.Year < year)
                .ToList();

            _personServiceMock
                .Setup(s => s.FilterByBirthYear(year, choice))
                .Returns(expectedResults);

            var _controller = CreateController();

            // Act
            var result = _controller.FilterByBirthYear(year, choice);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(expectedResults);

            // Check ViewBag
            ((int)_controller.ViewBag.Year).Should().Be(year); // Fixed dynamic typing issue by casting ViewBag.Year to int
            ((string)_controller.ViewBag.Choice).Should().Be(choice); // Fixed dynamic typing issue by casting ViewBag.Choice to string

            _personServiceMock.Verify(s => s.FilterByBirthYear(year, choice), Times.Once);
        }

        [Fact]
        public void FilterByBirthYear_WhenChoiceIsAfter_ReturnsPersonsBornAfterYear()
        {
            // Arrange
            SetUpDependencies();

            int year = 1995;
            string choice = "greater";

            var expectedResults = PeopleDataSamples.GetSamplePeople()
                .Where(p => p.DateOfBirth.Year > year)
                .ToList();

            _personServiceMock
                .Setup(s => s.FilterByBirthYear(year, choice))
                .Returns(expectedResults);

            var _controller = CreateController();

            // Act
            var result = _controller.FilterByBirthYear(year, choice);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(expectedResults);

            // Check ViewBag
            ((int)_controller.ViewBag.Year).Should().Be(year); // Fixed dynamic typing issue by casting ViewBag.Year to int
            ((string)_controller.ViewBag.Choice).Should().Be(choice); // Fixed dynamic typing issue by casting ViewBag.Choice to string

            _personServiceMock.Verify(s => s.FilterByBirthYear(year, choice), Times.Once);
        }

        [Fact]
        public void FilterByBirthYear_WhenChoiceIsEquals_ReturnsPersonsBornInYear()
        {
            // Arrange
            SetUpDependencies();

            int year = 1995;
            string choice = "equals";

            var expectedResults = PeopleDataSamples.GetSamplePeople()
                .Where(p => p.DateOfBirth.Year == year)
                .ToList();

            _personServiceMock
                .Setup(s => s.FilterByBirthYear(year, choice))
                .Returns(expectedResults);

            var _controller = CreateController();

            // Act
            var result = _controller.FilterByBirthYear(year, choice);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(expectedResults);

            // Check ViewBag
            ((int)_controller.ViewBag.Year).Should().Be(year); // Fixed dynamic typing issue by casting ViewBag.Year to int
            ((string)_controller.ViewBag.Choice).Should().Be(choice); // Fixed dynamic typing issue by casting ViewBag.Choice to string

            _personServiceMock.Verify(s => s.FilterByBirthYear(year, choice), Times.Once);
        }

        [Fact]
        public void AddAPerson_Get_ReturnsView()
        {
            // Arrange
            SetUpDependencies();
            var _controller = CreateController();

            // Act
            var result = _controller.AddAPerson();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

    }
}

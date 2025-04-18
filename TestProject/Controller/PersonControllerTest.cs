using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
            var controller = CreateController();

            // Act
            var result = controller.Index(pageNumber, pageSize);

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

            var controller = CreateController();

            // Act
            var result = controller.Index();

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

            var controller = CreateController();

            // Act
            var result = controller.Males();

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

            var controller = CreateController();

            // Act
            var result = controller.Males();

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

            var controller = CreateController();

            // Act
            var result = controller.Oldest();

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

            var controller = CreateController();

            // Act
            var result = controller.Oldest();

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

            var controller = CreateController();

            // Act
            var result = controller.FullNames();

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

            var controller = CreateController();

            // Act
            var result = controller.FullNames();

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

            var controller = CreateController();

            // Act
            var result = controller.FilterByBirthYear(year, choice);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<List<Person>>().Subject;
            model.Should().BeEmpty();

            // Check ViewBag
            ((int)controller.ViewBag.Year).Should().Be(year); // Fixed dynamic typing issue by casting ViewBag.Year to int
            ((string)controller.ViewBag.Choice).Should().Be(choice); // Fixed dynamic typing issue by casting ViewBag.Choice to string

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

            var controller = CreateController();

            // Act
            var result = controller.FilterByBirthYear(year, choice);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<List<Person>>().Subject;
            model.Should().BeEmpty();

            // Check ViewBag
            ((int)controller.ViewBag.Year).Should().Be(year); // Fixed dynamic typing issue by casting ViewBag.Year to int
            ((string)controller.ViewBag.Choice).Should().Be(choice); // Fixed dynamic typing issue by casting ViewBag.Choice to string

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

            var controller = CreateController();

            // Act
            var result = controller.FilterByBirthYear(year, choice);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(expectedResults);

            // Check ViewBag
            ((int)controller.ViewBag.Year).Should().Be(year); // Fixed dynamic typing issue by casting ViewBag.Year to int
            ((string)controller.ViewBag.Choice).Should().Be(choice); // Fixed dynamic typing issue by casting ViewBag.Choice to string

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

            var controller = CreateController();

            // Act
            var result = controller.FilterByBirthYear(year, choice);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(expectedResults);

            // Check ViewBag
            ((int)controller.ViewBag.Year).Should().Be(year); // Fixed dynamic typing issue by casting ViewBag.Year to int
            ((string)controller.ViewBag.Choice).Should().Be(choice); // Fixed dynamic typing issue by casting ViewBag.Choice to string

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

            var controller = CreateController();

            // Act
            var result = controller.FilterByBirthYear(year, choice);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(expectedResults);

            // Check ViewBag
            ((int)controller.ViewBag.Year).Should().Be(year); // Fixed dynamic typing issue by casting ViewBag.Year to int
            ((string)controller.ViewBag.Choice).Should().Be(choice); // Fixed dynamic typing issue by casting ViewBag.Choice to string

            _personServiceMock.Verify(s => s.FilterByBirthYear(year, choice), Times.Once);
        }

        [Fact]
        public void AddAPerson_Get_ReturnsView()
        {
            // Arrange
            SetUpDependencies();
            var controller = CreateController();

            // Act
            var result = controller.AddAPerson();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void AddAPerson_Post_ValidPerson_CreatesPersonAndRedirects()
        {
            // Arrange
            SetUpDependencies();
            var person = PeopleDataSamples.GetSamplePeople().First();
            var controller = CreateController();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.AddAPerson(person);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>()
                  .Which.ActionName.Should().Be("Index");

            controller.TempData["Message"].Should().Be($"Person {person.FullName} added successfully.");
            _personServiceMock.Verify(s => s.CreatePerson(person), Times.Once);
        }

        [Fact]
        public void AddAPerson_Post_WhenException_ReturnsErrorMessageAndRedirects()
        {
            // Arrange
            SetUpDependencies();
            var person = PeopleDataSamples.GetSamplePeople().First();
            _personServiceMock.Setup(s => s.CreatePerson(person)).Throws(new Exception("Database error"));

            var controller = CreateController();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.AddAPerson(person);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>()
                  .Which.ActionName.Should().Be("Index");

            controller.TempData["Message"].Should().Be($"Error adding person {person.FullName}.");
            _personServiceMock.Verify(s => s.CreatePerson(person), Times.Once);
        }

        [Fact]
        public void ViewAPerson_ExistingId_ReturnsViewWithPerson()
        {
            // Arrange
            SetUpDependencies();
            var person = PeopleDataSamples.GetSamplePeople().First();
            _personServiceMock.Setup(s => s.GetPerson(person.Id)).Returns(person);
            var controller = CreateController();

            // Act
            var result = controller.ViewAPerson(person.Id);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(person);
            _personServiceMock.Verify(s => s.GetPerson(person.Id), Times.Once);
        }

        [Fact]
        public void ViewAPerson_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            SetUpDependencies();
            var nonExistingId = Guid.NewGuid();
            _personServiceMock.Setup(s => s.GetPerson(nonExistingId)).Returns((Person)null);
            var controller = CreateController();

            // Act
            var result = controller.ViewAPerson(nonExistingId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _personServiceMock.Verify(s => s.GetPerson(nonExistingId), Times.Once);
        }

        [Fact]
        public void EditAPerson_Get_ExistingId_ReturnsViewWithPerson()
        {
            // Arrange
            SetUpDependencies();
            var person = PeopleDataSamples.GetSamplePeople().First();
            _personServiceMock.Setup(s => s.GetPerson(person.Id)).Returns(person);
            var controller = CreateController();

            // Act
            var result = controller.EditAPerson(person.Id);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeEquivalentTo(person);
            _personServiceMock.Verify(s => s.GetPerson(person.Id), Times.Once);
        }

        [Fact]
        public void EditAPerson_Get_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            SetUpDependencies();
            var id = Guid.NewGuid();
            _personServiceMock.Setup(s => s.GetPerson(id)).Returns((Person)null);
            var controller = CreateController();

            // Act
            var result = controller.EditAPerson(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            _personServiceMock.Verify(s => s.GetPerson(id), Times.Once);
        }

        [Fact]
        public void EditAPerson_Post_ValidUpdate_ReturnsRedirectToIndex()
        {
            // Arrange
            SetUpDependencies();
            var person = PeopleDataSamples.GetSamplePeople().First();
            _personServiceMock.Setup(s => s.UpdatePerson(person.Id, person)).Returns(true);
            var controller = CreateController();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());


            // Act
            var result = controller.EditAPerson(person);

            // Assert
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectResult.ActionName.Should().Be("Index");
            controller.TempData["Message"].Should().Be($"Person {person.FullName} edited successfully.");
            _personServiceMock.Verify(s => s.UpdatePerson(person.Id, person), Times.Once);
        }

        [Fact]
        public void EditAPerson_Post_UpdateReturnsFalse_ReturnsNotFound()
        {
            // Arrange
            SetUpDependencies();
            var person = PeopleDataSamples.GetSamplePeople().First();
            _personServiceMock.Setup(s => s.UpdatePerson(person.Id, person)).Returns(false);
            var controller = CreateController();


            // Act
            var result = controller.EditAPerson(person);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            _personServiceMock.Verify(s => s.UpdatePerson(person.Id, person), Times.Once);
        }

        [Fact]
        public void EditAPerson_Post_ThrowsException_ReturnsRedirectToIndex()
        {
            // Arrange
            SetUpDependencies();
            var person = PeopleDataSamples.GetSamplePeople().First();
            _personServiceMock.Setup(s => s.UpdatePerson(person.Id, person)).Throws<Exception>();
            var controller = CreateController();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.EditAPerson(person);

            // Assert
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectResult.ActionName.Should().Be("Index");
            controller.TempData["Message"].Should().Be($"Error editing person {person.FullName}.");

            _personServiceMock.Verify(s => s.UpdatePerson(person.Id, person), Times.Once);
        }

        [Fact]
        public void DeleteAPerson_ValidId_DeletesAndRedirects()
        {
            // Arrange
            SetUpDependencies();
            var person = PeopleDataSamples.GetSamplePeople().First();
            _personServiceMock.Setup(s => s.GetFullName(person.Id)).Returns(person.FullName);
            _personServiceMock.Setup(s => s.DeletePerson(person.Id));
            var controller = CreateController();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.DeleteAPerson(person.Id);

            // Assert
            var redirect = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirect.ActionName.Should().Be("Index");
            controller.TempData["Message"].Should().Be($"Person {person.FullName} deleted successfully.");

            _personServiceMock.Verify(s => s.GetFullName(person.Id), Times.Once);
            _personServiceMock.Verify(s => s.DeletePerson(person.Id), Times.Once);
        }

        [Fact]
        public void DeleteAPerson_ExceptionThrown_ReturnsRedirectToIndexWithError()
        {
            // Arrange
            SetUpDependencies();
            var person = PeopleDataSamples.GetSamplePeople().First();
            _personServiceMock.Setup(s => s.GetFullName(person.Id)).Returns(person.FullName);
            _personServiceMock.Setup(s => s.DeletePerson(person.Id)).Throws<Exception>();
            var controller = CreateController();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.DeleteAPerson(person.Id);

            // Assert
            var redirect = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirect.ActionName.Should().Be("Index");
            controller.TempData["Message"].Should().Be($"Error deleting person {person.FullName}.");

            _personServiceMock.Verify(s => s.GetFullName(person.Id), Times.Once);
            _personServiceMock.Verify(s => s.DeletePerson(person.Id), Times.Once);
        }

        [Fact]
        public void ExportToExcel_ReturnsFile_WhenServiceSucceeds()
        {
            // Arrange
            SetUpDependencies();
            var fileResult = new FileContentResult(new byte[] { 1, 2, 3 }, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "people.xlsx"
            };

            _personServiceMock.Setup(s => s.ExportToExcel()).Returns(fileResult);
            var controller = CreateController();

            // Act
            var result = controller.ExportToExcel();

            // Assert
            var file = result.Should().BeOfType<FileContentResult>().Subject;
            file.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            file.FileDownloadName.Should().Be("people.xlsx");

            _personServiceMock.Verify(s => s.ExportToExcel(), Times.Once);
        }

        [Fact]
        public void ExportToExcel_ThrowsException_ReturnsBadRequestWithTempDataMessage()
        {
            // Arrange
            SetUpDependencies();
            _personServiceMock.Setup(s => s.ExportToExcel()).Throws<Exception>();
            var controller = CreateController();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.ExportToExcel();

            // Assert
            result.Should().BeOfType<BadRequestResult>();
            controller.TempData["Message"].Should().Be("Error generating Excel file.");

            _personServiceMock.Verify(s => s.ExportToExcel(), Times.Once);
        }

    }
}

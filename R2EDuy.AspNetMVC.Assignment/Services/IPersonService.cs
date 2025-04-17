using Microsoft.AspNetCore.Mvc;
using R2EDuy.AspNetMVC.Assignment.Models;

namespace R2EDuy.AspNetMVC.Assignment.Services
{
    public interface IPersonService
    {
        Person? GetPerson(Guid id);
        List<Person> GetAllPeople();
        bool CreatePerson(Person user);
        bool UpdatePerson(Guid id, Person user);
        bool DeletePerson(Guid id);
        List<Person> GetMales();
        Person? GetOldestPerson();
        string? GetFullName(Guid id);
        List<string> GetFullNames();
        List<Person> FilterByBirthYear(int year, string choice);
        PagedResult<Person> GetPagedPersons(int pageNumber, int pageSize);
        FileContentResult ExportToExcel();
    }
}

using ClosedXML.Excel;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using R2EDuy.AspNetMVC.Assignment.Models;
using R2EDuy.AspNetMVC.Assignment.Services;

public class PersonServiceImplement : IPersonService
{
    private IPersonRepository _personRepository { get; set; }

    public PersonServiceImplement(IPersonRepository personRepository)
    {
        _personRepository = personRepository;

    }

    public Person? GetPerson(Guid id)
    {
        return _personRepository.GetById(id);
    }

    public List<Person> GetAllPeople()
    {
        return _personRepository.GetAll();
    }

    public bool CreatePerson(Person person)
    {
        if (person == null)
        {
            throw new ArgumentNullException(nameof(person));
        }


        _personRepository.Add(person);
        return true;
    }

    public bool UpdatePerson(Guid id, Person person)
    {
        if (person == null)
        {
            throw new ArgumentNullException(nameof(person));
        }

        var existingPerson = _personRepository.GetById(id);
        if (existingPerson != null)
        {
            person.Id = existingPerson.Id;
            _personRepository.Update(person);
            return true;
        }
        return false;
    }

    public bool DeletePerson(Guid id)
    {
        var existingPerson = _personRepository.GetById(id);
        if (existingPerson == null)
        {
            return false;
        }
        _personRepository.Delete(id);
        return true;
    }

    public List<Person> GetMales()
    {
        return _personRepository.GetAll().Where(p => p.Gender == "Male").ToList();
    }

    public Person? GetOldestPerson()
    {
        return _personRepository.GetAll().OrderBy(p => p.DateOfBirth).FirstOrDefault();
    }
    public string? GetFullName(Guid id)
    {
        var person = _personRepository.GetById(id);
        return person?.FullName;
    }

    public List<string> GetFullNames()
    {
        return _personRepository.GetAll().Select(p => p.FullName).ToList();
    }

    public List<Person> FilterByBirthYear(int year, string choice)
    {
        if (year < 0 || year > DateTime.Now.Year)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Year must be between 0 and the current year.");
        }
        var people = _personRepository.GetAll();
        switch (choice)
        {
            case "equal":
                return people.Where(p => p.DateOfBirth.Year == year).ToList();
            case "greater":
                return people.Where(p => p.DateOfBirth.Year > year).ToList();
            case "less":
                return people.Where(p => p.DateOfBirth.Year < year).ToList();
            default:
                return people;
        }
    }

    public FileContentResult ExportToExcel()
    {
        var people = _personRepository.GetAll();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("People");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "First Name";
            worksheet.Cell(1, 3).Value = "Last Name";
            worksheet.Cell(1, 4).Value = "Gender";
            worksheet.Cell(1, 5).Value = "Date of Birth";
            worksheet.Cell(1, 6).Value = "Phone Number";
            worksheet.Cell(1, 7).Value = "Birth Place";
            worksheet.Cell(1, 8).Value = "Graduated";

            int row = 2;
            foreach (var person in people)
            {
                worksheet.Cell(row, 1).Value = person.Id.ToString();
                worksheet.Cell(row, 2).Value = person.FirstName;
                worksheet.Cell(row, 3).Value = person.LastName;
                worksheet.Cell(row, 4).Value = person.Gender;
                worksheet.Cell(row, 5).Value = person.DateOfBirth.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 6).Value = person.PhoneNumber;
                worksheet.Cell(row, 7).Value = person.BirthPlace;
                worksheet.Cell(row, 8).Value = person.IsGraduated ? "Yes" : "No";
                row++;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                stream.Position = 0;

                return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "PeopleData.xlsx"
                };
            }
        }
    }

    public PagedResult<Person> GetPagedPersons(int pageNumber, int pageSize)
    {
        var persons = _personRepository.GetAll();
        var pagedPersons = persons.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var totalRecords = persons.Count();

        return new PagedResult<Person>
        {
            Items = pagedPersons,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords
        };

    }
}

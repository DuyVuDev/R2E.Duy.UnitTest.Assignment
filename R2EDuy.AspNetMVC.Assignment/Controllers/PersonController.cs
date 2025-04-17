using Microsoft.AspNetCore.Mvc;
using R2EDuy.AspNetMVC.Assignment.Models;
using R2EDuy.AspNetMVC.Assignment.Services;

[Route("NashTech/Rookies/[action]")]
public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private const int PAGE_SIZE = 8;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    public IActionResult Index(int pageNumber = 1, int pageSize = PAGE_SIZE)
    {
        var pagedResult = _personService.GetPagedPersons(pageNumber, pageSize);
        return View(pagedResult);
    }

    public IActionResult Males()
    {
        var males = _personService.GetMales();
        return View(males);
    }

    public IActionResult Oldest()
    {
        var oldest = _personService.GetOldestPerson();
        return View(oldest);
    }

    public IActionResult FullNames()
    {
        var fullNames = _personService.GetFullNames();
        return View(fullNames);
    }

    public IActionResult FilterByBirthYear(int year, string choice)
    {
        var result = _personService.FilterByBirthYear(year, choice);
        ViewBag.Year = year;
        ViewBag.Choice = choice;
        return View(result);
    }

    [HttpGet]
    public IActionResult AddAPerson()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddAPerson(Person newPerson)
    {
        string fullName = newPerson.FullName;
        try
        {
            _personService.CreatePerson(newPerson);
            TempData["Message"] = $"Person {fullName} added successfully.";
        }
        catch (Exception)
        {
            TempData["Message"] = $"Error adding person {fullName}.";
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ViewAPerson(Guid id)
    {
        var person = _personService.GetPerson(id);
        if (person == null)
        {
            return NotFound();
        }
        return View(person);
    }

    [HttpGet]
    public IActionResult EditAPerson(Guid id)
    {
        var person = _personService.GetPerson(id);
        if (person == null)
        {
            return NotFound();
        }
        return View(person);
    }

    [HttpPost]
    public IActionResult EditAPerson(Person updatedPerson)
    {
        Guid id = updatedPerson.Id;
        string fullName = updatedPerson.FullName;
        try
        {
            bool state = _personService.UpdatePerson(id, updatedPerson);
            if (state)
            {
                TempData["Message"] = $"Person {fullName} edited successfully.";
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception)
        {
            TempData["Message"] = $"Error editing person {fullName}.";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeleteAPerson(Guid id)
    {
        string fullName = _personService.GetFullName(id);
        try
        {
            _personService.DeletePerson(id);
            TempData["Message"] = $"Person {fullName} deleted successfully.";
        }
        catch (Exception)
        {
            TempData["Message"] = $"Error deleting person {fullName}.";
        }
        return RedirectToAction("Index");
    }

    public IActionResult ExportToExcel()
    {
        try { return _personService.ExportToExcel(); }
        catch (Exception)
        {
            TempData["Message"] = $"Error generating Excel file.";
            return BadRequest();
        }
    }
}

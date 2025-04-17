using R2EDuy.AspNetMVC.Assignment.Models;

public interface IPersonRepository
{
    Person? GetById(Guid id);
    List<Person> GetAll();
    void Add(Person person);
    void Update(Person person);
    void Delete(Guid id);
}

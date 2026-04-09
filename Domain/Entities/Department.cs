namespace Domain.Entities;

public class Department
{
    private readonly List<Employee> _employees = new();

    public int Id { get; }
    public string Name { get; private set; }
    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

    public Department(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
    }
}

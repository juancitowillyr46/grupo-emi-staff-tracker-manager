namespace Domain.Entities;

public class Project
{
    private readonly List<EmployeeProject> _employeeProjects = new();

    public int Id { get; }
    public string Name { get; private set; }
    public IReadOnlyCollection<EmployeeProject> EmployeeProjects => _employeeProjects.AsReadOnly();

    public Project(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public void AddEmployee(Employee employee)
    {
        _employeeProjects.Add(new EmployeeProject(employee.Id, Id));
    }
}

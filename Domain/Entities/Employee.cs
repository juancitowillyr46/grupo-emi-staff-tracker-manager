namespace Domain.Entities;

public class Employee
{
    private readonly List<PositionHistory> _positionHistories = new();
    private readonly List<EmployeeProject> _employeeProjects = new();

    public int Id { get; }
    public string Name { get; private set; }
    public int CurrentPosition { get; private set; }
    public decimal Salary { get; private set; }
    public int DepartmentId { get; private set; }
    public Department? Department { get; private set; }
    public IReadOnlyCollection<PositionHistory> PositionHistories => _positionHistories.AsReadOnly();
    public IReadOnlyCollection<EmployeeProject> EmployeeProjects => _employeeProjects.AsReadOnly();

    public Employee(int id, string name, int currentPosition, decimal salary, int departmentId)
    {
        Id = id;
        Name = name;
        CurrentPosition = currentPosition;
        Salary = salary;
        DepartmentId = departmentId;
    }

    public decimal CalculateAnnualBonus()
    {
        return CurrentPosition == 1 ? Salary * 0.10m : Salary * 0.20m;
    }

    public void AddPositionHistory(PositionHistory history)
    {
        _positionHistories.Add(history);
    }

    public void SetDepartment(int departmentId)
    {
        DepartmentId = departmentId;
    }

    public void UpdateDetails(string name, int currentPosition, decimal salary, int departmentId)
    {
        Name = name;
        CurrentPosition = currentPosition;
        Salary = salary;
        DepartmentId = departmentId;
    }

    public void AddProject(Project project)
    {
        _employeeProjects.Add(new EmployeeProject(Id, project.Id));
    }
}

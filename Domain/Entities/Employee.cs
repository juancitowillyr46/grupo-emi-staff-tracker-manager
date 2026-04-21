namespace Domain.Entities;

public class Employee
{
    private readonly List<PositionHistory> _positionHistories = new();
    private readonly List<EmployeeProject> _employeeProjects = new();

    public int Id { get; private set; }
    public string Name { get; private set; }
    public int CurrentPosition { get; private set; }
    public Position? CurrentPositionInfo { get; private set; }
    public decimal Salary { get; private set; }
    public int DepartmentId { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Department? Department { get; private set; }
    public IReadOnlyCollection<PositionHistory> PositionHistories => _positionHistories.AsReadOnly();
    public IReadOnlyCollection<EmployeeProject> EmployeeProjects => _employeeProjects.AsReadOnly();

    public Employee(string name, int currentPosition, decimal salary, int departmentId)
    {
        Name = name;
        CurrentPosition = currentPosition;
        Salary = salary;
        DepartmentId = departmentId;
    }

    public decimal CalculateAnnualBonus()
    {
        return CurrentPositionInfo?.IsManager == true ? Salary * 0.20m : Salary * 0.10m;
    }

    public void AddPositionHistory(PositionHistory history)
    {
        _positionHistories.Add(history);
    }

    public void SetDepartment(int departmentId)
    {
        DepartmentId = departmentId;
    }

    public void UpdateDetails(string name, decimal salary, int departmentId)
    {
        Name = name;
        Salary = salary;
        DepartmentId = departmentId;
    }

    public void SetCurrentPosition(Position position)
    {
        InitializeCurrentPosition(position, DateTime.UtcNow);
    }

    public void InitializeCurrentPosition(Position position, DateTime startedAt)
    {
        ArgumentNullException.ThrowIfNull(position);

        CurrentPosition = position.Id;
        CurrentPositionInfo = position;
        AddPositionHistory(new PositionHistory(Id, position.Name, startedAt));
    }

    public void ChangeCurrentPosition(Position position, DateTime changedAt)
    {
        ArgumentNullException.ThrowIfNull(position);

        if (CurrentPosition == position.Id)
        {
            CurrentPositionInfo = position;
            return;
        }

        var currentHistory = _positionHistories.LastOrDefault(x => x.EndDate is null);
        currentHistory?.EndPosition(changedAt);

        CurrentPosition = position.Id;
        CurrentPositionInfo = position;
        AddPositionHistory(new PositionHistory(Id, position.Name, changedAt));
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public void AddProject(Project project)
    {
        _employeeProjects.Add(new EmployeeProject(Id, project.Id));
    }
}

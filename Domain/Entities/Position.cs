namespace Domain.Entities;

public class Position
{
    private readonly List<Employee> _employees = new();

    public int Id { get; }
    public string Name { get; private set; }
    public PositionType Type { get; private set; }
    public bool IsManager => Type == PositionType.Manager;
    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

    public Position(int id, string name, PositionType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }
}

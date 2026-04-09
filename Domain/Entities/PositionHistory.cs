namespace Domain.Entities;

public class PositionHistory
{
    public int Id { get; private set; }
    public int EmployeeId { get; }
    public string Position { get; }
    public DateTime StartDate { get; }
    public DateTime? EndDate { get; private set; }
    public Employee? Employee { get; private set; }

    public PositionHistory(int employeeId, string position, DateTime startDate, DateTime? endDate = null)
    {
        EmployeeId = employeeId;
        Position = position;
        StartDate = startDate;
        EndDate = endDate;
    }

    public void EndPosition(DateTime endDate)
    {
        EndDate = endDate;
    }
}

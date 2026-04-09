namespace Domain.Entities;

public class EmployeeProject
{
    public int EmployeeId { get; }
    public int ProjectId { get; }

    public Employee? Employee { get; private set; }
    public Project? Project { get; private set; }

    public EmployeeProject(int employeeId, int projectId)
    {
        EmployeeId = employeeId;
        ProjectId = projectId;
    }
}

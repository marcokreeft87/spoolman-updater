namespace Domain;

public class UpdateTrayInput : IInput
{
    public int SpoolId { get; set; }
    public string ActiveTrayId { get; set; }
}
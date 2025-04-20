namespace Domain;

public class UpdateSpoolInput : IInput
{
    public string Name { get; set; }
    public string Color { get; set; }
    public string Material { get; set; }
    public string TagUid { get; set; }
    public float UsedWeight { get; set; }
    public string ActiveTrayId { get; set; }
}
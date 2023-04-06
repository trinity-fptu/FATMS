namespace Application.ViewModels.SyllabusViewModels;

public class UpdateUnitInSyllabuViewModel
{
    public int SyllabusId { get; set; }
    public int UnitId { get; set; }
    public int Session { get; set; }
    public int Duration { get; set; }
    
    public string LastModifiedOn { get; set; }
    public string LastModifiedBy { get; set; }
}
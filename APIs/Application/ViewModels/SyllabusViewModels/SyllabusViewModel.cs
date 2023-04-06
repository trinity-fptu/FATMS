namespace Application.ViewModels.SyllabusViewModels
{
    public class SyllabusViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Version { get; set; }
        public string Code { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public int DaysDuration { get; set; }
        public int TimeDuration { get; set; }
        public IEnumerable<string> OutputStandardCode { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public string LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        
    }
}

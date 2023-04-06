using Domain.Enums.ClassEnums;

namespace Application.ViewModels.ClassViewModels
{

    public class CreateClassViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ClassLocation? Location { get; set; }
        public ClassAttendeeType? AttendeeType { get; set; }
        public ClassFSU? FSU { get; set; }
        public ClassTime ClassTime { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime FinishedOn { get; set; }

        public string? LectureStartedTime { get; set; }
        public string? LectureFinishedTime { get; set; }
        /*public User? ApprovedAdmin { get; set; }
        public User? CreatedAdmin { get; set; }*/
        public int? TrainingProgramId { get; set; }
        /*public TrainingProgram? TrainingProgram { get; set; }*/
       

        /*public ICollection<ClassUsers> ClassUsers { get; set; }*/

    }
}

using Domain.Enums.LectureEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AddLectureViewModels
{
    public class AddLectureViewModel
    {
        //public int UnitId { get; set; }
        public string NameLecture { get; set; }
        public int? Duration { get; set; }
        public int? OutputStandardId { get; set; }
        public bool LessonType { get; set; }
        // public LectureLessonType LessonType { get; set; }
        public LectureDeliveryType? DeliveryType { get; set; }
    }
}

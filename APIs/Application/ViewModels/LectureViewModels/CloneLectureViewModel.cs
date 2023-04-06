using Application.ViewModels.OutputStandardViewModels;
using Domain.Enums.LectureEnums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.LectureViewModels
{
    public class CloneLectureViewModel
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public LectureLessonType LessonType { get; set; }
        public LectureDeliveryType DeliveryType { get; set; }
        public int? OutputStandardId { get; set; }
    }
}

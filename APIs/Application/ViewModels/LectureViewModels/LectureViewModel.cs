using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.LectureViewModels
{
    public class LectureViewModel
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public int? OutputStandardId { get; set; }
        public string LessonType { get; set; }
        public string DeliveryType { get; set; }
    }
}

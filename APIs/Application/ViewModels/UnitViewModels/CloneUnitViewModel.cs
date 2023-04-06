using Application.ViewModels.LectureViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UnitViewModels
{
    public class CloneUnitViewModel
    {
        public string Name { get; set; }
        public int Session { get; set; }
        public bool isDeleted { get; set; } = false;
        public int Duration { get; set; }

        public ICollection<CloneLectureViewModel> Lectures { get; set; } = new List<CloneLectureViewModel>();
    }
}

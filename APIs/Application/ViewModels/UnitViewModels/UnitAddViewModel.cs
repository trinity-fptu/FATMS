using System.Collections.Generic;
using Application.ViewModels.AddLectureViewModels;
using Domain.Models;

namespace Application.ViewModels.UnitViewModels
{
    public class UnitAddViewModel : BaseUnitViewModel
    {
        public ICollection<AddLectureViewModel> Lectures { get; set; }
    }
}
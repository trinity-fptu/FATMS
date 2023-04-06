using Application.ViewModels.LectureViewModels;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UnitViewModels
{
    public class ImportUnitModel
    {
        public int Session { get; set; }
        public string Name { get; set; }
        public ICollection<ImportLectureModel> Lectures { get; set; } = new List<ImportLectureModel>();
    }
}

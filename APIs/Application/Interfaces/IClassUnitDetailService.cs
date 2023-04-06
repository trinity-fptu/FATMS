using Application.ViewModels.ClassUnitViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClassUnitDetailService
    {
        Task<String> EditListClassUnitAsync(List<EditClassUnitViewModel> ListClassUnit, int classId);
        Task<bool> AddClassUnitAsync(AddClassUnitViewModel addClassUnitViewModel);
    }
}

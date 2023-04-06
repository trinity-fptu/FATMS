using Application.ViewModels.ClassUnitViewModel;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IClassUnitDetailRepository : IGenericRepository<ClassUnitDetail>
    {
        Task<ClassUnitDetail> GetClassUnit(int classId, int unitId);


    }
}

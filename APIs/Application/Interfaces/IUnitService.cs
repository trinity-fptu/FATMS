using Application.ViewModels.UnitViewModels;
using Domain.Models;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitService
    {        
        //Task AddAsync(UnitAddViewModel viewModel);
        Task<UnitDetailModel> GetUnitByIdAsync(int unitId);
    }
}

using Domain.Enums.TMSEnums;
using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.TimeManagementSystemViewModels
{
    public class TMSPendingListViewModel
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Reason { get; set; }
        public string TraineeName { get; set; }
        public string TraineeEmail { get; set; }
        public string TraineePhone { get; set; }

    }
}

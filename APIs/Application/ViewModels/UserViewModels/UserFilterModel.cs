using Domain.Enums.UserEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserViewModels
{
    public class UserFilterModel
    {
        public string FullName { get; set; }
        public string[] DateOfBirth { get; set; }
        public string Level { get; set; } = null;
        public string Status { get; set; } = null;
        public bool? IsMale { get; set; } 
    }
}

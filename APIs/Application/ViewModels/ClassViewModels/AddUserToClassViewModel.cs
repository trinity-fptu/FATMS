﻿using Domain.Enums.ClassUserEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ClassViewModels
{
    public class AddUserToClassViewModel
    {
        public ClassRole Role { get; set; }
    public int ClassId { get; set; }
        public int UserId { get; set; }
    }
}

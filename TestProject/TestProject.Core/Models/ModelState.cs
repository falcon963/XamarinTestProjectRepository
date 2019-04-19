﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Core.Models
{
    public class ModelState
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public class Assignment
    {
        [Key]
        public int IdAssignment { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public int IdStudent { get; set; }
    }
}

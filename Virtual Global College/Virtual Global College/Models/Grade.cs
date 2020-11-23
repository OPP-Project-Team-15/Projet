using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    class Grade
    {
        [Key]
        public int IdGrade { get; set; }
        public string Subject { get; set; }
        public decimal Mark { get; set; }
        public int IdStudent { get; set; }
    }
}

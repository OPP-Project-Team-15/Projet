using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    class Attendance
    {
        [Key]
        public int IdAttendance { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public int IdStudent { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public class Admin : User
    {
        public string[,] TimetableOfClass { get; set; }

        public Admin(string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, string[,] timetableOfClass) 
        : base (name, surname, id, phoneNumber, sexe, mail, password)
        {
            TimetableOfClass = timetableOfClass;
        }
    }
}

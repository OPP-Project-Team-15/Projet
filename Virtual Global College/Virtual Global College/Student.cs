using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public class Student : User
    {
        private string branch;
        private string[,] timetable;

        public Student(string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, string branch, string[,] timetable)
            : base(name, surname, id, phoneNumber, sexe, mail, password)
        {
            Branch = branch;
            Timetable = timetable;
        }

        public string Branch
        {
            get { return branch; }
            set
            {
                if (value != "ESILV" || value != "EMLV" || value != "IIM")
                    throw new Exception("This branch doesn't exist");
                else
                    branch = value;
            }
        }
        public string[,] Timetable
        {
            get { return timetable; }
            set
            {
                for (int i = 0; i < timetable.GetLength(0); i++)
                    for (int j = 0; j < timetable.GetLength(1); j++)
                    {
                        if (timetable[i, j] == null)
                            throw new Exception("The time slot [" + i + "," + j + "] isn't valid");
                    }
                timetable = value;
            }
        }

        public override string ToString() => $"{base.ToString()}\n\nType : Student\n Branch : {branch}";

        public void CourseRegistration()
        {

        }
    }
}

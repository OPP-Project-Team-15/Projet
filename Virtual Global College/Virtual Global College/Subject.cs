using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public class Subject : IComparable
    {
        public string NameSubject { get; set; }
        public string Branch { get; set; }
        public string Day { get; set; }
        public string Hours { get; set; }

        public Subject(string nameSubj, string bran, string day, string hours)
        {
            NameSubject = nameSubj;
            Branch = bran;
            Hours = hours;
            Day = day;
        }

        public int CompareTo(object obj)
        {
            Subject c = (Subject)obj;
            return String.Compare(this.NameSubject, c.NameSubject);
        }
    }
}

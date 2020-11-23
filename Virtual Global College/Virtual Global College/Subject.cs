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
        public string Number { get; set; }

        public Subject(string nameSubj, string bran, string day, string hours, string number)
        {
            NameSubject = nameSubj;
            Branch = bran;
            Hours = hours;
            Day = day;
            Number = number;
        }

        public int CompareTo(object obj)
        {
            Subject c = (Subject)obj;
            return String.Compare(this.Number, c.Number);
        }
    }
}

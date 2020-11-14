using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public class Teacher : User
    {
        public string Subject { get; set; }

        public Teacher (string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, string subject)
            : base(name, surname, id, phoneNumber, sexe, mail, password)
        {
            Subject = subject;
        }

        public override string ToString() => $"{base.ToString()}\n\nSubject : {Subject}";

        public void Add_Assignment()
        {
            Console.WriteLine("Specify the month number of the assignment");
            int month = Convert.ToInt32(Console.ReadLine());
            //if ()
        }
    }
}

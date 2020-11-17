using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public class Admin : User
    {
        public SortedList<string[,], List<Student>> TimetableOfClass { get; set; }

        public Admin(string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, SortedList<string[,], List<Student>> timetableOfClass) 
        : base (name, surname, id, phoneNumber, sexe, mail, password)
        {
            TimetableOfClass = timetableOfClass;
        }

        public string CreateTimetable()
        {
            Console.WriteLine("Specify the month number of the course :");
            int month = Convert.ToInt32(Console.ReadLine());
            while (month < 1 && month > 12)
            {
                Console.WriteLine("Please write a mounth number between 1 and 12");
                month = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("\nSpecify the day number of the course :");
            int day = Convert.ToInt32(Console.ReadLine());
            while (day < 1 && day > 31)
            {
                Console.WriteLine("Please write a mounth number between 1 and 12");
                day = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("\nSpecify the year of the course :");
            int year = Convert.ToInt32(Console.ReadLine());
            while (year < 2020)
            {
                Console.WriteLine("Please write a year of 4 numbers, and superior than 2019");
                year = Convert.ToInt32(Console.ReadLine());
            }

            DateTime courseDate = new DateTime(year, month, day);

            Console.WriteLine("\nSpecify the course :");
            string courseContent = Console.ReadLine();

            string course = $"{courseDate.Month}/{courseDate.Day}/{courseDate.Year} {courseContent}";

            return course;
        }
        public void HistoryOfPayment()
        {
            Console.WriteLine(Student.FeesHistory);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public class Admin : User
    {
        public string[,]TimetableOfClass { get; set; }

        public Admin(string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, string[,] timetableOfClass) 
        : base (name, surname, id, phoneNumber, sexe, mail, password)
        {
            TimetableOfClass = timetableOfClass;
        }

        /*public string[,] CreateTimetable()
        {
            int FirstYear = 0;
            int FirstDay = 0;
            int FirstMonth = 0;
            int compt = 0;
            try
            {
                Console.WriteLine("Enter the first year of student Timetable");
                FirstYear = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter the first month of student Timetable");
                FirstMonth = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter the first day of student Timetable");
                FirstDay = Convert.ToInt32(Console.ReadLine());
            }
            catch (DateTime)
            {

            }

        }
        */

        /*
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
        */
        public void HistoryOfPayment()
        {
            Console.WriteLine(Student.FeesHistory);
        }


    }
}

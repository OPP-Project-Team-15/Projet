using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public class Teacher : User
    {
        public List<string> Subjects { get; set; }

        public Teacher (string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, List<string> subject)
            : base(name, surname, id, phoneNumber, sexe, mail, password)
        {
            Subjects = subject;
        }

        public override string ToString() => $"{base.ToString()}\n\nSubject : {Subjects}";

        public string CreateAssignment()
        {
            Console.WriteLine("Specify the month number of the assignment :");
            int month = Convert.ToInt32(Console.ReadLine());
            while (month < 1 && month > 12)
            {
                Console.WriteLine("Please write a mounth number between 1 and 12");
                month = Convert.ToInt32(Console.ReadLine());
            }
            
            Console.WriteLine("\nSpecify the day number of the assignment :");
            int day = Convert.ToInt32(Console.ReadLine());
            while (day < 1 && day > 31)
            {
                Console.WriteLine("Please write a mounth number between 1 and 12");
                day = Convert.ToInt32(Console.ReadLine());
            }
            
            Console.WriteLine("\nSpecify the year of the assignment :");
            int year = Convert.ToInt32(Console.ReadLine());
            while (year < 2020)
            {
                Console.WriteLine("Please write a year of 4 numbers, and superior than 2019");
                year = Convert.ToInt32(Console.ReadLine());
            }

            DateTime assignmentDate = new DateTime(year, month, day);

            Console.WriteLine("\nSpecify the assignment :");
            string assignmentContent = Console.ReadLine();

            string assignment = $"{assignmentDate.Month}/{assignmentDate.Day}/{assignmentDate.Year} {assignmentContent}";

            return assignment;
        }

        public void PublishGrade(Student studentGrade)
        {
            bool subjectExist = false;
            string choice = "";
            while(subjectExist == false)
            {
                Console.WriteLine("\nIn which subject do you want to put grade ?");
                choice = Console.ReadLine();
                subjectExist = Subjects.Contains(choice);
            }
            Console.WriteLine("\nGive a name for the assignment :");
            string nameAssignment = Console.ReadLine();
            Console.WriteLine("\nGive a grade for " + studentGrade.Name + " for " + choice);
            int grade = Convert.ToInt32(Console.ReadLine());
            // Ici ajouter les informations aux fichiers notes 
        }
    }
}

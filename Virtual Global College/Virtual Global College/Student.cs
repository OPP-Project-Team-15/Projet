using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    interface ISchool
    {
        string SchoolName {get;}
        string SchoolInfo();
    }
    public class Student : User
    {
        private string branch;
        private string[,] timetable;
        private string schoolName;

        public Student(string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, string branch, string[,] timetable)
            : base(name, surname, id, phoneNumber, sexe, mail, password)
        {
            Branch = branch;
            Timetable = timetable;
        }
        
        public string SchoolName
        {
            get {return schoolName;}
        }
        public string Branch
        {
            get { return branch; }
            set
            {
                if (value != "ESILV" && value != "EMLV" && value != "IIM")
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
                for (int i = 0; i < value.GetLength(0); i++)
                    for (int j = 0; j < value.GetLength(1); j++)
                    {
                        if (value[i, j] == null)
                            throw new Exception("The time slot [" + i + "," + j + "] isn't valid");
                    }
                timetable = value;
            }
        }
        public string[] CoursesPicked { get; set; }

        public override string ToString() => $"{base.ToString()}\n\nType : Student\n Branch : {branch}";

        public void CourseRegistration(List<string> courses)
        {
            string[] picks = new string[2];

            Console.WriteLine("Which courses would you want to pick ?\n");
            int compt = 1;
            foreach (string element in courses)
            {
                Console.WriteLine($"{compt} - {element}");
                compt++;
            }
            Console.WriteLine();

            string answer;
            answer = Console.ReadLine();


            for (int i = 0; i < 2; i++)
            {
                while (courses.Contains(answer) != true)
                {
                    Console.WriteLine("This course doesn't exist. Please select another one\n");
                    answer = Console.ReadLine();
                }
                Console.WriteLine($"The course {answer} has been taken\n");
                picks[i] = answer;

                if (i < 2)
                    answer = Console.ReadLine();
            }

            Console.Write($"Thank you for your participation");
            this.CoursesPicked = picks;
        }
    }
}

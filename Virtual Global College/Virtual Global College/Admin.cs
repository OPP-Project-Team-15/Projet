using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Virtual_Global_College
{
    public class Admin : User
    {
        private SortedList<Subject, List<Student>> SubjectStudent { get; set; }

        public Admin(string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, SortedList<Subject, List<Student>> sujectStud) 
        : base (name, surname, id, phoneNumber, sexe, mail, password)
        {
            SubjectStudent = sujectStud;
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

        /// <summary>
        /// Put the subject mandatory in the timetable of the student
        /// </summary>
        public void SubjectMandatory()
        {
            foreach (KeyValuePair<Subject, List<Student>> list in SubjectStudent)
            {
                foreach (Student stud in list.Value)
                {
                    foreach (string[,] timetable in stud.timetablePerWeek)
                    {
                        string[] tab = new string[2] { list.Key.Hours, list.Key.Day };
                        int[] i = SearchTheIndexOfAnXAndYofMatrix(timetable, tab);
                        if (i[0] != 0 && i[1] != 0)
                        {
                            timetable[i[0], i[1]] = list.Key.NameSubject;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To find the index x and y in a matrix
        /// </summary>
        /// <param name="timetab"></param>
        /// <param name="cdx"></param>
        /// <returns></returns>
        public int[] SearchTheIndexOfAnXAndYofMatrix(string[,] timetab, string[] cdx)
        {
            int[] index = new int[2];
            for (int index1 = 0; index1 < cdx.Length; index1++)
            {
                for (int index2 = 0; index2 < timetab.GetLength(0); index2++)
                {
                    for (int index3 = 0; index3 < timetab.GetLength(1); index3++)
                    {
                        if (index1 == 0 && timetab[index2,index3] == cdx[0])
                        {
                            index[index1] = index2;
                        }
                        else if (index1 == 1 && timetab[index2, index3] == cdx[1])
                        {
                            index[index1] = index3;
                        }
                    }
                }
            }
            return index;
        }

        /// <summary>
        /// Allow the admin to create a course
        /// </summary>
        public void CreatingCourseOrModifyTimetable()
        {
            
        }

        public void CreateExam()
        {
            string branc = "";
            while (branc != "ESILV" || branc != "EMLV" || branc != "IIM")
            {
                Console.WriteLine("Give the name of the branch. The branch must exist.");
                branc = Console.ReadLine();
            }

            string subj = "";
            bool subjExist = false;
            while (subjExist != true)
            {
                Console.WriteLine("Specify the subject of the assignment :");
                subj = Console.ReadLine();
                for (int index1 = 0; index1 < SubjectStudent.Count && subjExist != true; index1++)
                {
                    subjExist = SubjectStudent.ElementAt(index1).Key.NameSubject.Contains(subj);
                }
            }

            Console.WriteLine("Specify the week of the assignment :");
            string week = "week ";
            int number = 0;
            while (number < 1 && number > 30)
            {
                Console.WriteLine("Please write a number week between 1 and 30 :");
                number = Convert.ToInt32(Console.ReadLine());
            }
            week = week + number;

            string day = "";
            while (day != "Monday" || day != "Tuesday" || day != "Wednesday" || day != "Thursday" || day != "Friday" || day != "Saturday" || day != "Sunday")
            {
                Console.WriteLine("Specify the day of the assignment :");
                day = Console.ReadLine();
            }

            int num = 0;
            string hour = "";
            while (num < 7 && num > 21)
            {
                Console.WriteLine("Specify the start of the assignment. It can start at minimum at 7 and maximum at 21. Every assignment last one hour. You must give exactly one hour.");
                num = Convert.ToInt32(Console.ReadLine());
            }
            hour = num + " - " + Convert.ToString(num + 1);

            string[] assignment = new string[5] { branc, subj, week, day, hour };


            foreach (KeyValuePair<Subject, List<Student>> list in SubjectStudent)
            {
                if (list.Key.Branch == branc)
                {
                    foreach (Student stud in list.Value)
                    {
                        foreach (string[,] timetable in stud.timetablePerWeek)
                        {
                            if (timetable[0, 8] == week)
                            {
                                string[] tab = new string[2] { hour, day };
                                int[] i = SearchTheIndexOfAnXAndYofMatrix(timetable, tab);
                                if (i[0] != 0 && i[1] != 0)
                                {
                                    timetable[i[0], i[1]] = "Assignment : " + subj;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void HistoryOfPayment()
        {
            Console.WriteLine(Student.FeesHistory);
        }
    }
}

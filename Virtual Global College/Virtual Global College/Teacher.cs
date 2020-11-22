using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public class Teacher : User
    {
        public SortedList<Subject, List<Student>> SubjectStudent { get; set; }
        public SortedList<Subject, List<string[,]>> Grade { get; set; }

        public Teacher (string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, SortedList<Subject, List<Student>> subject)
            : base(name, surname, id, phoneNumber, sexe, mail, password)
        {
            SubjectStudent = subject;

        }

        public override string ToString() => $"{base.ToString()}\n\nSubject : {SubjectStudent}";

        public void CreateAssignment()
        {
            string branc = "";
            while(branc != "ESILV" || branc != "EMLV" || branc != "IIM")
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
            hour = num + " - " + Convert.ToString(num+1);

            string[] assignment = new string[5] {branc, subj, week, day, hour};
            

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
                                    timetable[i[0], i[1]] = subj;
                                }
                            }
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
                        if (index1 == 0 && timetab[index2, index3] == cdx[0])
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
        /// Allow the teacher to publish a new grade for a specific subject and a specific student
        /// </summary>
        /// <param name="studentGrade"></param>
        public void PublishGrade(Student studentGraded)
        {
            bool subjectExist = false;
            string choice = "";
            while(subjectExist == false)
            {
                Console.WriteLine("\nIn which subject do you want to put grade ?");
                choice = Console.ReadLine();
                for (int index1 = 0; index1 < SubjectStudent.Count && subjectExist != true; index1++)
                {
                    subjectExist = SubjectStudent.ElementAt(index1).Key.NameSubject.Contains(choice);
                }
            }
            Console.WriteLine("\nGive a name for the assignment :");
            string nameAssignment = Console.ReadLine();
            Console.WriteLine("\nGive a grade for " + studentGraded.Name + " for " + choice);
            int grade = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Give a date for the exam :");
            string date = Console.ReadLine();
            // Ici ajouter les informations aux fichiers notes de la matiere en question
        }

        /// <summary>
        /// Allow the teacher to see all the coordinates of a specific student
        /// </summary>
        public void studentCoordinates()
        {
            bool nameExist = false;
            Student student = null;
            while (nameExist == false)
            {
                Console.WriteLine("From which student do you want to see the coordinate. Write a surname of a student who exist in your class.");
                string surname = Console.ReadLine();
                int index = 0;
                foreach (KeyValuePair<Subject,List<Student>> subj in SubjectStudent)
                {
                    index++;
                    if (nameExist == false)
                    {
                        foreach (Student stud in SubjectStudent.Values[index])
                        {
                            nameExist = stud.Surname.Contains(surname);
                            if (nameExist == true)
                            {
                                student = stud;
                                student.ToString();
                                Console.WriteLine();
                            }
                        }
                    }
                }              

                if(nameExist == true)
                {
                    student.ToString();
                    Console.WriteLine("Do you want to see the coordinates of an other student ? Answer YES or NOT");
                    string answer = Console.ReadLine();
                    while (answer != "YES" || answer != "NOT")
                    {
                        Console.WriteLine("Do you want to see the coordinates of an other student ? Answer YES or NOT");
                        answer = Console.ReadLine();
                    }
                    if (answer == "YES")
                    {
                        nameExist = false;
                    }
                }
            }
        }

        /// <summary>
        /// Allow the teacher to put a new grade 
        /// </summary>
        public void CreateGrade()
        {

            Console.WriteLine("Do you want to publish the grade ?");
        }


        /// <summary>
        /// Allow the teacher to see all the attendance of a specific student
        /// </summary>
        public void Attendance()
        {
            bool nameExist = false;
            Student student = null;
            while (nameExist == false)
            {
                Console.WriteLine("From which student do you want to see the attendance. Write a surname of a student who exist in your class.");
                string surname = Console.ReadLine();
                int index = 0;
                foreach (KeyValuePair<Subject, List<Student>> subj in SubjectStudent)
                {
                    index++;
                    if (nameExist == false)
                    {
                        foreach (Student stud in SubjectStudent.Values[index])
                        {
                            nameExist = stud.Surname.Contains(surname);
                            if (nameExist == true)
                            {
                                student = stud;
                                student.ToStringShowTheAttendance();
                                Console.WriteLine();
                            }
                        }
                    }
                }

                if (nameExist == true)
                {
                    student.ToString();
                    Console.WriteLine("Do you want to see the attendance of an other student ? Answer YES or NOT");
                    string answer = Console.ReadLine();
                    while (answer != "YES" || answer != "NOT")
                    {
                        Console.WriteLine("Do you want to see the attendance of an other student ? Answer YES or NOT");
                        answer = Console.ReadLine();
                    }
                    if (answer == "YES")
                    {
                        nameExist = false;
                    }
                }
            }
        }
    }
}

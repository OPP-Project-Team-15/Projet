﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public class Teacher : User
    {
        public LinkedList<string[,]> GradePerAssignment = new LinkedList<string[,]>();
        public SortedList<Subject, List<Student>> SubjectStudent { get; set; }
        public SortedList<Subject, string[,]> Grade { get; set; }

        public Teacher (string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, SortedList<Subject, List<Student>> subject)
            : base(name, surname, id, phoneNumber, sexe, mail, password)
        {
            SubjectStudent = subject;

        }

        public override string ToString() => $"{base.ToString()}\n\nSubject : {SubjectStudent}";

        public string Create_Assignment()
        {
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

            Console.WriteLine("Specify the date of the assignment (MM-DD-YYYY) :");
            string stringDate = Console.ReadLine();
            char[] split = { '-' };
            while (stringDate[2] != '-' || stringDate[5] != '-' || stringDate.Length != 10)
            {
                Console.WriteLine("Please separate by a '-'");
                stringDate = Console.ReadLine();
            }
            string[] dateSplited = stringDate.Split(split);
            while (Convert.ToInt32(dateSplited[1]) > 31 || Convert.ToInt32(dateSplited[1]) < 0 || Convert.ToInt32(dateSplited[0]) > 12 || Convert.ToInt32(dateSplited[0]) < 0 || Convert.ToInt32(dateSplited[2]) > 2021 || Convert.ToInt32(dateSplited[2]) < 1900)
            {
                Console.WriteLine("Please write your date of birth as the correct form (MM-DD-YYYY)");
                stringDate = Console.ReadLine();
                while (stringDate[2] != '-' || stringDate[5] != '-' || stringDate.Length != 10)
                {
                    Console.WriteLine("Please separate by a '-'");
                    stringDate = Console.ReadLine();
                }
                dateSplited = stringDate.Split(split);
            }

            Console.WriteLine("\nSpecify the assignment :");
            string assignmentContent = Console.ReadLine();

            string assignment = $"{Convert.ToInt32(dateSplited[0])}/{Convert.ToInt32(dateSplited[1])}/{Convert.ToInt32(dateSplited[2])} | {subj} | {assignmentContent}";

            return assignment;
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
                Console.WriteLine("Specify the subject :");
                subj = Console.ReadLine();
                for (int index1 = 0; index1 < SubjectStudent.Count && subjExist != true; index1++)
                {
                    subjExist = SubjectStudent.ElementAt(index1).Key.NameSubject.Contains(subj);
                }
            }

            Console.WriteLine("\nGive a name for the assignment :");
            string nameAssignment = Console.ReadLine();

            string day = "";
            while (day != "Monday" || day != "Tuesday" || day != "Wednesday" || day != "Thursday" || day != "Friday" || day != "Saturday" || day != "Sunday")
            {
                Console.WriteLine("Specify the day of the assignment :");
                day = Console.ReadLine();
            }

            Console.WriteLine("Specify the date of the assignment (MM-DD-YYYY) :");
            string stringDate = Console.ReadLine();
            char[] split = { '-' };
            while (stringDate[2] != '-' || stringDate[5] != '-' || stringDate.Length != 10)
            {
                Console.WriteLine("Please separate by a '-'");
                stringDate = Console.ReadLine();
            }
            string[] dateSplited = stringDate.Split(split);
            while (Convert.ToInt32(dateSplited[1]) > 31 || Convert.ToInt32(dateSplited[1]) < 0 || Convert.ToInt32(dateSplited[0]) > 12 || Convert.ToInt32(dateSplited[0]) < 0 || Convert.ToInt32(dateSplited[2]) > 2021 || Convert.ToInt32(dateSplited[2]) < 1900)
            {
                Console.WriteLine("Please write your date of birth as the correct form (MM-DD-YYYY)");
                stringDate = Console.ReadLine();
                while (stringDate[2] != '-' || stringDate[5] != '-' || stringDate.Length != 10)
                {
                    Console.WriteLine("Please separate by a '-'");
                    stringDate = Console.ReadLine();
                }
                dateSplited = stringDate.Split(split);
            }

            int num = 0;
            string hour = "";
            while (num < 7 && num > 21)
            {
                Console.WriteLine("Specify the start of the assignment. It can start at minimum at 7 and maximum at 21. Every assignment last one hour. You must give exactly one hour.");
                num = Convert.ToInt32(Console.ReadLine());
            }
            hour = num + " - " + Convert.ToString(num + 1);
            Console.WriteLine("Do you want to publish the grade ?");

            Subject newSubj = new Subject(subj, branc, day, hour);

            int index = 0;
            foreach (KeyValuePair<Subject, List<Student>> subject in SubjectStudent)
            {
                while (subject.Key.NameSubject != newSubj.NameSubject)
                {
                    index++;
                }
            }
            
            string[,] gradeOfAnExam = new string[SubjectStudent.ElementAt(index).Value.Count, 3];
            gradeOfAnExam[0, 0] = nameAssignment;
            gradeOfAnExam[1, 0] = newSubj.NameSubject;
            gradeOfAnExam[2, 0] = "Date " + stringDate;
            gradeOfAnExam[3, 0] = "Hours :" + newSubj.NameSubject;
            gradeOfAnExam[0, 1] = "Student";
            gradeOfAnExam[0, 2] = "Grade";

            int ih = 1;
            foreach (Student stud in SubjectStudent.ElementAt(index).Value)
            {
                gradeOfAnExam[ih, 1] = stud.Id;
                int mark = PublishGrade(stud);
                gradeOfAnExam[1, ih] = Convert.ToString(mark);
            }
        }

        /// <summary>
        /// Allow the teacher to publish a new grade
        /// </summary>
        /// <param name="studentGrade"></param>
        public int PublishGrade(Student studentGraded)
        {
            Console.WriteLine("\nGive a grade for " + studentGraded.Name);
            int grade = Convert.ToInt32(Console.ReadLine());
            return grade;
        }

        /// <summary>
        /// Allow the teacher to see a grade of an exam by giving the name of the exam
        /// </summary>
        public void ShowGrade()
        {
            foreach(string[,] subjectGrade in Grade.Values)
            {
                GradePerAssignment.AddLast(subjectGrade);
            }
            TimetableWeek();
        }

        /// <summary>
        /// Show the timetable for a student
        /// </summary>
        public void ToStringGrade(string[,] grade)
        {
            for (int index1 = 0; index1 < grade.GetLength(0); index1++)
            {
                for (int index2 = 0; index2 < grade.GetLength(1); index2++)
                {
                    Console.Write(grade[index1, index2] + "\t\t      ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Show the timetable for a student of the different week
        /// </summary>
        public void TimetableWeek()
        {
            bool choice = true;
            LinkedListNode<string[,]> current = GradePerAssignment.First;
            while (current != null && choice == true)
            {
                Console.Clear();
                ToStringGrade(current.Value);
                Console.WriteLine();
                Console.Write(current.Previous != null ? "< PREVIOUS [P]" : "");
                Console.Write(current.Next != null ? "[N] NEXT >".PadLeft(76) : string.Empty);
                Console.WriteLine();

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.N:
                        if (current.Next != null)
                        {
                            current = current.Next;
                        }
                        break;
                    case ConsoleKey.P:
                        if (current.Previous != null)
                        {
                            current = current.Previous;
                        }
                        break;
                    case ConsoleKey.Enter:
                        choice = false;
                        break;
                    default:
                        return;
                }
            }
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

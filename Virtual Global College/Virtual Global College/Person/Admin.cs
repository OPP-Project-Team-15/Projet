﻿using System;
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
        /// Allow the admin to modify the timetable
        /// </summary>
        public void CreateExamOrCourse(int numb)
        {
            List<Student> studentNewSubject = new List<Student>();
            Console.WriteLine("What do you want to do ? ");
            Console.WriteLine("1 Create a subject");
            Console.WriteLine("2 Create an exam");
            Console.WriteLine("3 Delete a course");
            int cx = Convert.ToInt32(Console.ReadLine());
            while(cx != 1 && cx != 2 && cx != 3)
            {
                Console.WriteLine("Write something either 1 or 2 or 3");
                cx = Convert.ToInt32(Console.ReadLine());
            }

            string branc = "";
            while (branc != "ESILV" && branc != "EMLV" && branc != "IIM")
            {
                Console.WriteLine("Give the name of the branch. The branch must exist.");
                branc = Console.ReadLine();
            }

            string subj = "";
            bool subjExist = false;
            if (cx == 2 || cx == 3)
            {
                while (subjExist != true)
                {
                    Console.WriteLine("Specify the subject :");
                    subj = Console.ReadLine();
                    for (int index1 = 0; index1 < SubjectStudent.Count && subjExist != true; index1++)
                    {
                        subjExist = SubjectStudent.ElementAt(index1).Key.NameSubject.Contains(subj);
                    }
                }
            }
            else
            {
                Console.WriteLine("Specify the subject :");
                subj = Console.ReadLine();
            }

            string week = "Week ";
            if (cx == 2 || cx == 3)
            {
                Console.WriteLine("Specify the week :");
                int number = 0;
                while (number < 1 || number > 30)
                {
                    Console.WriteLine("Please write a number week between 1 and 30 :");
                    number = Convert.ToInt32(Console.ReadLine());
                }
                week = week + number;
            }

            string day = "";
            while (day != "Monday" && day != "Tuesday" && day != "Wednesday" && day != "Thursday" && day != "Friday" && day != "Saturday" && day != "Sunday")
            {
                Console.WriteLine("Specify the day :");
                day = Console.ReadLine();
            }

            int num = 0;
            string hour = "";
            while (num < 7 || num > 20)
            {
                Console.WriteLine("Specify the start. It can start at minimum at 7 and maximum at 20. If it's an assignment it must last one hour. You must give exactly one hour.");
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
                        bool existAlready = false;
                        foreach (Student t in studentNewSubject)
                        {
                            if(existAlready == false && t.Id == stud.Id)
                            {
                                existAlready = true;
                            }
                        }
                        if (existAlready == false)
                        {
                            studentNewSubject.Add(stud);
                        }
                        foreach (string[,] timetable in stud.timetablePerWeek)
                        {
                            if (timetable[0, 8] == week)
                            {
                                string[] tab = new string[2] { hour, day };
                                int[] i = SearchTheIndexOfAnXAndYofMatrix(timetable, tab);
                                if (i[0] != 0 && i[1] != 0)
                                {
                                    if (cx == 2)
                                    {
                                        timetable[i[0], i[1]] = "Exam : " + subj;
                                    }
                                    else if (cx == 3 && list.Key.NameSubject == subj)
                                    {
                                        timetable[i[0], i[1]] = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (cx == 1)
            {
                Subject newSubj = new Subject(subj, branc, day, hour, Convert.ToString(numb++));
                SubjectStudent.Add(newSubj, studentNewSubject);
                SubjectMandatory();
            }
        }

        public void HistoryOfPayment()
        {
            Console.WriteLine(Student.FeesHistory);
        }
    }
}

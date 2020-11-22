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

        public string CreateAssignment()
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

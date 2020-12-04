using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Virtual_Global_College
{
    public class Teacher : User
    {
        public SortedList<Subject, List<Student>> SubjectStudent { get; set; }


        #region : Methods

        public string ToString(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string sql = $"SELECT Id, LastName, FirstName, BirthDate, Sexe, PhoneNumber, Mail FROM Teachers WHERE Id='{Id}'";
            cmd = new MySqlCommand(sql, conn);
            List<string> informations = Program.Pick(conn, cmd, rdr);

            string student_Informations = $"\nStudent Informations :\n\nId : {informations.ElementAt(0)}\nName : {informations.ElementAt(2)} {informations.ElementAt(1)}\nBirth Date : {informations.ElementAt(3)}";
            student_Informations += $"Sexe : {informations.ElementAt(4)}\nPhone Number : {informations.ElementAt(5)}\nMail : {informations.ElementAt(6)}";

            return student_Informations;
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
        /// Allow the teacher to create a new assignment for it's students
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        public void Add_Assignment(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string sql; string answer; string stringDate;
            string assignmentSubject; string assignmentName; DateTime assignmentDate;
            List<string> Subjects = new List<string>();
            List<string> IdStudents = new List<string>();
            List<string> idAssignmentSubject = new List<string>();

            // ASK FOR THE SUBJECT OF THE ASSIGNMENT

            Console.WriteLine("Please select one of your subjects :");
            sql = $"SELECT NameSubject FROM Subjects INNER JOIN SubjectsTeachers ON Subjects.IdSubject = SubjectsTeachers.IdSubject AND SubjectsTeachers.IdTeacher='{Id}'";
            cmd = new MySqlCommand(sql, conn);
            Subjects = Program.Pick(conn, cmd, rdr);

            foreach (string subject in Subjects)
                Console.WriteLine($"{Subjects.IndexOf(subject) + 1} - {subject}");
            answer = Console.ReadLine();
            while (!Subjects.Contains(answer))
            {
                Console.WriteLine("\nPlease write as it's written");
                answer = Console.ReadLine();
            }
            assignmentSubject = answer;

            // WE OBTAIN THE ID OF THIS SUBJECT

            sql = $"SELECT IdSubject FROM Subjects WHERE NameSubject='{assignmentSubject}'";
            cmd = new MySqlCommand(sql, conn);
            idAssignmentSubject = Program.Pick(conn, cmd, rdr);

            // ASK FOR THE DATE OF THE ASSIGNMENT

            Console.WriteLine("\nSpecify the date of the assignment (MM-DD-YYYY) :");
            stringDate = Console.ReadLine();
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
            assignmentDate = new DateTime(Convert.ToInt32(dateSplited[2]), Convert.ToInt32(dateSplited[0]), Convert.ToInt32(dateSplited[1]));

            // ASK FOR THE NAME OF THE ASSIGNMENT

            Console.WriteLine("\nSpecify the name of the assignment :");
            assignmentName = Console.ReadLine();

            // OBTAIN THE LIST OF THE STUDENTS WHO HAVE THIS TEACHER IN THIS SUBJECT

            sql = $"SELECT IdStudent FROM Students INNER JOIN TeachersStudents ON Students.Id = TeachersStudents.IdStudent AND TeachersStudents.IdTeacher='{Id}' AND TeachersStudents.IdSubject='{idAssignmentSubject.ElementAt(0)}'";
            cmd = new MySqlCommand(sql, conn);
            IdStudents = Program.Pick(conn, cmd, rdr);

            foreach (string idStudent in IdStudents)
            {
                //sql = $"INSERT INTO Assignments (Date, Subject, Name, IdStudent) VALUES ('{assignmentDate}', '{assignmentSubject}', '{assignmentName}', '{idStudent}')";
                //cmd = new MySqlCommand(sql, conn);
                sql = "INSERT INTO Assignments SET Date = @date, Subject = @subject, Name = @name, IdStudent = @idstudent";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@date", assignmentDate);
                cmd.Parameters.AddWithValue("@subject", assignmentSubject);
                cmd.Parameters.AddWithValue("@type", assignmentName);
                cmd.Parameters.AddWithValue("@idstudent", idStudent);
                Program.Insert(conn, cmd, rdr);
            }


        }

        /// <summary>
        /// Alow the teacher to put new grades for  a classroom
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        public void Add_Grades(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string sql; string subjectGrade; string answer; string nameGrade; string markGrade;
            int compteur;
            string[,] Grades;
            List<string> idSubjectGrade = new List<string>();
            List<string> Subjects = new List<string>();

            List<string> Students = new List<string>();
            List<string> AllStudents = new List<string>();
            List<string> idStudents = new List<string>();


            // ASK FOR THE SUBJECT OF THE GRADE

            sql = $"SELECT NameSubject FROM Subjects INNER JOIN SubjectsTeachers ON Subjects.IdSubject = SubjectsTeachers.IdSubject WHERE IdTeacher='{Id}'";
            cmd = new MySqlCommand(sql, conn);
            Subjects = Program.Pick(conn, cmd, rdr);

            if (Subjects.Count == 1)
            {
                Console.WriteLine($"\nYou have only one subject : {Subjects.ElementAt(0)}");
                subjectGrade = Subjects.ElementAt(0);
            }
            else
            {
                Console.WriteLine($"\nYou have some subjects. Please select the subject in which you want to add grade :");
                foreach (string subject in Subjects)
                {
                    Console.WriteLine($"{Subjects.IndexOf(subject) + 1} - {subject}");
                }
                Console.WriteLine();
                answer = Console.ReadLine();
                while (!Subjects.Contains(answer))
                {
                    Console.WriteLine("Please write the subject as it's written");
                    answer = Console.ReadLine();
                }
                subjectGrade = answer;
            }

            // ASK FOR THE NAME OF THE GRADE

            Console.WriteLine("\nPlease write the name of the grade :");
            nameGrade = Console.ReadLine();

            // SEARCHING FOR THE ID OF THE SUBJECT, ID OF THE STUDENTS AND NAME OF STUDENTS

            sql = $"SELECT IdSubject FROM Subjects WHERE NameSubject='{subjectGrade}'";
            cmd = new MySqlCommand(sql, conn);
            idSubjectGrade = Program.Pick(conn, cmd, rdr);

            sql = $"SELECT Id FROM Students INNER JOIN TeachersStudents ON Students.Id = TeachersStudents.IdStudent AND TeachersStudents.IdTeacher ='{Id}' AND TeachersStudents.IdSubject='{idSubjectGrade.ElementAt(0)}'";
            cmd = new MySqlCommand(sql, conn);
            idStudents = Program.Pick(conn, cmd, rdr);

            foreach (string idStudent in idStudents)
            {
                sql = $"SELECT FirstName FROM Students WHERE Id='{Convert.ToInt32(idStudent)}'";
                cmd = new MySqlCommand(sql, conn);
                Students = Program.Pick(conn, cmd, rdr);
                AllStudents.Add(Students.ElementAt(0));
            }

            // FILL OF THE MATRIX

            Grades = new string[idStudents.Count + 1, 3];
            Grades[0, 0] = $"{FirstName} {LastName}";
            Grades[1, 0] = $"{subjectGrade}";
            Grades[2, 0] = $"{nameGrade}";
            Grades[0, 1] = "Student";
            Grades[0, 2] = "Mark";

            // ASK FOR THE MARK, AND INSERT INTO SQL DATABASE

            int index = 0;
            foreach (string idStudent in idStudents)
            {
                Console.WriteLine($"\nPlease write a mark out of 20 for {AllStudents.ElementAt(index)} ({idStudents.ElementAt(index)}) :");
                markGrade = Console.ReadLine();

                // ANALYSE IF THE MARK IS A CORRECT MARK

                if (!markGrade.All(char.IsDigit) || ((markGrade.Length == 1 || markGrade.Length == 2) && !markGrade.All(char.IsDigit)) || markGrade.Length > 2 && (Convert.ToInt32(markGrade.Substring(0, 2)) > 19 || Convert.ToInt32(markGrade.Substring(0, 2)) < 0) || (markGrade.Length == 2 && (Convert.ToInt32(markGrade) < 0 || Convert.ToInt32(markGrade) > 20)) || !markGrade[markGrade.Length - 1].ToString().All(char.IsDigit) || !markGrade.All(char.IsDigit) && !markGrade.Contains(','))
                {
                    compteur = 0;
                    foreach (char number in markGrade)
                    {
                        if (number.ToString().All(char.IsDigit) == true)
                            compteur++;
                    }
                    while (compteur != markGrade.Length - 1 || ((markGrade.Length == 1 || markGrade.Length == 2) && !markGrade.All(char.IsDigit) || markGrade.Length > 2 && (Convert.ToInt32(markGrade.Substring(0, 2)) > 19 || Convert.ToInt32(markGrade.Substring(0, 2)) < 0) || (markGrade.Length == 2 && (Convert.ToInt32(markGrade) < 0 || Convert.ToInt32(markGrade) > 20)) || !markGrade[markGrade.Length - 1].ToString().All(char.IsDigit)) || !markGrade.All(char.IsDigit) && !markGrade.Contains(','))
                    {
                        Console.WriteLine("Please write a correct mark out of 20 :");
                        markGrade = Console.ReadLine();

                        if (!markGrade.All(char.IsDigit) || ((markGrade.Length == 1 || markGrade.Length == 2) && !markGrade.All(char.IsDigit)) || markGrade.Length > 2 && (Convert.ToInt32(markGrade.Substring(0, 2)) > 19 || Convert.ToInt32(markGrade.Substring(0, 2)) < 0) || (markGrade.Length == 2 && (Convert.ToInt32(markGrade) < 0 || Convert.ToInt32(markGrade) > 20)) || !markGrade[markGrade.Length - 1].ToString().All(char.IsDigit) || !markGrade.All(char.IsDigit) && !markGrade.Contains(','))
                        {
                            compteur = 0;
                            foreach (char number in markGrade)
                            {
                                if (number.ToString().All(char.IsDigit) == true)
                                    compteur++;
                            }
                        }
                        else
                            break;
                    }

                }

                // INSERT THE MARK IN THE SQL

                //sql = $"INSERT INTO Grades (NameSubject, GradeName, Mark, IdStudent, IdTeacher) VALUES ('{subjectGrade}', '{nameGrade}', '{markGrade}', '{idStudent}', '{Id}')";
                //cmd = new MySqlCommand(sql, conn);
                //Program.Insert(conn, cmd, rdr);
                sql = "INSERT INTO Grades SET NameSubject = @namesubject, GradeName = @gradename, Mark = @mark, IdStudent = @idstudent, IdTeacher = @idteacher";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@namesubject", subjectGrade);
                cmd.Parameters.AddWithValue("@gradename", nameGrade);
                cmd.Parameters.AddWithValue("@mark", markGrade);
                cmd.Parameters.AddWithValue("@idstudent", idStudent);
                cmd.Parameters.AddWithValue("@idteacher", Id);
                Program.Insert(conn, cmd, rdr);

                Grades[index + 1, 1] = $"{idStudent}";
                Grades[index + 1, 2] = $"{markGrade}";
                index++;
            }
        }


        /// <summary>
        /// Allow the teacher to see all the coordinates of a specific student
        /// </summary>
        public void Print_Student_Informations(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string idOfStudent; string sql; string result;
            List<string> StudentInformations;
            bool idExist;


            Console.WriteLine("Please enter the ID of the student you want to see his/her informations :");
            idOfStudent = Console.ReadLine();

            sql = "SELECT Id FROM Students";
            cmd = new MySqlCommand(sql, conn);
            idExist = Program.Exist(idOfStudent, conn, cmd, rdr);

            while (idExist == false)
            {
                Console.WriteLine("This ID doesn't exist. Please enter an existing ID :");
                idOfStudent = Console.ReadLine();

                sql = "SELECT Id FROM Students";
                cmd = new MySqlCommand(sql, conn);
                idExist = Program.Exist(idOfStudent, conn, cmd, rdr);
            }

            sql = $"SELECT Id, LastName, FirstName, BirthDate, Sexe, PhoneNumber, Mail, Branch FROM Students WHERE Id='{idOfStudent}'";
            cmd = new MySqlCommand(sql, conn);
            StudentInformations = Program.Pick(conn, cmd, rdr);

            Console.WriteLine("\nStudent Informations :\n\nId : " + StudentInformations[0] + "\nLast Name : " + StudentInformations[1] + "\nFirst Name : " + StudentInformations[2] + "\nBirth Date : " + StudentInformations[3] + "\nSexe : " + StudentInformations[4]);
            Console.WriteLine("Phone Number : " + StudentInformations[5] + "\nMail : " + StudentInformations[6] + "\nBranch : " + StudentInformations[7] + "\n\n");

            Console.WriteLine("Do you want to see infomations from another student : (yes/no)");
            result = Console.ReadLine();

            while (result != "yes" && result != "no")
            {
                Console.WriteLine("\nPlease write as it's written (yes or no) ");
                result = Console.ReadLine();
            }

            if (result == "yes")
                Print_Student_Informations(conn, cmd, rdr);

            else
                Console.WriteLine("Goodbye ;-(");

        }

        /// <summary>
        /// Allow the teacher to see all the attendances of a specific student
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdRead"></param>
        /// <param name="rdr"></param>
        public void Print_Student_Attendances(MySqlConnection conn, MySqlCommand cmdRead, MySqlDataReader rdr)
        {
            bool endMethod = false; bool idExist;
            string answer; string idOfStudent; string sqlRead;
            int anyAttendances;

            while (endMethod == false)
            {
                Console.WriteLine("Please enter the ID of the student who you want to see his attendances :");
                idOfStudent = Console.ReadLine();
                sqlRead = "SELECT Id FROM Students";
                cmdRead = new MySqlCommand(sqlRead, conn);
                idExist = Program.Exist(idOfStudent, conn, cmdRead, rdr);
                while (idExist == false)
                {
                    Console.WriteLine("This ID doesn't exist. Please enter an existing ID :");
                    idOfStudent = Console.ReadLine();
                    idExist = Program.Exist(idOfStudent, conn, cmdRead, rdr);
                }

                Console.WriteLine();
                sqlRead = $"SELECT Date, Subject, Type FROM Attendances WHERE IdStudent={idOfStudent}";
                cmdRead = new MySqlCommand(sqlRead, conn);
                anyAttendances = Program.Read(conn, cmdRead, rdr, 1);
                if (anyAttendances == 0)
                    Console.WriteLine("The student hasn't any lates or absences\n");

                Console.WriteLine("Do you want to see the attendances of another student ? Please write as it's written :\n- yes\n- no\n");
                answer = Console.ReadLine();
                while (answer != "yes" && answer != "no")
                {
                    Console.WriteLine("This answer doesn't exist. Please write as it's written :");
                    answer = Console.ReadLine();
                }

                if (answer == "no")
                {
                    endMethod = true;
                    Console.WriteLine("\nThank you. Have a good day.");
                }

                else
                    Console.WriteLine();
            }
        }

        /// <summary>
        /// Allow the teacher to see his/her grades notebook
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        public void Print_Grades_Notebook(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            LinkedList<string[,]> Grades_NoteBook = new LinkedList<string[,]>();
            List<string> Subjects = new List<string>();
            List<string> AllGradesName = new List<string>();
            List<string> gradesName = new List<string>();
            List<string> Names_Marks_Sql = new List<string>();
            string[,] Names_Marks;
            string sql; int index;
            int gradeExist;

            // WE OBTAIN THE TEACHER SUBJECTS

            sql = $"SELECT NameSubject FROM Subjects INNER JOIN SubjectsTeachers ON Subjects.IdSubject = SubjectsTeachers.IdSubject AND SubjectsTeachers.IdTeacher='{Id}'";
            cmd = new MySqlCommand(sql, conn);
            Subjects = Program.Pick(conn, cmd, rdr);

            // WE REPEAT THE OPERATION FOR EACH SUBJECT

            foreach (string subject in Subjects)
            {
                // WE EXAMINE IF THERE ARE ANY GRADES OF THE TEACHER IN THE SUBJECT
                sql = $"SELECT IdGrade FROM Grades WHERE NameSubject='{subject}' AND IdTeacher='{Id}'";
                cmd = new MySqlCommand(sql, conn);
                gradeExist = Program.Read(conn, cmd, rdr, 4);

                if (gradeExist != 0)
                {
                    // WE OBTAIN ALL THE NAME GRADES OF EACH GRADES

                    index = 0;
                    sql = $"SELECT GradeName FROM Grades WHERE NameSubject='{subject}' AND IdTeacher='{Id}'";
                    cmd = new MySqlCommand(sql, conn);
                    AllGradesName = Program.Pick(conn, cmd, rdr);

                    // WE OBTAIN ALL THE NAME GRADES WITHOUT DOUBLES

                    foreach (string name in AllGradesName)
                    {
                        if (!gradesName.Contains(name))
                            gradesName.Add(name);
                    }

                    // WE REPEAT THE OPERATION FOR EACH GRADES NAME

                    foreach (string name in gradesName)
                    {
                        index = 0;

                        sql = $"SELECT IdStudent, Mark FROM Grades WHERE NameSubject='{subject}' AND IdTeacher='{Id}' AND GradeName='{name}'";
                        cmd = new MySqlCommand(sql, conn);
                        Names_Marks_Sql = Program.Pick(conn, cmd, rdr);

                        Names_Marks = new string[Names_Marks_Sql.Count / 2 + 1, 3];
                        Names_Marks[0, 0] = $"{FirstName} {LastName}";
                        Names_Marks[1, 0] = $"{subject}";
                        Names_Marks[2, 0] = $"{name}";
                        Names_Marks[0, 1] = "Student :";
                        Names_Marks[0, 2] = "Mark :";

                        foreach (string data in Names_Marks_Sql)
                        {
                            if (index % 2 == 0)
                                Names_Marks[index / 2 + 1, 1] = Names_Marks_Sql.ElementAt(index);
                            else if (index / 2 != 0)
                                Names_Marks[index / 2, 2] = Names_Marks_Sql.ElementAt(index);
                            index++;
                        }

                        Grades_NoteBook.AddLast(Names_Marks);
                    }
                }
            }

            GradeExam(Grades_NoteBook);
        }


        public void Modify_Student_Grade(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {

        }


        /// <summary>
        /// Show the timetable for a student
        /// </summary>
        public static void ToStringGrade(string[,] grade)
        {
            for (int index1 = 0; index1 < grade.GetLength(0); index1++)
            {
                for (int index2 = 0; index2 < grade.GetLength(1); index2++)
                {
                    if (grade[index1, index2] == null)
                    {
                        for (int i = 0; i < grade[0, 0].Length; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.Write(grade[index1, index2] + "\t\t\t\t      ");
                    }
                    else
                    {
                        if (grade[index1, index2].Length < grade[0, index2].Length)
                        {
                            Console.Write(grade[index1, index2] + "\t\t\t\t      ");
                            for (int i = 0; i < grade[0, index2].Length - grade[index1, index2].Length; i++)
                            {
                                Console.Write(" ");
                            }
                        }
                        else if (grade[index1, index2].Length > grade[0, index2].Length)
                        {
                            int number = grade[index1, index2].Length - grade[0, index2].Length;
                            Console.Write(grade[index1, index2]);
                            for (int i = 0; i < 36 - number; i++)
                            {
                                Console.Write(" ");
                            }
                        }
                        else
                        {
                            Console.Write(grade[index1, index2] + "\t\t\t\t      ");
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Show the grade for an exam
        /// </summary>
        public static void GradeExam(LinkedList<string[,]> Grades_NoteBook)
        {
            bool choice = true;
            LinkedListNode<string[,]> current = Grades_NoteBook.First;
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
            Grades_NoteBook.Clear();
        }
        #endregion
    }
}

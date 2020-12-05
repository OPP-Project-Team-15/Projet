using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;


namespace Virtual_Global_College
{
    interface ISchool
    {
        string SchoolName { get; }
        string SchoolInfo();
    }



    public class Student : User
    {
        private string branch;
        private bool paymentIsOk = false;
        public LinkedList<string[,]> timetablePerWeek = new LinkedList<string[,]>();


        #region : Properties

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
        public string[,] Timetable { get; set; }
        public decimal Money { get; set; } //Money that student have in his school account
        public string ProcessPayment { get; set; } //Payment in once or in thrice
        public int NumberOfPayment { get; set; } //Number of times student paid for a payment in several times
        public bool PaymentIsOk
        {
            get { return paymentIsOk; }
            set { paymentIsOk = value; }
        } //True if student has paid all the fees for the year
        public bool SubjectsPicked { get; set; }
        #endregion



        #region : Methods

        public string ToString(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string sql = $"SELECT Id, LastName, FirstName, BirthDate, Sexe, PhoneNumber, Mail, Branch FROM Students WHERE Id='{Id}'";
            cmd = new MySqlCommand(sql, conn);
            List<string> informations = Program.Pick(conn, cmd, rdr);

            string student_Informations = $"######## STUDENT INFORMATIONS ########\n\nId : {informations.ElementAt(0)}\nName : {informations.ElementAt(2)} {informations.ElementAt(1)}\nBirth Date : {informations.ElementAt(3)}\n";
            student_Informations += $"Sexe : {informations.ElementAt(4)}\nPhone Number : {informations.ElementAt(5)}\nMail : {informations.ElementAt(6)}\nBranch : {informations.ElementAt(7)}";

            return student_Informations;
        }


        /// <summary>
        /// Allow the student to choose his/her courses for the year
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        /// <param name="random"></param>
        public void Course_Registration(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr, Random random)
        {
            Console.WriteLine("######## COURSE REGISTRATION ########\n");

            if (SubjectsPicked)
                Console.WriteLine("You have already chosen your courses");
            else
            {
                // VARIABLES CREATIONS
                string sql;
                int indexRandom;
                List<string> TeachersId = new List<string>(); //Contains the ID of all the teachers who teach a subject (required firstly, then not required) of the student branch
                List<string> TeachersSubjects = new List<string>(); //Contains the subject of all the teachers
                List<string> Subjects = new List<string>(); //Contains the required subjects firstly, and then the non required subjects
                List<string> SubjectId = new List<string>();


                // ------------------- WE BEGIN WITH THE REQUIREDS SUBJECTS --------------------

                // We obtain the list of the Id teachers who teach a required subject of the student branch
                sql = $"SELECT IdTeacher FROM SubjectsTeachers INNER JOIN Subjects ON SubjectsTeachers.IdSubject = Subjects.IdSubject WHERE Branch='{Branch}' AND Required='1'";
                cmd = new MySqlCommand(sql, conn);
                TeachersId = Program.Pick(conn, cmd, rdr);

                // We obtain the list of the exact subject of each teachers of the last list
                sql = $"SELECT NameSubject FROM Subjects INNER JOIN SubjectsTeachers ON Subjects.IdSubject = SubjectsTeachers.IdSubject AND Required='1' WHERE Branch='{Branch}'";
                cmd = new MySqlCommand(sql, conn);
                TeachersSubjects = Program.Pick(conn, cmd, rdr);

                // We obtain the list of the requireds subjects
                sql = $"SELECT NameSubject FROM Subjects WHERE Required='1' AND Branch='{Branch}'";
                cmd = new MySqlCommand(sql, conn);
                Subjects = Program.Pick(conn, cmd, rdr);

                // INSERT A SQL INSTANCE FOR EACH LINK BETWEEN A STUDENT AND A TEACHER (student who has class with a teacher with a required subject)
                for (int i = 0; i < Subjects.Count; i++)
                {
                    // We obtain the Id of each required subject
                    sql = $"SELECT IdSubject FROM Subjects WHERE NameSubject='{Subjects.ElementAt(i)}'";
                    cmd = new MySqlCommand(sql, conn);
                    SubjectId = Program.Pick(conn, cmd, rdr);

                    // We obtain the index of the Id of a random teacher who teachs this subject
                    indexRandom = random.Next(TeachersId.Count);
                    while (TeachersSubjects.ElementAt(indexRandom) != Subjects.ElementAt(i))
                        indexRandom = random.Next(TeachersId.Count);

                    // We insert the instance
                    //sql = $"INSERT INTO TeachersStudents (IdStudent, IdTeacher, IdSubject) VALUES ('{Id}', '{Convert.ToInt32(TeachersId.ElementAt(indexRandom))}', '{SubjectId.ElementAt(0)}')";
                    sql = "INSERT INTO TeachersStudents SET IdStudent = @idstudent, IdTeacher = @idteacher, IdSubject = @idsubject";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@idstudent", Id);
                    cmd.Parameters.AddWithValue("@idteacher", Convert.ToInt32(TeachersId.ElementAt(indexRandom)));
                    cmd.Parameters.AddWithValue("@idsubject", SubjectId.ElementAt(0));
                    Program.Insert(conn, cmd, rdr);
                }

                Console.WriteLine($"The requireds subjects in {Branch} are : ");
                foreach (string subject in Subjects)
                {
                    Console.WriteLine($"{Subjects.IndexOf(subject) + 1} - {subject}");
                }
                Console.WriteLine($"For these requireds subjects, you have been affected randomly in a teacher classroom.\n\nThe non requireds subjects in {Branch} are :");


                // ------------------- THEN, WE WORK WITH THE NON REQUIREDS SUBJECTS -------------------

                // We print the non requireds subjects
                sql = $"SELECT NameSubject FROM Subjects WHERE Branch='{Branch}' AND Required = '0'";
                cmd = new MySqlCommand(sql, conn);
                Program.Read(conn, cmd, rdr, 3);

                // We obtain the pick of the student
                Console.WriteLine("\nPlease select 2 non requireds subjects by writing the name of the subjects as it's written :");
                string answer;
                bool exist;
                List<string> NotRequiredSubjectsPicked = new List<string>();
                for (int i = 0; i < 2; i++)
                {
                    if (i != 0)
                        Console.WriteLine("\nThank you. Please select the second non required subject :");

                    answer = Console.ReadLine();
                    sql = $"SELECT NameSubject FROM Subjects WHERE NameSubject='{answer}'";
                    cmd = new MySqlCommand(sql, conn);
                    exist = Program.Exist(answer, conn, cmd, rdr);
                    while (!exist)
                    {
                        Console.WriteLine("\nThis subject doesn't exist. Please write as it's written :");
                        answer = Console.ReadLine();
                        sql = $"SELECT NameSubject FROM Subjects WHERE NameSubject='{answer}'";
                        cmd = new MySqlCommand(sql, conn);
                        exist = Program.Exist(answer, conn, cmd, rdr);
                    }
                    NotRequiredSubjectsPicked.Add(answer);
                }

                // We obtain the list of the Id teachers who teach a non required subject of the student branch
                sql = $"SELECT IdTeacher FROM SubjectsTeachers INNER JOIN Subjects ON SubjectsTeachers.IdSubject = Subjects.IdSubject WHERE Branch='{Branch}' AND Required='0'";
                cmd = new MySqlCommand(sql, conn);
                TeachersId = Program.Pick(conn, cmd, rdr);

                // We obtain the list of the exact subject of each teachers of the last list
                sql = $"SELECT NameSubject FROM Subjects INNER JOIN SubjectsTeachers ON Subjects.IdSubject = SubjectsTeachers.IdSubject WHERE Branch='{Branch}' AND Required='0'";
                cmd = new MySqlCommand(sql, conn);
                TeachersSubjects = Program.Pick(conn, cmd, rdr);

                // INSERT A SQL INSTANCE FOR EACH LINK BETWEEN A STUDENT AND A TEACHER (student who has class with a teacher with a non required subject)
                for (int i = 0; i < NotRequiredSubjectsPicked.Count; i++)
                {
                    // We obtain the Id of each non required subject
                    sql = $"SELECT IdSubject FROM Subjects WHERE NameSubject='{NotRequiredSubjectsPicked.ElementAt(i)}'";
                    cmd = new MySqlCommand(sql, conn);
                    SubjectId = Program.Pick(conn, cmd, rdr);

                    // We obtain the index of the Id of a random teacher who teachs this subject
                    indexRandom = random.Next(TeachersId.Count);
                    while (TeachersSubjects.ElementAt(indexRandom) != NotRequiredSubjectsPicked.ElementAt(i))
                        indexRandom = random.Next(TeachersId.Count);

                    // We insert the instance
                    //sqlInsert = $"INSERT INTO TeachersStudents (IdStudent, IdTeacher, IdSubject) VALUES ('{Id}', '{Convert.ToInt32(TeachersId.ElementAt(indexRandom))}', '{SubjectId.ElementAt(0)}')";
                    sql = "INSERT INTO TeachersStudents SET IdStudent = @idstudent, IdTeacher = @idteacher, IdSubject = @idsubject";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@idstudent", Id);
                    cmd.Parameters.AddWithValue("@idteacher", Convert.ToInt32(TeachersId.ElementAt(indexRandom)));
                    cmd.Parameters.AddWithValue("@idsubject", SubjectId.ElementAt(0));
                    Program.Insert(conn, cmd, rdr);
                }

                Console.WriteLine("\nYour courses have been saved. You have been affected randomly in a teacher classroom for these non required subjects too.\nHave a good day :)");
                SubjectsPicked = true;
            }
        }

        /// <summary>
        /// Allow the student to add money in his/her school account
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdInsert"></param>
        /// <param name="rdr"></param>
        public void Add_Money(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            Console.WriteLine("######## ADD MONEY ########\n");
            Console.WriteLine("How much money do you want to add?");
            decimal moneyAdded = Convert.ToDecimal(Console.ReadLine());
            this.Money += moneyAdded;
            DateTime thisDay = DateTime.Today;
            string FeeName = $"Added {moneyAdded} euros to your school account";
            //string sqlInsert = $"INSERT INTO Fees (Date, Amount, Name, IdStudent) VALUES ('{thisDay}','{moneyAdded}', '{FeeName}', '{Id}')";
            string sql = "INSERT INTO Fees SET Date = @date, Amount = @amount, Name = @name, IdStudent = @idstudent";
            cmd = new MySqlCommand(sql, conn);
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@date", thisDay);
            cmd.Parameters.AddWithValue("@amount", moneyAdded);
            cmd.Parameters.AddWithValue("@name", FeeName);
            cmd.Parameters.AddWithValue("@idstudent", Id);
            Program.Insert(conn, cmd, rdr);
        }

        /// <summary>
        /// Allow the student to choose a process payment for the first use of the method
        /// And allow the student to pay with the method he has chosen
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdInsert"></param>
        /// <param name="rdr"></param>
        public void Payment(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            Console.WriteLine("######## ADD MONEY ########\n");


            string FeeName;
            string sql;
            DateTime thisDay = DateTime.Today;

            if (PaymentIsOk == true)
                Console.WriteLine("The payment for the year is done");
            else
            {
                if (ProcessPayment == null)
                {
                    Console.WriteLine("Which payment process would you take ? Please write your response as it's written\n- once\n- thrice");
                    string process = Console.ReadLine();
                    while (process != "once" && process != "thrice")
                    {
                        Console.WriteLine("This process doesn't exist, please write as it's written");
                        process = Console.ReadLine();
                    }


                    if (process == "once")
                    {
                        ProcessPayment = "once";
                        FeeName = "Selected payment by once";
                        //sqlInsert = $"INSERT INTO Fees (Date, Amount, Name, IdStudent) VALUES ('{thisDay}','0', '{FeeName}', '{Id}')";
                        sql = "INSERT INTO Fees SET Date = @date, Amount = @amount, Name = @name, IdStudent = @idstudent";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@date", thisDay);
                        cmd.Parameters.AddWithValue("@amount", 0);
                        cmd.Parameters.AddWithValue("@name", FeeName);
                        cmd.Parameters.AddWithValue("@idstudent", Id);
                        Program.Insert(conn, cmd, rdr);
                    }
                    else
                    {
                        ProcessPayment = "thrice";
                        FeeName = "Selected payment by thrice";
                        //sqlInsert = $"INSERT INTO Fees (Date, Amount, Name, IdStudent) VALUES ('{thisDay}','0', '{FeeName}', '{Id}')";
                        sql = "INSERT INTO Fees SET Date = @date, Amount = @amount, Name = @name, IdStudent = @idstudent";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@date", thisDay);
                        cmd.Parameters.AddWithValue("@amount", 0);
                        cmd.Parameters.AddWithValue("@name", FeeName);
                        cmd.Parameters.AddWithValue("@idstudent", Id);
                        Program.Insert(conn, cmd, rdr);
                    }
                }

                Console.WriteLine($"\nYour process payment is : {ProcessPayment}\nDo you want to pay ? Please write your response as it's written\n- yes\n- no");
                string payment = Console.ReadLine();
                while (payment != "yes" && payment != "no")
                {
                    Console.WriteLine("\nPlease write as it's written");
                    payment = Console.ReadLine();
                }

                if (payment == "yes")
                {
                    bool possiblePayment = true;
                    if (ProcessPayment == "once")
                    {
                        if (this.Money <= 9000)
                            possiblePayment = false;
                        else
                        {
                            this.Money -= 9000;
                            FeeName = "Spend 9000 euros for payment in once";
                            //sqlInsert = $"INSERT INTO Fees (Date, Amount, Name, IdStudent) VALUES ('{thisDay}','9000', '{FeeName}', '{Id}')";
                            sql = "INSERT INTO Fees SET Date = @date, Amount = @amount, Name = @name, IdStudent = @idstudent";
                            cmd = new MySqlCommand(sql, conn);
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@date", thisDay);
                            cmd.Parameters.AddWithValue("@amount", 9000);
                            cmd.Parameters.AddWithValue("@name", FeeName);
                            cmd.Parameters.AddWithValue("@idstudent", Id);
                            Program.Insert(conn, cmd, rdr);
                            NumberOfPayment = 1;
                            PaymentIsOk = true;
                        }
                    }

                    else
                    {
                        if (this.Money <= 3000)
                            possiblePayment = false;
                        else
                        {
                            this.Money -= 3000;
                            FeeName = "Spend 3000 euros for payment in thrice";
                            //sqlInsert = $"INSERT INTO Fees (Date, Amount, Name, IdStudent) VALUES ('{thisDay}','3000', '{FeeName}', '{Id}')";
                            sql = "INSERT INTO Fees SET Date = @date, Amount = @amount, Name = @name, IdStudent = @idstudent";
                            cmd = new MySqlCommand(sql, conn);
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@date", thisDay);
                            cmd.Parameters.AddWithValue("@amount", 3000);
                            cmd.Parameters.AddWithValue("@name", FeeName);
                            cmd.Parameters.AddWithValue("@idstudent", Id);
                            Program.Insert(conn, cmd, rdr);
                            NumberOfPayment++;
                            if (NumberOfPayment == 3)
                                PaymentIsOk = true;
                        }
                    }

                    if (possiblePayment == true)
                        Console.WriteLine($"\nThe payment has been done\nYou have {this.Money} euros left");
                    else
                        Console.WriteLine($"\nThe payment failed because you don't have enough money (You have {this.Money} euros)");
                }

                else
                    Console.WriteLine("Thank you for your process choice");

            }
        }


        /// <summary>
        /// Allow the student to see his/her fees history
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdRead"></param>
        /// <param name="rdr"></param>
        public void Print_Fees_History(MySqlConnection conn, MySqlCommand cmdRead, MySqlDataReader rdr)
        {
            Console.WriteLine("######## FEES HISTORY ########");

            string sqlRead = $"SELECT Date, Name FROM Fees WHERE IdStudent={Id}";
            cmdRead = new MySqlCommand(sqlRead, conn);
            int anyFees = Program.Read(conn, cmdRead, rdr, 1);
            if (anyFees == 0)
                Console.WriteLine("You haven't any fees in your history.");
        }

        /// <summary>
        /// Allow the student to see his/her abscenses and lates
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdRead"></param>
        /// <param name="rdr"></param>
        public void Print_Attendances(MySqlConnection conn, MySqlCommand cmdRead, MySqlDataReader rdr)
        {
            Console.WriteLine("########## ATTENDANCES ##########\n");

            string sqlRead = $"SELECT Date, Subject, Type FROM Attendances WHERE IdStudent={Id}";
            cmdRead = new MySqlCommand(sqlRead, conn);
            int anyAbsencesOrLates = Program.Read(conn, cmdRead, rdr, 1);
            if (anyAbsencesOrLates == 0)
                Console.WriteLine("You haven't any absences or lates.");
        }

        /// <summary>
        /// Allow the student to see his/her abscenses and lates
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdRead"></param>
        /// <param name="rdr"></param>
        public void Print_Assignments(MySqlConnection conn, MySqlCommand cmdRead, MySqlDataReader rdr)
        {
            Console.WriteLine("########## ASSIGNMENTS ##########");

            string sqlRead = $"SELECT Date, Subject, Name FROM Assignments WHERE IdStudent={Id}";
            cmdRead = new MySqlCommand(sqlRead, conn);
            int anyAssignments = Program.Read(conn, cmdRead, rdr, 1);
            if (anyAssignments == 0)
                Console.WriteLine("You haven't any assignments yet.");
        }

        /// <summary>
        /// Allow the student to see his/her grades
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        public void Print_Grades(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string sql;
            int gradeExist; int index;
            string[,] Names_Marks;
            List<string> Subjects; List<string> Names_Marks_Sql;
            LinkedList<string[,]> Grades_NoteBook = new LinkedList<string[,]>();


            // WE OBTAIN THE TEACHER SUBJECTS

            sql = $"SELECT NameSubject FROM Subjects INNER JOIN TeachersStudents ON TeachersStudents.IdSubject = Subjects.IdSubject AND TeachersStudents.Idstudent='{Id}'";
            cmd = new MySqlCommand(sql, conn);
            Subjects = Program.Pick(conn, cmd, rdr);

            // WE REPEAT THE OPERATION FOR EACH SUBJECT

            foreach (string subject in Subjects)
            {
                // WE EXAMINE IF THERE ARE ANY GRADES OF THE STUDENTS IN THE SUBJECT

                sql = $"SELECT IdGrades FROM Grades WHERE NameSubject='{subject}' AND IdStudent='{Id}'";
                cmd = new MySqlCommand(sql, conn);
                gradeExist = Program.Read(conn, cmd, rdr, 4);

                if (gradeExist != 0)
                {
                    // WE OBTAIN ALL THE NAME GRADES OF EACH GRADES

                    index = 0;
                    sql = $"SELECT GradeName, Mark FROM Grades WHERE NameSubject='{subject}' AND IdStudent='{Id}'";
                    cmd = new MySqlCommand(sql, conn);
                    Names_Marks_Sql = Program.Pick(conn, cmd, rdr);

                    Names_Marks = new string[Names_Marks_Sql.Count + 3, 3];
                    Names_Marks[0, 0] = $"{FirstName} {LastName}";
                    Names_Marks[1, 0] = "Grades";
                    Names_Marks[2, 0] = $"{subject}";
                    Names_Marks[0, 1] = "Grade Name";
                    Names_Marks[0, 2] = "Mark";

                    foreach (string data in Names_Marks_Sql)
                    {
                        if (index % 2 == 0)
                            Names_Marks[index / 2 + 1, 1] = Names_Marks_Sql.ElementAt(index);
                        else
                            Names_Marks[(index+1) / 2, 2] = Names_Marks_Sql.ElementAt(index);
                        index++;
                    }

                    Grades_NoteBook.AddLast(Names_Marks);
                }
            }
            Teacher.GradeExam(Grades_NoteBook);
        }



        /// <summary>
        /// Show the timetable for a student
        /// </summary>
        public void ToStringTimetable(string[,] tmtable)
        {
            for (int index1 = 0; index1 < tmtable.GetLength(0); index1++)
            {
                for (int index2 = 0; index2 < tmtable.GetLength(1); index2++)
                {
                    if (tmtable[index1, index2] == null)
                    {
                        for(int i = 0; i < tmtable[0,index2].Length; i++)
                        {
                            Console.Write(" ");
                        }
                        Console.Write("\t           ");
                    }
                    else
                    {
                        Console.Write(tmtable[index1, index2] + "\t           ");
                        if (tmtable[index1,index2].Length < tmtable[0, index2].Length)
                        {
                            for (int i = 0; i < tmtable[0, index2].Length - tmtable[index1, index2].Length; i++)
                            {
                                Console.Write(" ");
                            }
                        }
                    }
                }
                Console.WriteLine();
                Console.Write("--------------------------------------------------------------------------------------");
                Console.Write("--------------------------------------------------------------------------------------");
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
            LinkedListNode<string[,]> current = timetablePerWeek.First;
            while (current != null && choice == true)
            {
                Console.Clear();
                ToStringTimetable(current.Value);
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
        #endregion
    }
}

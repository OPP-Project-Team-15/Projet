using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MySql.Data.MySqlClient;


namespace Virtual_Global_College
{
    public class Admin : User
    {
        private SortedList<Subject, List<Student>> SubjectStudent { get; set; }


        #region : Methods

        public string ToString(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string sql = $"SELECT Id, FirstName, LastName, BirthDate, Sexe, PhoneNumber, Mail FROM Admins WHERE Id='{Id}'";
            cmd = new MySqlCommand(sql, conn);
            List<string> informations = Program.Pick(conn, cmd, rdr);

            string admin_Informations = $"\nAdmin Informations :\n\nId : {informations.ElementAt(0)}\nName : {informations.ElementAt(1)} {informations.ElementAt(2)}\nBirth Date : {informations.ElementAt(3)}";
            admin_Informations += $"Sexe : {informations.ElementAt(4)}\nPhone Number : {informations.ElementAt(5)}\nMail : {informations.ElementAt(6)}";

            return admin_Informations;
        }

        /// <summary>
        /// Put the subject mandatory in the timetable of the student
        /// </summary>
        //public void SubjectMandatory()
        //{
        //    foreach (KeyValuePair<Subject, List<Student>> list in SubjectStudent)
        //    {
        //        foreach (Student stud in list.Value)
        //        {
        //            foreach (string[,] timetable in stud.timetablePerWeek)
        //            {
        //                string[] tab = new string[2] { list.Key.Hours, list.Key.Day };
        //                int[] i = SearchTheIndexOfAnXAndYofMatrix(timetable, tab);
        //                if (i[0] != 0 && i[1] != 0)
        //                {
        //                    timetable[i[0], i[1]] = list.Key.NameSubject;
        //                }
        //            }
        //        }
        //    }
        //}

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
            while (cx != 1 && cx != 2 && cx != 3)
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
                            if (existAlready == false && t.Id == stud.Id)
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
                //Subject newSubj = new Subject(subj, branc, day, hour, Convert.ToString(numb++));
                //SubjectStudent.Add(newSubj, studentNewSubject);
                //SubjectMandatory();
            }
        }


        /// <summary>
        /// Allow the administration to see all the coordinates of a specific student
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
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
                Console.WriteLine("This student ID doesn't exist. Please enter an existing ID :");
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

        public void Print_Student_Attendances(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {

        }

        /// <summary>
        /// Allow the admin to see the fees history of a specific student
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdRead"></param>
        /// <param name="rdr"></param>
        public void Print_Student_Fees_History(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string sqlRead = "SELECT Id FROM Students";
            cmd = new MySqlCommand(sqlRead, conn);

            Console.WriteLine("Please enter the ID of the student who you want to see his fees history");
            string idOfStudent = Console.ReadLine();
            bool idExist = Program.Exist(idOfStudent, conn, cmd, rdr);
            while (idExist == false)
            {
                Console.WriteLine("This ID doesn't exist. Please enter an existing ID :");
                idOfStudent = Console.ReadLine();
                idExist = Program.Exist(idOfStudent, conn, cmd, rdr);
            }

            sqlRead = $"SELECT Date, Name FROM Fees WHERE IdStudent={idOfStudent}";
            cmd = new MySqlCommand(sqlRead, conn);

            Program.Read(conn, cmd, rdr, 1);
        }


        /// <summary>
        /// Allow the admin to add attendances for a student
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        public void Add_Student_Attendances(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string sql; string idOfStudent;
            string stringDate; char[] split = { '-' }; string[] dateSplited; DateTime absenceDate;
            string absenceSubject = ""; string absenceType;
            bool exist; bool newAbsenceSameID = true; bool exist1;

            // ASK FOR THE ID OF THE STUDENT
            Console.WriteLine("\nWrite the ID of the student you want to add an absence/late : ");
            idOfStudent = Console.ReadLine();

            sql = "SELECT Id FROM Students";
            cmd = new MySqlCommand(sql, conn);
            exist = Program.Exist(idOfStudent, conn, cmd, rdr);

            while (exist == false)
            {
                Console.WriteLine("This ID doesn't exist. Please enter an existing ID :");
                idOfStudent = Console.ReadLine();

                sql = "SELECT Id FROM Students";
                cmd = new MySqlCommand(sql, conn);
                exist = Program.Exist(idOfStudent, conn, cmd, rdr);
            }

            Console.WriteLine("\nHere is the attendances of the student :");

            sql = $"SELECT Date, Subject, Type FROM Attendances WHERE IdStudent={idOfStudent}";
            cmd = new MySqlCommand(sql, conn);
            int absence = Program.Read(conn, cmd, rdr, 2);
            if (absence == 0)
                Console.WriteLine("The student doesn't have absence.");

            while (newAbsenceSameID == true)
            {
                // ASK FOR THE DATE OF THE ABSENCE 
                Console.WriteLine("Specify the date of the absence (MM-DD-YYYY) : ");
                stringDate = Program.Ask_Date();
                dateSplited = stringDate.Split(split);
                absenceDate = new DateTime(Convert.ToInt32(dateSplited[2]), Convert.ToInt32(dateSplited[0]), Convert.ToInt32(dateSplited[1]));

                // ASK FOR THE SUBJECT OF THE ABSENCE
                Console.WriteLine("\nSpecify the subject : ");
                exist = false; exist1 = false;
                while (!exist || !exist1)
                {
                    exist = false; exist1 = false;

                    absenceSubject = Console.ReadLine();
                    sql = $"SELECT NameSubject FROM Subjects WHERE NameSubject='{absenceSubject}'";
                    cmd = new MySqlCommand(sql, conn);
                    exist = Program.Exist(absenceSubject, conn, cmd, rdr);

                    sql = $"SELECT NameSubject From Subjects INNER JOIN TeachersStudents ON Subjects.IdSubject = TeachersStudents.IdSubject AND TeachersStudents.IdStudent = '{idOfStudent}'";
                    cmd = new MySqlCommand(sql, conn);
                    exist1 = Program.Exist(absenceSubject, conn, cmd, rdr);

                    if (!exist || !exist1)
                        Console.WriteLine("This subject doesn't exist or the student doesn't have this subject. Please write another subject :");
                }

                // ASK FOR THE TYPE OF THE ABSENCE

                Console.WriteLine("\nSpecify the type (Late or Absent) : ");
                absenceType = Console.ReadLine();

                while (absenceType != "Late" && absenceType != "Absent")
                {
                    Console.WriteLine("Please write Late or Absent ");
                    absenceType = Console.ReadLine();
                }

                // FINAL CHECK

                Console.WriteLine($"\nYou want to add an absence for this student (ID : {idOfStudent}), the {stringDate}, in {absenceSubject} for {absenceType} ? Is that good for you ?\n- yes\n- no");
                string answer = Console.ReadLine();

                while (answer != "yes" && answer != "no")
                {
                    Console.WriteLine($"Incorrect. Please write as it's written :");
                    answer = Console.ReadLine();
                }

                if (answer == "no")
                    Add_Student_Attendances(conn, cmd, rdr);

                // ADD THE ATTENDANCE IN THE DATA BASE

                else
                {
                    //sql = $"INSERT INTO Attendances (Date, Subject, Type, IdStudent) VALUES ('{absenceDate}','{absenceSubject}','{absenceType}','{idOfStudent}')";
                    //cmd = new SqlCommand(sql, conn);
                    sql = "INSERT INTO Attendances SET Date = @date, Subject = @subject, Type = @type, IdStudent = @idstudent";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@date", absenceDate);
                    cmd.Parameters.AddWithValue("@subject", absenceSubject);
                    cmd.Parameters.AddWithValue("@type", absenceType);
                    cmd.Parameters.AddWithValue("@idstudent", idOfStudent);
                    Program.Insert(conn, cmd, rdr);
                }

                Console.WriteLine("\nThe attendance has been added in the data base.");

                Console.WriteLine("\nDo you want to add another absence/late for this student ?\n- yes\n- no");
                string answer2 = Console.ReadLine();

                while (answer2 != "yes" && answer2 != "no")
                {
                    Console.WriteLine("\nIncorrect. Do you want to add another absence/late for this student ? Answer by yes or no : ");
                    answer2 = Console.ReadLine();
                }

                if (answer2 == "no")
                    newAbsenceSameID = false;
            }

            Console.WriteLine("\nOK. Do you want to add an absence for another student ? \n- yes\n- no ");
            string answer3 = Console.ReadLine();

            while (answer3 != "yes" && answer3 != "no")
            {
                Console.WriteLine("Incorrect. Do you want to add another absence for another student ? Answer by yes or no :");
                answer3 = Console.ReadLine();
            }

            if (answer3 == "yes")
                Add_Student_Attendances(conn, cmd, rdr);

            else
                Console.WriteLine("\nThank you. Goodbye ;)");
        }

        /// <summary>
        /// Allox the admin to add a subject and teacher who teach this subject
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        public void Add_Subject(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string subjectName = ""; string subjectBranch = ""; bool subjectRequired = false;
            string[] dateSplited; char[] split = { '-' }; DateTime birthDate;
            string sql = ""; string answer = "no"; string requiredAnswer;
            List<string> teacher_Info = new List<string>(); List<string> Id; List<string> teachersName = new List<string>();
            string teacherId = ""; string subjectId;
            bool exist = false; bool subjectExist = true; List<string> sameSubject;

            while (answer == "no") // asnwer is 'no' at the end if the admin make a mistake in the adding of the subject
            {
                Console.WriteLine("\nEnter the name of the subject you want to add :");
                while (subjectExist)
                {
                    // NAME OF THE SUBJECT
                    subjectName = Console.ReadLine();

                    // BRANCH OF THE SUBJECT
                    Console.WriteLine("\nEnter the branch of the subject :");
                    subjectBranch = Console.ReadLine();
                    while (subjectBranch != "ESILV" && subjectBranch != "EMLV" && subjectBranch != "IIM")
                    {
                        Console.WriteLine("This branch doesn't exist. Please select between ESILV, EMLV and IIM :");
                        subjectBranch = Console.ReadLine();
                    }

                    // WE CHECK IF THE SUBJECT ALREADY EXIST
                    sql = $"SELECT IdSubject FROM Subjects WHERE NameSubject='{subjectName}' AND Branch='{subjectBranch}'";
                    cmd = new MySqlCommand(sql, conn);
                    sameSubject = Program.Pick(conn, cmd, rdr);

                    if (sameSubject.Count == 0)
                        subjectExist = false;
                    else
                        Console.WriteLine("\nThis subject already exist. Please enter another subject or another branch :");
                }

                // REQUIRED ?
                Console.WriteLine("\nIs this subject required or not ?\n- yes\n- no");
                requiredAnswer = Console.ReadLine();
                while (requiredAnswer != "yes" && requiredAnswer != "no")
                {
                    Console.WriteLine("Please select a correct answer :");
                    requiredAnswer = Console.ReadLine();
                }
                if (requiredAnswer == "yes")
                    subjectRequired = true;
                else
                    subjectRequired = false;

                // FINAL CHECK
                Console.WriteLine($"\nYou are adding a new subject '{subjectName}' in the branch '{subjectBranch}'\nDoes it suit you ?\n- yes\n- no");
                answer = Console.ReadLine();
                while (answer != "yes" && answer != "no")
                {
                    Console.WriteLine("Please select a correct answer :");
                    answer = Console.ReadLine();
                }
            }

            // ADD THE SUBJECT IN THE DATA BASE
            //sql = $"INSERT INTO Subjects (NameSubject, Branch, Required) VALUES ('{subjectName}', '{subjectBranch}', '{subjectRequired}')";
            //cmd = new SqlCommand(sql, conn);
            sql = "INSERT INTO Subjects SET NameSubject = @namesubject, Branch = @branch, Required = @required";
            cmd = new MySqlCommand(sql, conn);
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@namesubject", subjectName);
            cmd.Parameters.AddWithValue("@branch", subjectBranch);
            cmd.Parameters.AddWithValue("@required", subjectRequired);
            Program.Insert(conn, cmd, rdr);

            Console.WriteLine("\nThe subject has been added in the database.\n\nThe subject must be teached by at least one teacher.");

            // WE OBTAIN THE ID OF THIS SUBJECT
            sql = $"SELECT IdSubject FROM Subjects WHERE NameSubject='{subjectName}' AND Branch='{subjectBranch}' AND Required='{subjectRequired}'";
            cmd = new MySqlCommand(sql, conn);
            Id = Program.Pick(conn, cmd, rdr);
            subjectId = Id.ElementAt(0);

            answer = "yes"; // answer is 'yes' at the end if the admin want to add another teacher for the subject
            while (answer == "yes")
            {
                Console.WriteLine("Do you want to add a new teacher who will teach the subject, or add the subject to a teacher's subjects ?\n- new teacher\n- existing teacher");
                answer = Console.ReadLine();
                while (answer != "new teacher" && answer != "existing teacher")
                {
                    Console.WriteLine("Please write as it's written");
                    answer = Console.ReadLine();
                }

                if (answer == "new teacher")
                {
                    // ASK FOR THE TEACHER INFORMATION
                    Console.WriteLine("\nPlease write the informations about the teacher :");
                    teacher_Info = Program.User_Informations("");
                    while (teacher_Info[6] != subjectBranch)
                    {
                        Console.WriteLine($"\nThe teacher must be in the same branch as the subject ({subjectBranch}). Please put the same branch for the teacher :");
                        teacher_Info = Program.User_Informations("");
                    }

                    teachersName.Add($"{teacher_Info[0]} {teacher_Info[1]}");

                    dateSplited = teacher_Info[2].Split(split);
                    birthDate = new DateTime(Convert.ToInt32(dateSplited[2]), Convert.ToInt32(dateSplited[0]), Convert.ToInt32(dateSplited[1]));

                    // ADD THE TEACHER IN THE DATA BASE
                    //sql = $"INSERT INTO Teachers (FirstName, LastName, BirthDate, Sexe, PhoneNumber, Mail, Branch, Password) VALUES ('{teacher_Info[0]}', '{teacher_Info[1]}', '{birthDate}', '{teacher_Info[3]}', '{teacher_Info[4]}', '{teacher_Info[5]}', '{teacher_Info[6]}', '{teacher_Info[7]}')";
                    //cmd = new SqlCommand(sql, conn);
                    sql = "INSERT INTO Teachers SET FirstName = @firstname, LastName = @lastname, BirthDate = @birthdate, Sexe = @sexe, PhoneNumber = @phonenumber, Mail = @mail, Branch = @branch, Password = @password";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@firstname", teacher_Info[0]);
                    cmd.Parameters.AddWithValue("@lastname", teacher_Info[1]);
                    cmd.Parameters.AddWithValue("@birthdate", birthDate);
                    cmd.Parameters.AddWithValue("@sexe", teacher_Info[3]);
                    cmd.Parameters.AddWithValue("@phonenumber", teacher_Info[4]);
                    cmd.Parameters.AddWithValue("@mail", teacher_Info[5]);
                    cmd.Parameters.AddWithValue("@branch", teacher_Info[6]);
                    cmd.Parameters.AddWithValue("@password", teacher_Info[7]);
                    Program.Insert(conn, cmd, rdr);

                    Console.WriteLine("\nThe teacher has been added in the data base.");

                    // WE OBTAIN THE ID OF THIS TEACHER
                    sql = $"SELECT Id FROM Teachers WHERE FirstName='{teacher_Info[0]}' AND LastName='{teacher_Info[1]}' AND PhoneNumber='{teacher_Info[4]}'";
                    cmd = new MySqlCommand(sql, conn);
                    Id = Program.Pick(conn, cmd, rdr);
                    teacherId = Id[0];
                }

                else
                {
                    // WE OBTAIN THE TEACHER ID
                    Console.WriteLine("\nPlease enter the ID of the teacher : :");
                    while (exist == false)
                    {
                        teacherId = Console.ReadLine();
                        sql = $"SELECT Id FROM Teachers WHERE Branch='{subjectBranch}'";
                        cmd = new MySqlCommand(sql, conn);
                        exist = Program.Exist(teacherId, conn, cmd, rdr);

                        if (exist == false)
                            Console.WriteLine("This ID is incorrect (or doesn't correspond to a teacher which have the same branch as the subject). Please enter a correct teacher ID :");
                    }

                    sql = $"SELECT FirstName, LastName FROM Teachers WHERE Id='{teacherId}'";
                    cmd = new MySqlCommand(sql, conn);
                    teacher_Info = Program.Pick(conn, cmd, rdr);
                    teachersName.Add($"{teacher_Info[0]} {teacher_Info[1]}");
                }

                // ADD THE SUBJECT TEACHER IN THE DATA BASE
                //sql = $"INSERT INTO SubjectsTeachers (IdTeacher, IdSubject) VALUES ('{teacherId}', '{subjectId}')";
                //cmd = new SqlCommand(sql, conn);
                sql = "INSERT INTO SubjectsTeachers SET IdTeacher = @idteacher, IdSubject = @idsubject";
                cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@idteacher", teacherId);
                cmd.Parameters.AddWithValue("@idsubject", subjectId);
                Program.Insert(conn, cmd, rdr);

                Console.WriteLine("\nThe teacher's subject has been added in the data base.");

                // ASK FOR ADDING ANOTHER TEACHER FOR THE SUBJECT
                Console.WriteLine("\nWould you want to add another teacher who will teach the subject or associate the subject to an existing teacher ?\n- yes\n- no");
                answer = Console.ReadLine();
                while (answer != "yes" && answer != "no")
                {
                    Console.WriteLine("Please select as it's written :");
                    answer = Console.ReadLine();
                }

                if (answer == "yes")
                    Console.WriteLine();
            }

            if (teachersName.Count == 1)
                Console.WriteLine($"\nThe subject {subjectName} can be teach next semester.\nIt will be teach by {teachersName.Count} teacher for the moment : {teachersName[0]}");
            else
            {
                Console.WriteLine($"\nThe subject {subjectName} can be teach next semester.\nIt will be teach by {teachersName.Count} teachers for the moment :");
                foreach (string name in teachersName)
                    Console.WriteLine(name);
            }

            Console.WriteLine("\nThank you. Have a good day :)");
        }

        /// <summary>
        /// Allow the admin to add a teacher in the database, and to give him/her one or many subjects of his/her branch
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        public void Add_Teacher(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string sql; string answer; string subjectPicked;
            string[] dateSplited; char[] split = { '-' }; DateTime birthDate;
            List<string> teacher_Info; List<string> teacherId;
            List<string> subjectId; List<string> branchSubjects; List<string> teacherSubjects = new List<string>();
            bool otherSubject = true;

            // ASK FOR THE TEACHER INFORMATION
            Console.WriteLine("\nPlease write the informations about the teacher :");
            teacher_Info = Program.User_Informations();

            dateSplited = teacher_Info[2].Split(split);
            birthDate = new DateTime(Convert.ToInt32(dateSplited[2]), Convert.ToInt32(dateSplited[0]), Convert.ToInt32(dateSplited[1]));

            // ADD THE TEACHER IN THE DATA BASE
            //sql = $"INSERT INTO Teachers (FirstName, LastName, BirthDate, Sexe, PhoneNumber, Mail, Branch, Password) VALUES ('{teacher_Info[0]}', '{teacher_Info[1]}', '{birthDate}', '{teacher_Info[3]}', '{teacher_Info[4]}', '{teacher_Info[5]}', '{teacher_Info[6]}', '{teacher_Info[7]}')";
            //cmd = new SqlCommand(sql, conn);

            sql = "INSERT INTO Teachers SET FirstName = @firstname, LastName = @lastname, BirthDate = @birthdate, Sexe = @sexe, PhoneNumber = @phonenumber, Mail = @mail, Branch = @branch, Password = @password";
            cmd = new MySqlCommand(sql, conn);
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@firstname", teacher_Info[0]);
            cmd.Parameters.AddWithValue("@lastname", teacher_Info[1]);
            cmd.Parameters.AddWithValue("@birthdate", birthDate);
            cmd.Parameters.AddWithValue("@sexe", teacher_Info[3]);
            cmd.Parameters.AddWithValue("@phonenumber", teacher_Info[4]);
            cmd.Parameters.AddWithValue("@mail", teacher_Info[5]);
            cmd.Parameters.AddWithValue("@branch", teacher_Info[6]);
            cmd.Parameters.AddWithValue("@password", teacher_Info[7]);
            Program.Insert(conn, cmd, rdr);

            Console.WriteLine("\nThe teacher has been added in the data base.");

            // ADD A TEACHER'S SUBJECT
            Console.WriteLine("\nWould you want to add a teacher's subject ?\n- yes \n- no");
            answer = Console.ReadLine();
            while (answer != "yes" && answer != "no")
            {
                Console.WriteLine("Please select a correct answer :");
                answer = Console.ReadLine();
            }

            if (answer == "yes")
            {
                // WE OBTAIN THE TEACHER ID
                sql = $"SELECT Id FROM Teachers WHERE FirstName='{teacher_Info[0]}' AND LastName='{teacher_Info[1]}' AND PhoneNumber='{teacher_Info[4]}'";
                cmd = new MySqlCommand(sql, conn);
                teacherId = Program.Pick(conn, cmd, rdr);

                // WE OBTAIN AND PRINT THE SUBJECTS OF THE TEACHER BRANCH
                Console.WriteLine("\nHere is the subjects of the branch teacher :");
                sql = $"SELECT NameSubject FROM Subjects WHERE Branch='{teacher_Info[6]}'";
                cmd = new MySqlCommand(sql, conn);
                Program.Read(conn, cmd, rdr, 3);
                branchSubjects = Program.Pick(conn, cmd, rdr);

                // WE ASK THE NEW TEACHER'S SUBJECT
                Console.WriteLine("\nWhich subject will the teacher teach ?");

                while (otherSubject)
                {
                    subjectPicked = Console.ReadLine();
                    while (!branchSubjects.Contains(subjectPicked) || teacherSubjects.Contains(subjectPicked))
                    {
                        if (!branchSubjects.Contains(subjectPicked))
                            Console.WriteLine("This subject doesn't exist for this branch. Please write as it's written :");
                        else
                            Console.WriteLine("The teacher already has this subject. Please write another one :");
                        subjectPicked = Console.ReadLine();
                    }

                    // WE OBTAIN THE SUBJECT ID
                    sql = $"SELECT IdSubject FROM Subjects WHERE NameSubject='{subjectPicked}' AND Branch='{teacher_Info[6]}'";
                    cmd = new MySqlCommand(sql, conn);
                    subjectId = Program.Pick(conn, cmd, rdr);

                    // WE INSERT THE TEACHER'S SUBJECT IN THE DATA BASE
                    //sql = $"INSERT INTO SubjectsTeachers (IdTeacher, IdSubject) VALUES ('{teacherId[0]}', '{subjectId[0]}')";
                    sql = "INSERT INTO SubjectsTeachers SET IdTeacher = @idteacher, IdSubject = @idsubject";
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@idteacher", teacherId[0]);
                    cmd.Parameters.AddWithValue("@idsubject", subjectId[0]);
                    Program.Insert(conn, cmd, rdr);
                    teacherSubjects.Add(subjectPicked);

                    Console.WriteLine($"\nThe teacher's subject has been added in the database. He/She can now teach the subject '{subjectPicked}'.");

                    // WE ASK FOR ANOTHER SUBJECT'S TEACHER
                    Console.WriteLine("\nDo you want to add another subject for this teacher ?\n- yes\n- no");
                    answer = Console.ReadLine();
                    while (answer != "yes" && answer != "no")
                    {
                        Console.WriteLine("Please select a correct answer :");
                        answer = Console.ReadLine();
                    }

                    if (answer == "no")
                        otherSubject = false;
                    else
                        Console.WriteLine("\nWhich other subject can the teacher teach ?");
                }

                if (teacherSubjects.Count == 1)
                    Console.WriteLine($"\nThe teacher {teacher_Info[0]} {teacher_Info[1]} (ID : {teacherId[0]}) can now teach the subject : {teacherSubjects[0]}");
                else
                {
                    Console.WriteLine($"\nThe teacher {teacher_Info[0]} {teacher_Info[1]} (ID : {teacherId[0]}) can now teach the subjects :");
                    foreach (string subject in teacherSubjects)
                        Console.WriteLine(subject);
                }
            }

            Console.WriteLine("\nThank you. Have a good day :)");
        }

        /// <summary>
        /// Allow the admin to add a subject which will be teach by a teacher
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        public void Add_Subject_Teacher(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string idOfTeacher = ""; string sql; string answer; string subjectPicked;
            bool idExist = false; bool otherSubject = true;
            List<string> teacherSubjects; List<string> branchSubjects;
            List<string> teacherBranch; List<string> subjectId;

            // WE ASK FOR THE TEACHER ID

            Console.WriteLine("\nPlease enter the ID of the teacher you want to add a subject :");

            while (idExist == false)
            {
                idOfTeacher = Console.ReadLine();
                sql = "SELECT Id FROM Teachers";
                cmd = new MySqlCommand(sql, conn);
                idExist = Program.Exist(idOfTeacher, conn, cmd, rdr);

                if (idExist == false)
                    Console.WriteLine("This teacher ID doesn't exist. Please enter an existing ID :");
            }

            // WE OBTAIN THE SUBJECTS TEACHER

            sql = $"SELECT NameSubject FROM Subjects INNER JOIN SubjectsTeachers ON SubjectsTeachers.IdSubject = Subjects.IdSubject AND SubjectsTeachers.IdTeacher = '{idOfTeacher}'";
            cmd = new MySqlCommand(sql, conn);
            teacherSubjects = Program.Pick(conn, cmd, rdr);

            Console.WriteLine("\nHere, the subjects which are teached by the teacher :");
            foreach (string subject in teacherSubjects)
                Console.WriteLine(subject);

            // WE OBTAIN THE BRANCH OF THE TEACHER

            sql = $"SELECT Branch FROM Teachers WHERE Id='{idOfTeacher}'";
            cmd = new MySqlCommand(sql, conn);
            teacherBranch = Program.Pick(conn, cmd, rdr);

            // WE OBTAIN AND PRINT THE SUBJECTS WHICH THE TEACHER CAN TEACH (SUBJECTS OF HIS BRANCH)

            sql = $"SELECT NameSubject FROM Subjects WHERE Branch='{teacherBranch[0]}'";
            cmd = new MySqlCommand(sql, conn);
            branchSubjects = Program.Pick(conn, cmd, rdr);
            Console.WriteLine("\nHere, the subjects which can be teached by the teacher :");
            int index = 1;
            foreach (string subject in branchSubjects)
            {
                if (!teacherSubjects.Contains(subject))
                {
                    Console.WriteLine($"{index} - {subject}");
                    index++;
                }
            }

            // WE ASK THE NEW TEACHER'S SUBJECT
            Console.WriteLine("\nWhich subject will the teacher teach ?");

            while (otherSubject)
            {
                subjectPicked = Console.ReadLine();
                while (!branchSubjects.Contains(subjectPicked) || teacherSubjects.Contains(subjectPicked))
                {
                    if (!branchSubjects.Contains(subjectPicked))
                        Console.WriteLine("This subject doesn't exist for this branch. Please write as it's written :");
                    else
                        Console.WriteLine("The teacher already has this subject. Please write another one :");
                    subjectPicked = Console.ReadLine();
                }

                // WE OBTAIN THE SUBJECT ID
                sql = $"SELECT IdSubject FROM Subjects WHERE NameSubject='{subjectPicked}' AND Branch='{teacherBranch[0]}'";
                cmd = new MySqlCommand(sql, conn);
                subjectId = Program.Pick(conn, cmd, rdr);

                // WE INSERT THE TEACHER'S SUBJECT IN THE DATA BASE
                //sql = $"INSERT INTO SubjectsTeachers (IdTeacher, IdSubject) VALUES ('{idOfTeacher}', '{subjectId[0]}')";
                //cmd = new SqlCommand(sql, conn);
                sql = "INSERT INTO SubjectsTeachers SET IdTeacher = @idteacher, IdSubject = @idsubject";
                cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@idteacher", idOfTeacher);
                cmd.Parameters.AddWithValue("@idsubject", subjectId[0]);
                Program.Insert(conn, cmd, rdr);
                teacherSubjects.Add(subjectPicked);

                Console.WriteLine($"\nThe teacher's subject has been added in the database. He/She can now teach the subject '{subjectPicked}'.");

                // WE ASK FOR ANOTHER SUBJECT'S TEACHER
                Console.WriteLine("\nDo you want to add another subject for this teacher ?\n- yes\n- no");
                answer = Console.ReadLine();
                while (answer != "yes" && answer != "no")
                {
                    Console.WriteLine("Please select a correct answer :");
                    answer = Console.ReadLine();
                }

                if (answer == "no")
                    otherSubject = false;
                else
                    Console.WriteLine("\nWhich other subject can the teacher teach ?");
            }

            if (teacherSubjects.Count == 1)
                Console.WriteLine($"\nThe teacher (ID : {idOfTeacher}) can now teach the subject : {teacherSubjects[0]}");
            else
            {
                Console.WriteLine($"\nThe teacher (ID : {idOfTeacher}) can now teach the subjects :");
                foreach (string subject in teacherSubjects)
                    Console.WriteLine(subject);
            }

            Console.WriteLine("\nThank you. Have a good day :)");

        }

        /// <summary>
        /// Allow the admin to modify/delete a attendance of a student
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        public void Modify_Student_Attendances(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string answer; string idOfStudent; string modifySubjectAbsence;
            string sql; string stringDate; int absence; string answer2;
            bool exist; bool verif; bool modif = true;
            string[] dateSplited; char[] split = { '-' }; DateTime absenceDate;
            List<string> absenceType; List<string> studentName;

            Console.WriteLine("\nDo you want to modify the attendances of a student ?\n - yes\n - no");
            answer = Console.ReadLine();
            while (answer != "yes" && answer != "no")
            {
                Console.WriteLine($"Incorrect. Please write as it's written :");
                answer = Console.ReadLine();
            }

            if (answer == "no")
                Console.WriteLine("Ok, have a nice day :-)");

            else
            {
                Console.WriteLine("\nOf which student do you want to modify the attendance ? Write his/her ID : ");
                idOfStudent = Console.ReadLine();
                sql = "SELECT Id FROM Students";
                cmd = new MySqlCommand(sql, conn);
                exist = Program.Exist(idOfStudent, conn, cmd, rdr);

                while (exist == false)
                {
                    Console.WriteLine("This ID doesn't exist. Please enter an existing ID :");
                    idOfStudent = Console.ReadLine();

                    sql = "SELECT Id FROM Students";
                    cmd = new MySqlCommand(sql, conn);
                    exist = Program.Exist(idOfStudent, conn, cmd, rdr);
                }

                Console.WriteLine("\nHere is the attendances of the student :");

                sql = $"SELECT Date, Subject, Type FROM Attendances WHERE IdStudent={idOfStudent}";
                cmd = new MySqlCommand(sql, conn);
                absence = Program.Read(conn, cmd, rdr, 2);
                if (absence == 0)
                    Console.WriteLine("The student doesn't have absence.");

                sql = $"SELECT FirstName, LastName FROM Students WHERE Id = '{idOfStudent}'";
                cmd = new MySqlCommand(sql, conn);
                studentName = Program.Pick(conn, cmd, rdr);

                // ASK FOR THE SUBJECT OF THE ABSENCE

                Console.WriteLine($"\nFor which course do you want to modify the absence of {studentName[0]} {studentName[1]}");
                modifySubjectAbsence = Console.ReadLine();

                sql = $"SELECT Subject FROM Attendances WHERE IdStudent={idOfStudent}";
                cmd = new MySqlCommand(sql, conn);
                verif = Program.Exist(modifySubjectAbsence, conn, cmd, rdr);

                while (verif == false)
                {
                    Console.WriteLine("Please write an existing subject");
                    modifySubjectAbsence = Console.ReadLine();
                    verif = Program.Exist(modifySubjectAbsence, conn, cmd, rdr);
                }

                // ASK FOR THE DATE OF THE ABSENCE

                Console.WriteLine($"\nSpecify the date of the absence (MM-DD-YYYY) of {studentName[0]} {studentName[1]} in {modifySubjectAbsence} : ");
                stringDate = Program.Ask_Date();

                sql = $"SELECT Date FROM Attendances WHERE IdStudent={idOfStudent} AND Subject = '{modifySubjectAbsence}'";
                cmd = new MySqlCommand(sql, conn);
                verif = Program.Exist(stringDate, conn, cmd, rdr);
                while (verif == false)
                {
                    Console.WriteLine("Please write an existing date");
                    stringDate = Program.Ask_Date();
                    verif = Program.Exist(stringDate, conn, cmd, rdr);
                }
                dateSplited = stringDate.Split(split);
                absenceDate = new DateTime(Convert.ToInt32(dateSplited[2]), Convert.ToInt32(dateSplited[0]), Convert.ToInt32(dateSplited[1]));

                sql = $"SELECT Type FROM Attendances WHERE IdStudent='{idOfStudent}' AND Date = '{absenceDate}' AND Subject = '{modifySubjectAbsence}'";
                cmd = new MySqlCommand(sql, conn);
                absenceType = Program.Pick(conn, cmd, rdr);

                Console.WriteLine($"\nDo you want to modify the absence of {studentName[0]} {studentName[1]} in {modifySubjectAbsence} the {stringDate} for {absenceType[0]} ?\n - yes\n - no");
                answer2 = Console.ReadLine();

                while (answer2 != "yes" && answer2 != "no")
                {
                    Console.WriteLine($"Incorrect. Please write as it's written.\n - yes\n - no");
                    answer2 = Console.ReadLine();
                }

                if (answer2 == "yes")
                {
                    Console.WriteLine("\nDo you want to modify it or delete it?\n - modify\n - delete");
                    string caseSwitchTeacher = Console.ReadLine();

                    while (modif)
                    {
                        switch (caseSwitchTeacher)
                        {
                            case "modify":
                                Console.WriteLine($"\nDo you want to change {studentName[0]} {studentName[1]}'s attendance by a late or a absent : \n - late\n - absent");
                                string answer3 = Console.ReadLine();

                                while (answer3 != "late" && answer3 != "absent")
                                {
                                    Console.WriteLine("Please write as it's written.");
                                    answer3 = Console.ReadLine();
                                }

                                if (answer3 == "late")
                                {
                                    //sql = $"UPDATE Attendances SET Type ='Late' WHERE Date = '{absenceDate}' AND Subject = '{modifySubjectAbsence}' AND Type = '{absenceType[0]}' AND IdStudent='{idOfStudent}'";
                                    //cmd = new SqlCommand(sql, conn);
                                    sql = "UPDATE Attendances SET Type ='Late' WHERE Date = @date, Subject = @subject, Type = @type, IdStudent = @idstudent";
                                    cmd = new MySqlCommand(sql, conn);
                                    cmd.Prepare();
                                    cmd.Parameters.AddWithValue("@date", absenceDate);
                                    cmd.Parameters.AddWithValue("@subject", modifySubjectAbsence);
                                    cmd.Parameters.AddWithValue("@type", absenceType[0]);
                                    cmd.Parameters.AddWithValue("@idstudent", idOfStudent);
                                    Program.Insert(conn, cmd, rdr);
                                    Console.WriteLine("\nThe attendance has been modified to : late");
                                    modif = false;
                                }

                                else
                                {
                                    sql = "UPDATE Attendances SET Type ='Absent' WHERE Date = @date, Subject = @subject, Type = @type, IdStudent = @idstudent";
                                    cmd = new MySqlCommand(sql, conn);
                                    cmd.Prepare();
                                    cmd.Parameters.AddWithValue("@date", absenceDate);
                                    cmd.Parameters.AddWithValue("@subject", modifySubjectAbsence);
                                    cmd.Parameters.AddWithValue("@type", absenceType[0]);
                                    cmd.Parameters.AddWithValue("@idstudent", idOfStudent);
                                    Program.Insert(conn, cmd, rdr);

                                    Console.WriteLine("\nThe attendance has been modified to : absent");
                                    modif = false;
                                }
                                break;
                            case "delete":
                                sql = $"DELETE FROM Attendances WHERE Date = '{absenceDate}' AND Subject = '{modifySubjectAbsence}' AND Type = '{absenceType[0]}' AND IdStudent='{idOfStudent}'";
                                cmd = new MySqlCommand(sql, conn);
                                Program.Delete(conn, cmd, rdr);
                                Console.WriteLine("\nThe attendance has been deleted.");
                                modif = false;
                                break;
                            default:
                                Console.WriteLine("\nIncorrect. Please write as it's written. ");
                                caseSwitchTeacher = Console.ReadLine();
                                break;
                        }
                    }
                }

                Console.WriteLine("\nThank you. Have a good day :)");
            }


        }

        public void Modify_Subject(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {

        }


        /// <summary>
        /// Allow the admin to delete all datas about a teacher
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        public void Delete_Student(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string idOfStudent; string answer; string sql;
            bool exist;

            // RECUPE THE ID OF THE STUDENT 
            Console.WriteLine("\nEnter the ID of the student you want to delete : ");
            idOfStudent = Console.ReadLine();

            sql = "SELECT Id FROM Students";
            cmd = new MySqlCommand(sql, conn);
            exist = Program.Exist(idOfStudent, conn, cmd, rdr);

            while (exist == false)
            {
                Console.WriteLine("This ID doesn't exist. Please enter an existing ID :");
                idOfStudent = Console.ReadLine();

                sql = "SELECT Id FROM Students";
                cmd = new MySqlCommand(sql, conn);
                exist = Program.Exist(idOfStudent, conn, cmd, rdr);
            }

            List<string> studentName;
            sql = $"SELECT FirstName, LastName FROM Students WHERE Id = '{idOfStudent}'";
            cmd = new MySqlCommand(sql, conn);
            studentName = Program.Pick(conn, cmd, rdr);


            Console.WriteLine($"\nDo you really want to delete {idOfStudent}, {studentName[0]} {studentName[1]} ? \n- yes\n- no");
            answer = Console.ReadLine();

            while (answer != "yes" && answer != "no")
            {
                Console.WriteLine("\nIncorrect. Please write yes or no.");
                answer = Console.ReadLine();
            }

            if (answer == "yes")
            {
                // DELETE THE STUDENT FROM THE TABLE ATTENDANCES
                sql = $"DELETE FROM Attendances WHERE IdStudent = '{idOfStudent}'";
                cmd = new MySqlCommand(sql, conn);
                Program.Delete(conn, cmd, rdr);

                // DELETE THE STUDENT FROM THE TABLE GRADES
                sql = $"DELETE FROM Grades WHERE IdStudent = '{idOfStudent}'";
                cmd = new MySqlCommand(sql, conn);
                Program.Delete(conn, cmd, rdr);

                // DELETE THE STUDENT FROM THE TABLE ASSIGNMENTS
                sql = $"DELETE FROM Assignments WHERE IdStudent = '{idOfStudent}'";
                cmd = new MySqlCommand(sql, conn);
                Program.Delete(conn, cmd, rdr);

                // DELETE THE STUDENT FROM THE TABLE FEES
                sql = $"DELETE FROM Fees WHERE IdStudent = '{idOfStudent}'";
                cmd = new MySqlCommand(sql, conn);
                Program.Delete(conn, cmd, rdr);

                // DELETE THE STUDENT FROM THE TABLE TEACHERSSTUDENTS
                sql = $"DELETE FROM TeachersStudents WHERE IdStudent = '{idOfStudent}'";
                cmd = new MySqlCommand(sql, conn);
                Program.Delete(conn, cmd, rdr);

                // DELETE THE STUDENT FROM THE TABLE STUDENTS
                sql = $"DELETE FROM Students WHERE Id = '{idOfStudent}'";
                cmd = new MySqlCommand(sql, conn);
                Program.Delete(conn, cmd, rdr);

                Console.WriteLine("\nThe student has been deleted.");
            }

            Console.WriteLine("\nOk. Do you want to delete another student ? \n- yes\n- no");
            string answer2 = Console.ReadLine();

            while (answer2 != "yes" && answer2 != "no")
            {
                Console.WriteLine("\nIncorrect. Please write yes or no.");
                answer2 = Console.ReadLine();
            }

            if (answer2 == "yes")
                Delete_Student(conn, cmd, rdr);

            else
                Console.WriteLine("\nOK. Goodbye :-)");
        }

        /// <summary>
        /// Allow the admin to delete all datas about a student
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="rdr"></param>
        public void Delete_Teacher(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            string sql; string answer;
            bool exist;
            string idOfTeacher; List<string> teacherName;

            // RECUPE THE ID OF THE TEACHER 
            Console.WriteLine("\nEnter the ID of the teacher you want to delete : ");
            idOfTeacher = Console.ReadLine();

            sql = "SELECT Id FROM Teachers";
            cmd = new MySqlCommand(sql, conn);
            exist = Program.Exist(idOfTeacher, conn, cmd, rdr);

            while (exist == false)
            {
                Console.WriteLine("This ID doesn't exist. Please enter an existing ID :");
                idOfTeacher = Console.ReadLine();

                sql = "SELECT Id FROM Teachers";
                cmd = new MySqlCommand(sql, conn);
                exist = Program.Exist(idOfTeacher, conn, cmd, rdr);
            }


            sql = $"SELECT FirstName, LastName FROM Teachers WHERE Id = '{idOfTeacher}'";
            cmd = new MySqlCommand(sql, conn);
            teacherName = Program.Pick(conn, cmd, rdr);


            Console.WriteLine($"\nDo you really want to delete {idOfTeacher}, {teacherName[0]} {teacherName[1]} ? \n- yes\n- no");
            answer = Console.ReadLine();

            while (answer != "yes" && answer != "no")
            {
                Console.WriteLine("\nIncorrect. Please write yes or no.");
                answer = Console.ReadLine();
            }

            if (answer == "yes")
            {
                // DELETE THE TEACHER FROM THE TABLE GRADES
                sql = $"DELETE FROM Grades WHERE IdTeacher = '{idOfTeacher}'";
                cmd = new MySqlCommand(sql, conn);
                Program.Delete(conn, cmd, rdr);

                // DELETE THE TEACHER FROM THE TABLE TEACHERSSTUDENTS
                sql = $"DELETE FROM TeachersStudents WHERE IdTeacher = '{idOfTeacher}'";
                cmd = new MySqlCommand(sql, conn);
                Program.Delete(conn, cmd, rdr);

                // DELETE THE TEACHER FROM THE TABLE SUBJECTSTEACHERS
                sql = $"DELETE FROM SubjectsTeachers WHERE IdTeacher = '{idOfTeacher}'";
                cmd = new MySqlCommand(sql, conn);
                Program.Delete(conn, cmd, rdr);

                // DELETE THE TEACHER FROM THE TABLE TEACHERS
                sql = $"DELETE FROM Teachers WHERE Id = '{idOfTeacher}'";
                cmd = new MySqlCommand(sql, conn);
                Program.Delete(conn, cmd, rdr);

                Console.WriteLine("\nThe teacher has been deleted.");
            }

            Console.WriteLine("\nOk. Do you want to delete another teacher ? \n- yes\n- no");
            string answer2 = Console.ReadLine();

            while (answer2 != "yes" && answer2 != "no")
            {
                Console.WriteLine("\nIncorrect. Please write yes or no.");
                answer2 = Console.ReadLine();
            }

            if (answer2 == "yes")
                Delete_Teacher(conn, cmd, rdr);

            else
                Console.WriteLine("\nOK. Goodbye :-)");
        }
        #endregion
    }
}

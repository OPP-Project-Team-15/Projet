using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;



namespace Virtual_Global_College
{
    public class Program
    {
        /// <summary>
        /// Allow to write sql datas in the console
        /// </summary>
        /// <param name="conn"> SqlConnection </Mparam>
        /// <param name="cmd"> SqlCommand </param>
        /// <param name="rdr"> SqlDataReader </param>
        /// <param name="PrintType"> 1 for a print with a WriteLine(), 2 for a print with a Write() and space by a dash, 3 for a print of one select data with a Write()</param>
        /// <returns></returns>
        public static int Read(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr, int PrintType)
        {
            int rdrCount = 0;
            int index = 1;
            DateTime mydate;

            try
            {
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (PrintType == 1)
                    {
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            if (DateTime.TryParse(rdr[i].ToString(), out mydate))
                            {
                                Console.WriteLine(rdr[i].ToString().Remove(10, 9));
                                rdrCount++;
                            }
                            else
                            {
                                Console.WriteLine(rdr[i]);
                                rdrCount++;
                            }
                        }
                        Console.WriteLine();
                    }

                    else if (PrintType == 2)
                    {
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            if (DateTime.TryParse(rdr[i].ToString(), out mydate))
                            {
                                Console.Write($" - {rdr[i].ToString().Remove(10, 9)}");
                                rdrCount++;
                            }
                            else
                            {
                                Console.Write($" - {rdr[i]}");
                                rdrCount++;
                            }
                        }
                        Console.WriteLine();
                    }

                    else if (PrintType == 3)
                    {
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            if (DateTime.TryParse(rdr[i].ToString(), out mydate))
                            {
                                if (index < 10)
                                {
                                    Console.WriteLine($" {index} - {rdr[i].ToString().Remove(10, 9)}");
                                    rdrCount++;
                                }
                                else
                                {
                                    Console.WriteLine($"{index} - {rdr[i].ToString().Remove(10, 9)}");
                                    rdrCount++;
                                }
                            }
                            else
                            {
                                if (index < 10)
                                {
                                    Console.Write($" {index} - {rdr[i]}\n");
                                    rdrCount++;
                                }
                                else
                                {
                                    Console.Write($"{index} - {rdr[i]}\n");
                                    rdrCount++;
                                }
                            }
                        }
                    }

                    else if (PrintType == 4)
                    {
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            rdrCount++;
                        }
                    }
                    index++;
                }
                Console.WriteLine();
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return rdrCount;
        }

        public static void Insert(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception();
            }
        }

        public static void Delete(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static bool Exist(string ObjectToVerify, MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            bool exist = false;
            DateTime mydate;
            char[] split = { '/', ' ', '-' };
            string[] rdrSplited;
            string[] dateSplited;

            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (DateTime.TryParse(rdr[0].ToString(), out mydate))
                {
                    rdrSplited = rdr[0].ToString().Split(split);
                    dateSplited = ObjectToVerify.ToString().Split(split);
                    if (rdrSplited[0] == dateSplited[1] && rdrSplited[1] == dateSplited[0] && rdrSplited[2] == dateSplited[2])
                    {
                        exist = true;
                        break;
                    }
                }
                else
                {
                    if (rdr[0].ToString() == ObjectToVerify)
                    {
                        exist = true;
                        break;
                    }
                }
            }
            rdr.Close();

            return exist;
        }

        public static List<string> Pick(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr)
        {
            List<string> list = new List<string>();
            DateTime mydate;

            try
            {
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        if (DateTime.TryParse(rdr[i].ToString(), out mydate))
                        {
                            list.Add(rdr[i].ToString().Remove(10, 9));
                        }
                        else
                        {
                            list.Add(rdr[i].ToString());
                        }
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return list;
        }

        /// <summary>
        /// Allow to print user informations (for student, teacher and admin)
        /// </summary>
        /// <param name="User"> Write 'admin' to don't have branch informations, else write nothing for teacher and student (to have branch) </param>
        /// <returns></returns>
        public static List<string> User_Informations(string User = "")
        {
            List<string> Informations = new List<string>();
            string answer = "no"; string stringDate;
            string sexe; string branch; string password; string password2; string mail;

            while (answer == "no")
            {
                Console.Write("\nFirst Name : ");
                Informations.Add(Console.ReadLine());

                Console.Write("Last Name : ");
                Informations.Add(Console.ReadLine());

                Console.Write("Birth Date (MM-DD-YYYY) : ");
                stringDate = Program.Ask_Date();
                Informations.Add(stringDate);

                Console.Write("Sexe (Male or Female) : ");
                sexe = Console.ReadLine();
                while (sexe != "Male" && sexe != "Female")
                {
                    Console.Write("Please write Male or Female : ");
                    sexe = Console.ReadLine();
                }
                Informations.Add(sexe);

                Console.Write("Phone Number : ");
                Informations.Add(Console.ReadLine());

                if (Informations[0].Contains(' ') || Informations[1].Contains(' '))
                    mail = $"{Informations[0].ToLower().Replace(' ', '-')}.{Informations[1].ToLower().Replace(' ', '-')}@vgc.ie";
                else
                    mail = $"{Informations[0].ToLower()}.{Informations[1].ToLower()}@vgc.ie";
                Informations.Add(mail);

                if (User != "admin")
                {
                    Console.Write("Branch : ");
                    branch = Console.ReadLine();
                    while (branch != "ESILV" && branch != "EMLV" && branch != "IIM")
                    {
                        Console.Write("This branch doesn't exist. Please select between ESILV, EMLV and IIM : ");
                        branch = Console.ReadLine();
                    }
                    Informations.Add(branch);
                }

                password = "*";
                password2 = "-";
                while (password != password2)
                {
                    Console.Write("Password : ");
                    password = Console.ReadLine();
                    Console.Write("Password (a second time to verify) : ");
                    password2 = Console.ReadLine();

                    if (password != password2)
                        Console.WriteLine("You must write correctly the second password, please write again.");
                }
                Informations.Add(password);

                Console.WriteLine();
                if (Informations.Count == 7)
                    Console.WriteLine($"You are {Informations[0]} {Informations[1]} ({Informations[3]})\nYour birth date is : {Informations[2]}\nYour phone number is : {Informations[4]}\nYour password is : {Informations[6]}");
                else
                    Console.WriteLine($"You are {Informations[0]} {Informations[1]} ({Informations[3]})\nYour birth date is : {Informations[2]}\nYour phone number is : {Informations[4]}\nYour school is : {Informations[6]}\nYour password is : {Informations[7]}");

                Console.WriteLine("\nDo these informations suit you ?\n- yes\n- no");
                answer = Console.ReadLine();
                while (answer != "yes" && answer != "no")
                {
                    Console.WriteLine("Please select a correct answer :");
                    answer = Console.ReadLine();
                }

                if (answer == "no")
                {
                    Console.WriteLine("\nPlease write again the informations about the teacher :");
                    Informations = new List<string>();
                }
            }

            return Informations;
        }

        /// <summary>
        /// Allow to ask a date with the correct form (MM-DD-YYYY)
        /// </summary>
        /// <returns></returns>
        public static string Ask_Date()
        {
            string stringDate; DateTime date;

            stringDate = Console.ReadLine();
            char[] split = { '-' };
            while (stringDate.Length < 9 || stringDate[2] != '-' || stringDate[5] != '-' || stringDate.Length != 10)
            {
                Console.WriteLine("Please separate by a '-'");
                stringDate = Console.ReadLine();
            }
            string[] dateSplited = stringDate.Split(split);
            while (Convert.ToInt32(dateSplited[1]) > 31 || Convert.ToInt32(dateSplited[1]) < 0 || Convert.ToInt32(dateSplited[0]) > 12 || Convert.ToInt32(dateSplited[0]) < 0 || Convert.ToInt32(dateSplited[2]) > 2021 || Convert.ToInt32(dateSplited[2]) < 1900)
            {
                Console.WriteLine("Please write the date as the correct form (MM-DD-YYYY)");
                stringDate = Console.ReadLine();
                while (stringDate[2] != '-' || stringDate[5] != '-' || stringDate.Length != 10)
                {
                    Console.WriteLine("Please separate by a '-'");
                    stringDate = Console.ReadLine();
                }
                dateSplited = stringDate.Split(split);
            }
            date = new DateTime(Convert.ToInt32(dateSplited[2]), Convert.ToInt32(dateSplited[0]), Convert.ToInt32(dateSplited[1]));

            return stringDate;
        }

        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }

        public class ConsoleSpiner
        {
            int counter;
            public ConsoleSpiner()
            {
                counter = 0;
            }
            public void Turn()
            {
                counter++;
                switch (counter % 4)
                {
                    case 0: Console.Write("/"); break;
                    case 1: Console.Write("-"); break;
                    case 2: Console.Write("\\"); break;
                    case 3: Console.Write("|"); break;
                }
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }


        public static void Main(string[] args)
        {
            Student student;
            Admin admin;
            Teacher teacher;
            Random random = new Random();

            string connStr = "server=cactus.vocaly-app.com;user=root;database=remi;port=3306;password=remi";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;

            bool pass; bool exist;
            string id = " ";
            
            string sql;

            Console.Write("ID : ");
            id = Console.ReadLine();
            sql = "SELECT Id From Students";
            cmd = new MySqlCommand(sql, conn);
            exist = Exist(id, conn, cmd, rdr);

            if (exist == false)
            {
                sql = "SELECT Id From Teachers";
                cmd = new MySqlCommand(sql, conn);
                exist = Exist(id, conn, cmd, rdr);
            }

            if (exist == false)
            {
                sql = "SELECT Id From Admins";
                cmd = new MySqlCommand(sql, conn);
                exist = Exist(id, conn, cmd, rdr);
            }

            while (exist == false)
            {
                Console.Write("This ID doesn't exist. Please enter an existing ID : ");
                id = Console.ReadLine();

                sql = "SELECT Id FROM Students";
                cmd = new MySqlCommand(sql, conn);
                exist = Program.Exist(id, conn, cmd, rdr);

                if (exist == false)
                {
                    sql = "SELECT Id From Teachers";
                    cmd = new MySqlCommand(sql, conn);
                    exist = Exist(id, conn, cmd, rdr);
                }

                if (exist == false)
                {
                    sql = "SELECT Id From Admins";
                    cmd = new MySqlCommand(sql, conn);
                    exist = Exist(id, conn, cmd, rdr);
                }
            }

            Console.Write("Password : ");
            var password = ReadPassword();
            sql = $"SELECT Password FROM Students WHERE Id = '{id}'";
            cmd = new MySqlCommand(sql, conn);
            pass = Exist(password, conn, cmd, rdr);

            if (pass == false)
            {
                sql = $"SELECT Password From Teachers WHERE Id = '{id}' ";
                cmd = new MySqlCommand(sql, conn);
                pass = Exist(password, conn, cmd, rdr);
            }

            if (pass == false)
            {
                sql = $"SELECT Password From Admins WHERE Id = '{id}' ";
                cmd = new MySqlCommand(sql, conn);
                pass = Exist(password, conn, cmd, rdr);
            }

            while (pass == false)
            {
                Console.Write("Wrong password. Please type again: ");
                password = ReadPassword();

                sql = $"SELECT Password FROM Students WHERE Id = '{id}'";
                cmd = new MySqlCommand(sql, conn);
                pass = Program.Exist(password, conn, cmd, rdr);

                if (pass == false)
                {
                    sql = $"SELECT Password From Teachers WHERE Id = '{id}'";
                    cmd = new MySqlCommand(sql, conn);
                    pass = Exist(password, conn, cmd, rdr);
                }

                if (pass == false)
                {
                    sql = $"SELECT Password From Admins WHERE Id = '{id}'";
                    cmd = new MySqlCommand(sql, conn);
                    pass = Exist(password, conn, cmd, rdr);
                }
            }

            Console.Write("Success. Datas loading ...");
            ConsoleSpiner spin = new ConsoleSpiner();
            for (int delay = 0; delay < 15000; delay++)
                spin.Turn();
            Console.WriteLine("\n");

            int i = Convert.ToInt32(id);
            if (pass == true)
            {
                if (i > 0 && i <= 99)
                {
                    using (var dbCtx = new VGC_SqlContext())
                    {
                        List<Admin> listing = dbCtx.Admins.ToList();
                        admin = listing[i-1];
                        Console.WriteLine($"{admin.ToString(conn, cmd, rdr)}\n\n");
                        string key = " ";

                        while (key != "Exit")
                        {
                            int choice = 0;
                            while (choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 5 && choice != 6 && choice != 7 && choice != 8 && choice != 9 && choice != 10 && choice != 11 && choice != 12 && choice != 13 && choice != 14)
                            {
                                Console.WriteLine("What do you want to do ?");
                                Console.WriteLine("1 - Create a subject, an exam or delete a course");
                                Console.WriteLine("2 - Print student information");
                                Console.WriteLine("3 - Print student attendance");
                                Console.WriteLine("4 - Print student fees history");
                                Console.WriteLine("5 - Add attendances for a student");
                                Console.WriteLine("6 - Add subject");
                                Console.WriteLine("7 - Add teacher");
                                Console.WriteLine("8 - Add subject teacher");
                                Console.WriteLine("9 - Modify student attendances");
                                Console.WriteLine("10 - Modify subject");
                                Console.WriteLine("11 - Delete student");
                                Console.WriteLine("12 - Delete teacher");
                                Console.WriteLine("13 - Modify your contact");
                                Console.WriteLine("14 - Fill the timetable at the start of the year");
                                choice = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine();
                            }

                            Console.Clear();
                            switch (choice)
                            {
                                //case 1:
                                //    admin.CreateExamOrCourse(conn, cmd, rdr);
                                //    break;
                                case 2:
                                    admin.Print_Student_Informations(conn, cmd, rdr);
                                    break;
                                case 3:
                                    admin.Print_Student_Attendances(conn, cmd, rdr);
                                    break;
                                case 4:
                                    admin.Print_Student_Fees_History(conn, cmd, rdr);
                                    break;
                                case 5:
                                    admin.Add_Student_Attendances(conn, cmd, rdr);
                                    break;
                                case 6:
                                    admin.Add_Subject(conn, cmd, rdr);
                                    break;
                                case 7:
                                    admin.Add_Teacher(conn, cmd, rdr);
                                    break;
                                case 8:
                                    admin.Add_Subject_Teacher(conn, cmd, rdr);
                                    break;
                                case 9:
                                    admin.Modify_Student_Attendances(conn, cmd, rdr);
                                    break;
                                case 10:
                                    admin.Modify_Subject(conn, cmd, rdr);
                                    break;
                                case 11:
                                    admin.Delete_Student(conn, cmd, rdr);
                                    break;
                                case 12:
                                    admin.Delete_Teacher(conn, cmd, rdr);
                                    break;
                                case 13:
                                    admin.ModifyContact();
                                    break;
                                //case 14:
                                //    admin.SubjectMandatory();
                                //    break;
                                default:
                                    return;
                            }
                            Console.WriteLine("If you want to stop, write : Exit\nElse, type Enter");
                            key = Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                else if (i >= 100 && i <= 999)
                {
                    using (var dbCtx = new VGC_SqlContext())
                    {
                        List<Teacher> listing = dbCtx.Teachers.ToList();
                        teacher = listing[i-100];
                        string key = " ";

                        while (key != "Exit")
                        {
                            Console.WriteLine($"{teacher.ToString(conn, cmd, rdr)}\n\n");

                            int choice = 0;
                            while (choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 5 && choice != 6 && choice != 7)
                            {
                                Console.WriteLine("What do you want to do ?");
                                Console.WriteLine("1 - Add assignment");
                                Console.WriteLine("2 - Add grades");
                                Console.WriteLine("3 - Print student informations");
                                Console.WriteLine("4 - Print student attendances");
                                Console.WriteLine("5 - Print grades notebook");
                                Console.WriteLine("6 - Modify student grade");
                                Console.WriteLine("7 - Modify your contact");
                                choice = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine();

                            }

                            Console.Clear();
                            switch (choice)
                            {
                                case 1:
                                    teacher.Add_Assignment(conn, cmd, rdr);
                                    break;
                                case 2:
                                    teacher.Add_Grades(conn, cmd, rdr);
                                    break;
                                case 3:
                                    teacher.Print_Student_Informations(conn, cmd, rdr);
                                    break;
                                case 4:
                                    teacher.Print_Student_Attendances(conn, cmd, rdr);
                                    break;
                                case 5:
                                    teacher.Print_Grades_Notebook(conn, cmd, rdr);
                                    break;
                                case 6:
                                    teacher.Modify_Student_Grade(conn, cmd, rdr);
                                    break;
                                case 7:
                                    teacher.ModifyContact();
                                    break;
                                default:
                                    return;
                            }
                            Console.WriteLine("If you want to stop, write : Exit\nElse, type Enter");
                            key = Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                else if (i > 1000)
                {
                    using (var dbCtx = new VGC_SqlContext())
                    {
                        List<Student> listing = dbCtx.Students.ToList();
                        student = listing[i-1001];
                        string key = " ";

                        while (key != "Exit")
                        {
                            Console.WriteLine($"{student.ToString(conn, cmd, rdr)}\n\n");

                            int choice = 0;
                            while (choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 5 && choice != 6 && choice != 7 && choice != 8 && choice != 9 && choice != 10)
                            {
                                Console.WriteLine("What do you want to do ?");
                                Console.WriteLine("1 - Course registration");
                                Console.WriteLine("2 - Add money");
                                Console.WriteLine("3 - Payment");
                                Console.WriteLine("4 - Print fees history");
                                Console.WriteLine("5 - Print attendances");
                                Console.WriteLine("6 - Print assignments");
                                Console.WriteLine("7 - Print grades");
                                Console.WriteLine("8 - Report card");
                                Console.WriteLine("9 - TimetableWeek");
                                Console.WriteLine("10 - Modify your contact\n");
                                choice = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine();

                            }

                            Console.Clear();
                            switch (choice)
                            {
                                case 1:
                                    student.Course_Registration(conn, cmd, rdr, random);
                                    break;
                                case 2:
                                    student.Add_Money(conn, cmd, rdr);
                                    break;
                                case 3:
                                    student.Payment(conn, cmd, rdr);
                                    break;
                                case 4:
                                    student.Print_Fees_History(conn, cmd, rdr);
                                    break;
                                case 5:
                                    student.Print_Attendances(conn, cmd, rdr);
                                    break;
                                case 6:
                                    student.Print_Assignments(conn, cmd, rdr);
                                    break;
                                case 7:
                                    student.Print_Grades(conn, cmd, rdr);
                                    break;
                                //case 8:
                                //    student.ReportCard();
                                //    break;
                                case 9:
                                    student.TimetableWeek();
                                    break;
                                case 10:
                                    student.ModifyContact();
                                    break;
                                default:
                                    return;
                            }
                            Console.WriteLine("If you want to stop, write : Exit\nElse, type Enter");
                            key = Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
            }
        }
    }
}

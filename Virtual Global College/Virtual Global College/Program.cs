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
using System.Timers;
using System.Threading;
using System.Windows;



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
                                Console.WriteLine($"{rdr[i].ToString()[3]}{rdr[i].ToString()[4]}-{rdr[i].ToString()[0]}{rdr[i].ToString()[1]}-{rdr[i].ToString()[6]}{rdr[i].ToString()[7]}{rdr[i].ToString()[8]}{rdr[i].ToString()[9]}");
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
                                Console.Write($" - {rdr[i].ToString()[3]}{rdr[i].ToString()[4]}-{rdr[i].ToString()[0]}{rdr[i].ToString()[1]}-{rdr[i].ToString()[6]}{rdr[i].ToString()[7]}{rdr[i].ToString()[8]}{rdr[i].ToString()[9]}");
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
                                    Console.WriteLine($" {index} - {rdr[i].ToString()[3]}{rdr[i].ToString()[4]}-{rdr[i].ToString()[0]}{rdr[i].ToString()[1]}-{rdr[i].ToString()[6]}{rdr[i].ToString()[7]}{rdr[i].ToString()[8]}{rdr[i].ToString()[9]}");
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

            int choice; string key;
            bool pass = false; bool exist;
            string id = " ";
            int j = 0;
            string sql;
            bool forgetPassword = false;

            while (forgetPassword == false)
            {
                Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\t\t\t\t\t\t\t\t\t\t\t\tID       : ");
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
                    Console.Clear();
                    Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\t\t\t\t\t\t\t\t\t\t\t\tThis ID doesn't exist. \n\n\t\t\t\t\t\t\t\t\t\t\t\tID       : ");
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

                Console.Clear();
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\t\t\t\t\t\t\t\t\t\t\t\tID       : " + id);
                Console.Write("\t\t\t\t\t\t\t\t\t\t\t\tPassword : ");
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

                forgetPassword = true;

                while (pass == false)
                {
                    j += 1;
                    Console.Clear();
                    Console.Write($"\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\t\t\t\t\t\t\t\t\t\t\t\tWrong password. \n\n\t\t\t\t\t\t\t\t\t\t\t\tID       : {id}\n\t\t\t\t\t\t\t\t\t\t\t\tPassword : ");
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

                    forgetPassword = true;

                    if (j > 1 && 0 < Convert.ToInt32(id) && Convert.ToInt32(id) < 100 && pass == false)
                    {
                        Console.Clear();
                        Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\t\t\t\t\t\t\t\t\t\t\t\tYou have to change your password. \n\n\t\t\t\t\t\t\t\t\t\t\t\tWrite a new one : ");
                        string passwordForget = Console.ReadLine();
                        sql = $"UPDATE Admins SET Password ='{passwordForget}' WHERE Id = @id";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@id", id);
                        Program.Insert(conn, cmd, rdr);
                        Console.Write("\n\n\t\t\t\t\t\t\t\t\t\t\t\tYour password has been changed.\n\t\t\t\t\t\t\t\t\t\t\t\tPress 'enter' to connect.");
                        forgetPassword = false;
                        pass = true;
                        Console.ReadKey();
                        Console.Clear();
                    }

                    if (j > 1 && 100 <= Convert.ToInt32(id) && Convert.ToInt32(id) < 1000 && pass == false)
                    {
                        Console.Clear();
                        Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\t\t\t\t\t\t\t\t\t\t\t\tYou have to change your password. \n\n\t\t\t\t\t\t\t\t\t\t\t\tWrite a new one : ");
                        string passwordForget = Console.ReadLine();
                        sql = $"UPDATE Teachers SET Password ='{passwordForget}' WHERE Id = @id";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@id", id);
                        Program.Insert(conn, cmd, rdr);
                        Console.Write("\n\n\t\t\t\t\t\t\t\t\t\t\t\tYour password has been changed.\n\t\t\t\t\t\t\t\t\t\t\t\tPress 'enter' to connect.");
                        forgetPassword = false;
                        pass = true;
                        Console.ReadKey();
                        Console.Clear();
                    }

                    if (j > 1 && Convert.ToInt32(id) > 1000 && pass == false)
                    {
                        Console.Clear();
                        Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\t\t\t\t\t\t\t\t\t\t\t\tYou have to change your password. \n\n\t\t\t\t\t\t\t\t\t\t\t\tWrite a new one : ");
                        string passwordForget = Console.ReadLine();
                        sql = $"UPDATE Admins SET Password ='{passwordForget}' WHERE Id = @id";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@id", id);
                        Program.Insert(conn, cmd, rdr);
                        Console.Write("\n\n\t\t\t\t\t\t\t\t\t\t\t\tYour password has been changed.\n\t\t\t\t\t\t\t\t\t\t\t\tPress 'enter' to connect.");
                        forgetPassword = false;
                        pass = true;
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
            
            

            Console.WriteLine("\n\n\t\t\t\t\t\t\t\t\t\t\t\tSuccess");
            ConsoleSpiner spin = new ConsoleSpiner();
            Console.Write("\n\n\t\t\t\t\t\t\t\t\t\t\t\tLoading....");
            for (int delay = 0; delay < 15000; delay++)
                spin.Turn();

            int i = Convert.ToInt32(id);
            if (pass == true)
            {

                if (i > 0 && i <= 99)
                {
                    using (var dbCtx = new VGC_SqlContext())
                    {
                        List<Admin> listing = dbCtx.Admins.ToList();
                        admin = listing[i-1];
                        key = " ";

                        while (key != "Exit")
                        {
                            choice = 0;
                            while (!Enumerable.Range(1,13).Contains(choice))
                            {
                                Console.Clear();
                                Console.WriteLine(admin.ToString(conn, cmd, rdr));

                                Console.WriteLine("\n\nWhat do you want to do ?\n");
                                Console.WriteLine(" 1 - Print student information");
                                Console.WriteLine(" 2 - Print student attendance (TO VERIFY)");
                                Console.WriteLine(" 3 - Print student fees history\n");
                                Console.WriteLine(" 4 - Add lesson/exam (TO DO)");
                                Console.WriteLine(" 5 - Add attendance");
                                Console.WriteLine(" 6 - Add subject");
                                Console.WriteLine(" 7 - Add teacher");
                                Console.WriteLine(" 8 - Add subject teacher\n");
                                Console.WriteLine(" 9 - Modify student attendances");
                                Console.WriteLine("10 - Delete lesson (TO DO)");
                                Console.WriteLine("11 - Delete student");
                                Console.WriteLine("12 - Delete teacher");
                                Console.WriteLine("13 - Modify your contact\n");
                                choice = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine();
                            }

                            Console.Clear();
                            switch (choice)
                            {
                                case 1:
                                    admin.Print_Student_Informations(conn, cmd, rdr);
                                    break;
                                case 2:
                                    admin.Print_Student_Attendances(conn, cmd, rdr);
                                    break;
                                case 3:
                                    admin.Print_Student_Fees_History(conn, cmd, rdr);
                                    break;
                                case 4:
                                    admin.Add_Lesson();
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
                                //case 10:
                                //    admin.Delete_Lesson(conn, cmd, rdr);
                                //    break;
                                case 11:
                                    admin.Delete_Student(conn, cmd, rdr);
                                    break;
                                case 12:
                                    admin.Delete_Teacher(conn, cmd, rdr);
                                    break;
                                case 13:
                                    admin.ModifyContact(conn, cmd, rdr, "admin");
                                    break;
                                default:
                                    return;
                            }
                            Console.WriteLine("\nIf you want to stop. Type : Exit");
                            key = Console.ReadLine();
                            dbCtx.SaveChanges();
                        }
                    }
                }
                else if (i >= 100 && i <= 999)
                {
                    using (var dbCtx = new VGC_SqlContext())
                    {
                        List<Teacher> listing = dbCtx.Teachers.ToList();
                        teacher = listing[i-100];
                        key = " ";

                        while (key != "Exit")
                        {
                            choice = 0;
                            while (!Enumerable.Range(1, 6).Contains(choice))
                            {
                                Console.Clear();
                                Console.WriteLine(teacher.ToString(conn, cmd, rdr));

                                Console.WriteLine("\n\nWhat do you want to do ?\n");
                                Console.WriteLine(" 1 - Add assignment");
                                Console.WriteLine(" 2 - Add grades\n");
                                Console.WriteLine(" 3 - Print student informations");
                                Console.WriteLine(" 4 - Print student attendances");
                                Console.WriteLine(" 5 - Print grades notebook\n");
                                Console.WriteLine(" 6 - Modify your contact\n");
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
                                    teacher.ModifyContact(conn, cmd, rdr, "teacher");
                                    break;
                                default:
                                    return;
                            }
                            Console.WriteLine("\nIf you want to stop. Type : Exit");
                            key = Console.ReadLine();
                            dbCtx.SaveChanges();
                        }
                    }
                }
                else if (i > 1000)
                {
                    using (var dbCtx = new VGC_SqlContext())
                    {
                        List<Student> listing = dbCtx.Students.ToList();
                        student = listing[i-1001];
                        key = " ";

                        while (key != "Exit")
                        {
                            choice = 0;
                            while (!Enumerable.Range(1, 9).Contains(choice))
                            {
                                Console.Clear();
                                Console.WriteLine(student.ToString(conn, cmd, rdr));

                                Console.WriteLine("\n\nWhat do you want to do ?\n");
                                Console.WriteLine(" 1 - Course registration");
                                Console.WriteLine(" 2 - Add money");
                                Console.WriteLine(" 3 - Payment\n");
                                Console.WriteLine(" 4 - Print fees history");
                                Console.WriteLine(" 5 - Print attendances");
                                Console.WriteLine(" 6 - Print assignments");
                                Console.WriteLine(" 7 - Print grades");
                                Console.WriteLine(" 8 - Print timetable");
                                Console.WriteLine(" 9 - Modify your contact\n");
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
                                case 8:
                                    student.Print_Timetable(conn, cmd, rdr);
                                    break;
                                case 9:
                                    student.ModifyContact(conn, cmd, rdr, "student");
                                    break;
                                default:
                                    return;
                            }
                            
                            Console.WriteLine("\nIf you want to stop. Type : Exit");
                            key = Console.ReadLine();
                            dbCtx.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}

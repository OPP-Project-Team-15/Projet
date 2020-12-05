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



            using (var dbCtx = new VGC_SqlContext())
            {
                List<Admin> listing = dbCtx.Admins.ToList();
                admin = listing[0];
                admin.Modify_Student_Attendances(conn, cmd, rdr);
                dbCtx.SaveChanges();
            }



            Console.ReadKey();
        }
    }
}

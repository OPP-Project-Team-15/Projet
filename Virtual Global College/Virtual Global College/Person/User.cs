﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;




namespace Virtual_Global_College
{
    public abstract class User
    {
        #region : Properties

        [Key]
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string Sexe { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        #endregion



        #region : Methods

        public void ModifyContact(MySqlConnection conn, MySqlCommand cmd, MySqlDataReader rdr, string user)
        {
            Console.WriteLine("######## MODIFY CONTACT ########\n");

            bool Modify = true; string sql;

            while (Modify)
            {
                if (user == "student")
                {
                    Console.WriteLine("What do you want to change ? Please write your response as it's written :\n- phone number \n- password");
                    string ModifyInfo = Console.ReadLine();
                    switch (ModifyInfo)
                    {
                        case "phone number":
                            Console.WriteLine("\nWhat is your new phone number ?");
                            PhoneNumber = Console.ReadLine();
                            sql = $"UPDATE Students SET PhoneNumber ='{PhoneNumber}' WHERE Id = @id";
                            cmd = new MySqlCommand(sql, conn);
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@id", Id);
                            Program.Insert(conn, cmd, rdr);
                            Console.WriteLine("\nYour phone number has been changed");
                            break;
                        case "password":
                            Console.WriteLine("\nWhat is your new password ?");
                            Password = Console.ReadLine();
                            sql = $"UPDATE Students SET Password ='{Password}' WHERE Id = @id";
                            cmd = new MySqlCommand(sql, conn);
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@id", Id);
                            Program.Insert(conn, cmd, rdr);
                            Console.WriteLine("\nYour password has been changed");
                            break;
                    }
                }

                if (user == "admin")
                {
                    Console.WriteLine("What do you want to change ? Please write your response as it's written\n- phone number \n- password");
                    string ModifyInfo = Console.ReadLine();
                    switch (ModifyInfo)
                    {
                        case "phone number":
                            Console.WriteLine("\nWhat is your new phone number ?");
                            PhoneNumber = Console.ReadLine();
                            sql = $"UPDATE Admins SET PhoneNumber ='{PhoneNumber}' WHERE Id = @id";
                            cmd = new MySqlCommand(sql, conn);
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@id", Id);
                            Program.Insert(conn, cmd, rdr);
                            Console.WriteLine("\nYour phone number has been changed");
                            break;
                        case "password":
                            Console.WriteLine("\nWhat is your new password ?");
                            Password = Console.ReadLine();
                            sql = $"UPDATE Admins SET Password ='{Password}' WHERE Id = @id";
                            cmd = new MySqlCommand(sql, conn);
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@id", Id);
                            Program.Insert(conn, cmd, rdr);
                            Console.WriteLine("\nYour password has been changed");
                            break;
                    }
                }

                if (user == "teacher")
                {
                    Console.WriteLine("What do you want to change ? Please write your response as it's written\n- phone number \n- password");
                    string ModifyInfo = Console.ReadLine();
                    switch (ModifyInfo)
                    {
                        case "phone number":
                            Console.WriteLine("\nWhat is your new phone number ?");
                            PhoneNumber = Console.ReadLine();
                            sql = $"UPDATE Teachers SET PhoneNumber ='{PhoneNumber}' WHERE Id = @id";
                            cmd = new MySqlCommand(sql, conn);
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@id", Id);
                            Program.Insert(conn, cmd, rdr);
                            Console.WriteLine("\nYour phone number has been changed");
                            break;
                        case "password":
                            Console.WriteLine("\nWhat is your new password ?");
                            Password = Console.ReadLine();
                            sql = $"UPDATE Teachers SET Password ='{Password}' WHERE Id = @id";
                            cmd = new MySqlCommand(sql, conn);
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@id", Id);
                            Program.Insert(conn, cmd, rdr);
                            Console.WriteLine("\nYour password has been changed");
                            break;
                    }
                }

                Console.WriteLine("Do you want to change something else ? Please write your response as it's written\n- yes \n- no");

                string response = Console.ReadLine();
                while (response != "yes" && response != "no")
                {
                    Console.WriteLine("\nPlease write as it's written : \n- yes \n- no");
                    response = Console.ReadLine();
                }

                if (response == "no")
                {
                    Modify = false;
                }
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public abstract class User
    {
        public string Name { get; }
        public string Surname { get; }
        public string Id { get; }
        public string PhoneNumber { get; set; }
        public string Sexe { get; }
        public string Mail { get; set; }
        public string Password { get; set; }        

        public User (string name, string surname, string id, string phoneNumber, string sexe, string mail, string password)
        {
            Name = name;
            Surname = surname;
            Id = id;
            PhoneNumber = phoneNumber;
            Sexe = sexe;
            Mail = mail;
            Password = password;
        }

        public override string ToString() => $"Profile :\n\n{Name} {Surname}\nId : {Id}/nPhone number : {PhoneNumber}\nSexe : {Sexe}\nMail : {Mail}\nPassword : {Password}";
        

        public bool VerifyLogin(string passwordToVerify)
        {
            bool passwordVerified = false;
            
            if (passwordToVerify == Password)
                passwordVerified = true;
            
            return passwordVerified;
        }

        public void ModifyContact()
        {
            bool Modify = true;

            while (Modify)
            {
                Console.WriteLine("What do you want to change ? Please write your response as it's written\n- Phone number \n- Mail \n- Password");
                string ModifyInfo = Console.ReadLine();
                switch (ModifyInfo)
                {
                    case "Phone number":
                        Console.WriteLine("\nWhat is your new phone number ?");
                        PhoneNumber = Console.ReadLine();
                        Console.WriteLine("\nYour phone number has been changed");
                        break;

                    case "Mail":
                        Console.WriteLine("\nWhat is your new mail ?");
                        Mail = Console.ReadLine();
                        Console.WriteLine("\nYour mail has been changed");
                        break;

                    case "Password":
                        Console.WriteLine("\nWhat is your new password ?");
                        Password = Console.ReadLine();
                        Console.WriteLine("\nYour password has been changed");
                        break;
                }

                Console.WriteLine("Do you want to change something else ? Please write your response as it's written\n- Yes \n- No");

                string response = Console.ReadLine();
                while (response != "Yes" && response != "No")
                {
                    Console.WriteLine("\nPlease write as it's written : \n- Yes \n- No");
                    response = Console.ReadLine();
                }

                if (response == "No")
                {
                    Modify = false;
                }
            }
        }
    }
}

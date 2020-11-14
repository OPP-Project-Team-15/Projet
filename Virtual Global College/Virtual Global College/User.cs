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
        public string PhoneNumber { get; }
        public string Sexe { get; }
        public string Mail { get; }
        public string Password { get; }

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

        public override string ToString() => $"Profile :/n/n{Name} {Surname}/nId : {Id}/nPhone number : {PhoneNumber}/nSexe : {Sexe}/nMail : {Mail}/nPassword : {Password}";
        

        public bool VerifyLogin(string passwordToVerify)
        {
            bool passwordVerified = false;
            
            if (passwordToVerify == Password)
                passwordVerified = true;
            
            return passwordVerified;
        }
    }
}

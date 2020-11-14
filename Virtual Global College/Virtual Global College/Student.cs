using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    interface ISchool
    {
        string SchoolName {get;}
        string SchoolInfo();
    }


    public class Student : User
    {
        private string branch;
        private string[,] timetable;
        private List<string> feesHistory = new List<string>();
        private bool paymentIsOk = false;
        private List<string> 

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
        public string[,] Timetable
        {
            get { return timetable; }
            set
            {
                for (int i = 0; i < value.GetLength(0); i++)
                    for (int j = 0; j < value.GetLength(1); j++)
                    {
                        if (value[i, j] == null)
                            throw new Exception("The time slot [" + i + "," + j + "] isn't valid");
                    }
                timetable = value;
            }
        }
        public string[] CoursesPicked { get; set; } //Courses that student has chosen
        public double Money { get; set; } //Money that student have in the school account
        public List<string> FeesHistory
        {
            get { return feesHistory; }
            set { feesHistory = value; }
        } //History of actions relevant to the fees
        public string ProcessPayment { get; set; } //Payment in once or in thrice
        public int TimesNumberOfPayment { get; set; } //Number of times student paid for a payment in several times
        public bool PaymentIsOk
        {
            get { return paymentIsOk; }
            set { paymentIsOk = value; }
        } //True if student has paid all the fees for the year
        

        public Student(string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, string branch, string[,] timetable)
            : base(name, surname, id, phoneNumber, sexe, mail, password)
        {
            Branch = branch;
            Timetable = timetable;
        }

        public override string ToString() => $"{base.ToString()}\n\nType : Student\n Branch : {branch}";
        
        /// <summary>
        /// Allow the student the chooose his courses for the year
        /// <summary>
        public void Course_Registration(List<string> courses)
        {
            string[] picks = new string[2];

            Console.WriteLine("Which courses would you want to pick ?\n");
            int compt = 1;
            foreach (string element in courses)
            {
                Console.WriteLine($"{compt} - {element}");
                compt++;
            }
            Console.WriteLine();

            string answer;
            answer = Console.ReadLine();


            for (int i = 0; i < 2; i++)
            {
                while (courses.Contains(answer) != true)
                {
                    Console.WriteLine("This course doesn't exist. Please select another one\n");
                    answer = Console.ReadLine();
                }
                Console.WriteLine($"The course {answer} has been taken\n");
                picks[i] = answer;

                if (i < 2)
                    answer = Console.ReadLine();
            }

            Console.Write($"Thank you for your participation");
            this.CoursesPicked = picks;
        }

        /// <summary>
        /// Allow the student to add money in his school account
        /// <summary>
        public void Add_Money()
        {
            Console.WriteLine("How much money do you want to add?");
            double moneyAdded = Convert.ToDouble(Console.ReadLine());
            this.Money += moneyAdded;
            FeesHistory.Add($"Add {moneyAdded} euros");
        }

        /// <summary>
        /// Allow the student to choose a process payment for the first use of the method
        /// And allow the student to pay with the method he has chosen
        /// <summary>
        public void Payment()
        {
            if (PaymentIsOk == true)
                Console.WriteLine("The payment for the year is done");
            else
            {
                if (TimesNumberOfPayment == 0)
                {
                    Console.WriteLine("\nWhich payment process would you take ? Please write your response as it's written\n- once\n- thrice");
                    string process = Console.ReadLine();
                    while (process != "once" && process != "thrice")
                    {
                        Console.WriteLine("\nThis process doesn't exist, please write as it's written");
                        process = Console.ReadLine();
                    }

                    if (process == "once")
                    {
                        ProcessPayment = "once";
                        FeesHistory.Add("Selected payment by once");
                    }
                    else
                    {
                        ProcessPayment = "thrice";
                        FeesHistory.Add("Selected payment by thrice");
                    }
                }

                bool possiblePayment = true;
                if (ProcessPayment == "once")
                {
                    if (this.Money <= 9000)
                        possiblePayment = false;
                    else
                    {
                        this.Money -= 9000;
                        FeesHistory.Add($"Spend 9000 euros for payment in once");
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
                        FeesHistory.Add($"Spend 3000 euros for payment in thrice");
                        TimesNumberOfPayment++;
                        if (TimesNumberOfPayment == 3)
                            PaymentIsOk = true;
                    }
                }

                if (possiblePayment == true)
                    Console.WriteLine($"\nThe payment has been done\nYou have {this.Money} euros left");
                else
                    Console.WriteLine($"\nThe payment failed because you don't have enough money (You have {this.Money} euros)");
            }
        }

        /// <summary>
        /// Print the content of the list "FeesHistory"
        /// <summary>
        public void Print_Payment_History()
        {
            foreach (string element in FeesHistory)
            {
                Console.WriteLine(element);
            }
        }

        public void ModifyContact()
        {
            bool Modify = true;
            Console.WriteLine("What do you want to change ? Please write your response as it's written\n- Phone number \n- Mail \n- Password");
            string ModifyInfo = Console.ReadLine();

            while (Modify)
            {
                switch (ModifyInfo)
                {
                    case "Phone number":
                        Console.WriteLine("What is your new phone number ?");
                        PhoneNumber = Console.ReadLine();
                        Console.WriteLine("Your phone number has been changed");
                        break;

                    case "Mail":
                        Console.WriteLine("What is your new mail ?");
                        Mail = Console.ReadLine();
                        Console.WriteLine("Your mail has been changed");
                        break;

                    case "Password":
                        Console.WriteLine("What is your new password ?");
                        Password = Console.ReadLine();
                        Console.WriteLine("Your password has been changed");
                        break;

                }

                Console.WriteLine("Do you want to change something else ? Please write your response as it's written\n- Yes \n- No");

                while (Console.ReadLine() != "Yes" && Console.ReadLine() != "No")
                {
                    Console.WriteLine("\nPlease write as it's written : \n- Yes \n- No");
                }

                if (Console.ReadLine() == "No")
                {
                    Modify = false;
                }
            }
           
        }
    }
}

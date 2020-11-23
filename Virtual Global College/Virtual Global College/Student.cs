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
        string SchoolName { get; }
        string SchoolInfo();
    }



    public class Student : User
    {
        private string branch;
        public LinkedList<string[,]> timetablePerWeek = new LinkedList<string[,]>();
        private bool paymentIsOk = false;
        //private List<string> 

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
        public string[] CoursesPicked { get; set; } //Courses that student has chosen
        public double Money { get; set; } //Money that student have in the school account
        public static List<string> FeesHistory { get; set; } //History of actions relevant to the fees
        public string ProcessPayment { get; set; } //Payment in once or in thrice
        public int TimesNumberOfPayment { get; set; } //Number of times student paid for a payment in several times
        public List<string> Attendance { get; set; } //List of the abscence with the date of the student
        public bool PaymentIsOk
        {
            get { return paymentIsOk; }
            set { paymentIsOk = value; }
        } //True if student has paid all the fees for the year
        public List<string> Assignment { get; set; }




        public Student(string name, string surname, string id, string phoneNumber, string sexe, string mail, string password, string branch, string[,] timetable)
            : base(name, surname, id, phoneNumber, sexe, mail, password)
        {
            Branch = branch;
            Timetable = timetable;
            Timetable[0, 0] = "Hours";
            Timetable[0, 1] = "Monday";
            Timetable[0, 2] = "Tuesday";
            Timetable[0, 3] = "Wednesday";
            Timetable[0, 4] = "Thursday";
            Timetable[0, 5] = "Friday";
            Timetable[0, 6] = "Saturday";
            Timetable[0, 7] = "Sunday";
            for (int index1 = 8, index2 = 1; index1 <= 22; index1++, index2++)
            {
                timetable[index2, 0] = Convert.ToString(index1 - 1) + " - " + Convert.ToString(index1);
            }
            for (int index = 1; index <= 30; index++)
            {
                string[,] timetablePerW = new string[16,9];
                for (int index1 = 0; index1 < Timetable.GetLength(0); index1++)
                {
                    for (int index2 = 0; index2 < Timetable.GetLength(1); index2++)
                    {
                        timetablePerW[index1, index2] = Timetable[index1, index2];
                    }
                }
                timetablePerW[0, 8] = "Week " + Convert.ToString(index);
                timetablePerWeek.AddLast(timetablePerW);
            }
        }

        public override string ToString() => $"{base.ToString()}\n\nType : Student\n Branch : {branch}";

        /// <summary>
        /// Allow the student to choose his courses for the year
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
                {
                    Console.WriteLine("Which courses would you want to pick ?\n");
                    answer = Console.ReadLine();
                }
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

                Console.WriteLine("\nDo you want to pay ? Please write your response as it's written\n- yes\n- no");
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

                else
                    Console.WriteLine("Thank you for your process choice");

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

        /// <summary>
        /// Show the abscense of the student
        /// </summary>
        public void ToStringShowTheAttendance()
        {
            if (Attendance.Count != 0)
            {
                Console.WriteLine("The abscense of the student :");
                foreach (string element in Attendance)
                {
                    Console.WriteLine(element);
                }
            }
            else
            {
                Console.WriteLine("The student have no abscense.");
            }
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
                    Console.Write(tmtable[index1, index2] + "\t           ");
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

        /// <summary>
        /// We create the report card of the student
        /// </summary>
        public void ReportCard()
        {
            List<string[]> gradeOfTheStudent = new List<string[]>();
            string[] start = new string[1] { "Report card of " + Name + Surname + " :" };
            gradeOfTheStudent.Add(start);
            string[] start2 = new string[5] { "Name Assignment", "NameSubject", "Date", "Hours", "Grade" };
            gradeOfTheStudent.Add(start2);
            foreach (KeyValuePair<Subject, string[,]> grade in Grade)
            {
                for(int index = 0; index < grade.Value.GetLength(0); index++)
                {
                    if (grade.Value[index,1] == Id)
                    {
                        string[] mark = new string[5] { grade.Value[0, 0], grade.Value[1, 0], grade.Value[2, 0], grade.Value[3,0], grade.Value[index,2]};
                        gradeOfTheStudent.Add(mark);
                    }
                }
            }
            ToStringGradeStudent(gradeOfTheStudent);
        }

        /// <summary>
        /// Show the report card for a student
        /// </summary>
        public void ToStringGradeStudent(List<string[]> grade)
        {
            foreach (string[] exam in grade)
            {
                for (int index1 = 0; index1 < exam.Length; index1++)
                {
                    Console.Write(exam[index1] + "\t\t\t");
                }
                Console.WriteLine();
            }
        }
    }        
}

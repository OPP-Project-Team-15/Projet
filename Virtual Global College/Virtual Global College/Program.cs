using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Virtual_Global_College
{
    public class Program 
    {
        /// <summary>
        /// Add a student in the student lists of his courses
        /// <summary>
        public static void Student_Courses_Attribution(Student student, SortedList<string, List<Student>> Student_Courses)
        {
            int index;
            foreach (string element in student.CoursesPicked)
            {
                index = Student_Courses.IndexOfKey(element); // obtain the index of the course in the Sorted List
                Student_Courses.Values.ElementAt(index).Add(student); // add the student in the right course
            }
        }
        
        public static void Insert(SqlConnection conn, SqlCommand cmd)
        {
            Console.WriteLine("Please enter your informations \n 1 - Last name :");
            string lastName = Console.ReadLine();
            
            Console.WriteLine("\n 2 - First name :");
            string firstName = Console.ReadLine();
            
            Console.WriteLine("\n 3 - Date of birth (MM-DD-YYYY) :");
            string stringBirthDate = Console.ReadLine();
            char[] split = { '-' };
            string[] birthDateSplited = stringBirthDate.Split(split);
            DateTime birthDate = new DateTime(Convert.ToInt32(birthDateSplited[2]), Convert.ToInt32(birthDateSplited[0]), Convert.ToInt32(birthDateSplited[1]));
            
            Console.WriteLine("\n 4 - Sexe (write 'Male' or 'Female') :");
            string sexe = Console.ReadLine();
            while (sexe != "Male" && sexe != "Female")
            {
                Console.WriteLine("Please write 'Male' or 'Female'");
                sexe = sexe = Console.ReadLine();
            }
            
            Console.WriteLine("\n 5 - Phone number :");
            string phoneNumber = Console.ReadLine();
            
            Console.WriteLine("\n 6 - Mail :");
            string mail = Console.ReadLine();
            while (mail.Contains('@') == false)
            {
                Console.WriteLine("This adress doesn't countains '@'. Please write a correct mail adress");
                mail = Console.ReadLine();
            }
            
            Console.WriteLine("\n 7 - Your new password :");
            string password = Console.ReadLine();
            while (password == password.ToLower() || !password.Any(char.IsLetter) || !password.Any(char.IsDigit))
            {
                Console.WriteLine("The password must contain : a capital letter, a lower letter and a number");
                password = Console.ReadLine();
            }
            
            Console.WriteLine("\n 8 - Branch (write 'ESILV', 'EMLV' or 'IIM') :");
            string branch = Console.ReadLine();


            try
            {
                string sql = $"INSERT INTO Students (LastName, FirstName, BirthDate, Sexe, PhoneNumber, Mail, Password, Branch) VALUES ('{lastName}', '{firstName}', '{birthDate}', '{sexe}', '{phoneNumber}', '{mail}', '{password}', '{branch}')";
                conn.Open();
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            Console.WriteLine("You have been added to our data.");
        } 


        public static void Main(string[] args)
        {
            string mot = "OK1";
            Console.WriteLine(mot == mot.ToLower());
            Console.ReadKey();
            Console.WriteLine("\n 7 - Your new password :");
            string password = Console.ReadLine();
            while (password == password.ToLower() || password.All(char.IsLetter) || password.All(char.IsDigit))
            {
                Console.WriteLine("The password must contain : a capital letter, a lower letter and a number");
                password = Console.ReadLine();
            }
            Console.ReadKey();

            // LINK TO SQL
            string connectionString = @"Data Source=DESKTOP-GHLL41C\SQLEXPRESS;Initial Catalog=Virtual Global College;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            //SqlDataReader dataReader = null;

            Insert(conn, cmd);

            Console.ReadKey();
            // END OF LINK TO SQL



            string[,] timetable = new string[16,9];
            Student student1 = new Student("Jake", "Smith", "1234", "0601020304", "male", "jakesmith@gmail.com", "jsmith", "ESILV", timetable);
            Student student2 = new Student("Jake", "Smith", "1234", "0601020304", "male", "jakesmith@gmail.com", "jsmith", "ESILV", timetable);
            
            List<string> courses_name = new List<string>();
            courses_name.Add("Fluids Mechanics");
            courses_name.Add("Statistical Inference");
            courses_name.Add("OOP");
            courses_name.Add("Numerical Analysis");
            courses_name.Add("Data Structure");

            //student1.ToStringTimetable();
            SortedList<string, List<Student>> Student_Courses = new SortedList<string, List<Student>>();

            List<Student> Student_FluidsMechanics = new List<Student>();
            List<Student> Student_StatisticalInference = new List<Student>();
            List<Student> Student_OOP = new List<Student>();
            List<Student> Student_NumericalAnalysis = new List<Student>();
            List<Student> Student_DataStructure = new List<Student>();

            Student_Courses.Add("Fluids Mechanics", Student_FluidsMechanics);
            Student_Courses.Add("Statistical Inference", Student_StatisticalInference);
            Student_Courses.Add("OOP", Student_OOP);
            Student_Courses.Add("Numerical Analysis", Student_NumericalAnalysis);
            Student_Courses.Add("Data Structure", Student_DataStructure);

            /*
            student1.Course_Registration(courses_name);
            Student_Courses_Attribution(student1, Student_Courses);
            Console.Write($"\n\n Students in OOP : ");
            foreach (Student student in Student_OOP)
                Console.Write($"{student.Name}");
            Console.Write($"\n Students in Fluids Mechanics : ");
            foreach (Student student in Student_FluidsMechanics)
                Console.Write($"{student.Name}");
            Console.Write($"\n Students in Numerical Analysis : ");
            foreach (Student student in Student_NumericalAnalysis)
                Console.Write($"{student.Name}");*/
            
            student1.Add_Money();
            student1.Payment();

            Console.WriteLine("\n");
            student1.Print_Payment_History();
            Console.WriteLine($"\nThe payment for the year is done : {student1.PaymentIsOk}");

            
            

            Console.ReadKey();
            
        }
    }
}

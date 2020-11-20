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
            Console.WriteLine("Veuillez rentrer vos informations \n 1 - Nom ");
            string nom = Console.ReadLine();
            Console.WriteLine("\n 2 - Prenom ");
            string prenom = Console.ReadLine();
            Console.WriteLine("\n 3 - Date de naissance (YYYY-MM-DD) ");
            string dateNaissance = Console.ReadLine();
            Console.WriteLine("\n 4 - Ville ");
            string ville = Console.ReadLine();
            Console.WriteLine("\n 5 - idSubject ");
            int idSubject = Convert.ToInt32(Console.ReadLine());

            try
            {
                string sql = ($"INSERT INTO Students (FirstName, LastName, DateNaissance, Ville, IdSubject) VALUES ('{prenom}', '{nom}', '{dateNaissance}', '{ville}', '{idSubject}')");
                //Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
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
            // LINK TO SQL
            string connectionString = @"Data Source=DESKTOP-GHLL41C\SQLEXPRESS;Initial Catalog=Virtual Global College;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            
            string sql = "SELECT Password FROM Students";
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                Console.WriteLine(dataReader[0]);
            }

            dataReader.Close();
            command.Dispose();
            cnn.Close();

            //Console.ReadKey();
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

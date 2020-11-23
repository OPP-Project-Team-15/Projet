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
        public static void Student_Courses_Attribution(Student student, SortedList<Subject, List<Student>> Student_Courses)
        {
            int index = 0;
            bool subjectOk = false;
            foreach (string element in student.CoursesPicked)
            {
                foreach(KeyValuePair<Subject, List<Student>> list in Student_Courses)
                {
                    if (subjectOk == false)
                    {
                        subjectOk = list.Key.NameSubject.Contains(element);
                        index++; // obtain the index of the course in the Sorted List
                    }
                }
                Student_Courses.Values.ElementAt(index).Add(student); // add the student in the right course
            }
        }

        /// <summary>
        /// Add a student in the student data base using Alexander's SQL server
        /// <summary>
        public static void Insert(SqlConnection conn, SqlCommand cmd)
        {
            Console.WriteLine("Please enter your informations \n 1 - Last name :");
            string lastName = Console.ReadLine();
            
            Console.WriteLine("\n 2 - First name :");
            string firstName = Console.ReadLine();
            
            Console.WriteLine("\n 3 - Date of birth (MM-DD-YYYY) :");
            string stringBirthDate = Console.ReadLine();
            char[] split = { '-' };
            while (stringBirthDate[2] != '-' || stringBirthDate[5] != '-' || stringBirthDate.Length != 10)
            {
                Console.WriteLine("Please separate by a '-'");
                stringBirthDate = Console.ReadLine();
            }
            string[] birthDateSplited = stringBirthDate.Split(split);
            while (Convert.ToInt32(birthDateSplited[1]) > 31 || Convert.ToInt32(birthDateSplited[1]) < 0 || Convert.ToInt32(birthDateSplited[0]) > 12 || Convert.ToInt32(birthDateSplited[0]) < 0 || Convert.ToInt32(birthDateSplited[2]) > 2021 || Convert.ToInt32(birthDateSplited[2]) < 1900)
            {
                Console.WriteLine("Please write your date of birth as the correct form (MM-DD-YYYY)");
                stringBirthDate = Console.ReadLine();
                while (stringBirthDate[2] != '-' || stringBirthDate[5] != '-' || stringBirthDate.Length != 10)
                {
                    Console.WriteLine("Please separate by a '-'");
                    stringBirthDate = Console.ReadLine();
                }
                birthDateSplited = stringBirthDate.Split(split);
            }
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
            while (password.All(char.IsLetter) || password.All(char.IsDigit) || !password.Any(char.IsLower) || !password.Any(char.IsUpper))
            {
                Console.WriteLine("The password must contain : a capital letter, a lower letter and a number");
                password = Console.ReadLine();
            }

            Console.WriteLine("\n 8 - Branch (write 'ESILV', 'EMLV' or 'IIM') :");
            string branch = Console.ReadLine();
            while (branch != "ESILV" && branch != "EMLV" && branch != "IIM")
            {
                Console.WriteLine("Please write as it's written");
                branch = Console.ReadLine();
            }


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

        public static void Read(string sql, SqlConnection conn, SqlCommand cmd, SqlDataReader dataReader)
        {
            try
            {
                conn.Open();

                cmd = new SqlCommand(sql, conn);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Console.WriteLine(dataReader[0] + " " + dataReader[1] + " étudie la matière " + dataReader[2] + "\n");
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            Console.WriteLine("Done.");
        }


        public static void Main(string[] args)
        {
            //// --------------- LINK TO SQL ---------------

            //string connectionString = @"Data Source=DESKTOP-GHLL41C\SQLEXPRESS;Initial Catalog=Virtual Global College;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //SqlConnection conn = new SqlConnection(connectionString);
            //SqlCommand cmd = null;
            //SqlDataReader dataReader = null;
            //string sql = "SELECT Students.LastName, Students.FirstName, Students.IdFluidsMechanicsTeacher FROM Students INNER JOIN Teachers ON Students.IdFluidsMechanicsTeacher = Teachers.IdTeacher ORDER BY LastName ASC "; //INNER JOIN Filiere ON etudiant.idFiliere = Filiere.nomFiliere (Filiere.nomFiliere = Etudiant.idFiliere)
            //string sql1 = "SELECT Teachers.FirstName FROM Teachers INNER JOIN Students ON Teachers.IdTeacher = Students.IdFluidsMechanicsTeacher WHERE Students.FirstName = 'Vincent'";
            //Read(sql, conn, cmd, dataReader);

            //Console.ReadKey();

            ////------------ END OF LINK TO SQL ------------



            string[,] timetable = new string[15,9];
            Student student1 = new Student("Jake", "Gawie", "040578", "0678164957", "Male", "jake.gawie@gmail.com", "PassWord1", "ESILV", timetable);
            Student student2 = new Student("Camille", "Toc", "256456", "0556455251", "Female", "camille.gawie@gmail.com", "PassWord2", "ESILV", timetable);
            Student student3 = new Student("Juliette", "Buc", "456789", "0689758545", "Female", "juliette.gawie@gmail.com", "PassWord3", "ESILV", timetable);
            Student student4 = new Student("Gregoire", "Sar", "589456", "0778955225", "Male", "gregoire.gawie@gmail.com", "PassWord4", "ESILV", timetable);
            Student student5 = new Student("Aurelien", "Git", "782598", "0112254558", "Male", "aurelien.gawie@gmail.com", "PassWord5", "ESILV", timetable);
            Student student6 = new Student("Remy", "Jet", "735964", "0112236554", "Male", "remy.gawie@gmail.com", "PassWord6", "ESILV", timetable);
            Student student7 = new Student("Justine", "Sla", "764946", "0256788965", "Femelle", "justine.gawie@gmail.com", "PassWord7", "ESILV", timetable);
            Student student8 = new Student("Jean", "Hut", "893156", "0558788965", "Male", "jean.gawie@gmail.com", "PassWord8", "ESILV", timetable);
            Student student9 = new Student("Alexandre", "Yod", "125489", "044554552", "Male", "alexandre.gawie@gmail.com", "PassWord9", "ESILV", timetable);


            List<string> courses_name = new List<string> { "Fluids Mechanics", "Statistical Inference", "OOP", "Numerical Analysis", "Data Structure" };
            SortedList<Subject, List<Student>> Student_Courses = new SortedList<Subject, List<Student>>();
            List<Student> Student_FluidsMechanics = new List<Student>();
            List<Student> Student_StatisticalInference = new List<Student>();
            List<Student> Student_OOP = new List<Student>();
            List<Student> Student_NumericalAnalysis = new List<Student>();
            List<Student> Student_DataStructure = new List<Student>();

            Subject Fluids = new Subject("Fluids Mechanics", "ESILV", "Monday", "8 - 9");
            Subject Stats = new Subject("Statistical Inference", "ESILV", "Tuesday", "9 - 10");
            Subject Oop = new Subject("OOP", "ESILV", "Tuesday", "14 - 15");
            Subject Numerical = new Subject("Numerical Analysis", "ESILV", "Wednesday", "16 - 17");
            Subject Data = new Subject("Data Structure", "ESILV", "Friday", "14 - 15");

            Student_FluidsMechanics.Add(student1);
            Student_StatisticalInference.Add(student1);
            Student_OOP.Add(student1);
            Student_NumericalAnalysis.Add(student1);
            Student_DataStructure.Add(student1);

            Student_FluidsMechanics.Add(student2);
            Student_FluidsMechanics.Add(student3);
            Student_FluidsMechanics.Add(student4);
            Student_FluidsMechanics.Add(student5);
            Student_FluidsMechanics.Add(student6);
            Student_FluidsMechanics.Add(student7);

            Student_OOP.Add(student2);
            Student_OOP.Add(student3);
            Student_OOP.Add(student4);
            Student_OOP.Add(student6);
            Student_OOP.Add(student8);
            Student_OOP.Add(student9);

            Student_Courses.Add(Fluids, Student_FluidsMechanics);
            Student_Courses.Add(Stats, Student_StatisticalInference);
            Student_Courses.Add(Oop, Student_OOP);
            Student_Courses.Add(Numerical, Student_NumericalAnalysis);
            Student_Courses.Add(Data, Student_DataStructure);

            // Code inutile juste pour tester 
            Admin th = new Admin("John", "Rohley", "754455", "0889455226", "Male", "john.gawie@gmail.com", "PassWordhd", Student_Courses);
            th.SubjectMandatory();
            student1.TimetableWeek();
            Console.WriteLine();
            //th.CreateExamOrCourse();
            //student1.TimetableWeek();
            //Console.WriteLine();

            //student1.Course_Registration(courses_name);
            //Student_Courses_Attribution(student1, Student_Courses);

            SortedList<Subject, string[,]> Grade = new SortedList<Subject, string[,]>();
            Teacher Luc = new Teacher("Luc", "Skywalker", "456456", "0225894589", "male", "lucSlywalker@gmail.com", "password0", Student_Courses, Grade);
            //Luc.studentCoordinates();
            Luc.CreateGrade();
            Luc.ShowGrade();
            Luc.CreateGrade();
            Luc.ShowGrade();

            Console.ReadKey();
            
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Global_College
{
    public class Program 
    {
        public static void Student_Courses_Attribution(Student student, SortedList<string, List<Student>> Student_Courses)
        {
            int index;
            foreach (string element in student.CoursesPicked)
            {
                index = Student_Courses.IndexOfKey(element);
                Student_Courses.Values.ElementAt(index).Add(student);
            }
        }

        public static void Main(string[] args)
        {
            string[,] timetable = { { "ok" } };
            Student student1 = new Student("Jake", "Smith", "1234", "0601020304", "male", "jakesmith@gmail.com", "jsmith", "ESILV", timetable);
            Student student2 = new Student("Jake", "Smith", "1234", "0601020304", "male", "jakesmith@gmail.com", "jsmith", "ESILV", timetable);

            List<string> courses_name = new List<string>();
            courses_name.Add("Fluids Mechanics");
            courses_name.Add("Statistical Inference");
            courses_name.Add("OOP");
            courses_name.Add("Numerical Analysis");
            courses_name.Add("Data Structure");


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


            student1.CourseRegistration(courses_name);
            Student_Courses_Attribution(student1, Student_Courses);
            Console.Write($"\n\n Students in OOP : ");
            foreach (Student student in Student_OOP)
                Console.Write($"{student.Name}");
            Console.Write($"\n Students in Fluids Mechanics : ");
            foreach (Student student in Student_FluidsMechanics)
                Console.Write($"{student.Name}");
            Console.Write($"\n Students in Numerical Analysis : ");
            foreach (Student student in Student_NumericalAnalysis)
                Console.Write($"{student.Name}");


            Console.ReadKey();
            
        }
    }
}

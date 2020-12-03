using System;
using System.ComponentModel.DataAnnotations;

namespace Virtual_Global_College.Models
{
    public class Lesson
    {
        [Key]
        public int IdLesson { get; set; }
        public string Subject { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int IdTeacher { get; set; }
    }
}



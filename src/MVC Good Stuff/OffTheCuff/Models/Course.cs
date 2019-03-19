using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OffTheCuff.Models
{
    public class CourseSummary
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string CourseNumber { get; set; }
    }
    public class Course
    {
        public CourseSummary Info { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<Student> Students { get; set; }
    }
    public class Assignment
    {
        public string Name { get; internal set; }
        public int Weight { get; internal set; }
    }
    public class Student
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
}
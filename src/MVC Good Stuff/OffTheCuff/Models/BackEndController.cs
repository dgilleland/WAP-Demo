using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OffTheCuff.Models
{
    public class BackEndController
    {
        public static BackEndController Instance => new BackEndController();
        public Course GetCourseInfo(int id)
        {
            return new Course
            {
                Info = new CourseSummary
                {
                    CourseID = id,
                    CourseName = "MVC for the Impatient",
                    CourseNumber = "DMIT499"
                },
                Assignments = new List<Assignment>
                {
                    new Assignment{Name = "Quiz 1", Weight = 20},
                    new Assignment{Name = "Quiz 2", Weight = 20},
                    new Assignment{Name = "Quiz 3", Weight = 20},
                    new Assignment{Name = "Quiz 4", Weight = 20},
                    new Assignment{Name = "Quiz 5", Weight = 20}
                },
                Students = new List<Student>
                {
                    new Student{Name = "Bob", ID = 1234569},
                    new Student{Name = "Bob", ID = 1234568},
                    new Student{Name = "Bob", ID = 1234567},
                    new Student{Name = "Bob", ID = 1234566}
                }
            };
        }
    }
}
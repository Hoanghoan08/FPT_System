using AsmAppDev2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsmAppDev2.ViewModels
{
    public class AssignTraineetoCourseViewModel
    {
        public AssignTraineetoCourse AssignTraineetoCourse { get; set; }
        public IEnumerable<ApplicationUser> Trainees { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}
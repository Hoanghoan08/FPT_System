using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AsmAppDev2.Models
{
    public class Topic
    {
		public int ID { get; set; }

		[Required]
		[DisplayName("Topic Name")]
		public string Name { get; set; }

		[Required]
		[DisplayName("Description")]
		public string Description { get; set; }

		[Required]
		public int CourseID { get; set; }
		public Course Course { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AsmAppDev2.Models
{
		public class TrainerUser
		{
		public int ID { get; set; }

		[DisplayName("Trainer ID")]
		[Required]
		public string TrainerID { get; set; }

		[DisplayName("Full Name")]
		public string Full_Name { get; set; }


		[DisplayName("Working Place")]
		public string Working_Place { get; set; }

		public int Phone { get; set; }
		public IEnumerable<ApplicationUser> Trainers { get; set; }

		public ApplicationUser Trainer { get; set; }
	}
}
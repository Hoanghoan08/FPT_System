using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AsmAppDev2.Models
{
  public class TraineeUser
  {
    public int ID { get; set; }
    [DisplayName("Trainee ID")]
    [Required]
    public string TraineeID { get; set; }

    [DisplayName("Full Name")]
    public string Full_Name { get; set; }
    public string Email { get; set; }
    public string Education { get; set; }

    [DisplayName("Programming Language")]
    public string Programming_Language { get; set; }

    [DisplayName("Experience Details")]
    public string Experience_Details { get; set; }
    public string Department { get; set; }
    public int Phone { get; set; }
    public IEnumerable<ApplicationUser> Trainees { get; set; }
    public ApplicationUser Trainee { get; set; }
  }
}
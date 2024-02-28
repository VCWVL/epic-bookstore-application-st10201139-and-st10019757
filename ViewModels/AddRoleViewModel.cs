using System.ComponentModel.DataAnnotations;

namespace EpicBookstoreSprint.ViewModels
{
    public class AddRoleViewModel
    {

        [Required]
        [Display(Name="Role ")]
        public string RoleName { get; set; }
    }
}
